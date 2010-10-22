
using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Arena.Core;
using Arena.Custom.CCV.DataLayer.HumanResources;

namespace Arena.Custom.CCV.HumanResources
{
	/// <summary>
	/// Summary description for LeaveHistory.
	/// </summary>
	[Serializable]
	public class LeaveHistoryCollection : ArenaCollectionBase
	{
		#region Class Indexers

		public new LeaveHistory this [int index]
		{
			get
			{
				if (this.List.Count > 0)
				{
					return (LeaveHistory)this.List[index];
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

        #region Public Properties

        public LeaveHistory MostRecent
        {
            get
            {
                DateTime date = DateTime.MinValue;
                LeaveHistory mostRecent = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].DateCreated > date)
                    {
                        date = this[i].DateCreated;
                        mostRecent = this[i];
                    }
                }

                return mostRecent;
            }
        }

        #endregion

        #region Constructors

        public LeaveHistoryCollection()
		{
		}

		public LeaveHistoryCollection(int personID)
		{
			SqlDataReader reader = new LeaveHistoryData().GetLeaveHistoryByPersonID(personID);
			while (reader.Read())
				this.Add(new LeaveHistory(reader));
			reader.Close();
		}

		#endregion

		#region Public Methods

		public void Add(LeaveHistory item)
		{
			this.List.Add(item);
		}

		public void Insert(int index, LeaveHistory item)
		{
			this.List.Insert(index, item);
		}

        #endregion
    }
}
