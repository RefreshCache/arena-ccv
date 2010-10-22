using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Arena.Core;
using Arena.Assignments;
using Arena.Portal;

namespace Arena.Custom.CCV.CustomActions
{
    [Serializable]
    [Description("[CCV] Set Area Field From Requester")]
    [ActionRequiredField("Requester's Area", "Arena.Portal.UI.FieldTypes.AreaField, Arena.Portal.UI", "1203")]
    public class SetAreaFromRequester : WorkFlowAction
    {
        public override bool PerformAction(Assignment assignment, Person currentPerson)
        {
            try
            {
                if (assignment.Requester != null && assignment.Requester.Area != null)
                {
                    AssignmentTypeField field = assignment.AssignmentType.Fields.FindByTitle("Requester's Area");
                    if (field != null)
                    {
                        AssignmentFieldValue fieldValue = assignment.FieldValues.FindByID(field.CustomFieldId);
                        if (fieldValue == null)
                        {
                            fieldValue = new AssignmentFieldValue();
                            fieldValue.AssignmentId = assignment.AssignmentId;
                            fieldValue.CustomFieldId = field.CustomFieldId;
                            assignment.FieldValues.Add(fieldValue);
                        }
                        fieldValue.SelectedValue = assignment.Requester.Area.AreaID.ToString();
                        assignment.Save("SetAreaFromRequester", null);
                    }
                }

                return true;
            }
            catch (System.Exception ex)
            {
                assignment.AddNote(ex.Message, false, currentPerson, "SetAreaFromRequester");
                return false;
            }

        }
    }
}
