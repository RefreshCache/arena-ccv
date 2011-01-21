namespace ArenaWeb.UserControls.Custom.CCV.SmallGroup
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
    public partial class GroupMap : PortalControl
    {
        #region Module Settings

        // Module Settings
        [NumericSetting("Category", "The category ID of small group to display.", true)]
        public string CategorySetting { get { return Setting("Category", "", true); } }

        [NumericSetting("Cluster Type Filter", "Filter category by specific cluster type ID.", false)]
        public string ClusterTypeSetting { get { return Setting("ClusterType", "", false); } }

        [PageSetting("Registration Page", "Page to redirect user to when they want to register for a specific group.", true)]
        public string RegistrationPageIDSetting { get { return Setting("RegistrationPageID", "", true); } }

        [PageSetting("Area Page", "When displaying all areas, the page to display when user clicks on a specific area.", true)]
        public string AreaPageIDSetting { get { return Setting("AreaPageID", "", true); } }

        [PageSetting("User Confirm Page", "The page that should be used to display the User Confirm Page", true)]
        public string UserConfirmPageIDSetting { get { return Setting("UserConfirmPageID", "", true); } }

        [TextSetting("Group Icon", "Image to use as the icon for groups on the map (Default = 'images/map/pin_blue.png')", false)]
        public string GroupIconSetting { get { return Setting("GroupIcon", "images/map/pin_blue.png", false); } }

        [TextSetting("Address Icon", "Image to use as the icon for the selected address (Default = 'images/map/pin_red.png')", false)]
        public string AddressIconSetting { get { return Setting("AddressIcon", "images/map/pin_red.png", false); } }

        [NumericSetting("Map Width", "Pixels wide that map should be (default=600).", false)]
        public string MapWidthSetting { get { return Setting("MapWidth", "600", false); } }

        [NumericSetting("Map Height", "Pixels wide that map should be (default=400).", false)]
        public string MapHeightSetting { get { return Setting("MapHeight", "400", false); } }

        [TextSetting("Map Dashboard Size", "Sets the size of the dashboard for the map.  This allows you to remove features like aerial view.  Valid values are Normal, Small or Tiny.  Default is Normal.", false)]
        public string MapDashboardSizeSetting { get { return Setting("MapDashboardSize", "Normal", false); } }

        [TextSetting("Area Border Width", "Pixel width to use for area shape border. Default value is '2'", false)]
        public string AreaBorderWidthSetting { get { return Setting("AreaBorderWidth", "2", false); } }

        [TextSetting("Area Border Color", "Color and transparency values to use for shape border.  Format is RED,GREEN,BLUE,TRANSPARENCY.  Red, Green, and Blue values must be between 0 and 255.  Transparency value must be between 0 and 1.  Default value is '255,0,0,.8'", false)]
        public string AreaBorderColorSetting { get { return Setting("AreaBorderColor", "255,0,0,.8", false); } }

        [TextSetting("Area Background Color", "Color and transparency values to use for shape background.  Format is RED,GREEN,BLUE,TRANSPARENCY.  Red, Green, and Blue values must be between 0 and 255.  Transparency value must be between 0 and 1.  Default value is '236,183,49,.3'", false)]
        public string AreaBackgroundColorSetting { get { return Setting("AreaBackgroundColor", "236,183,49,.3", false); } }

        [BooleanSetting("Show Group Type", "Flag indicating if group type field should be displayed", false, false)]
        public string ShowGroupTypeSetting { get { return Setting("ShowGroupType", "false", false); } }

        [BooleanSetting("Show Meeting Day", "Flag indicating if Meeting Day should be displayed", false, true)]
        public string ShowMeetingDaySetting { get { return Setting("ShowMeetingDay", "true", false); } }

        [BooleanSetting("Show Meeting Time", "Flag indicating if group meeting time fields should be displayed", false, false)]
        public string ShowMeetingTimeSetting { get { return Setting("ShowMeetingTime", "false", false); } }

        [BooleanSetting("Show Topic", "Flag indicating if Topic should be displayed", false, true)]
        public string ShowTopicSetting { get { return Setting("ShowTopic", "true", false); } }

        [BooleanSetting("Show Marital Preference", "Flag indicating if group marital preference should be displayed", false, false)]
        public string ShowMaritalPreferenceSetting { get { return Setting("ShowMaritalPreference", "false", false); } }

        [BooleanSetting("Show Age Group", "Flag indicating if group age group should be displayed", false, false)]
        public string ShowAgeGroupSetting { get { return Setting("ShowAgeGroup", "false", false); } }

        [BooleanSetting("Show Average Age", "Flag indicating if Average age of group members should be displayed", false, true)]
        public string ShowAverageAgeSetting { get { return Setting("ShowAverageAge", "true", false); } }

        [BooleanSetting("Show City", "Flag indicating if group city state fields should be displayed", false, false)]
        public string ShowCitySetting { get { return Setting("ShowCity", "false", false); } }

        [BooleanSetting("Show Description", "Flag indicating if group Description fields should be displayed", false, false)]
        public string ShowDescriptionSetting { get { return Setting("ShowDescription", "false", false); } }

        [BooleanSetting("Show Notes", "Flag indicating if group Notes fields should be displayed", false, true)]
        public string ShowNotesSetting { get { return Setting("ShowNotes", "true", false); } }

        [BooleanSetting("Show Full Groups", "Flag indicating if groups that are full should be displayed", false, true)]
        public string ShowFullGroupsSetting { get { return Setting("ShowFullGroups", "true", false); } }

        #endregion

        #region Private Variables

        private Area Area = null;
        private Category category = null;
        private int clusterTypeFilterID = -1;

        private double maxLatitude = double.MinValue;
        private double maxLongitude = double.MinValue;
        private double minLatitude = double.MinValue;
        private double minLongitude = double.MinValue;

        private bool showGroupType = false;
        private bool showTopic = true;
        private bool showMaritalPreference = false;
        private bool showAgeGroup = false;
        private bool showAverageAge = true;
        private bool showMeetingDay = true;
        private bool showMeetingTime = false;
        private bool showCity = false;
        private bool showDescription = false;
        private bool showNotes = true;

        private int mapGroupCount = 0;
        private GroupCollection allGroups = new GroupCollection();

        #endregion

        #region Events

        protected void Page_Load(object sender, System.EventArgs e)
        {
            category = new Category(Int32.Parse(CategorySetting));
            if (ClusterTypeSetting != string.Empty)
                try { clusterTypeFilterID = Int32.Parse(ClusterTypeSetting); }
            catch {}

            if (Area == null)
            {
                int AreaID = -1;

                string[] keys;
                keys = Request.QueryString.AllKeys;
                foreach (string key in keys)
                {
                    switch (key.ToUpper())
                    {
                        case "AREA":
                            try
                            {
                                AreaID = Int32.Parse(Request.QueryString.Get(key));
                            }
                            catch (System.Exception ex)
                            {
                                throw new ModuleException(CurrentPortalPage, CurrentModule, string.Format("The '{0}' AreaDetail module requires a numeric" +
                                    "Area ID. ", CurrentModule.Title, ex));
                            }
                            break;
                    }
                }

                Area = new Area(AreaID);
            }

            BuildMap();
            if (!Page.IsPostBack)
            {
                FormatListColumns();
                BindList();
            }

            if (Area.AreaID != -1)
                this.CurrentPortalPage.TemplateControl.Title = Area.Name;
        }

        void gvGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Register")
            {
                string groupId = gvGroups.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();
                string urlString = string.Format("default.aspx?page={0}&group={1}", RegistrationPageIDSetting, groupId);
                string redirectString = string.Format("default.aspx?page={0}&requestUrl={1}&group={2}", UserConfirmPageIDSetting, HttpUtility.UrlEncode(urlString), groupId);
                Response.Redirect(redirectString, true);
            }
        }

        void gvGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Group group = allGroups[e.Row.RowIndex];

                HtmlAnchor aGroupTitle = (HtmlAnchor)e.Row.FindControl("aGroupTitle");
                Label lblGroupTitle = (Label)e.Row.FindControl("lblGroupTitle");

                if (group.TargetLocation != null &&
                    group.TargetLocation.AddressID != -1 &&
                    group.TargetLocation.Latitude != 0 &&
                    group.TargetLocation.Longitude != 0)
                {
                    aGroupTitle.Visible = true;
                    lblGroupTitle.Visible = false;
                    aGroupTitle.HRef = string.Format("javascript:ShowGroup({0});", group.MatchScore.ToString("N0"));
                }
                else
                {
                    aGroupTitle.Visible = false;
                    lblGroupTitle.Visible = true;
                }
            }
        }

        void gvGroups_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortGroups(e.SortExpression);
            BindList();
        }

        #endregion

        #region Private Methods

        private void BuildMap()
        {
            showGroupType = Boolean.Parse(ShowGroupTypeSetting);
            showMeetingDay = Boolean.Parse(ShowMeetingDaySetting) && category.MeetingDayCaption.Trim() != string.Empty;
            showMeetingTime = Boolean.Parse(ShowMeetingTimeSetting);
            showTopic = Boolean.Parse(ShowTopicSetting) && category.TopicCaption.Trim() != string.Empty;
            showMaritalPreference = Boolean.Parse(ShowMaritalPreferenceSetting) && category.MaritalPreferenceCaption.Trim() != string.Empty;
            showAgeGroup = Boolean.Parse(ShowAgeGroupSetting) && category.AgeGroupCaption.Trim() != string.Empty;
            showAverageAge = Boolean.Parse(ShowAverageAgeSetting);
            showCity = Boolean.Parse(ShowCitySetting);
            showDescription = Boolean.Parse(ShowDescriptionSetting) && category.DescriptionCaption.Trim() != string.Empty;
            showNotes = Boolean.Parse(ShowNotesSetting) && category.NotesCaption.Trim() != string.Empty;

            pnlMap.Controls.Clear();

            if (Request.Url.Port == 443)
                Page.ClientScript.RegisterStartupScript(typeof(string), "VirtualEarth", "<script src=\"https://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6&s=1\"></script>", false);
            else
                Page.ClientScript.RegisterStartupScript(typeof(string), "VirtualEarth", "<script src=\"http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6\"></script>", false);

            pnlMap.Style.Add("position", "relative");
            pnlMap.Style.Add("width", string.Format("{0}px", MapWidthSetting));
            pnlMap.Style.Add("height", string.Format("{0}px", MapHeightSetting));

            StringBuilder sbVEScript = new StringBuilder();

            sbVEScript.Append("var map = null;\n");
            sbVEScript.Append("var mapGroupLayer = null;\n");

            sbVEScript.Append("window.onload = function() {LoadMyMap();};\n");

            sbVEScript.Append("\nfunction LoadMyMap(){\n");
            sbVEScript.AppendFormat("\tmap = new VEMap('{0}');\n", pnlMap.ClientID);

            switch (MapDashboardSizeSetting)
            {
                case "Normal":
                    sbVEScript.Append("\tmap.SetDashboardSize(VEDashboardSize.Normal);\n");
                    break;
                case "Small":
                    sbVEScript.Append("\tmap.SetDashboardSize(VEDashboardSize.Small);\n");
                    break;
                case "Tiny":
                    sbVEScript.Append("\tmap.SetDashboardSize(VEDashboardSize.Tiny);\n");
                    break;
            }

            sbVEScript.Append("\tmap.LoadMap();\n\n");
            //sbVEScript.Append("\tmap.ClearInfoBoxStyles();\n\n");

            Address selectedAddress = null;
            if (Session["PersonAddress"] != null)
                selectedAddress = (Address)Session["PersonAddress"];

            if (selectedAddress == null && CurrentPerson != null)
                selectedAddress = CurrentPerson.PrimaryAddress;

            if (selectedAddress != null &&
                selectedAddress.Latitude != 0 &&
                selectedAddress.Longitude != 0)
            {
                sbVEScript.AppendFormat("\n\tshape = new VEShape(VEShapeType.Pushpin, new VELatLong({0}, {1}));\n",
                    selectedAddress.Latitude.ToString(),
                    selectedAddress.Longitude.ToString());
                sbVEScript.AppendFormat("\tshape.SetCustomIcon('{0}');\n", AddressIconSetting);
                sbVEScript.AppendFormat("\tshape.SetTitle(\"{0}\");\n", Utilities.replaceCRLF(selectedAddress.ToString()));
                sbVEScript.Append("\tmap.AddShape(shape);\n");
            }

            if (Area != null && Area.AreaID != -1)
            {
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

                sbVEScript.AppendFormat("\tshape.SetLineColor(new VEColor({0}));\n", AreaBorderColorSetting);
                sbVEScript.AppendFormat("\tshape.SetLineWidth({0});\n", AreaBorderWidthSetting);
                sbVEScript.AppendFormat("\tshape.SetFillColor(new VEColor({0}));\n", AreaBackgroundColorSetting);
                sbVEScript.Append("\tshape.HideIcon();\n");
                sbVEScript.AppendFormat("\tshape.SetTitle('{0}');\n", Area.Name);
                sbVEScript.Append("\n\tmap.AddShape(shape);\n");

                sbVEScript.Append("\n\tmapGroupLayer = new VEShapeLayer();\n");
                sbVEScript.Append("\tmapGroupLayer.SetTitle('Groups');\n");
                sbVEScript.Append("\tmap.AddShapeLayer(mapGroupLayer);\n");

                mapGroupCount = 0;
                GroupCollection groups = new GroupCollection();
                groups.LoadByArea(Area.AreaID, category.CategoryID);
                foreach (Group group in groups)
                {
                    if (group.Active && ( clusterTypeFilterID == -1 || group.ClusterTypeID == clusterTypeFilterID ))
                    {
                        if (Boolean.Parse(ShowFullGroupsSetting) == false)
                        {
                            int groupCount = (group.LeaderID == -1 ? 0 : 1);
                            foreach (GroupMember member in group.Members)
                                if (member.Active)
                                    groupCount++;

                            if (groupCount < group.MaxMembers)
                                sbVEScript.Append(GroupShape(group));
                        }
                        else
                            sbVEScript.Append(GroupShape(group));
                    }
                }
            }
            else
            {
                GroupClusterCollection clusters = new GroupClusterCollection(category.CategoryID, CurrentOrganization.OrganizationID);
                sbVEScript.Append(RecurseClusters(clusters));

                if (selectedAddress != null &&
                    selectedAddress.Latitude != 0 &&
                    selectedAddress.Longitude != 0)
                {
                    sbVEScript.AppendFormat("\tmap.SetCenterAndZoom(new VELatLong({0}, {1}),12);\n", selectedAddress.Latitude, selectedAddress.Longitude);
                }
                else
                {
                    sbVEScript.Append("\tvar maxPoints = new Array(\n");
                    sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1}),\n", minLatitude.ToString(), minLongitude.ToString());
                    sbVEScript.AppendFormat("\t\tnew VELatLong({0}, {1}))\n", maxLatitude.ToString(), maxLongitude.ToString());
                    sbVEScript.Append("\tmap.SetMapView(maxPoints);\n");
                    //sbVEScript.Append("\tmap.ZoomIn();\n\n");
                }
            }

            sbVEScript.Append("}\n");

            sbVEScript.Append("\nfunction ShowGroup(shapeIndex){\n");
            sbVEScript.Append("\tvar shape = mapGroupLayer.GetShapeByIndex(shapeIndex);\n");
            sbVEScript.Append("\tvar latLong = shape.GetPoints();\n");
            sbVEScript.Append("\tmap.SetCenterAndZoom(latLong[0], 15);\n");
            sbVEScript.Append("\tsetTimeout(\"ShowInfoBox(\" + shapeIndex + \")\", 1000);\n");
            sbVEScript.Append("}\n");

            sbVEScript.Append("\nfunction ShowInfoBox(shapeIndex){\n");
            sbVEScript.Append("\tvar shape = mapGroupLayer.GetShapeByIndex(shapeIndex);\n");
            sbVEScript.Append("\tvar latLong = shape.GetPoints();\n");
            sbVEScript.Append("\tmap.ShowInfoBox(shape, latLong[0]);\n");
            sbVEScript.Append("}\n");

            // add Safari fix
            sbVEScript.Append("VEValidator.ValidateFloat = function(float) {");
            sbVEScript.Append("\treturn true;");
            sbVEScript.Append("}");

            Page.ClientScript.RegisterStartupScript(typeof(string), "LoadMap", sbVEScript.ToString(), true);
        }

        private void FormatListColumns()
        {
            gvGroups.Columns[0].HeaderText = ArenaTextTools.Pluralize(category.CategoryName);

            gvGroups.Columns[1].Visible = showDescription;
            gvGroups.Columns[1].HeaderText = category.DescriptionCaption;

            gvGroups.Columns[2].Visible = showGroupType;

            gvGroups.Columns[3].Visible = showMeetingDay;
            gvGroups.Columns[3].HeaderText = category.MeetingDayCaption;

            gvGroups.Columns[4].Visible = showMeetingTime;

            gvGroups.Columns[5].Visible = showTopic;
            gvGroups.Columns[5].HeaderText = category.TopicCaption;
            
            gvGroups.Columns[6].Visible = showMaritalPreference;
            gvGroups.Columns[6].HeaderText = category.MaritalPreferenceCaption;
            
            gvGroups.Columns[7].Visible = showAgeGroup;
            gvGroups.Columns[7].HeaderText = category.AgeGroupCaption;
            
            gvGroups.Columns[8].Visible = showAverageAge;
            
            gvGroups.Columns[9].Visible = showCity;

            gvGroups.Columns[10].Visible = showNotes;
            gvGroups.Columns[10].HeaderText = category.NotesCaption;
        }

        private void BindList()
        {
            gvGroups.DataSource = allGroups;
            gvGroups.DataBind();
        }

        private void SortGroups(string sort)
        {
            DataTable dt = allGroups.DataTable();
            if (sort != string.Empty)
                dt.DefaultView.Sort = sort;

            allGroups = new GroupCollection();
            foreach (DataRowView drv in dt.DefaultView)
            {
                Group group = new Group((int)drv["GroupID"]);
                group.MatchScore = Convert.ToDecimal(drv["MatchScore"]);
                allGroups.Add(group);
            }
        }
        
        private string RecurseClusters(GroupClusterCollection groupClusters)
        {
            StringBuilder sb = new StringBuilder();

            if (groupClusters != null && groupClusters.Count > 0)
                foreach (GroupCluster cluster in groupClusters)
                {
                    foreach (Group group in cluster.SmallGroups)
                        sb.Append(GroupShape(group));

                    sb.Append(RecurseClusters(cluster.ChildClusters));
                }

            return sb.ToString();
        }

        private string GroupShape(Group group)
        {
            StringBuilder sb = new StringBuilder();

            if (!group.Private &&
                group.Active &&
                group.ClusterType.Category.CategoryID == category.CategoryID)
            {
                if (group.TargetLocation != null &&
                    group.TargetLocation.AddressID != -1 &&
                    group.TargetLocation.Latitude != 0 &&
                    group.TargetLocation.Longitude != 0)
                {
                    double latitude = group.TargetLocation.Latitude;
                    double longitude = group.TargetLocation.Longitude;

                    if (maxLatitude == double.MinValue || maxLatitude < latitude) maxLatitude = latitude;
                    if (maxLongitude == double.MinValue || maxLongitude < longitude) maxLongitude = longitude;
                    if (minLatitude == double.MinValue || minLatitude > latitude) minLatitude = latitude;
                    if (minLongitude == double.MinValue || minLongitude > longitude) minLongitude = longitude;

                    sb.AppendFormat("\n\tshape = new VEShape(VEShapeType.Pushpin, new VELatLong({0}, {1}));\n", latitude.ToString(), longitude.ToString());
                    sb.AppendFormat("\tshape.SetCustomIcon('{0}');\n", GroupIconSetting);
                    sb.AppendFormat("\tshape.SetTitle(\"{0}\");\n", BuildDetailTitle(group.Title));
                    sb.AppendFormat("\tshape.SetDescription(\"{0}\");\n", BuildDetailPanel(group));
                    sb.Append("\tmapGroupLayer.AddShape(shape);\n");

                    group.MatchScore = mapGroupCount;
                    mapGroupCount++;
                }

                allGroups.Add(group);
            }

            return sb.ToString();
        }

        private string BuildDetailTitle(string title)
        {
            return string.Format("<div style='text-align:left' class='heading2'>{0}</div>", title);
        }

        private string BuildDetailPanel(Group group)
        {
            StringBuilder sbGroup = new StringBuilder();

            sbGroup.Append("<div class='mapGroupDetail' style='text-align:left'>");

            // Leaders's Photo
            if (group.Leader != null && group.Leader.Blob != null && group.Leader.Blob.BlobID != -1)
                sbGroup.AppendFormat("<div class='mapGroupImage'><img src='cachedblob.aspx?guid={0}&width=120&height=120' border='1'></div>", group.Leader.Blob.GUID.ToString());

            string divFormat = "<div><span class='formLabel'>{0}: </span><span class='formItem'>{1}</span></div>";

            // Group Type
            if (showGroupType)
                sbGroup.AppendFormat(divFormat, "Group Type", group.GroupType.ToString());

            // Meeting Day
            if (showMeetingDay)
                sbGroup.AppendFormat(divFormat, category.MeetingDayCaption, group.MeetingDay.Value);

            // Meeting Time
            if (showMeetingTime)
                sbGroup.AppendFormat(divFormat, "Meeting Time", group.Schedule.ToString().Replace("\r\n", ""));

            // Topic
            if (showTopic)
                sbGroup.AppendFormat(divFormat, category.TopicCaption, group.Topic.Value);

            // Marital Preference
            if (showMaritalPreference)
                sbGroup.AppendFormat(divFormat, category.MaritalPreferenceCaption, group.PrimaryMaritalStatus.Value);

            // Age Group
            if (showAgeGroup)
                sbGroup.AppendFormat(divFormat, category.AgeGroupCaption, group.PrimaryAge.Value);

            // Average Age
            if (showAverageAge)
                sbGroup.AppendFormat(divFormat, "Average Age", group.AverageAge.ToString());

            // City 
            if (showCity)
                sbGroup.AppendFormat(divFormat, "City", group.TargetLocation.City.ToString());

            // Description
            if (showDescription && group.Description.Trim() != string.Empty)
            {
                sbGroup.AppendFormat("<div class='formLabel' style='padding-top:10px'>{0}: </div>", category.DescriptionCaption);
                sbGroup.AppendFormat("<div class='formItem' style='padding-left:10px'>{0}</div>", Utilities.replaceCRLF(group.Description.Trim().Replace("\"", "")));
            }

            // Notes
            if (showNotes && group.Notes.Trim() != string.Empty)
            {
                sbGroup.AppendFormat("<div class='formLabel' style='padding-top:10px'>{0}: </div>", category.NotesCaption);
                sbGroup.AppendFormat("<div class='formItem' style='padding-left:10px'>{0}</div>", Utilities.replaceCRLF(group.Notes.Trim().Replace("\"", "")));
            }

            // Registration Link
            if (category.AllowRegistrations)
            {
                string urlString = string.Format("default.aspx?page={0}&group={1}", RegistrationPageIDSetting, group.GroupID.ToString());
                string redirectString = string.Format("default.aspx?page={0}&requestUrl={1}&group={2}", UserConfirmPageIDSetting, HttpUtility.UrlEncode(HttpUtility.UrlEncode(urlString)), group.GroupID.ToString());

                sbGroup.AppendFormat("<div style='padding-top:20px'><a href='{0}'>Register for this {1}</a></div>", redirectString, category.CategoryName);
            }

            sbGroup.Append("</div>");

            return sbGroup.ToString();
            //.Replace("'", "");
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
            gvGroups.RowDataBound += new GridViewRowEventHandler(gvGroups_RowDataBound);
            gvGroups.RowCommand += new GridViewCommandEventHandler(gvGroups_RowCommand);
            gvGroups.Sorting += new GridViewSortEventHandler(gvGroups_Sorting);
        }

        #endregion
    }
}
