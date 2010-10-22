using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Arena.Assignments;
using Arena.Core;
using Arena.Core.Communications;
using Arena.DataLayer.Assignments;
using Arena.Portal;
using Arena.Utility;

namespace Arena.Custom.CCV.Data
{
    [Serializable]
    [Description("[CCV] Launch Assignment on Person Attribute Change")]
    [MonitorDataUpdate("core_person_attribute")]
    public class PersonAttributeChange : DataUpdateAction
    {
        [ListFromSqlSetting("Person Attribute Type", "Type of person attribute to monitor?", true, "-1", 
            "select a.attribute_id, g.group_name + ' - ' + a.attribute_name from core_attribute a inner join core_attribute_group g on g.attribute_group_id = a.attribute_group_id order by g.group_order, a.attribute_order")]
        public string AttributeIdSetting { get { return Setting("AttributeId", "-1", true); } }

        [ListFromSqlSetting("Assignment Type", "Type of assignment to launch?", true, "-1",
            "select assignment_type_id, REPLICATE('-',3*level) + name as name from cust_ccv_v_asgn_assignment_type_heirarchy order by sort_order")]
        public string AssignmentTypeIdSetting { get { return Setting("AssignmentTypeId", "-1", true); } }

        [NumericSetting("Ignore Date Older Than Days", "If Attribute is a Date type, ignore any new value older than this number of days", false)]
        public string IgnoreDateDaysSetting { get { return Setting("IgnoreDateDays", "-1", false); } }

        [TextSetting("Specific Value", "Only start a new assignment if the person attribute's new value is set to this specific value", false)]
        public string SpecificValueSetting { get { return Setting("SpecificValue", "", false); } }

        public override bool PerformAction(DataUpdate dataUpdate)
        {
            // Create objects based on setting
            Arena.Core.Attribute attribute = new Arena.Core.Attribute((Convert.ToInt32(AttributeIdSetting)));

            // Add primary key to 'Before' dataset so that it can be searched 
            if (dataUpdate.DataBefore != null)
            {
                DataColumn[] primaryKey = new DataColumn[2];
                primaryKey[0] = dataUpdate.DataBefore.Columns["person_id"];
                primaryKey[1] = dataUpdate.DataBefore.Columns["attribute_id"];
                dataUpdate.DataBefore.PrimaryKey = primaryKey;
            }

            // Loop through each new row
            if (dataUpdate.DataAfter != null)
                foreach (DataRow newRow in dataUpdate.DataAfter.Rows)
                {
                    // If this update is for the attribute type we care about
                    if (Convert.ToInt32(newRow["attribute_id"].ToString()) == attribute.AttributeId)
                    {
                        // Load New Person Attribute
                        PersonAttribute newPA = new PersonAttribute(attribute.AttributeId);
                        newPA.PersonID = Convert.ToInt32(newRow["person_id"].ToString());
                        if (newRow["int_value"] != DBNull.Value)
                            newPA.IntValue = Convert.ToInt32(newRow["int_value"].ToString());
                        if (newRow["decimal_value"] != DBNull.Value)
                            newPA.DecimalValue = Convert.ToDecimal(newRow["decimal_value"].ToString());
                        if (newRow["varchar_value"] != DBNull.Value)
                            newPA.StringValue = newRow["varchar_value"].ToString();
                        if (newRow["datetime_value"] != DBNull.Value)
                            newPA.DateValue = Convert.ToDateTime(newRow["datetime_value"].ToString());

                        // If this is a date attribute and the ignore date setting is set, make sure the date setting is not too old
                        if (IgnoreDateDaysSetting == "-1" ||
                            newPA.AttributeType != Arena.Enums.DataType.DateTime ||
                            newPA.DateValue.AddDays(Convert.ToInt32(IgnoreDateDaysSetting)).CompareTo(DateTime.Today) >= 0)
                        {
                            // If we don't need a specific value or the value is equal to our test value
                            if (SpecificValueSetting == string.Empty || newPA.ToString().Trim().ToLower() == SpecificValueSetting.Trim().ToLower())
                            {
                                // Look for the previous version of this row
                                DataRow oldRow = null;
                                if (dataUpdate.DataBefore != null)
                                {
                                    object[] key = new object[2];
                                    key[0] = newPA.PersonID;
                                    key[1] = newPA.AttributeId;
                                    oldRow = dataUpdate.DataBefore.Rows.Find(key);
                                }

                                string title = string.Empty;
                                string description = string.Empty;

                                // If a previous version doesn't exist (was an add)
                                if (oldRow == null)
                                {
                                    title = attribute.AttributeName + " Value Added";
                                    description = string.Format("{0} was updated to '{1}' on {2} at {3}.",
                                        attribute.AttributeName, newPA.ToString(), dataUpdate.UpdateDateTime.ToShortDateString(),
                                        dataUpdate.UpdateDateTime.ToShortTimeString());
                                }
                                else
                                {
                                    // Load Old Person Attribute
                                    PersonAttribute oldPA = new PersonAttribute(attribute.AttributeId);
                                    oldPA.PersonID = Convert.ToInt32(oldRow["person_id"].ToString());
                                    if (oldRow["int_value"] != DBNull.Value)
                                        oldPA.IntValue = Convert.ToInt32(oldRow["int_value"].ToString());
                                    if (oldRow["decimal_value"] != DBNull.Value)
                                        oldPA.DecimalValue = Convert.ToDecimal(oldRow["decimal_value"].ToString());
                                    if (oldRow["varchar_value"] != DBNull.Value)
                                        oldPA.StringValue = oldRow["varchar_value"].ToString();
                                    if (oldRow["datetime_value"] != DBNull.Value)
                                        oldPA.DateValue = Convert.ToDateTime(oldRow["datetime_value"].ToString());

                                    // Or it was different (was an update)
                                    if (newPA.ToString() != oldPA.ToString())
                                    {
                                        title = attribute.AttributeName + " Value Updated";
                                        description = string.Format("{0} was updated from '{1}' to '{2}' on {3} at {4}.",
                                            attribute.AttributeName, oldPA.ToString(), newPA.ToString(),
                                            dataUpdate.UpdateDateTime.ToShortDateString(),
                                            dataUpdate.UpdateDateTime.ToShortTimeString());
                                    }
                                }

                                // Create a new assignment
                                if (title != string.Empty)
                                {
                                    AssignmentType assignmentType = new AssignmentType((Convert.ToInt32(AssignmentTypeIdSetting)));

                                    Assignment assignment = new Assignment();
                                    assignment.AssignmentTypeId = assignmentType.AssignmentTypeId;
                                    assignment.Title = title;
                                    assignment.Description = description;
                                    assignment.RequesterPersonId = Convert.ToInt32(newRow["person_id"].ToString());
                                    assignment.PriorityId = assignmentType.DefaultPriorityId;
                                    assignment.SubmitAssignmentEntry(null, "PersonAttributeDataChange");
                                }
                            }
                        }
                    }
                }
            return true;
        }
    }
}
