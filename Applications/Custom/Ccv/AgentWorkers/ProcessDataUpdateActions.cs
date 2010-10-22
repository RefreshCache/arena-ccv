using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

using Agent;

using Arena.Core;
using Arena.Document;
using Arena.Portal;

namespace Arena.Custom.CCV.AgentWorkers
{
    [Serializable]
    [Description("Agent to process data update actions")]
    public class ProcessDataUpdateActions : AgentWorker
    {
        const int STATE_OK = 0;
    
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
                        WorkerResultStatus status = ProcessActions(out message, out state);
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

        public WorkerResultStatus ProcessActions(out string message, out int state)
        {
            WorkerResultStatus workerResultStatus = WorkerResultStatus.Ok;
            message = string.Empty;
            state = STATE_OK;

            try
            {
                System.Text.StringBuilder sbErrors = new System.Text.StringBuilder();

                // Process Actions
                Arena.Custom.CCV.Data.ActionCollection actions = new Arena.Custom.CCV.Data.ActionCollection();
                actions.LoadAll();

                foreach (Arena.Custom.CCV.Data.Action action in actions)
                    try
                    {
                        action.DoAction();
                    }
                    catch (Exception ex)
                    {
                        sbErrors.AppendFormat("Action Failed...\n\tAction Name: {0}\n\tAction Assembly: {1}\n\tError Message: {2}\n\n",
                            action.Name, action.ActionAssembly, ex.Message);
                    }

                if (sbErrors.Length > 0)
                    throw new Arena.Exceptions.ArenaApplicationException("Data Update Processing encountered following problems:\n\n" + sbErrors.ToString());
            }
			catch (Exception ex)
			{
				workerResultStatus = WorkerResultStatus.Exception;
				message = "Error occurred while processing data update actions.\n\nMessage:\n" + ex.Message + "\n\nStack Trace\n------------------------" + ex.StackTrace;
			}

			return workerResultStatus;
        }
    }
}