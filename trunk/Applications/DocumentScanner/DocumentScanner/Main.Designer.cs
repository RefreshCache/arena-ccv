namespace DocumentScanner
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblScannerStatus = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.docViewer = new Atalasoft.Imaging.WinControls.DocumentViewer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.miDocumentType = new System.Windows.Forms.ToolStripMenuItem();
            this.miPageSize = new System.Windows.Forms.ToolStripMenuItem();
            this.letter85X110ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legal85X14ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statement55X85ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miRotation = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miRotateRight = new System.Windows.Forms.ToolStripMenuItem();
            this.miRotateLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miImageDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.docViewer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(810, 604);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(810, 628);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.lblScannerStatus);
            this.panel1.Controls.Add(this.btnUpload);
            this.panel1.Controls.Add(this.btnScan);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(804, 62);
            this.panel1.TabIndex = 1;
            // 
            // lblScannerStatus
            // 
            this.lblScannerStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScannerStatus.Location = new System.Drawing.Point(405, 0);
            this.lblScannerStatus.Name = "lblScannerStatus";
            this.lblScannerStatus.Size = new System.Drawing.Size(399, 62);
            this.lblScannerStatus.TabIndex = 2;
            this.lblScannerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(120, 19);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(25, 19);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // docViewer
            // 
            this.docViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.docViewer.ImageControl.AntialiasDisplay = Atalasoft.Imaging.WinControls.AntialiasDisplayMode.ScaleToGray;
            this.docViewer.ImageControl.AutoZoom = Atalasoft.Imaging.WinControls.AutoZoomMode.BestFit;
            this.docViewer.ImageControl.BackColor = System.Drawing.SystemColors.Control;
            this.docViewer.ImageControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.docViewer.ImageControl.Magnifier.BackColor = System.Drawing.Color.White;
            this.docViewer.ImageControl.Magnifier.BorderColor = System.Drawing.Color.Black;
            this.docViewer.ImageControl.Magnifier.Size = new System.Drawing.Size(100, 100);
            this.docViewer.Location = new System.Drawing.Point(0, 71);
            this.docViewer.Name = "docViewer";
            this.docViewer.Separator.BackColor = System.Drawing.SystemColors.ControlLight;
            this.docViewer.Separator.Mode = Atalasoft.Imaging.WinControls.ControlSeparator.Splitter;
            this.docViewer.Size = new System.Drawing.Size(807, 550);
            this.docViewer.TabIndex = 5;
            this.docViewer.Text = "documentViewer1";
            this.docViewer.ThumbnailControl.BackColor = System.Drawing.SystemColors.Control;
            this.docViewer.ThumbnailControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.docViewer.ThumbnailControl.DragSelectionColor = System.Drawing.Color.Red;
            this.docViewer.ThumbnailControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.docViewer.ThumbnailControl.ForeColor = System.Drawing.SystemColors.WindowText;
            this.docViewer.ThumbnailControl.HighlightBackgroundColor = System.Drawing.SystemColors.Highlight;
            this.docViewer.ThumbnailControl.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.docViewer.ThumbnailControl.Margins = new Atalasoft.Imaging.WinControls.Margin(4, 4, 4, 4);
            this.docViewer.ThumbnailControl.SelectionMode = Atalasoft.Imaging.WinControls.ThumbnailSelectionMode.SingleSelect;
            this.docViewer.ThumbnailControl.SelectionRectangleBackColor = System.Drawing.Color.Transparent;
            this.docViewer.ThumbnailControl.SelectionRectangleDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.docViewer.ThumbnailControl.SelectionRectangleLineColor = System.Drawing.Color.Black;
            this.docViewer.ThumbnailControl.ThumbnailOffset = new System.Drawing.Point(0, 0);
            this.docViewer.ThumbnailControl.ThumbnailSize = new System.Drawing.Size(100, 100);
            this.docViewer.UndoLevels = 100;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.imageToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(810, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miScanner,
            this.miDocumentType,
            this.miPageSize,
            this.miRotation,
            this.toolStripSeparator3,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            // 
            // miScanner
            // 
            this.miScanner.Name = "miScanner";
            this.miScanner.Size = new System.Drawing.Size(159, 22);
            this.miScanner.Text = "&Scanner";
            this.miScanner.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.miScanner_DropDownItemClicked);
            // 
            // miDocumentType
            // 
            this.miDocumentType.Name = "miDocumentType";
            this.miDocumentType.Size = new System.Drawing.Size(159, 22);
            this.miDocumentType.Text = "&Document Type";
            this.miDocumentType.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mi_DropDownItemClicked);
            // 
            // miPageSize
            // 
            this.miPageSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.letter85X110ToolStripMenuItem,
            this.legal85X14ToolStripMenuItem,
            this.statement55X85ToolStripMenuItem});
            this.miPageSize.Name = "miPageSize";
            this.miPageSize.Size = new System.Drawing.Size(159, 22);
            this.miPageSize.Text = "&Page Size";
            this.miPageSize.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mi_DropDownItemClicked);
            // 
            // letter85X110ToolStripMenuItem
            // 
            this.letter85X110ToolStripMenuItem.Name = "letter85X110ToolStripMenuItem";
            this.letter85X110ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.letter85X110ToolStripMenuItem.Text = "Letter (8.5 x 11.0)";
            // 
            // legal85X14ToolStripMenuItem
            // 
            this.legal85X14ToolStripMenuItem.Name = "legal85X14ToolStripMenuItem";
            this.legal85X14ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.legal85X14ToolStripMenuItem.Text = "Legal (8.5 x 14.)";
            // 
            // statement55X85ToolStripMenuItem
            // 
            this.statement55X85ToolStripMenuItem.Name = "statement55X85ToolStripMenuItem";
            this.statement55X85ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.statement55X85ToolStripMenuItem.Text = "Statement (5.5 x 8.5)";
            // 
            // miRotation
            // 
            this.miRotation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.miRotation.Name = "miRotation";
            this.miRotation.Size = new System.Drawing.Size(159, 22);
            this.miRotation.Text = "Scan &Rotation";
            this.miRotation.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mi_DropDownItemClicked);
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.noneToolStripMenuItem.Text = "None";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem2.Text = "90";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem3.Text = "180";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem4.Text = "270";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(156, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.miExit.Size = new System.Drawing.Size(159, 22);
            this.miExit.Text = "Exit";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRotateRight,
            this.miRotateLeft,
            this.toolStripSeparator1,
            this.miImageDelete});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "&Image";
            // 
            // miRotateRight
            // 
            this.miRotateRight.Name = "miRotateRight";
            this.miRotateRight.Size = new System.Drawing.Size(139, 22);
            this.miRotateRight.Text = "Rotate &Right";
            this.miRotateRight.Click += new System.EventHandler(this.miRotateRight_Click);
            // 
            // miRotateLeft
            // 
            this.miRotateLeft.Name = "miRotateLeft";
            this.miRotateLeft.Size = new System.Drawing.Size(139, 22);
            this.miRotateLeft.Text = "Rotate &Left";
            this.miRotateLeft.Click += new System.EventHandler(this.miRotateLeft_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // miImageDelete
            // 
            this.miImageDelete.Name = "miImageDelete";
            this.miImageDelete.Size = new System.Drawing.Size(139, 22);
            this.miImageDelete.Text = "&Delete";
            this.miImageDelete.Click += new System.EventHandler(this.miImageDelete_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 628);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Arena Document Scanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private Atalasoft.Imaging.WinControls.DocumentViewer docViewer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miScanner;
        private System.Windows.Forms.ToolStripMenuItem miDocumentType;
        private System.Windows.Forms.ToolStripMenuItem miPageSize;
        private System.Windows.Forms.ToolStripMenuItem miRotation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem letter85X110ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legal85X14ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statement55X85ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miRotateRight;
        private System.Windows.Forms.ToolStripMenuItem miRotateLeft;
        private System.Windows.Forms.ToolStripMenuItem miImageDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label lblScannerStatus;
    }
}

