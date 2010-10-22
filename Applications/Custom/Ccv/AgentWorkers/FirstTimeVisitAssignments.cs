using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

using Agent;
using Arena.Core;
using Arena.Assignments;
using Arena.Portal;

namespace Arena.Custom.CCV.AgentWorkers
{
    [Serializable]
    [Description("Agent to create new assignments for people who have recently visited for the first time. " + 
        "Can create one of three different assignment types for each family depending on if family includes children " +
        "who checked in for the first time, and/or if they indicated they are interested in neighborhood groups.")]
    public class FirstTimeVisitAssignments : AgentWorker
    {
        const int STATE_OK = 0;
    
        // private fields
        //private int _groupVisitAssignmentType = -1;
        private int _childVisitAssignmentType = -1;
        private int _juniorHighVisitAssignmentType = -1;
        private int _highSchoolVisitAssignmentType = -1;
        private int _adultVisitAssignmentType = -1;

        private int _firstTimeVisitAttributeID = -1;
        //private int _interestedInGroupAttributeID = -1;
        private int _daysBack = 7;
        private int _regionPastorRoleID = -1;
        private int _neighborhoodLeaderRoleID = -1;

        private int _regionalPastorID = -1;

        private int _juniorHighAttendanceType = -1;
        private int _highSchoolAttendanceType = -1;

        //AssignmentType groupAssignmentType = null;
        AssignmentType childAssignmentType = null;
        AssignmentType juniorHighAssignmentType = null;
        AssignmentType highSchoolAssignmentType = null;
        AssignmentType adultAssignmentType = null;

        //AssignmentTypeField groupFamilyIDField = null;
        //AssignmentTypeField groupFirstVisitDateField = null;

        AssignmentTypeField childFamilyIDField = null;
        AssignmentTypeField childFirstVisitDateField = null;

        AssignmentTypeField juniorHighFamilyIDField = null;
        AssignmentTypeField juniorHighFirstVisitDateField = null;

        AssignmentTypeField highSchoolFamilyIDField = null;
        AssignmentTypeField highSchoolFirstVisitDateField = null;

        AssignmentTypeField adultFamilyIDField = null;
        AssignmentTypeField adultFirstVisitDateField = null;

        #region Agent Settings

        //[NumericSetting("Neighborhood Group Assignment Type", "The Assignment Type ID for families who indicated they are interested in a Neighborhood Group.", true)]
        //[Description("The Assignment Type ID for families who indicated they are interested in a Neighborhood Group.")]
        //public int GroupVisitAssignmentType { get { return _groupVisitAssignmentType; } set { _groupVisitAssignmentType = value; } }

        [NumericSetting("Child Assignment Type", "The Assignment Type ID for families with children who checked in for the first time.", true)]
        [Description("The Assignment Type ID for families with children who checked in for the first time.")]
        public int ChildVisitAssignmentType { get { return _childVisitAssignmentType; } set { _childVisitAssignmentType = value; } }

        [NumericSetting("Junior High Assignment Type", "The Assignment Type ID for Junior High first time visitors.", true)]
        [Description("The Assignment Type ID for Junior High first time visitors.")]
        public int JuniorHighVisitAssignmentType { get { return _juniorHighVisitAssignmentType; } set { _juniorHighVisitAssignmentType = value; } }

        [NumericSetting("High School Assignment Type", "The Assignment Type ID for High School first time visitors.", true)]
        [Description("The Assignment Type ID for High School first time visitors.")]
        public int HighSchoolVisitAssignmentType { get { return _highSchoolVisitAssignmentType; } set { _highSchoolVisitAssignmentType = value; } }

        [NumericSetting("Adult Assignment Type", "The Assignment Type ID for families where checkin was not used.", true)]
        [Description("The Assignment Type ID for families where checkin was not used.")]
        public int AdultVisitAssignmentType { get { return _adultVisitAssignmentType; } set { _adultVisitAssignmentType = value; } }

        [NumericSetting("First Time Visit Attribute", "The ID of the First Time Visit Attribute.", true)]
        [Description("The ID of the First Time Visit Attribute.")]
        public int FirstTimeVisitAttributeID { get { return _firstTimeVisitAttributeID; } set { _firstTimeVisitAttributeID = value; } }

