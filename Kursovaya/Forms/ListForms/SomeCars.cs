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
    public partial class SomeCars : Form
    {
        DB DB;
        Place place;
        public SomeCars(DB db, List<Car> cars, Place place)
        {
            InitializeComponent();
            DB = db;
            this.place = place;
            listView1.Items.Clear();
            foreach (var car in cars)
            {
                ListViewItem row = new ListViewItem(car.Number);
                row.SubItems.Add(car.Color);
                row.SubItems.Add(car.Mark);
                row.SubItems.Add(car.Model);
                row.Tag = car;
                listView1.Items.Add(row);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            if(MessageBox.Show("Вы уверены в своем выборе?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DB.newcar = (Car)listView1.SelectedItems[0].Tag;
                Car car = DB.newcar;
                DB.newClient = null;
                DB.newcar = null;
                place.Free = false;
                place.LastOccupationDate = DateTime.Now;
                place.LastPayDate = DateTime.Now;
                car.Client.pay1 = 0;
                car.Client.pay2 = 0;
                car.Client.pay3 = 0;
                car.Client.DayPayment = car.Client.Payment;
                car.Client.MonthPayment = car.Client.Payment;
                place.DPayDate = DateTime.Now;
                place.MPayDate = DateTime.Now;
                car.Place = place;
                Ticket tic = new Ticket
                {
                    Num = DB.SetTicket(),
                    Client = car.Client
                };
                car.Ticket = tic;
                MessageBox.Show($"Номер вашего билета {tic.Num}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DB.EventSave(EventType.CarIn, DateTime.Now, car.Client);
                DB.Save();
                Close();
            }
        }

        private void выбратьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show("Вы уверены в своем выборе?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DB.newcar = (Car)listView1.SelectedItems[0].Tag;
                Car car = DB.newcar;
                DB.newClient = null;
                DB.newcar = null;
                place.Free = false;
                place.LastOccupationDate = DateTime.Now;
                place.LastPayDate = DateTime.Now;
                car.Client.pay1 = 0;
                car.Client.pay2 = 0;
                car.Client.pay3 = 0;
                car.Client.DayPayment = car.Client.Payment;
                car.Client.MonthPayment = car.Client.Payment;
                place.DPayDate = DateTime.Now;
                place.MPayDate = DateTime.Now;
                car.Place = place;
                Ticket tic = new Ticket
                {
                    Num = DB.SetTicket(),
                    Client = car.Client
                };
                car.Ticket = tic;
                MessageBox.Show($"Номер вашего билета {tic.Num}", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DB.EventSave(EventType.CarIn, DateTime.Now, car.Client);
                DB.Save();
                Close();
            }
        }
    }
}
