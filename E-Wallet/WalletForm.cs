using System;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;


namespace E_Wallet
{
    public partial class WalletForm : Form
    {
        OracleDataAdapter adapter;
        DataSet dt;
        OracleCommandBuilder builder;
        private readonly int _userId;
        string _connStr = "Data Source=orcl; User Id=hr; Password=hr;";

        public WalletForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private int GetWalletId(OracleConnection conn)
        {
            using (OracleCommand cmd = new OracleCommand("SP_GET_WALLET_BY_USERID", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("p_UserID", OracleDbType.Int32).Value = _userId;
                cmd.Parameters.Add("p_IsFound", OracleDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("p_WalletID", OracleDbType.Int32).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("p_Balance", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                int isFound = Convert.ToInt32(cmd.Parameters["p_IsFound"].Value.ToString());

                if (isFound == 1)
                {
                    return Convert.ToInt32(cmd.Parameters["p_WalletID"].Value.ToString());
                }
                else
                {
                    throw new Exception("Wallet not found.");
                }
            }
        }
        private void UpdateButtonsVisibilityBasedOnStatus()
        {

            if (!radioButton2.Checked)
            {
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                return;
            }


            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString().Trim().ToUpper() == "PENDING")
            {

                dgvTransactions_SelectionChanged(null, null);
            }
            else
            {

                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
            }
        }
        private void LoadStatuses()
        {
            comboBox1.Items.Clear();

            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                conn.Open();

                string cmdStr = "SELECT DISTINCT STATUS FROM REQUESTS";

                using (OracleCommand cmd = new OracleCommand(cmdStr, conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["STATUS"].ToString());
                    }
                }
            }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void WalletForm_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            LoadStatuses();
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            

            dgvTransactions.SelectionChanged += dgvTransactions_SelectionChanged;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RequestForm req = new RequestForm(_userId);
            req.FormClosed += (s, args) => this.Show();
            req.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?",
                    "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                new Form1().Show();
                this.Close();
            }
        }

