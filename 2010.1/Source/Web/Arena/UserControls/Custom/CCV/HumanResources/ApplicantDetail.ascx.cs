using System;
using System.Collections;
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
//using Arena.Custom.CCV.Core;
using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Custom.CCV.HumanResources;
using Arena.Portal;
using Arena.Portal.UI;
using Arena.Security;

namespace ArenaWeb.UserControls.Custom.HumanResources
{
    public partial class ApplicantDetail : PortalControl
    {
        #region Module Settings

        [PageSetting("Person Detail Page", "The page that is used for displaying person details.", false, 7)]
        public string PersonDetailPageIDSetting { get { return Setting("PersonDetailPageID", "7", false); } }

        [PageSetting("Applicant List Page", "The page that is used for displaying applicant list.", true)]
        public string ApplicantListPageIDSetting { get { return Setting("ApplicantListPageID", "", true); } }

        [PageSetting("Person Popup Search Page", "The page that is used for the popup search.", false, 16)]
        public string PopupSearchWindowSetting { get { return Setting("PopupSearchWindow", "16", false); } }

        #endregion

        #region Private Members

        bool _editEnabled = false;
        bool _viewEnabled = false;

        #endregion

        #region Page Events

        protected void Page_Init(object sender, EventArgs e)
        {
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            lbRemovePerson.Click += new EventHandler(lbRemovePerson_Click);
            cbClass100.CheckedChanged += new EventHandler(cbClass100_CheckedChanged);
            cbServing.CheckedChanged += new EventHandler(cbServing_CheckedChanged);
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            _editEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);
            _viewEnabled = CurrentModule.Permissions.Allowed(OperationType.View, CurrentUser);
            RegisterScripts();

            if (!Page.IsPostBack)
            {
                if (!_editEnabled)
                {
                    tbFirstName.Enabled = false;
                    tbLastName.Enabled = false;
                    tbEmail.Enabled = false;
                    tbPosition.Enabled = false;
                    tbHeard.Enabled = false;
                    tbChristian.Enabled = false;
                    cbClass100.Enabled = false;
                    dtbClass100Date.Enabled = false;
                    cbMember.Enabled = false;
                    cbGroup.Enabled = false;
                    tbMinistry.Enabled = false;
                    cbBaptized.Enabled = false;
                    cbTithe.Enabled = false;
                    tbExperience.Enabled = false;
                    tbLed.Enabled = false;
                    tbCoverletter.Enabled = false;
                    cbRejection.Enabled = false;

                    phChangePerson.Visible = false;
                    lbRemovePerson.Visible = false;
                    btnUpdate.Visible = false;
                }

                if (_viewEnabled)
                    ShowDetails();
            }
        }

        #endregion

        #region Private Methods

