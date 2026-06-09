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
    public partial class ReportsMenuForm : Form
    {
        int _userId;
        public ReportsMenuForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(_userId, "");
            frm.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DateForm frm = new DateForm(_userId);
            frm.Show();
        }
    }
}
