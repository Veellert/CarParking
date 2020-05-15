using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Ticket
    {
        public int Num { get; set; }
        public Client Client { get; set; }
    }
}
