using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PCOLogin : System.Web.UI.Page
{
    protected string _email = string.Empty;
    protected string _password = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        frmPCO.Action = "https://www.planningcenteronline.com/login";
        frmPCO.Method = "post";

        _email = Request.QueryString["email"];
        _password = Request.QueryString["password"];
    }
}
