using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Arena.Core;
using Arena.Core.Communications;
using Arena.Assignments;
using Arena.Portal;

namespace Arena.Custom.CCV.CustomActions
{
    [Serializable]
    [Description("[CCV] Assign Area Leader")]
    [ActionRequiredField("Requester's Area", "Arena.Portal.UI.FieldTypes.AreaField, Arena.Portal.UI", "1203")]
    public class AssignAreaLeader : WorkFlowAction
    {
        [BooleanSetting("Notify Requester", "Should requester be notified when worker is assigned?", false, true)]
        public string NotifyRequesterSetting { get { return Setting("NotifyRequester", "true", false); } }

        [BooleanSetting("Notify Worker", "Should worker be notified when worker is assigned?", false, true)]
        public string NotifyWorkerSetting { get { return Setting("NotifyWorker", "true", false); } }

		[LookupSetting("Area Role", "The area leader role to assign to.", true, "E499057B-85CE-41B9-9C2C-7A703C8756A7")]
		public string AreaRoleSetting {get{return Setting("AreaRole", "-1", true);}}

        public override bool PerformAction(Assignment assignment, Person currentPerson)
        {
            try
            {
                if (assignment.WorkerPersonId == -1)
                {
                    AssignmentTypeField field = assignment.AssignmentType.Fields.FindByTitle("Requester's Area");
                    if (field != null)
                    {
                        AssignmentFieldValue fieldValue = assignment.FieldValues.FindByID(field.CustomFieldId);
                        if (fieldValue != null)
                        {
                            Area area = new Area(Convert.ToInt32(fieldValue.SelectedValue));
                            if (area != null)
                            {
                                foreach (AreaOutreachCoordinator leader in area.OutreachCoordinators)
                                    if (leader.AreaRole.LookupID == Convert.ToInt32(AreaRoleSetting))
                                    {
                                        assignment.WorkerPersonId = leader.PersonId;
                                        assignment.Save("AssignAreaLeader", null);
                                        break;
                                    }
                            }
                        }
                    }
                }

                if (assignment.WorkerPersonId != -1)
                {
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
                assignment.AddNote(ex.Message, false, currentPerson, "AssignAreaLeader");
                return false;
            }

        }
    }
}
