using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Arena.Enums;
using Arena.Core;
using Arena.Custom.CCV.Core.Communications;
using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Custom.CCV.HumanResources;
using Arena.Portal;

namespace ArenaWeb.UserControls.Custom.HumanResources
{
    public partial class JobApplication : PortalControl
    {
        #region Module Settings

        [TextSetting("Recipient", "The email address that the job application will be sent to.", true)]
        public string RecipientSetting { get { return Setting("Recipient", string.Empty, true); } }

        [PageSetting("Thank You Page", "The page that this form will redirect to once the application has been submitted.", true)]
        public string ThankYouPageSetting { get { return Setting("ThankYouPage", "", true); } }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            JobPosting posting = new JobPosting(GetJobPostingGuid());

            if (!Page.IsPostBack)
            {
                tbPosition.Text = posting != null ? posting.Title : string.Empty;
                DropDownList[] ddls = { ddlBaptized, ddlClass100, ddlGroup, ddlMember, ddlServing, ddlTithe };

                foreach (DropDownList list in ddls)
                {
                    list.Items.Add(new ListItem("No", bool.FalseString));
                    list.Items.Add(new ListItem("Yes", bool.TrueString));
                }
            }
        }

        #region Private Methods

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

        private void SendMail(JobApplicant applicant)
        {
            JobApplicationNotification applicationMail = new JobApplicationNotification();

            if (applicationMail.TemplateID != -1)
            {
                Dictionary<string, string> fields = new Dictionary<string, string>();
                fields.Add("##JobPostingName##", tbPosition.Text);
                fields.Add("##FirstName##", applicant.FirstName);
                fields.Add("##LastName##", applicant.LastName);
                fields.Add("##Email##", applicant.Email);
                fields.Add("##HowDidYouHear##", applicant.HowHeard);
                fields.Add("##HowLongChristian##", applicant.HowLongChristian);
                fields.Add("##TakenClass100##", ddlClass100.SelectedItem.Text);  // display yes/no rather than true/false in email

                if (applicant.Class100Date != DateTime.Parse("1/1/1900"))
                    fields.Add("##Class100Date##", applicant.Class100Date.ToString());
                else
                    fields.Add("##Class100Date##", string.Empty);
                
                fields.Add("##Member##", ddlMember.SelectedItem.Text);
                fields.Add("##NeighborhoodGroup##", ddlGroup.SelectedItem.Text);
                fields.Add("##Serving##", ddlServing.SelectedItem.Text);
                fields.Add("##ServingMinistry##", applicant.ServingMinistry);
                fields.Add("##Baptized##", ddlBaptized.SelectedItem.Text);
                fields.Add("##Tithing##", ddlTithe.SelectedItem.Text);
                fields.Add("##Experience##", applicant.Experience);
                fields.Add("##LedToWorkAtCCV##", applicant.LedToApply);
                fields.Add("##Coverletter##", applicant.Coverletter);

                aspNetEmail.Attachment resume = new aspNetEmail.Attachment(fuResume.PostedFile.InputStream, fuResume.PostedFile.FileName);
                List<string> validExtensions = new List<string>() { ".doc", ".docx", ".pdf", ".rtf", ".txt" };

                if (validExtensions.Contains(Path.GetExtension(resume.FileName).ToLower()))
                {
                    List<aspNetEmail.Attachment> attachments = new List<aspNetEmail.Attachment>();
                    attachments.Add(resume);
                    applicationMail.Send(RecipientSetting, fields, attachments);
                }
            }
        }

        #endregion

        #region Events

         private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                JobApplicant applicant = new JobApplicant();
                applicant.JobPosting = new JobPosting(GetJobPostingGuid());
                applicant.FirstName = tbFirstName.Text;
                applicant.LastName = tbLastName.Text;
                applicant.Email = tbEmail.Text;
                applicant.HowHeard = tbHear.Text;
                applicant.HowLongChristian = tbChristian.Text;
                applicant.Class100 = bool.Parse(ddlClass100.SelectedValue);
                applicant.Class100Date = tbClass100.Text.Trim() != string.Empty ? DateTime.Parse(tbClass100.Text) : DateTime.Parse("1/1/1900");
                applicant.ChurchMember = bool.Parse(ddlMember.SelectedValue);
                applicant.NeighborhoodGroup = bool.Parse(ddlGroup.SelectedValue);
                applicant.Serving = bool.Parse(ddlServing.SelectedValue);
                applicant.ServingMinistry = tbServing.Text;
                applicant.Baptized = bool.Parse(ddlBaptized.SelectedValue);
                applicant.Tithing = bool.Parse(ddlTithe.SelectedValue);
                applicant.Experience = tbExperience.Text;
                applicant.LedToApply = tbLed.Text;
                applicant.Coverletter = tbCoverLetter.Text;

                applicant.Resume.ByteArray = fuResume.FileBytes;
                applicant.Resume.SetFileInfo(fuResume.PostedFile.FileName);
                applicant.Resume.Save(CurrentUser.Identity.Name);
                applicant.Save(CurrentUser.Identity.Name, true);

                SendMail(applicant);
                Response.Redirect(string.Format("~/default.aspx?pageid={0}", ThankYouPageSetting), true);
            }
        }

        #endregion
    }

}