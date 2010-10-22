using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Custom.CCV.PCO;
using Arena.Organization;

public partial class PCOResult : System.Web.UI.Page
{
    const string DEFAULT_CONSUMER_TOKEN = "7jVAULjCdp2FzSbmezO7";
    const string DEFAULT_CONSUMER_SECRET = "v7oLc9IzsCDABmWtPrWvwYzEg135Fyd06A7vPBRI";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string pageId = string.Empty;

            Arena.Portal.Module module = new Arena.Portal.Module("UserControls/Custom/CCV/PCO/Authorization.ascx");
            if (module.Pages.Count > 0)
                pageId = module.Pages[0].PortalPageID.ToString();

            aRetry.HRef = "default.aspx?page=" + pageId;

            int _organizationId = Int32.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["organization"]);
            Organization org = new Organization(_organizationId);

            string consumerToken = Organization.GetSettingValue("PCO_Consumer_Token", DEFAULT_CONSUMER_TOKEN);
            string consumerSecret = Organization.GetSettingValue("PCO_Consumer_Secret", DEFAULT_CONSUMER_SECRET);

            InSessionTokenManager tokenManager = new InSessionTokenManager();
            tokenManager.ConsumerKey = consumerToken;
            tokenManager.ConsumerSecret = consumerSecret;

            string account = Request.QueryString["account"] ?? string.Empty;
            string oauthToken = Request.QueryString["oauth_token"] ?? string.Empty;
            string oauthVerifier = Request.QueryString["oauth_verifier"] ?? string.Empty;

            if (account.Contains("?oauth_token=") && oauthToken == string.Empty)
            {
                int indexOf = account.IndexOf("?oauth_token=");
                oauthToken = account.Substring(indexOf + 13);
                account = account.Substring(0, indexOf);
            }

            string token = Consumer.GetAccessToken(tokenManager, oauthToken, oauthVerifier);

            string[] tokenKeyValue = token.Split('=');
            if (tokenKeyValue.Length == 2)
                Response.Redirect(string.Format("~/default.aspx?page={0}&account={1}&token={2}&secret={3}",
                    pageId,
                    account,
                    tokenKeyValue[0],
                    tokenKeyValue[1]), true);
            else
                lError.Text = "Invalid Token returned from GetAccessToken()";
        }
        catch (System.Exception ex)
        {
            lError.Text = "<br/>";

            while (ex != null)
            {
                lError.Text += ex.Message + "<br/>";
                ex = ex.InnerException;
            }
        }
    }
}