        //[NumericSetting("Interested In Group Attribute", "The ID of the Interested in Neighborhood Group Attribute.", true)]
        //[Description("The ID of the Interested in Neighborhood Group Attribute.")]
        //public int InterestedInGroupAttributeID { get { return _interestedInGroupAttributeID; } set { _interestedInGroupAttributeID = value; } }

        [NumericSetting("Junior High Attendance Type ID", "The Attendance Type ID for Junior High.", true)]
        [Description("The Attendance Type ID for Junior High..")]
        public int JuniorHighAttendanceTypeID { get { return _juniorHighAttendanceType; } set { _juniorHighAttendanceType = value; } }

        [NumericSetting("High School Attendance Type ID", "The Attendance Type ID for High School.", true)]
        [Description("The Attendance Type ID for High School..")]
        public int HighSchoolAttendanceTypeID { get { return _highSchoolAttendanceType; } set { _highSchoolAttendanceType = value; } }

        [NumericSetting("Days Back", "Number of days back to look for first time visitors (default = 7)", false)]
        [Description("Number of days back to look for first time visitors (default = 7)")]
        public int DaysBack { get { return _daysBack; } set { _daysBack = value; } }

        [NumericSetting("Regional Pastor Role ID", "The Area Role ID of Regional Pastors", false)]
        [Description("The Area Role ID of Regional Pastors")]
        public int RegionalPastorRoleID { get { return _regionPastorRoleID; } set { _regionPastorRoleID = value; } }

        [NumericSetting("Neighborhood Leader Role ID", "The Area Role ID of Neighborhood Leaders", false)]
        [Description("The Area Role ID of Neighborhood Leaders")]
        public int NeighborhoodLeaderRoleID { get { return _neighborhoodLeaderRoleID; } set { _neighborhoodLeaderRoleID = value; } }

        #endregion

        public override WorkerResult Run(bool previousWorkersActive)
        {
            try
            {
                int state;
                string message;

                if (Convert.ToBoolean(Enabled))
                {
                    if (RunIfPreviousWorkersActive || !previousWorkersActive)
                    {
                        WorkerResultStatus status = ProcessVisitors(out message, out state);
                        return new WorkerResult(state, status, string.Format(Description), message);
                    }
                    else
                        return new WorkerResult(STATE_OK, WorkerResultStatus.Abort, string.Format(Description), "Did not run because previous worker instance still active.");
                }
                else
                    return new WorkerResult(STATE_OK, WorkerResultStatus.Abort, string.Format(Description), "Did not run because worker not enabled.");
            }
            catch (Exception e)
            {
                // handle special exceptions here...
                throw (e);
            }
        }

