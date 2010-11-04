using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Arena.Core;
using Arena.Core.Communications;
using Arena.Exceptions;
using Arena.Utility;

namespace Arena.Custom.CCV.Core.Communications
{
    [Description("Agent | Group Attendance Reminder")]
    public class GroupAttendanceReminder : CommunicationType
    {
        public override string[] GetMergeFields()
        {
            List<string> fields = new List<string>();

            fields.Add("##RecipientFirstName##");
            fields.Add("##RecipientLastName##");
            fields.Add("##RecipientEmail##");
            fields.Add("##RecipientID##");
            fields.Add("##RecipientGuid##");
            fields.Add("##GroupID##");
            fields.Add("##GroupGuid##");

            return fields.ToArray();
        }
    }
}
