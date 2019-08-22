using System;
using System.Drawing;
using System.Windows.Forms;
using LogBook.Objects;

namespace LogBook
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.log4NetPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabLogController = new LogBook.Objects.OrderableTabControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(687, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newLogToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newLogToolStripMenuItem
            // 
            this.newLogToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.log4NetPageToolStripMenuItem,
            this.textPageToolStripMenuItem});
            this.newLogToolStripMenuItem.Name = "newLogToolStripMenuItem";
            this.newLogToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.newLogToolStripMenuItem.Text = "New Page";
            // 
            // log4NetPageToolStripMenuItem
            // 
            this.log4NetPageToolStripMenuItem.Name = "log4NetPageToolStripMenuItem";
            this.log4NetPageToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.log4NetPageToolStripMenuItem.Text = "Log4Net Page";
            this.log4NetPageToolStripMenuItem.Click += new System.EventHandler(this.log4NetPageToolStripMenuItem_Click);
            // 
            // textPageToolStripMenuItem
            // 
            this.textPageToolStripMenuItem.Name = "textPageToolStripMenuItem";
            this.textPageToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.textPageToolStripMenuItem.Text = "Text Page";
            this.textPageToolStripMenuItem.Click += new System.EventHandler(this.textPageToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tabLogController
            // 
            this.tabLogController.AllowDrop = true;
            this.tabLogController.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabLogController.Location = new System.Drawing.Point(0, 24);
            this.tabLogController.Name = "tabLogController";
            this.tabLogController.SelectedIndex = 0;
            this.tabLogController.Size = new System.Drawing.Size(687, 26);
            this.tabLogController.TabIndex = 3;
            this.tabLogController.Visible = false;
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(687, 549);
            this.Controls.Add(this.tabLogController);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(703, 588);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogBook";
            this.MdiChildActivate += new System.EventHandler(this.MainWindow_MdiChildActivate);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newLogToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem log4NetPageToolStripMenuItem;
        private ToolStripMenuItem textPageToolStripMenuItem;
        private OrderableTabControl tabLogController;
    }
}