        private void dgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pay_Click(object sender, EventArgs e)
        {
            payment payform = new payment(_userId);
            payform.FormClosed += (s, args) =>
            {
                this.Show();
            };
            payform.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constr = _connStr;

            try
            {
                using (OracleConnection conn = new OracleConnection(constr))
                {
                    conn.Open();

                    dgvTransactions.Rows.Clear();

                    if (radioButton1.Checked)
                    {
                        int walletId = GetWalletId(conn);

                        using (OracleCommand cmd = new OracleCommand("SP_SEARCH_TRANSACTIONS", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("p_WalletID", OracleDbType.Int32).Value = walletId;
                            cmd.Parameters.Add("p_Search", OracleDbType.Varchar2).Value = textBox1.Text;

                            cmd.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                            using (OracleDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string direction = (Convert.ToInt32(reader["SenderWalletID"]) == walletId)
                                        ? "Sent to " + reader["ReceiverName"]
                                        : "Received from " + reader["SenderName"];

                                    dgvTransactions.Rows.Add(
                                        reader["TransactionDate"],
                                        direction,
                                        reader["Amount"],
                                        reader["TransactionType"],
                                        reader["Status"]
                                    );
                                }
                            }
                        }
                    }

                    
                    else if (radioButton2.Checked)
                    {
                        string cmdStr = @"SELECT r.REQUESTID,
                                         r.SENDERUSERID,
                                         r.RECEIVERUSERID,
                                         r.AMOUNT,
                                         r.REQUESTDATE,
                                         r.STATUS,
                                         r.NOTE,
                                         u1.USERNAME AS SenderName,
                                         u2.USERNAME AS ReceiverName
                                  FROM REQUESTS r
                                  JOIN USERS u1 ON r.SENDERUSERID = u1.USERID
                                  JOIN USERS u2 ON r.RECEIVERUSERID = u2.USERID
                                  WHERE (r.SENDERUSERID = :id OR r.RECEIVERUSERID = :id)
                                  AND r.STATUS = :status";
                        
                        OracleDataAdapter adapter = new OracleDataAdapter(cmdStr, constr);

                        adapter.SelectCommand.Parameters.Add("id", OracleDbType.Int32).Value = _userId;
                        adapter.SelectCommand.Parameters.Add("status", OracleDbType.Varchar2).Value = comboBox1.Text;

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                       
                        dgvTransactions.DataSource = dt;
                        dgvTransactions.Columns["SENDERUSERID"].Visible = false;
                        dgvTransactions.Columns["RECEIVERUSERID"].Visible = false;
                    }

                    else
                    {
                        MessageBox.Show("Please select Transactions or Requests");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
    }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to exit the E-Wallet?",
                                          "Confirm Exit",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;


            textBox1.Text = "Search by Name";
            search.Text = "search";
            textBox1.Visible = true;
            search.Visible = true;
            label1.Visible = false;
            comboBox1.Visible = false;
            try
            {
                using (OracleConnection conn = new OracleConnection(_connStr))
                {
                    conn.Open();

                    int walletId = -1;
                    decimal balance = 0;

                    // Reset Grid
                    dgvTransactions.DataSource = null;
                    dgvTransactions.Rows.Clear();
                    dgvTransactions.Columns.Clear();
                    // Define columns manually
                    dgvTransactions.Columns.Add("Date", "Date");
                    dgvTransactions.Columns.Add("Name", "Name");
                    dgvTransactions.Columns.Add("Amount", "Amount");
                    dgvTransactions.Columns.Add("Type", "Type");
                    dgvTransactions.Columns.Add("Status", "Status");

                    // 1. Get Wallet
                    using (OracleCommand cmd = new OracleCommand("SP_GET_WALLET_BY_USERID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_UserID", OracleDbType.Int32).Value = _userId;
                        cmd.Parameters.Add("p_IsFound", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_WalletID", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_Balance", OracleDbType.Decimal).Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        int isFound = Convert.ToInt32(cmd.Parameters["p_IsFound"].Value.ToString());

                        if (isFound == 1)
                        {
                            walletId = Convert.ToInt32(cmd.Parameters["p_WalletID"].Value.ToString());
                            balance = Convert.ToDecimal(cmd.Parameters["p_Balance"].Value.ToString());
                            lblBalance.Text = $"Balance: {balance:N2} EGP";
                        }
                        else
                        {
                            lblBalance.Text = "Balance: N/A";
                            return;
                        }
                    }

                    // 2. Load Transactions
                    using (OracleCommand cmd = new OracleCommand("SP_GET_ALL_TRANSACTIONS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_WalletID", OracleDbType.Int32).Value = walletId;
                        cmd.Parameters.Add("p_Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string direction = (Convert.ToInt32(reader["SenderWalletID"]) == walletId)
                                    ? "Sent to " + reader["ReceiverName"]
                                    : "Received from " + reader["SenderName"];

                                dgvTransactions.Rows.Add(
                                    reader["TransactionDate"],
                                    direction,
                                    reader["Amount"],
                                    reader["TransactionType"],
                                    reader["Status"]
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
           
            textBox1.Visible = false;
            search.Visible = false;
            label1.Visible = true;
            comboBox1.Visible = true;

            //3shan appearance
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;


            dgvTransactions.DataSource =null;
            dgvTransactions.Rows.Clear();

            string constr = "Data Source=orcl; User Id=hr; Password=hr;";

            string cmdStrs = @"SELECT r.REQUESTID,
                                      r.SENDERUSERID,
                                      r.RECEIVERUSERID,
                                      r.AMOUNT, 
                                      r.REQUESTDATE, 
                                      r.STATUS, 
                                      r.NOTE,
                                      (SELECT u.USERNAME FROM USERS u WHERE u.USERID = r.SENDERUSERID) AS SenderName,
                                      (SELECT u.USERNAME FROM USERS u WHERE u.USERID = r.RECEIVERUSERID) AS ReceiverName
                                      FROM REQUESTS r
                                      WHERE r.RECEIVERUSERID = :p_UserID OR r.SENDERUSERID = :p_UserID
                                      ORDER BY r.REQUESTDATE DESC";

            adapter = new OracleDataAdapter(cmdStrs, constr);

            adapter.SelectCommand.Parameters.Add("p_UserID", OracleDbType.Int32).Value = _userId;
            builder = new OracleCommandBuilder(adapter);

            dt = new DataSet();
            adapter.Fill(dt);

            dgvTransactions.DataSource = null;
            dgvTransactions.Columns.Clear();

            dgvTransactions.DataSource = dt.Tables[0];


            dgvTransactions.Columns["RequestID"].Visible = false;
            dgvTransactions.Columns["SENDERUSERID"].Visible = false;
            dgvTransactions.Columns["RECEIVERUSERID"].Visible = false;

            foreach (DataGridViewColumn col in dgvTransactions.Columns)
                col.ReadOnly = true;


            dgvTransactions.Columns["note"].ReadOnly = false;


            UpdateButtonsVisibilityBasedOnStatus();


        }

        private void button2_Click(object sender, EventArgs e)
        {

            int rowsAffected = adapter.Update(dt.Tables[0]);

            MessageBox.Show($"{rowsAffected} record(s) updated successfully!");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!radioButton2.Checked) return; 

            string constr = _connStr;

            string cmdStr = @"SELECT r.REQUESTID,
                            r.SENDERUSERID,
                          r.RECEIVERUSERID,
                             r.AMOUNT,
                             r.REQUESTDATE,
                             r.STATUS,
                             r.NOTE,
                             u1.USERNAME AS SenderName,
                             u2.USERNAME AS ReceiverName
                      FROM REQUESTS r
                      JOIN USERS u1 ON r.SENDERUSERID = u1.USERID
                      JOIN USERS u2 ON r.RECEIVERUSERID = u2.USERID
                      WHERE (r.SENDERUSERID = :id1 OR r.RECEIVERUSERID = :id2)
                      AND r.STATUS = :status";
            
            OracleDataAdapter adapter = new OracleDataAdapter(cmdStr, constr);

            adapter.SelectCommand.Parameters.Add("id1", _userId);
            adapter.SelectCommand.Parameters.Add("id2", _userId);
            adapter.SelectCommand.Parameters.Add("status",comboBox1.SelectedItem.ToString());

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgvTransactions.DataSource = dt;
            dgvTransactions.Columns["SENDERUSERID"].Visible = false;
            dgvTransactions.Columns["RECEIVERUSERID"].Visible = false;
            UpdateButtonsVisibilityBasedOnStatus();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Accept this request?", "Confirm",
        MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var row = dgvTransactions.CurrentRow;
            if (row == null) return;

            int requestId = Convert.ToInt32(row.Cells["REQUESTID"].Value);

            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                conn.Open();

                OracleTransaction trans = conn.BeginTransaction();

                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = trans;

                    // 1. Get data
                    cmd.CommandText = @"
                SELECT r.Amount,
                       r.Status,
                       w1.WalletID AS ReceiverWalletID,
                       w2.WalletID AS SenderWalletID
                FROM Requests r
                JOIN Wallets w1 ON w1.UserID = r.ReceiverUserID
                JOIN Wallets w2 ON w2.UserID = r.SenderUserID
                WHERE r.RequestID = :id FOR UPDATE";

                    cmd.Parameters.Add("id", requestId);

                    OracleDataReader dr = cmd.ExecuteReader();

                    if (!dr.Read())
                        throw new Exception("Request not found");

                    decimal amount = Convert.ToDecimal(dr["Amount"]);
                    string status = dr["Status"].ToString();
                    int receiverWallet = Convert.ToInt32(dr["ReceiverWalletID"]);
                    int senderWallet = Convert.ToInt32(dr["SenderWalletID"]);

                    dr.Close();

                    if (status != "Pending")
                        throw new Exception("Already processed");

                    // 2. Check balance
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT Balance FROM Wallets WHERE WalletID = :id FOR UPDATE";
                    cmd.Parameters.Add("id", receiverWallet);

                    decimal balance = Convert.ToDecimal(cmd.ExecuteScalar());

                    if (balance < amount)
                        throw new Exception("Insufficient balance");

                    // 3. Deduct from receiver
                    cmd.Parameters.Clear();
                    cmd.CommandText = "UPDATE Wallets SET Balance = Balance - :amt WHERE WalletID = :id";
                    cmd.Parameters.Add("amt", amount);
                    cmd.Parameters.Add("id", receiverWallet);
                    cmd.ExecuteNonQuery();

                    // 4. Add to sender
                    cmd.Parameters.Clear();
                    cmd.CommandText = "UPDATE Wallets SET Balance = Balance + :amt WHERE WalletID = :id";
                    cmd.Parameters.Add("amt", amount);
                    cmd.Parameters.Add("id", senderWallet);
                    cmd.ExecuteNonQuery();

                    // 5. Insert transaction
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"
                                      INSERT INTO Transactions
                                      (SenderWalletID, ReceiverWalletID, Amount, TransactionType, Status)
                                      VALUES (:s, :r, :a, 'Request Payment', 'Completed')";

                    cmd.Parameters.Add("s", receiverWallet);
                    cmd.Parameters.Add("r", senderWallet);
                    cmd.Parameters.Add("a", amount);
                    cmd.ExecuteNonQuery();

                    // 6. Update request
                    cmd.Parameters.Clear();
                    cmd.CommandText = "UPDATE Requests SET Status = 'Accepted' WHERE RequestID = :id";
                    cmd.Parameters.Add("id", requestId);
                    cmd.ExecuteNonQuery();

                    
                    trans.Commit();

                    MessageBox.Show("Request Accepted & Money Transferred ");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }

            radioButton2_CheckedChanged(null, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reject this request?", "Confirm",
    MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var row = dgvTransactions.CurrentRow;
            if (row == null) return;

            int requestId = Convert.ToInt32(row.Cells["REQUESTID"].Value);

            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                conn.Open();

                OracleCommand cmd = new OracleCommand(
                    "UPDATE REQUESTS SET STATUS = 'Rejected' WHERE REQUESTID = :id", conn);

                cmd.Parameters.Add("id", OracleDbType.Int32).Value = requestId;
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Request Rejected");

            radioButton2_CheckedChanged(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this request?",
        "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var row = dgvTransactions.CurrentRow;
            if (row == null) return;

            int requestId = Convert.ToInt32(row.Cells["REQUESTID"].Value);

            using (OracleConnection conn = new OracleConnection(_connStr))
            {
                conn.Open();

                OracleCommand cmd = new OracleCommand(
                    "DELETE FROM REQUESTS WHERE REQUESTID = :id", conn);

                cmd.Parameters.Add("id", OracleDbType.Int32).Value = requestId;
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Request Deleted");

            radioButton2_CheckedChanged(null, null);
        }

        private void dgvTransactions_SelectionChanged(object sender, EventArgs e)
        {

            if (!radioButton2.Checked) return;

            if (dgvTransactions.CurrentRow == null) return;

            if (!dgvTransactions.Columns.Contains("SENDERUSERID") ||
                !dgvTransactions.Columns.Contains("RECEIVERUSERID") ||
                !dgvTransactions.Columns.Contains("STATUS"))
                return;

            var row = dgvTransactions.CurrentRow;

            int senderId = Convert.ToInt32(row.Cells["SENDERUSERID"].Value);
            int receiverId = Convert.ToInt32(row.Cells["RECEIVERUSERID"].Value);
            string status = row.Cells["STATUS"].Value.ToString();

            // normalize
            status = status.Trim().ToUpper();

            // reset
            button3.Visible = false; // Accept
            button4.Visible = false; // Reject
            button5.Visible = false; // Delete

            
            if (_userId == receiverId && status == "PENDING")
            {
                button3.Visible = true; // Accept
                button4.Visible = true; // Reject
            }

           
            if (_userId == senderId && status == "PENDING")
            {
                button5.Visible = true; // Delete
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            ReportsMenuForm frm = new ReportsMenuForm(_userId);
            frm.Show();
        }
    }
}
