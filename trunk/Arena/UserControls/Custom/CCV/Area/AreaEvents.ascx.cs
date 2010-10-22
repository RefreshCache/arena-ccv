namespace ArenaWeb.UserControls.Custom.CCV.Area
{
	using System;
	using System.Text;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Arena.Core;
	using Arena.Portal;
	using Arena.SmallGroup;
	using Arena.Security;
	using Arena.Exceptions;
	using Arena.DataLayer.Core;

	/// <summary>
	///		Summary description for MemberList.
	/// </summary>
	public partial class AreaEvents : PortalControl
	{
        #region Module Settings

        [NumericSetting("Days Past To Show", "The number of days back to show past events for.  The default is 0 days.", false)]
        public string DaysPastSetting { get { return Setting("DaysPast", "0", false); } }

        #endregion

        #region Private Variables

        private Area area = null;

        #endregion

        #region Events

        protected void Page_Load(object sender, System.EventArgs e)
		{
            int _areaID = -1;

			string[] keys;
			keys = Request.QueryString.AllKeys;
			foreach (string key in keys)
			{
				switch(key.ToUpper())
				{
					case "AREA":
                        try { _areaID = Int32.Parse(Request.QueryString.Get(key)); }
                        catch { }
                        break;
				}
			}

            if (_areaID != -1)
                area = new Area(_areaID);

			ShowList();
        }

        #endregion

        #region Private Methods

        public void ShowList()
		{
            int daysPast = Int32.Parse(DaysPastSetting);
            DateTime startDate = DateTime.Today.AddDays(0 - daysPast);
            DataTable dt = new OccurrenceData().GetOccurrencesByAreaAndDate_DT(area.AreaID, startDate);

            if (dt.Rows.Count > 0)
            {
                lblHeading.Visible = true;
                dgOccurrences.Visible = true;

                lblHeading.Text = area.Name + " Events";

                dgOccurrences.DataKeyField = "occurrence_id";
                dgOccurrences.ItemType = "Event";
                dgOccurrences.AddEnabled = false;
                dgOccurrences.MoveEnabled = false;
                dgOccurrences.DeleteEnabled = false;
                dgOccurrences.EditEnabled = false;
                dgOccurrences.MergeEnabled = false;
                dgOccurrences.MailEnabled = false;
                dgOccurrences.ExportEnabled = false;
                dgOccurrences.DataSource = dt;
                dgOccurrences.DataBind();

            }
            else
            {
                lblHeading.Visible = false;
                dgOccurrences.Visible = false;
            }

        }

        #endregion

        #region Protected Methods

        protected string GetFormattedDateTime(object dateCol)
		{
			DateTime dd = (DateTime)dateCol;
			return dd.ToString("MM/dd/yy h:mm tt");
        }

        #endregion

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
		}

		#endregion

	}
}
