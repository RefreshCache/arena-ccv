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

public partial class Authorization : PortalControl
{
    const string DEFAULT_CONSUMER_TOKEN = "7jVAULjCdp2FzSbmezO7";
    const string DEFAULT_CONSUMER_SECRET = "v7oLc9IzsCDABmWtPrWvwYzEg135Fyd06A7vPBRI";

    LookupType _pcoAuthorizationType = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadAccounts();

        if (!Page.IsPostBack)
        {
            if (Request.QueryString["account"] != null &&
                Request.QueryString["token"] != null &&
                Request.QueryString["secret"] != null)
            {
                Lookup lookup = _pcoAuthorizationType.Values.FindByID(Convert.ToInt32(Request.QueryString["account"]));
                if (lookup != null)
                {
                    lookup.Qualifier = Request.QueryString["token"];
                    lookup.Qualifier8 = Request.QueryString["secret"];
                    lookup.Save();
                }
            }

            ShowList();
        }
    }

    private void ShowList()
    {
        lvAccounts.DataSource = _pcoAuthorizationType.Values;
        lvAccounts.DataBind();
    }

    private void LoadAccounts()
    {
        _pcoAuthorizationType = new LookupType(Arena.Custom.CCV.PCO.Core.SystemLookupType.PCOAuthorization);
        if (_pcoAuthorizationType.LookupTypeID == -1)
        {
            // Create Default Lookup Type and Lookup Value for PCO Authorization
            _pcoAuthorizationType.Name = "PCO Accounts";
            _pcoAuthorizationType.Description = "The PCO Accounts to sync with.  Each instance of the sync agent can sync any number of Arena roles with one PCO account.";
            _pcoAuthorizationType.OrganizationID = CurrentOrganization.OrganizationID;
            _pcoAuthorizationType.Qualifier1Title = "Access Token";
            _pcoAuthorizationType.Qualifier2Title = "Attribute Group Guid";
            _pcoAuthorizationType.Qualifier8Title = "Access Secret";
            _pcoAuthorizationType.SystemFlag = true;
            _pcoAuthorizationType.Save();

            string sqlStatement = string.Format("UPDATE core_lookup_type SET guid = '{0}' WHERE lookup_type_id = {1}",
                Arena.Custom.CCV.PCO.Core.SystemLookupType.PCOAuthorization.ToString(), _pcoAuthorizationType.LookupTypeID.ToString());

            new Arena.DataLayer.Organization.OrganizationData().ExecuteNonQuery(sqlStatement);
        }

        lTypeTitle.Text = _pcoAuthorizationType.Name;

        if (_pcoAuthorizationType.Values.Count == 0)
        {
            Lookup defaultAuthorization = new Lookup(Arena.Custom.CCV.PCO.Core.SystemLookup.PCOAuthorization_Default);
            if (defaultAuthorization.LookupID == -1)
            {
                defaultAuthorization.LookupTypeID = _pcoAuthorizationType.LookupTypeID;
                defaultAuthorization.Guid = Arena.Custom.CCV.PCO.Core.SystemLookup.PCOAuthorization_Default;
                defaultAuthorization.Value = "Default";
                defaultAuthorization.Save();

                string sqlStatement = string.Format("UPDATE core_lookup SET system_flag = 1 WHERE lookup_id = {0}",
                    defaultAuthorization.LookupID.ToString());

                new Arena.DataLayer.Organization.OrganizationData().ExecuteNonQuery(sqlStatement);
            }
        }
    }

    protected void lvAccounts_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Authorize")
        {
            string consumerToken = Organization.GetSettingValue("PCO_Consumer_Token", DEFAULT_CONSUMER_TOKEN);
            string consumerSecret = Organization.GetSettingValue("PCO_Consumer_Secret", DEFAULT_CONSUMER_SECRET);

            InSessionTokenManager tokenManager = new InSessionTokenManager();
            tokenManager.ConsumerKey = consumerToken; ;
            tokenManager.ConsumerSecret = consumerSecret; ;

            Uri redirect = Consumer.GetRequestURL(tokenManager, e.CommandArgument.ToString());

            Response.Redirect(redirect.AbsoluteUri, true);
        }
    }

    protected string GetAuthStatus(string token, string secret)
    {
        if (token.Trim() == string.Empty || secret.Trim() == string.Empty)
            return "Not Authorized!";
        else
            return string.Format("Successfully Authorized (Access Token: {0}, Secret: {1}",
                token, secret);
    }
}
