namespace ArenaWeb.UserControls.Custom.CCV.Contributions
{
	using System;
    using System.Collections.Specialized;
	using System.Text;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Arena.Core;
	using Arena.Portal;
	using Arena.SmallGroup;
	using Arena.Security;
	using Arena.Exceptions;
	using Arena.DataLayer.Core;

	/// <summary>
	///		Summary description for MemberList.
	/// </summary>
    public partial class ProfileReport : PortalControl
	{
        #region Events

        void btnSubmit_Click(object sender, EventArgs e)
        {
            Arena.Payment.RepeatingPayment rp = new Arena.Payment.RepeatingPayment(tbProfileID.Text);
            if (rp == null || rp.RepeatingPaymentId == -1)
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Profile ID";
            }
            else
            {
                Arena.Payment.GatewayAccount ga = rp.GatewayAccount;

                Arena.Payment.Processors.PayFlowReporting payFlowReporting = new Arena.Payment.Processors.PayFlowReporting(
                    ga.Username,
                    ga.Username,
                    ga.MerchantAccount,
                    ga.Password);

                NameValueCollection reportParams = new NameValueCollection();
                reportParams.Add("profile_id", string.Empty);
                reportParams.Add("start_date", tbStartDate.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                reportParams.Add("end_date", tbEndDate.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                reportParams.Add("tender_type", rp.GatewayAccount.AccountType == Arena.Payment.AccountType.ACH ? "ACH" : "Credit");
                reportParams.Add("timezone", "GMT-08:00");
                reportParams["profile_id"] = rp.TransactionDetail;

                DataTable dt = payFlowReporting.Report("RecurringProfileReport", reportParams);
                if (dt != null)
                {
                    dgTransactions.AutoGenerateColumns = true;
                    dgTransactions.DataSource = dt;
                    dgTransactions.DataBind();
                    dgTransactions.Visible = true;
                    lblError.Visible = false;
                }
                else
                {
                    dgTransactions.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = payFlowReporting.Message;
                }
            }
        }

        #endregion

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
		}

		#endregion

	}
}