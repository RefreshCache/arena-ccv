namespace ArenaWeb.UserControls.Custom.CCV.Phone
{
	using System;
	using System.Xml;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
    using System.Text;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

    using Arena.Core;
    using Arena.Enums;
    using Arena.Exceptions;
    using Arena.Phone;
	using Arena.Portal;
	using Arena.Portal.UI;

	/// <summary>
	///		Summary description for SubscribedProfileList.
	/// </summary>
	public partial class SpeedDialer : PortalControl
	{
        #region Module Settings

        [PageSetting("Person Detail Page", "The page that is used for displaying person details.", false, 7)]
        public string PersonDetailPageIDSetting { get { return Setting("PersonDetailPageID", "7", false); } }

        #endregion

        #region Private Variables

        LookupCollection extensionRules = null;
        Lookup PBXSystem = null;
        PBXManager manager = null;

        #endregion
        
        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (CurrentPerson == null || CurrentPerson.PersonID == -1)
                throw new ArenaApplicationException("SpeedDialer module requires that a user be logged in.  Please configure security so that only 'registered' users have access to this page.");

            if (CurrentPerson.Peers.Count > 0)
            {
                string phoneTagName = CurrentOrganization.Settings["PBXPersonalListTagName"];
                if (phoneTagName != null)
                {
                    ProfileCollection personalTags = new ProfileCollection();
                    personalTags.LoadChildProfileHierarchy(-1, CurrentOrganization.OrganizationID, ProfileType.Personal, CurrentPerson.PersonID);
                    Arena.Core.Profile contactList = GetContactList(personalTags, phoneTagName.Trim().ToLower());

                    if (contactList != null)
                    {
                        lTagName.Text = contactList.Title;

                        extensionRules = new LookupType(SystemLookupType.PhoneInternalExtensionRules).Values;
                        PBXSystem = PBXHelper.DefaultPBXSystem(CurrentOrganization);
                        manager = PBXHelper.GetPBXClass(PBXSystem);

                        lvContacts.DataSource = contactList.Members;
                        lvContacts.DataBind();
                    }
                    else
                        this.Visible = false;
                }
                else
                    this.Visible = false;
            }
            else
                this.Visible = false;
        }

        void lvContacts_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                ProfileMember pm = (ProfileMember)dataItem.DataItem;

                PersonPhone hp = pm.Phones.FindByType(SystemLookup.PhoneType_Home);
                if (hp != null && hp.Number.Trim() != string.Empty)
                {
                    Literal lCtcHome = (Literal)e.Item.FindControl("lCtcHome");
                    lCtcHome.Text = string.Format("<img src=\"images/home.gif\" style=\"cursor:pointer\" onclick=\"ClickToCall.MakeCall({0},'{1}','{2}','{3}');\"/>",
                        PBXSystem.LookupID.ToString(),
                        CurrentPerson.PeerChannel.Replace("'", "\\'"),
                        hp.Number.Replace("'", "\\'"),
                        CurrentPerson.FullName.Replace("'", "\\'"));
                }

                PersonPhone bp = pm.Phones.FindByType(SystemLookup.PhoneType_Business);
                if (bp != null && bp.Number.Trim() != string.Empty)
                {
                    Literal lCtcWork = (Literal)e.Item.FindControl("lCtcWork");
                    lCtcWork.Text = string.Format("<img src=\"images/business.gif\" style=\"cursor:pointer\" onclick=\"ClickToCall.MakeCall({0},'{1}','{2}','{3}');\"/>",
                        PBXSystem.LookupID.ToString(),
                        CurrentPerson.PeerChannel.Replace("'", "\\'"),
                        bp.Number.Replace("'", "\\'"),
                        CurrentPerson.FullName.Replace("'", "\\'"));
                }

                PersonPhone cp = pm.Phones.FindByType(SystemLookup.PhoneType_Cell);
                if (cp != null && cp.Number.Trim() != string.Empty)
                {
                    Literal lCtcCell = (Literal)e.Item.FindControl("lCtcCell");
                    lCtcCell.Text = string.Format("<img src=\"images/cell.gif\" style=\"cursor:pointer\" onclick=\"ClickToCall.MakeCall({0},'{1}','{2}','{3}');\"/>",
                        PBXSystem.LookupID.ToString(),
                        CurrentPerson.PeerChannel.Replace("'", "\\'"),
                        cp.Number.Replace("'", "\\'"),
                        CurrentPerson.FullName.Replace("'", "\\'"));
                }

                HyperLink hlName = (HyperLink)e.Item.FindControl("hlName");
                hlName.NavigateUrl = string.Format("~/default.aspx?page={0}&guid={1}",
                    PersonDetailPageIDSetting,
                    pm.PersonGUID.ToString());
                hlName.Text = string.Format("{0}, {1}", pm.LastName, pm.NickName);
           }
        }

        protected Profile GetContactList(ProfileCollection profiles, string tagName)
        {
            foreach (Profile profile in profiles)
                if (profile.Name.Trim().ToLower() == tagName)
                    return profile;

            foreach (Profile profile in profiles)
            {
                Arena.Core.Profile contactList = GetContactList(profile.ChildProfiles, tagName);
                if (contactList != null)
                    return contactList;
            }

            return null;
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
            lvContacts.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvContacts_ItemDataBound);
		}
		#endregion
	}
}
