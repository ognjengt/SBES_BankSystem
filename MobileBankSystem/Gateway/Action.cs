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
    public static class Action
    {
        //                        user /  his action
        private static Dictionary<string, ActionSet> actions = new Dictionary<string, ActionSet>();
        public static int attemptsLimit;
        public static TimeSpan time;

        static Action()
        {
            ReadXml("Config.xml");
        }

        public static bool actionOccuered(string user, string action, DateTime dt)
        {
            if (!actions.ContainsKey(user))
            {
                ActionInfo a = new ActionInfo(dt);
                ActionSet set = new ActionSet(action, a);
                actions.Add(user, set);
                return false;
            }
            else
            {
                return actions[user].newAction(action, dt);
            }
        }

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
                if (Action.time.TotalSeconds < time.Subtract(actionTime.First()).TotalSeconds)
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
