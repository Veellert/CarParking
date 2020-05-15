using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaya
{
    public enum Category
    {
        Common,
        Student,
        Pensioner
    }

    public enum EventType
    {
        RegClient,
        CarIn,
        CarOut,
        Pay
    }
    public class DB
    {
        public static Parking Parking = new Parking();
        public Client newClient = new Client();
        public Car newcar = new Car();
        static int ticketID = 1;
        static BinaryFormatter BF = new BinaryFormatter();
        //static string myDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //static string dbPath = myDoc+@"\Parkovka\Res\DB.db";
        static string dbPath = @"Res\DB.db";

        public DB()
        {
            if (!File.Exists(dbPath))
                return;
            using (FileStream fs = new FileStream(dbPath, FileMode.Open, FileAccess.Read))
            {
                byte[] array = new byte[4];
                fs.Read(array, 0, 4);
                ticketID = BitConverter.ToInt32(array, 0);
                Parking = ((Parking)BF.Deserialize(fs));
            }
        }

        public void CheckPay()
        {
            DateTime date = Parking.LastOpenDate;
            foreach(var client in Parking.Clients)
                foreach (var car in client.Cars.Where(s => !s.Place.Free))
                {
                    while (car.Place.LastPayDate < date)
                    {
                        if (car.Place.LastPayDate.AddSeconds(5.0) > date)
                            car.Place.LastPayDate = car.Place.LastPayDate.AddSeconds(1.0);
                        if (car.Place.LastPayDate.AddSeconds(5.0) <= date)
                            car.Client.pay1 += car.Client.PayPerHour;
                        if (car.Place.DPayDate.AddSeconds(10.0) <= date)
                            car.Client.pay2 += car.Client.PayPerDay;
                        if (car.Place.MPayDate.AddSeconds(30.0) <= date)
                            car.Client.pay3 += car.Client.PayPerMonth;
                        if (car.Client.pay1 != 0 && car.Client.pay2 == 0 && car.Client.pay3 == 0)
                        {
                            car.Place.LastPayDate = car.Place.LastPayDate.AddSeconds(5.0);
                            car.Client.Payment += car.Client.PayPerHour;
                            car.Client.pay1 = 0;
                        }
                        if (car.Client.pay2 != 0 && car.Client.pay3 == 0)
                        {
                            car.Place.LastPayDate = car.Place.DPayDate.AddSeconds(10.0);
                            car.Place.DPayDate = car.Place.DPayDate.AddSeconds(10.0);
                            car.Client.Payment = car.Client.DayPayment;
                            car.Client.Payment += car.Client.PayPerDay;
                            car.Client.DayPayment = car.Client.Payment;
                            car.Client.pay1 = 0;
                            car.Client.pay2 = 0;
                        }
                        if (car.Client.pay3 != 0)
                        {
                            car.Place.LastPayDate = car.Place.MPayDate.AddSeconds(30.0);
                            car.Place.DPayDate = car.Place.MPayDate.AddSeconds(30.0);
                            car.Place.MPayDate = car.Place.MPayDate.AddSeconds(30.0);
                            car.Client.Payment = car.Client.MonthPayment;
                            car.Client.Payment += car.Client.PayPerMonth;
                            car.Client.DayPayment = car.Client.Payment;
                            car.Client.MonthPayment = car.Client.Payment;
                            car.Client.pay1 = 0;
                            car.Client.pay2 = 0;
                            car.Client.pay3 = 0;
                        }
                    }return;
                }
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(dbPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(BitConverter.GetBytes(ticketID), 0, 4);
                BF.Serialize(fs, Parking);
            }
        }

        public void DeSave()
        {
            File.Delete(dbPath);
            Parking = new Parking();
            newClient = new Client();
            newcar = new Car();
            ticketID = 1;
            Application.Exit();
        }

        public string NewEventMessege(EventType type)
        {
            string EventMessege = "";
            switch (type)
            {
                case EventType.RegClient:
                    EventMessege = "Зарегистрирован новый клиент";
                    return EventMessege;
                case EventType.CarIn:
                    EventMessege = "Машина заехала";
                    return EventMessege;
                case EventType.CarOut:
                    EventMessege = "Машина выехала";
                    return EventMessege;
                case EventType.Pay:
                    EventMessege = "Заплатил за стоянку";
                    return EventMessege;
            }
            return EventMessege;
        }

        public void EventSave(EventType type, DateTime date, Client Client)
        {
            Event newEvent = new Event
            {
                Date = date,
                Client = Client,
                EventMesege = NewEventMessege(type)
            };
            Parking.Events.Add(newEvent);
            Save();
        }

        public int SetTicket() => ticketID++;

        public void UpdatePay()
        {
            var humen = Parking.Clients;
            foreach (var ClientL in humen)
                foreach (var car in ClientL.Cars.Where(s => !s.Place.Free))
                {
                    if (car.Place.LastPayDate.AddSeconds(5.0) <= DateTime.Now)
                        car.Client.pay1 += car.Client.PayPerHour;
                    if (car.Place.DPayDate.AddSeconds(10.0) <= DateTime.Now)
                        car.Client.pay2 += car.Client.PayPerDay;
                    if (car.Place.MPayDate.AddSeconds(30.0) <= DateTime.Now)
                        car.Client.pay3 += car.Client.PayPerMonth;
                    if (car.Client.pay1 != 0 && car.Client.pay2 == 0 && car.Client.pay3 == 0)
                    {
                        car.Place.LastPayDate = DateTime.Now;
                        car.Client.Payment += car.Client.PayPerHour;
                        car.Client.pay1 = 0;
                    }
                    if (car.Client.pay2 != 0 && car.Client.pay3 == 0)
                    {
                        car.Place.LastPayDate = DateTime.Now;
                        car.Place.DPayDate = DateTime.Now;
                        car.Client.Payment = car.Client.DayPayment;
                        car.Client.Payment += car.Client.PayPerDay;
                        car.Client.DayPayment = car.Client.Payment;
                        car.Client.pay1 = 0;
                        car.Client.pay2 = 0;
                    }
                    if(car.Client.pay3 != 0)
                    {
                        car.Place.LastPayDate = DateTime.Now;
                        car.Place.DPayDate = DateTime.Now;
                        car.Place.MPayDate = DateTime.Now;
                        car.Client.Payment = car.Client.MonthPayment;
                        car.Client.Payment += car.Client.PayPerMonth;
                        car.Client.DayPayment = car.Client.Payment;
                        car.Client.MonthPayment = car.Client.Payment;
                        car.Client.pay1 = 0;
                        car.Client.pay2 = 0;
                        car.Client.pay3 = 0;
                    }
                }
        }

        public void SetPrice(int costH, int costD, int costM, int perS, int perP, int perD)
        {
            Parking.PercentStudent = perS;
            Parking.PercentPensioner = perP;
            Parking.PercentDisability = perD;
            Parking.CostPerHour.CostForCommon = costH;
            Parking.CostPerHour.CostForStudent = costH - (costH * perS / 100);
            Parking.CostPerHour.CostForPensioner = costH - (costH * perP / 100);
            Parking.CostPerHour.CostForDisability = costH - (costH * perD / 100);
            Parking.CostPerDay.CostForCommon = costD;
            Parking.CostPerDay.CostForStudent = costD - (costD * perS / 100);
            Parking.CostPerDay.CostForPensioner = costD - (costD * perP / 100);
            Parking.CostPerDay.CostForDisability = costD - (costD * perD / 100);
            Parking.CostPerMonth.CostForCommon = costM;
            Parking.CostPerMonth.CostForStudent = costM - (costM * perS / 100);
            Parking.CostPerMonth.CostForPensioner = costM - (costM * perP / 100);
            Parking.CostPerMonth.CostForDisability = costM - (costM * perD / 100);
        }

        public void SetPlaces(int count, int countDisability)
        {
            Parking.Places.RemoveAll(s => s.Free);
            int newNumber = Parking.Places.Count + 1;
            for (int i = 0; i < countDisability; i++)
            {
                Place newPlace = new Place
                {
                    Disability = true,
                    Free = true,
                    Number = newNumber++.ToString(),
                    LastFreeDate = DateTime.Now
                };
                Parking.Places.Add(newPlace);
            }
            for (int i = 0; i < count; i++)
            {
                Place newPlace = new Place
                {
                    Disability = false,
                    Free = true,
                    Number = newNumber++.ToString(),
                    LastFreeDate = DateTime.Now
                };
                Parking.Places.Add(newPlace);
            }
            Parking.CountPlaces = count;
            Parking.CountDisabilityPlaces = countDisability;
        }

        public Client AddClient(Client Client, Passport Passport, List<string> Phones, List<Car> cars, Category category)
        {
            var result = Parking.Clients.Find(s => s.Passport == Passport);
            var result2 = Parking.Clients.Find(s => s.Phones.Equals(Phones));
            var result3 = Parking.Clients.Find(s => s.Cars.Equals(cars));
            if (result != null)
            {
                MessageBox.Show($"{Passport.FIO()} уже существует", "Error.InvalidClientData", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return result;
            }
            if (result2 != null)
                if (MessageBox.Show($"По этому номеру уже зарегистрирован {result2.Passport.FIO()}, продолжить?", "Error.InvalidClientData",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return result2;
            if (result3 != null)
                if (MessageBox.Show($"Эта машина зарегистрирована на {result3.Passport.FIO()}, продолжить?",
                    "Error.InvalidClientData", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return result3;
            Client.Payment = 0;
            Client.Passport = Passport;
            Client.Category = category;
            Client.Cars = new List<Car>();
            Client.Phones = new List<string>();
            Client.Cars.AddRange(cars);
            Client.Phones.AddRange(Phones);
            int payH = 0;
            int payD = 0;
            int payM = 0;
            if (category == Category.Common)
            {
                payH = Parking.CostPerHour.CostForCommon;
                payD = Parking.CostPerDay.CostForCommon;
                payM = Parking.CostPerMonth.CostForCommon;
            }
            if (category == Category.Student)
            {
                payH = Parking.CostPerHour.CostForStudent;
                payD = Parking.CostPerDay.CostForStudent;
                payM = Parking.CostPerMonth.CostForStudent;
            }
            if (category == Category.Pensioner)
            {
                payH = Parking.CostPerHour.CostForPensioner;
                payD = Parking.CostPerDay.CostForPensioner;
                payM = Parking.CostPerMonth.CostForPensioner;
            }
            if (Client.Disability)
            {
                payH = Parking.CostPerHour.CostForDisability;
                payD = Parking.CostPerDay.CostForDisability;
                payM = Parking.CostPerMonth.CostForDisability;
            }
            Client.PayPerHour = payH;
            Client.PayPerDay = payD;
            Client.PayPerMonth = payM;
            Parking.Clients.Add(Client);
            return Client;
        }

        public void RedactClient(Client Client, Passport Passport, List<string> Phones, List<Car> cars, Category category)
        {
            Client.Passport = Passport;
            Client.Category = category;
            Client.Cars = cars;
            Client.Phones = Phones;
            int payH = 0;
            int payD = 0;
            int payM = 0;
            if (category == Category.Common)
            {
                payH = Parking.CostPerHour.CostForCommon;
                payD = Parking.CostPerDay.CostForCommon;
                payM = Parking.CostPerMonth.CostForCommon;
            }
            if (category == Category.Student)
            {
                payH = Parking.CostPerHour.CostForStudent;
                payD = Parking.CostPerDay.CostForStudent;
                payM = Parking.CostPerMonth.CostForStudent;
            }
            if (category == Category.Pensioner)
            {
                payH = Parking.CostPerHour.CostForPensioner;
                payD = Parking.CostPerDay.CostForPensioner;
                payM = Parking.CostPerMonth.CostForPensioner;
            }
            if (Client.Disability)
            {
                payH = Parking.CostPerHour.CostForDisability;
                payD = Parking.CostPerDay.CostForDisability;
                payM = Parking.CostPerMonth.CostForDisability;
            }
            Client.PayPerHour = payH;
            Client.PayPerDay = payD;
            Client.PayPerMonth = payM;
            Save();
        }
    }
}
