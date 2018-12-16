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
        private XDocument doc;

        private ListView target;

        private List<string[]> commands = new List<string[]>();

        private List<string> directories = new List<string>();

        private string configurePath = Application.StartupPath + "\\configure.xml";

        public CommandSelector()
        {

        }

        public CommandSelector(ListView listview)
        {
            target = listview;
            Initialize();
        }

        public void Initialize()
        {
            if (!File.Exists(configurePath))
            {
                CreateConfigureFile();
                return;
            }
            target.Items.Clear();

            commands = new List<string[]>();

            doc = XDocument.Load(Application.StartupPath + "\\configure.xml");

            foreach (XElement temp in doc.Root.Element("predefine-commands").Elements())
                commands.Add(new string[] { temp.Element("name").Value, temp.Element("value").Value });

            foreach (string directory in doc.Root.Element("directories").Elements())
                foreach (string temp in GetAllFile(directory))
                    commands.Add(new string[] { "start " + Path.GetFileNameWithoutExtension(temp), temp });

            CommandChange("");
        }

        private string[] GetAllFile(string path)
        {
            List<string> files = new List<string>();

            if (!Directory.Exists(path))
                return files.ToArray();

            files.AddRange(Directory.GetFiles(path));

            foreach (string temp in Directory.GetDirectories(path))
                files.AddRange(GetAllFile(temp));

            return files.ToArray();
        }

        public void CommandChange(string command)
        {
            List<string[]> tableList = new List<string[]>();

            if (command == "")
                tableList.AddRange(GetPreCommands());

                string pattern = @".*";
            foreach (char temp in command)
                pattern += temp + @".*";

            foreach (string[] temp in commands)
                if (Regex.Match(temp[0], pattern, RegexOptions.IgnoreCase).Value == temp[0])
                    tableList.Add(temp);
            pattern = @"[" + command + @"]";

            if (command != "")
                tableList.Sort((left, right) =>
                {
                    int leftL = Regex.Replace(left[0], pattern, "", RegexOptions.IgnoreCase).Length;
                    int rightL = Regex.Replace(right[0], pattern, "", RegexOptions.IgnoreCase).Length;

                    if (leftL > rightL)
                        return 1;
                    else if (leftL < rightL)
                        return -1;
                    else
                        return 0;
                });

            target.Items.Clear();
            foreach (string[] temp in tableList)
                target.Items.Add(new ListViewItem(temp));
        }

        public void CreateConfigureFile()
        {
            XDocument newDoc = new XDocument();
            newDoc.Add(new XElement("root"));
            newDoc.Root.Add(new XElement("pre-commands"));
            newDoc.Root.Add(new XElement("predefine-commands"));
            newDoc.Root.Add(new XElement("directories"));
            XElement ipconfig = new XElement("ipconfig");
            ipconfig.Add(new XElement("address","0.0.0.0"));
            ipconfig.Add(new XElement("mask", "0.0.0.0"));
            ipconfig.Add(new XElement("gateway", "0.0.0.0"));
            newDoc.Root.Add(new XElement("record-number"));
            newDoc.Root.Add(ipconfig);

            newDoc.Save(configurePath);
            Initialize();
        }

        private List<string[]> GetPreCommands()
        {
            List<string[]> result = new List<string[]>();
            doc = XDocument.Load(configurePath);
            foreach (XElement temp in doc.Root.Element("pre-commands").Elements())
                result.Add(new string[] { temp.Element("name").Value, temp.Element("value").Value });

            return result;
        }

        public void AddDirectory(string path)
        {
            doc.Root.Element("directories").Add(new XElement("directory",path));
            doc.Save(configurePath);
            Initialize();
        }
    }
}
