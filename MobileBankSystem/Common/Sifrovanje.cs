using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public static class Sifrovanje
    {

        
        public static byte[] sifrujCBC(string ulaznaRec, string kljuc)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md.ComputeHash(utf8.GetBytes(kljuc));
            tDES.Mode = CipherMode.CBC; //valjda moze ovako
            tDES.Padding = PaddingMode.Zeros; //ovo nznm sta je
            tDES.GenerateIV();
            ICryptoTransform trans = tDES.CreateEncryptor();
            byte[] sifrovano = trans.TransformFinalBlock(utf8.GetBytes(ulaznaRec), 0, utf8.GetBytes(ulaznaRec).Length);
            //string ispis = BitConverter.ToString(sifrovano);
            //Console.WriteLine("Sifrovan tekst: " + ispis);
            byte[] newMess = new byte[sifrovano.Length + tDES.IV.Length];
            for (int i = 0; i < newMess.Length; i++)
            {
                if (i < tDES.IV.Length)
                {
                    newMess[i] = tDES.IV[i];
                }
                else
                {
                    newMess[i] = sifrovano[i - tDES.IV.Length];
                }
            }
            return newMess;

        }

        //dekripcija
        public static string desifrujCBC(byte[] sifrovana, string kljuc)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md.ComputeHash(utf8.GetBytes(kljuc));
            tDES.Mode = CipherMode.CBC; //valjda moze ovako
            tDES.Padding = PaddingMode.Zeros;

            byte[] vektor = new byte[tDES.BlockSize / 8];
            byte[] message = new byte[sifrovana.Length - vektor.Length];
            for (int i = 0; i < sifrovana.Length; i++)
            {
                if (i < tDES.BlockSize / 8)
                {
                    vektor[i] = sifrovana[i];
                }
                else
                {
                    message[i - vektor.Length] = sifrovana[i];
                }
            }

            tDES.IV = vektor;
            ICryptoTransform trans = tDES.CreateDecryptor();

            string desifrovano = utf8.GetString(trans.TransformFinalBlock(message, 0, message.Length));

            string sredjeno = desifrovano.Replace("\0","");
            return sredjeno;

        }

        public static byte[] spremiZaDesifrovanje(string ulaz)
        {
            int length = (ulaz.Length + 1) / 3;
            byte[] arr1 = new byte[length];
            for (int i = 0; i < length; i++)
            {
                arr1[i] = Convert.ToByte(ulaz.Substring(3 * i, 2), 16);
            }
            return arr1;
        }






        public static byte[] sifrujECB(string ulaznaRec, string kljuc)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md.ComputeHash(utf8.GetBytes(kljuc));
            tDES.Mode = CipherMode.ECB; //valjda moze ovako
            tDES.Padding = PaddingMode.Zeros; //ovo nznm sta je
            ICryptoTransform trans = tDES.CreateEncryptor();
            byte[] sifrovano = trans.TransformFinalBlock(utf8.GetBytes(ulaznaRec), 0, utf8.GetBytes(ulaznaRec).Length);
            
            
            return sifrovano;
        }

        //dekripcija
        public static string desifrujECB(byte[] sifrovana, string kljuc)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md.ComputeHash(utf8.GetBytes(kljuc));
            tDES.Mode = CipherMode.ECB; //valjda moze ovako
            tDES.Padding = PaddingMode.Zeros; //ovo nznm sta je
            ICryptoTransform trans = tDES.CreateDecryptor();
            string desifrovano = utf8.GetString(trans.TransformFinalBlock(sifrovana, 0, sifrovana.Length));
            return desifrovano;
        }



        public static User sifrujUsera(User u)
        {
            User sifrovan = new User();
            if (String.IsNullOrEmpty(u.Username))
            {
                sifrovan.Username = BitConverter.ToString(Sifrovanje.sifrujCBC(u.Username, "kljuc"));
            }
            if (String.IsNullOrEmpty(u.Password))
            {
                sifrovan.Password = BitConverter.ToString(Sifrovanje.sifrujCBC(u.Password, "kljuc"));
            }
            if (String.IsNullOrEmpty(u.Uloga))
            {
                sifrovan.Uloga = BitConverter.ToString(Sifrovanje.sifrujCBC(u.Uloga, "kljuc"));
            }
            return sifrovan;
        }

        public static Racun sifrujRacun(Racun r)
        {
            Racun sifrovan = new Racun();
            if (String.IsNullOrEmpty(r.BrojRacuna))
            {
                sifrovan.BrojRacuna = BitConverter.ToString(Sifrovanje.sifrujCBC(r.BrojRacuna, "kljuc"));
            }
            if (String.IsNullOrEmpty(r.Username))
            {
                sifrovan.Username = BitConverter.ToString(Sifrovanje.sifrujCBC(r.Username, "kljuc"));
            }
            if (String.IsNullOrEmpty(r.StanjeRacuna))
            {
                sifrovan.StanjeRacuna = BitConverter.ToString(Sifrovanje.sifrujCBC(r.StanjeRacuna, "kljuc"));
            }
            if (String.IsNullOrEmpty(r.Operater))
            {
                sifrovan.Operater = BitConverter.ToString(Sifrovanje.sifrujCBC(r.Operater, "kljuc"));
            }
            if (String.IsNullOrEmpty(r.TipRacuna))
            {
                sifrovan.TipRacuna = BitConverter.ToString(Sifrovanje.sifrujCBC(r.TipRacuna, "kljuc"));
            }
            return sifrovan;
        }

        public static User desifrujUsera(User u)
        {
            User desifrovanKorisnik = new User();
            if (String.IsNullOrEmpty(u.Username))
            {
                desifrovanKorisnik.Username = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(u.Username), "kljuc");
            }
            if (String.IsNullOrEmpty(u.Password))
            {
                desifrovanKorisnik.Password = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(u.Password), "kljuc");
            }
            if (String.IsNullOrEmpty(u.Uloga))
            {
                desifrovanKorisnik.Uloga = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(u.Uloga), "kljuc");
            }
            return desifrovanKorisnik;
        }

        public static Racun desifrujRacun(Racun r)
        {
            Racun desifrovanRacun = new Racun();
            if (String.IsNullOrEmpty(r.BrojRacuna))
            {
                desifrovanRacun.BrojRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.BrojRacuna), "kljuc");
            }
            if (String.IsNullOrEmpty(r.Username))
            {
                desifrovanRacun.Username = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Username), "kljuc");
            }
            if (String.IsNullOrEmpty(r.StanjeRacuna))
            {
                desifrovanRacun.StanjeRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.StanjeRacuna), "kljuc");
            }
            if (String.IsNullOrEmpty(r.TipRacuna))
            {
                desifrovanRacun.TipRacuna = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.TipRacuna), "kljuc");
            }
            if (String.IsNullOrEmpty(r.Operater))
            {
                desifrovanRacun.Operater = Sifrovanje.desifrujCBC(Sifrovanje.spremiZaDesifrovanje(r.Operater), "kljuc");
            }
            return desifrovanRacun;
        }
    }
}