        private void RegisterScripts()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("\n\n<script type=\"text/javascript\">\n");
            sbScript.Append("\tfunction openSearchWindow(searchType)\n");
            sbScript.Append("\t{\n");
            sbScript.Append("\t\tvar tPos = window.screenTop + 100;\n");
            sbScript.Append("\t\tvar lPos = window.screenLeft + 100;\n");
            sbScript.AppendFormat("\t\tdocument.frmMain.ihPersonListID.value = '{0}';\n", ihPersonList.ClientID);
            sbScript.AppendFormat("\t\tdocument.frmMain.ihRefreshButtonID.value = '{0}';\n", bRefresh.ClientID);
            sbScript.AppendFormat("\t\tvar searchWindow = window.open('Default.aspx?page={0}','Search','height=400,width=600,resizable=yes,scrollbars=yes,toolbar=no,location=no,directories=no,status=no,menubar=no,top=' + tPos + ',left=' + lPos);\n", PopupSearchWindowSetting);
            sbScript.Append("\t\tsearchWindow.focus();\n");
            sbScript.Append("\t}\n");
            sbScript.Append("</script>\n\n");

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenSearchWindow", sbScript.ToString());
        }

        private Guid GetApplicantGuid()
        {
            Guid applicantGuid = Guid.Empty;
            string[] keys = Request.QueryString.AllKeys;

            foreach (string key in keys)
                if (key.ToUpper() == "GUID")
                {
                    try { applicantGuid = new Guid(Request.QueryString.Get(key)); }
                    catch { applicantGuid = Guid.Empty; }
                    break;
                }

            return applicantGuid;
        }

        private void DisplayRelated(CheckBox box, Label label, Control textbox)
        {
            if (box.Checked)
            {
                label.Visible = true;
                textbox.Visible = true;
            }
            else
            {
                label.Visible = false;
                textbox.Visible = false;
            }
        }

        private void ShowDetails()
        {
            pnlDetail.Visible = true;

            JobApplicant applicant = new JobApplicant(GetApplicantGuid());
            tbFirstName.Text = applicant.FirstName;
            tbLastName.Text = applicant.LastName;
            tbEmail.Text = applicant.Email;
            tbPosition.Text = applicant.JobPosting.Title;
            tbHeard.Text = applicant.HowHeard;
            tbChristian.Text = applicant.HowLongChristian;
            cbClass100.Checked = applicant.Class100;
            DisplayRelated(cbClass100, lblClass100Date, dtbClass100Date);
            dtbClass100Date.Text = applicant.Class100Date.ToShortDateString();
            cbMember.Checked = applicant.ChurchMember;
            cbGroup.Checked = applicant.NeighborhoodGroup;
            cbServing.Checked = applicant.Serving;
            DisplayRelated(cbServing, lblMinistry, tbMinistry);
            tbMinistry.Text = applicant.ServingMinistry;
            cbBaptized.Checked = applicant.Baptized;
            cbTithe.Checked = applicant.Tithing;
            tbExperience.Text = applicant.Experience;
            tbLed.Text = applicant.LedToApply;
            tbCoverletter.Text = applicant.Coverletter;
            cbRejection.Checked = applicant.RejectionNoticeSent;
            cbReviewed.Checked = applicant.ReviewedByHR;

            if (applicant.Person.PersonID != -1)
            {
                if (applicant.Person != null)
                {
                    lblPersonEdit.Text = string.Format("<a href='default.aspx?page={0}&guid={1}' target='_blank'>{2}</a> ",
                        PersonDetailPageIDSetting,
                        applicant.Person.PersonGUID,
                        applicant.Person.FullName);
                    ihPersonID.Value = applicant.Person.PersonID.ToString();
                }
                else
                {
                    lblPersonEdit.Text = string.Empty;
                    ihPersonID.Value = string.Empty;
                }
            }
            else
            {
                lblPersonEdit.Text = string.Empty;
                ihPersonID.Value = string.Empty;
            }

            lbRemovePerson.Visible = (applicant.Person.PersonID != -1);
            aResume.NavigateUrl = string.Format("~/download.aspx?guid={0}", HttpUtility.UrlEncode(applicant.Resume.GUID.ToString()));
        }

        #endregion

        #region Events

        private void lbRemovePerson_Click(object sender, EventArgs e)
        {
            JobApplicant applicant = new JobApplicant(GetApplicantGuid());
            applicant.Person.PersonID = -1;
            ihPersonID.Value = string.Empty;
            lblPersonEdit.Text = string.Empty;
            lbRemovePerson.Visible = false;
            applicant.Save(CurrentUser.Identity.Name, true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            JobApplicant applicant = new JobApplicant(GetApplicantGuid());
            Response.Redirect(string.Format("~/default.aspx?page={0}&guid={1}&parCurrentGroup=Finance", ApplicantListPageIDSetting, applicant.JobPosting.JobPostingGuid.ToString()));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            JobApplicant applicant = new JobApplicant(GetApplicantGuid());

            if (ihPersonID.Value.Trim() != string.Empty)
                applicant.Person.PersonID = int.Parse(ihPersonID.Value);
            else
                applicant.Person.PersonID = -1;

            applicant.FirstName = tbFirstName.Text;
            applicant.LastName = tbLastName.Text;
            applicant.Email = tbEmail.Text;
            applicant.JobPosting = new JobPosting(tbPosition.Text);
            applicant.HowHeard = tbHeard.Text;
            applicant.HowLongChristian = tbChristian.Text;
            applicant.Class100 = cbClass100.Checked;
            applicant.Class100Date = DateTime.Parse(dtbClass100Date.Text);
            applicant.ChurchMember = cbMember.Checked;
            applicant.NeighborhoodGroup = cbGroup.Checked;
            applicant.Serving = cbServing.Checked;
            applicant.ServingMinistry = tbMinistry.Text;
            applicant.Baptized = cbBaptized.Checked;
            applicant.Tithing = cbTithe.Checked;
            applicant.Experience = tbExperience.Text;
            applicant.LedToApply = tbLed.Text;
            applicant.Coverletter = tbCoverletter.Text;
            applicant.RejectionNoticeSent = cbRejection.Checked;
            applicant.ReviewedByHR = cbReviewed.Checked;
            applicant.Save(CurrentUser.Identity.Name, true);
            Response.Redirect(string.Format("~/default.aspx?page={0}&guid={1}&parCurrentGroup=Finance", ApplicantListPageIDSetting, applicant.JobPosting.JobPostingGuid.ToString()));
        }

        private void cbClass100_CheckedChanged(object sender, EventArgs e)
        {
            DisplayRelated(cbClass100, lblClass100Date, dtbClass100Date);
        }

        private void cbServing_CheckedChanged(object sender, EventArgs e)
        {
            DisplayRelated(cbServing, lblMinistry, tbMinistry);
        }

        protected void bRefresh_Click(object sender, System.EventArgs e)
        {
            JobApplicant applicant = new JobApplicant(GetApplicantGuid());
            string[] newPersonIDs = ihPersonList.Value.Split(',');

            foreach (string id in newPersonIDs)
            {
                if (id.Trim() != string.Empty)
                {
                    applicant.Person.PersonID = int.Parse(id);
                    Person person = new Person(applicant.Person.PersonID);
                    lblPersonEdit.Text = person.FullName + "<br />";
                    lbRemovePerson.Visible = applicant.Person.PersonID != -1;

                    if (tbFirstName.Text.Trim() == string.Empty)
                        tbFirstName.Text = person.NickName;
                    if (tbLastName.Text.Trim() == string.Empty)
                        tbLastName.Text = person.LastName;
                    if (tbEmail.Text.Trim() == string.Empty)
                        tbEmail.Text = person.Emails.FirstActive;

                    ihPersonID.Value = id;
                }
            }
            ihPersonList.Value = string.Empty;
        }

        #endregion
    }
}