namespace ArenaWeb.UserControls.Custom.CCV.Admin
{
	using System;
	using System.Xml;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Arena.Exceptions;
	using Arena.Portal;
	using Arena.Portal.UI;
	using Arena.Core;

	/// <summary>
	///		Summary description for SubscribedProfileList.
	/// </summary>
	public partial class Alerts : PortalControl
	{
        [NumericSetting("Minutes Between Processing Alerts", "The number of minutes that alert messages should be cached (default = 60)", false)]
        public string MinutesSetting { get { return Setting("Minutes", "60", false); } }

        protected void Page_Load(object sender, System.EventArgs e)
		{
            lblError.Visible = false;

            if (!Page.IsPostBack)
            {
                try
                {
                    DateTime lastProcessed = DateTime.MinValue;
                    if (Application["CCVAlertsLastProcessed"] != null)
                        lastProcessed = (DateTime)Application["CCVAlertsLastProcessed"];

                    if (lastProcessed.AddMinutes(Convert.ToInt32(MinutesSetting)).CompareTo(DateTime.Now) < 0)
                        RefreshAlerts();

                    ShowList();
                }
                catch (System.Exception ex)
                {
                    lblError.Text = "Exception Occurred trying to retrieve alerts: " + ex.Message;
                    lblError.Visible = true;
                }
            }
		}

        void lbRefresh_Click(object sender, EventArgs e)
        {
            RefreshAlerts();
            ShowList();
        }


        private void RefreshAlerts()
        {
            new Arena.DataLayer.Organization.OrganizationData().ExecuteNonQuery("cust_ccv_process_alerts");
            Application["CCVAlertsLastProcessed"] = DateTime.Now;
        }

        private void ShowList()
        {
            lvAlerts.DataSource = new Arena.DataLayer.Organization.OrganizationData().ExecuteReader("cust_ccv_get_alerts");
            lvAlerts.DataBind();
            this.Visible = lvAlerts.Items.Count > 0;
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
            lbRefresh.Click += new EventHandler(lbRefresh_Click);
		}

		#endregion
	}
}
