using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public partial class Form1 : Form
    {
        DB DB = new DB();
        public Form1()
        {
            InitializeComponent();
            DB.Parking.LastOpenDate = DateTime.Now;
            if (DB.Parking.Accounts.Count == 0)
            {
                Pay pay = new Pay { Money = 0, Date = DateTime.Now };
                Account Admin = new Account
                {
                    UserName = "admin",
                    Password = "123",
                    Fname = " ",
                    Iname = " ",
                    Oname = " ",
                    Number = " ",
                    Mail = " ",
                    LastOnline = DateTime.Now,
                    Acct = new List<Pay> { pay }
                };
                DB.Parking.Accounts.Add(Admin);
            }
            if (DB.Parking.CountPlaces == 0)
            {
                if (MessageBox.Show("Для продолжения заполните данные. Вы сможете изменить их в любое время в дальнейшем во вкладке 'Настройки'", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    new Settings(DB, 1).ShowDialog();
                    DB.Save();
                }
                else
                {
                    FindForm().Close();
                    return;
                }
            }
            label11.Text = DB.Parking.CostPerHour.CostForCommon.ToString();
            label18.Text = DB.Parking.CostPerHour.CostForStudent.ToString();
            label22.Text = DB.Parking.CostPerHour.CostForPensioner.ToString();
            label20.Text = DB.Parking.CostPerHour.CostForDisability.ToString();
            label16.Text = DB.Parking.CostPerDay.CostForCommon.ToString();
            label17.Text = DB.Parking.CostPerDay.CostForStudent.ToString();
            label21.Text = DB.Parking.CostPerDay.CostForPensioner.ToString();
            label19.Text = DB.Parking.CostPerDay.CostForDisability.ToString();
            label29.Text = DB.Parking.CostPerMonth.CostForCommon.ToString();
            label28.Text = DB.Parking.CostPerMonth.CostForStudent.ToString();
            label27.Text = DB.Parking.CostPerMonth.CostForPensioner.ToString();
            label26.Text = DB.Parking.CostPerMonth.CostForDisability.ToString();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            if (DB.Parking.Accounts.Count().Equals(1))
                DB.Parking.CurrentAccount = DB.Parking.Accounts.Find(s => s.UserName == "admin");
            else
            {
                DB.Parking.CurrentAccount = null;
                new LogIn(DB).ShowDialog();
            }
            DB.CheckPay();
            timer1.Interval = 1000;
            timer1.Start();
            DB.Save();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                DB.UpdatePay();
                DB.Save();
                return;
            }
            if (listView2.SelectedItems.Count != 0)
            {
                DB.UpdatePay();
                DB.Save();
                return;
            }
            if(checkBox1.Checked)
            {
                DB.UpdatePay();
                DB.Save();
                return;
            }
            if (textBox1.Text != "")
            {
                DB.UpdatePay();
                DB.Save();
                return;
            }
            if (textBox2.Text != "")
            {
                DB.UpdatePay();
                DB.Save();
                return;
            }
            if (checkBox2.Checked)
            {
                ShowFreePlaces();
                ShowOccupiedPlaces();
            }
            DB.UpdatePay();
            DB.Save();
        }

        private void ShowFreePlaces()
        {
            label6.Text = DB.Parking.Places.FindAll(s => s.Free && !s.Disability).Count().ToString();
            label25.Text = DB.Parking.Places.FindAll(s => s.Free && s.Disability).Count().ToString();
            listView2.Items.Clear();
            foreach (Place place in DB.Parking.Places.FindAll(s => s.Free))
            {
                ListViewItem row = new ListViewItem(place.Number);
                string s = "";
                if (!place.Disability)
                    s = "Обычное";
                if (place.Disability)
                    s = "Инвалидное";
                row.SubItems.Add(s);
                row.Tag = place;
                listView2.Items.Add(row);
            }
        }

        private void ShowOccupiedPlaces()
        {
            listView1.Items.Clear();
            foreach (Client Client in DB.Parking.Clients)
                foreach (Car car in Client.Cars)
                {
                    if (!car.Place.Free)
                    {
                        string str1 = "Человек простой";
                        string str2 = "Да";
                        if (!car.Client.Disability)
                            str2 = "Нет";
                        if (car.Client.Category == Category.Student)
                            str1 = "Студент";
                        if (car.Client.Category == Category.Pensioner)
                            str1 = "Пенсионер";
                        ListViewItem row = new ListViewItem(car.Place.Number);
                        row.SubItems.Add(car.Client.Passport.FIO());
                        row.SubItems.Add(car.Number);
                        row.SubItems.Add(str1);
                        row.SubItems.Add(str2);
                        row.SubItems.Add(car.Place.LastOccupationDate.ToString());
                        row.Tag = car;
                        listView1.Items.Add(row);
                    }
                }
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Customers(DB).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void машиныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Automobiles(DB).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void должникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Debetors(DB).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void мастаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ParkingPlaces(DB).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void событияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EventHistory(DB).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DB.Parking.CurrentAccount.UserName == "admin")
            {
                new Settings(DB).Show();
                ShowFreePlaces();
                ShowOccupiedPlaces();
                DB.Save();
            }
            else
                MessageBox.Show("У вас нет прав администратора","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                listView2.Items.Clear();
                foreach (Place place in DB.Parking.Places.FindAll(s => s.Free && s.Disability))
                {
                    string str = "Инвалидное";
                    listView2.Items.Add(new ListViewItem(place.Number)
                    {
                        SubItems = 
                        {
                            str
                        },
                        Tag = place
                    });
                }
            }
            else
                ShowFreePlaces();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != null)
            {
                if (DB.Parking.Places.Find(s => s.Number == textBox3.Text).Free)
                {
                    if (MessageBox.Show($"Место {textBox3.Text} на данный момент свободно, хотите занять его?", "Найдено свободное место", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                        return;
                    Place place = (Place)listView2.Items[Convert.ToInt32(textBox3.Text) - 1].Tag;
                    if (DB.newClient.Cars != null)
                    {
                        Car car = DB.newClient.Cars.Find(s => s.Place.Free);
                        DB.newClient = null;
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
                    }
                    else
                        MessageBox.Show("Нет клиента", "Error.NotEnoughClientData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show($"Место {textBox3.Text} на данный момент занято", "Место занято другим клиентом", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("Для начала введите номер места", "Error.InvalidPlaceNumber", MessageBoxButtons.OK, MessageBoxIcon.Error);
            textBox3.Text = "";
            ShowFreePlaces();
            ShowOccupiedPlaces();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AddClient(DB, new Client()).ShowDialog();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new IdentifyClient(DB).ShowDialog();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void занятьМестоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
                return;
            if (DB.newClient != null)
            {
                if (DB.newClient.Cars.FindAll(s => s.Place.Free).Count != 0)
                {
                    Place place = (Place)listView2.SelectedItems[0].Tag;
                    if (place.Disability != DB.newClient.Disability)
                    {
                        MessageBox.Show("Вы не можете встать на это место", "Error.InvalidPlaceType", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (DB.newClient.Cars.FindAll(s => s.Place.Free).Count > 1)
                    {
                        List<Car> cars = DB.newClient.Cars.FindAll(s => s.Place.Free);
                        new SomeCars(DB, cars, place).Show();
                    }
                    else
                    {
                        DB.newcar = DB.newClient.Cars.Find(s => s.Place.Free);
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
                    }
                }
                else
                    MessageBox.Show("У клиента нет машин", "Error.NotEnoughClientData", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
                MessageBox.Show("Нет клиента", "Error.NotEnoughClientData", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            ShowFreePlaces();
            ShowOccupiedPlaces();
        }

        private void освободитьМестоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            Car tag = (Car)listView1.SelectedItems[0].Tag;
            new CarOut(DB, tag).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(DB.Parking.CurrentAccount != null)
            {
                if (MessageBox.Show("Для начала нужно выйти из текущего аккаунта. Выйти?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (MessageBox.Show("Показать отчет?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        new Otchet().Show();
                    DB.Parking.CurrentAccount.LastOnline = DateTime.Now;
                    DB.Parking.CurrentAccount = null;
                }
                else
                    return;
            }
            new LogIn(DB).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void панельАдминистратораToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(DB.Parking.CurrentAccount != DB.Parking.Accounts.Find(s => s.UserName == "admin"))
            {
                ShowFreePlaces();
                ShowOccupiedPlaces();
                MessageBox.Show("У вас нет прав администратора", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new AdminForm(DB).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void панельАдминистратораToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(DB.Parking.CurrentAccount == null)
            {
                ShowFreePlaces();
                ShowOccupiedPlaces();
                MessageBox.Show("Вы не вошли в аккаунт","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            new PrivateOffice(DB, DB.Parking.CurrentAccount).Show();
            ShowFreePlaces();
            ShowOccupiedPlaces();
            DB.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                listView2.Items.Clear();
                foreach (Place place in DB.Parking.Places.FindAll(s => s.Free && s.Disability))
                {
                    string str = "Инвалидное";
                    listView2.Items.Add(new ListViewItem(place.Number)
                    {
                        SubItems =
                        {
                            str
                        },
                        Tag = place
                    });
                }
            }
            else
                ShowFreePlaces();
            ShowOccupiedPlaces();
        }

        private void добавитьМаркуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Add(DB).ShowDialog();
            DB.Save();
        }

        private void добавитьМодельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Add(DB,1).ShowDialog();
            DB.Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (Client Client in DB.Parking.Clients.Where(s => s.Passport.FName.Contains(textBox1.Text)))
                foreach (Car car in Client.Cars)
                {
                    if (!car.Place.Free)
                    {
                        string str1 = "Человек простой";
                        string str2 = "Да";
                        if (!car.Client.Disability)
                            str2 = "Нет";
                        if (car.Client.Category == Category.Student)
                            str1 = "Студент";
                        if (car.Client.Category == Category.Pensioner)
                            str1 = "Пенсионер";
                        ListViewItem row = new ListViewItem(car.Place.Number);
                        row.SubItems.Add(car.Client.Passport.FIO());
                        row.SubItems.Add(car.Number);
                        row.SubItems.Add(str1);
                        row.SubItems.Add(str2);
                        row.SubItems.Add(car.Place.LastOccupationDate.ToString());
                        row.Tag = car;
                        listView1.Items.Add(row);
                    }
                }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (Client Client in DB.Parking.Clients.Where(s => s.Passport.FName.Contains(textBox1.Text)))
                foreach (Car car in Client.Cars.Where(s => s.Number.Contains(textBox2.Text)))
                {
                    if (!car.Place.Free)
                    {
                        string str1 = "Человек простой";
                        string str2 = "Да";
                        if (!car.Client.Disability)
                            str2 = "Нет";
                        if (car.Client.Category == Category.Student)
                            str1 = "Студент";
                        if (car.Client.Category == Category.Pensioner)
                            str1 = "Пенсионер";
                        ListViewItem row = new ListViewItem(car.Place.Number);
                        row.SubItems.Add(car.Client.Passport.FIO());
                        row.SubItems.Add(car.Number);
                        row.SubItems.Add(str1);
                        row.SubItems.Add(str2);
                        row.SubItems.Add(car.Place.LastOccupationDate.ToString());
                        row.Tag = car;
                        listView1.Items.Add(row);
                    }
                }
        }
    }
}
