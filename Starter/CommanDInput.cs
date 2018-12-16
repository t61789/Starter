using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starter
{
    public class CommandInput
    {
        private TextBox input;
        private ListView commandList;

        public CommandInput(TextBox input, ListView commandList)
        {
            this.input = input;
            this.commandList = commandList;
        }

        public void TextChange(object sender, EventArgs e)
        {
            form_main.mainForm.selector.CommandChange(input.Text);
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && input.Focused)
                form_main.mainForm.processer.Process(input.Text);

            if (commandList.Items.Count != 0 && (e.KeyCode == Keys.Down || e.KeyCode == Keys.ControlKey))
            {
                if (!commandList.Focused)
                {
                    commandList.Focus();
                    commandList.Items[0].Selected = true;
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                input.Text = "";
                form_main.mainForm.Visible = false;
            }
        }
    }
}
