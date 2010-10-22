namespace ArenaWeb.UserControls.Custom.CCV.Area
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;

    using Arena.Portal;

    public partial class AreaRedirect : PortalControl
    {
        [PageSetting("Area Detail Page", "The page to display when a user's area can be determined.", true)]
        public string AreaDetailPageSetting { get { return Setting("AreaDetailPage", "", true); } }

        [PageSetting("Area Locator Page", "The page to display when a user's area cannot be determined.", true)]
        public string AreaLocatorPageSetting { get { return Setting("AreaLocatorPage", "", true); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPerson != null && 
                CurrentPerson.PersonID != -1 &&
                CurrentPerson.PrimaryAddress != null &&
                CurrentPerson.PrimaryAddress.Area != null &&
                CurrentPerson.PrimaryAddress.Area.AreaID != -1)
                Response.Redirect(string.Format("default.aspx?page={0}&area={1}", AreaDetailPageSetting, CurrentPerson.PrimaryAddress.Area.AreaID.ToString()));
            else
                Response.Redirect(string.Format("default.aspx?page={0}", AreaLocatorPageSetting));

        }
    }
}
