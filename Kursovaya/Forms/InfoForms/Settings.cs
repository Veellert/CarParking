using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public partial class Settings : Form
    {
        DB DB;
        int p = 0;
        public Settings(DB DB, int p)
        {
            InitializeComponent();
            this.DB = DB;
            this.p = p;
            textBox6.Visible = false;
            label6.Visible = false;
        }
        public Settings(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            FillTextBoxes();
        }

        void FillTextBoxes()
        {
            textBox1.Text = DB.Parking.CountPlaces.ToString();
            textBox2.Text = DB.Parking.CountDisabilityPlaces.ToString();
            textBox3.Text = DB.Parking.CostPerHour.CostForCommon.ToString();
            textBox4.Text = DB.Parking.CostPerDay.CostForCommon.ToString();
            textBox5.Text = DB.Parking.CostPerMonth.CostForCommon.ToString();
            textBox7.Text = DB.Parking.PercentStudent.ToString();
            textBox8.Text = DB.Parking.PercentPensioner.ToString();
            textBox9.Text = DB.Parking.PercentDisability.ToString();
            if (DB.Parking.PercentZP)
                checkBox1.Checked = true;
            if (!checkBox1.Checked)
            {
                textBox6.Visible = false;
                label6.Visible = false;
            }
            if (checkBox1.Checked)
            {
                textBox6.Visible = true;
                label6.Visible = true;
                textBox6.Text = DB.Parking.Percent.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var s = 0;
            if (!DB.Parking.Places.Exists(f => !f.Free))
            {
                if (textBox1.Text != null && textBox2.Text != null)
                {
                    if (DB.Parking.CountPlaces != Convert.ToInt32(textBox1.Text) || DB.Parking.CountDisabilityPlaces != Convert.ToInt32(textBox2.Text))
                        MessageBox.Show("Успех!", "             ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DB.SetPlaces(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                    s = 1;
                }
                else
                {
                    MessageBox.Show("Заполните все поля с количестов!", "Error.InvalidPlaceCountData", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    s = 1;
                }
            }
            else
                MessageBox.Show("Все места должны быть свободны", "             ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (textBox3.Text != null && textBox4.Text != null && textBox5.Text != null && textBox7.Text != null && textBox8.Text != null && textBox9.Text != null)
            {
                DB.SetPrice(Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox7.Text), Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox9.Text));
                if (s == 0 && (DB.Parking.CostPerHour.CostForCommon != Convert.ToInt32(textBox3.Text) || DB.Parking.CostPerDay.CostForCommon != Convert.ToInt32(textBox4.Text) || DB.Parking.CostPerMonth.CostForCommon != Convert.ToInt32(textBox5.Text)))
                    MessageBox.Show("Успех!", "             ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (s == 0)
                    MessageBox.Show("Заполните все поля с ценой!", "Error.InvalidPriceData", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (checkBox1.Checked)
            {
                DB.Parking.PercentZP = true;
                if (textBox6.Text != null)
                    DB.Parking.Percent = Convert.ToInt32(textBox6.Text);
                else
                    checkBox1.Checked = false;
            }
            else
                DB.Parking.PercentZP = false;
            DB.Save();
            FillTextBoxes();
            if (p == 1)
                Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FillTextBoxes();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите безвозвратно удалить все данные?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DB.DeSave();
                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
                textBox4.Text = null;
                textBox5.Text = null;
                textBox7.Text = null;
                textBox8.Text = null;
                textBox9.Text = null;
                if (DB.Parking.PercentZP)
                    checkBox1.Checked = true;
                if (!checkBox1.Checked)
                {
                    textBox6.Visible = false;
                    label6.Visible = false;
                }
                if (checkBox1.Checked)
                {
                    textBox6.Visible = true;
                    label6.Visible = true;
                    textBox6.Text = null;
                }
                Close();
            }
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBox1.Text == null || textBox2.Text == null || textBox3.Text == null || textBox4.Text == null ||
                textBox5.Text == null || (textBox6.Text == null && textBox6.Visible) || textBox7.Text == null ||
                textBox8.Text == null || textBox9.Text == null)
                Application.Exit();
            else
                DB.Save();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && !Char.IsControl(number))
                e.Handled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                textBox6.Visible = false;
                label6.Visible = false;
            }
            if (checkBox1.Checked)
            {
                textBox6.Visible = true;
                label6.Visible = true;
            }
        }
    }
}
