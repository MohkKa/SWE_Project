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
    public partial class Form2 : Form
    {
        int _userId;
        string _bank;

        public Form2(int userId, string bank)
        {
            InitializeComponent();
            _userId = userId;
            _bank = bank;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("CIB Bank");
            comboBox1.Items.Add("National Bank");
            comboBox1.Items.Add("Banque Misr");

            comboBox1.SelectedIndex = 0;
        }
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bank = comboBox1.SelectedItem.ToString();

            BankReport rpt = new BankReport();
            rpt.SetDatabaseLogon("hr", "hr", "orcl", "");

            rpt.SetParameterValue("P_USER_ID", _userId);
            rpt.SetParameterValue("P_BANK", comboBox1.Text);

            crystalReportViewer1.ReportSource = rpt;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
