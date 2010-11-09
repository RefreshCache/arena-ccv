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
	public partial class MyTags : PortalControl
	{
        [PageSetting("Profile Detail Page", "The page that should be used to display profile details.", false)]
        public string ProfileDetailPageIDSetting { get { return Setting("ProfileDetailPageID", "25", false); } }

        [PageSetting("Event Detail Page", "The page that should be used to display event details.", false)]
        public string EventDetailPageIDSetting { get { return Setting("EventDetailPageID", "376", false); } }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            ProfileCollection myProfiles = new ProfileCollection();
            SqlDataReader rdr = new Arena.DataLayer.Core.ProfileData().GetProfileByOwnerID(CurrentOrganization.OrganizationID, CurrentPerson.PersonID, true);
            while (rdr.Read())
                myProfiles.Add(new Profile(rdr));
            rdr.Close();

            rptTags.DataSource = myProfiles;
            rptTags.DataBind();
		}

        void rptTags_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Profile myProfile = (Profile)e.Item.DataItem;
                HyperLink hlTag = (HyperLink)e.Item.FindControl("hlTag");

                string pageId = ProfileDetailPageIDSetting;
                if (myProfile.ProfileType == Arena.Enums.ProfileType.Event)
                    pageId = EventDetailPageIDSetting;

                hlTag.NavigateUrl = string.Format("~/default.aspx?page={0}&profile={1}", pageId, myProfile.ProfileID.ToString());
                hlTag.Text = myProfile.Title;
                hlTag.ToolTip = myProfile.NavigationCaption;
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
            rptTags.ItemDataBound += new RepeaterItemEventHandler(rptTags_ItemDataBound);
		}

		#endregion
	}
}
