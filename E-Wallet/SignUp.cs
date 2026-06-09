using System;
using System.Data;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace E_Wallet
{
    public partial class SignUp : Form
    {
        private readonly string _connStr = "Data Source=orcl; User Id=hr; Password=hr;";

        public SignUp()
        {
            InitializeComponent();
            LoadBankNames();
        }

        private void LoadBankNames()
        {
            bankname.Items.Clear();
            bankname.Items.AddRange(new object[]
            {
                "National Bank of Egypt",
                "Banque Misr",
                "CIB",
                "QNB",
                "HSBC Egypt",
                "Arab African International Bank",
                "Other"
            });
            bankname.SelectedIndex = 0;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text))
            { ShowError("Username is required."); return; }
            if (!SecurityHelper.IsValidPassword(password.Text))
            { ShowError("Password must be at least 8 characters."); return; }
            if (password.Text != confirmpassword.Text)
            { ShowError("Passwords do not match."); return; }
            if (!SecurityHelper.IsValidPIN(pin.Text))
            { ShowError("PIN must be exactly 4 digits."); return; }
            if (string.IsNullOrWhiteSpace(phoneno.Text))
            { ShowError("Phone number is required."); return; }
            if (string.IsNullOrWhiteSpace(cardno.Text))
            { ShowError("Card number is required."); return; }

            string hashedPassword = SecurityHelper.HashPassword(password.Text);

            try
            {
                using (OracleConnection conn = new OracleConnection(_connStr))
                {
                    conn.Open();
                    using (OracleTransaction tx = conn.BeginTransaction()) 
                    {
                        try
                        {
                            decimal newUserID;

                            using (OracleCommand cmd1 = new OracleCommand())
                            {
                                cmd1.Connection = conn;
                                cmd1.Transaction = tx;
                                cmd1.CommandType = CommandType.Text;
                                cmd1.CommandText = @"INSERT INTO Users (UserName, PasswordHash, PIN, PhoneNumber, IsLocked)
                                                     VALUES (:p_UserName, :p_PasswordHash, :p_PIN, :p_PhoneNumber, 0)
                                                     RETURNING UserID INTO :p_UserID";

                                cmd1.Parameters.Add("p_UserName", OracleDbType.Varchar2, 100).Value = username.Text.Trim();
                                cmd1.Parameters.Add("p_PasswordHash", OracleDbType.Varchar2, 200).Value = hashedPassword;
                                cmd1.Parameters.Add("p_PIN", OracleDbType.Varchar2, 200).Value = pin.Text.Trim();
                                cmd1.Parameters.Add("p_PhoneNumber", OracleDbType.Varchar2, 20).Value = phoneno.Text.Trim();

                                OracleParameter userIDParam = new OracleParameter("p_UserID", OracleDbType.Decimal);
                                userIDParam.Direction = ParameterDirection.Output;
                                cmd1.Parameters.Add(userIDParam);

                                cmd1.ExecuteNonQuery();

                                object raw = cmd1.Parameters["p_UserID"].Value;
                                if (raw == null || raw == DBNull.Value)
                                    throw new Exception("User insert did not return a UserID.");

                                newUserID = ((Oracle.DataAccess.Types.OracleDecimal)raw).Value;
                            }

                            using (OracleCommand cmd2 = new OracleCommand())
                            {
                                cmd2.Connection = conn;
                                cmd2.Transaction = tx;
                                cmd2.CommandType = CommandType.Text;
                                cmd2.CommandText = @"INSERT INTO Wallets (UserID, Balance, BankName, CardNumber)
                                                     VALUES (:p_UserID, 0, :p_BankName, :p_CardNumber)";

                                cmd2.Parameters.Add("p_UserID", OracleDbType.Decimal, ParameterDirection.Input).Value = newUserID;
                                cmd2.Parameters.Add("p_BankName", OracleDbType.Varchar2, 100).Value = bankname.SelectedItem?.ToString();
                                cmd2.Parameters.Add("p_CardNumber", OracleDbType.Varchar2, 20).Value = cardno.Text.Trim();

                                cmd2.ExecuteNonQuery();
                            }

                            tx.Commit(); 

                            MessageBox.Show("Registration successful! You can now log in.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback(); 
                            ShowError("Registration failed: " + ex.Message);
                        }
                    }
                }
            }
            catch (OracleException ex) { ShowError("Database error: " + ex.Message); }
            catch (Exception ex) { ShowError("Unexpected error: " + ex.Message); }
        }
        private static void ShowError(string message)
        {
            MessageBox.Show(message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
