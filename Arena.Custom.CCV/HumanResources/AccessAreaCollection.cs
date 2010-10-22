
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
	/// Summary description for AccessArea.
	/// </summary>
	[Serializable]
	public class AccessAreaCollection : ArenaCollectionBase
	{
		#region Class Indexers

		public new AccessArea this [int index]
		{
			get
			{
				if (this.List.Count > 0)
				{
					return (AccessArea)this.List[index];
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

        public AccessArea Current
        {
            get
            {
                DateTime date = DateTime.MinValue;
                AccessArea current = null;

                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].DateCreated > date)
                    {
                        date = this[i].DateCreated;
                        current = this[i];
                    }
                }

                return current;
            }
        }

        #endregion

        #region Constructors

        public AccessAreaCollection()
		{
		}

        public AccessAreaCollection(int personID)
        {
            SqlDataReader reader = new  AccessAreaData().GetAccessAreaByID(personID);
            while (reader.Read())
                    this.Add(new AccessArea(reader));
            reader.Close();
        }

		#endregion

		#region Public Methods

		public void Add(AccessArea item)
		{
			this.List.Add(item);
		}

		public void Insert(int index, AccessArea item)
		{
			this.List.Insert(index, item);
		}

		#endregion
	}
}
