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

        private List<Command> startCommands = new List<Command>();

        public CommandSelector(ListView listview)
        {
            target = listview;
            Initialize();
        }

        public void Initialize()
        {
            RefreshStartFiles();
            CommandChange("");
        }

        public void CommandChange(string command)
        {
            List<Command> selectedCommands = new List<Command>();
            List<Command> preCommands = new List<Command>();

            string pattern = @".*";
            foreach (char temp in command)
                pattern += temp + @".*";

            foreach (XElement temp1 in form_main.mainForm.config.Root.Element("pre-commands").Elements())
                foreach (XElement temp in temp1.Elements())
                    if (Regex.Match(temp.Element("name").Value, pattern, RegexOptions.IgnoreCase).Value == temp.Element("name").Value)
                    {
                        Command pre = new Command(temp.Element("name").Value, temp.Element("param").Value);
                        pre.Date = DateTime.ParseExact(temp.Element("date").Value,"yyyy-MM-dd-HH-mm-ss-ffff", System.Globalization.CultureInfo.CurrentCulture);
                        preCommands.Add(pre);
                    }//添加已使用命令

            selectedCommands.AddRange(preCommands);
            startCommands.ForEach(x=> 
            {
                if (selectedCommands.IndexOf(x) == -1 && Regex.Match(x.Name, pattern, RegexOptions.IgnoreCase).Value == x.Name)
                    selectedCommands.Add(x);
            });//不重复地添加启动命令

            foreach(XElement temp in form_main.mainForm.config.Root.Element("forbid-commands").Elements())
            {
                Command temp1 = new Command(temp.Element("name").Value, temp.Element("param").Value);
                if (selectedCommands.IndexOf(temp1) != -1)
                    selectedCommands.Remove(temp1);
            }//删除禁止的命令

            if (command == "")
                pattern = @".*";
            else
                pattern = @"[" + command + @"]";
            selectedCommands.Sort((left,right)=> 
            {
                bool leftE = preCommands.IndexOf(left)==-1?false:true;
                bool rightE = preCommands.IndexOf(right) == -1 ? false : true;

                if (leftE&& !rightE)
                    return -1;
                else if (!leftE&& rightE)
                    return 1;
                else if (leftE&& rightE)
                    return -(int)(left.Date-right.Date).TotalMilliseconds;
                else
                {
                    int leftL = Regex.Replace(left.Name, pattern, "", RegexOptions.IgnoreCase).Length;
                    int rightL = Regex.Replace(right.Name, pattern, "", RegexOptions.IgnoreCase).Length;

                    if (leftL > rightL)
                        return 1;
                    else if (leftL < rightL)
                        return -1;
                    else
                        return 0;
                }
            });

            target.Items.Clear();
            selectedCommands.ForEach(x=> target.Items.Add(new ListViewItem(new string[] { x.Name,x.Param})));
        }

        public void RefreshStartFiles()
        {
            startCommands.Clear();
            foreach (XElement temp in form_main.mainForm.config.Root.Element("directories").Elements())
            {
                List<string> files = GetAllFiles(temp.Value);
                files.ForEach(x => startCommands.Add(new Command("start " + Path.GetFileNameWithoutExtension(x), x)));
            }
            form_main.mainForm.RefreshCommand();
        }

        private List<string> GetAllFiles(string path)
        {
            List<string> files = new List<string>();

            if (!Directory.Exists(path))
                return files;

            files.AddRange(Directory.GetFiles(path));

            foreach (string temp in Directory.GetDirectories(path))
                files.AddRange(GetAllFiles(temp));

            return files;
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
