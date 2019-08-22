using System.Windows.Forms;

namespace LogBook.Objects.Forms
{
    partial class Page: Form
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
            foreach (Control control in Controls)
            {
                control.Dispose();
            }

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Page));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dockControlImageList = new System.Windows.Forms.ImageList(this.components);
            this.resultsDataGridView = new System.Windows.Forms.DataGridView();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.resultsDataSet = new System.Data.DataSet();
            this.mediaControlImages = new System.Windows.Forms.ImageList(this.components);
            this.nextRecordButton = new System.Windows.Forms.Button();
            this.playControlButton = new System.Windows.Forms.CheckBox();
            this.pageDockButton = new System.Windows.Forms.Button();
            this.resultsStatusStrip = new System.Windows.Forms.StatusStrip();
            this.resultsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.resultsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.resultsDisplayedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.firstRecordButton = new System.Windows.Forms.Button();
            this.lastRecordButton = new System.Windows.Forms.Button();
            this.pageControlPanel = new System.Windows.Forms.Panel();
            this.startPositionButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.loadingPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataSet)).BeginInit();
            this.resultsStatusStrip.SuspendLayout();
            this.pageControlPanel.SuspendLayout();
            this.loadingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockControlImageList
            // 
            this.dockControlImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("dockControlImageList.ImageStream")));
            this.dockControlImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.dockControlImageList.Images.SetKeyName(0, "icons8-lock-50.png");
            this.dockControlImageList.Images.SetKeyName(1, "icons8-padlock-50.png");
            // 
            // resultsDataGridView
            // 
            this.resultsDataGridView.AllowUserToAddRows = false;
            this.resultsDataGridView.AllowUserToDeleteRows = false;
            this.resultsDataGridView.AllowUserToResizeRows = false;
            this.resultsDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.resultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.resultsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.resultsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsDataGridView.Location = new System.Drawing.Point(0, 47);
            this.resultsDataGridView.Name = "resultsDataGridView";
            this.resultsDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.resultsDataGridView.Size = new System.Drawing.Size(627, 392);
            this.resultsDataGridView.TabIndex = 7;
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 75;
            // 
            // resultsDataSet
            // 
            this.resultsDataSet.DataSetName = "NewDataSet";
            // 
            // mediaControlImages
            // 
            this.mediaControlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mediaControlImages.ImageStream")));
            this.mediaControlImages.TransparentColor = System.Drawing.Color.Transparent;
            this.mediaControlImages.Images.SetKeyName(0, "icons8-first-50.png");
            this.mediaControlImages.Images.SetKeyName(1, "icons8-last-50.png");
            this.mediaControlImages.Images.SetKeyName(2, "icons8-next-50.png");
            this.mediaControlImages.Images.SetKeyName(3, "icons8-pause-squared-50.png");
            this.mediaControlImages.Images.SetKeyName(4, "icons8-right-button-50.png");
            this.mediaControlImages.Images.SetKeyName(5, "icons8-drop-down-50.png");
            this.mediaControlImages.Images.SetKeyName(6, "icons8-up-squared-50.png");
            // 
            // nextRecordButton
            // 
            this.nextRecordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextRecordButton.Enabled = false;
            this.nextRecordButton.ImageIndex = 4;
            this.nextRecordButton.ImageList = this.mediaControlImages;
            this.nextRecordButton.Location = new System.Drawing.Point(528, 3);
            this.nextRecordButton.Name = "nextRecordButton";
            this.nextRecordButton.Size = new System.Drawing.Size(45, 40);
            this.nextRecordButton.TabIndex = 11;
            this.nextRecordButton.UseVisualStyleBackColor = true;
            this.nextRecordButton.Click += new System.EventHandler(this.NextRecordButton_Click);
            // 
            // playControlButton
            // 
            this.playControlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.playControlButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.playControlButton.Enabled = false;
            this.playControlButton.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.playControlButton.ImageIndex = 3;
            this.playControlButton.ImageList = this.mediaControlImages;
            this.playControlButton.Location = new System.Drawing.Point(477, 3);
            this.playControlButton.Name = "playControlButton";
            this.playControlButton.Size = new System.Drawing.Size(45, 40);
            this.playControlButton.TabIndex = 10;
            this.playControlButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.playControlButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.playControlButton.UseVisualStyleBackColor = true;
            this.playControlButton.CheckedChanged += new System.EventHandler(this.PlayControlButton_CheckedChanged);
            // 
            // pageDockButton
            // 
            this.pageDockButton.ImageIndex = 1;
            this.pageDockButton.ImageList = this.dockControlImageList;
            this.pageDockButton.Location = new System.Drawing.Point(3, 3);
            this.pageDockButton.Name = "pageDockButton";
            this.pageDockButton.Size = new System.Drawing.Size(45, 40);
            this.pageDockButton.TabIndex = 1;
            this.pageDockButton.UseVisualStyleBackColor = true;
            this.pageDockButton.Click += new System.EventHandler(this.PageDockButton_Click);
            // 
            // resultsStatusStrip
            // 
            this.resultsStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resultsLabel,
            this.resultsProgressBar,
            this.resultsDisplayedLabel});
            this.resultsStatusStrip.Location = new System.Drawing.Point(0, 439);
            this.resultsStatusStrip.Name = "resultsStatusStrip";
            this.resultsStatusStrip.Size = new System.Drawing.Size(627, 22);
            this.resultsStatusStrip.TabIndex = 12;
            this.resultsStatusStrip.Text = "statusStrip1";
            // 
            // resultsLabel
            // 
            this.resultsLabel.BackColor = System.Drawing.SystemColors.Control;
            this.resultsLabel.Name = "resultsLabel";
            this.resultsLabel.Size = new System.Drawing.Size(59, 17);
            this.resultsLabel.Text = "Loading...";
            // 
            // resultsProgressBar
            // 
            this.resultsProgressBar.Name = "resultsProgressBar";
            this.resultsProgressBar.Size = new System.Drawing.Size(100, 16);
            this.resultsProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // resultsDisplayedLabel
            // 
            this.resultsDisplayedLabel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.resultsDisplayedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsDisplayedLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.resultsDisplayedLabel.Name = "resultsDisplayedLabel";
            this.resultsDisplayedLabel.Size = new System.Drawing.Size(118, 17);
            this.resultsDisplayedLabel.Text = "toolStripStatusLabel1";
            this.resultsDisplayedLabel.Visible = false;
            // 
            // firstRecordButton
            // 
            this.firstRecordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.firstRecordButton.Enabled = false;
            this.firstRecordButton.ImageIndex = 0;
            this.firstRecordButton.ImageList = this.mediaControlImages;
            this.firstRecordButton.Location = new System.Drawing.Point(426, 3);
            this.firstRecordButton.Name = "firstRecordButton";
            this.firstRecordButton.Size = new System.Drawing.Size(45, 40);
            this.firstRecordButton.TabIndex = 13;
            this.firstRecordButton.UseVisualStyleBackColor = true;
            this.firstRecordButton.Click += new System.EventHandler(this.firstRecordButton_Click);
            // 
            // lastRecordButton
            // 
            this.lastRecordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lastRecordButton.Enabled = false;
            this.lastRecordButton.ImageIndex = 1;
            this.lastRecordButton.ImageList = this.mediaControlImages;
            this.lastRecordButton.Location = new System.Drawing.Point(579, 3);
            this.lastRecordButton.Name = "lastRecordButton";
            this.lastRecordButton.Size = new System.Drawing.Size(45, 40);
            this.lastRecordButton.TabIndex = 14;
            this.lastRecordButton.UseVisualStyleBackColor = true;
            this.lastRecordButton.Click += new System.EventHandler(this.lastRecordButton_Click);
            // 
            // pageControlPanel
            // 
            this.pageControlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.pageControlPanel.Controls.Add(this.startPositionButton);
            this.pageControlPanel.Controls.Add(this.button1);
            this.pageControlPanel.Controls.Add(this.pageDockButton);
            this.pageControlPanel.Controls.Add(this.firstRecordButton);
            this.pageControlPanel.Controls.Add(this.lastRecordButton);
            this.pageControlPanel.Controls.Add(this.nextRecordButton);
            this.pageControlPanel.Controls.Add(this.playControlButton);
            this.pageControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageControlPanel.Location = new System.Drawing.Point(0, 0);
            this.pageControlPanel.Name = "pageControlPanel";
            this.pageControlPanel.Size = new System.Drawing.Size(627, 47);
            this.pageControlPanel.TabIndex = 15;
            // 
            // startPositionButton
            // 
            this.startPositionButton.Enabled = false;
            this.startPositionButton.ImageIndex = 5;
            this.startPositionButton.ImageList = this.mediaControlImages;
            this.startPositionButton.Location = new System.Drawing.Point(135, 3);
            this.startPositionButton.Name = "startPositionButton";
            this.startPositionButton.Size = new System.Drawing.Size(45, 40);
            this.startPositionButton.TabIndex = 18;
            this.startPositionButton.UseVisualStyleBackColor = true;
            this.startPositionButton.Click += new System.EventHandler(this.startPositionButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(54, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 40);
            this.button1.TabIndex = 17;
            this.button1.Text = "Query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // loadingPanel
            // 
            this.loadingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadingPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.loadingPanel.Controls.Add(this.label1);
            this.loadingPanel.Location = new System.Drawing.Point(212, 180);
            this.loadingPanel.Name = "loadingPanel";
            this.loadingPanel.Size = new System.Drawing.Size(200, 100);
            this.loadingPanel.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 96);
            this.label1.TabIndex = 0;
            this.label1.Text = "Opening file...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(627, 461);
            this.Controls.Add(this.loadingPanel);
            this.Controls.Add(this.resultsDataGridView);
            this.Controls.Add(this.pageControlPanel);
            this.Controls.Add(this.resultsStatusStrip);
            this.MinimumSize = new System.Drawing.Size(643, 500);
            this.Name = "Page";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Log Page";
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataSet)).EndInit();
            this.resultsStatusStrip.ResumeLayout(false);
            this.resultsStatusStrip.PerformLayout();
            this.pageControlPanel.ResumeLayout(false);
            this.loadingPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button pageDockButton;
        private System.Windows.Forms.DataGridView resultsDataGridView;
        private System.Windows.Forms.CheckBox playControlButton;
        private System.Windows.Forms.ImageList dockControlImageList;
        private Timer updateTimer;
        private System.Data.DataSet resultsDataSet;
        private Button nextRecordButton;
        private ImageList mediaControlImages;
        private StatusStrip resultsStatusStrip;
        private ToolStripStatusLabel resultsLabel;
        private Button firstRecordButton;
        private Button lastRecordButton;
        private ToolStripProgressBar resultsProgressBar;
        private ToolStripStatusLabel resultsDisplayedLabel;
        private Panel pageControlPanel;
        private Panel loadingPanel;
        private Label label1;
        private Button button1;
        private Button startPositionButton;
    }
}