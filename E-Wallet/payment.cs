using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E_Wallet
{
    public partial class payment : Form
    {
        private int _userId;
        String _connStr = "Data Source=orcl; User Id=hr; Password=hr;";
        public payment(int userID)
        {
            InitializeComponent();
            _userId = userID;
        }

        private void payment_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||  // Phone
                string.IsNullOrWhiteSpace(textBox2.Text) ||  // Amount
                string.IsNullOrWhiteSpace(textBox3.Text))    // PIN
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            try
            {
                OracleConnection conn = new OracleConnection(_connStr);
                conn.Open();

                // 1. Get Sender WalletID + PIN
                OracleCommand cmdInfo = new OracleCommand();
                cmdInfo.Connection = conn;
                cmdInfo.CommandText = @"SELECT w.WalletID, u.PIN 
                                FROM Wallets w 
                                JOIN Users u ON w.UserID = u.UserID 
                                WHERE u.UserID = :id";

                cmdInfo.Parameters.Add("id", OracleDbType.Int32).Value = _userId;

                OracleDataReader dr = cmdInfo.ExecuteReader();

                int senderWalletId = -1;
                string dbPin = "";

                if (dr.Read())
                {
                    senderWalletId = Convert.ToInt32(dr["WalletID"]);
                    dbPin = dr["PIN"].ToString();
                }
                dr.Close();

                // 2. Check PIN
                if (textBox3.Text.Trim() != dbPin)
                {
                    MessageBox.Show("Invalid PIN");
                    return;
                }

                // 3. Get Receiver WalletID from Phone Number
                OracleCommand cmdRec = new OracleCommand();
                cmdRec.Connection = conn;
                cmdRec.CommandText = @"SELECT w.WalletID 
                               FROM Wallets w 
                               JOIN Users u ON w.UserID = u.UserID 
                               WHERE u.PhoneNumber = :phone";

                cmdRec.Parameters.Add("phone", OracleDbType.Varchar2).Value = textBox1.Text.Trim();

                object result = cmdRec.ExecuteScalar();

                if (result == null)
                {
                    MessageBox.Show("Phone number not registered");
                    return;
                }

                int receiverWalletId = Convert.ToInt32(result.ToString());

                // 4. Transfer
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SP_TRANSFER_FUNDS";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_SenderWalletID", OracleDbType.Int32).Value = senderWalletId;
                cmd.Parameters.Add("p_ReceiverWalletID", OracleDbType.Int32).Value = receiverWalletId;
                cmd.Parameters.Add("p_Amount", OracleDbType.Decimal).Value = Convert.ToDecimal(textBox2.Text);

                cmd.Parameters.Add("p_Success", OracleDbType.Int32).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int status = Convert.ToInt32(cmd.Parameters["p_Success"].Value.ToString());

                if (status == 1)
                {
                    MessageBox.Show("Transfer Successful");
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();

                    textBox1.Focus();

                }
                else
                {
                    MessageBox.Show("Transfer Failed (Check Balance)");
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
