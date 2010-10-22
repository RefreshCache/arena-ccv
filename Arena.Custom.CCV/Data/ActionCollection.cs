
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
	public class ActionCollection : ArenaCollectionBase
	{
		#region Class Indexers

        public new Action this[int index]
		{
			get
			{
				if (this.List.Count > 0)
				{
                    return (Action)this.List[index];
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

		#endregion

		#region Constructors

		public ActionCollection()
		{
		}

		#endregion

		#region Public Methods

        public void LoadAll()
        {
            using (SqlDataReader reader = new ActionData().GetActions())
			{
				while (reader.Read())
                    this.Add(new Action(reader));
				reader.Close();
			}
        }

        public void Add(Action item)
		{
			this.List.Add(item);
		}

        public void Insert(int index, Action item)
        {
            this.List.Insert(index, item);
        }

        public Action Find(int actionId)
        {
            foreach (Action action in this)
                if (action.ActionId == actionId)
                    return action;
            return null;
        }

        public Action FindByGuid(Guid guid)
        {
            foreach (Action action in this)
                if (action.Guid == guid)
                    return action;
            return null;
        }

        public void Save(string currentUser)
        {
            ActionCollection existingActions = new ActionCollection();
            existingActions.LoadAll();
            foreach (Action existingAction in existingActions)
                if (this.Find(existingAction.ActionId) == null)
                    existingAction.Delete();

            foreach (Action action in this)
            {
                if (action.Settings == null)
                    action.LoadSettings();

                action.Save(currentUser);
            }
        }

		#endregion
	}
}
