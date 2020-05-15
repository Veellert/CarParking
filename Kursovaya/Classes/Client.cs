using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Client
    {
        public int Payment { get; set; }//Задолжность
        public int DayPayment { get; set; }
        public int MonthPayment { get; set; }
        public int pay1 { get; set; }
        public int pay2 { get; set; }
        public int pay3 { get; set; }
        public int PayPerHour { get; set; }
        public int PayPerDay { get; set; }
        public int PayPerMonth { get; set; }
        public bool Disability { get; set; }//Инвалидность
        public Category Category { get; set; }
        public Passport Passport { get; set; }//Вся информация
        public List<string> Phones { get; set; }//Номера телефонов
        public List<Car> Cars { get; set; }//Список машин
    }
}
