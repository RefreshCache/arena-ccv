namespace ArenaWeb.UserControls.Custom.CCV.SmallGroup
{
	using System;
	using System.Data;
	using System.Drawing;
    using System.Text;
	using System.Web;
    using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using Arena.Core;
	using Arena.SmallGroup;
	using Arena.Enums;
	using Arena.Portal;
	using Arena.Exceptions;
	using Arena.DataLayer.SmallGroup;

	/// <summary>
	///		Summary description for ServingOppConfirmation.
	/// </summary>
	public partial class GroupRegistrationAdd : PortalControl
	{
        #region Module Settings

        #endregion

        #region Private Variables

        private Person person = null;
        private Person spouse = null;
        private Group group = null;

        #endregion 

        protected void Page_Load(object sender, System.EventArgs e)
		{
            string[] keys;
            keys = Request.QueryString.AllKeys;
            foreach (string key in keys)
            {
                switch (key.ToUpper())
                {
                    case "GROUP":
                        try { group = new Group(Int32.Parse(Request.QueryString.Get(key))); }
                        catch { }
                        break;
                    case "PERSON":
                        try { person = new Person(new Guid(Request.QueryString.Get(key))); }
                        catch { }
                        break;
                    case "SPOUSE":
                        try { spouse = new Person(new Guid(Request.QueryString.Get(key))); }
                        catch { }
                        break;
                }
            }

            if (group != null && group.GroupID != -1 && person != null && person.PersonID != -1)
            {
                GroupMember gMember = new GroupMember(group.GroupID, person.PersonID);
                if (gMember.GroupID == -1)
                {
                    bool exists = false;
                    bool spouseExists = false;
                    foreach (Registration registration in group.Registrations)
                        foreach (Person registrant in registration.Persons)
                        {
                            if (registrant.PersonID == person.PersonID)
                                exists = true;
                            if (spouse != null && registrant.PersonID == spouse.PersonID)
                                spouseExists = true;
                        }

                    if (!exists)
                    {
                        Registration registration = new Registration();
                        registration.OrganizationID = CurrentOrganization.OrganizationID;
                        registration.ClusterType = group.ClusterType;
                        registration.GroupID = group.GroupID;
                        registration.GroupType = group.GroupType;
                        registration.Notes = "User-Entered Registration";
                        registration.ClusterID = group.GroupClusterID;
                        
                        if (group.ClusterType.UnassignedRegistrationLevel >= 0)
                        {
                            GroupCluster gc = group.GroupCluster;
                            while (gc != null && gc.ParentClusterID != -1 && gc.ClusterLevel.Level > group.ClusterType.UnassignedRegistrationLevel)
                                gc = new GroupCluster(gc.ParentClusterID);
                            if (gc.ClusterLevel.Level == group.ClusterType.UnassignedRegistrationLevel)
                                registration.ClusterID = gc.GroupClusterID;
                        }

                        registration.Persons.Add(person);

                        // Add Spouse if specified
                        if (spouse != null && !spouseExists)
                            registration.Persons.Add(spouse);

                        registration.Save(CurrentOrganization.OrganizationID, CurrentUser.Identity.Name);

                        foreach (Person rPerson in registration.Persons)
                        {
                            PersonHistory history = new PersonHistory();
                            history.PersonID = rPerson.PersonID;
                            history.HistoryType = new Lookup(SystemLookup.PersonHistoryType_SmallGroupRegistration);
                            history.HistoryQualifierID = registration.RegistrationID;
                            history.Text = "User registered for group: " + group.Title;
                            history.Save(CurrentOrganization.OrganizationID);
                        }
                    }
                }
            }
            //else
            //{
            //    throw new ModuleException(CurrentPortalPage, CurrentModule, "The GroupRegistrationAdd module requires a valid Group and Person!");
            //}
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
		}
		#endregion
	}
}
