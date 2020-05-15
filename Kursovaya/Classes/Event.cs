using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Event
    {
        public DateTime Date { get; set; }
        public Client Client { get; set; }
        public string EventMesege { get; set; }
    }
}
