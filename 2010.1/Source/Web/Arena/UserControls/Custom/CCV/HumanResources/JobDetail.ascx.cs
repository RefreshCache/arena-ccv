using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Custom.CCV.HumanResources;
using Arena.Portal;

namespace ArenaWeb.UserControls.Custom.HumanResources
{
    public partial class JobDetail : PortalControl
    {
        [PageSetting("Job Application Page", "The page for viewer to fill out a job application.", true)]
        public string JobApplicationPageSetting { get { return Setting("JobApplicationPage", "", true); } }

        [TextSetting("No Positions Text", "The text that will display if there are no positions available", true)]
        public string NoPositionsTextSetting { get { return Setting("NoPositionsText", "We're sorry, this job posting is not available. Please select another position.", true); } }

        private JobPosting _job = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _job = new JobPosting(GetJobPostingGuid());

                if (_job.JobPostingGuid != Guid.Empty)
                    ShowJobPosting();
                else
                    ShowNoneAvailable();
            }
			
			//Set Custom Meta Description for SEO & Social network optimization
			Utilities.AddMetaTag(Page, "description", "CCV has an employment opportunity for: "+_job.Title);
        }

        private Guid GetJobPostingGuid()
        {
            Guid jobPostingGuid = Guid.Empty;
            string[] keys = Request.QueryString.AllKeys;

            foreach (string key in keys)
            {
                if (key.ToUpper().Equals("GUID"))
                {
                    try { jobPostingGuid = new Guid(Request.QueryString.Get(key)); }
                    catch { jobPostingGuid = Guid.Empty; }
                    break;
                }
            }

            return jobPostingGuid;
        }

        private void ShowJobPosting()
        {
            pnlJobPosting.Visible = true;
            pnlNoJobs.Visible = false;

            lJobTitle.Text = _job.Title;
            lJobDesc.Text = _job.Description;
            hlApply.NavigateUrl = string.Format("~/default.aspx?page={0}&guid={1}", JobApplicationPageSetting, HttpUtility.UrlEncode(_job.JobPostingGuid.ToString()));
        }

        private void ShowNoneAvailable()
        {
            pnlJobPosting.Visible = false;
            pnlNoJobs.Visible = true;
            lNoPositions.Text = NoPositionsTextSetting;
        }
    }
}