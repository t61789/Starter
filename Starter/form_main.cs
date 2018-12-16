using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Starter
{
    public partial class form_main : Form
    {
        public CommandProcesser processer;

        public CommandSelector selector;

        public ConfigManager configManager;

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
                if (m.Msg == 0x0400)
                {
                    f.Visible = true;
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
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            processer.Process(text_console.Text);
        }

        private void text_console_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && text_console.Focused)
                processer.Process(text_console.Text);

            if (list_commands.Items.Count != 0 && (e.KeyCode == Keys.Down || e.KeyCode == Keys.ControlKey))
            {
                if (!list_commands.Focused)
                {
                    list_commands.Focus();
                    list_commands.Items[0].Selected = true;
                }
            }

            if (e.KeyCode == Keys.Escape)
                Visible = false;
        }

        private void text_console_TextChanged(object sender, EventArgs e)
        {
            selector.CommandChange(text_console.Text);
        }

        private void menuItem_addDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowDialog();
            configManager.AddDirectory(folder.SelectedPath);
        }

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
            else if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down)
            {
                text_console.Focus();
            }
        }

        private void form_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
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

        private void form_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            configManager.Save();
        }
    }
}
