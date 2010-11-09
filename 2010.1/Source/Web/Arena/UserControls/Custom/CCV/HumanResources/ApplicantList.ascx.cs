using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Arena.Core;
using Arena.Enums;
using Arena.Custom.CCV.Core.Communications;
using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Custom.CCV.HumanResources;
using Arena.Portal;
using Arena.Security;

namespace ArenaWeb.UserControls.Custom.HumanResources
{
    public partial class ApplicantList : PortalControl
    {
        #region Module Settings

        [PageSetting("Applicant Detail Page", "The page that is used for displaying applicant details.", true)]
        public string ApplicantDetailPageIDSetting { get { return Setting("ApplicantDetailPageID", "", true); } }

        [PageSetting("Job Posting List Page", "The page that displays open job postings.", true)]
        public string JobPostingListPageIDSetting { get { return Setting("JobPostingListPageID", "", true); } }

        #endregion

        #region Private Fields

        private bool _editEnabled = false;

        #endregion

        #region Page Events

        protected void Page_Init(object sender, EventArgs e)
        {
            dgApplicants.ItemDataBound += new DataGridItemEventHandler(dgApplicants_ItemDataBound);
            dgApplicants.ItemCommand += new DataGridCommandEventHandler(dgApplicants_ItemCommand);
            dgApplicants.EditCommand += new DataGridCommandEventHandler(dgApplicants_EditCommand);
            dgApplicants.DeleteCommand += new DataGridCommandEventHandler(dgApplicants_DeleteCommand);
            dgApplicants.ReBind += new Arena.Portal.UI.DataGridReBindEventHandler(dgApplicants_ReBind);
            btnApply.Click += new EventHandler(btnApply_Click);
            btnSendEmail.Click += new EventHandler(btnSendEmail_Click);
            btnReviewed.Click += new EventHandler(btnReviewed_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _editEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);

            if (!Page.IsPostBack)
            {
                GetOpenPositions();
                ddlPostings.Items.Insert(0, new ListItem("All Positions", "-1"));
                LoadSettings();
                ShowList();
                btnSendEmail.Attributes.Add("onclick", "validateSend(event);");
                btnReviewed.Attributes.Add("onclick", "validateReview();");
            }

            RegisterScripts();
        }

        #endregion

        #region Private Methods

        private void RegisterScripts()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("\n\n<script type=\"text/javascript\">\n");
            sbScript.Append("\tfunction SaveSelectList(includeList)\n");
            sbScript.Append("\t{\n");
            sbScript.AppendFormat("\t\tdocument.frmMain.{0}.value = includeList;\n", ihIncludeList.ClientID);
            sbScript.Append("\t}\n");

            sbScript.Append("\tfunction validateSend(e)\n");
            sbScript.Append("\t{\n");
            sbScript.AppendFormat("\t\tvar includeList = document.getElementById(\"{0}\").value;\n", ihIncludeList.ClientID);
            sbScript.Append("\t\tif (includeList == '')\n");
            sbScript.Append("\t\t\talert(\"No applicants have been selected!\");\n");
            sbScript.Append("\t\telse\n");
            sbScript.Append("\t\t{\n");
            sbScript.Append("\t\t\tvar confirmText = \"Are you sure you want to send these rejection notices?\";\n");
            sbScript.Append("\t\t\tif (window.event)  // ie client\n");
            sbScript.Append("\t\t\t\tevent.returnValue = confirm(confirmText);\n");
            sbScript.Append("\t\t\telse\n");
            sbScript.Append("\t\t\t{\n");
            sbScript.Append("\t\t\t\tif (!confirm(confirmText))\n");
            sbScript.Append("\t\t\t\t\t e.preventDefault();\n");
            sbScript.Append("\t\t\t}\n");
            sbScript.Append("\t\t}\n");
            sbScript.Append("\t}\n");

            sbScript.Append("\tfunction validateReview()\n");
            sbScript.Append("\t{\n");
            sbScript.AppendFormat("\t\tvar includeList = document.getElementById(\"{0}\").value;\n", ihIncludeList.ClientID);
            sbScript.Append("\t\tif (includeList == '')\n");
            sbScript.Append("\t\t\talert(\"No applicants have been selected!\");\n");
            sbScript.Append("\t}\n");
            sbScript.Append("</script>\n\n");

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sendMail", sbScript.ToString());
        }

        private void GetOpenPositions()
        {
            ddlPostings.DataSource = new JobPostingData().GetJobPostingList(false);
            ddlPostings.DataTextField = "title";
            ddlPostings.DataValueField = "job_posting_id";
            ddlPostings.DataBind();
        }

        private Guid GetJobPostingGuid()
        {
            Guid jobPostingGuid = Guid.Empty;
            string[] keys = Request.QueryString.AllKeys;

            foreach (string key in keys)
                if (key.ToUpper() == "GUID")
                {
                    try { jobPostingGuid = new Guid(Request.QueryString.Get(key)); }
                    catch { jobPostingGuid = Guid.Empty; }
                    break;
                }

            return jobPostingGuid;
        }

