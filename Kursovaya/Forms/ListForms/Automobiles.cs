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
    public partial class Automobiles : Form
    {
        DB DB;
        List<Car> cars;
        public Automobiles(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            cars = new List<Car>();
            foreach (var Client in DB.Parking.Clients)
                cars.AddRange(Client.Cars);
            ShowCars(cars);
        }

        enum Search
        {
            Numb,
            Fio
        }

        void ShowCars(List<Car> cars)
        {
            listView1.Items.Clear();
                foreach (var car in cars)
                {
                    ListViewItem row = new ListViewItem(car.Client.Passport.FIO());
                    row.SubItems.Add(car.Number);
                    row.SubItems.Add(car.Mark);
                    row.SubItems.Add(car.Model);
                    row.SubItems.Add(car.Color);
                    row.Tag = car;
                    listView1.Items.Add(row);
                }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                textBox1.Enabled = true;
            if (comboBox1.SelectedIndex == 1)
                textBox1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.Enabled = false;
            comboBox1.SelectedItem = null;
            ShowCars(cars);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Car> carso = new List<Car>();
            var text = textBox1.Text.ToLower();
            var search = (Search)comboBox1.SelectedIndex;
            switch (search)
            {
                case Search.Fio:
                    foreach (var Client in DB.Parking.Clients)
                            carso.AddRange(Client.Cars.Where(s => s.Client.Passport.FName.ToLower().Contains(text)));
                    break;
                case Search.Numb:
                    foreach (var Client in DB.Parking.Clients)
                        carso.AddRange(Client.Cars.Where(s => s.Number.ToLower().Contains(text)));
                    break;
            }
            ShowCars(carso);
        }
    }
}