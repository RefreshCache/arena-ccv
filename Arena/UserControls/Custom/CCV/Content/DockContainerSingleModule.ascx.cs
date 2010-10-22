namespace ArenaWeb.UserControls.Custom.CCV.Content
{
	using System;
	using System.Data;
	using System.Configuration;
	using System.Collections;
	using System.Web;
	using System.Web.Security;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.WebControls.WebParts;
	using System.Web.UI.HtmlControls;
	using Arena.Portal;
	using Arena.Portal.UI;
	using Arena.Portal.UI.Docking;

	/// <summary>
	/// This module contains other dockable modules.
	/// </summary>
	public partial class DockContainerSingleModule : PortalControlDockContainer
	{
		// Dockable regions.  These must be defined as public properties so that they can be reflected.
		public Panel Main { get { return pnlContent; } }

		protected override void OnInit(EventArgs e)
		{
			// Sets whether the modules inside this container are movable or not.
			// This MUST be set in OnInit, so if you need to toggle between modes, you'll need to
			// check a querystring parameter.
			this.PositionMode = PortalControlDockContainerPositionMode.Movable;

			// Sets the server ID of the LinkButton or Button control that should be used to show
			// the catalog of available modules for the user to add to the page.  Don't set this
			// property if you don't want users to be able to choose modules.
			//this.ShowCatalogTargetControlID = lbCustomizeModules.ID;

			// Below are optional properties.

			/*

			// Style to use when docking an auto-generated window.
			this.DockingStyle = ComponentArt.Web.UI.SnapDockingStyleType.SolidOutline;
			
			// Style to use when dragging the auto-generated window.
			this.DraggingStyle = ComponentArt.Web.UI.SnapDraggingStyleType.Shadow;
			
			// CSS class to use for the content area inside of auto-generated the draggable window
			this.DockContentCssClass = "dockContent";
			
			// CSS class to use for the header of the auto-generated draggable window.
			this.DockHeaderCssClass = "dockHeader";
			
			*/

			base.OnInit(e);
		}
	}
}
