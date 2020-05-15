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
    public partial class EventHistory : Form
    {
        DB DB;
        public EventHistory(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            ShowEvent();
        }

        void ShowEvent()
        {
            listView1.Items.Clear();
            foreach (var e in DB.Parking.Events)
            {
                ListViewItem row = new ListViewItem(e.Date.ToString());
                row.SubItems.Add(e.Client.Passport.FIO());
                row.SubItems.Add(e.EventMesege);
                row.Tag = e;
                listView1.Items.Add(row);
            } 
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            var events = DB.Parking.Events.Where(s => s.Date.Year == dateTimePicker1.Value.Year && s.Date.Month == dateTimePicker1.Value.Month && s.Date.Day == dateTimePicker1.Value.Day);
            foreach (var f in events)
            {
                ListViewItem row = new ListViewItem(f.Date.ToString());
                row.SubItems.Add(f.Client.Passport.FIO());
                row.SubItems.Add(f.EventMesege);
                row.Tag = e;
                listView1.Items.Add(row);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            ShowEvent();
        }
    }
}
