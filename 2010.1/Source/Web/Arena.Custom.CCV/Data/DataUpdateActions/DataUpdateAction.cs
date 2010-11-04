using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

using Arena.Core;

namespace Arena.Custom.CCV.Data
{
    [Serializable]
    public class DataUpdateAction
    {
        private int _actionId = -1;
        private ActionSettingCollection _settings = null;

        public int ActionId
        {
            get { return _actionId; }
            set { _actionId = value; }
        }

        public ActionSettingCollection Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public virtual bool PerformAction(DataUpdate dataUpdate)
        {
            return true;
        }

        protected string Setting(string setting, string defaultValue, bool required)
        {
            string _setting = Settings[setting].Value;
            if (_setting.Trim() == string.Empty)
            {
                if (!required)
                    return defaultValue;
                else
                    throw new Arena.Exceptions.ArenaApplicationException(string.Format("Assignment Type Action Configuration Error:  '{0}' action setting is required.", setting));
            }
            else
                return _setting.Trim();

        }

        #region Static Methods

        public static DataUpdateAction GetActionClass(string assemblyQualifiedName)
        {
            if (assemblyQualifiedName.Trim() != string.Empty)
            {
                Type type = Type.GetType(assemblyQualifiedName);
                if (type != null)
                    return (DataUpdateAction)Activator.CreateInstance(type);
            }

            return null;
        }

        #endregion
    }

}
