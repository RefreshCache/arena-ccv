namespace ArenaWeb.UserControls.Custom.CCV.SmallGroup
{
	using System;
	using System.Xml;
    using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
    using System.Text;
	using System.Web;
    using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using Arena.Core;
    using Arena.DataLayer.Core;
	using Arena.Exceptions;
	using Arena.Portal;
	using Arena.Portal.UI;
    using Arena.SmallGroup;
    using Arena.Utility;

	/// <summary>
	///		Summary description for SubscribedProfileList.
	/// </summary>
	public partial class Attendance : PortalControl
	{
        #region Module Settings

        [PageSetting("Member Add Page", "The page used to submit a request to have a new person added to the group.", false)]
        public string MemberAddPageSetting { get { return Setting("MemberAddPage", "", false); } }

        [ListFromSqlSetting("Occurrence Type", "The occurrence type that should be used when adding new occurrences.", true, "", 
            "select t.occurrence_type_id, g.group_name + ' - ' + t.type_name from core_occurrence_type t inner join core_occurrence_type_group g on g.group_id = t.group_id order by g.group_name, t.type_order")]
        public string OccurrenceTypeSetting { get { return Setting("OccurrenceType", "", true); } }

        #endregion

        #region Private Variables

        private OccurrenceData occurrenceData = new OccurrenceData();

        private Person person = null;
        private Group group = null;
        private DateTime groupDate = DateTime.MinValue;

        #endregion

        #region Events

        protected void Page_Load(object sender, System.EventArgs e)
        {
            pnlMessage.Visible = false;
            aAddPerson.Visible = MemberAddPageSetting != string.Empty;

            person = CurrentPerson;

            if (CurrentPerson == null)
            {
                try
                {
                    Guid personGuid = new Guid(Request.QueryString["p"].Substring(36, 36));
                    int personId = Int32.Parse(Request.QueryString["p"].Substring(72));

                    if (personId > 0)
                    {
                        person = new Person(personGuid);
                        if (person != null && person.PersonID != personId)
                            person = null;
                    }
                }
                catch { }
            }

            if (Page.IsPostBack)
            {
                if (ViewState["GroupID"] != null)
                    group = new Group((int)ViewState["GroupID"]);
                if (ViewState["GroupDate"] != null)
                    groupDate = (DateTime)ViewState["GroupDate"];
            }
            else
            {
                try
                {
                    if (Request.QueryString["group"] != null && CurrentPerson != null)
                    {
                        group = new Group(Int32.Parse(Request.QueryString["group"]));
                        if (!group.Allowed(Arena.Security.OperationType.Edit, CurrentUser, CurrentPerson))
                            group = null;
                    }
                    else
                    {
                        Guid groupGuid = new Guid(Request.QueryString["p"].Substring(0, 36));
                        group = new Group(groupGuid);
                    }
                }
                catch { }

                if (group != null && group.GroupID != -1)
                {
                    RegisterScripts();

                    if (group.ClusterType.AllowOccurrences)
                    {
                        int today = (int)DateTime.Today.DayOfWeek;
                        int meetingDay = 0;
                        switch (group.MeetingDay.Value.ToLower())
                        {
                            case "sun": meetingDay = 0; break;
                            case "mon": meetingDay = 1; break;
                            case "tue": meetingDay = 2; break;
                            case "wed": meetingDay = 3; break;
                            case "thu": meetingDay = 4; break;
                            case "fri": meetingDay = 5; break;
                            case "sat": meetingDay = 6; break;
                        }

                        if (meetingDay > today)
                            today += 7;

                        groupDate = DateTime.Today.AddDays(0 - (today - meetingDay));
                        ShowAttendance();

                        ViewState.Add("GroupID", group.GroupID);
                    }
                    else
                    {
                        ShowMessage("The selected group does not support attendance.", true);
                        group = null;
                    }
                }
                else
                    ShowMessage("Invalid group specified on query string", true);
            }

            
        }

        protected void lbPrev_Click(object sender, EventArgs e)
        {
            groupDate = groupDate.AddDays(-7);
            ShowAttendance();
        }

        protected void lbNext_Click(object sender, EventArgs e)
        {
            DateTime nextDate = groupDate.AddDays(7);
            if (nextDate <= DateTime.Today)
                groupDate = nextDate;

            ShowAttendance();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GroupOccurrence occurrence = FindOccurrenceByDate(group.Occurrences, groupDate);

            if (occurrence == null)
            {
                occurrence = new GroupOccurrence();
                occurrence.GroupID = group.GroupID;
                occurrence.OccurrenceID = -1;
                occurrence.OccurrenceTypeID = Convert.ToInt32(OccurrenceTypeSetting);
                occurrence.Name = group.Title;
                occurrence.StartTime = groupDate;
                occurrence.EndTime = groupDate;
                group.Occurrences.Add(occurrence);
            }

            occurrence.OccurrenceClosed = cbNoMeet.Checked;
            occurrence.Save(CurrentUser.Identity.Name);

            if (occurrence.OccurrenceClosed)
            {
                foreach (OccurrenceAttendance attendance in occurrence.OccurrenceAttendances)
                    attendance.Delete();
            }
            else
            {
                foreach (ListViewDataItem item in lvMembers.Items)
                {
                    CheckBox cb = (CheckBox)item.FindControl("cbMember");
                    if (cb != null)
                    {
                        int personId = Int32.Parse(cb.Attributes["memberid"]);
                        OccurrenceAttendance attendance = new OccurrenceAttendance(occurrence.OccurrenceID, personId);
                        attendance.OccurrenceID = occurrence.OccurrenceID;
                        attendance.PersonID = personId;
                        attendance.Attended = cb.Checked;

                        if (attendance.Attended)
                            attendance.Save(CurrentUser.Identity.Name);
                        else
                        {
                            if (attendance.OccurrenceAttendanceID != -1)
                            {
                                if (attendance.PersonID == -1 || attendance.Notes == string.Empty)
                                    attendance.Delete();
                                else
                                    attendance.Save(CurrentUser.Identity.Name);
                            }
                        }
                    }
                }
            }

            ShowMessage(string.Format("Attendance has been saved for {0}", groupDate.ToLongDateString()), false);

            ShowAttendance();
        }

        protected void lvMembers_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                GroupMember member = group.Members.FindByID(Convert.ToInt32(e.CommandArgument));
                if (member != null)
                {
                    ShowMessage(string.Format("{0} has been removed from this group.", member.FullName), false);
                    member.Delete(CurrentOrganization.OrganizationID, CurrentUser.Identity.Name);
                }
            }

            ShowAttendance();
        }

        protected void btnAddPerson_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region Private Methods

        private void RegisterScripts()
        {
            if (MemberAddPageSetting != string.Empty)
            {
                StringBuilder sbGroupPath = new StringBuilder();
                sbGroupPath.Append(group.Title);
                GroupCluster gc = new GroupCluster(group.GroupClusterID);

                while (gc != null && gc.GroupClusterID != -1)
                {
                    sbGroupPath.Insert(0, gc.Title + " | ");
                    gc = new GroupCluster(gc.ParentClusterID);
                }

                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("\n\n<script language=\"javascript\">\n");

                sbScript.Append("\tfunction openAddWindow()\n");
                sbScript.Append("\t{\n");

                sbScript.AppendFormat("\t\twindow.location.href='default.aspx?page={0}&group_id={1}&referral_url={2}&group_path={3}';\n",
                    MemberAddPageSetting,
                    group.GroupID.ToString(),
                    HttpUtility.UrlEncode(Request.RawUrl).Replace("'", "\\'"),
                    HttpUtility.UrlEncode(sbGroupPath.ToString()).Replace("'", "\\'"));

                sbScript.Append("\t}\n");

                sbScript.Append("</script>\n\n");

                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "openAddWindow", sbScript.ToString());
            }
        }

        private GroupOccurrence FindOccurrenceByDate(GroupOccurrenceCollection occurrences, DateTime date)
        {
            foreach (GroupOccurrence groupOcc in occurrences)
                if (groupOcc.StartTime.Date == date)
                    return groupOcc;
            return null;
        }

        private void ShowAttendance()
        {
            lMtgDate.Text = groupDate.ToLongDateString();
            lbNext.Visible = groupDate.AddDays(7) <= DateTime.Today;

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("person_id", typeof(Int32)));
            dt.Columns.Add(new DataColumn("person_name", typeof(string)));
            dt.Columns.Add(new DataColumn("attended", typeof(bool)));

            List<int> people = new List<int>();

            GroupOccurrence occurrence = FindOccurrenceByDate(group.Occurrences, groupDate);
            if (occurrence != null)
            {
                cbNoMeet.Checked = occurrence.OccurrenceClosed;

                SqlDataReader rdr = occurrenceData.GetOccurrenceAttendanceByOccurrenceID(occurrence.OccurrenceID, -1);
                while (rdr.Read())
                {
                    int personId = (int)rdr["person_id"];
                    people.Add(personId);

                    dt.Rows.Add(BuildAttendanceRow(dt,
                        personId,
                        rdr["person_name"].ToString(),
                        (bool)rdr["attended"]));
                }
                rdr.Close();
            }
            else
                cbNoMeet.Checked = false;

            if (group.Leader.PersonID != -1 && !people.Contains(group.Leader.PersonID))
                dt.Rows.Add(BuildAttendanceRow(dt, group.Leader.PersonID, group.Leader.FullName, false));

            foreach (GroupMember member in group.Members)
                if (member.Active && !people.Contains(member.PersonID))
                    dt.Rows.Add(BuildAttendanceRow(dt, member.PersonID, member.FullName, false));

            lvMembers.DataSource = dt;
            lvMembers.DataBind();

            ViewState["GroupDate"] = groupDate;
        }

        private DataRow BuildAttendanceRow(DataTable dataTable, int person_id, string person_name, bool attended)
        {
            DataRow row = dataTable.NewRow();
            row["person_id"] = person_id;
            row["person_name"] = person_name;
            row["attended"] = attended;
            return row;
        }

        private void ShowMessage(string message, bool hideDetails )
        {
            pnlMessage.Controls.Clear();
            pnlMessage.Controls.Add(new LiteralControl(message));
            pnlMessage.Visible = true;

            phDetails.Visible = !hideDetails;
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
