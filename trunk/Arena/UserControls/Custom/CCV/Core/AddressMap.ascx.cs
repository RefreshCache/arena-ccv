namespace ArenaWeb.UserControls.Custom.CCV.Core
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
	using System.Configuration;
	using System.Collections;
	using System.Collections.Specialized;
	using Arena.Core;
	using Arena.Metric;
	using Arena.Enums;
	using Arena.SmallGroup;
	using Arena.Portal;
	using Arena.Portal.UI;
	using Arena.Exceptions;
	using Arena.Utility;
	using Arena.DataLayer.Core;
	using Arena.DataLayer.SmallGroup;

	/// <summary>
	///		Summary description for RequestDetail.
	/// </summary>
    public partial class AddressMap : PortalControl
	{
        #region Module Settings

        // Module Settings
        [NumericSetting("Map Width", "Width of map in pixels (default = 600).", false)]
        public string MapWidthSetting { get { return Setting("MapWidth", "600", false); } }

        [NumericSetting("Map Height", "Height of map in pixels (default = 400).", false)]
        public string MapHeightSetting { get { return Setting("MapHeight", "400", false); } }

        [NumericSetting("Address ID", "Address ID to use", true)]
        public string AddressIDSetting { get { return Setting("AddressID", "-1", true); } }

        #endregion

        #region Events

        protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
				ShowView();
        }

        #endregion

        #region Private Methods

        private void ShowView()
        {
            Address address = new Address(Int32.Parse(AddressIDSetting));

            phMap.Controls.Clear();
            Page.ClientScript.RegisterStartupScript(typeof(string), "VirtualEarth", "<script src=\"http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=5\"></script>", false);

            System.Web.UI.WebControls.Panel pnlMap = new System.Web.UI.WebControls.Panel();
            pnlMap.ID = "pnlMap";
            pnlMap.Style.Add("position", "relative");
            pnlMap.Style.Add("width", MapWidthSetting + "px");
            pnlMap.Style.Add("height", MapHeightSetting + "px");
            phMap.Controls.Add(pnlMap);

            StringBuilder sbVEScript = new StringBuilder();

            sbVEScript.Append("var map = null;\n");

            sbVEScript.Append("window.onload = function() {LoadMyMap();};\n");

            sbVEScript.Append("\nfunction LoadMyMap(){\n");
            sbVEScript.AppendFormat("\tmap = new VEMap('{0}');\n", pnlMap.ClientID);
            sbVEScript.Append("\tmap.LoadMap();\n\n");
            sbVEScript.Append("\tmap.AttachEvent('onclick', mapClick);\n\n");
            //sbVEScript.Append("\tmap.ClearInfoBoxStyles();\n\n");

            //sbVEScript.Append("\tvar points = new Array(\n");
            //for (int i = 0; i < Area.Coordinates.Count; i++)
            //{
            //    AreaCoordinate coord = Area.Coordinates[i];
            //    sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1})", coord.Latitude.ToString(), coord.Longitude.ToString());
            //    if (i < Area.Coordinates.Count - 1)
            //        sbVEScript.Append(",\n");
            //}
            //sbVEScript.Append("\n\t);\n");
            //sbVEScript.Append("\tvar shape = new VEShape(VEShapeType.Polygon, points);\n");
            //sbVEScript.Append("\tmap.SetMapView(points);\n");

            //sbVEScript.Append("\tshape.SetLineColor(new VEColor(255,0,0,1));\n");
            //sbVEScript.Append("\tshape.SetLineWidth(2);\n");
            //sbVEScript.Append("\tshape.SetFillColor(new VEColor(236,183,49,.3));\n");
            //sbVEScript.Append("\tshape.HideIcon();\n");
            //sbVEScript.AppendFormat("\tshape.SetTitle('{0}');\n", Area.Name);
            //sbVEScript.Append("\n\tmap.AddShape(shape);\n");

            sbVEScript.AppendFormat("\n\tshape = new VEShape(VEShapeType.Pushpin, new VELatLong({0}, {1}));\n",
                address.Latitude.ToString(),
                address.Longitude.ToString());
            sbVEScript.Append("\tshape.SetCustomIcon('images/map/pin_blue.png');\n");
            sbVEScript.Append("\tshape.SetTitle(\"Center\");\n");
            sbVEScript.Append("\tmap.AddShape(shape);\n");
            double maxLatitude = double.MinValue;
            double maxLongitude = double.MinValue;
            double minLatitude = double.MinValue;
            double minLongitude = double.MinValue;

            ArrayList lst = new ArrayList();
            lst.Add(new SqlParameter("@TargetAddressID", address.AddressID));
            SqlDataReader rdr = new Arena.DataLayer.Organization.OrganizationData().ExecuteReader("cust_sp_target_ccv_location_members", lst);
            while (rdr.Read())
            {
                double latitude = (double)rdr["Latitude"];
                double longitude = (double)rdr["Longitude"];

                if (maxLatitude == double.MinValue || maxLatitude < latitude) maxLatitude = latitude;
                if (maxLongitude == double.MinValue || maxLongitude < longitude) maxLongitude = longitude;
                if (minLatitude == double.MinValue || minLatitude > latitude) minLatitude = latitude;
                if (minLongitude == double.MinValue || minLongitude > longitude) minLongitude = longitude;

                sbVEScript.AppendFormat("\n\tshape = new VEShape(VEShapeType.Pushpin, new VELatLong({0}, {1}));\n",
                    latitude.ToString(),
                    longitude.ToString());
                sbVEScript.AppendFormat("\tshape.SetCustomIcon('images/map/{0}');\n",
                    rdr["pin_icon"].ToString());
                sbVEScript.AppendFormat("\tshape.SetTitle(\"{0}\");\n", rdr["family_name"].ToString());
                sbVEScript.AppendFormat("\tshape.SetDescription('{0}');\n", BuildDetailPanel(rdr));
                sbVEScript.AppendFormat("\tshape.SetMoreInfoURL('default.aspx?page=7&guid={0}');\n", rdr["person_guid"].ToString());
                sbVEScript.Append("\tmap.AddShape(shape);\n");
            }
            rdr.Close();

            sbVEScript.Append("\tvar allPoints = new Array(\n");
            sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1}),\n", minLatitude.ToString(), minLongitude.ToString());
            sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1}))\n", maxLatitude.ToString(), maxLongitude.ToString());
            sbVEScript.Append("\tmap.SetMapView(allPoints);\n");
            //sbVEScript.Append("\tmap.ZoomIn();\n");

            sbVEScript.Append("}\n");

            sbVEScript.Append("\nfunction mapClick(e){\n");
            sbVEScript.Append("\tvar shape = map.GetShapeByID(e.elementID);\n");
            sbVEScript.Append("\twindow.location = shape.GetMoreInfoURL();\n");
            sbVEScript.Append("}\n");
            
            Page.ClientScript.RegisterStartupScript(typeof(string), "LoadMap", sbVEScript.ToString(), true);
        }

        private string BuildDetailTitle(string title)
        {
            return string.Format("<div class=listHeader style='padding:2px'>{0}</div>", title);
        }

        private string BuildDetailPanel(SqlDataReader rdr)
        {
            StringBuilder sb = new StringBuilder();

            //sb.AppendFormat("<div class=normalText>{0}<br/>{1}</div>",
            //    rdr["adult_names"].ToString(),
            //    rdr["child_names"].ToString());

            sb.AppendFormat("<div class=smallText>{0}{1}<br/>{2}, {3} {4}</div>",
                rdr["street_address_1"].ToString(),
                rdr["street_address_2"].ToString() != string.Empty ? "<br>" + rdr["street_address_2"].ToString() : "",
                rdr["city"].ToString(),
                rdr["state"].ToString(),
                rdr["postal_code"].ToString());

            return sb.ToString();
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
