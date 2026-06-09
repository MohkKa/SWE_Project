using System;
using System.Data;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace E_Wallet
{
    public partial class LoginForm : Form
    {
        private readonly string _connStr = "Data Source=orcl; User Id=hr; Password=hr;";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username.Text) ||
                string.IsNullOrWhiteSpace(password.Text))
            {
                MessageBox.Show("Please enter both username and password.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = SecurityHelper.HashPassword(password.Text);

            try
            {
                using (OracleConnection conn = new OracleConnection(_connStr))
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection  = conn;
                        cmd.CommandText = "SP_VALIDATE_LOGIN";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_UserName",     OracleDbType.Varchar2).Value = username.Text.Trim();
                        cmd.Parameters.Add("p_PasswordHash", OracleDbType.Varchar2).Value = hashedPassword;
                        cmd.Parameters.Add("p_IsValid",      OracleDbType.Int32, ParameterDirection.Output);
                        cmd.Parameters.Add("p_UserID",       OracleDbType.Int32, ParameterDirection.Output);

                        cmd.ExecuteNonQuery();

                        int isValid = Convert.ToInt32(cmd.Parameters["p_IsValid"].Value.ToString());
                        int userId  = Convert.ToInt32(cmd.Parameters["p_UserID"].Value.ToString());

                        switch (isValid)
                        {
                            case 1:
                                MessageBox.Show("Welcome! Login successful.",
                                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                WalletForm dashboard = new WalletForm(userId);
                                dashboard.FormClosed += (s, ea) => new Form1().Show();
                                dashboard.Show();
                                this.Hide();
                                break;

                            case -1:
                                MessageBox.Show("This account is currently locked.",
                                    "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;

                            default:
                                MessageBox.Show("Invalid username or password.",
                                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                password.Clear();
                                break;
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Database error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
