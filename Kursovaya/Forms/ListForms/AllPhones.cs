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
    public partial class AllPhones : Form
    {
        DB DB;
        public AllPhones(DB DB)
        {
            InitializeComponent();
            this.DB = DB;
            listView1.Items.Clear();
            foreach (var Client in DB.Parking.Clients)
            {
                foreach (var phone in Client.Phones)
                {
                    ListViewItem row = new ListViewItem(phone);
                    row.SubItems.Add(Client.Passport.FIO());
                    row.Tag = Client;
                    listView1.Items.Add(row);
                }
            }
        }
    }
}
