namespace ArenaWeb.UserControls.Custom.CCV.Core
{
	using System;
	using System.Text;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Security;
    using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
	using Arena.Core;
	using Arena.Enums;
	using Arena.Security;
	using Arena.Portal;
	using Arena.Exceptions;
	using Arena.DataLayer.Security;

	/// <summary>
	///		Summary description for UserLogin.
	/// </summary>
	public partial class NewFamily : PortalControl
	{
		#region Module Settings

		// Module Settings
		[LookupSetting("Member Status", "The Member Status to set a user to when they add themself through this form.", true, "0B4532DB-3188-40F5-B188-E7E6E4448C85")]
		public string MemberStatusIDSetting { get { return Setting("MemberStatusID", "", true); } }

        [CampusSetting("Default Campus", "The campus to assign a user to when they add themself through this form.", false)]
        public string CampusSetting { get { return Setting("Campus", "", false); } }

        [TextSetting("Redirect URL", "The URL that the user will be redirected to after selecting or creating their account.  This can be overridden by the query string.", false)]
		public string RedirectSetting { get { return Setting("Redirect", "", false); } }

        [TextSetting("Notification Email", "The Email address(es) that should be sent a notification.  If a value is specified here, a new family will not be created, but instead information will be emailed.", false)]
        public string NotificationEmailSetting { get { return Setting("NotificationEmail", "", false); } }

        [TextSetting("Internal Arena URL", "The internal Arena URL to prefix links in email with.", false)]
        public string ArenaURLSetting { get { return Setting("ArenaURL", "", false); } }

        [PageSetting("Person Detail Page", "The page that is used for displaying person details.", true)]
        public string PersonDetailPageIDSetting { get { return Setting("PersonDetailPageID", "", true); } }

        [PageSetting("Group Detail Page", "The page that is used for displaying group details.", true)]
        public string GroupDetailPageIDSetting { get { return Setting("GroupDetailPageID", "", true); } }

		#endregion

		private Person person = null;
        private Person spouse = null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			person = CurrentPerson;
            if (person == null)
                person = new Person();
            else
                spouse = person.Spouse();

            if (spouse == null)
                spouse = new Person();

			if (!Page.IsPostBack)
			{
				iRedirect.Value = string.Empty;
				if (Request.QueryString["requestpage"] != null)
					iRedirect.Value = string.Format("default.aspx?page={0}", Request.QueryString["requestpage"]);
                if (iRedirect.Value == string.Empty && Request.QueryString["requestUrl"] != null && (!Request.QueryString["requestUrl"].ToLower().Contains("http")))
					iRedirect.Value = Page.Server.HtmlDecode(Request.QueryString["requestUrl"]);
				if (iRedirect.Value == string.Empty && RedirectSetting != string.Empty)
					iRedirect.Value = RedirectSetting;

				SetInfo();
			}
        }

		private void SetInfo()
		{
			ddlState.SelectedIndex = -1;

            // Load Person Values
			tbFirstName.Text = person.FirstName;
			tbLastName.Text = person.LastName;

            PersonPhone phone = person.Phones.FindByType(SystemLookup.PhoneType_Home);
            if (phone != null)
                tbHomePhone.Text = phone.Number;
            phone = person.Phones.FindByType(SystemLookup.PhoneType_Cell);
            if (phone != null)
            {
                tbCellPhone.Text = phone.Number;
                cbCellSMS.Checked = phone.SMSEnabled;
            }

            tbEmail.Text = person.Emails.FirstActive;

            // Load Spouse Values
            tbSpouseFirstName.Text = spouse.FirstName;
            tbSpouseLastName.Text = spouse.LastName;

            phone = spouse.Phones.FindByType(SystemLookup.PhoneType_Home);
            if (phone != null)
                tbSpouseHomePhone.Text = phone.Number;
            phone = spouse.Phones.FindByType(SystemLookup.PhoneType_Cell);
            if (phone != null)
            {
                tbSpouseCellPhone.Text = phone.Number;
                cbSpouseSMS.Checked = phone.SMSEnabled;
            }

            tbSpouseEmail.Text = spouse.Emails.FirstActive;

            if (person.PrimaryAddress != null && person.PrimaryAddress.AddressID != -1)
			{
				tbStreetAddress.Text = person.PrimaryAddress.StreetLine1;
				tbCity.Text = person.PrimaryAddress.City;
				ListItem li = ddlState.Items.FindByValue(person.PrimaryAddress.State);
				if (li != null) li.Selected = true;
				tbZipCode.Text = person.PrimaryAddress.PostalCode;
			}
		}

