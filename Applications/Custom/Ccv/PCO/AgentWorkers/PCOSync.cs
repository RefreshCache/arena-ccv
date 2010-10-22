using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;

using Agent;
using Arena.Core;
using Arena.Portal;
using Arena.Custom.CCV.PCO;

namespace Arena.Custom.CCV.PCO.AgentWorkers
{
	/// <summary>
	/// This agent will sync every member in the selected role(s) with the selected PCO account.
	/// </summary>
	[Serializable]
    [Description("This agent will sync every member in the selected role(s) with the selected PCO account.")]
	public class PCOSync : AgentWorker
	{
		const int STATE_OK = 0;
        private int _organizationId = -1;
        private string _publicArenaURL = "";
        private int _PCOAccountLUID = -1;
        string _roleList = string.Empty;
        string _editorRoleList = string.Empty;

        /// <summary>
        /// Organization ID.
        /// </summary>
        [NumericSetting("Organization ID", "Organization ID.", true)]
        [Description("Organization ID.")]
        public int OrganizationId { get { return _organizationId; } set { _organizationId = value; } }

        /// <summary>
        /// The Public Arena URL that PCO can use to capture Arena images (i.e. 'http://www.ccvonline.com/arena'.
        /// </summary>
        [NumericSetting("Public Arena URL", "The Public Arena URL that PCO can use to capture Arena images (i.e. 'http://www.ccvonline.com/arena'.", true)]
        [Description("The Public Arena URL that PCO can use to capture Arena images (i.e. 'http://www.ccvonline.com/arena'.")]
        public string PublicArenaURL { get { return _publicArenaURL; } set { _publicArenaURL = value; } }

        /// <summary>
        /// The 'PCO Account' Lookup ID of the PCO Account that should be synced.
        /// </summary>
        [NumericSetting("PCO Account", "The 'PCO Account' Lookup ID of the PCO Account that should be synced.", true)]
        [Description("The 'PCO Account' Lookup ID of the PCO Account that should be synced.")]
        public int PCOAccountLUID { get { return _PCOAccountLUID; } set { _PCOAccountLUID = value; } }

        /// <summary>
        /// Comma delimited list of role IDs containing people who should be synced with PCO.
		/// </summary>
        [TextSetting("Viewer Role List", "Comma delimited list of role IDs containing people who should be synced with the selected PCO Account as a Viewer.", true)]
        [Description("Comma delimited list of role IDs containing people who should be synced with PCO.")]
		public string RoleList { get { return _roleList; } set { _roleList = value; } }

        /// <summary>
        /// Comma delimited list of role IDs containing people who should be synced with PCO as an Editor
        /// </summary>
        [TextSetting("Editor Role List", "Comma delimited list of role IDs containing people who should be synced with the selected PCO Account as an Editor.", true)]
        [Description("Comma delimited list of role IDs containing people who should be synced with PCO as an Editor.")]
        public string EditorRoleList { get { return _editorRoleList; } set { _editorRoleList = value; } }

        public WorkerResultStatus SyncUsers(out string message, out int state)
		{
			WorkerResultStatus workerResultStatus = WorkerResultStatus.Ok;
			message = string.Empty;
			state = STATE_OK;

            try
            {
                System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();

                Lookup pcoAccount = new Lookup(Convert.ToInt32(_PCOAccountLUID));
                Arena.Custom.CCV.PCO.People _pcoPeople = new People(_organizationId, pcoAccount, _publicArenaURL);

                List<int> editorRoleMembers = new List<int>();
                Dictionary<int, Person> roleMembers = new Dictionary<int, Person>();

                foreach (var id in _editorRoleList.Split(','))
                    if (id.Trim() != string.Empty)
                    {
                        Arena.Security.Role role = new Arena.Security.Role(Int32.Parse(id));
                        foreach (int memberID in role.RoleMemberIds)
                        {
                            if (!editorRoleMembers.Contains(memberID))
                                editorRoleMembers.Add(memberID);
                            if (!roleMembers.ContainsKey(memberID))
                                roleMembers.Add(memberID, new Person(memberID));
                        }
                    }

                foreach (var id in _roleList.Split(','))
                    if (id.Trim() != string.Empty)
                    {
                        Arena.Security.Role role = new Arena.Security.Role(Int32.Parse(id));
                        foreach (int memberID in role.RoleMemberIds)
                            if (!roleMembers.ContainsKey(memberID))
                                roleMembers.Add(memberID, new Person(memberID));
                    }

                // Update existing users
                List<int> activeUsers = new List<int>();
                foreach (KeyValuePair<int, Person> member in roleMembers)
                {
                    activeUsers.Add(member.Key);
                    try
                    {
                        _pcoPeople.SyncPerson(member.Value, "PCOSync", editorRoleMembers.Contains(member.Key));
                    }
                    catch (Exception ex)
                    {
                        StringBuilder errorMessage = new StringBuilder();

                        System.Exception exception = ex;
                        while (exception != null)
                        {
                            errorMessage.AppendFormat("{0}\n\n", exception.Message);
                            exception = exception.InnerException;
                        }

                        sbErrors.AppendFormat("PCO Sync Failed...\n\tPerson: {0}\n\tError Message: {1}",
                            member.Value.FullName, errorMessage.ToString());
                    }
                }

                // Disable Old Users
                _pcoPeople.Disable(activeUsers);

                if (sbErrors.Length > 0)
                    throw new Arena.Exceptions.ArenaApplicationException("PCO Sync Processing encountered following problems:\n\n" + sbErrors.ToString());
            }
            catch (Exception ex)
            {
                workerResultStatus = WorkerResultStatus.Exception;
                message = "Error occurred while processing PCO Sync.\n\nMessage:\n" + ex.Message + "\n\nStack Trace\n------------------------" + ex.StackTrace;
            }

            return workerResultStatus;
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
                        WorkerResultStatus status = SyncUsers(out message, out state);
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
