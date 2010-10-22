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
	using Arena.Portal;
    using Arena.Portal.UI;

    using Arena.Custom.CCV.Core;


	/// <summary>
	///		Summary description for UserLogin.
	/// </summary>
    public partial class FamilyRegistration : PortalControl
	{
		#region Module Settings

        // Module Settings

        [LookupSetting("Member Status", "The Member Status to set a user to when they add themself through this form.", true, "0B4532DB-3188-40F5-B188-E7E6E4448C85")]
		public string MemberStatusIDSetting { get { return Setting("MemberStatusID", "", true); } }

        [CampusSetting("Default Campus", "The campus to assign a user to when they add themself through this form.", false)]
        public string CampusSetting { get { return Setting("Campus", "", false); } }

		#endregion

        #region Private Variables

        private Family _family = null;

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (_family == null)
            {
                FamilyMember you = null;
                FamilyMember spouse = null;

                if (CurrentPerson != null)
                {
                    _family = CurrentPerson.Family();

                    you = _family.FamilyMembers.FindByGuid(CurrentPerson.PersonGUID);

                    spouse = _family.Spouse(CurrentPerson);
                }
                else
                {
                    _family = new Family();

                    you = new FamilyMember();
                    you.PersonGUID = Guid.NewGuid();
                    you.FamilyRole = new Lookup(SystemLookup.FamilyRole_Adult);
                    you.Gender = Arena.Enums.Gender.Unknown;
                    _family.FamilyMembers.Add(you);
                }

                // Save Spouse
                if (spouse == null)
                {
                    spouse = new FamilyMember();
                    spouse.PersonGUID = Guid.NewGuid();
                    spouse.FamilyRole = new Lookup(SystemLookup.FamilyRole_Adult);
                    if (CurrentPerson != null && CurrentPerson.Gender == Arena.Enums.Gender.Male)
                        you.Gender = Arena.Enums.Gender.Female;
                    else if (CurrentPerson != null && CurrentPerson.Gender == Arena.Enums.Gender.Female)
                        you.Gender = Arena.Enums.Gender.Male;
                    else
                        you.Gender = Arena.Enums.Gender.Unknown;
                    _family.FamilyMembers.Add(spouse);
                }

                // Save Guids
                hfYouGuid.Value = you.PersonGUID.ToString();
                hfSpouseGuid.Value = spouse.PersonGUID.ToString();
            }

            if (Page.IsPostBack)
                UpdateChanges();
            else
                ShowYou();
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (ViewState["Family"] != null)
                _family = (Family)ViewState["Family"];
        }

        protected override object SaveViewState()
        {
            ViewState["Family"] = _family;

            return base.SaveViewState();
        }

        #endregion

        #region You Events

        protected void lbYouNext_Click(object sender, EventArgs e)
        {
            ShowSpouse();
        }

        #endregion

        #region Sposue Events

        protected void lbSpousePrev_Click(object sender, EventArgs e)
        {
            ShowYou();
        }

        protected void lbSpouseNext_Click(object sender, EventArgs e)
        {
            ShowFamilyMembers();
        }

        #endregion

        #region Family Member Events

        protected void lvFamilyMembers_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                FamilyMember you = _family.FamilyMembers.FindByGuid(new Guid(hfYouGuid.Value));

                FamilyMember fm = new FamilyMember();
                fm.PersonGUID = Guid.NewGuid();
                fm.FamilyRole = new Lookup(SystemLookup.FamilyRole_Child);
                fm.LastName = you != null ? you.LastName : string.Empty;
                _family.FamilyMembers.Add(fm);

                ShowFamilyMembers();
            }

            if (e.CommandName == "Remove")
            {
                HiddenField hfPersonGuid = (HiddenField)e.Item.FindControl("hfPersonGuid");
                _family.FamilyMembers.Remove(new Guid(hfPersonGuid.Value));

                ShowFamilyMembers();
            }
        }

        protected void lbFamilyMembersPrev_Click(object sender, EventArgs e)
        {
            ShowSpouse();
        }

        protected void lbFamilyMembersNext_Click(object sender, EventArgs e)
        {
            ShowContactInfo();
        }

        #endregion

        #region Contact Info Events

        protected void lbContactInfoPrev_Click(object sender, EventArgs e)
        {
            ShowFamilyMembers();
        }
        protected void lbContactInfoNext_Click(object sender, EventArgs e)
        {
            SaveChanges();
            Response.Redirect(string.Format("eWelcome.aspx?p={0}", hfYouGuid.Value), true);
        }

        #endregion

        #endregion

        #region Private Methods

        private void UpdateChanges()
        {
            if (pnlYou.Visible)
            {
                FamilyMember fm = _family.FamilyMembers.FindByGuid(new Guid(hfYouGuid.Value));
                if (fm != null)
                {
                    fm.NickName = tbYouFirstName.Text;
                    fm.LastName = tbYouLastName.Text;

                    PersonAddress homeAddress = fm.Addresses.FindByType(SystemLookup.AddressType_Home);
                    if (homeAddress == null)
                    {
                        homeAddress = new PersonAddress();
                        homeAddress.AddressType = new Lookup(SystemLookup.AddressType_Home);
                        homeAddress.Address = new Address();

                        if (fm.PrimaryAddress == null)
                            homeAddress.Primary = true;

                        fm.Addresses.Add(homeAddress);
                    }

                    homeAddress.Address.StreetLine1 = tbAddress.Text;
                    homeAddress.Address.City = tbCity.Text;
                    homeAddress.Address.State = tbState.Text;
                    homeAddress.Address.PostalCode = tbZip.Text;

                    PersonPhone homePhone = fm.Phones.FindByType(SystemLookup.PhoneType_Home);
                    if (homePhone == null)
                    {
                        homePhone = new PersonPhone();
                        homePhone.PhoneType = new Lookup(SystemLookup.PhoneType_Home);
                        fm.Phones.Add(homePhone);
                    }
                    homePhone.Number = ptbYouHomeNum.PhoneNumber;

                    fm.BirthDate = dtYouBirthDate.SelectedDate;

                    if (ddlYouGender.SelectedValue == "Male")
                        fm.Gender = Arena.Enums.Gender.Male;
                    else if (ddlYouGender.SelectedValue == "Female")
                        fm.Gender = Arena.Enums.Gender.Female;
                    else
                        fm.Gender = Arena.Enums.Gender.Unknown;
                }
            }

            if (pnlSpouse.Visible)
            {
                FamilyMember fm = _family.FamilyMembers.FindByGuid(new Guid(hfSpouseGuid.Value));
                if (fm != null)
                {
                    fm.NickName = tbSpouseFirstName.Text;
                    fm.LastName = tbSpouseLastName.Text;

                    fm.BirthDate = dtSpouseBirthDate.SelectedDate;

                    FamilyMember spouse = _family.Spouse(fm);
                    if (spouse != null)
                    {
                        if (spouse.Gender == Arena.Enums.Gender.Male)
                            fm.Gender = Arena.Enums.Gender.Female;
                        else if (spouse.Gender == Arena.Enums.Gender.Female)
                            fm.Gender = Arena.Enums.Gender.Male;
                    }
                }
            }

            if (pnlFamilyMembers.Visible)
            {
                foreach (ListViewDataItem item in lvFamilyMembers.Items)
                {
                    HiddenField hfPersonGuid = (HiddenField)item.FindControl("hfPersonGuid");
                    FamilyMember fm = _family.FamilyMembers.FindByGuid(new Guid(hfPersonGuid.Value));

                    fm.NickName = ((TextBox)item.FindControl("tbFirstName")).Text;
                    fm.LastName = ((TextBox)item.FindControl("tbLastName")).Text;
                    fm.BirthDate = ((DateTextBox)item.FindControl("dtbBirthDate")).SelectedDate;

                    string gender = ((DropDownList)item.FindControl("ddlGender")).SelectedValue;
                    switch (gender)
                    {
                        case "Male": fm.Gender = Arena.Enums.Gender.Male; break;
                        case "Female": fm.Gender = Arena.Enums.Gender.Female; break;
                        default: fm.Gender = Arena.Enums.Gender.Unknown; break;
                    }

                    int grade = Convert.ToInt32(((DropDownList)item.FindControl("ddlGrade")).SelectedValue);
                    fm.GraduationDate = Person.CalculateGraduationYear(grade, CurrentOrganization.GradePromotionDate);
                }
            }

            if (pnlContactInfo.Visible)
            {
                foreach (ListViewDataItem item in lvEmailCell.Items)
                {
                    HiddenField hfPersonGuid = (HiddenField)item.FindControl("hfPersonGuid");
                    FamilyMember fm = _family.FamilyMembers.FindByGuid(new Guid(hfPersonGuid.Value));
                    fm.Emails.FirstActive = ((TextBox)item.FindControl("tbEmail")).Text;

                    PersonPhone cellPhone = fm.Phones.FindByType(SystemLookup.PhoneType_Cell);
                    PhoneTextBox ptbCellNum = ((PhoneTextBox)item.FindControl("ptbCellNum"));
                    if (cellPhone == null && ptbCellNum.PhoneNumber.Trim() != string.Empty)
                    {
                        cellPhone = new PersonPhone();
                        cellPhone.PhoneType = new Lookup(SystemLookup.PhoneType_Cell);
                        fm.Phones.Add(cellPhone);
                    }
                    if (cellPhone != null)
                        cellPhone.Number = ptbCellNum.PhoneNumber;
                }
            }
        }

        private void SaveChanges()
        {
            string userId = CurrentUser != null ? CurrentUser.Identity.Name : "PreRegistration";

            // Default Member Status
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

            // Default Campus
            Arena.Organization.Campus campus = null;
            if (CampusSetting != string.Empty)
                campus = new Arena.Organization.Campus(Int32.Parse(CampusSetting));

            FamilyMember you = _family.FamilyMembers.FindByGuid(new Guid(hfYouGuid.Value));

            if (_family.FamilyID == -1)
            {
                _family.OrganizationID = CurrentOrganization.OrganizationID;
                _family.FamilyName = you.LastName.Trim() + " Family";
                _family.Save(userId);
            }

            foreach (FamilyMember fm in _family.FamilyMembers)
            {
                //*******Marital Status
                if (fm.PersonID == -1)
                {
                    fm.FamilyID = _family.FamilyID;
                    fm.MemberStatus = memberStatus;
                    fm.Campus = campus;
                    fm.OrganizationID = CurrentOrganization.OrganizationID;
                }

                if (fm.PersonID != -1 || fm.NickName != string.Empty)
                {
                    if (fm.PersonGUID != you.PersonGUID)
                    {
                        PersonAddress homeAddress = fm.Addresses.FindByType(SystemLookup.AddressType_Home);
                        if (homeAddress == null)
                        {
                            homeAddress = new PersonAddress();
                            homeAddress.AddressType = new Lookup(SystemLookup.AddressType_Home);
                            fm.Addresses.Add(homeAddress);
                            if (fm.Addresses.PrimaryAddress() == null)
                                homeAddress.Primary = true;
                        }
                        homeAddress.Address = you.Addresses.FindByType(SystemLookup.AddressType_Home).Address;

                        PersonPhone homePhone = fm.Phones.FindByType(SystemLookup.PhoneType_Home);
                        if (homePhone == null)
                        {
                            homePhone = new PersonPhone();
                            homePhone.PhoneType = new Lookup(SystemLookup.PhoneType_Home);
                            fm.Phones.Add(homePhone);
                        }
                        homePhone.Number = you.Phones.FindByType(SystemLookup.PhoneType_Home).Number;
                    }


                    // Save Person
                    fm.Save(CurrentOrganization.OrganizationID, userId, false);
                    fm.SaveAddresses(CurrentOrganization.OrganizationID, userId);
                    fm.SavePhones(CurrentOrganization.OrganizationID, userId);
                    fm.SaveEmails(CurrentOrganization.OrganizationID, userId);
                    fm.Save(userId);
                }
            }
        }

        private void ShowYou()
        {
            FamilyMember fm = _family.FamilyMembers.FindByGuid(new Guid(hfYouGuid.Value));
            if (fm != null)
            {
                tbYouFirstName.Text = fm.NickName;
                tbYouLastName.Text = fm.LastName;

                PersonAddress homeAddress = fm.Addresses.FindByType(SystemLookup.AddressType_Home);
                tbAddress.Text = homeAddress != null ? homeAddress.Address.StreetLine1 : string.Empty;
                tbCity.Text = homeAddress != null ? homeAddress.Address.City : string.Empty;
                tbState.Text = homeAddress != null ? homeAddress.Address.State : string.Empty;
                tbZip.Text = homeAddress != null ? homeAddress.Address.PostalCode : string.Empty;


                PersonPhone homePhone = fm.Phones.FindByType(SystemLookup.PhoneType_Home);
                ptbYouHomeNum.PhoneNumber = homePhone != null ? homePhone.Number : string.Empty;
                ptbYouHomeNum.ShowExtension = false;

                dtYouBirthDate.SelectedDate = fm.BirthDate;
                if (fm.Gender == Arena.Enums.Gender.Male)
                    ddlYouGender.SelectedValue = "Male";
                else if (fm.Gender == Arena.Enums.Gender.Female)
                    ddlYouGender.SelectedValue = "Female";
                else
                    ddlYouGender.SelectedValue = "Unknown";
            }
            ShowPanel(pnlYou);
        }

        private void ShowSpouse()
        {
            FamilyMember fm = _family.FamilyMembers.FindByGuid(new Guid(hfSpouseGuid.Value));
            if (fm != null)
            {
                tbSpouseFirstName.Text = fm.NickName;
                tbSpouseLastName.Text = fm.LastName;
                dtSpouseBirthDate.SelectedDate = fm.BirthDate;
            }
            ShowPanel(pnlSpouse);
        }

        private void ShowFamilyMembers()
        {
            lvFamilyMembers.DataSource = _family.Children();
            lvFamilyMembers.DataBind();

            ShowPanel(pnlFamilyMembers);
        }

        private void ShowContactInfo()
        {
            lvEmailCell.DataSource = _family.FamilyMembers;
            lvEmailCell.DataBind();

            ShowPanel(pnlContactInfo);
        }

        private void ShowPanel(Panel panel)
        {
            pnlYou.Visible = pnlYou.Equals(panel);
            pnlSpouse.Visible = pnlSpouse.Equals(panel);
            pnlFamilyMembers.Visible = pnlFamilyMembers.Equals(panel);
            pnlContactInfo.Visible = pnlContactInfo.Equals(panel);
        }

        #endregion

        #region Protected Methods

        protected string GetGrade(DateTime graduationDate)
        {
            return Person.CalculateGradeLevel(graduationDate, CurrentOrganization.GradePromotionDate).ToString();
        }

        #endregion
    }
}
