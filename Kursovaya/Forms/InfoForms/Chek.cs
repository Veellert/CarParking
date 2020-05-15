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
    public partial class Chek : Form
    {
        public Chek(DB DB, Car car, string money, string sdacha)
        {
            InitializeComponent();
            label2.Text = car.Client.Passport.FullFIO();
            label4.Text = car.Client.Payment.ToString();
            label6.Text = money;
            label8.Text = sdacha;
            label10.Text = car.Place.LastPayDate.ToString();
            string a = "Владелец";
            if (DB.Parking.CurrentAccount.UserName != "admin")
                a = DB.Parking.CurrentAccount.FIO();
            label11.Text = a;
        }
    }
}
