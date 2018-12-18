using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Starter
{
    public partial class form_main : Form
    {
        public CommandProcesser processer;
        public CommandSelector selector;
        public ConfigManager configManager;
        public CommandInput input;
        public static form_main mainForm;
        public XDocument config;

        public form_main()
        {
            DetecteDuplicate();
            InitializeComponent();
            Initialize();
        }

        public void ClearCommand()
        {
            text_console.Text = "";
        }

        public void RefreshCommand()
        {
            selector.CommandChange(text_console.Text);
        }

        public void ShowMessage(string message)
        {
            Text = "Starter\\" + message;
        }

        public void SetVisible(bool arg)
        {
            Visible = arg;
        }

        private void DetecteDuplicate()
        {
            Application.AddMessageFilter(new MsgFilter(this));
            foreach (Process temp in Process.GetProcessesByName("Starter"))
                if (temp.Id != Process.GetCurrentProcess().Id)
                {
                    PostThreadMessage(temp.Threads[0].Id, 0x0400, (IntPtr)0, (IntPtr)0);
                    Environment.Exit(0);
                }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(int threadId, uint msg, IntPtr wParam, IntPtr lParam);

        private class MsgFilter : IMessageFilter
        {
            private form_main f;

            public MsgFilter(form_main f)
            {
                this.f = f;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 0x0400 && m.WParam == (IntPtr)0 && m.LParam == (IntPtr)0)
                {
                    f.Visible = true;
                    return true;
                }
                return false;
            }
        }

        private void Initialize()
        {
            mainForm = this;
            configManager = new ConfigManager();
            processer = new CommandProcesser();
            selector = new CommandSelector(list_commands);
            input = new CommandInput(text_console, list_commands);
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            processer.Process(text_console.Text);
        }

        private void text_console_KeyDown(object sender, KeyEventArgs e)
        {
            input.KeyDown(sender, e);
        }

        private void text_console_TextChanged(object sender, EventArgs e)
        {
            input.TextChange(sender, e);
        }

        private void menuItem_addDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            configManager.AddDirectory(folder.SelectedPath);
        }

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private void list_commands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && list_commands.SelectedItems.Count != 0)
            {
                if (list_commands.SelectedItems[0].SubItems[1].Text != "")
                    text_console.Text = "start " + list_commands.SelectedItems[0].SubItems[1].Text;
                else
                    text_console.Text = list_commands.SelectedItems[0].SubItems[0].Text;
                processer.Process(text_console.Text);
            }
            else if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Control)
            {
                text_console.Focus();
                keybd_event((byte)e.KeyCode, 0,0,0);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                text_console.Text = "";
                Visible = false;
            }
        }

        private void stripMenuItem_show_Click(object sender, EventArgs e)
        {
            Visible = true;
        }

        private void stripMenuItem_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon_main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Visible = Visible ? false : true;
        }

        private void form_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        private void form_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            configManager.Save();
        }

        private void form_main_Shown(object sender, EventArgs e)
        {
            text_console.Focus();
        }

        private void form_main_Enter(object sender, EventArgs e)
        {
            text_console.Focus();
        }
    }
}
