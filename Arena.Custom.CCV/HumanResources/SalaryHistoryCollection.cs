
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
	/// Summary description for SalaryHistory.
	/// </summary>
	[Serializable]
	public class SalaryHistoryCollection : ArenaCollectionBase
    {
        #region Public Properties

        public SalaryHistory MostRecent
        {
            get
            {
                DateTime date = DateTime.MinValue;
                SalaryHistory mostRecent = null;

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

        #region Class Indexers

        public new SalaryHistory this [int index]
		{
			get
			{
				if (this.List.Count > 0)
				{
					return (SalaryHistory)this.List[index];
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

		public SalaryHistoryCollection()
		{
		}

		#endregion

		#region Public Methods

		public void Add(SalaryHistory item)
		{
			this.List.Add(item);
		}

		public void Insert(int index, SalaryHistory item)
		{
			this.List.Insert(index, item);
		}

		public void LoadByPersonID(int personID)
		{
			SqlDataReader reader = new SalaryHistoryData().GetSalaryHistoryByPersonID(personID);
			while (reader.Read())
				this.Add(new SalaryHistory(reader));
			reader.Close();
		}

		public void LoadByReviewerID(int reviewerID)
		{
			SqlDataReader reader = new SalaryHistoryData().GetSalaryHistoryByReviewerID(reviewerID);
			while (reader.Read())
				this.Add(new SalaryHistory(reader));
			reader.Close();
		}

		#endregion
	}
}
