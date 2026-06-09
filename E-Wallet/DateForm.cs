using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace E_Wallet
{
    public partial class DateForm : Form

    {
        private int _userId;
        public DateForm(int userId)
        {
            InitializeComponent();
            _userId = userId; 
        }

        private void DateForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(fromDate.Value.ToString());
                MessageBox.Show(toDate.Value.ToString());
                DateReport rpt = new DateReport(); 

                rpt.SetDatabaseLogon("hr", "hr", "orcl", "");

                rpt.SetParameterValue("P_USER_ID", _userId);
                rpt.SetParameterValue("P_FROM_DATE", fromDate.Value);
                rpt.SetParameterValue("P_TO_DATE", toDate.Value);

                crystalReportViewer1.ReportSource = rpt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
