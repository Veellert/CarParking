using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Account
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Number { get; set; }
        public string Fname { get; set; }
        public string Iname { get; set; }
        public string Oname { get; set; }
        public List<Pay> Acct { get; set; }
        public DateTime LastOnline { get; set; }

        public string FIO()
        {
            string fio = Fname + " " + Iname.First() + ". " + Oname.First() + ".";
            return fio;
        }
    }
}