        public WorkerResultStatus ProcessVisitors(out string message, out int state)
        {
            Trace.Write("Starting FirstTimeVisitAssignments Agent...\n");

            WorkerResultStatus workerResultStatus = WorkerResultStatus.Ok;
            message = string.Empty;
            state = STATE_OK;

            try
            {
                //groupAssignmentType = new AssignmentType(GroupVisitAssignmentType);
                //if (groupAssignmentType.AssignmentTypeId == -1)
                //    throw new Exception("Invalid 'Neighborhood Group Assignment Type' setting");

                childAssignmentType = new AssignmentType(ChildVisitAssignmentType);
                if (childAssignmentType.AssignmentTypeId == -1)
                    throw new Exception("Invalid 'Child Assignment Type' setting");

                juniorHighAssignmentType = new AssignmentType(JuniorHighVisitAssignmentType);
                if (juniorHighAssignmentType.AssignmentTypeId == -1)
                    throw new Exception("Invalid 'Junior High Assignment Type' setting");

                highSchoolAssignmentType = new AssignmentType(HighSchoolVisitAssignmentType);
                if (highSchoolAssignmentType.AssignmentTypeId == -1)
                    throw new Exception("Invalid 'High School Assignment Type' setting");

                adultAssignmentType = new AssignmentType(AdultVisitAssignmentType);
                if (adultAssignmentType.AssignmentTypeId == -1)
                    throw new Exception("Invalid 'Adult Assignment Type' setting");

                //groupFamilyIDField = GetAssignmentTypeField(groupAssignmentType, "Family ID");
                //groupFirstVisitDateField = GetAssignmentTypeField(groupAssignmentType, "First Visit Date");

                childFamilyIDField = GetAssignmentTypeField(childAssignmentType, "Family ID");
                childFirstVisitDateField = GetAssignmentTypeField(childAssignmentType, "First Visit Date");

                juniorHighFamilyIDField = GetAssignmentTypeField(juniorHighAssignmentType, "Family ID");
                juniorHighFirstVisitDateField = GetAssignmentTypeField(juniorHighAssignmentType, "First Visit Date");

                highSchoolFamilyIDField = GetAssignmentTypeField(highSchoolAssignmentType, "Family ID");
                highSchoolFirstVisitDateField = GetAssignmentTypeField(highSchoolAssignmentType, "First Visit Date");

                adultFamilyIDField = GetAssignmentTypeField(adultAssignmentType, "Family ID");
                adultFirstVisitDateField = GetAssignmentTypeField(adultAssignmentType, "First Visit Date");

                Lookup child = new Lookup(SystemLookup.FamilyRole_Child);

                DateTime beginDate = DateTime.Today.Date.AddDays(-DaysBack);

			    ArrayList lst = new ArrayList();
			    lst.Add(new SqlParameter("@AttributeID", FirstTimeVisitAttributeID));
			    lst.Add(new SqlParameter("@BeginDate", beginDate));
                //lst.Add(new SqlParameter("@GroupFamilyIDField", groupFamilyIDField.CustomFieldId));
                lst.Add(new SqlParameter("@ChildFamilyIDField", childFamilyIDField.CustomFieldId));
                lst.Add(new SqlParameter("@JuniorHighFamilyIDField", juniorHighFamilyIDField.CustomFieldId));
                lst.Add(new SqlParameter("@HighSchoolFamilyIDField", highSchoolFamilyIDField.CustomFieldId));
                lst.Add(new SqlParameter("@AdultFamilyIDField", adultFamilyIDField.CustomFieldId));

                Arena.DataLayer.Organization.OrganizationData oData = new Arena.DataLayer.Organization.OrganizationData();

                SqlDataReader rdr = oData.ExecuteReader("cust_ccv_recent_visitor_families", lst);

                while (rdr.Read())
                {
                    OccurrenceAttendanceCollection attendances = new OccurrenceAttendanceCollection();
                    //bool interestedInGroup = false;

                    Family family = new Family((int)rdr["family_id"]);

                    bool newFamily = true;
                    int childCount = 0;
                    int adultCount = 0;
                    bool attendedHS = false;
                    bool attendedJH = false;

                    foreach (FamilyMember fm in family.FamilyMembers)
                    {
                        PersonAttribute pa = (PersonAttribute)fm.Attributes.FindByID(FirstTimeVisitAttributeID);
                        if (pa != null && !pa.DateValue.IsEmptyDate() && pa.DateValue < beginDate)
                            newFamily = false;

                        if (newFamily)
                        {
                            if (fm.FamilyRole.LookupID == child.LookupID)
                                childCount++;
                            else
                                adultCount++;

                            //pa = (PersonAttribute)fm.Attributes.FindByID(InterestedInGroupAttributeID);
                            //if (pa != null && pa.IntValue == 1)
                            //    interestedInGroup = true;

                            ArrayList lstOccurrence = new ArrayList();
                            lstOccurrence.Add(new SqlParameter("@PersonID", fm.PersonID));
                            SqlDataReader rdrOccurrence = oData.ExecuteReader("cust_ccv_recent_visitor_first_checkin", lstOccurrence);
                            if (rdrOccurrence.Read())
                            {
                                if (((DateTime)rdrOccurrence["first_attended"]).Date >= beginDate)
                                {
                                    OccurrenceAttendance oa = new OccurrenceAttendance((int)rdrOccurrence["occurrence_attendance_id"]);
                                    if (oa.Occurrence.OccurrenceType.OccurrenceTypeId == HighSchoolAttendanceTypeID)
                                        attendedHS = true;
                                    else if (oa.Occurrence.OccurrenceType.OccurrenceTypeId == JuniorHighAttendanceTypeID)
                                        attendedJH = true;

                                    attendances.Add(oa);
                                }
                            }
                            rdrOccurrence.Close();
                        }
                    }

                    if (newFamily)
                    {
                        _regionalPastorID = -1;

                        Assignment assignment = new Assignment();
                        assignment.Title = family.FamilyName;
                        assignment.Description = BuildDescription(family, attendances);
                        assignment.RequesterPersonId = family.FamilyHead.PersonID;

                        //if (interestedInGroup)
                        //{
                        //    assignment.AssignmentTypeId = groupAssignmentType.AssignmentTypeId;
                        //    assignment.PriorityId = groupAssignmentType.DefaultPriorityId;
                        //    assignment.StatusId = groupAssignmentType.DefaultStatusId;

                        //    AssignmentFieldValue familyIDField = new AssignmentFieldValue(groupFamilyIDField.CustomFieldId);
                        //    familyIDField.SelectedValue = family.FamilyID.ToString();
                        //    assignment.FieldValues.Add(familyIDField);

                        //    AssignmentFieldValue firstVisitDateField = new AssignmentFieldValue(groupFirstVisitDateField.CustomFieldId);
                        //    firstVisitDateField.SelectedValue = ((DateTime)rdr["first_visit"]).ToShortDateString();
                        //    assignment.FieldValues.Add(firstVisitDateField);

                        //    if (_regionalPastorID != -1)
                        //        assignment.SubmitAssignmentEntry(childAssignmentType.Owner != null ? adultAssignmentType.Owner : family.FamilyHead, "FirstTimeVisitAssignment Agent", _regionalPastorID);
                        //    else
                        //        assignment.SubmitAssignmentEntry(childAssignmentType.Owner != null ? adultAssignmentType.Owner : family.FamilyHead, "FirstTimeVisitAssignment Agent");
                        //}
                        //else
                        //{

                        if (attendedHS)
                        {
                            assignment.AssignmentTypeId = highSchoolAssignmentType.AssignmentTypeId;
                            assignment.PriorityId = highSchoolAssignmentType.DefaultPriorityId;

                            AssignmentFieldValue familyIDField = new AssignmentFieldValue(highSchoolFamilyIDField.CustomFieldId);
                            familyIDField.SelectedValue = family.FamilyID.ToString();
                            assignment.FieldValues.Add(familyIDField);

                            AssignmentFieldValue firstVisitDateField = new AssignmentFieldValue(highSchoolFirstVisitDateField.CustomFieldId);
                            firstVisitDateField.SelectedValue = ((DateTime)rdr["first_visit"]).ToShortDateString();
                            assignment.FieldValues.Add(firstVisitDateField);

                            assignment.SubmitAssignmentEntry(highSchoolAssignmentType.Owner != null ? highSchoolAssignmentType.Owner : family.FamilyHead, "FirstTimeVisitAssignment Agent");
                        }
                        else if (attendedJH)
                        {
                            assignment.AssignmentTypeId = juniorHighAssignmentType.AssignmentTypeId;
                            assignment.PriorityId = juniorHighAssignmentType.DefaultPriorityId;

                            AssignmentFieldValue familyIDField = new AssignmentFieldValue(juniorHighFamilyIDField.CustomFieldId);
                            familyIDField.SelectedValue = family.FamilyID.ToString();
                            assignment.FieldValues.Add(familyIDField);

                            AssignmentFieldValue firstVisitDateField = new AssignmentFieldValue(juniorHighFirstVisitDateField.CustomFieldId);
                            firstVisitDateField.SelectedValue = ((DateTime)rdr["first_visit"]).ToShortDateString();
                            assignment.FieldValues.Add(firstVisitDateField);

                            assignment.SubmitAssignmentEntry(highSchoolAssignmentType.Owner != null ? juniorHighAssignmentType.Owner : family.FamilyHead, "FirstTimeVisitAssignment Agent");
                        }
                        else if (childCount == 0)
                        {
                            assignment.AssignmentTypeId = adultAssignmentType.AssignmentTypeId;
                            assignment.PriorityId = adultAssignmentType.DefaultPriorityId;

                            AssignmentFieldValue familyIDField = new AssignmentFieldValue(adultFamilyIDField.CustomFieldId);
                            familyIDField.SelectedValue = family.FamilyID.ToString();
                            assignment.FieldValues.Add(familyIDField);

                            AssignmentFieldValue firstVisitDateField = new AssignmentFieldValue(adultFirstVisitDateField.CustomFieldId);
                            firstVisitDateField.SelectedValue = ((DateTime)rdr["first_visit"]).ToShortDateString();
                            assignment.FieldValues.Add(firstVisitDateField);

                            assignment.SubmitAssignmentEntry(adultAssignmentType.Owner != null ? adultAssignmentType.Owner : family.FamilyHead, "FirstTimeVisitAssignment Agent");
                        }
                        else if (adultCount > 0)
                        {
                            assignment.AssignmentTypeId = childAssignmentType.AssignmentTypeId;
                            assignment.PriorityId = childAssignmentType.DefaultPriorityId;

                            AssignmentFieldValue familyIDField = new AssignmentFieldValue(childFamilyIDField.CustomFieldId);
                            familyIDField.SelectedValue = family.FamilyID.ToString();
                            assignment.FieldValues.Add(familyIDField);

                            AssignmentFieldValue firstVisitDateField = new AssignmentFieldValue(childFirstVisitDateField.CustomFieldId);
                            firstVisitDateField.SelectedValue = ((DateTime)rdr["first_visit"]).ToShortDateString();
                            assignment.FieldValues.Add(firstVisitDateField);

                            assignment.SubmitAssignmentEntry(childAssignmentType.Owner != null ? childAssignmentType.Owner : family.FamilyHead, "FirstTimeVisitAssignment Agent");
                        }
                    }
                }
                rdr.Close();

            }
            catch (Exception ex)
            {
                workerResultStatus = WorkerResultStatus.Exception;
                message = "An error occured while processing First Time Visitor Assignments.\n\nMessage\n------------------------\n" + ex.Message + "\n\nStack Trace\n------------------------\n" + ex.StackTrace;
            }
            finally
            {

            }

            return workerResultStatus;
        }

