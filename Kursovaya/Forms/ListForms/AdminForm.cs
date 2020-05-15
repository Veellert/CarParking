using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public partial class AdminForm : Form
    {
        DB DB;
        public AdminForm(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            ShowUsers(DB.Parking.Accounts.Where(s => s.UserName != "admin"));
        }

        void ShowUsers(IEnumerable<Account> accs)
        {
            listView1.Items.Clear();
            foreach (var acc in accs)
            {
                ListViewItem row = new ListViewItem();
                row.SubItems.Add(acc.UserName);
                row.SubItems.Add(acc.Password);
                row.SubItems.Add(acc.FIO());
                row.SubItems.Add(acc.Number);
                row.SubItems.Add(acc.Mail);
                row.SubItems.Add(acc.Acct.ToString());
                row.SubItems.Add(acc.LastOnline.ToString());
                row.Tag = acc;
                listView1.Items.Add(row);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            textBox2.Text = null;
            ShowUsers(DB.Parking.Accounts.Where(s => s.UserName != "admin"));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == null)
                return;
            var accs = DB.Parking.Accounts.Where(s => s.Number.Contains(textBox2.Text) && s.UserName != "admin");
            ShowUsers(accs);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
                return;
            var accs = DB.Parking.Accounts.Where(s => s.Fname.Contains(textBox1.Text) && s.UserName != "admin");
            ShowUsers(accs);
        }
    }
}
