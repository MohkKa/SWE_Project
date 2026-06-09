// ============================================================
//  E-Wallet Financial System
//  File: ManageWalletForm.cs
//  Purpose: Wallet management — Disconnected Mode demo
//
//  ODP.Net DISCONNECTED MODE (Requirement B):
//    B1. Select certain rows for a given value entered by the user
//        → user types a receiver username, adapter fills DataTable
//    B2. Update using OracleCommandBuilder
//        → user edits the Note column in the grid, clicks Save
// ============================================================
using System;
using System.Data;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace E_Wallet
{
    public partial class ManageWalletForm : Form
    {
        private readonly int    _userId;
        private readonly string _connStr = "Data Source=orcl; User Id=hr; Password=hr;";

        // Keep adapter + table alive for the CommandBuilder update
        private OracleDataAdapter _adapter;
        private DataTable         _requestsTable;

        public ManageWalletForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void ManageWalletForm_Load(object sender, EventArgs e)
        {
            lblInfo.Text = "Enter a status to filter your payment requests (e.g. Pending, Accepted, Rejected):";
        }

        // ── B1: Select rows for user-entered filter value ─────────────────────
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string statusFilter = txtFilter.Text.Trim();
            if (string.IsNullOrWhiteSpace(statusFilter)) statusFilter = "Pending";

            try
            {
                // Disconnected: fill DataTable via OracleDataAdapter then CLOSE connection
                string sql =
                    "SELECT r.RequestID, r.SenderUserID, r.ReceiverUserID, " +
                    "       r.Amount, r.Status, r.Note, " +
                    "       us.UserName AS SenderName, ur.UserName AS ReceiverName " +
                    "FROM   Requests r " +
                    "JOIN   Users us ON us.UserID = r.SenderUserID " +
                    "JOIN   Users ur ON ur.UserID = r.ReceiverUserID " +
                    "WHERE  (r.SenderUserID = :uid OR r.ReceiverUserID = :uid2) " +
                    "  AND  r.Status = :status";

                using (OracleConnection conn = new OracleConnection(_connStr))
                {
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.Parameters.Add("uid",    OracleDbType.Int32,    _userId,      ParameterDirection.Input);
                    cmd.Parameters.Add("uid2",   OracleDbType.Int32,    _userId,      ParameterDirection.Input);
                    cmd.Parameters.Add("status", OracleDbType.Varchar2, statusFilter, ParameterDirection.Input);

                    _adapter       = new OracleDataAdapter(cmd);
                    _requestsTable = new DataTable();
                    _adapter.Fill(_requestsTable); // connection opens/closes automatically
                }

                // Bind to grid — user can edit the Note column
                dgvRequests.DataSource = _requestsTable;

                // Make non-editable columns read-only for safety
                foreach (DataGridViewColumn col in dgvRequests.Columns)
                {
                    col.ReadOnly = (col.Name != "Note" && col.Name != "Status");
                }

                lblResult.Text = $"{_requestsTable.Rows.Count} request(s) found.";
            }
            catch (OracleException ex)
            {
                MessageBox.Show("DB error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── B2: Update via OracleCommandBuilder (disconnected) ────────────────
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_adapter == null || _requestsTable == null)
            {
                MessageBox.Show("Please search first.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // CommandBuilder auto-generates UPDATE/INSERT/DELETE commands
                using (OracleConnection conn = new OracleConnection(_connStr))
                {
                    _adapter.SelectCommand.Connection = conn;
                    OracleCommandBuilder builder = new OracleCommandBuilder(_adapter);
                    _adapter.Update(_requestsTable); // pushes changed rows back to DB
                }

                MessageBox.Show("Changes saved successfully.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _requestsTable.AcceptChanges();
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Save error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
