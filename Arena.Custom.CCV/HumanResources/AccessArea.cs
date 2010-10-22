
using System;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Arena.Core;
using Arena.Utility;
using Arena.Custom.CCV.DataLayer.HumanResources;

namespace Arena.Custom.CCV.HumanResources
{
	/// <summary>
	/// Summary description for AccessArea.
	/// </summary>
	[Serializable]
	public class AccessArea : ArenaObjectBase
	{
		#region Private Members

		private int _personID = -1;
        private Person _person = null;
        private DateTime _dateCreated = DateTime.Now;
        private DateTime _dateModified = DateTime.Now;
        private string _createdBy = string.Empty;
        private string _modifiedBy = string.Empty;
		private int _areaLuid = -1; 
		private Lookup _area = null; 

		#endregion

		#region Public Properties
		
		public Person Person
        {
            get
            {
                if (_person == null || _personID != _person.PersonID)
                    _person = new Person(_personID);
                return _person;
            }
        }

        public int PersonID
        {
            get { return _personID; }
            set { _personID = value; }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public DateTime DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

		public Lookup Area
		{
			get
			{
				if (_area == null)
					_area = new Lookup(_areaLuid);
				return _area;
			}
			set
			{
				_area = value;
				if (_area == null)
					_areaLuid = -1;
				else
					_areaLuid = _area.LookupID;
			}
		}


		#endregion

		#region Public Methods
		public void Save(string userID)
		{
			SaveAccessArea(userID);	
		}


		public static void Delete(int personID)
		{
			new AccessAreaData().DeleteAccessArea(personID);
		}
		
		public void Delete()
		{

			// delete record
			AccessAreaData accessareaData = new AccessAreaData();
			accessareaData.DeleteAccessArea(_personID);
												
		}
		
		#endregion

		#region Private Methods

		private void SaveAccessArea(string userID)
		{
			 new AccessAreaData().SaveAccessArea(
				_personID,
				_areaLuid,
				userID);
		}

		private void LoadAccessArea(SqlDataReader reader)
		{
			if (!reader.IsDBNull(reader.GetOrdinal("person_id")))
				_personID = (int)reader["person_id"];

            if (!reader.IsDBNull(reader.GetOrdinal("date_created")))
                _dateCreated = (DateTime)reader["date_created"];

            if (!reader.IsDBNull(reader.GetOrdinal("date_modified")))
                _dateModified = (DateTime)reader["date_modified"];

            _createdBy = reader["created_by"].ToString();
            _modifiedBy = reader["modified_by"].ToString();

			if (!reader.IsDBNull(reader.GetOrdinal("area_luid")))
				_areaLuid = (int)reader["area_luid"];

		}


		#endregion
		
		#region Constructors
		
		public AccessArea()
		{
			
		}

		public AccessArea(int personID)
		{
			SqlDataReader reader = new AccessAreaData().GetAccessAreaByID(personID);
			if (reader.Read())
				LoadAccessArea(reader);
			reader.Close();
		}

		public AccessArea(SqlDataReader reader)
		{
			LoadAccessArea(reader);
		}

		#endregion
	}
}





