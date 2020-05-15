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
    public partial class PrivateOffice : Form
    {
        DB DB;
        Account acc;
        public PrivateOffice(DB DB, Account acc)
        {
            InitializeComponent();
            this.DB = DB;
            this.acc = acc;
            textBox1.Text = acc.UserName;
            textBox2.Text = acc.Password;
            textBox3.Text = acc.Fname;
            textBox4.Text = acc.Iname;
            textBox5.Text = acc.Oname;
            textBox6.Text = acc.Number;
            textBox7.Text = acc.Mail;
            double a = 0;
            foreach (var i in acc.Acct)
                a += i.Money;
            if (acc.UserName == "admin")
                a = DB.Parking.Cash;
            if (!DB.Parking.PercentZP)
            {
                textBox8.Visible = false;
                label8.Visible = false;
            }
            else
                textBox8.Text = a.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Otchet().Show();
            DB.Parking.CurrentAccount.LastOnline = DateTime.Now;
            DB.Parking.CurrentAccount = null;
            DB.Save();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FindForm().Controls.Clear();
            InitializeComponent2();
            textBox1.Text = acc.UserName;
            textBox2.Text = acc.Password;
            textBox3.Text = acc.Fname;
            textBox4.Text = acc.Iname;
            textBox5.Text = acc.Oname;
            textBox6.Text = acc.Number;
            textBox7.Text = acc.Mail;
            double a = 0;
            foreach (var i in acc.Acct)
                a += i.Money;
            if (acc.UserName == "admin")
                a = DB.Parking.Cash;
            if (!DB.Parking.PercentZP)
            {
                textBox8.Visible = false;
                label8.Visible = false;
            }
            else
                textBox8.Text = a.ToString();
        }
        private void button2_Click2(object sender, EventArgs e)
        {
            if (textBox2.Text != null && textBox3.Text != null && textBox4.Text != null && textBox5.Text != null && textBox6.Text != null && textBox7.Text != null)
            {
                acc.Password = textBox2.Text;
                acc.Fname = textBox3.Text;
                acc.Iname = textBox4.Text;
                acc.Oname = textBox5.Text;
                acc.Number = textBox6.Text;
                acc.Mail = textBox7.Text;
                DB.Save();
            }
            else
            {
                MessageBox.Show("Не оставляйте поля пустыми");
                return;
            }
            FindForm().Controls.Clear();
            InitializeComponent();
            textBox1.Text = acc.UserName;
            textBox2.Text = acc.Password;
            textBox3.Text = acc.Fname;
            textBox4.Text = acc.Iname;
            textBox5.Text = acc.Oname;
            textBox6.Text = acc.Number;
            textBox7.Text = acc.Mail;
            double a = 0;
            foreach (var i in acc.Acct)
                a += i.Money;
            if (acc.UserName == "admin")
                a = DB.Parking.Cash;
            if (!DB.Parking.PercentZP)
            {
                textBox8.Visible = false;
                label8.Visible = false;
            }
            else
                textBox8.Text = a.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Otchet().Show();
        }
    }
}
