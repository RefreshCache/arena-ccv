namespace ArenaWeb.UserControls.Custom.CCV.Core
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Collections;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;

    using Arena.Core;
    using Arena.Event;
    using Arena.Portal;
    using Arena.Portal.UI;
    
    using Arena.Custom.CCV.Core;

    public partial class ProfileMemberFields : PortalControl
    {
        #region Private Fields

        bool EditEnabled = false;

        private Arena.Custom.CCV.Core.Profile profile = null;
        private Arena.Custom.CCV.Core.ProfileMember profileMember = null;

        private DynamicFieldHelper _fieldHelper = null;
        private string StateID = "ProfileMemberDetail";
        
        private bool editMode = false;

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Session[StateID] = null;

            LoadProfileMember();

            if (profileMember.ProfileID == -1 || profileMember.PersonID == -1)
                throw new ModuleException(CurrentPortalPage, CurrentModule, string.Format("The '{0}' ProfileMemberDetail module requires that a " +
                    "Valid Profile and Person ID be specified. ", CurrentModule.Title));

            if (profile.FieldModules.Count > 0 || profile.Fields.Count > 0)
            {
                if (ViewState["editMode"] != null)
                    editMode = (bool)ViewState["editMode"];

                BuildFields(!Page.IsPostBack, editMode);
            }
            else
            {
                this.Visible = false;
            }
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            ShowEdit();
        }

        protected void lbSave_Click(object sender, EventArgs e)
        {
            GetFields();
            profileMember.Save(CurrentUser.Identity.Name);
            ShowView();
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            ShowView();
        }

        #endregion

        #region Private Methods

        private void ShowView()
        {
            lbEdit.Visible = true;
            tblSave.Visible = false;

            ViewState["editMode"] = false;
            BuildFields(true, false);
        }

        private void ShowEdit()
        {
            lbEdit.Visible = false;
            tblSave.Visible = true;

            ViewState["editMode"] = true;
            BuildFields(true, true);
        }

        private void LoadProfileMember()
        {
            if (profileMember == null)
            {
                if (Page.IsPostBack && Session[StateID] != null)
                {
                    profileMember = (Arena.Custom.CCV.Core.ProfileMember)Session[StateID];
                }
                else
                {
                    int ProfileID = -1;
                    int PersonID = -1;

                    string[] keys;
                    keys = Request.QueryString.AllKeys;
                    foreach (string key in keys)
                    {
                        switch (key.ToUpper())
                        {
                            case "PROFILE":
                                try
                                {
                                    ProfileID = Int32.Parse(Request.QueryString.Get(key));
                                }
                                catch
                                {
                                    ProfileID = -1;
                                }
                                break;

                            case "PERSON":
                                try
                                {
                                    PersonID = Int32.Parse(Request.QueryString.Get(key));
                                }
                                catch
                                {
                                    PersonID = -1;
                                }
                                break;
                        }
                    }

                    profileMember = new Arena.Custom.CCV.Core.ProfileMember(ProfileID, PersonID);
                    Session[StateID] = profileMember;
                }

                profileMember.PortalPageID = CurrentPortalPage.PortalPageID;
            }

            profile = new Arena.Custom.CCV.Core.Profile(profileMember.ProfileID);
        }

        private void BuildFields(bool setValues, bool edit)
        {
            tblFields.Controls.Clear();

            _fieldHelper = new DynamicFieldHelper();
            _fieldHelper.LabelCssClass = "formLabel";
            _fieldHelper.LabelRequiredCssClass = "formLabel";
            _fieldHelper.FormCssClass = "formItem";
            _fieldHelper.LabelVAlign = "top";
            _fieldHelper.LabelVAlign = "top";
            _fieldHelper.EditEnabled = edit;

            foreach (Field field in profile.Fields)
                if (field.Visible)
                    AddFieldRow(tblFields, field, setValues);

            foreach (FieldModule fieldModule in profile.FieldModules)
                foreach (CustomFieldModuleField field in fieldModule.CustomFieldModuleFields)
                    if (field.Visible)
                        AddFieldRow(tblFields, field, setValues);
        }

        private void AddFieldRow(HtmlTable table, CustomField field, bool setValues)
        {
            string value = string.Empty;
            RegistrantField profileMemberField = profileMember.Fields.FindByID(field.CustomFieldId);
            if (profileMemberField != null)
                value = profileMemberField.SelectedValue;

            field.Required = false;

            _fieldHelper.AddFieldRow(table, CurrentArenaContext, field, "field_", value, setValues);
        }

        private void GetFields()
        {
            foreach (FieldModule fieldModule in profile.FieldModules)
                foreach (CustomFieldModuleField field in fieldModule.CustomFieldModuleFields)
                    GetFieldRow(field);

            foreach (Field field in profile.Fields)
                GetFieldRow(field);
        }

        private void GetFieldRow(CustomField field)
        {
            string fieldID = "field_" + field.CustomFieldId.ToString();
            RegistrantField profileMemberField = profileMember.Fields.FindByID(field.CustomFieldId);
            if (profileMemberField == null)
            {
                profileMemberField = new RegistrantField(field.CustomFieldId);
                profileMember.Fields.Add(profileMemberField);
            }

            profileMemberField.SelectedValue = DynamicFieldHelper.GetFieldValue(tblFields, profileMemberField.FieldType, profileMemberField, fieldID);
        }

        #endregion

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
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
