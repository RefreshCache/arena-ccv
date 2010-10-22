
using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

using Arena.Core;
using Arena.Custom.CCV.DataLayer.Data;


namespace Arena.Custom.CCV.Data
{
	/// <summary>
	/// Summary description for ModuleInstanceSetting.
	/// </summary>
	[Serializable]
	public class ActionSettingCollection : ArenaCollectionBase
	{
		#region Class Indexers

        public new ActionSetting this[int index]
		{
			get
			{
				if (this.List.Count > 0)
				{
                    return (ActionSetting)this.List[index];
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.List[index] = value;
			}
		}

        public ActionSetting this[string key]
		{
			get
			{
                ActionSetting setting = this.FindByName(key);
				if (setting == null)
				{
                    setting = new ActionSetting(key, string.Empty);
					this.List.Add(setting);
				}
				return setting;
			}
			set
			{
				int i = 0;
				bool found = false;
				for (i = 0; i < this.List.Count; i++)
				{
                    if (((ActionSetting)this.List[i]).Name.ToLower() == value.Name.ToLower())
					{
						found = true;
						break;
					}
				}
				if (found)
				{
					this.List[i] = value;
				}
				else
				{
					this.List.Add(value);
				}
			}
		}

		#endregion

		#region Constructors

		public ActionSettingCollection()
		{
		}

		public ActionSettingCollection(int actionId)
		{
            using (SqlDataReader reader = new ActionSettingData().GetActionSettingByActionId(actionId))
			{
				while (reader.Read())
                    this.Add(new ActionSetting(reader));
				reader.Close();
			}
		}

		#endregion

		#region Public Methods

        public void Add(ActionSetting item)
		{
			this.List.Add(item);
		}

        public ActionSetting FindByName(string name)
		{
            foreach (ActionSetting setting in this.List)
			{
				if (setting.Name.ToLower() == name.ToLower())
					return setting;
			}
			return null;
		}

		public void Save(int actionId)
		{
            ActionSettingData data = new ActionSettingData();
            data.DeleteActionSettings(actionId);

            foreach (ActionSetting setting in this)
			{
                setting.ActionId = actionId;
				setting.Save();
			}
		}

		#endregion
	}
}
