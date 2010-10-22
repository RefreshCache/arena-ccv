using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Arena.Core;
using Arena.Enums;
using Arena.DataLayer.Core;

namespace DocumentScanner
{
	public partial class PersonSearchForm : Form
	{
		#region Private Variables

		private Person _selectedPerson = null;

		// Cached Lookups
		private Lookup _luHomePhone = new Lookup(SystemLookup.PhoneType_Home);
		private Lookup _luCellPhone = new Lookup(SystemLookup.PhoneType_Cell);
		private Lookup _luWorkPhone = new Lookup(SystemLookup.PhoneType_Business);
        private Lookup _luHomeAddress = new Lookup(SystemLookup.AddressType_Home);
		private Lookup _luAdultRole = new Lookup(SystemLookup.FamilyRole_Adult);
		private LookupType _lutMaritalStatus = new LookupType(SystemLookupType.MaritalStatus);

		#endregion

		#region Public Properties

		public Person SelectedPerson
		{
			get { return _selectedPerson; }
			set
			{
				_selectedPerson = value;
				DisplayPerson(_selectedPerson);
			}
		}

        #endregion

		#region Constructors

		public PersonSearchForm()
		{
			InitForm();
		}

		public PersonSearchForm(string firstName, string lastName)
		{
			InitForm();
			tbFirstName.Text = firstName;
			tbLastName.Text = lastName;
			PerformSearch();
		}

		#endregion

		#region Events

		#region Form

		private void Textbox_Enter(object sender, System.EventArgs e)
		{
			TextBox tb = (TextBox)sender;
			tb.SelectAll();
		}

		#endregion

		#region Search Events

		private void tbSearchField_Enter(object sender, EventArgs e)
		{
			TextBox tb = (TextBox)sender;
			tb.SelectAll();
			this.AcceptButton = btnSearch;
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			PerformSearch();
		}

		#endregion

		#region Results Datagrid Events

		private void dgvPersonSearch_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (dgvPersonSearch.SelectedRows.Count > 0)
				DisplayPerson(new Person((int)dgvPersonSearch.SelectedRows[0].Cells["PS_person_id"].Value));
		}

		#endregion

		#region Detail View Events

		private void lblFamilyMember_Click(object sender, EventArgs e)
		{
			Label lbl = (Label)sender;
			FamilyMember fm = (FamilyMember)lbl.Tag;
			DisplayPerson(fm);
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			if (_selectedPerson != null && _selectedPerson.PersonID != -1)
				this.DialogResult = DialogResult.OK;
			else
				ShowWarning("You have not selected a person", "Selection Error");
		}

		private void btnCancelSelect_Click(object sender, System.EventArgs e)
		{
			_selectedPerson = null;
			this.DialogResult = DialogResult.Cancel;
		}

		#endregion

		#endregion

		#region Private Methods

		private void InitForm()
		{
			InitializeComponent();

			_selectedPerson = null;

			tbFirstName.Text = string.Empty;
			tbLastName.Text = string.Empty;

			this.AcceptButton = null;
			this.CancelButton = btnCancelSelect;
		}

		private void PerformSearch()
		{
			DataTable dt = new PersonData().GetPersonByNameLimited_DT(tbFirstName.Text.Trim(), tbLastName.Text.Trim());
			dgvPersonSearch.DataSource = dt;

			if (dt.Rows.Count > 0)
			{
				dgvPersonSearch.Focus();
				DisplayPerson(new Person((int)dgvPersonSearch.SelectedRows[0].Cells["PS_person_id"].Value));
			}
			else
			{
				this.AcceptButton = null;
				this.CancelButton = btnCancelSelect;
				ShowMessage("There were not any people that matched your search criteria.", "No Matches");
			}
		}

