using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace Starter
{
    public class CommandProcesser
    {
        private string command = "";

        private XDocument doc;

        public void Process(string command)
        {
            this.command = Regex.Replace(command, @"^ *", "");
            this.command = Regex.Replace(command, @" *$", "");
            this.command = Regex.Replace(command, @" {2,}", " ");
            string commandType = GetCommandType(command);

            switch (commandType)
            {
                case "start":
                    Start();
                    break;
                case "shutdown":
                    Shutdown();
                    break;
                case "sleep":
                    Shutdown();
                    break;
                case "changeip":
                    ChangeIP();
                    break;
                default: break;
            }
        }

        private void Start()
        {
            string filePath = GetCommandParameter(command);
            if (!File.Exists(filePath))
            {
                form_main.mainForm.ShowMessage("\"" + command + "\" 执行失败");
                return;
            }

            form_main.mainForm.configManager.RecordCommand("start " + Path.GetFileNameWithoutExtension(filePath), filePath);

            string cmdCommand = "start \"\" \"" + filePath + "\"";

            CmdProcess(cmdCommand);

            form_main.mainForm.ShowMessage("\"" + command + "\" 执行成功");

            form_main.mainForm.ClearCommand();

            form_main.mainForm.SetVisible(false);
        }

        private void Shutdown()
        {
            string times = GetCommandParameter(command);
            float subTime = 0;
            string action = GetCommandType(command) == "sleep" ? "-h" : "-s";

            if (times == "s")
                CmdProcess("shutdown -a");
            else if (times == "" || action == "-h")
                CmdProcess("shutdown " + action);
            else if (times == Regex.Match(times, @"(\d*[hms])*").Value)
            {
                Hashtable table = new Hashtable();
                table.Add('h', 3600);
                table.Add('m', 60);
                table.Add('s', 1);

                foreach (Match match in Regex.Matches(times, @"\d*[hms]", RegexOptions.IgnoreCase))
                {
                    char timeScale = match.Value[match.Value.Length - 1];
                    if (timeScale < 97)
                        timeScale += (char)32;
                    subTime += (int)table[timeScale] * int.Parse(Regex.Match(match.Value, @"^\d*").Value);
                    table[timeScale] = 0;
                }
                CmdProcess("shutdown -s -t " + subTime.ToString());
            }
            else
            {
                form_main.mainForm.ShowMessage("\"" + command + "\" 执行失败");
                return;
            }

            form_main.mainForm.ShowMessage("\"" + command + "\" 执行成功");
            form_main.mainForm.configManager.RecordCommand(command, "");
            form_main.mainForm.ClearCommand();
        }

        private void ChangeIP()
        {
            string cmdCommand = "netsh interface ip set address \"以太网\" ";
            string parameter = GetCommandParameter(command);
            int cmdCommandLength = cmdCommand.Length;

            if (Regex.Match(parameter, @"(\d{1,3}(\.\d{1,3}){3} ){2}(\d{1,3}(\.\d{1,3}){3})").Value == parameter)
                cmdCommand += "static " + parameter;
            else if (parameter == "auto")
                cmdCommand += "dhcp";
            else if (parameter == "static")
            {
                cmdCommand += "static";
                foreach (XElement temp in XDocument.Load(Application.StartupPath + "\\configure.xml").Root.Element("ipconfig").Elements())
                    cmdCommand += " " + temp.Value;
            }
            else
            {
                form_main.mainForm.ShowMessage("\"" + command + "\" 执行失败");
                return;
            }

            form_main.mainForm.configManager.RecordCommand(command, "");
            CmdProcess(cmdCommand);
            form_main.mainForm.ShowMessage("\"" + command + "\" 执行成功");
            form_main.mainForm.ClearCommand();
        }

        private void CmdProcess(string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(command + "&exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();
            p.Close();
        }

        private int XElementCount(XElement e)
        {
            int i = 0;
            foreach (var temp in e.Elements())
                i++;
            return i;
        }

        private string GetCommandType(string command)
        {
            return Regex.Match(command, @"^.+?( |$)").Value.Replace(" ", "");
        }

        private string GetCommandParameter(string command)
        {
            string result = Regex.Match(command, @" .*$").Value;
            if (result.Length > 0)
                result = result.Remove(0, 1);

            return result;
        }
    }
}
