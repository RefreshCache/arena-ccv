using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Arena.Core.Communications;

namespace Arena.Custom.CCV.Core.Communications
{
	[Description("Job Application Notification")]
	public class JobApplicationNotification : CommunicationType
	{
		public override string[] GetMergeFields()
        {
            List<string> fields = new List<string>();
            fields.Add("##JobPostingName##");
            fields.Add("##JobPostingID##");
            fields.Add("##FirstName##");
            fields.Add("##LastName##");
            fields.Add("##Email##");
            fields.Add("##HowDidYouHear##");
            fields.Add("##HowLongChristian##");
            fields.Add("##TakenClass100##");
            fields.Add("##Class100Date##");
            fields.Add("##Member##");
            fields.Add("##NeighborhoodGroup##");
            fields.Add("##Serving##");
            fields.Add("##ServingMinistry##");
            fields.Add("##Baptized##");
            fields.Add("##Tithing##");
            fields.Add("##Experience##");
            fields.Add("##LedToWorkAtCCV##");
            fields.Add("##Coverletter##");
            return fields.ToArray();
        }
	}
}
