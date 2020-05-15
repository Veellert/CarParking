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
    public partial class Cash : Form
    {
        DB DB;
        Car car;
        public Cash(DB DB, Car car)
        {
            InitializeComponent();
            this.DB = DB;
            this.car = car;
            label2.Text = car.Client.Payment.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                label4.Text = textBox1.Text;
                label6.Text = (Convert.ToInt32(label4.Text) - Convert.ToInt32(label2.Text)).ToString();
            }
            if (Convert.ToInt32(label6.Text) < 0)
                label6.Text = "0";
            textBox1.Text= null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(label4.Text) < Convert.ToInt32(label2.Text))
                MessageBox.Show("Не достаточно денег", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (Convert.ToInt32(label4.Text) >= Convert.ToInt32(label2.Text))
            {
                car.Place.LastPayDate = DateTime.Now;
                if (MessageBox.Show("Оплата прошла успешно, показать чек?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    new Chek(DB, car, label4.Text, label6.Text).Show();
                if (DB.Parking.PercentZP)
                {
                    double money = Convert.ToDouble(car.Client.Payment) - (Convert.ToDouble(car.Client.Payment) * (100 - DB.Parking.Percent) / 100);
                    DB.Parking.Cash += Convert.ToDouble(car.Client.Payment) - (Convert.ToDouble(car.Client.Payment) * DB.Parking.Percent / 100);
                    Pay pay = new Pay { Money = money, Date = DateTime.Now };
                    DB.Parking.CurrentAccount.Acct.Add(pay);
                }
                else
                {
                    DB.Parking.Cash += Convert.ToDouble(car.Client.Payment);
                    Pay pay = new Pay { Money = 0, Date = DateTime.Now };
                    DB.Parking.CurrentAccount.Acct.Add(pay);
                }
                car.Client.Payment = 0;
            }
            DB.EventSave(EventType.Pay, DateTime.Now, car.Client);
            DB.Save();
            Close();
        }
    }
}
