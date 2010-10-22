using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Portal;
using Arena.Organization;

using Arena.Custom.CCV.PCO;

public partial class Login : PortalControl
{
    [RoleSetting("PCO Roles", "Roles that are synced with Planning Center Online (both Normal and Editor)", false, ListSelectionMode.Multiple)]
    public string PCORolesSetting { get { return Setting("PCORoles", "", false); } }

    [BooleanSetting("Automatic Redirect", "Flag indicating if module should automatically redirect user to PCO.  If False, user will need to click icon", false, false)]
    public string AutomaticRedirectSetting { get { return Setting("AutomaticRedirect", "false", false); } }

    [LookupSetting("PCO Account", "The PCO Account to login into", true, "07D3D7DC-2ADD-11DF-B622-4E6055D89593")]
    public string PCOAccountSetting { get { return Setting("PCOAccount", "-1", true); } }

    [TextSetting("Invalid PCO Redirect", "The URL that user should be redirected to if they don't have a PCO account yet (sync hasn't run)", false)]
    public string InvalidPCORedirectSetting { get { return Setting("InvalidPCORedirect", "", false); } }

    protected string _username = string.Empty;
    protected string _password = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bool display = false;

            if (!string.IsNullOrEmpty(PCORolesSetting) &&
                CurrentPerson != null && 
                CurrentPerson.Emails.FirstActive != string.Empty)
            {
                foreach (var id in PCORolesSetting.Split(','))
                {
                    if (CurrentUser.IsInRole("R" + id))
                    {
                        display = true;
                        break;
                    }
                }
            }

            if (display)
            {
                Lookup pcoAccount = new Lookup(Convert.ToInt32(PCOAccountSetting));

                People.GetPcoID(pcoAccount, CurrentPerson);

                string url = string.Empty;

                if (InvalidPCORedirectSetting != string.Empty && People.GetPcoID(pcoAccount, CurrentPerson) == -1)
                    url = InvalidPCORedirectSetting;
                else
                    url = string.Format("~/PCOLogin.aspx?email={0}&password={1}",
                        CurrentPerson.Emails.FirstActive, People.GetPcoPassword(pcoAccount, CurrentPerson));

                if (Boolean.Parse(AutomaticRedirectSetting))
                    Response.Redirect(url, true);
                else
                    hlPCOLogin.NavigateUrl = url;
            }

            this.Visible = display;
        }
    }
}
