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
    [Description("[CCV] Assign Worker (Current User)")]
    public class AssignUserAsWorker : WorkFlowAction  
    {
        [BooleanSetting("Notify Requester", "Should requester be notified when worker is assigned?", false, true)]
        public string NotifyRequesterSetting { get { return Setting("NotifyRequester", "true", false); } }

        [BooleanSetting("Notify Worker", "Should worker be notified when worker is assigned?", false, true)]
        public string NotifyWorkerSetting { get { return Setting("NotifyWorker", "true", false); } }

        public override bool PerformAction(Assignment assignment, Person currentPerson)
        {
            try
            {
                if (currentPerson != null && currentPerson.PersonID != -1)
                {
                    assignment.WorkerPersonId = currentPerson.PersonID;
                    assignment.Save("AssignUserAsWorker", null);

                    AssignmentHistory history = new AssignmentHistory();
                    history.AssignmentId = assignment.AssignmentId;
                    history.Action = "Assigned to " + assignment.Worker.FullName;
                    history.Save("AssignUserAsWorker");

                    if (assignment.RequesterPersonId != -1 && Boolean.Parse(NotifyRequesterSetting))
                    {
                        AssignmentEntryRequesterEmail email = new AssignmentEntryRequesterEmail();
                        if (currentPerson != null && currentPerson.Emails.FirstActive != string.Empty)
                        {
                            email.Template.Sender = currentPerson.FullName;
                            email.Template.SenderEmail = currentPerson.Emails.FirstActive;
                        }
                        email.Send(assignment);
                    }

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
                }
                return true;
            }
            catch (System.Exception ex)
            {
                assignment.AddNote("Exception", ex.Message, false, null, "AssignUserAsWorker");
                return false;
            }
        }
    }
}
