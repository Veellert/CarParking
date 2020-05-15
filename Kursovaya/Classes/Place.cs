using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Place
    {
        public bool Free { get; set; }//Свободно или занято
        public bool Disability { get; set; }//Для инвалидов 
        public string Number { get; set; }//Номер метса
        public DateTime LastOccupationDate { get; set; }
        public DateTime LastFreeDate { get; set; }
        public DateTime LastPayDate { get; set; }
        public DateTime DPayDate { get; set; }
        public DateTime MPayDate { get; set; }
    }
}
