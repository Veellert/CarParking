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
    public partial class LogIn : Form
    {
        DB DB;
        public LogIn(DB DB, int a)
        {
            InitializeComponent2();
            this.DB = DB;
            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;
        }

        public LogIn(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            textBox2.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.UseSystemPasswordChar = false;
            if (!checkBox1.Checked)
                textBox2.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged2(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
                textBox3.UseSystemPasswordChar = false;
            }
            if (!checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true;
                textBox3.UseSystemPasswordChar = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            FindForm().Controls.Clear();
            InitializeComponent2();
            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            FindForm().Controls.Clear();
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account acc = DB.Parking.Accounts.Find(s => s.UserName == textBox1.Text);
            if (acc != null)
                if (acc.Password == textBox2.Text)
                {
                    acc.LastOnline = DateTime.Now;
                    DB.Parking.CurrentAccount = acc;
                    DB.Save();
                    Close();
                }
                else
                    MessageBox.Show("Неверный пароль", "Error.InvalidPassword", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Неверное имя пользователя", "Error.InvalidUserName", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string mail = "";
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "" && textBox8.Text == "")
            {
                MessageBox.Show("Заполните данные", "Error.InvalidData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите имя пользователя", "Error.InvalidUserName", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите пароль", "Error.InvalidPassword", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Подтвердите пароль", "Error.InvalidPassword", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Пароли не совпадают", "Error.InvalidPassword", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(DB.Parking.Accounts.Contains(DB.Parking.Accounts.Find(s => s.UserName == textBox1.Text)))
            {
                MessageBox.Show("Аккаунт с таким именем уже существует", "Error.InvalidUserName", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите фамилию", "Error.InvalidFIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox5.Text == "")
            {
                MessageBox.Show("Введите имя", "Error.InvalidFIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox6.Text == "")
            {
                MessageBox.Show("Введите отчество", "Error.InvalidFIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox7.Text == "")
            {
                MessageBox.Show("Введите номер телефона", "Error.InvalidFIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox8.Text == "")
                mail = "Нет";
            else
                mail = textBox8.Text;
            Pay pay = new Pay { Money = 0, Date = DateTime.Now };
            Account acc = new Account
            {
                UserName = textBox1.Text,
                Password = textBox2.Text,
                Fname = textBox4.Text,
                Iname = textBox5.Text,
                Oname = textBox6.Text,
                Number = textBox7.Text,
                Mail = mail,
                Acct = new List<Pay> { pay },
                LastOnline = DateTime.Now
            };
            DB.Parking.Accounts.Add(acc);
            DB.Parking.CurrentAccount = acc;
            DB.Save();
            Close();
        }

        private void LogIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DB.Parking.CurrentAccount == null)
                Application.Exit();
        }
    }
}