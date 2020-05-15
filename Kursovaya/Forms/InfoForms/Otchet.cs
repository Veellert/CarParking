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
    public partial class Otchet : Form
    {
        public Otchet()
        {
            InitializeComponent();
            string fio = "Владелец";
            if(DB.Parking.CurrentAccount.UserName != "admin")
                fio = DB.Parking.CurrentAccount.FIO();
            label1.Text = fio;
            label3.Text = DB.Parking.Events.Where(s => s.EventMesege == "Машина заехала" && s.Date > DB.Parking.CurrentAccount.LastOnline).Count().ToString();
            label5.Text = DB.Parking.Events.Where(s => s.EventMesege == "Машина выехала" && s.Date > DB.Parking.CurrentAccount.LastOnline).Count().ToString();
            label7.Text = DB.Parking.Places.Where(s => !s.Free && s.LastFreeDate < DB.Parking.CurrentAccount.LastOnline).Count().ToString();
            label9.Text = DB.Parking.Places.Where(s => !s.Free && s.LastFreeDate > DB.Parking.CurrentAccount.LastOnline).Count().ToString();
            int indexStart = (DateTime.Now - DB.Parking.CurrentAccount.LastOnline).ToString().IndexOf(".");
            int length = (DateTime.Now - DB.Parking.CurrentAccount.LastOnline).ToString().Length;
            int count = length - indexStart;
            label11.Text = (DateTime.Now - DB.Parking.CurrentAccount.LastOnline).ToString().Remove(indexStart, count);
            double a = 0;
            foreach (var i in DB.Parking.CurrentAccount.Acct.Where(s => s.Date > DB.Parking.CurrentAccount.LastOnline))
                a += i.Money;
            if (DB.Parking.CurrentAccount.UserName == "admin")
                a = DB.Parking.Cash;
            if (!DB.Parking.PercentZP)
            {
                label13.Visible = false;
                label8.Visible = false;
            }
            else
                label13.Text = a.ToString();
        }
    }
}
