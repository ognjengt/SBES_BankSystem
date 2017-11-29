using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Gateway
{
    //klasa koja vodi evidenciju svih klijenata i svih njegovih akcija
    public static class Action
    {
        //                         user /  his action
        private static Dictionary<string, ActionSet> actions = new Dictionary<string, ActionSet>(); //key: user  value: skup akcija i njihovih info
        public static int attemptsLimit;    //maximalan broj pokusaja
        public static TimeSpan time;        //vremensko ogranicenje 

        static Action()
        {
            ReadXml("Config.xml");
        }

        //ukoliko se pojavi neka akcija treba pobrisati evidenciju svih ostalih
        //akcija jer se u zadatku trazi da akcije budu uzastopne.
        // tj. cim se pojavila nova akcija znaci da ta uzastopnost prestaje i treba
        //resetovati evidenciju za ostale akcije.
        private static void Reset(string user, string action) {
            actions[user].Reset(action);
        }

        //doslo je do odredjene akcije treba je evidentirati
        public static bool ActionOccuered(string user, string action, DateTime dt)
        {
            Reset(user, action);
            if (!actions.ContainsKey(user))
            {
                ActionInfo a = new ActionInfo(dt);
                ActionSet set = new ActionSet(action, a);
                actions.Add(user, set);
                return false;
            }
            else
            {
                return actions[user].Update(action, dt);
            }
        }

        //citam Config.xml da bi dobavio podatke od maksimalnom broju pokusaja 
        //i vremenskom ogranicenju
        private static void ReadXml(string filename)
        {
            bool e = File.Exists(Environment.CurrentDirectory + "\\" + filename);
            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.CurrentDirectory + "\\" + filename);

            XmlNode root = doc.FirstChild;
            //Display the contents of the child nodes.
            if (root.HasChildNodes)
            {
                attemptsLimit = Int32.Parse(root.ChildNodes[0].InnerText);
                string t = root.ChildNodes[1].InnerText;
                string[] x = t.Split(':');
                time = new TimeSpan(Int32.Parse(x[0]), Int32.Parse(x[1]), Int32.Parse(x[2]));
                
            } 
        }
    }

    //klasa koja vodi evidenciju svih pozvanih akcija
    //odredjenog klijenta
    class ActionSet
    {
        //              ime akcije    info
        private Dictionary<string, ActionInfo> set = new Dictionary<string, ActionInfo>();  // key: ime akcije    value: info o akciji

        public ActionSet(string actionName, ActionInfo ac)
        {
            set.Add(actionName, ac);
        }

        //azuriram stanje za odredjenu akciju
        public bool Update(string action, DateTime time)
        {
            if (!set.ContainsKey(action))
            {
                set.Add(action, new ActionInfo(time));
                return false;
            }
            else
            {
                return set[action].UpdateActionInfo(time);
            }
        }

        //brisem stanja svih akcija osim ove prosledjene
        //jer se ona poslednja inicirala i treba paziti o 
        //njenom uzastopnom pozivanju
        public void Reset(string action)
        {
            foreach(KeyValuePair<string, ActionInfo> kvp in set)
            {
                if(kvp.Key != action)
                {
                    kvp.Value.Reset();
                }
            }
        }
    }


    //klasa koja odrzava informacije o vremenu iniciranja odredjene akcije
    //kao i o broju uzastopnih iniciranja
    class ActionInfo
    {
        int numAction;  //br uzastopnih pokusaja
        Queue<DateTime> actionTime = new Queue<DateTime>(Action.attemptsLimit); //queue sa vremenima iniciranja akcija

        //public ActionContext(string user, string action, DateTime time)
        public ActionInfo(DateTime time)
        {
            //this.user = user;
            //this.action = action;
            numAction = 0;
            Push(time);
        }

        //azuriram vreme iniciranja akcije
        public bool UpdateActionInfo(DateTime time)
        {
            Refresh(time);
            Push(time);
            if (numAction == Action.attemptsLimit)
            {
                return true;
            }
            return false;

        }

        //cistim queue zabelezenih vremena
        public void Reset()
        {
            numAction = 0;
            actionTime = new Queue<DateTime>(Action.attemptsLimit);
        }

        //izbacujem iz queue zastarela vremena koja nisu od interesa
        void Refresh(DateTime time)
        {
            if (numAction > 0)
            {
                if (Action.time.TotalSeconds < time.Subtract(actionTime.First()).TotalSeconds)
                {
                    Pop();
                    Refresh(time);
                }
            }
        }

        //ubacujem vreme u queue i povecavam uzastopni poziv
        void Push(DateTime time)
        {
            numAction++;
            actionTime.Enqueue(time);
        }

        //izbacujem vreme i smanjujem uzastopni poziv
        void Pop()
        {
            numAction--;
            actionTime.Dequeue();
        }
    }
}
