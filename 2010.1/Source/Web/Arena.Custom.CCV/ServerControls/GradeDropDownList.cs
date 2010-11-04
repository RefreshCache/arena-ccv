using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Collections;

namespace Arena.Custom.CCV.ServerControls
{
    public class GradeDropDownList : DropDownList
	{
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Items.Clear();
            this.Items.Add(new ListItem(string.Empty, "-1"));
            for (int i = 0; i <= 12; i++)
            {
                this.Items.Add(new ListItem(GetGradeName(i), i.ToString()));
            }
        }
 
        private static string GetGradeName(int grade)
        {
            switch (grade)
            {
                case -1:
                    return "";
                case 0:
                    return "Kindergarten";
                case 1:
                    return grade.ToString() + "st";
                case 2:
                    return grade.ToString() + "nd";
                case 3:
                    return grade.ToString() + "rd";
                default:
                    return grade.ToString() + "th";
            }
        }
    }
}
