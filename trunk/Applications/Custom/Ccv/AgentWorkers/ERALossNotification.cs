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
using Arena.SmallGroup;

using Arena.Custom.CCV;
using aspNetEmail;

namespace Arena.Custom.CCV.AgentWorkers
{
    /// <summary>
    /// This class will send an email to any cluster leader in the group heirarchy for the category id specified.
    /// </summary>
    [Serializable]
    [Description("Sends an Email to people who may be a potential ERA Loss.")]
    public class ERALossNotification : AgentWorker
    {
        const int STATE_OK = 0;

        public WorkerResultStatus SendEmail(out string message, out int state)
        {
            WorkerResultStatus workerResultStatus = WorkerResultStatus.Ok;
            message = string.Empty;
            state = STATE_OK;

            try
            {
                AreaOutreachCoordinatorCollection pastors = new AreaOutreachCoordinatorCollection();
                pastors.LoadByRole(1623);

                Arena.DataLayer.Organization.OrganizationData oData = new Arena.DataLayer.Organization.OrganizationData();

                string query = "SELECT * FROM cust_ccv_era_losses WHERE processed = 1 AND send_email = 1 AND sent = 0";
                SqlDataReader rdr = oData.ExecuteReader(query);
                while (rdr.Read())
                {
                    Family family = new Family((int)rdr["family_id"]);
                    FamilyMember familyHead = family.FamilyHead;

                    if (familyHead != null && familyHead.Emails.Count > 0)
                    {
                        Area area = familyHead.Area;
                        if (area != null)
                        {
                            Person pastor = null;
                            foreach(AreaOutreachCoordinator coord in pastors)
                                if (coord.AreaId == area.AreaID)
                                {
                                    pastor = new Person(coord.PersonId);
                                    break;
                                }

                            if (pastor != null)
                            {
                                Arena.Custom.CCV.Core.Communications.PotentialLossNotification lossNotification = new Arena.Custom.CCV.Core.Communications.PotentialLossNotification();
                                Dictionary<string, string> fields = new Dictionary<string,string>();
                                fields.Add("##RecipientFirstName##", familyHead.NickName);
                                fields.Add("##RecipientLastName##", familyHead.LastName);
                                fields.Add("##RecipientEmail##", familyHead.Emails.FirstActive);
                                fields.Add("##PastorName##", pastor.FullName);
                                fields.Add("##PastorEmail##", pastor.Emails.FirstActive);

                                PersonPhone bPhone = pastor.Phones.FindByType(SystemLookup.PhoneType_Business);
                                fields.Add("##PastorBusinessPhone##", bPhone != null ? bPhone.Number : string.Empty);
                                PersonPhone cPhone = pastor.Phones.FindByType(SystemLookup.PhoneType_Cell);
                                fields.Add("##PastorCellPhone##", cPhone != null ? cPhone.Number : string.Empty);

                                if (lossNotification.Send(familyHead.Emails.FirstActive, fields, familyHead.PersonID))
                                {
                                    string updateQuery = string.Format("UPDATE cust_ccv_era_losses SET sent = 1 WHERE family_id = {0}",
                                        family.FamilyID.ToString());
                                    oData.ExecuteNonQuery(updateQuery);
                                }
                            }
                        }
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                workerResultStatus = WorkerResultStatus.Exception;
                message = "An error occured while processing ERA Loss Notifications.\n\nMessage\n------------------------\n" + ex.Message + "\n\nStack Trace\n------------------------\n" + ex.StackTrace;
            }

            return workerResultStatus;
        }

        private void LoadChildClusterGroups(GroupCluster cluster, GroupCollection groups)
        {
            foreach (Group group in new GroupCollection(cluster.GroupClusterID))
                groups.Add(group);

            foreach (GroupCluster childCluster in cluster.ChildClusters)
                LoadChildClusterGroups(childCluster, groups);
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
