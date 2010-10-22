
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;

using Arena.Core;
using Arena.Custom.CCV.DataLayer.Data;
using Arena.Portal;
using Arena.Utility;


namespace Arena.Custom.CCV.Data
{
    /// <summary>
    /// Summary description for Action.
    /// </summary>
    [Serializable]
    public class Action : ArenaObjectBase
    {
        #region Private Members

        private Guid _actionGuid = Guid.NewGuid();
        private int _actionId = -1;
        private int _actionOrder = 0;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _actionAssembly = string.Empty;
        private DataUpdateAction _dataUpdateAction = null;
        private ActionSettingCollection _settings = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Temporary identifier used before action is saved
        /// </summary>
        public Guid Guid
        {
            get { return _actionGuid; }
            set { _actionGuid = value; }
        }

        public int ActionId
        {
            get { return _actionId; }
            set { _actionId = value; }
        }

        public int ActionOrder
        {
            get { return _actionOrder; }
            set { _actionOrder = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DataUpdateAction DataUpdateAction
        {
            get 
            {
                if (_dataUpdateAction == null && _actionAssembly != string.Empty)
                    _dataUpdateAction = DataUpdateAction.GetActionClass(_actionAssembly);

                return _dataUpdateAction; 
            }
            set { _dataUpdateAction = value; }
        }

        public string ActionAssembly
        {
            get
            {
                if (DataUpdateAction != null)
                    return _dataUpdateAction.GetType().AssemblyQualifiedName;

                return _actionAssembly;
            }
        }

        /// <summary>
        /// Gets or sets settings specific to this state.
        /// </summary>
        public ActionSettingCollection Settings
        {
            get
            {
                if (_settings == null)
                    LoadSettings();

                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        public void LoadSettings()
        {
            _settings = new ActionSettingCollection(_actionId);
        }

        #endregion

        #region Public Methods

        public void Save(string currentUser)
        {
            SaveAction(currentUser);
        }

        public static void Delete(int actionId)
        {
            new ActionData().DeleteAction(actionId);
        }

        public void Delete()
        {
            new ActionData().DeleteAction(_actionId);
        }

        /// <summary>
        /// Gets a list of setting attributes associated with the current action
        /// </summary>
        /// <param name="pageToUse"></param>
        /// <param name="moduleInstance"></param>
        /// <returns></returns>
        public List<SettingAttribute> GetSettingAttributes()
        {
            List<SettingAttribute> properties = new List<SettingAttribute>();

            if (this.DataUpdateAction != null)
            {
                foreach (PropertyInfo pi in this.DataUpdateAction.GetType().GetProperties())
                {
                    Object[] customAttributes = pi.GetCustomAttributes(typeof(SettingAttribute), true);
                    foreach (object attr in customAttributes)
                    {
                        SettingAttribute settingAttribute = (SettingAttribute)attr;
                        settingAttribute.Setting = pi.Name.Replace("Setting", "");
                        settingAttribute.Value = this.Settings[settingAttribute.Setting].Value;

                        bool loaded = false;
                        foreach (SettingAttribute attribute in properties)
                            if (attribute.Setting.ToLower() == settingAttribute.Setting.ToLower())
                            {
                                loaded = true;
                                break;
                            }
                        if (!loaded)
                            properties.Add(settingAttribute);
                    }
                }
            }
            return properties;
        }

        public void DoAction()
        {
            this.DataUpdateAction.Settings = this.Settings;

            ActionData tData = new ActionData();

            List<DataUpdate> dataUpdates = new List<DataUpdate>();

            Type actionType = this.DataUpdateAction.GetType();
            object[] attrs = actionType.GetCustomAttributes(typeof(MonitorDataUpdate), true);
            for (int i = 0; i < attrs.Length; i++)
            {
                MonitorDataUpdate attribute = (MonitorDataUpdate)attrs[i];

                SqlDataReader reader = tData.GetDataUpdatesForAction(this.ActionId, attribute.DataType);
                while (reader.Read())
                    dataUpdates.Add(new DataUpdate(reader));
                reader.Close();
            }

            foreach (DataUpdate update in dataUpdates)
                if (DataUpdateAction.PerformAction(update))
                    tData.SaveDataActionProcessed(update.DataUpdateGuid, this.ActionId);
        }

        #endregion

        #region Private Methods

        private void SaveAction(string currentUser)
        {
            _actionId = new ActionData().SaveAction(
                ActionId,
                ActionOrder,
                Name,
                Description,
                ActionAssembly,
                currentUser);

            if (_settings != null)
                _settings.Save(_actionId);
        }

        private void LoadAction(SqlDataReader reader)
        {
            if (!reader.IsDBNull(reader.GetOrdinal("action_id")))
                _actionId = (int)reader["action_id"];

            if (!reader.IsDBNull(reader.GetOrdinal("action_order")))
                _actionOrder = (int)reader["action_order"];

            if (!reader.IsDBNull(reader.GetOrdinal("action_name")))
                _name = reader["action_name"].ToString();

            if (!reader.IsDBNull(reader.GetOrdinal("action_description")))
                _description = reader["action_description"].ToString();

            if (!reader.IsDBNull(reader.GetOrdinal("action_assembly")))
                _actionAssembly = reader["action_assembly"].ToString();
        }

        #endregion

        #region Constructors

        public Action()
        {

        }

        public Action(int actionId)
        {
            SqlDataReader reader = new ActionData().GetActionByID(actionId);
            if (reader.Read())
                LoadAction(reader);
            reader.Close();
        }

        public Action(SqlDataReader reader)
        {
            LoadAction(reader);
        }

        #endregion
    }
}





