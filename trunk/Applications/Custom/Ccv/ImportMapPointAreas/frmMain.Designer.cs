namespace ImportMapPointAreas
{
    partial class frmMain
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
            this.lbAreas = new System.Windows.Forms.ListBox();
            this.picImportedMap = new System.Windows.Forms.PictureBox();
            this.picAreaMap = new System.Windows.Forms.PictureBox();
            this.cbAreas = new System.Windows.Forms.ComboBox();
            this.btnLink = new System.Windows.Forms.Button();
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picImportedMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAreaMap)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbAreas
            // 
            this.lbAreas.CausesValidation = false;
            this.lbAreas.FormattingEnabled = true;
            this.lbAreas.Location = new System.Drawing.Point(12, 40);
            this.lbAreas.Name = "lbAreas";
            this.lbAreas.Size = new System.Drawing.Size(230, 498);
            this.lbAreas.TabIndex = 0;
            this.lbAreas.SelectedIndexChanged += new System.EventHandler(this.lbAreas_SelectedIndexChanged);
            // 
            // picImportedMap
            // 
            this.picImportedMap.BackColor = System.Drawing.Color.White;
            this.picImportedMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picImportedMap.Location = new System.Drawing.Point(248, 40);
            this.picImportedMap.Name = "picImportedMap";
            this.picImportedMap.Size = new System.Drawing.Size(300, 200);
            this.picImportedMap.TabIndex = 1;
            this.picImportedMap.TabStop = false;
            // 
            // picAreaMap
            // 
            this.picAreaMap.BackColor = System.Drawing.Color.White;
            this.picAreaMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAreaMap.Location = new System.Drawing.Point(248, 339);
            this.picAreaMap.Name = "picAreaMap";
            this.picAreaMap.Size = new System.Drawing.Size(300, 200);
            this.picAreaMap.TabIndex = 2;
            this.picAreaMap.TabStop = false;
            // 
            // cbAreas
            // 
            this.cbAreas.FormattingEnabled = true;
            this.cbAreas.Location = new System.Drawing.Point(248, 312);
            this.cbAreas.Name = "cbAreas";
            this.cbAreas.Size = new System.Drawing.Size(300, 21);
            this.cbAreas.TabIndex = 3;
            this.cbAreas.SelectedIndexChanged += new System.EventHandler(this.cbAreas_SelectedIndexChanged);
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(312, 246);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(115, 23);
            this.btnLink.TabIndex = 4;
            this.btnLink.Text = "Link to Area Below";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Location = new System.Drawing.Point(433, 246);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(115, 23);
            this.btnCreateNew.TabIndex = 5;
            this.btnCreateNew.Text = "Create New...";
            this.btnCreateNew.UseVisualStyleBackColor = true;
            this.btnCreateNew.Click += new System.EventHandler(this.btnCreateNew_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "MapPoint Areas";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "MapPoint Map";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 296);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Existing Arena Areas";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(312, 545);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(115, 23);
            this.btnImport.TabIndex = 9;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(433, 545);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "*.ptm";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 544);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Recalculate Address Areas";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 576);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(560, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(109, 17);
            this.status.Text = "toolStripStatusLabel1";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(433, 275);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(115, 23);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "Remove Area";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 598);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCreateNew);
            this.Controls.Add(this.btnLink);
            this.Controls.Add(this.cbAreas);
            this.Controls.Add(this.picAreaMap);
            this.Controls.Add(this.picImportedMap);
            this.Controls.Add(this.lbAreas);
            this.Name = "frmMain";
            this.Text = "Import MapPoint Areas";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picImportedMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAreaMap)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbAreas;
        private System.Windows.Forms.PictureBox picImportedMap;
        private System.Windows.Forms.PictureBox picAreaMap;
        private System.Windows.Forms.ComboBox cbAreas;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.Button btnCreateNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.Button btnRemove;
    }
}

