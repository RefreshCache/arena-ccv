using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Arena.Core;
using Arena.Core.Communications;
using Arena.Assignments;
using Arena.Portal;

namespace Arena.Custom.CCV.CustomActions
{
    [Serializable]
    [Description("[CCV] Assign Worker (Requester)")]
    public class AssignRequesterAsWorker : WorkFlowAction  
    {
        [BooleanSetting("Notify Worker", "Should worker be notified when worker is assigned?", false, true)]
        public string NotifyWorkerSetting { get { return Setting("NotifyWorker", "true", false); } }

        public override bool PerformAction(Assignment assignment, Person currentPerson)
        {
            try
            {
                assignment.WorkerPersonId = assignment.RequesterPersonId;
                assignment.Save("AssignRequesterAsWorker", null);

                AssignmentHistory history = new AssignmentHistory();
                history.AssignmentId = assignment.AssignmentId;
                history.Action = "Assigned to " + assignment.Worker.FullName;
                history.Save("AssignRequesterAsWorker");

                if (assignment.WorkerPersonId != -1 && Boolean.Parse(NotifyWorkerSetting))
                {
                    AssignmentEntryWorkerEmail email = new AssignmentEntryWorkerEmail();
                    if (currentPerson != null && currentPerson.Emails.FirstActive != string.Empty)
                    {
                        email.Template.Sender = currentPerson.FullName;
                        email.Template.SenderEmail = currentPerson.Emails.FirstActive;
                    }
                    email.Send(assignment);
                }

                return true;
            }
            catch (System.Exception ex)
            {
                assignment.AddNote("Exception", ex.Message, false, null, "AssignRequesterAsWorker");
                return false;
            }

        }
    }
}
