using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Starter
{
    public class ConfigManager
    {
        private string configPath = Application.StartupPath + "\\configure.xml";

        private XDocument doc;

        public ConfigManager()
        {
            if (File.Exists(configPath))
            {
                doc = XDocument.Load(configPath);
                form_main.mainForm.config = doc;
            }
            else
                CreateConfigFile();
        }

        public void Save()
        {
            doc.Save(configPath);
        }

        public void RecordCommand(string name, string value)
        {
            foreach (XElement temp in doc.Root.Element("pre-commands").Elements())
                if (temp.Element("name").Value == name && temp.Element("value").Value == value)
                    temp.Remove();

            XElement command = new XElement("command");
            command.Add(new XElement("name", name));
            command.Add(new XElement("value", value));
            doc.Root.Element("pre-commands").AddFirst(command);

            int recordNumber = doc.Root.Element("record-number").Value == "" ? 0 : int.Parse(doc.Root.Element("record-number").Value);
            int childNumber = ChildNumber(doc.Root.Element("pre-commands"));
            if (childNumber > recordNumber)
                for(int i = 0; i <childNumber-recordNumber; i++)
                    doc.Root.Element("pre-commands").LastNode.Remove();

            foreach (XElement temp in doc.Root.Element("commands").Elements())
                if (temp.Element("name").Value == name && temp.Element("value").Value == value)
                    return;
            doc.Root.Element("commands").Add(command);
        }

        public void AddDirectory(string path)
        {
            doc.Root.Element("directories").Add(new XElement("directory", path));
            RefreshFileCommands();
        }

        public void RefreshFileCommands()
        {
            List<string> curfiles = new List<string>();
            foreach (XElement temp in doc.Root.Element("directories").Elements())
                curfiles.AddRange(GetAllFiles(temp.Value));
            foreach (XElement temp in doc.Root.Element("commands").Elements())
                if (GetCommandType(temp.Element("name").Value) == "start")
                    if (curfiles.IndexOf(temp.Element("value").Value) == -1)
                        temp.Remove();
                    else
                        curfiles.Remove(temp.Element("value").Value);
            foreach (string temp in curfiles)
            {
                XElement command = new XElement("command");
                command.Add(new XElement("name", "start " + Path.GetFileNameWithoutExtension(temp)));
                command.Add(new XElement("value", temp));
                doc.Root.Element("commands").Add(command);
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

        private void CreateConfigFile()
        {
            doc = new XDocument();
            doc.Add(new XElement("root"));
            doc.Root.Add(new XElement("commands"));
            doc.Root.Add(new XElement("pre-commands"));
            doc.Root.Add(new XElement("record-number"));
            doc.Root.Add(new XElement("directories"));
            XElement ipconfig = new XElement("ipconfig");
            ipconfig.Add(new XElement("address", "0.0.0.0"));
            ipconfig.Add(new XElement("mask", "0.0.0.0"));
            ipconfig.Add(new XElement("gateway", "0.0.0.0"));
            doc.Root.Add(ipconfig);
            doc.Save(configPath);
            form_main.mainForm.config = doc;
        }

        private string GetCommandType(string command)
        {
            return Regex.Match(command, @"^.+?( |$)").Value.Replace(" ", "");
        }

        private int ChildNumber(XElement e)
        {
            int i = 0;
            foreach (var temp in e.Elements())
                i++;
            return i;
        }
    }
}
