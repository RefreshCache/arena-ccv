using System;
using System.Collections;
using System.ComponentModel;
using System.Data.SqlClient;

using Arena.Assignments;
using Arena.Core;
using Arena.Portal;

namespace Arena.Custom.CCV.CustomActions
{
    /// <summary>
    /// Executes a SQL Stored Procedure for a particular assignment.  Stored Procedure must have two parameters; @AssignmentID and @PersonID.
    /// @AssignmentID will be the id of the current assignment, and @PersonID will be the id of the person who set the state.  Either the
    /// stored procedure should update the state of the assingment, or the action should be configured to increment state after running so that
    /// this action is not continueally executed.
    /// </summary>
    [Serializable]
    [Description("[CCV] Execute SQL Statement")]
    public class ExecuteSQLProc : WorkFlowAction
    {
        [TextSetting("Stored Procedure Name", "Name of stored procedure to run.  The procedcure must have two parameters; @AssignmentID and @PersonID", true)]
        public string StoredProcSetting { get { return Setting("StoredProc", "", true); } }

        public override bool PerformAction(Assignment assignment, Person currentPerson)
        {
            try
            {
			    ArrayList lst = new ArrayList();
                lst.Add(new SqlParameter("@AssignmentId", assignment != null ? assignment.AssignmentId : -1));
                lst.Add(new SqlParameter("@PersonId", currentPerson != null ? currentPerson.PersonID : -1));

                bool result = true;
                SqlDataReader rdr = new Arena.DataLayer.Organization.OrganizationData().ExecuteReader(StoredProcSetting, lst);
                if (rdr.Read())
                {
                    try { result = (bool)rdr["result"]; }
                    catch { }
                }
                rdr.Close();

                if (result)
                {
                    // Because the Assignment Object's ProcessState method saves the assignment object before reading any
                    // changes that this action may have made, every property on the passed in object should be updated 
                    // prior to returning (since we don't really know what properties the SQL Proc may have updated)
                    Assignment newAssignment = new Assignment(assignment.AssignmentId);
                    assignment.Description = newAssignment.Description;
                    assignment.DueDate = newAssignment.DueDate;
                    assignment.FieldValues = newAssignment.FieldValues;
                    assignment.PriorityId = newAssignment.PriorityId;
                    assignment.RequesterPersonId = newAssignment.RequesterPersonId;
                    assignment.ResolutionText = newAssignment.ResolutionText;
                    assignment.ResolvedDate = newAssignment.ResolvedDate;
                    assignment.StateId = newAssignment.StateId;
                    assignment.Title = newAssignment.Title;
                    assignment.WorkerPersonId = newAssignment.WorkerPersonId;
                }

                return result;
            }
            catch (System.Exception ex)
            {
                assignment.AddNote("Exception", ex.Message, false, null, "ExecuteSQLProc");
                return false;
            }

        }
    }
}
