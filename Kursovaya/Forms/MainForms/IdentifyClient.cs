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
    public partial class IdentifyClient : Form
    {
        DB DB;
        public IdentifyClient(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
        }

        enum Search
        { 
            Fio,
            Car
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;

                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox8.Enabled = false;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;

                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AllCLear();
        }
        
        void AllCLear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";

            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;

            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;

            comboBox1.SelectedItem = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Client> humen = DB.Parking.Clients;
            List<Car> cars = new List<Car>();
            var text1 = textBox1.Text.ToLower();
            var text2 = textBox2.Text.ToLower();
            var text3 = textBox3.Text.ToLower();
            var text5 = textBox5.Text.ToLower();
            var text6 = textBox6.Text.ToLower();
            var text7 = textBox7.Text.ToLower();
            var text8 = textBox8.Text.ToLower();
            var search = (Search)comboBox1.SelectedIndex;
            switch (search)
            {
                case Search.Fio:
                    var newlist = humen.Find(s => s.Passport.FName.ToLower() == text1 && s.Passport.IName.ToLower() == text2 && s.Passport.OName.ToLower() == text3);
                    if (newlist != null)
                    {
                        MessageBox.Show("Клиент идентифицирован", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DB.newClient = newlist;
                        DB.newcar = null;
                        Close();
                    }
                    else
                    {
                        if (MessageBox.Show("Клиент не найден, добавить?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        {
                            Client humon = new Client();
                            Close();
                            AddClient form = new AddClient(DB, humon);
                            form.ShowDialog();
                            DB.Save();
                        }
                    }
                    break;
                case Search.Car:
                    foreach (var item in DB.Parking.Clients)
                    {
                        cars = item.Cars;
                        var newcars = cars.Find(s => s.Number.ToLower() == text5 && s.Mark.ToLower() == text6 && s.Model.ToLower() == text7 && s.Color.ToLower() == text8);
                        if (newcars != null)
                        {
                            MessageBox.Show("Клиент идентифицирован", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DB.newClient = null;
                            DB.newcar = newcars;
                            Close();
                        }
                        else
                        {
                            if (MessageBox.Show("Клиент не найден, добавить?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                            {
                                Client humon = new Client();
                                Close();
                                AddClient form = new AddClient(DB, humon);
                                form.ShowDialog();
                                DB.Save();
                            }
                        }
                    }
                    break;
            }
            AllCLear();
        }
    }
}
