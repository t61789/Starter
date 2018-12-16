namespace Starter
{
    partial class form_main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_main));
            this.text_console = new System.Windows.Forms.TextBox();
            this.button_confirm = new System.Windows.Forms.Button();
            this.panel_input = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.list_commands = new System.Windows.Forms.ListView();
            this.column_command = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_addDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.更新目录文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon_main = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip_notify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.stripMenuItem_show = new System.Windows.Forms.ToolStripMenuItem();
            this.stripMenuItem_close = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_input.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip_notify.SuspendLayout();
            this.SuspendLayout();
            // 
            // text_console
            // 
            resources.ApplyResources(this.text_console, "text_console");
            this.text_console.Name = "text_console";
            this.text_console.TextChanged += new System.EventHandler(this.text_console_TextChanged);
            this.text_console.KeyDown += new System.Windows.Forms.KeyEventHandler(this.text_console_KeyDown);
            // 
            // button_confirm
            // 
            resources.ApplyResources(this.button_confirm, "button_confirm");
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // panel_input
            // 
            resources.ApplyResources(this.panel_input, "panel_input");
            this.panel_input.Controls.Add(this.text_console);
            this.panel_input.Controls.Add(this.button_confirm);
            this.panel_input.Name = "panel_input";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.list_commands);
            this.panel1.Name = "panel1";
            // 
            // list_commands
            // 
            resources.ApplyResources(this.list_commands, "list_commands");
            this.list_commands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_command,
            this.column_value});
            this.list_commands.FullRowSelect = true;
            this.list_commands.Name = "list_commands";
            this.list_commands.UseCompatibleStateImageBehavior = false;
            this.list_commands.View = System.Windows.Forms.View.Details;
            this.list_commands.KeyDown += new System.Windows.Forms.KeyEventHandler(this.list_commands_KeyDown);
            // 
            // column_command
            // 
            resources.ApplyResources(this.column_command, "column_command");
            // 
            // column_value
            // 
            resources.ApplyResources(this.column_value, "column_value");
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_addDirectory,
            this.更新目录文件ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            resources.ApplyResources(this.文件ToolStripMenuItem, "文件ToolStripMenuItem");
            // 
            // menuItem_addDirectory
            // 
            this.menuItem_addDirectory.Name = "menuItem_addDirectory";
            resources.ApplyResources(this.menuItem_addDirectory, "menuItem_addDirectory");
            this.menuItem_addDirectory.Click += new System.EventHandler(this.menuItem_addDirectory_Click);
            // 
            // 更新目录文件ToolStripMenuItem
            // 
            this.更新目录文件ToolStripMenuItem.Name = "更新目录文件ToolStripMenuItem";
            resources.ApplyResources(this.更新目录文件ToolStripMenuItem, "更新目录文件ToolStripMenuItem");
            // 
            // notifyIcon_main
            // 
            this.notifyIcon_main.ContextMenuStrip = this.contextMenuStrip_notify;
            resources.ApplyResources(this.notifyIcon_main, "notifyIcon_main");
            this.notifyIcon_main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_main_MouseDoubleClick);
            // 
            // contextMenuStrip_notify
            // 
            this.contextMenuStrip_notify.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_notify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripMenuItem_show,
            this.stripMenuItem_close});
            this.contextMenuStrip_notify.Name = "contextMenuStrip_notify";
            resources.ApplyResources(this.contextMenuStrip_notify, "contextMenuStrip_notify");
            // 
            // stripMenuItem_show
            // 
            this.stripMenuItem_show.Name = "stripMenuItem_show";
            resources.ApplyResources(this.stripMenuItem_show, "stripMenuItem_show");
            this.stripMenuItem_show.Click += new System.EventHandler(this.stripMenuItem_show_Click);
            // 
            // stripMenuItem_close
            // 
            this.stripMenuItem_close.Name = "stripMenuItem_close";
            resources.ApplyResources(this.stripMenuItem_close, "stripMenuItem_close");
            this.stripMenuItem_close.Click += new System.EventHandler(this.stripMenuItem_close_Click);
            // 
            // form_main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_input);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "form_main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.form_main_FormClosed);
            this.panel_input.ResumeLayout(false);
            this.panel_input.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip_notify.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_console;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.Panel panel_input;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView list_commands;
        private System.Windows.Forms.ColumnHeader column_command;
        private System.Windows.Forms.ColumnHeader column_value;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem_addDirectory;
        private System.Windows.Forms.NotifyIcon notifyIcon_main;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_notify;
        private System.Windows.Forms.ToolStripMenuItem stripMenuItem_close;
        private System.Windows.Forms.ToolStripMenuItem stripMenuItem_show;
        private System.Windows.Forms.ToolStripMenuItem 更新目录文件ToolStripMenuItem;
    }
}

