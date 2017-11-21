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
                    BazaStatistikeInicijatora.Add(ini.Username,ini);
                }
            }
            catch
            {

            }
        }
        public static void NajcesceKoriscenaUslugaBanke()
        {
            // TODO Ognjen
        }
        public static void NajcesceKoriscenaUslugaOperatera()
        {
            // TODO Ognjen
        }
        public static void NajcesciInicijator()
        {
            // TODO Ognjen
        }
        public static void NajcesceAdrese()
        {
            // TODO Ognjen
        }
    }
}
