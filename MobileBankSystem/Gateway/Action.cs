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
    public class Action
    {
        //                        user /  his action
        private static Dictionary<string, ActionSet> actions = new Dictionary<string, ActionSet>();
        public static int attemptsLimit;
        public static DateTime time;
        public bool actionOccuered(string user, string action, DateTime time)
        {
            if (!actions.ContainsKey(user))
            {
                ActionInfo a = new ActionInfo(time);
                ActionSet set = new ActionSet(action, a);
                actions.Add(user, set);
                return false;
            }
            else
            {
                return actions[user].newAction(action, time);
            }
        }

        public static void ReadXml()
        {
            XmlTextReader reader = new XmlTextReader(Environment.CurrentDirectory + "\\korisnici.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if(reader.Name == "pokusaji")
                    {
                        attemptsLimit = Int32.Parse(reader.Value);
                    }else if(reader.Name == "vreme")
                    {
                        time = new DateTime();
                        string t = reader.Value;
                        string[] temp = t.Split(':');
                        time.AddHours(Double.Parse(temp[0]));
                        time.AddMinutes(Double.Parse(temp[1]));
                        time.AddSeconds(Double.Parse(temp[2]));
                    }
                }
            }
        }
    }

    class ActionSet
    {
        //              ime akcije    info
        private Dictionary<string, ActionInfo> set = new Dictionary<string, ActionInfo>();

        public ActionSet(string actionName, ActionInfo ac)
        {
            set.Add(actionName, ac);
        }
        public bool newAction(string action, DateTime time)
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
    }

    class ActionInfo
    {
        int numAction;
        Queue<DateTime> actionTime = new Queue<DateTime>(Action.attemptsLimit);

        //public ActionContext(string user, string action, DateTime time)
        public ActionInfo(DateTime time)
        {
            //this.user = user;
            //this.action = action;
            numAction = 0;
            Push(time);
        }

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

        void Refresh(DateTime time)
        {
            if (numAction > 0)
            {
                if (Action.time.Ticks < time.Subtract(actionTime.First()).Ticks)
                {
                    Pop();
                    Refresh(time);
                }
            }
        }

        void Reset(DateTime time)
        {
            numAction = 0;
            actionTime = new Queue<DateTime>(Action.attemptsLimit);
            Push(time);
        }

        void Push(DateTime time)
        {
            numAction++;
            actionTime.Enqueue(time);
        }

        void Pop()
        {
            numAction--;
            actionTime.Dequeue();
        }
    }
}
