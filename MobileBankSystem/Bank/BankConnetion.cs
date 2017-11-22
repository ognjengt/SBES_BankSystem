﻿using Common;
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
        /*
         kr rac
         transf

             */
        object locker = new object();
        public void AddAccount(User u,int mode)
        {
            // TODO izmeniti da vraca poruku o postojanju na klijenta a ne na serveru da ispisuje


            if (mode == 1)
            {

                User desifrovanKorisnik = new User();

                desifrovanKorisnik.Username = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(u.Username), "kljuc");
                desifrovanKorisnik.Password = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(u.Password), "kljuc");
                desifrovanKorisnik.Uloga = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(u.Uloga), "kljuc");


                if (BankDB.BazaKorisnika.ContainsKey(u.Username))
                {

                    Console.WriteLine("Ovaj korisnik vec postoji");
                    return;

                }
                BankDB.BazaKorisnika.Add(desifrovanKorisnik.Username, desifrovanKorisnik);

                upisiKorisnika(BankDB.BazaKorisnika,mode);
            }
            else
            {
                User desifrovanKorisnik = new User();

                desifrovanKorisnik.Username = Sifrovanje.desifrujECB(Sifrovanje.spremiZaDesifrovanje(u.Username), "kljuc");
                desifrovanKorisnik.Password = Sifrovanje.desifrujECB(Sifrovanje.spremiZaDesifrovanje(u.Password), "kljuc");
                desifrovanKorisnik.Uloga = Sifrovanje.desifrujECB(Sifrovanje.spremiZaDesifrovanje(u.Uloga), "kljuc");


                if (BankDB.BazaKorisnika.ContainsKey(u.Username))
                {

                    Console.WriteLine("Ovaj korisnik vec postoji");
                    return;

                }
                BankDB.BazaKorisnika.Add(desifrovanKorisnik.Username, desifrovanKorisnik);

                upisiKorisnika(BankDB.BazaKorisnika,mode);
            }

        }

        public User CheckLogin(string username, string password, string nacinLogovanja)
        {
            List<Object> userIRacun = new List<Object>();
            if (nacinLogovanja == "client")
            {

                string usernameDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(username), "kljuc");
                string passwordDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(password), "kljuc");

                if (BankDB.BazaKorisnika.ContainsKey(usernameDesifrovan))
                {
                    if (BankDB.BazaKorisnika[usernameDesifrovan].Password == passwordDesifrovan)
                    {
                        if (BankDB.BazaKorisnika[usernameDesifrovan].Uloga == "admin" || BankDB.BazaKorisnika[usernameDesifrovan].Uloga == "korisnik")
                        {
                            User u = BankDB.BazaKorisnika[usernameDesifrovan];
                            return u;
                        }
                        
                    }
                }
            }
            else if (nacinLogovanja == "operater")
            {
                string usernameDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(username), "kljuc");
                string passwordDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(password), "kljuc");
                if (BankDB.BazaKorisnika.ContainsKey(usernameDesifrovan))
                {
                    if (BankDB.BazaKorisnika[usernameDesifrovan].Password == passwordDesifrovan)
                    {
                        if (BankDB.BazaKorisnika[usernameDesifrovan].Uloga == "operater")
                        {
                            User u = BankDB.BazaKorisnika[usernameDesifrovan];
                            return u;
                        }

                    }
                }
            }
            return null;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Racun KreirajRacun(Racun r)
        {

            string usernameDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Username), "kljuc");
            string brojRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.BrojRacuna), "kljuc");
            string operatorDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Operater), "kljuc");
            string tipRacunaDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.TipRacuna), "kljuc");

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
            desifrovan.StanjeRacuna= Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.StanjeRacuna), "kljuc");
            desifrovan.TipRacuna = tipRacunaDesifrovan;
            desifrovan.Username = usernameDesifrovan;


            BankDB.BazaRacuna.Add(desifrovan.BrojRacuna, desifrovan);
            upisiRacun(BankDB.BazaRacuna);

            // Obavesti odgovarajuceg operatera kako bi dodao novi klijentski racun
            Client<IGatewayConnection> cli = new Client<IGatewayConnection>("mbgateway", "localhost", "63000");
            IGatewayConnection factory = cli.GetProxy();
            if (desifrovan.TipRacuna == "fizicki" && desifrovan.Operater != "null")
            {

                string sifrovanaIp = BitConverter.ToString(Sifrovanje.sifrujCBC(BankDB.BazaKorisnika[desifrovan.Operater].IpAddress, "kljuc"));
                string sifrovanPort = BitConverter.ToString(Sifrovanje.sifrujCBC(BankDB.BazaKorisnika[desifrovan.Operater].Port, "kljuc"));

                factory.BankToOperatorNotifyRacunAdded(r,sifrovanaIp ,sifrovanPort);
            }
            
            return desifrovan;

        }
        //kod klijenta sam stigao do Obrisi Racun...
        public bool ObrisiRacun(string brRacuna)
        {
            string desifrovanRacun = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(brRacuna), "kljuc");
            if (!BankDB.BazaRacuna.ContainsKey(desifrovanRacun))
            {
                return false;
            }
            BankDB.BazaRacuna.Remove(desifrovanRacun);
            return true;
        }

        public bool SetIpAndPort(string username, string ip, string port)
        {
            // Postavlja IP i PORT OPERATORA!
            string desifrovanUsername = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(username), "kljuc");
            // Nadji korisnika sa ovim usernameom i postavi mu port
            if (BankDB.BazaAktivnihOperatera.ContainsKey(desifrovanUsername))
            {
                return false;
            }
            BankDB.BazaKorisnika[desifrovanUsername].IpAddress = ip;
            BankDB.BazaKorisnika[desifrovanUsername].Port = port;
            BankDB.BazaAktivnihOperatera.Add(BankDB.BazaKorisnika[desifrovanUsername].Username, BankDB.BazaKorisnika[desifrovanUsername]);
            return true;
        }

        public bool SetIpAndPortClient(string username, string ip, string port)
        {
            // Postavlja IP i PORT CLIENTA!
            string desifrovanUsername = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(username), "kljuc");
            if (BankDB.BazaAktivnihKorisnika.ContainsKey(desifrovanUsername))
            {
                return false;
            }
            BankDB.BazaKorisnika[desifrovanUsername].IpAddress = ip;
            BankDB.BazaKorisnika[desifrovanUsername].Port = port;
            BankDB.BazaAktivnihKorisnika.Add(BankDB.BazaKorisnika[desifrovanUsername].Username, BankDB.BazaKorisnika[desifrovanUsername]);
            return true;
        }

        public bool ShutdownClient(string username)
        {
            string desifrovanUsername = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(username), "kljuc");
            if (!BankDB.BazaAktivnihKorisnika.ContainsKey(desifrovanUsername))
            {
                return false;
            }
            BankDB.BazaAktivnihKorisnika.Remove(desifrovanUsername);
            return true;
        }

        public bool ShutdownOperator(string username)
        {
            string desifrovanUsername = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(username), "kljuc");
            if (!BankDB.BazaAktivnihOperatera.ContainsKey(desifrovanUsername))
            {
                return false;
            }
            BankDB.BazaAktivnihOperatera.Remove(desifrovanUsername);
            return true;
        }

        //public bool Transfer(string mojRacun, string racunOperatera, string pozivNaBroj, int value)
        public bool Transfer(string brojKlijentskogRacuna, string brojOperatorskogRacuna, string korisnikKojiVrsiTransfer, string value)
        {
            string desifrovanBrojKlijentskogRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(brojKlijentskogRacuna), "kljuc");
            string desifrovanBrojOperatorskogRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(brojOperatorskogRacuna), "kljuc");
            string desifrovanKorisnikKojiVrsiTransfer = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(korisnikKojiVrsiTransfer), "kljuc");
            string desifrovanValue = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(value), "kljuc");

            if (!BankDB.BazaKorisnika.ContainsKey(desifrovanKorisnikKojiVrsiTransfer))
            {
                Console.WriteLine("Predstavili ste se kao nepostojeci korisnik");
                return false;
            }

            if (!BankDB.BazaRacuna.ContainsKey(desifrovanBrojOperatorskogRacuna))
            {
                Console.WriteLine("Nepostojeci racun!");
                return false;
            }

            if (Int32.Parse(desifrovanValue) <= 0)
            {
                Console.WriteLine("Novcana suma mora biti pozitivan broj");
                return false;
            }

            if (Int32.Parse(BankDB.BazaRacuna[desifrovanBrojKlijentskogRacuna].StanjeRacuna) < Int32.Parse(desifrovanValue))
            {
                Console.WriteLine("Nemate dovoljno novcanih sredstava na racunu");
                return false;
            }

            int stanjeKlijenta = Int32.Parse(BankDB.BazaRacuna[desifrovanBrojKlijentskogRacuna].StanjeRacuna) - Int32.Parse(desifrovanValue);
            int stanjeOperatora = Int32.Parse(BankDB.BazaRacuna[desifrovanBrojOperatorskogRacuna].StanjeRacuna) + Int32.Parse(desifrovanValue);

            BankDB.BazaRacuna[desifrovanBrojKlijentskogRacuna].StanjeRacuna = stanjeKlijenta.ToString();
            BankDB.BazaRacuna[desifrovanBrojOperatorskogRacuna].StanjeRacuna = stanjeOperatora.ToString();

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

            string operatorKomeJeUplaceno = BankDB.BazaRacuna[desifrovanBrojOperatorskogRacuna].Username;
            string sifrovanOperatorKomeJeUplaceno = BitConverter.ToString(Sifrovanje.sifrujCBC(operatorKomeJeUplaceno, "kljuc"));

            Client<IGatewayConnection> cli = new Client<IGatewayConnection>("mbgateway", "localhost", "63000");
            IGatewayConnection factory = cli.GetProxy();
            // opet se prosledjuje ovaj koji je bio sifrovan
            factory.BankToOperatorUpdateStatus(korisnikKojiVrsiTransfer,sifrovanOperatorKomeJeUplaceno,value, BankDB.BazaKorisnika[operatorKomeJeUplaceno].IpAddress, BankDB.BazaKorisnika[operatorKomeJeUplaceno].Port);

            return true;
        }

        public void upisiKorisnika(Dictionary<string, User> recnikKorisnika,int mode)
        {


            if (mode == 1)
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

            else
            {

                lock (locker)
                {
                    string putanja = Environment.CurrentDirectory + "\\korisnici.xml";
                    List<User> listaKorisnika = new List<User>();
                    //prepisati iz recnika u ovu listu
                    foreach (User u in recnikKorisnika.Values)
                    {
                        User sifrovan = new User();
                        sifrovan.Username = BitConverter.ToString(Sifrovanje.sifrujECB(u.Username, "kljuc"));
                        sifrovan.Password = BitConverter.ToString(Sifrovanje.sifrujECB(u.Password, "kljuc"));
                        sifrovan.Uloga = BitConverter.ToString(Sifrovanje.sifrujECB(u.Uloga, "kljuc"));

                        listaKorisnika.Add(sifrovan);
                    }

                    XmlSerializer ser = new XmlSerializer(typeof(List<User>));

                    StreamWriter sw = new StreamWriter(putanja);
                    ser.Serialize(sw, listaKorisnika);

                    sw.Close();
                }
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
            string usernameDesifrovan = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(username), "kljuc");

            foreach (var racun in BankDB.BazaRacuna.Values)
            {
                if (racun.Username == usernameDesifrovan)
                {
                    return racun;
                }
            }
            return null;
        }
    }
}
