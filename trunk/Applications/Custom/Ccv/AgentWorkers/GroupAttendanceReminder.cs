using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Agent;
using Arena.Core;
using Arena.Core.Communications;
using Arena.Portal;
using Arena.SmallGroup;

using Arena.Custom.CCV;
using aspNetEmail;

namespace Arena.Custom.CCV.AgentWorkers
{
    /// <summary>
    /// Sends an Email to group leaders on the day of their group meeting to remind them to enter attendance.
    /// </summary>
    [Serializable]
    [Description("Sends an Email to group leaders on the day of their group meeting to remind them to enter attendance.")]
    public class GroupAttendanceReminder : AgentWorker
    {
        const int STATE_OK = 0;

        // private fields
        private int _categoryId = -1;
        private int _organizationId = -1;

        #region Agent Settings

        // define attributes and public fields
        [ClusterCategorySetting("CategoryID", "The Group Category to send reminders to", true)]
        public int CategoryID { get { return _categoryId; } set { _categoryId = value; } }

        [NumericSetting("OrganizationID", "Organization ID to associate with the peers.", true)]
        public int OrganizationId { get { return _organizationId; } set { _organizationId = value; } }

        #endregion

        public WorkerResultStatus SendEmail(out string message, out int state)
        {
            WorkerResultStatus workerResultStatus = WorkerResultStatus.Ok;
            message = string.Empty;
            state = STATE_OK;

            try
            {
                string day = DateTime.Today.DayOfWeek.ToString().ToLower();

                GroupClusterCollection clusters = new GroupClusterCollection();
                clusters.LoadChildClusterHierarchy(-1, _categoryId, _organizationId);
                RecurseClusters(clusters, day);
            }
            catch (Exception ex)
            {
                workerResultStatus = WorkerResultStatus.Exception;
                message = "An error occured while processing ERA Loss Notifications.\n\nMessage\n------------------------\n" + ex.Message + "\n\nStack Trace\n------------------------\n" + ex.StackTrace;
            }

            return workerResultStatus;
        }

        private void RecurseClusters(GroupClusterCollection clusters, string day)
        {
            foreach (GroupCluster cluster in clusters)
                if (cluster.Active && cluster.ClusterType.AllowOccurrences)
                {
                    foreach (Group group in cluster.SmallGroups)
                        if (group.Active &&
                            day.StartsWith(group.MeetingDay.Value.Trim().ToLower()) &&
                            group.Leader != null &&
                            group.Leader.Emails.FirstActive != string.Empty)
                        {
                            Arena.Custom.CCV.Core.Communications.GroupAttendanceReminder reminder = new Arena.Custom.CCV.Core.Communications.GroupAttendanceReminder();
                            Dictionary<string, string> fields = new Dictionary<string, string>();
                            fields.Add("##RecipientFirstName##", group.Leader.NickName);
                            fields.Add("##RecipientLastName##", group.Leader.LastName);
                            fields.Add("##RecipientEmail##", group.Leader.Emails.FirstActive);
                            fields.Add("##RecipientID##", group.Leader.PersonID.ToString());
                            fields.Add("##RecipientGuid##", group.Leader.Guid.ToString());
                            fields.Add("##GroupID##", group.Leader.Emails.FirstActive);
                            fields.Add("##GroupGuid##", group.Guid.ToString());

                            reminder.Send(group.Leader.Emails.FirstActive, fields, group.Leader.PersonID);
                        }

                    RecurseClusters(cluster.ChildClusters, day);
                }
        }

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
                        WorkerResultStatus status = SendEmail(out message, out state);
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
    }
}
