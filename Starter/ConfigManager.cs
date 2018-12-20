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

        public void RecordCommand(Command command)
        {
            XElement newPreCommand = new XElement("command");
            newPreCommand.Add(new XElement("name", command.Name));
            newPreCommand.Add(new XElement("param", command.Param));
            newPreCommand.Add(new XElement("date", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff")));
            XElement curNode;

            if (command.Type == "start")
            {
                curNode = doc.Root.Element("pre-commands").Element("start");
                foreach (XElement temp in curNode.Elements().Where(x => CommandEquals(x, command)))
                    temp.Remove();//若已在已使用命令中存在，则移除它

                curNode.Add(newPreCommand);
            }
            else
            {
                curNode = doc.Root.Element("pre-commands").Element("default");
                foreach (XElement temp in curNode.Elements().Where(x => CommandEquals(x, command)))
                    temp.Remove();

                curNode.Add(newPreCommand);

                int recordNumber = doc.Root.Element("record-number").Value == "" ? 0 : int.Parse(doc.Root.Element("record-number").Value);
                int childNumber = ChildNumber(curNode);
                if (childNumber > recordNumber)
                    for (int i = 0; i < childNumber - recordNumber; i++)
                        curNode.LastNode.Remove();//如果已使用命令大于上限，删除多出的命令
            }
        }

        private bool CommandEquals(XElement command, Command c)
        {
            return command.Element("name").Value == c.Name && command.Element("param").Value == c.Param;
        }

        public void AddDirectory(string path)
        {
            doc.Root.Element("directories").Add(new XElement("directory", path));
            form_main.mainForm.selector.RefreshStartFiles();
        }

        private void CreateConfigFile()
        {
            doc = new XDocument();
            doc.Add(new XElement("root"));
            XElement temp = new XElement("pre-commands");
            temp.Add(new XElement("default"));
            temp.Add(new XElement("start"));
            doc.Root.Add(temp);
            doc.Root.Add(new XElement("record-number"));
            doc.Root.Add(new XElement("directories"));
            temp = new XElement("ipconfig");
            temp.Add(new XElement("address", "0.0.0.0"));
            temp.Add(new XElement("mask", "0.0.0.0"));
            temp.Add(new XElement("gateway", "0.0.0.0"));
            doc.Root.Add(temp);
            doc.Save(configPath);
            form_main.mainForm.config = doc;
        }

        private int ChildNumber(XElement e)
        {
            int i = 0;
            foreach (var temp in e.Elements())
                i++;
            return i;
        }

        public void ReloadConfig()
        {
            XElement preCommands = doc.Root.Element("pre-commands");
            doc = XDocument.Load(configPath);
            doc.Root.Element("pre-commands").Remove();
            doc.Root.Add(preCommands);
            Save();
            form_main.mainForm.RefreshCommand();
        }

        public void OpenConfig()
        {
            if (File.Exists(configPath))
                form_main.mainForm.processer.CmdProcess("start \"\" \"" + configPath + "\"");
            else
                form_main.mainForm.ShowMessage("配置文件打开失败");
        }
    }
}
