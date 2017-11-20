using Common;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank
{
    public class BankConnetion : IBankConnection
    {

        object locker = new object();
        public void AddAccount(User u)
        {
            // TODO izmeniti da vraca poruku o postojanju na klijenta a ne na serveru da ispisuje
            User desifrovanKorisnik = new User();
            


            desifrovanKorisnik.Username = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(u.Username), "kljuc");
            desifrovanKorisnik.Password = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(u.Password), "kljuc");
            desifrovanKorisnik.Uloga = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(u.Uloga), "kljuc");


            if (BankDB.BazaRacuna.ContainsKey(u.Username)) {

                Console.WriteLine("Ovaj korisnik vec postoji");
                return;

            }
            BankDB.BazaKorisnika.Add(desifrovanKorisnik.Username,desifrovanKorisnik);

            upisiKorisnika(BankDB.BazaKorisnika); //moras ih sifrovati pre upisa
        }

        public User CheckLogin(string username, string password, string nacinLogovanja)
        {
            List<Object> userIRacun = new List<Object>();
            if (nacinLogovanja == "client")
            {

                string usernameDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(username), "kljuc");
                string passwordDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(password), "kljuc");

                if (BankDB.BazaKorisnika.ContainsKey(username))
                {
                    if (BankDB.BazaKorisnika[username].Password == password)
                    {
                        if (BankDB.BazaKorisnika[username].Uloga == "admin" || BankDB.BazaKorisnika[username].Uloga == "korisnik")
                        {
                            User u = BankDB.BazaKorisnika[username];
                            //Racun r = !BankDB.BazaRacuna.ContainsKey(username) ? null : BankDB.BazaRacuna[username];
                            //userIRacun.Add(u);
                            //userIRacun.Add(r);

                            return u;
                        }
                        
                    }
                }
            }
            else if (nacinLogovanja == "operater")
            {
                string usernameDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(username), "kljuc");
                string passwordDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(password), "kljuc");
                if (BankDB.BazaKorisnika.ContainsKey(username))
                {
                    if (BankDB.BazaKorisnika[username].Password == password)
                    {
                        if (BankDB.BazaKorisnika[username].Uloga == "operater")
                        {
                            User u = BankDB.BazaKorisnika[username];
                            //Racun r = !BankDB.BazaRacuna.ContainsKey(username) ? null : BankDB.BazaRacuna[username];
                            //userIRacun.Add(u);
                            //userIRacun.Add(r);
                            return u;
                        }

                    }
                }
            }
            return null;
        }

        public Racun KreirajRacun(Racun r)
        {

            string usernameDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.Username), "kljuc");
            string brojRacunaDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.BrojRacuna), "kljuc");
            string operatorDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.Operater), "kljuc");
            string tipRacunaDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.TipRacuna), "kljuc");







            if (BankDB.BazaRacuna.ContainsKey(brojRacunaDesifrovan))
            {
                return null; // vec postoji
            }
            if (!BankDB.BazaKorisnika.ContainsKey(usernameDesifrovan))
            {
                return null; // ne postoji korisnik koji se stavlja kao vlasnik racuna
            }
            if (tipRacunaDesifrovan == "fizicki" && !BankDB.BazaKorisnika.ContainsKey(operatorDesifrovan))
            {
                return null; // ne postoji operater na kog zeli da se doda
            }
            if (tipRacunaDesifrovan == "pravni") // Ako je ovo zapravo racun od operatera
            {
                // Provera da li jedan operater ima vise racuna, ako vec postoji jedan racun za tog operatera vrati false
                foreach (var racun in BankDB.BazaRacuna)
                {
                    if (racun.Value.TipRacuna == "pravni" && racun.Value.Username == usernameDesifrovan)
                    {
                        // Prodji kroz sve operaterske racuna i proveri da li vec postoji racun koji je napravljen za tipa telenor
                        return null;
                    }
                }
            }
            Racun desifrovan = new Racun();
            desifrovan.BrojRacuna = brojRacunaDesifrovan;
            desifrovan.Operater = operatorDesifrovan;
            desifrovan.StanjeRacuna= Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(r.StanjeRacuna), "kljuc");
            desifrovan.TipRacuna = tipRacunaDesifrovan;
            desifrovan.Username = usernameDesifrovan;


            BankDB.BazaRacuna.Add(desifrovan.Username, desifrovan);
            upisiRacun(BankDB.BazaRacuna);

            // Obavesti odgovarajuceg operatera kako bi dodao novi klijentski racun
            Client cli = new Client();
            IGatewayConnection gatewayProxy = cli.GetGatewayProxy();
            if (desifrovan.TipRacuna == "fizicki" && desifrovan.Operater != "null")
            {

                string sifrovanaIp = BitConverter.ToString(Sifrovanje.sifrujCBC(BankDB.BazaKorisnika[desifrovan.Operater].IpAddress, "kljuc"));
                string sifrovanPort = BitConverter.ToString(Sifrovanje.sifrujCBC(BankDB.BazaKorisnika[desifrovan.Operater].Port, "kljuc"));

                gatewayProxy.BankToOperatorNotifyRacunAdded(r,sifrovanaIp ,sifrovanPort);
            }
            
            
            

            return desifrovan;

        }
        //kod klijenta sam stigao do Obrisi Racun...
        public bool ObrisiRacun(string brRacuna)
        {
            if (!BankDB.BazaRacuna.ContainsKey(brRacuna))
            {
                return false;
            }
            BankDB.BazaRacuna.Remove(brRacuna);
            return true;
        }

        public bool SetIpAndPort(string username, string ip, int port)
        {
            // Nadji korisnika sa ovim usernameom i postavi mu port
            if (BankDB.BazaAktivnihOperatera[username].IpAddress != null && BankDB.BazaAktivnihOperatera[username].Port != 0)
            {
                //Console.WriteLine("Instanca ovog operatera ili korisnika je vec pokrenuta");
                return false;
            }
            // Potrebno je napraviti 2 dictionaryja ko je aktivan
            BankDB.BazaKorisnika[username].IpAddress = ip;
            BankDB.BazaKorisnika[username].Port = port;
            BankDB.BazaAktivnihOperatera.Add(BankDB.BazaKorisnika[username].Username, BankDB.BazaKorisnika[username]);
            return true;
        }

        //public bool Transfer(string mojRacun, string racunOperatera, string pozivNaBroj, int value)
        public bool Transfer(string myUsername, string myUsernameOnOperator, string operatorUsername, int value)
        {
            bool retVal = true;
            if (BankDB.BazaKorisnika.ContainsKey(myUsername))
            {
                Console.WriteLine("Predstavili ste se kao nepostojeci korisnik");
                retVal = false;
            }

            if (BankDB.BazaKorisnika.ContainsKey(operatorUsername))
            {
                Console.WriteLine("Nepostojeci operator");
                retVal = false;
            }

            if (value <= 0)
            {
                Console.WriteLine("Novcana suma mora biti pozitivan broj");
                retVal = false;
            }

            if (BankDB.BazaRacuna[myUsername].StanjeRacuna < value)
            {
                Console.WriteLine("Nemate dovoljno novcanih sredstava na racunu");
                retVal = false;
            }

            BankDB.BazaRacuna[myUsername].StanjeRacuna -= value;
            BankDB.BazaRacuna[operatorUsername].StanjeRacuna += value;

            upisiRacun(BankDB.BazaRacuna);
            /*  
                Treba pozvati operatera preko wcf-a i obavestiti ga da je korisnik izvrsio uplatu na njegov racun.
                Da bi se to uradilo potrebno je da znamo ip adresu korisnika i njegov port na kome slusa. Te podatke o 
                korisniku treba dodati prikom njegovog dodavanja u banku. 

                Dodavanje u banku treba razdvojiti na dva slucaja:
                    * dodavanje korisnika -> dodavanje korisnika je ovo sto trenutno imamo
                    * dodavanje operatera -> za dodavanje operatera treba dodati informafije o ipAdresi i portu

                    Ognjen: I za klijente tj korisnike treba ip adresa i port, nema potrebe praviti 2 metode, zato sto i klijenti i operateri imaju username i password, samo im je uloga drugacija
                
                Kada se ovo odrati poziva se sledece: 

                IOperatorConnection proxy = Client.GetOperatorProxy(ip, port);
                proxy.UpdateStatus(myUsernameOnOperator, value);
            */
            Client cli = new Client();
            IGatewayConnection gatewayToOperator = cli.GetGatewayProxy();
            gatewayToOperator.BankToOperatorUpdateStatus(myUsername,operatorUsername,value, BankDB.BazaKorisnika[operatorUsername].IpAddress, BankDB.BazaKorisnika[operatorUsername].Port);

            return retVal;
        }

        public void upisiKorisnika(Dictionary<string, User> recnikKorisnika)
        {
            lock (locker)
            {
                string putanja = Environment.CurrentDirectory + "\\korisnici.xml";
                List<User> listaKorisnika = new List<User>();
                //prepisati iz recnika u ovu listu
                foreach (User u in recnikKorisnika.Values)
                {
                    User sifrovan = new User();
                    sifrovan.Username = BitConverter.ToString(Sifrovanje.sifrujCBC(u.Username, "kljuc"));
                    sifrovan.Password = BitConverter.ToString(Sifrovanje.sifrujCBC(u.Password, "kljuc"));
                    sifrovan.Uloga = BitConverter.ToString(Sifrovanje.sifrujCBC(u.Uloga, "kljuc"));



                    listaKorisnika.Add(sifrovan);
                }

                XmlSerializer ser = new XmlSerializer(typeof(List<User>));

                StreamWriter sw = new StreamWriter(putanja);
                ser.Serialize(sw, listaKorisnika);

                sw.Close();
            }

        }

        public void upisiRacun(Dictionary<string, Racun> recnikRacuna)
        {
            lock (locker)
            {
                string putanja = Environment.CurrentDirectory + "\\racuni.xml";

                List<Racun> listaRacuna = new List<Racun>();
                //prepisati iz recnika u ovu listu
                foreach (Racun r in recnikRacuna.Values)
                {
                    Racun sifrovan = new Racun();
                    sifrovan.BrojRacuna= BitConverter.ToString(Sifrovanje.sifrujCBC(r.BrojRacuna, "kljuc"));
                    sifrovan.Operater = BitConverter.ToString(Sifrovanje.sifrujCBC(r.Operater, "kljuc"));
                    sifrovan.StanjeRacuna = BitConverter.ToString(Sifrovanje.sifrujCBC(r.StanjeRacuna, "kljuc"));
                    sifrovan.TipRacuna = BitConverter.ToString(Sifrovanje.sifrujCBC(r.TipRacuna, "kljuc"));
                    sifrovan.Username = BitConverter.ToString(Sifrovanje.sifrujCBC(r.Username, "kljuc"));



                    listaRacuna.Add(sifrovan);
                }

                XmlSerializer ser = new XmlSerializer(typeof(List<Racun>));

                StreamWriter sw = new StreamWriter(putanja);
                ser.Serialize(sw, listaRacuna);

                sw.Close();
            }

        }

        public Racun UzmiKlijentskiRacun(string username)
        {
            string usernameDesifrovan = Sifrovanje.desifrujCBC(Encoding.ASCII.GetBytes(username), "kljuc");

            return BankDB.BazaRacuna[usernameDesifrovan];
        }
    }
}
