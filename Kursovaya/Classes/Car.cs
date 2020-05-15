using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Car
    {
        public Client Client { get; set; }
        public Place Place { get; set; }
        public Ticket Ticket { get; set; }
        public string Number { get; set; }//Номер машины
        public string Mark { get; set; }//Марка
        public string Model { get; set; }//Модель
        public string Color { get; set; }//Цвет
    }
}
