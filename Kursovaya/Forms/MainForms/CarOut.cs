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
    public partial class CarOut : Form
    {
        Car car;
        DB DB;
        public CarOut(DB db,Car car)
        {
            InitializeComponent();
            this.car = car;
            DB = db;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
                textBox1.Enabled = false;
            if(textBox2.Text == "" && textBox3.Text == "")
                textBox1.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
                textBox1.Enabled = false;
            if (textBox2.Text == "" && textBox3.Text == "")
                textBox1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "")
            {
                MessageBox.Show("Вы ничего не ввели","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if(!textBox1.Enabled)
            {
                if(car.Client.Passport.Seria == textBox2.Text && car.Client.Passport.Nomer == textBox3.Text)
                {
                    car.Place.Free = true;
                    car.Place.LastFreeDate = DateTime.Now;
                    if (MessageBox.Show($"Общая сумма равна {car.Client.Payment}. Oплатить Сейчас?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        new Cash(DB, car).ShowDialog();
                    DB.EventSave(EventType.CarOut, DateTime.Now, car.Client);
                    DB.Save();
                }
                else
                {
                    MessageBox.Show("Данные не совпали", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (car.Ticket.Num == Convert.ToInt32(textBox1.Text))
                {
                    if (MessageBox.Show($"Фамилия {car.Client.Passport.FName}, Имя {car.Client.Passport.IName}, Отчество {car.Client.Passport.OName} \nВерно?", "Проверьте данные", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        car.Place.Free = true;
                        car.Place.LastFreeDate = DateTime.Now;
                        if (MessageBox.Show($"Общая сумма равна {car.Client.Payment}. Oплатить Сейчас?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            new Cash(DB, car).ShowDialog();
                        DB.EventSave(EventType.CarOut, DateTime.Now, car.Client);
                        DB.Save();
                        Close();
                    }
                    else return;
                }
                else
                {
                    MessageBox.Show("Данные не совпали", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        }
    }
}
