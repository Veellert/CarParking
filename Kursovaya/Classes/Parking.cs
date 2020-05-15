using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaya
{
    [Serializable]
    public class Parking
    {
        public bool PercentZP = false;
        public int Percent = 0;
        public int CountPlaces = 0;
        public int CountDisabilityPlaces = 0;
        public int PercentStudent = 10;
        public int PercentPensioner = 25;
        public int PercentDisability = 60;
        public double Cash = 0;
        public Cost CostPerHour = new Cost();
        public Cost CostPerDay = new Cost();
        public Cost CostPerMonth = new Cost();
        public Account CurrentAccount;
        public List<Account> Accounts = new List<Account>();
        public List<Place> Places = new List<Place>();
        public List<Client> Clients = new List<Client>();
        public List<Event> Events = new List<Event>();
        public List<string> Marks = new List<string>();
        public List<string> Models = new List<string>();
        public DateTime LastOpenDate = DateTime.Now;
    }
}