		private void DisplayPerson(Person person)
		{
			_selectedPerson = person;

			if (_selectedPerson != null && _selectedPerson.PersonID != -1)
			{
				lblDisplayName.Text = _selectedPerson.FullName;
				if (_selectedPerson.Blob != null && _selectedPerson.Blob.ByteArray != null && _selectedPerson.Blob.ByteArray.Length > 0)
				{
					pbPersonPhoto.SizeMode = PictureBoxSizeMode.AutoSize;
					pbPersonPhoto.Image = _selectedPerson.Blob.GetImage(150, 150);
				}
				else
				{
					pbPersonPhoto.SizeMode = PictureBoxSizeMode.Normal;
					pbPersonPhoto.Size = new Size(150, 150);
					pbPersonPhoto.Image = null;
				}

				lblMemberStatus.Text = _selectedPerson.MemberStatus.Value;
				lblRecordStatus.Text = _selectedPerson.RecordStatus.ToString();
				if (_selectedPerson.BirthDate != DateTime.MinValue && _selectedPerson.BirthDate != PersonData.DateTimeMinValue && _selectedPerson.BirthDate != DateTime.Parse("1/1/1900"))
				{
					lblBirthDate.Text = _selectedPerson.BirthDate.ToShortDateString();
					lblAge.Text = _selectedPerson.Age.ToString();
				}
				else
				{
					lblBirthDate.Text = string.Empty;
					lblAge.Text = string.Empty;
				}
				lblGender.Text = _selectedPerson.Gender.ToString();
				lblMaritalStatus.Text = _selectedPerson.MaritalStatus.Value;

				DisplayPhone(_selectedPerson, lblHomePhone, _luHomePhone);
				DisplayPhone(_selectedPerson, lblBusinessPhone, _luWorkPhone);
				DisplayPhone(_selectedPerson, lblCellPhone, _luCellPhone);

				lblEmail.Text = _selectedPerson.Emails.FirstActive;

				PersonAddress homeAddress = _selectedPerson.Addresses.FindByType(_luHomeAddress.LookupID);
				if (homeAddress != null && homeAddress.Address != null)
					lblAddress1.Text = homeAddress.Address.ToString();
				else
					lblAddress1.Text = string.Empty;

				DisplayFamilyMembers(_selectedPerson);

				string createdBy = _selectedPerson.CreatedBy;
				if (createdBy.Trim() == string.Empty)
					createdBy = "?";

				string modifiedBy = _selectedPerson.ModifiedBy;
				if (modifiedBy.Trim() == string.Empty)
					modifiedBy = "?";

				lblAddedBy.Text = string.Format("{0} on {1}", createdBy, _selectedPerson.DateCreated.ToShortDateString());
				lblModifiedBy.Text = string.Format("{0} on {1}", modifiedBy, _selectedPerson.DateModified.ToShortDateString());
				lblLastVerified.Text = _selectedPerson.DateLastVerified.ToShortDateString();
			}

			this.AcceptButton = btnSelect;
			this.CancelButton = btnCancelSelect;
		}

		private void DisplayPhone(Person person, Label lbl, Lookup phoneType)
		{
			PersonPhone pp = person.Phones.FindByType(phoneType.LookupID);
			if (pp != null && pp.Number.Trim() != string.Empty)
				lbl.Text = pp.Number;
			else
				lbl.Text = string.Empty;
		}

		private void DisplayFamilyMembers(Person person)
		{
			pnlFamilyMembers.Controls.Clear();

			Font boldFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold);
			Font underlinedFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Underline);

			ArrayList adults = new ArrayList();
			ArrayList children = new ArrayList();
			bool adult = true;

			Family family = new Family();
			family.LoadByPersonID(person.PersonID);
			foreach (FamilyMember fm in family.FamilyMembers)
			{
				if (fm.PersonID != person.PersonID)
				{
					Label lbl = new Label();
					pnlFamilyMembers.Controls.Add(lbl);
					lbl.Text = fm.FullName;
					lbl.Tag = fm;
					lbl.Click += new EventHandler(lblFamilyMember_Click);
					lbl.Cursor = Cursors.Hand;
					lbl.Font = underlinedFont;
					lbl.ForeColor = Color.DarkBlue;
					lbl.Size = new Size(136, 16);
					lbl.Left = 16;
					if (fm.FamilyRole.Value.ToLower() == "adult")
						adults.Add(lbl);
					else
						children.Add(lbl);
				}
				else
				{
					if (fm.FamilyRole.Value.ToLower() == "adult")
						adult = true;
					else
						adult = false;
				}
			}

			int y = 0;
			if (adults.Count > 0)
			{
				Label lbl = new Label();
				pnlFamilyMembers.Controls.Add(lbl);
				if (adult)
					lbl.Text = "Spouse:";
				else
					lbl.Text = "Parent(s):";
				lbl.Font = boldFont;
				lbl.Size = new Size(152, 16);
				lbl.Left = 0;
				lbl.Top = y;
				y += 16;

				for (int i = 0; i < adults.Count; i++)
				{
					((Label)adults[i]).Top = y;
					y += 16;
				}

				y += 16;
			}
			if (children.Count > 0)
			{
				Label lbl = new Label();
				pnlFamilyMembers.Controls.Add(lbl);
				if (adult)
					lbl.Text = "Children:";
				else
					lbl.Text = "Siblings:";
				lbl.Font = boldFont;
				lbl.Size = new Size(152, 16);
				lbl.Left = 0;
				lbl.Top = y;
				y += 16;

				for (int i = 0; i < children.Count; i++)
				{
					((Label)children[i]).Top = y;
					y += 16;
				}

				y += 16;
			}
		}

		#region Utility Methods

		private void ShowMessage(string msg, string title)
		{
			this.Cursor = Cursors.Default;
			MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ShowWarning(string msg, string title)
		{
			this.Cursor = Cursors.Default;
			MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private void ReportError(System.Exception ex, bool fatalError)
		{
			// Add code to Log the Exception
			ReportError(ex.Message, fatalError);
		}

        private void ReportError(string msg, bool fatalError)
        {
            this.Cursor = Cursors.Default;

            if (fatalError)
            {
                MessageBox.Show(msg, "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _selectedPerson = null;
				this.DialogResult = DialogResult.Abort;
            }
            else
            {
                MessageBox.Show(msg, "Application Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

		#endregion
	}
}