        private AssignmentTypeField GetAssignmentTypeField(AssignmentType assignmentType, string fieldTitle)
        {
            foreach (AssignmentTypeField field in assignmentType.Fields)
                if (field.Title.Trim().Replace(" ", "").ToLower() == fieldTitle.Trim().Replace(" ", "").ToLower())
                    return field;
            return null;
        }

        private string BuildDescription(Family family, OccurrenceAttendanceCollection attendances)
        {
            StringBuilder sb = new StringBuilder();

            FamilyMemberCollection adults = family.Adults();
            FamilyMemberCollection children = family.Children();
            Address primaryAddress = null;

            if (adults.Count > 0)
            {
                sb.Append("\nAdults:\n");
                foreach (FamilyMember adult in adults)
                {
                    sb.AppendFormat("+ {0}", adult.FullName, adult.Age.ToString());
                    if (primaryAddress == null)
                        primaryAddress = adult.PrimaryAddress;

                    if (adult.Age != -1)
                        sb.AppendFormat(" (Age {0})", adult.Age.ToString());

                    sb.Append("\n");
                }
            }

            if (children.Count > 0)
            {
                sb.Append("\nChildren:\n");
                foreach (FamilyMember child in children)
                {
                    sb.AppendFormat("+ {0}", child.FullName);

                    OccurrenceAttendance attendance = attendances.FindByID(child.PersonID);
                    if (attendance != null)
                        sb.AppendFormat(" (Checked into {0} {1} {2})", 
                            attendance.Occurrence.StartTime.DayOfWeek.ToString(), 
                            attendance.Occurrence.Name, 
                            attendance.Occurrence.OccurrenceType.TypeName);
                    else
                        if (child.Age != -1)
                            sb.AppendFormat(" (Age {0})", child.Age.ToString());

                    sb.Append("\n");

                    if (primaryAddress == null)
                        primaryAddress = child.PrimaryAddress;
                }
            }

            if (primaryAddress != null && primaryAddress.Area != null && primaryAddress.Area.AreaID != -1)
            {
                sb.AppendFormat("\nNeighborhood: {0}\n", primaryAddress.Area.Name);
                sb.Append(AreaCoordinators(primaryAddress.Area, RegionalPastorRoleID));
                sb.Append(AreaCoordinators(primaryAddress.Area, NeighborhoodLeaderRoleID));
            }
            sb.Append("\n");

            return sb.ToString();
        }

        private string AreaCoordinators(Area area, int roleID)
        {
            StringBuilder sb = new StringBuilder();
            foreach(AreaOutreachCoordinator leader in area.OutreachCoordinators)
                if (leader.AreaRoleId == roleID)
                {
                    if (_regionalPastorID == -1 && leader.AreaRoleId == RegionalPastorRoleID)
                        _regionalPastorID = leader.PersonId;

                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append(new Person(leader.PersonId).FullName);
                }

            if (sb.Length > 0)
            {
                Lookup leaderType = new Lookup(roleID);
                return string.Format("{0}: {1}\n", leaderType.Value, sb.ToString());
            }

            return string.Empty;
        }
    }
}