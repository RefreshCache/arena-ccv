using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Agent;
using Arena.Core;
using Arena.SmallGroup;
using Arena.Portal;
using Arena.Custom.CCV.SmallGroup;

namespace Arena.Custom.CCV.AgentWorkers
{
    [Serializable]
    public class UpdateGroupMemberLog : AgentWorker
    {
        const int STATE_OK = 0;
    
        // private fields
        private int _clusterTypeId = -1;
        private int _organizationId = -1;

        #region Agent Settings

        // define attributes and public fields
        [ClusterTypeSetting("ClusterType", "The Cluster Type that should be tracked", true)]
        public int ClusterType { get { return _clusterTypeId; } set { _clusterTypeId = value; } }

        [NumericSetting("OrganizationID", "Organization ID to associate with the peers.",  true)]
        public int OrganizationId { get { return _organizationId; } set { _organizationId = value; } }

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
                        WorkerResultStatus status = ProcessGroups(out message, out state);
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
        public WorkerResultStatus ProcessGroups(out string message, out int state)
        {
            Trace.Write("Starting ProcessGroups Agent...\n");

            WorkerResultStatus workerResultStatus = WorkerResultStatus.Ok;
            message = string.Empty;
            state = STATE_OK;

            try
            {
                Arena.Custom.CCV.SmallGroup.GroupCollection groups = new Arena.Custom.CCV.SmallGroup.GroupCollection();
                groups.LoadByClusterType(_clusterTypeId);

                foreach (Group group in groups)
                {
                    // Log the Leader
                    if (group.LeaderID != -1)
                    {
                        GroupMemberLog leaderLog = new GroupMemberLog();
                        leaderLog.PersonID = group.LeaderID;
                        leaderLog.GroupID = group.GroupID;
                        leaderLog.Role = new Lookup();
                        leaderLog.SaveGroupMemberLog(group);
                    }

                    // Save the Members
                    foreach (GroupMember groupMember in group.Members)
                        new GroupMemberLog(groupMember).SaveGroupMemberLog(group);
                }
            }
            catch (Exception ex)
            {
                workerResultStatus = WorkerResultStatus.Exception;
                message = "An error occured while processing Group Status Update.\n\nMessage\n------------------------\n" + ex.Message + "\n\nStack Trace\n------------------------\n" + ex.StackTrace;
            }
            finally
            {

            }

            return workerResultStatus;
        }

    }
}