		private void btnSubmit_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				UpdateFamily();
			}
			else
				Page.FindControl("valSummary").Visible = true;
		}

		private void UpdateFamily()
		{
			int organizationID = CurrentPortal.OrganizationID;
			string userID = CurrentUser.Identity.Name;
			if (userID == string.Empty)
				userID = "UserConfirmation.ascx";

			bool newPerson = (person.PersonID == -1);
            bool newSpouse = (spouse.PersonID == -1);
            bool spouseUpdated = tbSpouseFirstName.Text.Trim() != string.Empty;

            if (NotificationEmailSetting != string.Empty &&
                (newPerson || (newSpouse && spouseUpdated)))
            {
                SendNotification();
            }
            else
            {

                Lookup memberStatus;
                try
                {
                    memberStatus = new Lookup(Int32.Parse(MemberStatusIDSetting));
                    if (memberStatus.LookupID == -1)
                        throw new ModuleException(CurrentPortalPage, CurrentModule, "Member Status setting must be a valid Member Status Lookup value.");
                }
                catch (System.Exception ex)
                {
                    throw new ModuleException(CurrentPortalPage, CurrentModule, "Member Status setting must be a valid Member Status Lookup value.", ex);
                }

                if (newPerson)
                {
                    person.RecordStatus = RecordStatus.Pending;
                    person.MemberStatus = memberStatus;

                    if (CampusSetting != string.Empty)
                        try { person.Campus = new Arena.Organization.Campus(Int32.Parse(CampusSetting)); }
                        catch { person.Campus = null; }

                    person.MaritalStatus = new Lookup(SystemLookup.MaritalStatus_Unknown);
                    person.Gender = Gender.Unknown;
                }

                person.FirstName = tbFirstName.Text.Trim();
                person.LastName = tbLastName.Text.Trim();

                Lookup HomePhoneType = new Lookup(SystemLookup.PhoneType_Home);
                PersonPhone HomePhone = person.Phones.FindByType(HomePhoneType.LookupID);
                if (HomePhone == null)
                {
                    HomePhone = new PersonPhone();
                    HomePhone.PhoneType = HomePhoneType;
                    person.Phones.Add(HomePhone);
                }
                HomePhone.Number = tbHomePhone.Text.Trim();

                Lookup CellPhoneType = new Lookup(SystemLookup.PhoneType_Cell);
                PersonPhone CellPhone = person.Phones.FindByType(CellPhoneType.LookupID);
                if (CellPhone == null)
                {
                    CellPhone = new PersonPhone();
                    CellPhone.PhoneType = CellPhoneType;
                    person.Phones.Add(CellPhone);
                }
                CellPhone.Number = tbCellPhone.Text.Trim();
                CellPhone.SMSEnabled = cbCellSMS.Checked;

                person.Emails.FirstActive = tbEmail.Text.Trim();

                Lookup HomeAddressType = new Lookup(SystemLookup.AddressType_Home);
                PersonAddress HomeAddress = person.Addresses.FindByType(HomeAddressType.LookupID);
                if (HomeAddress == null)
                {
                    HomeAddress = new PersonAddress();
                    HomeAddress.AddressType = HomeAddressType;
                    person.Addresses.Add(HomeAddress);
                }
                HomeAddress.Address = new Address(
                    tbStreetAddress.Text.Trim(),
                    string.Empty,
                    tbCity.Text.Trim(),
                    ddlState.SelectedValue,
                    tbZipCode.Text.Trim(),
                    false);
                HomeAddress.Primary = true;

                person.Save(organizationID, userID, false);
                person.SaveAddresses(organizationID, userID);
                person.SavePhones(organizationID, userID);
                person.SaveEmails(organizationID, userID);

                if (spouseUpdated)
                {
                    if (newSpouse)
                    {
                        spouse.RecordStatus = RecordStatus.Pending;
                        spouse.MemberStatus = memberStatus;

                        if (CampusSetting != string.Empty)
                            try { spouse.Campus = new Arena.Organization.Campus(Int32.Parse(CampusSetting)); }
                            catch { spouse.Campus = null; }

                        spouse.MaritalStatus = new Lookup(SystemLookup.MaritalStatus_Married);
                        person.MaritalStatus = spouse.MaritalStatus;
                        spouse.Gender = Gender.Unknown;
                    }

                    spouse.FirstName = tbSpouseFirstName.Text.Trim();
                    spouse.LastName = tbSpouseLastName.Text.Trim();

                    PersonPhone SpouseHomePhone = spouse.Phones.FindByType(HomePhoneType.LookupID);
                    if (SpouseHomePhone == null)
                    {
                        SpouseHomePhone = new PersonPhone();
                        SpouseHomePhone.PhoneType = HomePhoneType;
                        spouse.Phones.Add(SpouseHomePhone);
                    }
                    SpouseHomePhone.Number = tbSpouseHomePhone.Text.Trim();

                    PersonPhone SpouseCellPhone = spouse.Phones.FindByType(CellPhoneType.LookupID);
                    if (SpouseCellPhone == null)
                    {
                        SpouseCellPhone = new PersonPhone();
                        SpouseCellPhone.PhoneType = CellPhoneType;
                        spouse.Phones.Add(SpouseCellPhone);
                    }
                    SpouseCellPhone.Number = tbSpouseCellPhone.Text.Trim();
                    SpouseCellPhone.SMSEnabled = cbSpouseSMS.Checked;

                    spouse.Emails.FirstActive = tbSpouseEmail.Text.Trim();

                    spouse.Save(organizationID, userID, false);
                    spouse.SaveAddresses(organizationID, userID);
                    spouse.SavePhones(organizationID, userID);
                    spouse.SaveEmails(organizationID, userID);
                }

                if (newPerson)
                {
                    Family family = new Family();
                    family.OrganizationID = organizationID;
                    family.FamilyName = tbLastName.Text.Trim() + " Family";
                    family.Save(userID);

                    FamilyMember fm = new FamilyMember(family.FamilyID, person.PersonID);
                    fm.FamilyID = family.FamilyID;
                    fm.FamilyRole = new Lookup(SystemLookup.FamilyRole_Adult);
                    fm.Save(userID);

                    if (spouseUpdated)
                    {
                        fm = new FamilyMember(family.FamilyID, spouse.PersonID);
                        fm.FamilyID = family.FamilyID;
                        fm.FamilyRole = new Lookup(SystemLookup.FamilyRole_Adult);
                        fm.Save(userID);
                    }
                }
            }

            if (iRedirect.Value.Trim() != string.Empty)
            {
                StringBuilder sbRedirect = new StringBuilder();
                sbRedirect.Append(iRedirect.Value.Trim());
                sbRedirect.Append("&confirmed=true");

                // If this is a new person, the person object needs to be reloaded to get new GUID
                if (newPerson)
                    person = new Person(person.PersonID);

                sbRedirect.AppendFormat("&person={0}", person.PersonGUID.ToString());

                if (spouseUpdated)
                {
                    if (newSpouse)
                        spouse = new Person(spouse.PersonID);

                    sbRedirect.AppendFormat("&spouse={0}", spouse.PersonGUID.ToString());
                }

                Response.Redirect(sbRedirect.ToString(), true);
            }
		}

        private void SendNotification()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<style>\n");
            //sb.Append("\tbody { font-family:Verdana, Arial, Helvetica, sans-serif; font-size:12px; }\n");
            sb.Append("\ttd { font-family:Verdana, Arial, Helvetica, sans-serif; font-size:12px; padding-right:8px; font-size:12px; }\n");
            sb.Append("</style>\n");
            sb.Append("<div style='font-family:Verdana, Arial, Helvetica, sans-serif; font-size:12px;'>\n");
            sb.Append("The following information was updated from the Website.<br/><br/>\n");
            sb.Append("<table>\n");

            string heading = "New Person";
            string spouseHeading = "New Spouse"; 

            if (person.PersonID != -1)
                heading = string.Format("<a href='{0}/default.aspx?page={1}&guid={2}'>Existing Person</a>",
                    ArenaURLSetting, PersonDetailPageIDSetting, person.PersonGUID.ToString());

            if (spouse.PersonID != -1)
                spouseHeading = string.Format("<a href='{0}/default.aspx?page={1}&guid={2}'>Existing Spouse</a>",
                    ArenaURLSetting, PersonDetailPageIDSetting, spouse.PersonGUID.ToString());

            sb.AppendFormat("<tr><td colspan='2' style='border-bottom:1px solid black'>{0}</td><td style='border-bottom:1px solid black'>{1}</td></tr>\n", 
                heading, spouseHeading);

            sb.AppendFormat("<tr><td><strong>First Name</strong></td><td>{0}</td><td>{1}</td></tr>\n", 
                tbFirstName.Text, tbSpouseFirstName.Text);

            sb.AppendFormat("<tr><td><strong>Last Name</strong></td><td>{0}</td><td>{1}</td></tr>\n",
                tbLastName.Text, tbSpouseLastName.Text);

            sb.AppendFormat("<tr><td><strong>Home Phone</strong></td><td>{0}</td><td>{1}</td></tr>\n",
                tbHomePhone.Text, tbSpouseHomePhone.Text);

            sb.AppendFormat("<tr><td><strong>Cell Phone</strong></td><td>{0}</td><td>{1}</td></tr>\n",
                tbCellPhone.Text, tbSpouseCellPhone.Text);

            sb.AppendFormat("<tr><td><strong>E-Mail</strong></td><td>{0}</td><td>{1}</td></tr>\n",
                tbEmail.Text, tbSpouseEmail.Text);

            sb.AppendFormat("<tr><td><strong>Address</strong></td><td>{0}</td><td></td></tr>\n", 
                tbStreetAddress.Text);

            sb.AppendFormat("<tr><td><strong>City</strong></td><td>{0}</td><td></td></tr>\n", 
                tbCity.Text);

            sb.AppendFormat("<tr><td><strong>State</strong></td><td>{0}</td><td></td></tr>\n", 
                ddlState.SelectedValue);

            sb.AppendFormat("<tr><td><strong>Zip</strong></td><td>{0}</td><td></td></tr>\n", 
                tbZipCode.Text);

            sb.Append("</table><br/><br/>\n");

            try
            {
                if (Request.QueryString["group"] != null)
                {
                    Arena.SmallGroup.Group group = new Arena.SmallGroup.Group(Int32.Parse(Request.QueryString["group"]));
                    sb.AppendFormat("He and/or she was also interested in <a href='{0}default.aspx?page={1}&group={2}'>{3}</a>.<br/><br/>\n",
                        ArenaURLSetting, GroupDetailPageIDSetting, group.GroupID.ToString(), group.Title);
                }
            }
            catch { }

            sb.Append("</div>\n");

            Arena.Utility.ArenaSendMail.SendMail(string.Empty, string.Empty, NotificationEmailSetting, "New Family Update", sb.ToString());
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
			this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
		}
		#endregion

	}
}
