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
using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Custom.CCV.HumanResources;
using Arena.Exceptions;
using Arena.Portal;
using Arena.Portal.UI;
using Arena.Security;

namespace ArenaWeb.UserControls.Custom.HumanResources
{
    public partial class StaffDetail : PortalControl
    {
        #region Module Settings

        [PageSetting("Person Detail Page", "The page that is used for displaying person details.", false, 7)]
        public string PersonDetailPageIDSetting { get { return Setting("PersonDetailPageID", "7", false); } }

        [PageSetting("Person Popup Search Page", "The page that is used for the popup search.", false, 16)]
        public string PopupSearchWindowSetting { get { return Setting("PopupSearchWindow", "16", false); } }

        [ListFromSqlSetting("Job Class Lookup Type", "Lookup Type that holds job class", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string JobClassIDSetting { get { return Setting("JobClassID", "-1", true); } }

        [ListFromSqlSetting("Sub Department Lookup Type", "Lookup Type that holds Sub Department", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string SubDepartmentIDSetting { get { return Setting("SubDepartmentID", "-1", true); } }

        [ListFromSqlSetting("Termination Type Lookup Type", "Lookup Type that holds Termination Types", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string TerminationTypeIDSetting { get { return Setting("TerminationTypeID", "-1", true); } }

        [ListFromSqlSetting("Notice Type Lookup Type", "Lookup Type that holds Notice Types", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string NoticeTypeIDSetting { get { return Setting("NoticeTypeID", "-1", true); } }

        [ListFromSqlSetting("Review Score Lookup Type", "Lookup Type that holds Review Scores", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string ReviewScoreTypeIDSetting { get { return Setting("ReviewScoreTypeID", "-1", true); } }

        [ListFromSqlSetting("Benefit Eligibility Lookup Type", "Lookup Type that holds benefit eligibility Lookups", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string BenefitEligibilityIDSetting { get { return Setting("BenefitEligibilityID", "-1", true); } }

        [ListFromSqlSetting("Medical Insurance Lookup Type", "Lookup Type that holds Medical Insurance Lookups", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string MedicalInsTypeIDSetting { get { return Setting("MedicalInsTypeID", "-1", true); } }

        [ListFromSqlSetting("Dental Insurance Lookup Type", "Lookup Type that holds dental insurance Lookups", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string DentalInsTypeIDSetting { get { return Setting("DentalInsTypeID", "-1", true); } }

        [ListFromSqlSetting("Vision Insurance Lookup Type", "Lookup Type that holds vision insurance Lookups", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string VisionInsTypeIDSetting { get { return Setting("VisionInsTypeID", "-1", true); } }

        [ListFromSqlSetting("Life Insurance Lookup Type", "Lookup Type that holds life insurance Lookups", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string LifeInsTypeIDSetting { get { return Setting("LifeInsTypeID", "-1", true); } }

        [ListFromSqlSetting("Access Areas Lookup Type", "Lookup Type that holds area access Lookups", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string AccessAreaTypeIDSetting { get { return Setting("AccessAreaTypeID", "-1", true); } }

        [ListFromSqlSetting("Leave Lookup Types", "Lookup Type that holds leave Lookups", true, "-1", "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type ORDER BY lookup_type_name")]
        public string LeaveTypeIDSetting { get { return Setting("LeaveTypeID", "-1", true); } }

        #endregion

        #region Private Fields

        private bool _editEnabled = false;
        private bool _viewEnabled = false;
        private DateTime _defaultDate = DateTime.Parse("1/1/1900");
        private Staff _staff = null;

        #endregion

        #region Page Events

        protected void Page_Init(object sender, EventArgs e)
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveSalary.Click += new EventHandler(btnSaveSalary_Click);
            btnSaveLeave.Click += new EventHandler(btnSaveLeave_Click);
            btnCancelSalary.Click += new EventHandler(btnCancelSalary_Click);
            btnCancelLeave.Click += new EventHandler(btnCancelLeave_Click);
            lbRemoveSupervisor.Click += new EventHandler(lbRemoveSupervisor_Click);
            lbRemoveReviewer.Click += new EventHandler(lbRemoveReviewer_Click);
            cbParticipant.CheckedChanged += new EventHandler(cbParticipant_CheckedChanged);
            dgSalaryHistory.ReBind += new DataGridReBindEventHandler(dgSalaryHistory_ReBind);
            dgSalaryHistory.EditCommand += new DataGridCommandEventHandler(dgSalaryHistory_EditCommand);
            dgSalaryHistory.DeleteCommand += new DataGridCommandEventHandler(dgSalaryHistory_DeleteCommand);
            dgSalaryHistory.AddItem += new AddItemEventHandler(dgSalaryHistory_AddItem);
            dgLeaveHistory.ReBind += new DataGridReBindEventHandler(dgLeaveHistory_ReBind);
            dgLeaveHistory.EditCommand += new DataGridCommandEventHandler(dgLeaveHistory_EditCommand);
            dgLeaveHistory.DeleteCommand += new DataGridCommandEventHandler(dgLeaveHistory_DeleteCommand);
            dgLeaveHistory.AddItem += new AddItemEventHandler(dgLeaveHistory_AddItem);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewEnabled = CurrentModule.Permissions.Allowed(OperationType.View, CurrentUser);
            _editEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);
            _staff = new Staff(GetGuid());
            RegisterScripts();
            lblErrorMessage.Visible = false;

            if (!Page.IsPostBack)
            {
                if (_viewEnabled)
                {
                    RegisterDropDowns();
                    ShowDetails();

                    if (_staff.SalaryHistory.Count > 0)
                        ShowSalaryHistory();
                    else
                        EditSalaryHistory(-1);

                    if (_staff.LeaveHistory.Count > 0)
                        ShowLeaveHistory();
                    else
                        EditLeaveHistory(-1);
                }

                if (!_editEnabled)
                    DisableForm();
            }
        }

        #endregion

        #region Private Methods

        public Guid GetGuid()
        {
            Guid guid = Guid.Empty;
            string[] keys = Request.QueryString.AllKeys;

            foreach (string key in keys)
                if (key.ToUpper() == "GUID")
                {
                    try { guid = new Guid(Request.QueryString.Get(key)); }
                    catch { guid = Guid.Empty; }
                    break;
                }

            return guid;
        }

        private void RegisterScripts()
        {
            StringBuilder sbScript = new StringBuilder();
            sbScript.Append("\n\n<script type=\"text/javascript\">\n");
            sbScript.Append("\tfunction openSearchWindow(searchType)\n");
            sbScript.Append("\t{\n");
            sbScript.Append("\t\tvar tPos = window.screenTop + 100;\n");
            sbScript.Append("\t\tvar lPos = window.screenLeft + 100;\n");
            sbScript.AppendFormat("\t\tdocument.frmMain.{0}.value = searchType;\n", ihSearchType.ClientID);
            sbScript.AppendFormat("\t\tdocument.frmMain.ihPersonListID.value = '{0}';\n", ihPersonList.ClientID);
            sbScript.AppendFormat("\t\tdocument.frmMain.ihRefreshButtonID.value = '{0}';\n", bRefresh.ClientID);
            sbScript.AppendFormat("\t\tvar searchWindow = window.open('Default.aspx?page={0}','Search','height=400,width=600,resizable=yes,scrollbars=yes,toolbar=no,location=no,directories=no,status=no,menubar=no,top=' + tPos + ',left=' + lPos);\n", PopupSearchWindowSetting);
            sbScript.Append("\t\tsearchWindow.focus();\n");
            sbScript.Append("\t}\n");
            sbScript.Append("</script>\n\n");

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OpenSearchWindow", sbScript.ToString());
        }

        private void RegisterDropDowns()
        {
            LoadLookup(ddlClassLU, JobClassIDSetting);
            LoadLookup(ddlSubDeptLU, SubDepartmentIDSetting);
            LoadLookup(ddlTermTypeLU, TerminationTypeIDSetting);
            LoadLookup(ddlNoticeTypeLU, NoticeTypeIDSetting);
            LoadLookup(ddlReviewScoreLU, ReviewScoreTypeIDSetting);
            LoadLookup(ddlBenefitsLU, BenefitEligibilityIDSetting);
            LoadLookup(ddlMedicalLU, MedicalInsTypeIDSetting);
            LoadLookup(ddlDentalLU, DentalInsTypeIDSetting);
            LoadLookup(ddlVisionLU, VisionInsTypeIDSetting);
            LoadLookup(ddlLifeLU, LifeInsTypeIDSetting);
            LoadLookup(ddlAccessLU, AccessAreaTypeIDSetting);
            LoadLookup(ddlLeaveTypeLU, LeaveTypeIDSetting);
        }

        private void LoadLookup(LookupDropDown ldd, string setting)
        {
            ldd.SearchEnabled = false;
            ldd.AddEnabled = _editEnabled;
            ldd.LookupTypeID = int.Parse(setting);
            ldd.DataBind();
        }

        private void DisableForm()
        {
            foreach (Control control in tcStaffInfo.Controls)
            {
                if (control is CheckBox)
                    ((CheckBox)control).Enabled = false;
                else if (control is DropDownList)
                    ((DropDownList)control).Enabled = false;
                else if (control is TextBox)
                    ((TextBox)control).Enabled = false;
                else if (control is Button)
                    ((Button)control).Enabled = false;
                else if (control is DateTextBox)
                    ((DateTextBox)control).Enabled = false;
            }

            dgSalaryHistory.Enabled = false;
            dgLeaveHistory.Enabled = false;
        }

        private void ShowDetails()
        {
            // Department Tab
            ddlClassLU.SelectedLookup = _staff.Class;
            ddlSubDeptLU.SelectedLookup = _staff.SubDepartment;
            tbTitle.Text = _staff.JobTitle;

            if (_staff.SupervisorID != -1)
            {
                if (_staff.Supervisor != null)
                {
                    lblSupervisorEdit.Text = _staff.Supervisor.FullName + "<br />";
                    ihSupervisorID.Value = _staff.SupervisorID.ToString();
                }
                else
                {
                    lblSupervisorEdit.Text = string.Empty;
                    ihSupervisorID.Value = string.Empty;
                }
            }
            else
            {
                lblSupervisorEdit.Text = string.Empty;
                ihSupervisorID.Value = string.Empty;
            }

            lbRemoveSupervisor.Visible = lblSupervisorEdit.Text.Trim() != string.Empty;

            // Employee Status Tab
            dtbHireDate.Text = _staff.HireDate != _defaultDate ? _staff.HireDate.ToShortDateString() : string.Empty;
            dtbMinisterDate.Text = _staff.MinisterDate != _defaultDate ? _staff.MinisterDate.ToShortDateString() : string.Empty;
            tbFica.Text = string.Format("{0:F2}", _staff.Fica);
            tbHours.Text = _staff.WeeklyHours != -1 ? _staff.WeeklyHours.ToString() : string.Empty;
            dtbTermDate.Text = _staff.TerminationDate != _defaultDate ? _staff.TerminationDate.ToShortDateString() : string.Empty;
            ddlTermTypeLU.SelectedLookup = _staff.TerminationType;
            ddlNoticeTypeLU.SelectedLookup = _staff.NoticeType;
            dtbExitInt.Text = _staff.ExitInterview != _defaultDate ? _staff.ExitInterview.ToShortDateString() : string.Empty;
            cbRehireable.Checked = _staff.RehireEligible;
            dtbRehireDate.Text = _staff.RehireDate != _defaultDate ? _staff.RehireDate.ToShortDateString() : string.Empty;
            tbRehireNote.Text = _staff.RehireNote;

            if (_staff.AccessAreas.Current != null)
                ddlAccessLU.SelectedLookup = _staff.AccessAreas.Current.Area;

            tbKeys.Text = _staff.Keys;
            dtbBadgeIssued.Text = _staff.BadgeIssued != _defaultDate ? _staff.BadgeIssued.ToShortDateString() : string.Empty;
            tbBadgeFob.Text = _staff.BadgeFob != -1 ? _staff.BadgeFob.ToString() : string.Empty;

            // Benefits Tab
            ddlBenefitsLU.SelectedLookup = _staff.BenefitEligible;
            dtbBeneStartDate.Text = _staff.BenefitStartDate != _defaultDate ? _staff.BenefitStartDate.ToShortDateString() : string.Empty;
            ddlMedicalLU.SelectedLookup = _staff.MedicalChoice;
            ddlDentalLU.SelectedLookup = _staff.DentalChoice;
            ddlVisionLU.SelectedLookup = _staff.VisionChoice;
            ddlLifeLU.SelectedLookup = _staff.LifeInsuranceClass;
            cbParticipant.Checked = _staff.RetirementParticipant;
            tr403Contrib.Visible = cbParticipant.Checked;
            tr403Match.Visible = cbParticipant.Checked;
            tb403Contrib.Text = string.Format("{0:F2}", _staff.RetirementContribution);
            tb403Match.Text = string.Format("{0:F2}", _staff.RetirementMatch);
            tbHSA.Text = string.Format("{0:F2}", _staff.Hsa);

            // COBRA Tab
            dtbBeneEndDate.Text = _staff.BenefitEndDate != _defaultDate ? _staff.BenefitEndDate.ToShortDateString() : string.Empty;
            cbElectCobra.Checked = _staff.ElectCobra;
            dtbCobraLetterSent.Text = _staff.CobraLetterSent != _defaultDate ? _staff.CobraLetterSent.ToShortDateString() : string.Empty;
            dtbCobraEndDate.Text = _staff.CobraEndDate != _defaultDate ? _staff.CobraEndDate.ToShortDateString() : string.Empty;

            ShowSalaryHistory();
            ShowLeaveHistory();
        }

        private void ShowSalaryHistory()
        {
            pnlSalaryHistory.Visible = true;
            pnlSalaryDetails.Visible = false;

            dgSalaryHistory.Visible = true;
            dgSalaryHistory.ItemType = "Salary History";
            dgSalaryHistory.ItemBgColor = CurrentPortalPage.Setting("ItemBgColor", string.Empty, false);
            dgSalaryHistory.ItemAltBgColor = CurrentPortalPage.Setting("ItemAltBgColor", string.Empty, false);
            dgSalaryHistory.ItemMouseOverColor = CurrentPortalPage.Setting("ItemMouseOverColor", string.Empty, false);
            dgSalaryHistory.EditEnabled = _editEnabled;
            dgSalaryHistory.AddEnabled = _editEnabled;
            dgSalaryHistory.DeleteEnabled = _editEnabled;
            dgSalaryHistory.EditOverride = true;
            dgSalaryHistory.MergeEnabled = false;
            dgSalaryHistory.MailEnabled = false;
            dgSalaryHistory.ExportEnabled = _viewEnabled;
            dgSalaryHistory.AllowSorting = false;
            dgSalaryHistory.DataSource = _staff.SalaryHistory;
            dgSalaryHistory.DataBind();
        }

        private void ShowLeaveHistory()
        {
            pnlLeaveHistory.Visible = true;
            pnlLeaveDetails.Visible = false;

            dgLeaveHistory.Visible = true;
            dgLeaveHistory.ItemType = "Leave History";
            dgLeaveHistory.ItemBgColor = CurrentPortalPage.Setting("ItemBgColor", string.Empty, false);
            dgLeaveHistory.ItemAltBgColor = CurrentPortalPage.Setting("ItemAltBgColor", string.Empty, false);
            dgLeaveHistory.ItemMouseOverColor = CurrentPortalPage.Setting("ItemMouseOverColor", string.Empty, false);
            dgLeaveHistory.EditEnabled = _editEnabled;
            dgLeaveHistory.AddEnabled = _editEnabled;
            dgLeaveHistory.DeleteEnabled = _editEnabled;
            dgLeaveHistory.EditOverride = true;
            dgLeaveHistory.MergeEnabled = false;
            dgLeaveHistory.MailEnabled = false;
            dgLeaveHistory.ExportEnabled = _viewEnabled;
            dgLeaveHistory.AllowSorting = false;
            dgLeaveHistory.DataSource = _staff.LeaveHistory;
            dgLeaveHistory.DataBind();
        }

        private void EditSalaryHistory(int salaryHistoryID)
        {
            pnlSalaryHistory.Visible = false;
            pnlSalaryDetails.Visible = true;
            ihSalaryHistoryID.Value = salaryHistoryID.ToString();

            if (salaryHistoryID != -1)
            {
                SalaryHistory history = new SalaryHistory(salaryHistoryID);
                cbFullTime.Checked = history.FullTime;
                tbHourlyRate.Text = string.Format("{0:F2}", history.HourlyRate);
                tbCurrentSalary.Text = string.Format("{0:F2}", history.Salary);
                tbHousing.Text = string.Format("{0:F2}", history.Housing);
                tbFuel.Text = string.Format("{0:F2}", history.Fuel);
                dtbRaiseDate.Text = history.RaiseDate != _defaultDate ? history.RaiseDate.ToShortDateString() : string.Empty;
                tbRaiseAmt.Text = string.Format("{0:F2}", history.RaiseAmount);
                ddlReviewScoreLU.SelectedLookup = history.ReviewScore;
                dtbReviewDate.Text = history.ReviewDate != _defaultDate ? history.ReviewDate.ToShortDateString() : string.Empty;
                lblReviewerEdit.Text = history.Reviewer.FullName + "<br />";
                ihReviewerID.Value = history.Reviewer.PersonID.ToString();
                lbRemoveReviewer.Visible = lblReviewerEdit.Text.Trim() != string.Empty;
                dtbNextReview.Text = history.NextReview != _defaultDate ? history.NextReview.ToShortDateString() : string.Empty;
            }
        }

        private void EditLeaveHistory(int leaveHistoryID)
        {
            pnlLeaveHistory.Visible = false;
            pnlLeaveDetails.Visible = true;
            ihLeaveHistoryID.Value = leaveHistoryID.ToString();

            if (leaveHistoryID != -1)
            {
                LeaveHistory history = new LeaveHistory(leaveHistoryID);
                ddlLeaveTypeLU.SelectedLookup = history.LeaveType;
                tbLeaveReason.Text = history.LeaveReason;
                dtbLeaveDate.Text = history.LeaveDate != _defaultDate ? history.LeaveDate.ToShortDateString() : string.Empty;
                dtbReturnDate.Text = history.ReturnDate != _defaultDate ? history.ReturnDate.ToShortDateString() : string.Empty;
                tbLeaveNote.Text = history.Notes;
            }
        }

        private void SaveStaff()
        {
            try
            {
                // Department Tab
                _staff.Class = ddlClassLU.SelectedLookup;
                _staff.SubDepartment = ddlSubDeptLU.SelectedLookup;
                _staff.JobTitle = tbTitle.Text;
                _staff.SupervisorID = ihSupervisorID.Value != string.Empty ? int.Parse(ihSupervisorID.Value) : -1;

                // Employee Status Tab
                _staff.HireDate = dtbHireDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbHireDate.Text) : _defaultDate;
                _staff.MinisterDate = dtbMinisterDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbMinisterDate.Text) : _defaultDate;
                _staff.Fica = tbFica.Text.Trim() != string.Empty ? decimal.Parse(tbFica.Text) : 0;
                _staff.WeeklyHours = tbHours.Text.Trim() != string.Empty ? int.Parse(tbHours.Text) : 0;
                _staff.TerminationDate = dtbTermDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbTermDate.Text) : _defaultDate;
                _staff.TerminationType = ddlTermTypeLU.SelectedLookup;
                _staff.NoticeType = ddlNoticeTypeLU.SelectedLookup;
                _staff.ExitInterview = dtbExitInt.Text.Trim() != string.Empty ? DateTime.Parse(dtbExitInt.Text) : _defaultDate;
                AccessArea accessArea = new AccessArea();
                accessArea.PersonID = _staff.PersonID;
                accessArea.Area = ddlAccessLU.SelectedLookup;
                _staff.AccessAreas.Add(accessArea);
                _staff.Keys = tbKeys.Text;
                _staff.BadgeIssued = dtbBadgeIssued.Text.Trim() != string.Empty ? DateTime.Parse(dtbBadgeIssued.Text) : _defaultDate;
                _staff.BadgeFob = tbBadgeFob.Text != string.Empty ? int.Parse(tbBadgeFob.Text) : -1;
                _staff.RehireEligible = cbRehireable.Checked;
                _staff.RehireDate = dtbRehireDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbRehireDate.Text) : _defaultDate;
                _staff.RehireNote = tbRehireNote.Text;

                // Benefits Tab
                _staff.BenefitEligible = ddlBenefitsLU.SelectedLookup;
                _staff.BenefitStartDate = dtbBeneStartDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbBeneStartDate.Text) : _defaultDate;
                _staff.MedicalChoice = ddlMedicalLU.SelectedLookup;
                _staff.DentalChoice = ddlDentalLU.SelectedLookup;
                _staff.VisionChoice = ddlVisionLU.SelectedLookup;
                _staff.LifeInsuranceClass = ddlLifeLU.SelectedLookup;
                _staff.RetirementParticipant = cbParticipant.Checked;
                _staff.RetirementContribution = cbParticipant.Checked ? decimal.Parse(tb403Contrib.Text) : 0;
                _staff.RetirementMatch = cbParticipant.Checked ? decimal.Parse(tb403Match.Text) : 0;
                _staff.Hsa = decimal.Parse(tbHSA.Text);

                // COBRA Tab
                _staff.BenefitEndDate = dtbBeneEndDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbBeneEndDate.Text) : _defaultDate;
                _staff.ElectCobra = cbElectCobra.Checked;
                _staff.CobraLetterSent = dtbCobraLetterSent.Text.Trim() != string.Empty ? DateTime.Parse(dtbCobraLetterSent.Text) : _defaultDate;
                _staff.CobraEndDate = dtbCobraEndDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbCobraEndDate.Text) : _defaultDate;

                _staff.Save(CurrentUser.Identity.Name);
                accessArea.Save(CurrentUser.Identity.Name);
                lblErrorMessage.Visible = false;
            }
            catch
            {
                lblErrorMessage.Text = "Please enter valid dates or numbers in the text boxes below.";
                lblErrorMessage.Visible = true;
            }
        }

        private void SaveSalary(int salaryHistoryID)
        {
            try
            {
                SalaryHistory salaryHistory = new SalaryHistory(salaryHistoryID);

                salaryHistory.PersonID = _staff.PersonID;
                salaryHistory.FullTime = cbFullTime.Checked;
                salaryHistory.HourlyRate = tbHourlyRate.Text != string.Empty ? decimal.Parse(tbHourlyRate.Text) : 0;
                salaryHistory.Salary = tbCurrentSalary.Text != string.Empty ? decimal.Parse(tbCurrentSalary.Text) : 0;
                salaryHistory.Housing = tbHousing.Text != string.Empty ? decimal.Parse(tbHousing.Text) : 0;
                salaryHistory.Fuel = tbFuel.Text != string.Empty ? decimal.Parse(tbFuel.Text) : 0;
                salaryHistory.RaiseDate = dtbRaiseDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbRaiseDate.Text) : _defaultDate;
                salaryHistory.RaiseAmount = tbRaiseAmt.Text != string.Empty ? decimal.Parse(tbRaiseAmt.Text) : 0;
                salaryHistory.ReviewScore = ddlReviewScoreLU.SelectedLookup;
                salaryHistory.ReviewDate = dtbReviewDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbReviewDate.Text) : _defaultDate;

                if (ihReviewerID.Value.Trim() != string.Empty)
                    salaryHistory.ReviewerID = int.Parse(ihReviewerID.Value);

                salaryHistory.NextReview = dtbNextReview.Text.Trim() != string.Empty ? DateTime.Parse(dtbNextReview.Text) : _defaultDate;

                if (salaryHistoryID == -1)
                {
                    _staff.SalaryHistory.Insert(0, salaryHistory);
                    _staff.Save(CurrentUser.Identity.Name);
                }

                salaryHistory.Save(CurrentUser.Identity.Name);
                lblErrorMessage.Visible = false;
                ShowSalaryHistory();
            }
            catch
            {
                lblErrorMessage.Text = "Please enter valid dates or numbers in the text boxes below.";
                lblErrorMessage.Visible = true;
            }
        }

        private void SaveLeave(int leaveHistoryID)
        {
            try
            {
                LeaveHistory leaveHistory = new LeaveHistory(leaveHistoryID);

                leaveHistory.PersonID = _staff.PersonID;
                leaveHistory.LeaveType = ddlLeaveTypeLU.SelectedLookup;
                leaveHistory.LeaveReason = tbLeaveReason.Text;
                leaveHistory.LeaveDate = dtbLeaveDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbLeaveDate.Text) : _defaultDate;
                leaveHistory.ReturnDate = dtbReturnDate.Text.Trim() != string.Empty ? DateTime.Parse(dtbReturnDate.Text) : _defaultDate;
                leaveHistory.Notes = tbLeaveNote.Text;

                if (leaveHistoryID == -1)
                {
                    _staff.LeaveHistory.Insert(0, leaveHistory);
                    _staff.Save(CurrentUser.Identity.Name);
                }

                leaveHistory.Save(CurrentUser.Identity.Name);
                lblErrorMessage.Visible = false;
                ShowLeaveHistory();
            }
            catch
            {
                lblErrorMessage.Text = "Please enter valid dates in the date text boxes below.";
                lblErrorMessage.Visible = true;
            }
        }

        #endregion

        #region Events

        private void dgSalaryHistory_ReBind(object sender, EventArgs e)
        {
            ShowSalaryHistory();
        }

        private void dgLeaveHistory_ReBind(object sender, EventArgs e)
        {
            ShowLeaveHistory();
        }

        private void dgSalaryHistory_AddItem(object sender, EventArgs e)
        {
            EditSalaryHistory(-1);
        }

        private void dgLeaveHistory_AddItem(object sender, EventArgs e)
        {
            EditLeaveHistory(-1);
        }

        private void dgSalaryHistory_EditCommand(object source, DataGridCommandEventArgs e)
        {
            EditSalaryHistory(int.Parse(e.Item.Cells[0].Text));
        }

        private void dgLeaveHistory_EditCommand(object source, DataGridCommandEventArgs e)
        {
            EditLeaveHistory(int.Parse(e.Item.Cells[0].Text));
        }

        private void dgSalaryHistory_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            new SalaryHistoryData().DeleteSalaryHistory(int.Parse(e.Item.Cells[0].Text));
            ShowSalaryHistory();
        }

        private void dgLeaveHistory_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            new LeaveHistoryData().DeleteLeaveHistory(int.Parse(e.Item.Cells[0].Text));
            ShowLeaveHistory();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveStaff();
            ShowDetails();
        }

        private void btnCancelSalary_Click(object sender, EventArgs e)
        {
            if (_staff.SalaryHistory.Count > 0)
                ShowSalaryHistory();

            foreach (Control control in pnlSalaryDetails.Controls)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DateTextBox)
                    ((DateTextBox)control).Text = string.Empty;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control is DropDownList)
                    ((DropDownList)control).SelectedIndex = 0;
            }

            ihReviewerID.Value = string.Empty;
            lblReviewerEdit.Text = string.Empty;
            lbRemoveReviewer.Visible = false;
        }

        private void btnCancelLeave_Click(object sender, EventArgs e)
        {
            if (_staff.LeaveHistory.Count > 0)
                ShowLeaveHistory();
            
            foreach (Control control in pnlLeaveDetails.Controls)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DateTextBox)
                    ((DateTextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).SelectedIndex = 0;
            }
        }

        private void cbParticipant_CheckedChanged(object sender, EventArgs e)
        {
            if (cbParticipant.Checked)
            {
                tr403Contrib.Visible = true;
                tr403Match.Visible = true;
            }
            else
            {
                tr403Contrib.Visible = false;
                tr403Match.Visible = false;
                tb403Contrib.Text = "0";
                tb403Match.Text = "0";
            }
        }

        private void btnSaveSalary_Click(object sender, EventArgs e)
        {
            if (lblReviewerEdit.Text.Trim() == string.Empty)
                ScriptManager.RegisterStartupScript(upSalary, upSalary.GetType(), "NoReviewer", "alert('Please choose a reviewer!');", true);
            else
                SaveSalary(int.Parse(ihSalaryHistoryID.Value));
        }

        private void btnSaveLeave_Click(object sender, EventArgs e)
        {
            SaveLeave(int.Parse(ihLeaveHistoryID.Value));
        }

        private void lbRemoveSupervisor_Click(object sender, EventArgs e)
        {
            _staff.SupervisorID = -1;
            ihSupervisorID.Value = string.Empty;
            lblSupervisorEdit.Text = string.Empty;
            lbRemoveSupervisor.Visible = false;
            _staff.Save(CurrentUser.Identity.Name);
        }

        private void lbRemoveReviewer_Click(object sender, EventArgs e)
        {
            ihReviewerID.Value = string.Empty;
            lblReviewerEdit.Text = string.Empty;
            lbRemoveReviewer.Visible = false;
        }

        protected void bRefresh_Click(object sender, EventArgs e)
        {
            string[] newPersonIDs = ihPersonList.Value.Split(',');

            foreach (string id in newPersonIDs)
                if (id.Trim() != string.Empty)
                {
                    Person person = new Person(int.Parse(id));

                    if (ihSearchType.Value == "supervisor")
                    {
                        lblSupervisorEdit.Text = person.FullName + "<br />";
                        lbRemoveSupervisor.Visible = person.PersonID != -1;
                        ihSupervisorID.Value = id;
                    }
                    else
                    {
                        lblReviewerEdit.Text = person.FullName + "<br />";
                        lbRemoveReviewer.Visible = person.PersonID != -1;
                        ihReviewerID.Value = id;
                    }
                }

            ihPersonList.Value = string.Empty;
        }

        #endregion
    }
}