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
    [Description("Agent | Potential Loss Notifications")]
    public class PotentialLossNotification : CommunicationType
    {
        public override string[] GetMergeFields()
        {
            List<string> fields = new List<string>();

            fields.Add("##RecipientFirstName##");
            fields.Add("##RecipientLastName##");
            fields.Add("##RecipientEmail##");

            fields.Add("##PastorName##");
            fields.Add("##PastorEmail##");
            fields.Add("##PastorBusinessPhone##");
            fields.Add("##PastorCellPhone##");

            return fields.ToArray();
        }
    }
}
