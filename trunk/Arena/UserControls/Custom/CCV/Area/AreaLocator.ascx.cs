namespace ArenaWeb.UserControls.Custom.CCV.Area
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Collections;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    
    using Arena.Core;
    using Arena.Exceptions;
	using Arena.Portal;
	using Arena.Portal.UI;
    using Arena.Security;
	using Arena.Utility;

	/// <summary>
	///		Summary description for RequestDetail.
	/// </summary>
    public partial class AreaLocator : PortalControl
	{
		#region Module Settings

		// Module Settings
		[PageSetting("Successful Area Page", "The page to display a successful Area match.", true)]
        public string AreaPageIDSetting { get { return Setting("AreaPageID", "", true); } }

		[PageSetting("Unsuccessful Area Page", "The page to display an unsuccessful Area match.", true)]
        public string InvalidAreaPageIDSetting { get { return Setting("InvalidAreaPageID", "", true); } }

        [TextSetting("Address Caption", "The caption to use above the address fields (Default = 'Find Your Area').",  false)]
        public string AddressCaptionSetting { get { return Setting("AddressCaption", "Find Your Area", false); } }

        #endregion

        #region Private Variables

        Address cachedAddress = null;

        #endregion

        #region Events

        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Session["PersonAddress"] != null)
                cachedAddress = (Address)Session["PersonAddress"];

            if (!Page.IsPostBack)
            {
                lblAddressCaption.Text = AddressCaptionSetting;

                if (cachedAddress != null)
                {
                    tbAddress.Text = cachedAddress.StreetLine1;
                    tbCity.Text = cachedAddress.City;
                    tbState.Text = cachedAddress.State;
                    tbZip.Text = cachedAddress.PostalCode;
                }
                else if (CurrentPerson != null && CurrentPerson.PrimaryAddress != null)
                {
                    tbAddress.Text = CurrentPerson.PrimaryAddress.StreetLine1;
                    tbCity.Text = CurrentPerson.PrimaryAddress.City;
                    tbState.Text = CurrentPerson.PrimaryAddress.State;
                    tbZip.Text = CurrentPerson.PrimaryAddress.PostalCode;
                }
            }
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            Address address = new Address(tbAddress.Text, string.Empty, tbCity.Text, tbState.Text, tbZip.Text, "US", false);
            tbAddress.Text = address.StreetLine1;
            tbCity.Text = address.City;
            tbState.Text = address.State;
            tbZip.Text = address.PostalCode;

            try { address.Geocode("AreaLocator", true); }
            catch {}

            Session["PersonAddress"] = address;

            if (address.Area != null && address.Area.AreaID != -1)
                Response.Redirect(string.Format("default.aspx?page={0}&area={1}", AreaPageIDSetting, address.Area.AreaID.ToString()));
            else
                Response.Redirect(string.Format("default.aspx?page={0}", InvalidAreaPageIDSetting));
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
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
		}

		#endregion

	}
}
