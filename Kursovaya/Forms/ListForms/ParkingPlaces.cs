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
    public partial class ParkingPlaces : Form
    {
        DB DB;
        public ParkingPlaces(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            ShowPlace(DB.Parking.Places);
        }

        void ShowPlace(List<Place> places)
        {
            listView1.Items.Clear();
            foreach (var place in places)
            {
                string s = "Занято";
                string i = "Нет";
                if (place.Free == true)
                    s = "Свободно";
                if (place.Disability == true)
                    i = "Да";
                ListViewItem row = new ListViewItem(place.Number);
                row.SubItems.Add(s);
                row.SubItems.Add(i);
                row.Tag = place;
                listView1.Items.Add(row);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            ShowPlace(DB.Parking.Places);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                listView1.Items.Clear();
                foreach (var place in DB.Parking.Places.FindAll(s => s.Disability == true))
                {
                    string s = "Занято";
                    string i = "Нет";
                    if (place.Free == true)
                        s = "Свободно";
                    if (place.Disability == true)
                        i = "Да";
                    ListViewItem row = new ListViewItem(place.Number);
                    row.SubItems.Add(s);
                    row.SubItems.Add(i);
                    row.Tag = place;
                    listView1.Items.Add(row);
                }
            }
            else
                ShowPlace(DB.Parking.Places);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                listView1.Items.Clear();
                foreach (var place in DB.Parking.Places.FindAll(s => s.Free == true))
                {
                    string s = "Занято";
                    string i = "Нет";
                    if (place.Free == true)
                        s = "Свободно";
                    if (place.Disability == true)
                        i = "Да";
                    ListViewItem row = new ListViewItem(place.Number);
                    row.SubItems.Add(s);
                    row.SubItems.Add(i);
                    row.Tag = place;
                    listView1.Items.Add(row);
                }
            }
            else
                ShowPlace(DB.Parking.Places);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != null)
            {
                List<Place> placee = new List<Place>();
                var somePlace = DB.Parking.Places.Find(s => s.Number.Contains(textBox3.Text));
                if (somePlace == null)
                    return;
                placee.Add(somePlace);
                ShowPlace(placee);
            }
        }
    }
}
