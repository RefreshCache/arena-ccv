using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Arena.Custom.CCV.Core
{
    [Serializable]
    [System.ComponentModel.Description("CCV Address")]
    public class AddressField : Arena.Portal.UI.FieldTypes.AddressField
    {
        public override void RenderPrompt(PlaceHolder placeHolder, Arena.Core.ArenaContext currentContext, Arena.Core.IFieldInfo fieldInfo, bool setValues, string formCssClass)
        {
            base.RenderPrompt(placeHolder, currentContext, fieldInfo, setValues, formCssClass);

            foreach(Control control in placeHolder.Controls)
                if (control is LiteralControl)
                {
                    LiteralControl lControl = (LiteralControl)control;
                    lControl.Text = lControl.Text.Replace("<b>City</b>", "").Replace("&nbsp;<b>State</b>", "").Replace("&nbsp;<b>Zip</b>", "");
                }
        }
    }
}
