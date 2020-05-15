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
    public partial class AddClient : Form
    {
        DB DB;
        Client Client;
        List<string> phones;
        List<Car> cars;
        Place place;
        public AddClient(DB DB, Client Client)
        {
            InitializeComponent();
            this.DB = DB;
            this.Client = Client;
            phones = new List<string>();
            cars = new List<Car>();
            comboBox1.SelectedIndex = 0;
            place = new Place
            {
                Free = true
            };
            comboBox2.DataSource = DB.Parking.Marks;
            comboBox3.DataSource = DB.Parking.Models;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
            textBox11.Text = "";
            textBox10.Text = "";
            ShowPhones();
            ShowCars();
        }

        public AddClient(DB DB, Client Client,int a)
        {
            InitializeComponent2();
            this.DB = DB;
            this.Client = Client;
            if (Client.Phones == null)
                phones = new List<string>();
            else
                phones = Client.Phones;
            if (Client.Cars == null)
                cars = new List<Car>();
            else
                cars = Client.Cars;
            if(Client.Category == Category.Common)
                comboBox1.SelectedIndex = 0;
            if (Client.Category == Category.Student)
                comboBox1.SelectedIndex = 1;
            if (Client.Category == Category.Pensioner)
                comboBox1.SelectedIndex = 2;
            if (Client.Disability)
                checkBox1.Checked = true;
            place = new Place { Free = true };
            textBox1.Text = Client.Passport.FName;
            textBox2.Text = Client.Passport.IName;
            textBox3.Text = Client.Passport.OName;
            textBox4.Text = Client.Passport.BirthDate.ToString();
            textBox5.Text = Client.Passport.Seria;
            textBox7.Text = Client.Passport.Nomer;
            comboBox2.DataSource = DB.Parking.Marks;
            comboBox3.DataSource = DB.Parking.Models;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
            textBox11.Text = "";
            textBox10.Text = "";
            ShowPhones();
            ShowCars();
        }

        void ShowPhones()
        {
            listView1.Items.Clear();
            foreach (var number in phones)
            {
                ListViewItem row = new ListViewItem(number);
                row.Tag = number;
                listView1.Items.Add(row);
            }
        }

        void ShowCars()
        {
            listView2.Items.Clear();
            foreach (var car in cars)
            {
                ListViewItem row = new ListViewItem(car.Number);
                row.SubItems.Add(car.Mark);
                row.SubItems.Add(car.Model);
                row.SubItems.Add(car.Color);
                row.Tag = car;
                listView2.Items.Add(row);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
            {
                MessageBox.Show("Заполните все поля паспорта", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Passport Passport = new Passport
            {
                Client = Client,
                FName = textBox1.Text,
                IName = textBox2.Text,
                OName = textBox3.Text,
                BirthDate = textBox4.Text,
                Seria = textBox5.Text + textBox6.Text,
                Nomer = textBox7.Text
            };
            if(!DB.Parking.Clients.Contains(DB.Parking.Clients.Find(s => s.Passport.FName == Passport.FName && s.Passport.IName == Passport.IName && s.Passport.OName == Passport.OName && s.Passport.BirthDate == Passport.BirthDate && s.Passport.Seria == Passport.Seria && s.Passport.Nomer == Passport.Nomer)))
            {
                Category categoria = (Category)comboBox1.SelectedIndex;
                DB.newClient = DB.AddClient(Client, Passport, phones, cars, categoria);
                DB.EventSave(EventType.RegClient, DateTime.Now, Client);
                DB.Save();
                MessageBox.Show("Успешно!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                if (MessageBox.Show("Такой человек уже существует, продолжить?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    DB.newClient = DB.Parking.Clients.Find(s => s.Passport == Passport);
                    Close();
                }
                else
                    return;
            }
        }

        private void button3_Click2(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "")
            {
                MessageBox.Show("Заполните все поля паспорта", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Passport Passport = new Passport
            {
                Client = Client,
                FName = textBox1.Text,
                IName = textBox2.Text,
                OName = textBox3.Text,
                BirthDate = textBox4.Text,
                Seria = textBox5.Text + textBox6.Text,
                Nomer = textBox7.Text
            };
            Category categoria = (Category)comboBox1.SelectedIndex;
            DB.RedactClient(Client, Passport, phones, cars, categoria);
            DB.Save();
            MessageBox.Show("Успешно!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox8.Text == "")
            {
                MessageBox.Show("Пустое поле", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            phones.Add(textBox8.Text);
            MessageBox.Show("Добавлен номер телефона", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            textBox8.Text = "";
            ShowPhones();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox9.Text == "" || textBox10.Text == "" || textBox11.Text == "" || textBox12.Text == "")
            {
                MessageBox.Show("Пустые поля", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Car car = new Car
            {
                Client = Client,
                Number = textBox12.Text,
                Mark = textBox11.Text,
                Model = textBox10.Text,
                Color = textBox9.Text,
                Place = place,
                Ticket = new Ticket()
            };
            if (!DB.Parking.Marks.Contains(textBox11.Text))
                DB.Parking.Marks.Add(textBox11.Text);
            if (!DB.Parking.Models.Contains(textBox10.Text))
                DB.Parking.Models.Add(textBox10.Text);
            cars.Add(car);
            MessageBox.Show("Добавлена машина","",MessageBoxButtons.OK,MessageBoxIcon.Information);
            textBox12.Text = "";
            textBox11.Text = "";
            textBox10.Text = "";
            textBox9.Text = "";
            comboBox2.DataSource = DB.Parking.Marks;
            comboBox3.DataSource = DB.Parking.Models;
            ShowCars();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                Client.Disability = true;
            else
                Client.Disability = false;
        }


        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "")
                comboBox2.DataSource = DB.Parking.Marks.FindAll(s => s.ToLower().Contains(textBox11.Text.ToLower()));
        }

        private void textBox11_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox11.Text = comboBox2.SelectedItem.ToString();
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text != "")
                comboBox3.DataSource = DB.Parking.Models.FindAll(s => s.ToLower().Contains(textBox10.Text.ToLower()));
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox10.Text = comboBox3.SelectedItem.ToString();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            phones.Remove((string)listView1.SelectedItems[0].Tag);
            ShowPhones();
            DB.Save();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;
            cars.Remove((Car)listView2.SelectedItems[0].Tag);
            ShowCars();
            DB.Save();
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            var a = (string)listView1.SelectedItems[0].Tag;
            textBox8.Text = a;
            phones.Remove(a);
            ShowPhones();
            DB.Save();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;
            var a = (Car)listView2.SelectedItems[0].Tag;
            textBox12.Text = a.Number;
            textBox11.Text = a.Mark;
            textBox10.Text = a.Model;
            textBox9.Text = a.Color;
            cars.Remove(a);
            ShowCars();
            DB.Save();
        }
    }
}
