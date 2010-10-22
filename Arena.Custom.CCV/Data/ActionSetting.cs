
using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Arena.DataLayer;
using Arena.Custom.CCV.DataLayer.Data;
using Arena.Core;
using Arena.Portal;
using Arena.Utility;


namespace Arena.Custom.CCV.Data
{
	/// <summary>
	/// Summary description for StateSetting.
	/// </summary>
	[Serializable]
	public class ActionSetting : ArenaObjectBase
	{
		#region Private Members

		private int _actionSettingId = -1;
		private int _actionId = -1; 
		private string _name = string.Empty; 
		private string _value = string.Empty;
		private SettingAttributeType _type = SettingAttributeType.Text;

		#endregion

		#region Public Properties

		public int ActionSettingId
		{
			get { return _actionSettingId; }
			set { _actionSettingId = value; }
		}

		public int ActionId
		{
			get{return _actionId;}
			set{_actionId = value;}
		}

		public string Name
		{
			get{return _name;}
			set{_name = value;}
		}

		public string Value
		{
			get{return _value;}
			set{_value = value;}
		}

		#endregion

		#region Public Methods
		public void Save()
		{
			SaveActionSetting();	
		}


		static public void Delete(int stateSettingId)
		{
			new ActionSettingData().DeleteActionSetting(stateSettingId);
		}
		
		public void Delete()
		{
			// delete record
			ActionSettingData settingData = new ActionSettingData();
            settingData.DeleteActionSetting(_actionSettingId);
		}
		
		#endregion

		#region Private Methods

		private void SaveActionSetting()
		{
			 _actionSettingId = new ActionSettingData().SaveActionSetting(
				_actionSettingId,
				_actionId,
				_name,
				_value.Replace("=", "==").Replace(";", "^^"),
				Convert.ToInt32(_type));
		}

		private void LoadActionSetting(SqlDataReader reader)
		{
			_actionSettingId = (int)reader["action_setting_id"];

			if (!reader.IsDBNull(reader.GetOrdinal("action_id")))
                _actionId = (int)reader["action_id"];

			_name  = reader["name"].ToString();

			_value  = reader["value"].ToString().Replace("==", "=").Replace("^^", ";");

			_type = (SettingAttributeType)Enum.Parse(typeof(SettingAttributeType), reader["type_id"].ToString());
		}


		#endregion
		
		#region Constructors
		
		public ActionSetting()
		{
			
		}

		public ActionSetting(string name, string value) : this(name, value, SettingAttributeType.Text)
		{
		}

		public ActionSetting(string name, string value, SettingAttributeType type)
		{
			_name = name;
			_value = value;
			_type = type;
		}

        public ActionSetting(SqlDataReader reader)
		{
			LoadActionSetting(reader);
		}

		#endregion
	}
}





