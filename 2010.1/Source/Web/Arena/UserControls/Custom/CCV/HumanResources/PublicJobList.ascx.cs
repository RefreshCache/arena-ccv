using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Custom.CCV.HumanResources;
using Arena.Portal;

namespace ArenaWeb.UserControls.Custom.HumanResources
{
    public partial class PublicJobList : PortalControl
    {
        [PageSetting("Job Posting Page", "This page will display the details of a job posting.", true)]
        public string JobPostingPageSetting { get { return Setting("JobPostingPage", "", true); } }

       protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptrJobs.DataSource = new JobPostingData().GetJobPostingList(true);
                rptrJobs.DataBind();
            }
        }
    }

}