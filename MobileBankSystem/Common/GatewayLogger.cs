using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    public static class GatewayLogger
    {
        static object locker = new object();
        public static Dictionary<string, Metoda> BazaStatistikeMetoda = new Dictionary<string, Metoda>();
        public static Dictionary<string, Inicijator> BazaStatistikeInicijatora = new Dictionary<string, Inicijator>();

        public static void SacuvajStatistikuMetoda()
        {
            lock (locker)
            {
                string putanja = Environment.CurrentDirectory + "\\statistikaMetoda.xml";
                List<Metoda> listaMetoda = new List<Metoda>();
                //prepisati iz recnika u ovu listu
                foreach (Metoda m in BazaStatistikeMetoda.Values)
                {
                    listaMetoda.Add(m);
                }

                XmlSerializer ser = new XmlSerializer(typeof(List<Metoda>));

                StreamWriter sw = new StreamWriter(putanja);
                ser.Serialize(sw, listaMetoda);

                sw.Close();
            }

        }
        public static void SacuvajStatistikuInicijatora()
        {
            lock (locker)
            {
                string putanja = Environment.CurrentDirectory + "\\statistikaInicijatora.xml";
                List<Inicijator> listaInicijatora = new List<Inicijator>();
                //prepisati iz recnika u ovu listu
                foreach (Inicijator ini in BazaStatistikeInicijatora.Values)
                {
                    listaInicijatora.Add(ini);
                }

                XmlSerializer ser = new XmlSerializer(typeof(List<Inicijator>));

                StreamWriter sw = new StreamWriter(putanja);
                ser.Serialize(sw, listaInicijatora);

                sw.Close();
            }
        }
        public static void UcitajStatistikuMetoda()
        {
            try
            {
                string putanja = Environment.CurrentDirectory + "\\statistikaMetoda.xml";

                List<Metoda> listaMetoda = new List<Metoda>();
                XmlSerializer xs = new XmlSerializer(typeof(List<Metoda>));
                StreamReader sr = new StreamReader(putanja);
                listaMetoda = (List<Metoda>)xs.Deserialize(sr);
                sr.Close();
                foreach (Metoda m in listaMetoda)
                {
                    BazaStatistikeMetoda.Add(m.NazivMetode, m);
                }
            }
            catch
            {

            }
        }
        public static void UcitajStatistikuInicijatora()
        {
            try
            {
                string putanja = Environment.CurrentDirectory + "\\statistikaInicijatora.xml";

                List<Inicijator> listaInicijatora = new List<Inicijator>();
                XmlSerializer xs = new XmlSerializer(typeof(List<Inicijator>));
                StreamReader sr = new StreamReader(putanja);
                listaInicijatora = (List<Inicijator>)xs.Deserialize(sr);
                sr.Close();
                foreach (Inicijator ini in listaInicijatora)
                {
                    BazaStatistikeInicijatora.Add(ini.Username, ini);
                }
            }
            catch
            {

            }
        }
        public static void NajcesceKoriscenaUslugaBanke()
        {
            //var max = BazaStatistikeMetoda.Where(s => s.Value.NazivServisa =="Bank").Aggregate((l, r) => l.Value.BrojPoziva > r.Value.BrojPoziva ? l : r).Key;
            //Console.WriteLine("Najcesce koriscena usluga banke: " + BazaStatistikeMetoda[max].NazivMetode + " , Broj poziva: " + BazaStatistikeMetoda[max].BrojPoziva);

            Metoda max = new Metoda();
            max.BrojPoziva = -1;
            foreach (var metoda in BazaStatistikeMetoda.Values)
            {
                if (metoda.NazivServisa == "Bank")
                {
                    if (metoda.BrojPoziva > max.BrojPoziva) max = metoda;
                }
            }
            Console.WriteLine("Najcesce koriscena usluga banke: " + max.NazivMetode + " , Broj poziva: " + max.BrojPoziva);
        }
        public static void NajcesceKoriscenaUslugaOperatera()
        {
            // Nadji svakog ko nije "Bank" i dodaj ga u neku listu ( uzmem sve postojece operatere tj usernameove)
            // Za svakog postojeceg operatera, prodjem kroz xml i pokupim njegove metode
            // Na kraju ispisem najjacu metodu od svakog

            List<Metoda> sveOperatorskeMetode = BazaStatistikeMetoda.Values.ToList();
            sveOperatorskeMetode = sveOperatorskeMetode.Where(m => m.NazivServisa != "Bank").ToList();
            Dictionary<string, List<Metoda> > operatori = new Dictionary<string, List<Metoda> >();

            foreach (var metoda in sveOperatorskeMetode)
            {
                // ako dictionary ne sadrzi ni jednu metodu za tog operatera
                if (!operatori.ContainsKey(metoda.NazivServisa))
                {
                    // napravi listu za tog operatera
                    operatori.Add(metoda.NazivServisa, new List<Metoda>());
                }
                // dodaj u listu
                operatori[metoda.NazivServisa].Add(metoda);
            }

            // nakon ovog foreacha u dictionary operatori cu imati sve postojece operatere i sve metode od tih operatera

            // sad prolazim kroz svaki element liste i trazim maksimum od svakog
            if (operatori.Count == 0)
            {
                Console.WriteLine("Ne postoji najcesce koriscena usluga.");
                return;
            }
            foreach (var operater in operatori)
            {
                Metoda max = new Metoda();
                max.BrojPoziva = -1;
                Console.WriteLine("Najcesce koriscena metoda operatera " + operater.Key+" :");
                foreach (var metoda in operater.Value)
                {
                    if (metoda.BrojPoziva > max.BrojPoziva)
                    {
                        max = metoda;
                    }
                }
                Console.WriteLine(max.NazivMetode + " " + max.BrojPoziva);
            }
        }
        public static void AddMethod(string methodName, string serviceName)
        {
            if (!BazaStatistikeMetoda.ContainsKey(methodName))
            {
                Metoda m = new Metoda(methodName, 0, serviceName);
                BazaStatistikeMetoda.Add(m.NazivMetode, m);
            }
            BazaStatistikeMetoda[methodName].BrojPoziva++;
            SacuvajStatistikuMetoda();
        }

        public static void AddInicijator(string inicijatorUsername, string inicijatorIp, int inicijatorPort)
        {
            if (!BazaStatistikeInicijatora.ContainsKey(inicijatorUsername))
            {
                Inicijator i = new Inicijator(inicijatorUsername, 0, inicijatorIp, inicijatorPort);
                BazaStatistikeInicijatora.Add(i.Username, i);
            }
            BazaStatistikeInicijatora[inicijatorUsername].BrojPoziva++;
            SacuvajStatistikuInicijatora();
        }

        public static void NajcesciInicijator()
        {
            Inicijator max = new Inicijator();
            max.BrojPoziva = -1;
            foreach (var inicijator in BazaStatistikeInicijatora.Values)
            {
                if (inicijator.BrojPoziva > max.BrojPoziva) max = inicijator;
            }
            if (max.Username == null && max.BrojPoziva == -1)
            {
                Console.WriteLine("Jos uvek niko nije inicirao ni jednu akciju");
            }
            else Console.WriteLine("Inicijator sa najvise poziva u sistemu: " + max.Username + " , Broj poziva: " + max.BrojPoziva);
        }
        public static void NajcesceAdrese()
        {
            Console.WriteLine("--Top 2 najcesce adrese sa kojih su inicirane akcije--");
            List<Inicijator> listaInicijatora = BazaStatistikeInicijatora.Values.ToList();
            listaInicijatora = listaInicijatora.OrderByDescending(o => o.BrojPoziva).ToList();
            if (listaInicijatora.Count == 0)
            {
                Console.WriteLine("Jos uvek niko nije inicirao ni jednu akciju");
            }
            else if(listaInicijatora.Count == 1)
            {
                Console.WriteLine("Najcesca iniciranja poticu sa adrese: " + listaInicijatora[0].IpAddress + " : " + listaInicijatora[0].Port);
            }
            else
            {
                Console.WriteLine("Najcesca iniciranja poticu sa adresa: ");
                Console.WriteLine("Adresa 1: " + listaInicijatora[0].IpAddress + " : " + listaInicijatora[0].Port);
                Console.WriteLine("Adresa 2: " + listaInicijatora[1].IpAddress + " : " + listaInicijatora[1].Port);
            }
            
        }
        public static void GenerisiIzvestaj()
        {
            try
            {
                UcitajStatistikuMetoda();
                UcitajStatistikuInicijatora();

                Console.WriteLine("===STATISTIKA===");
                NajcesceKoriscenaUslugaBanke();
                Console.WriteLine("----------------------------");
                NajcesceKoriscenaUslugaOperatera();
                Console.WriteLine("----------------------------");
                NajcesciInicijator();
                Console.WriteLine("----------------------------");
                NajcesceAdrese();
                Console.WriteLine("----------------------------");
            }
            catch (Exception)
            {

                throw;
            }
            

        }
    }
}
