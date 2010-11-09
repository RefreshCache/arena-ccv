namespace ArenaWeb.UserControls.Custom.CCV.Core
{
	using System;
    using System.Collections;
    using System.Collections.Generic;
	using System.Data;
    using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.Security;
    using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;

    using Arena.Core;
    using Arena.Event;
	using Arena.Portal;
	using Arena.Exceptions;
    using Arena.DataLayer.Organization;

    public partial class NextSteps : PortalControl
	{
        #region Module Settings

        // Module Settings
        [TagSetting("Starting Point Tag", "The Parent Starting Point Tag", true)]
        public string StartingPointTagSetting { get { return Setting("StartingPointTag", "-1", true); } }

        [TagSetting("Baptism Tag", "The Parent Baptism Tag", true)]
        public string BaptismTagSetting { get { return Setting("BaptismTag", "-1", true); } }

        [TagSetting("Foundations Tag", "The Parent Foundations Tag", true)]
        public string FoundationsTagSetting { get { return Setting("FoundationsTag", "-1", true); } }

        [NumericSetting("Group Category", "The Category of groups to list", true)]
        public string GroupCategorySetting { get { return Setting("GroupCategory", "-1", true); } }

        #endregion

        private Person person = null;
        private int StartingPointProfileID = -1;
        private int BaptismProfileID = -1;
        private int FoundationsProfileID = -1;
        private int GroupCategoryID = -1;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                StartingPointProfileID = Convert.ToInt32(StartingPointTagSetting.Split('|')[1]);
                BaptismProfileID = Convert.ToInt32(BaptismTagSetting.Split('|')[1]);
                FoundationsProfileID = Convert.ToInt32(FoundationsTagSetting.Split('|')[1]);
                GroupCategoryID = Convert.ToInt32(GroupCategorySetting);

                person = new Person(new Guid(Request.QueryString["Guid"]));
                mpAddNextStep.Title = string.Format("Add {0} To", person.NickName);
            }
            catch { }

            if (!Page.IsPostBack)
                UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (person == null)
                this.Visible = false;
            else
            {
                this.Visible = true;
                OrganizationData oData = new OrganizationData();

                ArrayList lst = new ArrayList();
                lst.Add(new SqlParameter("Guid", Request.QueryString["Guid"]));

                SqlDataReader rdr = oData.ExecuteReader("cust_ccv_sp_personInvolvement", lst);
                if (rdr.Read())
                {
                    if (rdr["starting_point_attended"] != DBNull.Value)
                        img2.ImageUrl = "~/images/NextSteps/2e.jpg";
                    else if (Registered(person.PersonID, StartingPointProfileID))
                        img2.ImageUrl = "~/images/NextSteps/2r.jpg";
                    else
                        img2.ImageUrl = "~/images/NextSteps/2d.jpg";

                    if (rdr["baptism_date"] != DBNull.Value)
                        img3.ImageUrl = "~/images/NextSteps/3e.jpg";
                    else if (Registered(person.PersonID, BaptismProfileID))
                        img3.ImageUrl = "~/images/NextSteps/3r.jpg";
                    else
                        img3.ImageUrl = "~/images/NextSteps/3d.jpg";

                    if (rdr["bible_date"] != DBNull.Value)
                        img4_1.ImageUrl = "~/images/NextSteps/4-1e.jpg";
                    else
                        img4_1.ImageUrl = "~/images/NextSteps/4-1d.jpg";

                    if (rdr["worldview_date"] != DBNull.Value)
                        img4_2.ImageUrl = "~/images/NextSteps/4-2e.jpg";
                    else
                        img4_2.ImageUrl = "~/images/NextSteps/4-2d.jpg";

                    if (rdr["evangelism_date"] != DBNull.Value)
                        img4_3.ImageUrl = "~/images/NextSteps/4-3e.jpg";
                    else
                        img4_3.ImageUrl = "~/images/NextSteps/4-3d.jpg";

                    if (rdr["neighborhood_date"] != DBNull.Value)
                        img4_4.ImageUrl = "~/images/NextSteps/4-4e.jpg";
                    else
                        img4_4.ImageUrl = "~/images/NextSteps/4-4d.jpg";

                    if (rdr["bible_date"] != DBNull.Value ||
                        rdr["worldview_date"] != DBNull.Value ||
                        rdr["evangelism_date"] != DBNull.Value ||
                        rdr["neighborhood_date"] != DBNull.Value)
                        img4.ImageUrl = "~/images/NextSteps/4e.jpg";
                    else if (Registered(person.PersonID, FoundationsProfileID))
                        img4.ImageUrl = "~/images/NextSteps/4r.jpg";
                    else
                        img4.ImageUrl = "~/images/NextSteps/4d.jpg";

                    if (rdr["neighborhood_role"] != DBNull.Value)
                        img5.ImageUrl = "~/images/NextSteps/5e.jpg";
                    else
                    {
                        if (rdr["neighborhood_registered"].ToString() == "1")
                            img5.ImageUrl = "~/images/NextSteps/5r.jpg";
                        else
                            img5.ImageUrl = "~/images/NextSteps/5d.jpg";
                    }

                    if (rdr["transformation_role"] != DBNull.Value)
                        img6.ImageUrl = "~/images/NextSteps/6e.jpg";
                    else
                        img6.ImageUrl = "~/images/NextSteps/6d.jpg";
                }
                rdr.Close();
            }
        }

        void img7_Click(object sender, ImageClickEventArgs e)
        {
            LoadDropDowns();
            mpAddNextStep.Show();
        }

        void btnStartingPoint_Click(object sender, EventArgs e)
        {
            AddRegistration(ddlStartingPoint.SelectedItem);
        }

        void btnBaptism_Click(object sender, EventArgs e)
        {
            AddRegistration(ddlBaptism.SelectedItem);
        }

        void btnFoundations_Click(object sender, EventArgs e)
        {
            AddRegistration(ddlFoundations.SelectedItem);
        }

        void btnGroups_Click(object sender, EventArgs e)
        {
            if (ddlGroups.SelectedItem != null)
            {
                Arena.SmallGroup.Group group = new Arena.SmallGroup.Group(Int32.Parse(ddlGroups.SelectedItem.Value));

                Arena.SmallGroup.Registration registration = new Arena.SmallGroup.Registration();
                registration.Persons.Add(person);
                registration.GroupID = group.GroupID;
                registration.GroupType = group.GroupType;
                registration.ClusterID = group.GroupClusterID;
                registration.ClusterType = group.ClusterType;
                registration.Notes = "Added through Next Steps toolbar";
                registration.OrganizationID = person.OrganizationID;
                registration.Save(person.OrganizationID, CurrentUser.Identity.Name);

                UpdateDisplay();
            }
        }

        private bool Registered(int personId, int profileId)
        {
            bool registered = false;

            ArrayList lst = new ArrayList();
            lst.Add(new SqlParameter("PersonID", personId));
            lst.Add(new SqlParameter("ParentProfileID", profileId));

            SqlDataReader rdr = new OrganizationData().ExecuteReader("cust_ccv_sp_personInvolvementRegistered", lst);
            if (rdr.Read())
                if (rdr["start"] != DBNull.Value)
                    registered = true;
            rdr.Close();

            return registered;
        }

        private void LoadDropDowns()
        {
            AddChildEvents(StartingPointProfileID, ddlStartingPoint);
            AddChildEvents(BaptismProfileID, ddlBaptism);
            AddChildEvents(FoundationsProfileID, ddlFoundations);
            AddGroups();
        }

        private void AddChildEvents(int parentProfileID, DropDownList ddl)
        {
            ddl.Items.Clear();

            EventProfileCollection events = new EventProfileCollection();
            events.LoadProfileEventsByParentProfile(parentProfileID, CurrentOrganization.OrganizationID);

            foreach (EventProfile childEvent in events)
                if (childEvent.Active && childEvent.Start >= DateTime.Today)
                    ddl.Items.Insert(0,new ListItem(childEvent.Name, childEvent.ProfileID.ToString()));
        }

        private void AddGroups()
        {
            ddlGroups.Items.Clear();

            if (person.Area != null)
            {
                Arena.SmallGroup.GroupCollection groups = new Arena.SmallGroup.GroupCollection();
                groups.LoadByArea(person.Area.AreaID, GroupCategoryID);
                foreach (Arena.SmallGroup.Group group in groups)
                    ddlGroups.Items.Add(new ListItem(string.Format("{0} ({1})",
                        group.Title, group.MeetingDay.Value), group.GroupID.ToString()));
            }
        }

        private void AddRegistration(ListItem selectedItem)
        {
            if (selectedItem != null)
            {
                Registration Registration = new Registration();
                Registration.ProfileId = Convert.ToInt32(selectedItem.Value);
                Registration.OwnerId = person.PersonID;
                if (person.Emails.FirstActive != string.Empty)
                    Registration.CommunicationEmail = person.Emails.FirstActive;
                Registration.Save(CurrentUser.Identity.Name);

                Registrant registrant = new Registrant(Registration.ProfileId, Registration.OwnerId);
                if (registrant.RegistrationId == -1)
                {
                    registrant.RegistrationId = Registration.RegistrationId;
                    registrant.ProfileID = Registration.ProfileId;
                    registrant.PersonID = Registration.OwnerId;
                    registrant.Source = new LookupType(SystemLookupType.ProfileSource).Values[0];
                    registrant.Status = new Lookup(SystemLookup.TagMemberStatus_Connected);
                    registrant.DatePending = DateTime.Now;
                    registrant.Save(CurrentUser.Identity.Name);
                }

                Registration.SendConfirmation();

                UpdateDisplay();
            }
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
            img7.Click += new ImageClickEventHandler(img7_Click);
            btnStartingPoint.Click += new EventHandler(btnStartingPoint_Click);
            btnBaptism.Click += new EventHandler(btnBaptism_Click);
            btnFoundations.Click += new EventHandler(btnFoundations_Click);
            btnGroups.Click += new EventHandler(btnGroups_Click);
        }

        #endregion
    }
}
