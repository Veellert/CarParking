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
    public partial class Add : Form
    {
        DB DB;
        int s = 0;
        public Add(DB DB)
        {
            InitializeComponent();
            label1.Text = "Марка";
            this.DB = DB;
        }

        public Add(DB DB, int s)
        {
            InitializeComponent();
            label1.Text = "Модель";
            this.DB = DB;
            this.s = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (s != 0)
            {
                if (textBox1.Text != null)
                {
                    DB.Parking.Models.Add(textBox1.Text);
                    MessageBox.Show("Добавлена новая модель", "           ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("И что мне нужно добавить по твоему? Пустоту?", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBox1.Text != null)
                {
                    DB.Parking.Marks.Add(textBox1.Text);
                    MessageBox.Show("Добавлена новая марка","           ",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("И что мне нужно добавить по твоему? Пустоту?", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox1.Text = "";
            DB.Save();
        }
    }
}
