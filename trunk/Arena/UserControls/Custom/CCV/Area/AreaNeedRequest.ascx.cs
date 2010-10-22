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

    public partial class AreaNeedRequest : PortalControl
    {
        #region Module Settings

        [TextSetting("Caption Text", "The caption to use above the fields (Default = 'Enter Your Need Below').", false)]
        public string CaptionSetting { get { return Setting("Caption", "Enter Your Need Below", false); } }

        [TextSetting("Result Text", "The text to display after user submits request (Default = 'Thank you, your request has been sent.').", false)]
        public string ResultTextSetting { get { return Setting("ResultText", "Thank you, your request has been sent.", false); } }

        [TextSetting("Subject", "The subject to use for the email that is sent (default is 'Area Need').", false)]
        public string SubjectSetting { get { return Setting("Subject", "Area Need", false); } }

        [LookupSetting("Recipient Role", "The Area Leadership role that requests should be sent to.", true, "E499057B-85CE-41B9-9C2C-7A703C8756A7")]
        public string RecipientRoleSetting { get { return Setting("RecipientRole", "", true); } }

        [LookupSetting("Backup Recipient Role", "The Area Leadership role that requests should be CC'd to or sent to if there is no one with primary recipient role.", false, "E499057B-85CE-41B9-9C2C-7A703C8756A7"),]
        public string BackupRecipientRoleSetting { get { return Setting("BackupRecipientRole", "", false); } }

        #endregion

        #region Private Variables

        private Area _area = null;

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try { _area = new Area(Int32.Parse(Request.QueryString["Area"])); }
            catch {}

            if (_area == null || _area.AreaID == -1)
                throw new ArenaApplicationException("A Valid Area ID is required to use this control.");

            pnlError.Controls.Clear();
            pnlError.Visible = false;

            if (!Page.IsPostBack)
            {
                lblCaption.Text = CaptionSetting.Replace("##AREANAME##", _area.Name);
                lblResult.Text = ResultTextSetting.Replace("##AREANAME##", _area.Name);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            StringBuilder errorText = new StringBuilder();

            if (tbName.Text.Trim() == string.Empty)
            {
                errorText.Append("<li>Name is Required</li>");
                tbName.BackColor = System.Drawing.Color.Bisque;
            }

            if (tbEmail.Text.Trim() == string.Empty )
            {
                errorText.Append("<li>Email is Required</li>");
                tbEmail.BackColor = System.Drawing.Color.Bisque;
            }

            if (tbNeed.Text.Trim() == string.Empty)
            {
                errorText.Append("<li>Description of Need is Required</li>");
                tbNeed.BackColor = System.Drawing.Color.Bisque;
            }

            if (errorText.Length > 0)
            {
                pnlError.Controls.Add(new LiteralControl("Please correct the following...<ul>" + errorText.ToString() + "</ul>"));
                pnlError.Visible = true;
            }
            else
            {
                SendNotification();
            }
        }
        protected void btnSubmitAnother_Click(object sender, EventArgs e)
        {
            ShowEntry();
        }

        #endregion

        #region Private Methods

        private void ShowEntry()
        {
            tbName.BackColor = System.Drawing.SystemColors.Control;
            tbPhone.BackColor = System.Drawing.SystemColors.Control;
            tbEmail.BackColor = System.Drawing.SystemColors.Control;
            tbNeed.BackColor = System.Drawing.SystemColors.Control;
            tbNeed.Text = string.Empty;

            pnlEntryForm.Visible = true;
            pnlResultForm.Visible = false;
        }

        private void SendNotification()
        {
            StringBuilder sbTo = new StringBuilder();
            StringBuilder sbCc = new StringBuilder();

            foreach (AreaOutreachCoordinator recipient in FilterCoordinatorsByRole(_area.OutreachCoordinators, Int32.Parse(RecipientRoleSetting)))
                AddRecipient(sbTo, new Person(recipient.PersonId).Emails.FirstActive);

            foreach(AreaOutreachCoordinator recipient in FilterCoordinatorsByRole(_area.OutreachCoordinators, Int32.Parse(BackupRecipientRoleSetting)))
                AddRecipient(sbCc, new Person(recipient.PersonId).Emails.FirstActive);

            string toEmails = sbTo.ToString();
            string ccEmails = string.Empty;
            if (toEmails == string.Empty)
                toEmails = sbCc.ToString();
            else
                ccEmails = sbCc.ToString();

            string htmlEmail = CurrentOrganization.Settings["PostANeedHtml"];
            if (htmlEmail != null && htmlEmail.Trim() != string.Empty)
                htmlEmail = htmlEmail.Replace("##AREANAME##", _area.Name).Replace("##NAME##", tbName.Text).Replace("##PHONE##", tbPhone.Text).Replace("##EMAIL##", tbEmail.Text).Replace("##DESCRIPTION##", tbNeed.Text);
            else
                throw new ArenaApplicationException("The 'PostANeedHtml' Organization Setting is required for this module.");

            string asciiEmail = CurrentOrganization.Settings["PostANeedAscii"];
            if (asciiEmail != null && asciiEmail.Trim() != string.Empty)
                asciiEmail = asciiEmail.Replace("##AREANAME##", _area.Name).Replace("##NAME##", tbName.Text).Replace("##PHONE##", tbPhone.Text).Replace("##EMAIL##", tbEmail.Text).Replace("##DESCRIPTION##", tbNeed.Text);
            else
                throw new ArenaApplicationException("The 'PostANeedAscii' Organization Setting is required for this module.");

            Arena.Utility.ArenaSendMail.SendMail(tbEmail.Text, tbName.Text, toEmails, tbEmail.Text, ccEmails, string.Empty, SubjectSetting, htmlEmail, asciiEmail);

            ShowResult();
        }

        public AreaOutreachCoordinatorCollection FilterCoordinatorsByRole(AreaOutreachCoordinatorCollection coordinators, int roleId)
        {
            AreaOutreachCoordinatorCollection filtered = new AreaOutreachCoordinatorCollection();
            for (int i = 0; i < coordinators.Count; i++)
                if (coordinators[i].AreaRoleId == roleId)
                    filtered.Add(coordinators[i]);
            return filtered;
        }

        private void AddRecipient(StringBuilder sb, string email)
        {
            if (email.Trim() != string.Empty)
            {
                if (sb.Length > 0)
                    sb.Append(";");
                sb.Append(email);
            }
        }

        private void ShowResult()
        {
            pnlEntryForm.Visible = false;
            pnlResultForm.Visible = true;
        }
        #endregion
    }
}
