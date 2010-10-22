namespace ArenaWeb.UserControls.Custom.CCV.Core
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Configuration;
    using Arena.Organization;
    using Arena.Portal;
    using Arena.Portal.UI;
    using Arena.Security;
    using Arena.Exceptions;
    using Arena.Core;
    using Arena.Utility;

    /// <summary>
    ///		Summary description for MemberList.
    /// </summary>
    public partial class LossReport : PortalControl
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetDefaults();
                ShowList();
            }
        }

        public void SetDefaults()
        {
            // Load Pastors
            ddlPastor.Items.Add(new ListItem("[All]", "0"));
            AreaOutreachCoordinatorCollection pastors = new AreaOutreachCoordinatorCollection();
            pastors.LoadByRole(1623);
            foreach (AreaOutreachCoordinator pastor in pastors)
            {
                if (ddlPastor.Items.FindByValue(pastor.PersonId.ToString()) == null)
                {
                    Person person = new Person(pastor.PersonId);
                    ddlPastor.Items.Add(new ListItem(person.FullName, person.PersonID.ToString()));
                }
            }

            if (CurrentPerson != null)
            {
                string date = CurrentPerson.Settings["eraLoss_FromDate"] ?? string.Empty;
                tbFilterFrom.Text = date != "1/1/1900" ? date : string.Empty;

                date = CurrentPerson.Settings["eraLoss_ToDate"] ?? string.Empty;
                tbFilterTo.Text = date != "1/1/1900" ? date : string.Empty;

                ListItem li = ddlPastor.Items.FindByValue(CurrentPerson.Settings["eraLoss_Pastor"] ?? string.Empty);
                if (li != null)
                    li.Selected = true;

                cbProcessed.Checked = Boolean.Parse(CurrentPerson.Settings["eraLoss_Processed"] ?? "false");
            }
        }

        public void ShowList()
        {
			ArrayList lst = new ArrayList();
			lst.Add(new SqlParameter("@FromDate", tbFilterFrom.SelectedDate));
            lst.Add(new SqlParameter("@ToDate", tbFilterTo.SelectedDate));
            lst.Add(new SqlParameter("@Pastor", Convert.ToInt32(ddlPastor.SelectedValue)));
            lst.Add(new SqlParameter("@Processed", cbProcessed.Checked));
            DataSet ds = new Arena.DataLayer.Organization.OrganizationData().ExecuteDataSet("cust_ccv_sp_era_loss_report", lst);

            dgLosses.Columns[3].Visible = cbProcessed.Checked;
            dgLosses.Columns[6].Visible = ddlPastor.SelectedValue == "0";

            dgLosses.ItemType = "Family";
            dgLosses.ItemBgColor = CurrentPortalPage.Setting("ItemBgColor", string.Empty, false);
            dgLosses.ItemAltBgColor = CurrentPortalPage.Setting("ItemAltBgColor", string.Empty, false);
            dgLosses.ItemMouseOverColor = CurrentPortalPage.Setting("ItemMouseOverColor", string.Empty, false);
            dgLosses.AddEnabled = false;
            dgLosses.DeleteEnabled = false;
            dgLosses.EditEnabled = false;
            dgLosses.MergeEnabled = false;
            dgLosses.MailEnabled = false;
            dgLosses.ExportEnabled = true;
            dgLosses.DataSource = ds.Tables[0];
            dgLosses.DataBind();
        }

        private void dgLosses_Rebind(object source, System.EventArgs e)
        {
            ShowList();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (CurrentPerson != null)
            {
                CurrentPerson.Settings["eraLoss_FromDate"] = tbFilterFrom.SelectedDate.ToShortDateString();
                CurrentPerson.Settings["eraLoss_ToDate"] = tbFilterTo.SelectedDate.ToShortDateString();
                if (ddlPastor.SelectedIndex != -1)
                    CurrentPerson.Settings["eraLoss_Pastor"] = ddlPastor.SelectedValue;
                CurrentPerson.Settings["eraLoss_processed"] = cbProcessed.Checked.ToString();
                CurrentPerson.Settings.Save(CurrentPerson.PersonID, CurrentOrganization.OrganizationID);
            }

            dgLosses.CurrentPageIndex = 0;
            ShowList();
        }

        void btnProcess_Click(object sender, EventArgs e)
        {
            Arena.DataLayer.Organization.OrganizationData odata = new Arena.DataLayer.Organization.OrganizationData();

            foreach(DataGridItem item in dgLosses.Items)
            {
                int familyId = (int)dgLosses.DataKeys[item.ItemIndex];

                bool process = false;
                var cbProcess = item.Cells[1].Controls[0] as CheckBox;
                if (cbProcess != null && cbProcess.Checked)
                    process = true;

                bool send = false;
                var cbSend = item.Cells[2].Controls[0] as CheckBox;
                if (cbSend != null && cbSend.Checked)
                    send = true;

                string query = string.Format(
                    "update cust_ccv_era_losses set processed = {0}, send_email = {1} where family_id = {2}",
                    (process ? "1" : "0"),
                    (send ? "1" : "0"),
                    familyId.ToString());

                odata.ExecuteNonQuery(query);
            }

            ShowList();
        }

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
            dgLosses.ReBind += new DataGridReBindEventHandler(dgLosses_Rebind);
            btnApply.Click += new EventHandler(btnApply_Click);
            btnProcess.Click += new EventHandler(btnProcess_Click);
        }

        #endregion

    }
}
