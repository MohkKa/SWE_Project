// ============================================================
//  E-Wallet Financial System
//  File: ManageWalletForm.Designer.cs
//  Purpose: Designer layout for disconnected-mode manage form
// ============================================================
namespace E_Wallet
{
    partial class ManageWalletForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblInfo      = new System.Windows.Forms.Label();
            this.txtFilter    = new System.Windows.Forms.TextBox();
            this.btnSearch    = new System.Windows.Forms.Button();
            this.dgvRequests  = new System.Windows.Forms.DataGridView();
            this.btnSave      = new System.Windows.Forms.Button();
            this.lblResult    = new System.Windows.Forms.Label();
            this.lblTitle     = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 102, 153);
            this.lblTitle.Location  = new System.Drawing.Point(20, 12);
            this.lblTitle.Text      = "Manage Payment Requests";

            // lblInfo
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(20, 58);
            this.lblInfo.Text     = "Filter by status:";

            // txtFilter
            this.txtFilter.Location = new System.Drawing.Point(20, 80);
            this.txtFilter.Size     = new System.Drawing.Size(180, 22);
            this.txtFilter.Text     = "Pending";

            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(215, 78);
            this.btnSearch.Size     = new System.Drawing.Size(90, 26);
            this.btnSearch.Text     = "Search";
            this.btnSearch.Click   += new System.EventHandler(this.btnSearch_Click);

            // lblResult
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(320, 82);
            this.lblResult.Text     = "";

            // dgvRequests
            this.dgvRequests.Location              = new System.Drawing.Point(20, 120);
            this.dgvRequests.Size                  = new System.Drawing.Size(950, 320);
            this.dgvRequests.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRequests.AllowUserToAddRows    = false;
            this.dgvRequests.AllowUserToDeleteRows = false;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(20, 460);
            this.btnSave.Size     = new System.Drawing.Size(150, 32);
            this.btnSave.Text     = "💾 Save Changes";
            this.btnSave.Click   += new System.EventHandler(this.btnSave_Click);

            // ManageWalletForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1000, 520);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.dgvRequests);
            this.Controls.Add(this.btnSave);
            this.Name  = "ManageWalletForm";
            this.Text  = "E-Wallet — Manage Requests";
            this.Load += new System.EventHandler(this.ManageWalletForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label           lblTitle;
        private System.Windows.Forms.Label           lblInfo;
        private System.Windows.Forms.TextBox         txtFilter;
        private System.Windows.Forms.Button          btnSearch;
        private System.Windows.Forms.Label           lblResult;
        private System.Windows.Forms.DataGridView    dgvRequests;
        private System.Windows.Forms.Button          btnSave;
    }
}
