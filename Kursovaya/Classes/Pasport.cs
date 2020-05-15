using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Passport
    {
        public Client Client { get; set; }
        public string FName { get; set; }
        public string IName { get; set; }
        public string OName { get; set; }
        public string BirthDate { get; set; }
        public string Nomer { get; set; }
        public string Seria { get; set; }

        public string FIO() => FName + " " + IName.First() + ". " + OName.First() + ".";
        public string FullFIO() => FName + " " + IName + " " + OName;
    }
}