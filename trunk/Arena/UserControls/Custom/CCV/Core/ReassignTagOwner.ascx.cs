namespace ArenaWeb.UserControls.Custom.CCV.Core
{
	using System;
	using System.Xml;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Arena.Exceptions;
	using Arena.Portal;
	using Arena.Portal.UI;
	using Arena.Core;

	/// <summary>
	///		Summary description for SubscribedProfileList.
	/// </summary>
    public partial class ReassignTagOwner : PortalControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                // Load Staff Names
                ddlNewOwner.Items.Add(new ListItem("", "-1"));
                string query = @"
                    SELECT DISTINCT
                        P.person_id,
                        P.last_name + ', ' + P.nick_name AS person_name
                    FROM core_person P
                    WHERE P.staff_member = 1
                    ORDER BY person_name";
                SqlDataReader rdr = new Arena.DataLayer.Organization.OrganizationData().ExecuteReader(query);
                while (rdr.Read())
                    ddlNewOwner.Items.Add(new ListItem(rdr["person_name"].ToString(), rdr["person_id"].ToString()));
                rdr.Close();

                LoadCurrentOwners();
            }
		}

        void ddlCurrentOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTags(ddlCurrentOwner.SelectedValue);
        }

        void btnReassign_Click(object sender, EventArgs e)
        {
            if (ddlCurrentOwner.SelectedValue != "" &&
                ddlCurrentOwner.SelectedValue != "-1" &&
                ddlNewOwner.SelectedValue != "" &&
                ddlTag.SelectedValue != "")
            {
                Arena.Core.Profile profile = new Profile(Int32.Parse(ddlTag.SelectedValue));
                UpdateTagOwner(profile, 
                    Int32.Parse(ddlCurrentOwner.SelectedValue), 
                    new Person(Int32.Parse(ddlNewOwner.SelectedValue)));
            }
            LoadCurrentOwners();
        }

        void LoadCurrentOwners()
        {
            string currentValue = ddlCurrentOwner.SelectedValue;

            ddlCurrentOwner.Items.Clear();
            ddlCurrentOwner.Items.Add(new ListItem("", "-1"));

            // Load Current Owners
            string query = @"
                    SELECT DISTINCT
                        P.person_id,
                        P.last_name + ', ' + P.nick_name AS person_name
                    FROM core_profile T
                    LEFT OUTER JOIN evnt_event_profile E on E.profile_id = T.profile_id
                    INNER JOIN core_person P ON P.person_id = T.owner_id
                    WHERE (T.profile_type in (1,2) OR
	                    (T.profile_type = 4 and E.[end] > getdate())
	                    )
                    ORDER BY person_name";
            SqlDataReader rdr = new Arena.DataLayer.Organization.OrganizationData().ExecuteReader(query);
            while (rdr.Read())
            {
                ListItem li = new ListItem(rdr["person_name"].ToString(), rdr["person_id"].ToString());
                li.Selected = li.Value == currentValue;
                ddlCurrentOwner.Items.Add(li);
            }
            rdr.Close();

            LoadTags(ddlCurrentOwner.SelectedValue);
        }

        void LoadTags(string ownerId)
        {
            ddlTag.Items.Clear();

            if (ownerId != "" && ownerId != "-1")
            {
                // Load Tags
                string query = @"
                    SELECT DISTINCT
                        T.profile_id,
                        T.profile_type,
                        dbo.cust_ccv_profile_path(T.profile_id) as profile_path
                    FROM core_profile T
                    LEFT OUTER JOIN evnt_event_profile E on E.profile_id = T.profile_id
                    WHERE (T.profile_type in (1,2) OR
	                    (T.profile_type = 4 and E.[end] > getdate())
	                    )
                    AND T.owner_id = " + ownerId + @" 
                    ORDER BY T.profile_type, profile_path";
                SqlDataReader rdr = new Arena.DataLayer.Organization.OrganizationData().ExecuteReader(query);
                while (rdr.Read())
                {
                    string profileName = rdr["profile_path"].ToString();

                    switch ((int)rdr["profile_type"])
                    {
                        case 1: profileName = "[Ministry] " + profileName; break;
                        case 2: profileName = "[Serving] " + profileName; break;
                        case 4: profileName = "[Event] " + profileName; break;
                    }

                    ddlTag.Items.Add(new ListItem(profileName, rdr["profile_id"].ToString()));

                }
                rdr.Close();
            }
        }

        void UpdateTagOwner(Profile profile, int currentOwnerID, Person newOwner)
        {
            if (profile.Owner.PersonID == currentOwnerID)
            {
                profile.Owner = newOwner;
                profile.Save(CurrentUser.Identity.Name);
            }

            foreach (Profile childProfile in profile.ChildProfiles)
                UpdateTagOwner(childProfile, currentOwnerID, newOwner);
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
            ddlCurrentOwner.SelectedIndexChanged += new EventHandler(ddlCurrentOwner_SelectedIndexChanged);
            btnReassign.Click += new EventHandler(btnReassign_Click);
		}

		#endregion
	}
}
