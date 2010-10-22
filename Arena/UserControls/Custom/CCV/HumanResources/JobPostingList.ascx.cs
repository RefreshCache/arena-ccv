using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Arena.Enums;
using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Custom.CCV.HumanResources;
using Arena.Portal;
using Arena.Security;
using Telerik.Web.UI;

namespace ArenaWeb.UserControls.Custom.HumanResources
{
    public partial class JobPostingList : PortalControl
    {
        #region Module Settings

        [FileSetting("Tools File", "Path to the editors tool file which stores the tool bar layout (default '~/Content/RadEditor/ToolsFileBasicText.xml').", false)]
        public string ToolsFileSetting { get { return Setting("ToolsFile", "~/Content/RadEditor/ToolsFileBasicText.xml", false); } }

        [PageSetting("Applicant List Page", "The page that is used for displaying applicant list.", true)]
        public string ApplicantListPageIDSetting { get { return Setting("ApplicantListPageID", "", true); } }

        #endregion

        private bool editEnabled = false;
        private JobPosting posting = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            dgList.ReBind += new Arena.Portal.UI.DataGridReBindEventHandler(dgList_ReBind);
            dgList.EditCommand += new DataGridCommandEventHandler(dgList_EditCommand);
            dgList.AddItem += new Arena.Portal.UI.AddItemEventHandler(dgList_AddItem);
            dgList.DeleteCommand += new DataGridCommandEventHandler(dgList_DeleteCommand);
            lbAdd.Click += new EventHandler(dgList_AddItem);
            cbShowExternal.CheckedChanged += new EventHandler(cbShowExternal_CheckedChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
		{
            editEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);
            Utilities.SetupRadEditor(radDescription, ToolsFileSetting);

            if (ihPositionID.Value.Trim() != string.Empty)
            {
                try
                {
                    posting = new JobPosting(int.Parse(ihPositionID.Value.Trim()));
                }
                catch
                {
                    throw new ModuleException(CurrentPortalPage, CurrentModule, "Invalid Job Posting ID in hidden field");
                }
            }

            if (!Page.IsPostBack)
            {
                bool external = false;
                bool.TryParse(CurrentPerson.Settings["JobPostingList_ShowExternal"], out external);
                cbShowExternal.Checked = external;
                ShowList();
            }
        }

        #region Private Methods

        private void ShowList()
		{
            HyperLinkColumn hlc = (HyperLinkColumn)dgList.Columns[2];
            hlc.DataNavigateUrlFormatString = "~/default.aspx?page=" + ApplicantListPageIDSetting + "&guid={0}";

            dgList.Visible = true;
            dgList.ItemType = "Job Posting";
            dgList.ItemBgColor = CurrentPortalPage.Setting("ItemBgColor", string.Empty, false);
            dgList.ItemAltBgColor = CurrentPortalPage.Setting("ItemAltBgColor", string.Empty, false);
            dgList.ItemMouseOverColor = CurrentPortalPage.Setting("ItemMouseOverColor", string.Empty, false);
            dgList.AddEnabled = editEnabled;
            dgList.AddImageUrl = "~/images/add_job.gif";
            dgList.DeleteEnabled = editEnabled;
            dgList.EditEnabled = editEnabled;
            dgList.EditOverride = true;
            dgList.MergeEnabled = false;
            dgList.MailEnabled = false;
            dgList.ExportEnabled = true;
            dgList.AllowSorting = true;
            dgList.DataSource = new JobPostingData().GetJobPostingList_DT(cbShowExternal.Checked);
            dgList.DataBind();

            pnlDetails.Visible = false;
            pnlList.Visible = true;

            if (dgList.Items.Count > 0)
            {
                dgList.Visible = true;
                lbAdd.Visible = false;
            }
            else
            {
                if (editEnabled)
                {
                    dgList.Visible = false;
                    lbAdd.Visible = true;
                }
                else
                    pnlList.Visible = false;
            }
		}

        private void ShowEdit(int jobPostingID)
        {
            ihPositionID.Value = jobPostingID.ToString();
            posting = new JobPosting(jobPostingID);
            CurrentPortalPage.TemplateControl.Title = posting.Title == string.Empty ? "New Position" : posting.Title;

            tbTitle.Text = posting.Title;
            radDescription.Content = posting.Description;
            cbFullTime.Checked = posting.FullTime;
            cbPaidPosition.Checked = posting.PaidPosition;
            cbShowExternalEdit.Checked = posting.ShowExternal;

            btnUpdate.Visible = editEnabled;

            pnlDetails.Visible = true;
            pnlList.Visible = false;
        }

        #endregion

        #region Events

        private void dgList_ReBind(object sender, EventArgs e)
		{
			ShowList();
		}

        private void dgList_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ShowEdit(int.Parse(e.Item.Cells[0].Text));
        }

        private void dgList_AddItem(object sender, EventArgs e)
        {
            ShowEdit(-1);
        }

        private void dgList_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            new JobPostingData().DeleteJobPosting(int.Parse(e.Item.Cells[0].Text));
            ShowList();
        }

        private void cbShowExternal_CheckedChanged(object sender, EventArgs e)
		{
            CurrentPerson.Settings["JobPostingList_ShowExternal"] = cbShowExternal.Checked.ToString();
            CurrentPerson.Settings.Save(CurrentPerson.PersonID, CurrentOrganization.OrganizationID);
            ShowList();
		}        

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            posting = new JobPosting(int.Parse(ihPositionID.Value));
            posting.Title = tbTitle.Text;
            posting.Description = radDescription.Content;
            posting.FullTime = cbFullTime.Checked;
            posting.PaidPosition = cbPaidPosition.Checked;
            posting.ShowExternal = cbShowExternalEdit.Checked;
            posting.Save(CurrentUser.Identity.Name);

            ShowList();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ShowList();
        }

		#endregion
	}
}
