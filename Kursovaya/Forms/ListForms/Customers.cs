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
    public partial class Customers : Form
    {
        enum Search
        {
            Fname,
            comm,
            stud,
            pens
        }

        DB DB;

        public Customers(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            columnHeader0.TextAlign = HorizontalAlignment.Center;
            timer1.Tick += timer1_Tick;
            timer1.Interval = 1000;
            timer1.Start();
        }

        void ShowClients(List<Client> humen)
        {
            listView1.Items.Clear();
            foreach (var Client in humen)
            {
                string category = "";
                if (Client.Category == Category.Common)
                    category = "Человек простой";
                if (Client.Category == Category.Student)
                    category = "Студент";
                if (Client.Category == Category.Pensioner)
                    category = "Пенсионер";
                ListViewItem row = new ListViewItem();
                row.SubItems.Add(Client.Passport.FName);
                row.SubItems.Add(Client.Passport.IName);
                row.SubItems.Add(Client.Passport.OName);
                row.SubItems.Add(category);
                row.SubItems.Add(Client.Payment.ToString());
                row.Tag = Client;
                listView1.Items.Add(row);
            }
        }

        void ClearT()
        {
            timer1.Tick -= button1_Click;
            timer1.Tick -= checkBox1_CheckedChanged;
            timer1.Tick -= timer1_Tick;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AllPhones form = new AllPhones(DB);
            form.Show();
            DB.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
                return;
            else
            {
                ClearT();
                if (checkBox1.Checked == true)
                {
                    List<Client> humen = new List<Client>();
                    humen.AddRange(DB.Parking.Clients.FindAll(s => s.Disability == true));
                    ShowClients(humen);
                    timer1.Tick += checkBox1_CheckedChanged;
                    return;
                }
                timer1.Tick += timer1_Tick;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
                return;
            else
            {
                if (comboBox1.SelectedItem != null)
                {
                    ClearT();
                    List<Client> humen = new List<Client>();
                    var text = textBox1.Text;
                    var search = (Search)comboBox1.SelectedIndex;
                    if (checkBox1.Checked == false)
                        switch (search)
                        {
                            case Search.Fname:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Passport.FName.ToLower().Contains(text)));
                                break;
                            case Search.comm:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Category == Category.Common));
                                break;
                            case Search.stud:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Category == Category.Student));
                                break;
                            case Search.pens:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Category == Category.Pensioner));
                                break;
                        }
                    else
                        switch (search)
                        {
                            case Search.Fname:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Passport.FName.ToLower().Contains(text) && s.Disability.Equals(true)));
                                break;
                            case Search.comm:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Category == Category.Common && s.Disability.Equals(true)));
                                break;
                            case Search.stud:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Category == Category.Student && s.Disability.Equals(true)));
                                break;
                            case Search.pens:
                                humen.AddRange(DB.Parking.Clients.Where(s => s.Category == Category.Pensioner && s.Disability.Equals(true)));
                                break;
                        }
                    ShowClients(humen);
                    timer1.Tick += button1_Click;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                textBox1.Enabled = true;
            else
                textBox1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedItem = null;
            checkBox1.Checked = false;
            ClearT();
            timer1.Tick += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
                return;
            else
                ShowClients(DB.Parking.Clients);
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            Client Client = (Client)listView1.SelectedItems[0].Tag;
            new AddClient(DB, Client, 1).Show();
            DB.Save();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            Client Client = (Client)listView1.SelectedItems[0].Tag;
            DB.Parking.Clients.Remove(Client);
            ShowClients(DB.Parking.Clients);
            DB.Save();
        }
    }
}
