using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace E_Wallet
{
    public partial class RequestForm : Form
    {
        private int _userId;
        string _connStr = "Data Source=orcl; User Id=hr; Password=hr;";
        public RequestForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtAmount_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Enter phone and amount");
                return;
            }

            try
            {
                using (OracleConnection conn = new OracleConnection(_connStr))
                {
                    conn.Open();

                    OracleCommand cmd = new OracleCommand("SP_CREATE_REQUEST", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_SenderUserID", OracleDbType.Int32).Value = _userId;
                    cmd.Parameters.Add("p_ReceiverPhone", OracleDbType.Varchar2).Value = txtPhone.Text.Trim();
                    cmd.Parameters.Add("p_Amount", OracleDbType.Decimal).Value = Convert.ToDecimal(txtAmount.Text);
                    cmd.Parameters.Add("p_Note", OracleDbType.Varchar2).Value = txtNote.Text;

                    cmd.Parameters.Add("p_Success", OracleDbType.Int32).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int status = Convert.ToInt32(cmd.Parameters["p_success"].Value.ToString());

                    if (status == 1) { 
                        MessageBox.Show("Request Sent");
                        this.Close(); 
                    }
                    else if (status == 0)
                        MessageBox.Show("User not found");
                    else
                        MessageBox.Show("Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
