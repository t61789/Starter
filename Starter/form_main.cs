using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starter
{
    public partial class form_main : Form
    {
        private CommandProcesser processer;

        private CommandSelector selector;

        private static form_main mainForm;

        public form_main()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            mainForm = this;
            processer = new CommandProcesser(this);
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
            selector.AddDirectory(folder.SelectedPath);
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

        public void ClearCommand()
        {
            text_console.Text = "";
        }

        public void ShowMessage(string message)
        {
            Text = "Starter\\" + message;
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

        public void SetVisible(bool arg)
        {
            Visible = arg;
        }

        private void notifyIcon_main_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Visible = Visible ? false : true;
        }
    }
}
