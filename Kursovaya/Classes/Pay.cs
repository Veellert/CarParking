using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Pay
    {
        public double Money { get; set; }
        public DateTime Date { get; set; }
    }
}
