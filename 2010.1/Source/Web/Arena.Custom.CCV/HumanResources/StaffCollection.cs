
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
	/// Summary description for Staff.
	/// </summary>
	[Serializable]
	public class StaffCollection : ArenaCollectionBase
	{
		#region Class Indexers

		public new Staff this [int index]
		{
			get
			{
				if (this.List.Count > 0)
				{
					return (Staff)this.List[index];
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

		public StaffCollection()
		{
		}

        public StaffCollection(int organizationID)
        {
            LoadAllStaff(organizationID);
        }

		#endregion

		#region Public Methods

		public void Add(Staff item)
		{
			this.List.Add(item);
		}

		public void Insert(int index, Staff item)
		{
			this.List.Insert(index, item);
		}

		public void LoadBySupervisorID(int supervisorID)
		{
			SqlDataReader reader = new StaffData().GetStaffBySupervisorID(supervisorID);
			while (reader.Read())
				this.Add(new Staff(reader));
			reader.Close();
		}

        public void LoadAllStaff(int organizationID)
        {
            SqlDataReader reader = new StaffData().GetStaffByOrganizationID(organizationID);
            while (reader.Read())
                this.Add(new Staff(reader));
            reader.Close();
        }

		#endregion
	}
}
