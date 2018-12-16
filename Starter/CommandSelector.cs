using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Starter
{
    public class CommandSelector
    {
        private ListView target;

        public CommandSelector(ListView listview)
        {
            target = listview;
            Initialize();
        }

        public void Initialize()
        {
            CommandChange("");
        }

        public void CommandChange(string command)
        {
            List<string[]> tableList = new List<string[]>();

            string pattern = @".*";
            foreach (char temp in command)
                pattern += temp + @".*";

            foreach (XElement temp in form_main.mainForm.config.Root.Element("commands").Elements())
                if (Regex.Match(temp.Element("name").Value, pattern, RegexOptions.IgnoreCase).Value == temp.Element("name").Value)
                    tableList.Add(new string[] { temp.Element("name").Value, temp.Element("value").Value });
            
            if (command == "")
                pattern = @".*";
            else
                pattern = @"[" + command + @"]";
            tableList.Sort((left, right) =>
            {
                XNode leftE = IsPreCommand(left[0], left[1]);
                XNode rightE = IsPreCommand(right[0], right[1]);

                if (leftE != null && rightE == null)
                    return -1;
                else if (leftE == null && rightE != null)
                    return 1;
                else if (leftE != null && rightE != null)
                {
                    while(leftE.NextNode!=null)
                        if (leftE.NextNode == rightE)
                            return -1;
                        else
                            leftE = leftE.NextNode;
                    return 1;
                }
                else
                {
                    int leftL = Regex.Replace(left[0], pattern, "", RegexOptions.IgnoreCase).Length;
                    int rightL = Regex.Replace(right[0], pattern, "", RegexOptions.IgnoreCase).Length;

                    if (leftL > rightL)
                        return 1;
                    else if (leftL < rightL)
                        return -1;
                    else
                        return 0;
                }
            });

            target.Items.Clear();
            foreach (string[] temp in tableList)
                target.Items.Add(new ListViewItem(temp));
        }

        private XElement IsPreCommand(string name, string value)
        {
            foreach (XElement temp in form_main.mainForm.config.Root.Element("pre-commands").Elements())
                if (temp.Element("name").Value == name && temp.Element("value").Value == value)
                    return temp;
            return null;
        }
    }
}
