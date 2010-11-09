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
	public partial class AreaMap : PortalControl
	{
        #region Module Settings

        // Module Settings
        [PageSetting("Area Page", "When displaying all areas, the page to display when user clicks on a specific area.", true)]
        public string AreaPageIDSetting { get { return Setting("AreaPageID", "", true); } }

        [NumericSetting("Map Width", "Width of map in pixels (default = 600).", false)]
        public string MapWidthSetting { get { return Setting("MapWidth", "600", false); } }

        [NumericSetting("Map Height", "Height of map in pixels (default = 400).", false)]
        public string MapHeightSetting { get { return Setting("MapHeight", "400", false); } }

        #endregion

        #region Private Variables

        private Area Area = null;

        #endregion

        #region Events

        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (Area == null)
			{
				int AreaID = -1;

				string[] keys;
				keys = Request.QueryString.AllKeys;
				foreach (string key in keys)
				{
					switch(key.ToUpper())
					{
						case "AREA":
							try
							{
								AreaID = Int32.Parse(Request.QueryString.Get(key));
							}
							catch(System.Exception ex)
							{
								throw new ModuleException(CurrentPortalPage, CurrentModule, string.Format("The '{0}' AreaDetail module requires a numeric" +
									"Area ID. ", CurrentModule.Title, ex));
							}
							break;
					}
				}

				Area = new Area(AreaID);
			}

			if (!Page.IsPostBack)
				ShowView();

			if (Area.AreaID != -1)
				this.CurrentPortalPage.TemplateControl.Title = Area.Name;
        }

        #endregion

        #region Private Methods

        private void ShowView()
        {
            phMap.Controls.Clear();
            Page.ClientScript.RegisterStartupScript(typeof(string), "VirtualEarth", "<script src=\"http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=5\"></script>", false);

            if (Area == null || Area.AreaID == -1)
            {
                System.Web.UI.WebControls.Panel pnlMap = new System.Web.UI.WebControls.Panel();
                pnlMap.ID = "pnlMap";
				pnlMap.Attributes.Add("class", "areaMap");
                pnlMap.Style.Add("position", "relative");
                pnlMap.Style.Add("width", MapWidthSetting + "px");
                pnlMap.Style.Add("height", MapHeightSetting + "px");
                phMap.Controls.Add(pnlMap);

                StringBuilder sbVEScript = new StringBuilder();

                sbVEScript.Append("var map = null;\n");
                sbVEScript.Append("var currentShape = null;\n");

                sbVEScript.Append("window.onload = function() {LoadMyMap();};\n");

                sbVEScript.Append("\nfunction LoadMyMap(){\n\n");
                sbVEScript.AppendFormat("\tmap = new VEMap('{0}');\n", pnlMap.ClientID);
                sbVEScript.Append("\tmap.SetDashboardSize(VEDashboardSize.Tiny);\n");
                sbVEScript.Append("\tmap.LoadMap();\n\n");
                sbVEScript.Append("\tmap.AttachEvent('onmouseover', mapMouseOver);\n");
                sbVEScript.Append("\tmap.AttachEvent('onmouseout', mapMouseOut);\n");
                sbVEScript.Append("\tmap.AttachEvent('onclick', mapClick);\n\n");
                //sbVEScript.Append("\tmap.ClearInfoBoxStyles();\n\n");

                sbVEScript.Append("\tvar shape = null;\n");
                sbVEScript.Append("\tvar areaPoints = null;\n\n");

                StringBuilder sbAllPoints = new StringBuilder();

                double maxLatitude = double.MinValue;
                double maxLongitude = double.MinValue;
                double minLatitude = double.MinValue;
                double minLongitude = double.MinValue;

                foreach (Area area in new AreaCollection(CurrentOrganization.OrganizationID))
                {
                    if (area.Coordinates.Count > 0)
                    {
                        sbVEScript.Append("\tareaPoints = new Array(\n");
                        for (int i = 0; i < area.Coordinates.Count; i++)
                        {
                            AreaCoordinate coord = area.Coordinates[i];

                            if (maxLatitude == double.MinValue || maxLatitude < coord.Latitude) maxLatitude = coord.Latitude;
                            if (maxLongitude == double.MinValue || maxLongitude < coord.Longitude) maxLongitude = coord.Longitude;
                            if (minLatitude == double.MinValue || minLatitude > coord.Latitude) minLatitude = coord.Latitude;
                            if (minLongitude == double.MinValue || minLongitude > coord.Longitude) minLongitude = coord.Longitude;

                            sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1})", coord.Latitude.ToString(), coord.Longitude.ToString());
                            if (i < area.Coordinates.Count - 1)
                                sbVEScript.Append(",\n");

                            sbAllPoints.AppendFormat("\t\tnew VELatLong({0}, {1}),\n", coord.Latitude.ToString(), coord.Longitude.ToString());
                        }
                        sbVEScript.Append(");\n");

                        sbVEScript.Append("\tshape = new VEShape(VEShapeType.Polygon, areaPoints);\n\n");
                        sbVEScript.Append("\tshape.SetLineColor(new VEColor(255,0,0,1));\n");
                        sbVEScript.Append("\tshape.SetLineWidth(1);\n");
                        sbVEScript.Append("\tshape.SetFillColor(new VEColor(236,183,49,.3));\n");
                        sbVEScript.AppendFormat("\tshape.SetTitle(\"{0}\");\n", area.Name);
                        sbVEScript.AppendFormat("\tshape.SetDescription(\"{0}\");\n", AreaDetailsHTML(area));
                        sbVEScript.AppendFormat("\tshape.SetMoreInfoURL('default.aspx?page={0}&area={1}');\n", AreaPageIDSetting, area.AreaID.ToString());
                        sbVEScript.Append("\tshape.HideIcon();\n");
                        sbVEScript.Append("\tmap.AddShape(shape);\n\n");
                    }
                }

                sbVEScript.Append("\tvar allPoints = new Array(\n");
                sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1}),\n", minLatitude.ToString(), minLongitude.ToString());
                sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1}))\n", maxLatitude.ToString(), maxLongitude.ToString());
                sbVEScript.Append("\tmap.SetMapView(allPoints);\n");
                sbVEScript.Append("\tmap.ZoomIn();\n");
                sbVEScript.Append("}\n");

                sbVEScript.Append("\nfunction mapMouseOver(e){\n");
                sbVEScript.Append("\tvar shape = map.GetShapeByID(e.elementID);\n");
                sbVEScript.Append("\tshape.SetLineWidth(2);\n");
                sbVEScript.Append("\tif (currentShape != null)\n");
                sbVEScript.Append("\t\tcurrentShape.SetLineWidth(1);\n");
                sbVEScript.Append("\tcurrentShape = shape;\n");
                sbVEScript.Append("}\n");

                sbVEScript.Append("\nfunction mapMouseOut(e){\n");
                sbVEScript.Append("\tvar shape = map.GetShapeByID(e.elementID);\n");
                sbVEScript.Append("\tshape.SetLineWidth(1);\n");
                sbVEScript.Append("}\n");

                sbVEScript.Append("\nfunction mapClick(e){\n");
                sbVEScript.Append("\tvar shape = map.GetShapeByID(e.elementID);\n");
                sbVEScript.Append("\twindow.location = shape.GetMoreInfoURL();\n");
                sbVEScript.Append("}\n");

                Page.ClientScript.RegisterStartupScript(typeof(string), "LoadMap", sbVEScript.ToString(), true);
            }
            else
            {
                System.Web.UI.WebControls.Panel pnlMap = new System.Web.UI.WebControls.Panel();
                pnlMap.ID = "pnlMap";
                pnlMap.Style.Add("position", "relative");
                pnlMap.Style.Add("width", "600px");
                pnlMap.Style.Add("height", "400px");
                phMap.Controls.Add(pnlMap);

                StringBuilder sbVEScript = new StringBuilder();

                sbVEScript.Append("var map = null;\n");

                sbVEScript.Append("window.onload = function() {LoadMyMap();};\n");

                sbVEScript.Append("\nfunction LoadMyMap(){\n");
                sbVEScript.AppendFormat("\tmap = new VEMap('{0}');\n", pnlMap.ClientID);
                sbVEScript.Append("\tmap.LoadMap();\n\n");
                sbVEScript.Append("\tmap.ClearInfoBoxStyles();\n\n");

                sbVEScript.Append("\tvar points = new Array(\n");
                for (int i = 0; i < Area.Coordinates.Count; i++)
                {
                    AreaCoordinate coord = Area.Coordinates[i];
                    sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1})", coord.Latitude.ToString(), coord.Longitude.ToString());
                    if (i < Area.Coordinates.Count - 1)
                        sbVEScript.Append(",\n");
                }
                sbVEScript.Append("\n\t);\n");
                sbVEScript.Append("\tvar shape = new VEShape(VEShapeType.Polygon, points);\n");
                sbVEScript.Append("\tmap.SetMapView(points);\n");

                sbVEScript.Append("\tshape.SetLineColor(new VEColor(255,0,0,1));\n");
                sbVEScript.Append("\tshape.SetLineWidth(2);\n");
                sbVEScript.Append("\tshape.SetFillColor(new VEColor(236,183,49,.3));\n");
                sbVEScript.Append("\tshape.HideIcon();\n");
                sbVEScript.AppendFormat("\tshape.SetTitle('{0}');\n", Area.Name);
                sbVEScript.Append("\n\tmap.AddShape(shape);\n");

                GroupCollection groups = new GroupCollection();
                groups.LoadByArea(Area.AreaID, 1);
                foreach (Group group in groups)
                {
                    if (group.TargetLocation != null)
                    {
                        sbVEScript.AppendFormat("\n\tshape = new VEShape(VEShapeType.Pushpin, new VELatLong({0}, {1}));\n",
                            group.TargetLocation.Latitude.ToString(),
                            group.TargetLocation.Longitude.ToString());
                        sbVEScript.Append("\tshape.SetCustomIcon('images/map/pin_blue.png');\n");
                        sbVEScript.AppendFormat("\tshape.SetTitle(\"{0}\");\n", BuildDetailTitle(group.Title));
                        sbVEScript.AppendFormat("\tshape.SetDescription('{0}');\n", BuildDetailPanel(group));

                        sbVEScript.Append("\tmap.AddShape(shape);\n");
                    }
                }

                sbVEScript.Append("}\n");

                Page.ClientScript.RegisterStartupScript(typeof(string), "LoadMap", sbVEScript.ToString(), true);
            }
        }

        public string AreaDetailsHTML(Area area)
        {
            StringBuilder sbDesc = new StringBuilder();

            sbDesc.Append("<div style='text-align:left'>");

            foreach (AreaOutreachCoordinator coordinator in area.OutreachCoordinators)
            {
                Person leader = new Person(coordinator.PersonId);
                if (leader != null && leader.PersonID != -1)
                {
                    sbDesc.Append("<div style='clear:both;margin-bottom:10px;'>");

                    if (leader.Blob != null && leader.Blob.ByteArray != null && leader.Blob.ByteArray.Length != 0)
                        sbDesc.AppendFormat("<img src='cachedblob.aspx?guid={0}&width=100&height=100' border='1px' style='float:right'>", leader.Blob.GUID.ToString());

                    sbDesc.AppendFormat("<div class=formLabel>{0}</div>", coordinator.AreaRole.Value);
                    sbDesc.AppendFormat("<div class=formItem style='padding-left:5px;'>{0}</div>", leader.FullName);
                    sbDesc.Append("</div>");
                }
            }

            sbDesc.Append("</div>");

            return sbDesc.ToString();
        }

        private string BuildDetailTitle(string title)
        {
            return string.Format("<div class=listHeader style='padding:2px'>{0}</div>", title);
        }
        
        private string BuildDetailPanel(Group group)
        {
            StringBuilder sbGroup = new StringBuilder();
            sbGroup.Append("<table cellpadding=2 cellspacing=0>");
            sbGroup.Append("<tr><td colspan=2 class=listLine><table cellpadding=1 cellspacing=0>");
            sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap colspan=2>{0}<br>{1}, {2} {3}</td></tr>", group.TargetLocation.StreetLine1, group.TargetLocation.City, group.TargetLocation.State, group.TargetLocation.PostalCode);
            sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap>Meeting Day:&nbsp;</td><td class=smallText valign=top nowrap><b>{0}</b></td></tr>", group.MeetingDay.Value);
            sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap>Group Size:&nbsp;</td><td class=smallText valign=top nowrap><b>{0}</b></td></tr>", group.Members.Count.ToString());
            sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap>Registrations:&nbsp;</td><td class=smallText valign=top nowrap><b>{0}</b></td></tr>", group.Registrations.Count.ToString());
            sbGroup.Append("</table></td></tr>");

            // Leader
            if (group.Leader != null && group.Leader.PersonID != -1)
            {
                string cellClass = "listItem";


                sbGroup.Append("<tr>");
                if (group.Leader.Blob.BlobID != -1)
                    sbGroup.AppendFormat("<td class={0} valign=top><img src='cachedblob.aspx?guid={1}&width=100&height=100' border='1'></td>", cellClass, group.Leader.Blob.GUID.ToString());
                else
                    sbGroup.AppendFormat("<td class={0}></td>", cellClass);

                sbGroup.AppendFormat("<td class={0} valign=top>", cellClass);
                sbGroup.Append("<table cellpadding=1 cellspacing=0>");
                sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap><b>{0}</b></td></tr>", group.Leader.FullName);
                PersonPhone homePhone = group.Leader.Phones.FindByType(SystemLookup.PhoneType_Home);
                if (homePhone != null)
                    sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap>Home #: {0}</td></tr>", homePhone.ToString());
                if (group.Leader.Gender != Gender.Undefined && group.Leader.Gender != Gender.Unknown)
                    sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap>{0}</td></tr>", group.Leader.Gender.ToString());
                if (group.Leader.MaritalStatus.Value.Trim() != string.Empty)
                    sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap>{0}</td></tr>", group.Leader.MaritalStatus.Value);
                sbGroup.AppendFormat("<tr><td class=smallText valign=top nowrap>Age: {0}</td></tr>", group.Leader.Age.ToString());
                sbGroup.Append("</table></td></tr>");
            }
            sbGroup.Append("</table>");

            return sbGroup.ToString().Replace("'", "");
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
