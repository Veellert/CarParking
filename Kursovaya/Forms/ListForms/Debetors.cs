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
    public partial class Debetors : Form
    {
        DB DB;
        List<Client> humen;
        public Debetors(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            humen = new List<Client>();
            humen.AddRange(DB.Parking.Clients.Where(s => s.Payment != 0));
            ShowDebetors(humen);
            timer1.Tick += timer1_Tick;
            timer1.Interval = 1000;
            timer1.Start();
        }

        enum Search
        {
            PhoneNumb,
            CarNumb,
            Fio,
            Pay
        }

        void ShowDebetors(List<Client> humen)
        {
            listView1.Items.Clear();
            foreach (var Client in humen)
                foreach (var car in Client.Cars)
                {
                    foreach (var phone in car.Client.Phones)
                    {
                        ListViewItem row = new ListViewItem(car.Client.Passport.FIO());
                        row.SubItems.Add(car.Number);
                        row.SubItems.Add(car.Mark);
                        row.SubItems.Add(car.Model);
                        row.SubItems.Add(phone);
                        row.SubItems.Add(car.Client.Payment.ToString());
                        row.Tag = car;
                        listView1.Items.Add(row);
                    }
                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
                return;
            ShowDebetors(humen);
        }

        void ClearT()
        {
            timer1.Tick -= button1_Click;
            timer1.Tick -= timer1_Tick;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                textBox1.Enabled = true;
            if (comboBox1.SelectedIndex == 1)
                textBox1.Enabled = true;
            if (comboBox1.SelectedIndex == 2)
                textBox1.Enabled = true;
            if (comboBox1.SelectedIndex == 3)
                textBox1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
                return;
            List<Client> carso = new List<Client>();
            List<Car> caro = new List<Car>();
            var text = textBox1.Text.ToLower();
            var search = (Search)comboBox1.SelectedIndex;
            switch (search)
            {
                case Search.Fio:
                    carso.AddRange(humen.Where(s => s.Passport.FName.ToLower().Contains(text)));
                    break;
                case Search.CarNumb:
                    foreach (var Client in humen)
                        caro.AddRange(Client.Cars.Where(s => s.Number.ToLower().Contains(text)));
                    foreach (var Client in carso)
                        Client.Cars.AddRange(caro);
                    break;
                case Search.Pay:
                    carso.AddRange(humen.Where(s => s.Payment.ToString().Contains(text)));
                    break;
                case Search.PhoneNumb:
                    carso.AddRange(humen.Where(s => s.Phones.Contains(text)));
                    break;
            }
            ClearT();
            timer1.Tick += button1_Click;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Enabled = false;
            comboBox1.SelectedItem = null;
            ClearT();
            timer1.Tick += timer1_Tick;
        }

        private void оплатитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            Car car = (Car)listView1.SelectedItems[0].Tag;
            new Cash(DB, car).ShowDialog();
            if(car.Client.Payment == 0)
                humen.Remove(car.Client);
            ShowDebetors(humen);
            DB.Save();
        }
    }
}
