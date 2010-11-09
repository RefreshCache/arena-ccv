namespace ArenaWeb.UserControls.Custom.CCV.Data
{
	using System;
    using System.Collections.Specialized;
    using System.Collections.Generic;
    using System.ComponentModel;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
    using System.Reflection;
    using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
    using System.Xml;

    using Arena.Custom.CCV.Data;

	using Arena.Exceptions;
	using Arena.Portal;
	using Arena.Portal.UI;
	using Arena.Core;

	/// <summary>
	///		Summary description for SubscribedProfileList.
	/// </summary>
    public partial class DataUpdateActions : PortalControl
	{
        private Arena.Custom.CCV.Data.Action _action = null;

        protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!Page.IsPostBack)
                ShowList();

            if (hdnActionID.Value != string.Empty)
            {
                _action = new Arena.Custom.CCV.Data.Action(Convert.ToInt32(hdnActionID.Value));
                BuildActionSettings(false);
                mpEditAction.Show();
            }
        }

        private void ShowList()
        {
            dgActions.ItemType = "Action";
            dgActions.ItemBgColor = CurrentPortalPage.Setting("ItemBgColor", string.Empty, false);
            dgActions.ItemAltBgColor = CurrentPortalPage.Setting("ItemAltBgColor", string.Empty, false);
            dgActions.ItemMouseOverColor = CurrentPortalPage.Setting("ItemMouseOverColor", string.Empty, false);
            dgActions.AddEnabled = true;
            dgActions.AddImageUrl = "~/images/add_action.png";
            dgActions.MoveEnabled = true;
            dgActions.SourceTableName = "cust_ccv_data_action";
            dgActions.SourceTableKeyColumnName = "action_id";
            dgActions.SourceTableOrderColumnName = "action_order";
            dgActions.DeleteEnabled = true;
            dgActions.EditEnabled = true;
            dgActions.EditOverride = true;
            dgActions.MergeEnabled = false;
            dgActions.MailEnabled = false;
            dgActions.ExportEnabled = true;
            dgActions.AllowSorting = false;
            dgActions.AddIsAsync = true;
            dgActions.EditIsAsync = true;
            dgActions.DeleteIsAsync = true;
            dgActions.DataSource = new Arena.Custom.CCV.DataLayer.Data.ActionData().GetActions_DT();
            dgActions.DataBind();
        }

        private void ShowActionEdit(int actionId)
        {
            if (actionId >= 0)
                _action = new Arena.Custom.CCV.Data.Action(actionId);
            else
            {
                _action = new Arena.Custom.CCV.Data.Action();
                _action.Guid = Guid.Empty;
            }

            // Load Action Types
            ddlActions.Items.Clear();
            foreach (KeyValuePair<string, Type> type in Arena.Utility.ArenaReflection.GetTypes(typeof(DataUpdateAction)))
            {
                ListItem item = new ListItem(type.Key, type.Value.AssemblyQualifiedName);
                if (_action.DataUpdateAction != null)
                    item.Selected = _action.DataUpdateAction.GetType().Equals(type.Value);
                ddlActions.Items.Add(item);
            }

            tbActionName.Text = _action.Name;
            tbActionDescription.Text = _action.Description;

            BuildActionSettings(true);

            hdnActionID.Value = _action.ActionId.ToString();
            mpEditAction.Show();
        }

        #region Action Datagrid Events

        void dgActions_AddItem(object sender, EventArgs e)
        {
            ShowActionEdit(-1);
        }

        void dgActions_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ShowActionEdit(Convert.ToInt32(e.Item.Cells[0].Text));
        }

        void dgActions_ReBind(object sender, EventArgs e)
        {
            ShowList();
        }

        void dgActions_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            new Arena.Custom.CCV.DataLayer.Data.ActionData().DeleteAction(Convert.ToInt32(e.Item.Cells[0].Text));
            ShowList();
        }

        void dgActions_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            ShowList();
        }

        private void BuildActionSettings(bool setValues)
        {
            if (ddlActions.SelectedItem != null)
            {
                _action.DataUpdateAction = DataUpdateAction.GetActionClass(ddlActions.SelectedValue);
                BuildActionSettings(_action, setValues);
            }
            else
            {
                sgActionSettings.BuildSettings(0, new List<Arena.Portal.SettingAttribute>(), setValues);
            }
        }

        private void BuildActionSettings(Arena.Custom.CCV.Data.Action action, bool setValues)
        {
            sgActionSettings.BuildSettings(0, action.GetSettingAttributes(), setValues);
        }

        protected void btnCancelAction_Click(object sender, EventArgs e)
        {
            hdnActionID.Value = string.Empty;
        }

        protected void btnSaveAction_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (_action != null)
                {
                    _action.Name = tbActionName.Text;
                    _action.Description = tbActionDescription.Text;

                    SaveCurrentActionSettings();

                    _action.Save(CurrentUser.Identity.Name);
                }

                hdnActionID.Value = string.Empty;
                mpEditAction.Hide();
                ShowList();
            }
        }

        private void SaveCurrentActionSettings()
        {
            if (_action != null)
            {
                // Save current settings
                NameValueCollection settings = new NameValueCollection();
                sgActionSettings.PopulateSettings(settings);
                foreach (string key in settings.AllKeys)
                {
                    ActionSetting actionSetting = _action.Settings.FindByName(key);
                    if (actionSetting == null)
                    {
                        actionSetting = new ActionSetting();
                        actionSetting.Name = key;
                        _action.Settings.Add(actionSetting);
                    }
                    actionSetting.Value = settings[key];
                }
            }
        }

        protected string FormatAction(int actionId)
        {
            Arena.Custom.CCV.Data.Action action = new Arena.Custom.CCV.Data.Action(actionId);
            if (action != null)
                return FormatType(action.ActionAssembly);
            else
                return string.Empty;
        }

        protected string FormatType(string typeAssembly)
        {
            try
            {
                if (typeAssembly.Trim() == string.Empty)
                    return string.Empty;

                Type type = Type.GetType(typeAssembly);
                object[] attrs = type.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
                return type.Name;
            }
            catch
            {
                return "<Unknown>";
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
            dgActions.AddItem += new Arena.Portal.UI.AddItemEventHandler(dgActions_AddItem);
            dgActions.EditCommand += new DataGridCommandEventHandler(dgActions_EditCommand);
            dgActions.ReBind += new Arena.Portal.UI.DataGridReBindEventHandler(dgActions_ReBind);
            dgActions.DeleteCommand += new DataGridCommandEventHandler(dgActions_DeleteCommand);
            dgActions.ItemCommand += new DataGridCommandEventHandler(dgActions_ItemCommand);
        }

		#endregion
	}
}
