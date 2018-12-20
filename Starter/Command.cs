using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Starter
{
    public class Command
    {
        private string name;
        private string type;
        private string arg;
        private string param="";
        private DateTime date;

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public string Type
        {
            get { return type; }
            private set { type = value; }
        }

        public string Arg
        {
            get { return arg; }
            private set { arg = value; }
        }

        public string Param
        {
            get { return param; }
            private set { param = value; }
        }

        public DateTime Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        public Command(string name)
        {
            Name = name;
            Type = Regex.Match(name, @"^.+?( |$)").Value.Replace(" ", "");
            Arg = Regex.Match(name, @" .*$").Value;
            if (Arg.Length > 0)
                Arg = Arg.Remove(0, 1);
        }

        public Command(string name, string param) : this(name)
        {
            Param = param;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Command)) return false;
            return (obj as Command).Name == Name && (obj as Command).Param == Param;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public static bool operator ==(Command l, Command r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Command l, Command r)
        {
            return !(l == r);
        }
    }
}
