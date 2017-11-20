using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class Sifrovanje
    {

        
        public byte[] sifrujCBC(string ulaznaRec, string kljuc)
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
            string ispis = BitConverter.ToString(sifrovano);
            Console.WriteLine("Sifrovan tekst: " + ispis);
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
        public string desifrujCBC(byte[] sifrovana, string kljuc)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md.ComputeHash(utf8.GetBytes(kljuc));
            tDES.Mode = CipherMode.CBC; //valjda moze ovako
            tDES.Padding = PaddingMode.Zeros;

            byte[] niz = new byte[tDES.BlockSize / 8];
            byte[] message = new byte[sifrovana.Length - niz.Length];
            for (int i = 0; i < sifrovana.Length; i++)
            {
                if (i < tDES.BlockSize / 8)
                {
                    niz[i] = sifrovana[i];
                }
                else
                {
                    message[i - niz.Length] = sifrovana[i];
                }
            }

            tDES.IV = niz;
            ICryptoTransform trans = tDES.CreateDecryptor();
            string desifrovano = utf8.GetString(trans.TransformFinalBlock(message, 0, message.Length));
            return desifrovano;
        }




        public byte[] sifrujECB(string ulaznaRec, string kljuc)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md.ComputeHash(utf8.GetBytes(kljuc));
            tDES.Mode = CipherMode.ECB; //valjda moze ovako
            tDES.Padding = PaddingMode.PKCS7; //ovo nznm sta je
            ICryptoTransform trans = tDES.CreateEncryptor();
            byte[] sifrovano = trans.TransformFinalBlock(utf8.GetBytes(ulaznaRec), 0, utf8.GetBytes(ulaznaRec).Length);
            string ispis = BitConverter.ToString(sifrovano);
            Console.WriteLine("Sifrovan tekst: " + ispis);
            return sifrovano;
        }

        //dekripcija
        public string desifrujECB(byte[] sifrovana, string kljuc)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            UTF8Encoding utf8 = new UTF8Encoding();
            TripleDESCryptoServiceProvider tDES = new TripleDESCryptoServiceProvider();
            tDES.Key = md.ComputeHash(utf8.GetBytes(kljuc));
            tDES.Mode = CipherMode.ECB; //valjda moze ovako
            tDES.Padding = PaddingMode.PKCS7; //ovo nznm sta je
            ICryptoTransform trans = tDES.CreateDecryptor();
            string desifrovano = utf8.GetString(trans.TransformFinalBlock(sifrovana, 0, sifrovana.Length));
            return desifrovano;
        }








    }
}
