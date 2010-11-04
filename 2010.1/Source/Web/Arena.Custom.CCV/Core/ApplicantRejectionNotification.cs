using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Arena.Core.Communications;

namespace Arena.Custom.CCV.Core.Communications
{
    [Description("Applicant Rejection Notification")]
    public class ApplicantRejectionNotification : CommunicationType
    {
        public override string[] GetMergeFields()
        {
            List<string> fields = new List<string>();
            fields.Add("##ApplicantFirstName##");
            fields.Add("##ApplicantLastName##");
            fields.Add("##ApplicantEmail##");
            fields.Add("##JobPostingName##");
            fields.Add("##DateApplied##");
            return fields.ToArray();
        }
    }
}