        private void LoadSettings()
        {
            JobPosting posting = new JobPosting(GetJobPostingGuid());

            foreach (ListItem item in ddlPostings.Items)
                if (int.Parse(item.Value) == posting.JobPostingID)
                {
                    item.Selected = true;
                    break;
                }

            bool noticeSent = false;
            bool.TryParse(CurrentPerson.Settings["ApplicantList_NoticeSent"], out noticeSent);
            cbRejectionSent.Checked = noticeSent;

            bool reviewed = false;
            bool.TryParse(CurrentPerson.Settings["ApplicantList_ReviewedByHR"], out reviewed);
            cbReviewed.Checked = reviewed;
        }

        private void SaveSettings()
        {
            CurrentPerson.Settings["ApplicantList_NoticeSent"] = cbRejectionSent.Checked.ToString();
            CurrentPerson.Settings["ApplicantList_ReviewedByHR"] = cbReviewed.Checked.ToString();
            CurrentPerson.Settings.Save(CurrentPerson.PersonID, CurrentOrganization.OrganizationID);
        }

        private void ShowList()
        {
            pnlList.Visible = true;

            HyperLinkColumn hlc = (HyperLinkColumn)dgApplicants.Columns[3];
            hlc.DataNavigateUrlFormatString = "~/default.aspx?page=" + ApplicantDetailPageIDSetting + "&guid={0}";

            dgApplicants.Visible = true;
            dgApplicants.ItemType = "Job Applicant";
            dgApplicants.ItemBgColor = CurrentPortalPage.Setting("ItemBgColor", string.Empty, false);
            dgApplicants.ItemAltBgColor = CurrentPortalPage.Setting("ItemAltBgColor", string.Empty, false);
            dgApplicants.ItemMouseOverColor = CurrentPortalPage.Setting("ItemMouseOverColor", string.Empty, false);
            dgApplicants.AddEnabled = false;
            dgApplicants.EditEnabled = false;
            dgApplicants.DeleteEnabled = _editEnabled;
            dgApplicants.EditOverride = true;
            dgApplicants.MergeEnabled = false;
            dgApplicants.MailEnabled = false;
            dgApplicants.ExportEnabled = true;
            dgApplicants.AllowSorting = true;
            dgApplicants.DataSource = new JobApplicantData().GetJobApplicantList_DT(int.Parse(ddlPostings.SelectedValue), cbRejectionSent.Checked, cbReviewed.Checked);
            dgApplicants.DataBind();
        }

        private void SendMail(string firstName, string lastName, string email, string jobPosting, DateTime dateApplied)
        {
            ApplicantRejectionNotification notice = new ApplicantRejectionNotification();

            if (notice.TemplateID != -1)
            {
                Dictionary<string, string> fields = new Dictionary<string,string>();
                fields.Add("##ApplicantFirstName##", firstName);
                fields.Add("##ApplicantLastName##", lastName);
                fields.Add("##ApplicantEmail##", email);
                fields.Add("##JobPostingName##", jobPosting);
                fields.Add("##DateApplied##", dateApplied.ToShortDateString());
                notice.Send(email, fields);
            }
        }

        #endregion

        #region Events

        private void dgApplicants_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            ListItemType li = (ListItemType)e.Item.ItemType;

            if (li == ListItemType.Item || li == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;

                System.Web.UI.WebControls.Image cbImage = (System.Web.UI.WebControls.Image)e.Item.FindControl("imgSelect");
                if (cbImage != null)
                    cbImage.Attributes["applicant_id"] = row["applicant_id"].ToString();
            }
        }

        private void dgApplicants_ReBind(object sender, EventArgs e)
        {
            ShowList();
        }

        private void dgApplicants_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName != "")
                ShowList();
        }

        private void dgApplicants_EditCommand(object source, DataGridCommandEventArgs e)
        {
            JobApplicant applicant = new JobApplicant(int.Parse(e.Item.Cells[0].Text));
            Response.Redirect(string.Format("~/default.aspx?page={0}&guid={1}",
                    ApplicantDetailPageIDSetting, applicant.Guid.ToString()));
        }

        private void dgApplicants_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            new JobApplicantData().DeleteJobApplicant(int.Parse(e.Item.Cells[0].Text));
            ShowList();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ShowList();
            SaveSettings();
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            string[] sApplicants = ihIncludeList.Value.Trim().Split(',');

            foreach (string id in sApplicants)
                if (id.Trim() != string.Empty)
                {
                    try
                    {
                        JobApplicant applicant = new JobApplicant(int.Parse(id));
                        SendMail(applicant.FirstName, applicant.LastName, applicant.Email, applicant.JobPosting.Title, applicant.DateCreated);
                        applicant.RejectionNoticeSent = true;
                        applicant.Save(CurrentUser.Identity.Name, false);
                        ihIncludeList.Value = string.Empty;
                    }
                    catch { }
                }

            ShowList();
        }

        private void btnReviewed_Click(object sender, EventArgs e)
        {
            string[] sApplicants = ihIncludeList.Value.Trim().Split(',');

            foreach (string id in sApplicants)
                if (id.Trim() != string.Empty)
                {
                    try
                    {
                        JobApplicant applicant = new JobApplicant(int.Parse(id));
                        applicant.ReviewedByHR = true;
                        applicant.Save(CurrentUser.Identity.Name, false);
                        ihIncludeList.Value = string.Empty;
                    }
                    catch { }
                }

            ShowList();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/default.aspx?page={0}&parCurrentGroup=Finance", JobPostingListPageIDSetting));
        }

        #endregion
    }
}