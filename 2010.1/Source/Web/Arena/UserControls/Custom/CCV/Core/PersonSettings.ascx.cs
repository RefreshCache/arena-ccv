namespace ArenaWeb.UserControls.Custom.CCV.Core
{
	using System;
	using System.Text;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

    using Arena.Core;
	using Arena.Portal;
    using Arena.Portal.UI;
	using Arena.Security;
	using Arena.Exceptions;
	using Arena.DataLayer.Core;

	/// <summary>
	///		Summary description for PersonSettingList.
	/// </summary>
	public partial class PersonSettings : PortalControl
	{
        private Person Person = null;
        private PersonSetting personSetting = null;
        private bool EditEnabled = false;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            lblMessage.Visible = false;

            LoadPerson();

            if (Person != null && Person.PersonID != -1)
            {
                EditEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);

                if (ihPersonSettingID.Value.Trim() != string.Empty)
                {
                    try
                    {
                        personSetting = new PersonSetting(Person.PersonID, CurrentOrganization.OrganizationID, ihPersonSettingID.Value.Trim());
                    }
                    catch
                    {
                        throw new ModuleException(CurrentPortalPage, CurrentModule, "Invalid Person Setting Key in hidden field");
                    }
                }

                if (!Page.IsPostBack)
                    ShowList();
            }
		}

 		public void ShowList()
		{
            DataTable dt = new PersonSettingData().GetPersonSettingByPersonId_DT(Person.PersonID, CurrentOrganization.OrganizationID);
            if (tbKeyFilter.Text.Trim() != string.Empty)
            {
                foreach (DataRow row in dt.Rows)
                    if (!row["key"].ToString().Trim().ToLower().StartsWith(tbKeyFilter.Text.Trim().ToLower()))
                        row.Delete();
                dt.AcceptChanges();
            }

			pnlPersonSettings.Visible = true;
			dgPersonSettings.Visible = true;
			pnlDetails.Visible = false;

			ihPersonSettingID.Value = string.Empty;

			dgPersonSettings.Visible = true;
			dgPersonSettings.ItemType = "Person Setting";
			dgPersonSettings.ItemBgColor = CurrentPortalPage.Setting("ItemBgColor", string.Empty, false);
			dgPersonSettings.ItemAltBgColor = CurrentPortalPage.Setting("ItemAltBgColor", string.Empty, false);
			dgPersonSettings.ItemMouseOverColor = CurrentPortalPage.Setting("ItemMouseOverColor", string.Empty, false);
			dgPersonSettings.AddEnabled = EditEnabled;
			dgPersonSettings.AddImageUrl = "~/images/add_personSetting.gif";
			dgPersonSettings.MoveEnabled = false;
			dgPersonSettings.SourceTableName = "core_person_setting";
			dgPersonSettings.SourceTableKeyColumnName = "key";
			dgPersonSettings.DeleteEnabled = EditEnabled;
			dgPersonSettings.EditEnabled = false;
			dgPersonSettings.MergeEnabled = false;
			dgPersonSettings.MailEnabled = false;
			dgPersonSettings.ExportEnabled = true;
			dgPersonSettings.AllowSorting = true;
            dgPersonSettings.DataSource = dt;
			dgPersonSettings.DataBind();

			if (dgPersonSettings.Items.Count > 0)
			{
				dgPersonSettings.Visible = true;
				lbAdd.Visible = false;
			}
			else
			{
				if (EditEnabled)
				{
					dgPersonSettings.Visible = false;
					lbAdd.Visible = true;
				}
				else
					pnlPersonSettings.Visible = false;
			}
		}

		private void ShowEdit(string personSettingKey)
		{
			ihPersonSettingID.Value = personSettingKey.ToString();

            personSetting = new PersonSetting(Person.PersonID, CurrentOrganization.OrganizationID, personSettingKey);

			if (personSetting.Key == string.Empty)
				personSetting.Key = personSettingKey;

            // Key
            tbKey.Text = personSetting.Key;

            // Value
            tbValue.Text = personSetting.Value;

			btnUpdate.Visible = EditEnabled;

			pnlPersonSettings.Visible = false;
			pnlDetails.Visible = true;
		}

        void btnApply_Click(object sender, EventArgs e)
        {
            ShowList();
        }

        private void dgPersonSettings_DeleteCommand(object sender, DataGridCommandEventArgs e)
		{
			new PersonSettingData().DeletePersonSetting(Person.PersonID, CurrentOrganization.OrganizationID, e.Item.Cells[0].Text);
			ShowList();
		}

		private void dgPersonSettings_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName != string.Empty)
			{
				if (e.CommandName == "EditPersonSetting")
					ShowEdit(e.Item.Cells[0].Text);
				else
					ShowList();
			}
		}

		private void dgPersonSettings_Rebind(object source, System.EventArgs e)
		{
			ShowList();
		}

		private void dgPersonSettings_AddItem(object sender, System.EventArgs e)
		{
			ShowEdit(string.Empty);
		}
		
		protected void btnUpdate_Click(object sender, System.EventArgs e)
		{
            if (personSetting == null)
                personSetting = new PersonSetting();

            if (tbKey.Text.Trim().ToLower() != personSetting.Key.Trim().ToLower())
            {
                PersonSetting dupSetting = new PersonSetting(Person.PersonID, CurrentOrganization.OrganizationID, tbKey.Text.Trim());
                if (dupSetting.Key.Trim().ToLower() == tbKey.Text.Trim().ToLower())
                {
                    lblMessage.Text = string.Format("'{0}' setting Key already exists.", dupSetting.Key);
                    lblMessage.Visible = true;
                }
            }

            if (!lblMessage.Visible)
            {
                if (tbKey.Text.Trim() == "gridSize")
                {
                    int gridSize = Int32.Parse(tbValue.Text.Trim());
                    Session["gridSize"] = gridSize;
                    dgPersonSettings.PageSize = gridSize;
                }

                personSetting.PersonId = Person.PersonID;
                personSetting.OrganizationId = CurrentOrganization.OrganizationID;
                personSetting.Key = tbKey.Text.Trim();
                personSetting.Value = tbValue.Text.Trim();

                personSetting.Save();

                if (CurrentPerson.PersonID == Person.PersonID)
                    CurrentPerson.Settings = null;

                ShowList();
            }
		}
			
		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			ShowList();
		}

        private void LoadPerson()
        {
            if (Person == null)
            {
                int PersonID = -1;
                Guid PersonGUID = Guid.Empty;
                bool isMe = false;

                string[] keys;
                keys = Request.QueryString.AllKeys;
                foreach (string key in keys)
                {
                    switch (key.ToUpper())
                    {
                        case "GUID":
                            try { PersonGUID = new Guid(Request.QueryString.Get(key)); }
                            catch { PersonGUID = Guid.Empty; }
                            break;
                        case "ME":
                            isMe = true;
                            break;
                    }
                }

                if (PersonID != -1)
                    Person = new Person(PersonID);
                else if (PersonGUID != Guid.Empty)
                    Person = new Person(PersonGUID);
                else if (isMe)
                    Person = new Person(CurrentPerson.PersonID);
            }
        }

        protected string EncodedValue(object settingValue)
        {
            string encodedString = HttpUtility.HtmlEncode((string)settingValue);
            if (encodedString.Length > 60)
                return encodedString.Substring(0, 60) + "...";
            else
                return encodedString;
        }

        #region Web Form Designer generated code

		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbAdd.Click += new EventHandler(dgPersonSettings_AddItem);
            this.btnApply.Click += new EventHandler(btnApply_Click);
			this.dgPersonSettings.ReBind += new DataGridReBindEventHandler(this.dgPersonSettings_Rebind);
			this.dgPersonSettings.AddItem += new AddItemEventHandler(dgPersonSettings_AddItem);
			this.dgPersonSettings.DeleteCommand += new DataGridCommandEventHandler(dgPersonSettings_DeleteCommand);
			this.dgPersonSettings.ItemCommand += new DataGridCommandEventHandler(dgPersonSettings_ItemCommand);
		}

		#endregion

	}
}
