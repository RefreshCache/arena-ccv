namespace ArenaWeb.UserControls.Custom.CCV.Area
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Collections;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    
    using Arena.Core;
    using Arena.Exceptions;
	using Arena.Portal;
	using Arena.Portal.UI;
    using Arena.Security;
	using Arena.Utility;

	/// <summary>
	///		Summary description for RequestDetail.
	/// </summary>
    public partial class AreaLeadership : PortalControl
	{
		#region Module Settings

		// Module Settings
        [LookupSetting("Main Leader Role", "Those individuals with this role will be listed first and have their picture displayed.", true, "E499057B-85CE-41B9-9C2C-7A703C8756A7")]
        public string MainLeaderRoleSetting { get { return Setting("MainLeaderRole", "", true); } }

        [BooleanSetting("Show Team", "Flag indicating if the remaining leadership team should be displayed", false, true)]
        public string ShowTeamSetting { get { return Setting("ShowTeam", "True", false); } }

        [TextSetting("Team Caption", "The heading to use above the remaining leadership roles (default='Leadership Team').", false)]
        public string TeamCaptionSetting { get { return Setting("TeamCaption", "Leadership Team", false); } }

        [TextSetting("Page Title", "This module will set the page title based on the text you enter her.  Any occurrence of '##AREA_NAME##' will be replaced with the current Area's name. (default='Your Area is ##AREA_NAME##').", false)]
        public string PageTitleSetting { get { return Setting("PageTitle", "Your Area is ##AREA_NAME##", false); } }

        #endregion

        #region Private Variables

        private Area _area = null;

        #endregion

        #region Events

        protected void Page_Init(object sender, System.EventArgs e)
        {
            int AreaID = -1;

            string[] keys;
            keys = Request.QueryString.AllKeys;
            foreach (string key in keys)
            {
                switch (key.ToUpper())
                {
                    case "AREA":
                        try
                        {
                            AreaID = Int32.Parse(Request.QueryString.Get(key));
                        }
                        catch (System.Exception ex)
                        {
                            throw new ModuleException(CurrentPortalPage, CurrentModule, string.Format("The '{0}' AreaLeadership module requires a numeric Area ID.", CurrentModule.Title, ex));
                        }
                        break;
                }
            }

            _area = new Area(AreaID);
            //CurrentPortalPage.ContentContext = this.ContentContext;
        }

        protected void Page_Load(object sender, System.EventArgs e)
		{
            BasePage.AddJavascriptInclude(Page, "include/popupWindow.js");

            if (_area != null && _area.AreaID != -1)
            {
                CurrentPortalPage.TemplateControl.Title = PageTitleSetting.Replace("##AREA_NAME##", _area.Name);

                Lookup mainRole = new Lookup(Int32.Parse(MainLeaderRoleSetting));
                pnlLeaderHeading.Controls.Add(new LiteralControl(mainRole.Value));

                pnlTeamHeading.Controls.Add(new LiteralControl(TeamCaptionSetting));

                tdImages.Controls.Clear();

                foreach (AreaOutreachCoordinator leader in _area.OutreachCoordinators)
                {
                    Person person = new Person(leader.PersonId);

                    if (leader.AreaRole.LookupID == mainRole.LookupID)
                    {
                        if (person.Blob != null && person.Blob.ByteArray != null && person.Blob.ByteArray.Length > 0)
                        {
                            tdImages.Controls.Add(new LiteralControl(string.Format(
                                "<img border=\"0\" src=\"cachedblob.aspx?guid={0}&width=200&height=200\" alt=\"{1}\">",
                                person.Blob.GUID.ToString(),
                                person.FullName)));
                        }

                        tblLeaders.Rows.Add(LeaderRow(leader, person, false, false));
                    }
                    else
                    {
                        tblTeam.Rows.Add(LeaderRow(leader, person, true, true));
                    }
                }
            }
            else
            {
                throw new ModuleException(CurrentPortalPage, CurrentModule, string.Format("The '{0}' AreaLeadership module requires a valid Area ID", CurrentModule.Title));
            }

            tdTeam.Visible = Boolean.Parse(ShowTeamSetting);
        }

        #endregion

        #region Private Methods

        private TableRow LeaderRow(AreaOutreachCoordinator leader, Person person, bool includeImage, bool includeRole)
        {
            TableRow row = new TableRow();
            row.VerticalAlign = VerticalAlign.Top;

            TableCell cell = new TableCell();
            row.Cells.Add(cell);
            cell.VerticalAlign = VerticalAlign.Top;
            if (includeImage && person.Blob != null && person.Blob.ByteArray != null && person.Blob.ByteArray.Length > 0)
                cell.Controls.Add(new LiteralControl(string.Format(
					"<img border=\"0\" src=\"images/photo_portrait.gif\" onmouseover=\"javascript:popupBlob('{0}');\" onmousemove=\"javascript:get_mouse(event);\" onmouseout=\"javascript:kill();\">",
                    person.Blob.GUID.ToString())));

            StringBuilder sb = new StringBuilder();
            if (person.Emails.FirstActive != string.Empty)
                sb.AppendFormat("<a href='mailto:{0}'>{1}</a>", person.Emails.FirstActive, person.FullName);
            else
                sb.Append(person.FullName);
            if (includeRole)
                sb.AppendFormat(" ({0})", leader.AreaRole.Value);

            cell = new TableCell();
            row.Cells.Add(cell);
            cell.VerticalAlign = VerticalAlign.Top;
            cell.Controls.Add(new LiteralControl(sb.ToString()));

            return row;
        }

        #endregion

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


        #region iContentContextPublisher Members

        //public ContentContext ContentContext
        //{
        //    get
        //    {
        //        if (_area == null)
        //            throw new ArgumentNullException("Area");
        //        return new ContentContext("core_area", _area.GUID);
        //    }
        //}

        #endregion
    }
}
