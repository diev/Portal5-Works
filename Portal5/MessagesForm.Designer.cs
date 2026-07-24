namespace Portal5
{
    partial class MessagesForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            messagesGrid = new DataGridView();
            flowLayoutPanel1 = new FlowLayoutPanel();
            inBox = new CheckBox();
            outBox = new CheckBox();
            taskBox = new ComboBox();
            minDateBox = new DateTimePicker();
            maxDateBox = new DateTimePicker();
            statusStrip1 = new StatusStrip();
            StatusLabel = new ToolStripStatusLabel();
            contextMenu = new ContextMenuStrip(components);
            refreshMenuItem = new ToolStripMenuItem();
            filterMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            saveMessagesMenuItem = new ToolStripMenuItem();
            saveMessageMenuItem = new ToolStripMenuItem();
            saveZipMenuItem = new ToolStripMenuItem();
            saveFilesMenuItem = new ToolStripMenuItem();
            decryptFilesMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            viewLkMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)messagesGrid).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            contextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // messagesGrid
            // 
            messagesGrid.AllowUserToAddRows = false;
            messagesGrid.AllowUserToDeleteRows = false;
            messagesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            messagesGrid.Dock = DockStyle.Fill;
            messagesGrid.Location = new Point(0, 31);
            messagesGrid.Name = "messagesGrid";
            messagesGrid.ReadOnly = true;
            messagesGrid.Size = new Size(800, 397);
            messagesGrid.TabIndex = 0;
            messagesGrid.CellContentDoubleClick += messagesGrid_CellContentDoubleClick;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel1.Controls.Add(inBox);
            flowLayoutPanel1.Controls.Add(outBox);
            flowLayoutPanel1.Controls.Add(taskBox);
            flowLayoutPanel1.Controls.Add(minDateBox);
            flowLayoutPanel1.Controls.Add(maxDateBox);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(800, 31);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // inBox
            // 
            inBox.AutoSize = true;
            inBox.Checked = true;
            inBox.CheckState = CheckState.Checked;
            inBox.Location = new Point(3, 3);
            inBox.Name = "inBox";
            inBox.Size = new Size(39, 19);
            inBox.TabIndex = 0;
            inBox.Text = "Вх";
            inBox.UseVisualStyleBackColor = true;
            inBox.CheckedChanged += RefreshGrid;
            // 
            // outBox
            // 
            outBox.AutoSize = true;
            outBox.Checked = true;
            outBox.CheckState = CheckState.Checked;
            outBox.Location = new Point(48, 3);
            outBox.Name = "outBox";
            outBox.Size = new Size(47, 19);
            outBox.TabIndex = 1;
            outBox.Text = "Исх";
            outBox.UseVisualStyleBackColor = true;
            outBox.CheckedChanged += RefreshGrid;
            // 
            // taskBox
            // 
            taskBox.DropDownStyle = ComboBoxStyle.DropDownList;
            taskBox.FormattingEnabled = true;
            taskBox.Location = new Point(101, 3);
            taskBox.Name = "taskBox";
            taskBox.Size = new Size(136, 23);
            taskBox.Sorted = true;
            taskBox.TabIndex = 2;
            taskBox.SelectedIndexChanged += RefreshGrid;
            // 
            // minDateBox
            // 
            minDateBox.Format = DateTimePickerFormat.Short;
            minDateBox.Location = new Point(243, 3);
            minDateBox.MinDate = new DateTime(2016, 1, 1, 0, 0, 0, 0);
            minDateBox.Name = "minDateBox";
            minDateBox.ShowCheckBox = true;
            minDateBox.Size = new Size(123, 23);
            minDateBox.TabIndex = 4;
            minDateBox.ValueChanged += RefreshGrid;
            // 
            // maxDateBox
            // 
            maxDateBox.Format = DateTimePickerFormat.Short;
            maxDateBox.Location = new Point(372, 3);
            maxDateBox.MinDate = new DateTime(2016, 1, 1, 0, 0, 0, 0);
            maxDateBox.Name = "maxDateBox";
            maxDateBox.ShowCheckBox = true;
            maxDateBox.Size = new Size(123, 23);
            maxDateBox.TabIndex = 5;
            maxDateBox.ValueChanged += RefreshGrid;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { StatusLabel });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            StatusLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(64, 17);
            StatusLabel.Text = "Загрузка...";
            // 
            // contextMenu
            // 
            contextMenu.Items.AddRange(new ToolStripItem[] { refreshMenuItem, filterMenuItem, toolStripSeparator2, saveMessagesMenuItem, saveMessageMenuItem, saveZipMenuItem, saveFilesMenuItem, decryptFilesMenuItem, toolStripSeparator1, viewLkMenuItem });
            contextMenu.Name = "contextMenu";
            contextMenu.Size = new Size(208, 192);
            contextMenu.Opening += contextMenu_Opening;
            // 
            // refreshMenuItem
            // 
            refreshMenuItem.Name = "refreshMenuItem";
            refreshMenuItem.Size = new Size(207, 22);
            refreshMenuItem.Text = "Обновить";
            refreshMenuItem.Click += RefreshGrid;
            // 
            // filterMenuItem
            // 
            filterMenuItem.Name = "filterMenuItem";
            filterMenuItem.Size = new Size(207, 22);
            filterMenuItem.Text = "Фильтр по TaskName";
            filterMenuItem.Click += filterMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(204, 6);
            // 
            // saveMessagesMenuItem
            // 
            saveMessagesMenuItem.Name = "saveMessagesMenuItem";
            saveMessagesMenuItem.Size = new Size(207, 22);
            saveMessagesMenuItem.Text = "Сохранить Messages...";
            saveMessagesMenuItem.Click += saveMessagesMenuItem_Click;
            // 
            // saveMessageMenuItem
            // 
            saveMessageMenuItem.Name = "saveMessageMenuItem";
            saveMessageMenuItem.Size = new Size(207, 22);
            saveMessageMenuItem.Text = "Сохранить Message...";
            saveMessageMenuItem.Click += saveMessageMenuItem_Click;
            // 
            // saveZipMenuItem
            // 
            saveZipMenuItem.Name = "saveZipMenuItem";
            saveZipMenuItem.Size = new Size(207, 22);
            saveZipMenuItem.Text = "Сохранить Zip...";
            saveZipMenuItem.Click += saveZipMenuItem_Click;
            // 
            // saveFilesMenuItem
            // 
            saveFilesMenuItem.Name = "saveFilesMenuItem";
            saveFilesMenuItem.Size = new Size(207, 22);
            saveFilesMenuItem.Text = "Сохранить файлы...";
            saveFilesMenuItem.Click += saveFilesMenuItem_Click;
            // 
            // decryptFilesMenuItem
            // 
            decryptFilesMenuItem.Name = "decryptFilesMenuItem";
            decryptFilesMenuItem.Size = new Size(207, 22);
            decryptFilesMenuItem.Text = "Расшифровать файлы...";
            decryptFilesMenuItem.Click += decryptFilesMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(204, 6);
            // 
            // viewLkMenuItem
            // 
            viewLkMenuItem.Name = "viewLkMenuItem";
            viewLkMenuItem.Size = new Size(207, 22);
            viewLkMenuItem.Text = "Открыть на сайте ЦБ";
            viewLkMenuItem.Click += viewLkMenuItem_Click;
            // 
            // MessagesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(messagesGrid);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(statusStrip1);
            Name = "MessagesForm";
            Text = "Portal5 Messages";
            ((System.ComponentModel.ISupportInitialize)messagesGrid).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            contextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView messagesGrid;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox inBox;
        private CheckBox outBox;
        private ComboBox taskBox;
        private DateTimePicker minDateBox;
        private DateTimePicker maxDateBox;
        private StatusStrip statusStrip1;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem saveMessagesMenuItem;
        private ToolStripMenuItem saveMessageMenuItem;
        private ToolStripMenuItem saveZipMenuItem;
        private ToolStripMenuItem saveFilesMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem viewLkMenuItem;
        private ToolStripMenuItem filterMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripStatusLabel StatusLabel;
        private ToolStripMenuItem refreshMenuItem;
        private ToolStripMenuItem decryptFilesMenuItem;
    }
}
