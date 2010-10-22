namespace DocumentScanner
{
	partial class PersonSearchForm
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
            this.lblFirstNameC = new System.Windows.Forms.Label();
            this.lblLastNameC = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvPersonSearch = new Arena.WinControls.DataGridView();
            this.PS_person_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PS_first_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PS_last_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblResults = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.pnlSelectedPerson = new System.Windows.Forms.Panel();
            this.btnCancelSelect = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblLastVerified = new System.Windows.Forms.Label();
            this.lblLastVerifiedC = new System.Windows.Forms.Label();
            this.lblModifiedBy = new System.Windows.Forms.Label();
            this.lblModifiedByC = new System.Windows.Forms.Label();
            this.lblAddedBy = new System.Windows.Forms.Label();
            this.lblAddedByC = new System.Windows.Forms.Label();
            this.lblAddress1 = new System.Windows.Forms.Label();
            this.lblAddress1C = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblEmailC = new System.Windows.Forms.Label();
            this.lblCellPhone = new System.Windows.Forms.Label();
            this.lblCellPhoneC = new System.Windows.Forms.Label();
            this.lblBusinessPhone = new System.Windows.Forms.Label();
            this.lblBusinessPhoneC = new System.Windows.Forms.Label();
            this.lblHomePhone = new System.Windows.Forms.Label();
            this.lblHomePhoneC = new System.Windows.Forms.Label();
            this.lblMaritalStatus = new System.Windows.Forms.Label();
            this.lblMaritalStatusC = new System.Windows.Forms.Label();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblGenderC = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.lblAgeC = new System.Windows.Forms.Label();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblBirthDateC = new System.Windows.Forms.Label();
            this.lblRecordStatus = new System.Windows.Forms.Label();
            this.lblRecordStatusC = new System.Windows.Forms.Label();
            this.lblMemberStatus = new System.Windows.Forms.Label();
            this.lblMemberStatusC = new System.Windows.Forms.Label();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.pnlFamilyMembers = new System.Windows.Forms.Panel();
            this.pbPersonPhoto = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonSearch)).BeginInit();
            this.pnlSelectedPerson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPersonPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFirstNameC
            // 
            this.lblFirstNameC.AutoSize = true;
            this.lblFirstNameC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstNameC.Location = new System.Drawing.Point(13, 9);
            this.lblFirstNameC.Name = "lblFirstNameC";
            this.lblFirstNameC.Size = new System.Drawing.Size(58, 13);
            this.lblFirstNameC.TabIndex = 0;
            this.lblFirstNameC.Text = "First Name";
            // 
            // lblLastNameC
            // 
            this.lblLastNameC.AutoSize = true;
            this.lblLastNameC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastNameC.Location = new System.Drawing.Point(148, 9);
            this.lblLastNameC.Name = "lblLastNameC";
            this.lblLastNameC.Size = new System.Drawing.Size(57, 13);
            this.lblLastNameC.TabIndex = 2;
            this.lblLastNameC.Text = "Last Name";
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(16, 25);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(128, 21);
            this.tbFirstName.TabIndex = 1;
            this.tbFirstName.Enter += new System.EventHandler(this.tbSearchField_Enter);
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(151, 25);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(128, 21);
            this.tbLastName.TabIndex = 3;
            this.tbLastName.Enter += new System.EventHandler(this.tbSearchField_Enter);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(285, 23);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvPersonSearch
            // 
            this.dgvPersonSearch.AllowUserToAddRows = false;
            this.dgvPersonSearch.AllowUserToDeleteRows = false;
            this.dgvPersonSearch.AllowUserToOrderColumns = true;
            this.dgvPersonSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPersonSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPersonSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PS_person_id,
            this.PS_first_name,
            this.PS_last_name});
            this.dgvPersonSearch.Location = new System.Drawing.Point(16, 80);
            this.dgvPersonSearch.MultiSelect = false;
            this.dgvPersonSearch.Name = "dgvPersonSearch";
            this.dgvPersonSearch.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvPersonSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPersonSearch.Size = new System.Drawing.Size(344, 480);
            this.dgvPersonSearch.TabIndex = 6;
            this.dgvPersonSearch.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPersonSearch_RowEnter);
            // 
            // PS_person_id
            // 
            this.PS_person_id.DataPropertyName = "person_id";
            this.PS_person_id.HeaderText = "Person ID";
            this.PS_person_id.Name = "PS_person_id";
            this.PS_person_id.ReadOnly = true;
            this.PS_person_id.Visible = false;
            // 
            // PS_first_name
            // 
            this.PS_first_name.DataPropertyName = "nick_name";
            this.PS_first_name.HeaderText = "First Name";
            this.PS_first_name.Name = "PS_first_name";
            this.PS_first_name.ReadOnly = true;
            // 
            // PS_last_name
            // 
            this.PS_last_name.DataPropertyName = "last_name";
            this.PS_last_name.HeaderText = "Last Name";
            this.PS_last_name.Name = "PS_last_name";
            this.PS_last_name.ReadOnly = true;
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResults.Location = new System.Drawing.Point(13, 59);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(97, 14);
            this.lblResults.TabIndex = 5;
            this.lblResults.Text = "Search Results";
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(365, 60);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(94, 14);
            this.lblDetails.TabIndex = 7;
            this.lblDetails.Text = "Person Details";
            // 
            // pnlSelectedPerson
            // 
            this.pnlSelectedPerson.BackColor = System.Drawing.Color.White;
            this.pnlSelectedPerson.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSelectedPerson.Controls.Add(this.btnCancelSelect);
            this.pnlSelectedPerson.Controls.Add(this.btnSelect);
            this.pnlSelectedPerson.Controls.Add(this.lblLastVerified);
            this.pnlSelectedPerson.Controls.Add(this.lblLastVerifiedC);
            this.pnlSelectedPerson.Controls.Add(this.lblModifiedBy);
            this.pnlSelectedPerson.Controls.Add(this.lblModifiedByC);
            this.pnlSelectedPerson.Controls.Add(this.lblAddedBy);
            this.pnlSelectedPerson.Controls.Add(this.lblAddedByC);
            this.pnlSelectedPerson.Controls.Add(this.lblAddress1);
            this.pnlSelectedPerson.Controls.Add(this.lblAddress1C);
            this.pnlSelectedPerson.Controls.Add(this.lblEmail);
            this.pnlSelectedPerson.Controls.Add(this.lblEmailC);
            this.pnlSelectedPerson.Controls.Add(this.lblCellPhone);
            this.pnlSelectedPerson.Controls.Add(this.lblCellPhoneC);
            this.pnlSelectedPerson.Controls.Add(this.lblBusinessPhone);
            this.pnlSelectedPerson.Controls.Add(this.lblBusinessPhoneC);
            this.pnlSelectedPerson.Controls.Add(this.lblHomePhone);
            this.pnlSelectedPerson.Controls.Add(this.lblHomePhoneC);
            this.pnlSelectedPerson.Controls.Add(this.lblMaritalStatus);
            this.pnlSelectedPerson.Controls.Add(this.lblMaritalStatusC);
            this.pnlSelectedPerson.Controls.Add(this.lblGender);
            this.pnlSelectedPerson.Controls.Add(this.lblGenderC);
            this.pnlSelectedPerson.Controls.Add(this.lblAge);
            this.pnlSelectedPerson.Controls.Add(this.lblAgeC);
            this.pnlSelectedPerson.Controls.Add(this.lblBirthDate);
            this.pnlSelectedPerson.Controls.Add(this.lblBirthDateC);
            this.pnlSelectedPerson.Controls.Add(this.lblRecordStatus);
            this.pnlSelectedPerson.Controls.Add(this.lblRecordStatusC);
            this.pnlSelectedPerson.Controls.Add(this.lblMemberStatus);
            this.pnlSelectedPerson.Controls.Add(this.lblMemberStatusC);
            this.pnlSelectedPerson.Controls.Add(this.lblDisplayName);
            this.pnlSelectedPerson.Controls.Add(this.pnlFamilyMembers);
            this.pnlSelectedPerson.Controls.Add(this.pbPersonPhoto);
            this.pnlSelectedPerson.Location = new System.Drawing.Point(368, 80);
            this.pnlSelectedPerson.Name = "pnlSelectedPerson";
            this.pnlSelectedPerson.Size = new System.Drawing.Size(512, 520);
            this.pnlSelectedPerson.TabIndex = 8;
            // 
            // btnCancelSelect
            // 
            this.btnCancelSelect.Location = new System.Drawing.Point(416, 480);
            this.btnCancelSelect.Name = "btnCancelSelect";
            this.btnCancelSelect.Size = new System.Drawing.Size(75, 23);
            this.btnCancelSelect.TabIndex = 35;
            this.btnCancelSelect.Text = "&Cancel";
            this.btnCancelSelect.UseVisualStyleBackColor = true;
            this.btnCancelSelect.Click += new System.EventHandler(this.btnCancelSelect_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(336, 480);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 34;
            this.btnSelect.Text = "Se&lect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblLastVerified
            // 
            this.lblLastVerified.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastVerified.Location = new System.Drawing.Point(112, 448);
            this.lblLastVerified.Name = "lblLastVerified";
            this.lblLastVerified.Size = new System.Drawing.Size(360, 16);
            this.lblLastVerified.TabIndex = 30;
            this.lblLastVerified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLastVerifiedC
            // 
            this.lblLastVerifiedC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastVerifiedC.Location = new System.Drawing.Point(8, 448);
            this.lblLastVerifiedC.Name = "lblLastVerifiedC";
            this.lblLastVerifiedC.Size = new System.Drawing.Size(104, 16);
            this.lblLastVerifiedC.TabIndex = 29;
            this.lblLastVerifiedC.Text = "Last Verified:";
            this.lblLastVerifiedC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblModifiedBy
            // 
            this.lblModifiedBy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModifiedBy.Location = new System.Drawing.Point(112, 432);
            this.lblModifiedBy.Name = "lblModifiedBy";
            this.lblModifiedBy.Size = new System.Drawing.Size(360, 16);
            this.lblModifiedBy.TabIndex = 28;
            this.lblModifiedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblModifiedByC
            // 
            this.lblModifiedByC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModifiedByC.Location = new System.Drawing.Point(8, 432);
            this.lblModifiedByC.Name = "lblModifiedByC";
            this.lblModifiedByC.Size = new System.Drawing.Size(104, 16);
            this.lblModifiedByC.TabIndex = 27;
            this.lblModifiedByC.Text = "Modified By:";
            this.lblModifiedByC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddedBy
            // 
            this.lblAddedBy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddedBy.Location = new System.Drawing.Point(112, 416);
            this.lblAddedBy.Name = "lblAddedBy";
            this.lblAddedBy.Size = new System.Drawing.Size(360, 16);
            this.lblAddedBy.TabIndex = 26;
            this.lblAddedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddedByC
            // 
            this.lblAddedByC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddedByC.Location = new System.Drawing.Point(8, 416);
            this.lblAddedByC.Name = "lblAddedByC";
            this.lblAddedByC.Size = new System.Drawing.Size(104, 16);
            this.lblAddedByC.TabIndex = 25;
            this.lblAddedByC.Text = "Added By:";
            this.lblAddedByC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddress1
            // 
            this.lblAddress1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress1.Location = new System.Drawing.Point(184, 287);
            this.lblAddress1.Name = "lblAddress1";
            this.lblAddress1.Size = new System.Drawing.Size(296, 32);
            this.lblAddress1.TabIndex = 24;
            this.lblAddress1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAddress1C
            // 
            this.lblAddress1C.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress1C.Location = new System.Drawing.Point(168, 271);
            this.lblAddress1C.Name = "lblAddress1C";
            this.lblAddress1C.Size = new System.Drawing.Size(112, 16);
            this.lblAddress1C.TabIndex = 23;
            this.lblAddress1C.Text = "Address:";
            this.lblAddress1C.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEmail
            // 
            this.lblEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(184, 239);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(296, 16);
            this.lblEmail.TabIndex = 22;
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEmailC
            // 
            this.lblEmailC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmailC.Location = new System.Drawing.Point(168, 223);
            this.lblEmailC.Name = "lblEmailC";
            this.lblEmailC.Size = new System.Drawing.Size(112, 16);
            this.lblEmailC.TabIndex = 21;
            this.lblEmailC.Text = "E-mail:";
            this.lblEmailC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCellPhone
            // 
            this.lblCellPhone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCellPhone.Location = new System.Drawing.Point(296, 192);
            this.lblCellPhone.Name = "lblCellPhone";
            this.lblCellPhone.Size = new System.Drawing.Size(184, 16);
            this.lblCellPhone.TabIndex = 18;
            this.lblCellPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCellPhoneC
            // 
            this.lblCellPhoneC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCellPhoneC.Location = new System.Drawing.Point(168, 192);
            this.lblCellPhoneC.Name = "lblCellPhoneC";
            this.lblCellPhoneC.Size = new System.Drawing.Size(112, 16);
            this.lblCellPhoneC.TabIndex = 17;
            this.lblCellPhoneC.Text = "Cell Phone:";
            this.lblCellPhoneC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBusinessPhone
            // 
            this.lblBusinessPhone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBusinessPhone.Location = new System.Drawing.Point(296, 176);
            this.lblBusinessPhone.Name = "lblBusinessPhone";
            this.lblBusinessPhone.Size = new System.Drawing.Size(184, 16);
            this.lblBusinessPhone.TabIndex = 16;
            this.lblBusinessPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBusinessPhoneC
            // 
            this.lblBusinessPhoneC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBusinessPhoneC.Location = new System.Drawing.Point(168, 176);
            this.lblBusinessPhoneC.Name = "lblBusinessPhoneC";
            this.lblBusinessPhoneC.Size = new System.Drawing.Size(112, 16);
            this.lblBusinessPhoneC.TabIndex = 15;
            this.lblBusinessPhoneC.Text = "Business Phone:";
            this.lblBusinessPhoneC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHomePhone
            // 
            this.lblHomePhone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHomePhone.Location = new System.Drawing.Point(296, 160);
            this.lblHomePhone.Name = "lblHomePhone";
            this.lblHomePhone.Size = new System.Drawing.Size(184, 16);
            this.lblHomePhone.TabIndex = 14;
            this.lblHomePhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHomePhoneC
            // 
            this.lblHomePhoneC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHomePhoneC.Location = new System.Drawing.Point(168, 160);
            this.lblHomePhoneC.Name = "lblHomePhoneC";
            this.lblHomePhoneC.Size = new System.Drawing.Size(112, 16);
            this.lblHomePhoneC.TabIndex = 13;
            this.lblHomePhoneC.Text = "Home Phone:";
            this.lblHomePhoneC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMaritalStatus
            // 
            this.lblMaritalStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaritalStatus.Location = new System.Drawing.Point(296, 128);
            this.lblMaritalStatus.Name = "lblMaritalStatus";
            this.lblMaritalStatus.Size = new System.Drawing.Size(184, 16);
            this.lblMaritalStatus.TabIndex = 12;
            this.lblMaritalStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMaritalStatusC
            // 
            this.lblMaritalStatusC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaritalStatusC.Location = new System.Drawing.Point(168, 128);
            this.lblMaritalStatusC.Name = "lblMaritalStatusC";
            this.lblMaritalStatusC.Size = new System.Drawing.Size(112, 16);
            this.lblMaritalStatusC.TabIndex = 11;
            this.lblMaritalStatusC.Text = "Marital Status:";
            this.lblMaritalStatusC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGender
            // 
            this.lblGender.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.Location = new System.Drawing.Point(296, 112);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(184, 16);
            this.lblGender.TabIndex = 10;
            this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGenderC
            // 
            this.lblGenderC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenderC.Location = new System.Drawing.Point(168, 112);
            this.lblGenderC.Name = "lblGenderC";
            this.lblGenderC.Size = new System.Drawing.Size(112, 16);
            this.lblGenderC.TabIndex = 9;
            this.lblGenderC.Text = "Gender:";
            this.lblGenderC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAge
            // 
            this.lblAge.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.Location = new System.Drawing.Point(296, 96);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(184, 16);
            this.lblAge.TabIndex = 8;
            this.lblAge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAgeC
            // 
            this.lblAgeC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgeC.Location = new System.Drawing.Point(168, 96);
            this.lblAgeC.Name = "lblAgeC";
            this.lblAgeC.Size = new System.Drawing.Size(112, 16);
            this.lblAgeC.TabIndex = 7;
            this.lblAgeC.Text = "Age:";
            this.lblAgeC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.Location = new System.Drawing.Point(296, 80);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(184, 16);
            this.lblBirthDate.TabIndex = 6;
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBirthDateC
            // 
            this.lblBirthDateC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDateC.Location = new System.Drawing.Point(168, 80);
            this.lblBirthDateC.Name = "lblBirthDateC";
            this.lblBirthDateC.Size = new System.Drawing.Size(112, 16);
            this.lblBirthDateC.TabIndex = 5;
            this.lblBirthDateC.Text = "Birth Date:";
            this.lblBirthDateC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecordStatus
            // 
            this.lblRecordStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordStatus.Location = new System.Drawing.Point(296, 64);
            this.lblRecordStatus.Name = "lblRecordStatus";
            this.lblRecordStatus.Size = new System.Drawing.Size(184, 16);
            this.lblRecordStatus.TabIndex = 4;
            this.lblRecordStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecordStatusC
            // 
            this.lblRecordStatusC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordStatusC.Location = new System.Drawing.Point(168, 64);
            this.lblRecordStatusC.Name = "lblRecordStatusC";
            this.lblRecordStatusC.Size = new System.Drawing.Size(112, 16);
            this.lblRecordStatusC.TabIndex = 3;
            this.lblRecordStatusC.Text = "Record Status:";
            this.lblRecordStatusC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMemberStatus
            // 
            this.lblMemberStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMemberStatus.Location = new System.Drawing.Point(296, 48);
            this.lblMemberStatus.Name = "lblMemberStatus";
            this.lblMemberStatus.Size = new System.Drawing.Size(184, 16);
            this.lblMemberStatus.TabIndex = 2;
            this.lblMemberStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMemberStatusC
            // 
            this.lblMemberStatusC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMemberStatusC.Location = new System.Drawing.Point(168, 48);
            this.lblMemberStatusC.Name = "lblMemberStatusC";
            this.lblMemberStatusC.Size = new System.Drawing.Size(112, 16);
            this.lblMemberStatusC.TabIndex = 1;
            this.lblMemberStatusC.Text = "Member Status:";
            this.lblMemberStatusC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplayName.Location = new System.Drawing.Point(168, 8);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(312, 24);
            this.lblDisplayName.TabIndex = 0;
            // 
            // pnlFamilyMembers
            // 
            this.pnlFamilyMembers.Location = new System.Drawing.Point(8, 160);
            this.pnlFamilyMembers.Name = "pnlFamilyMembers";
            this.pnlFamilyMembers.Size = new System.Drawing.Size(152, 248);
            this.pnlFamilyMembers.TabIndex = 31;
            // 
            // pbPersonPhoto
            // 
            this.pbPersonPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPersonPhoto.Location = new System.Drawing.Point(8, 8);
            this.pbPersonPhoto.Name = "pbPersonPhoto";
            this.pbPersonPhoto.Size = new System.Drawing.Size(150, 150);
            this.pbPersonPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPersonPhoto.TabIndex = 0;
            this.pbPersonPhoto.TabStop = false;
            // 
            // PersonSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 608);
            this.ControlBox = false;
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.dgvPersonSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbLastName);
            this.Controls.Add(this.tbFirstName);
            this.Controls.Add(this.lblLastNameC);
            this.Controls.Add(this.lblFirstNameC);
            this.Controls.Add(this.pnlSelectedPerson);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersonSearchForm";
            this.ShowInTaskbar = false;
            this.Text = "Person Search";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonSearch)).EndInit();
            this.pnlSelectedPerson.ResumeLayout(false);
            this.pnlSelectedPerson.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPersonPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFirstNameC;
		private System.Windows.Forms.Label lblLastNameC;
		private System.Windows.Forms.TextBox tbFirstName;
		private System.Windows.Forms.TextBox tbLastName;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Label lblResults;
		private System.Windows.Forms.Label lblDetails;
		private System.Windows.Forms.Panel pnlSelectedPerson;
		private System.Windows.Forms.Label lblMemberStatusC;
		private System.Windows.Forms.Label lblDisplayName;
		private System.Windows.Forms.Panel pnlFamilyMembers;
		private System.Windows.Forms.PictureBox pbPersonPhoto;
		private System.Windows.Forms.Label lblMemberStatus;
		private System.Windows.Forms.Label lblMaritalStatus;
		private System.Windows.Forms.Label lblMaritalStatusC;
		private System.Windows.Forms.Label lblGender;
		private System.Windows.Forms.Label lblGenderC;
		private System.Windows.Forms.Label lblAge;
		private System.Windows.Forms.Label lblAgeC;
		private System.Windows.Forms.Label lblBirthDate;
		private System.Windows.Forms.Label lblBirthDateC;
		private System.Windows.Forms.Label lblRecordStatus;
        private System.Windows.Forms.Label lblRecordStatusC;
		private System.Windows.Forms.Label lblCellPhone;
		private System.Windows.Forms.Label lblCellPhoneC;
		private System.Windows.Forms.Label lblBusinessPhone;
		private System.Windows.Forms.Label lblBusinessPhoneC;
		private System.Windows.Forms.Label lblHomePhone;
		private System.Windows.Forms.Label lblHomePhoneC;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.Label lblEmailC;
		private System.Windows.Forms.Label lblAddress1;
		private System.Windows.Forms.Label lblAddress1C;
		private System.Windows.Forms.Label lblAddedBy;
		private System.Windows.Forms.Label lblAddedByC;
		private System.Windows.Forms.Label lblLastVerified;
		private System.Windows.Forms.Label lblLastVerifiedC;
		private System.Windows.Forms.Label lblModifiedBy;
		private System.Windows.Forms.Label lblModifiedByC;
		private System.Windows.Forms.Button btnCancelSelect;
        private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridViewTextBoxColumn PS_person_id;
		private System.Windows.Forms.DataGridViewTextBoxColumn PS_first_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn PS_last_name;
        private Arena.WinControls.DataGridView dgvPersonSearch;
	}
}