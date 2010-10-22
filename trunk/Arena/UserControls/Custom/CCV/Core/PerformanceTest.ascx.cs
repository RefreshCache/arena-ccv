namespace ArenaWeb.UserControls.Custom.CCV.Core
{
	using System;
	using System.Xml;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
    using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using System.Collections.Generic;
	using Arena.Exceptions;
	using Arena.Portal;
	using Arena.Portal.UI;
	using Arena.Core;
    using Arena.SmallGroup;

	/// <summary>
	///		Summary description for SubscribedProfileList.
	/// </summary>
	public partial class PerformanceTest : PortalControl
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            DateTime startTime = DateTime.Now;

            GroupClusterCollection clusters = new GroupClusterCollection();
            clusters.LoadChildClusterHierarchy(-1, 1, 3);
            DisplayClusters(clusters, "");
            TimeSpan timeSpan = DateTime.Now.Subtract(startTime);
            ph1.Controls.Add(new LiteralControl(string.Format("[{0} milliseconds]", timeSpan.Milliseconds.ToString("N0"))));

            //startTime = DateTime.Now;
            //ProfileItems profileItems = new ProfileItems(-1, 3, Arena.Enums.ProfileType.Event, 109179);
            //DisplayProfileItems(profileItems, "");
            //timeSpan = DateTime.Now.Subtract(startTime);
            //ph2.Controls.Add(new LiteralControl(string.Format("[{0} milliseconds]", timeSpan.Milliseconds.ToString("N0"))));
		}

        private void DisplayClusters(GroupClusterCollection clusters, string prefix)
        {
            foreach (GroupCluster cluster in clusters)
            {
                ph1.Controls.Add(new LiteralControl(prefix + cluster.GroupClusterID.ToString() + ":" + cluster.Name + "<br/>"));
                DisplayClusters(cluster.ChildClusters, prefix + "--");
            }
        }


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
		}

		#endregion
	}
}
