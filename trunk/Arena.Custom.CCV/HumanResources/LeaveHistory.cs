
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
	/// Summary description for LeaveHistory.
	/// </summary>
	[Serializable]
	public class LeaveHistory : ArenaObjectBase
	{
		#region Private Members

		private int _leaveHistoryID = -1; 
		private DateTime _dateCreated = DateTime.Now; 
		private DateTime _dateModified = DateTime.Now; 
		private string _createdBy = string.Empty; 
		private string _modifiedBy = string.Empty; 
		private int _personID = -1;
		private int _leaveTypeLuid = -1; 
		private Lookup _leaveType = null; 
		private string _leaveReason = string.Empty;
        private DateTime _leaveDate = DateTime.Parse("1/1/1900");
        private DateTime _returnDate = DateTime.Parse("1/1/1900");
		private string _notes = string.Empty; 

		#endregion

		#region Public Properties
		
		public int LeaveHistoryID
		{
			get{return _leaveHistoryID;}
			set{_leaveHistoryID = value;}
		}

		public DateTime DateCreated
		{
			get{return _dateCreated;}
			set{_dateCreated = value;}
		}

		public DateTime DateModified
		{
			get{return _dateModified;}
			set{_dateModified = value;}
		}

		public string CreatedBy
		{
			get{return _createdBy;}
			set{_createdBy = value;}
		}

		public string ModifiedBy
		{
			get{return _modifiedBy;}
			set{_modifiedBy = value;}
		}

        public int PersonID
        {
            get { return _personID; }
            set { _personID = value; }
        }

		public Lookup LeaveType
		{
			get
			{
				if (_leaveType == null)
					_leaveType = new Lookup(_leaveTypeLuid);
				return _leaveType;
			}
			set
			{
				_leaveType = value;
				if (_leaveType == null)
					_leaveTypeLuid = -1;
				else
					_leaveTypeLuid = _leaveType.LookupID;
			}
		}

		public string LeaveReason
		{
			get{return _leaveReason;}
			set{_leaveReason = value;}
		}

		public DateTime LeaveDate
		{
			get{return _leaveDate;}
			set{_leaveDate = value;}
		}

		public DateTime ReturnDate
		{
			get{return _returnDate;}
			set{_returnDate = value;}
		}

		public string Notes
		{
			get{return _notes;}
			set{_notes = value;}
		}


		#endregion

		#region Public Methods
		public void Save(string userId)
		{
			SaveLeaveHistory(userId);	
		}


		public static void Delete(int leaveHistoryID)
		{
			new LeaveHistoryData().DeleteLeaveHistory(leaveHistoryID);
		}
		
		public void Delete()
		{

			// delete record
			LeaveHistoryData leavehistoryData = new LeaveHistoryData();
			leavehistoryData.DeleteLeaveHistory(_leaveHistoryID);
												
			_leaveHistoryID = -1;
		}
		
		#endregion

		#region Private Methods

		private void SaveLeaveHistory(string userID)
		{
			_leaveHistoryID = new LeaveHistoryData().SaveLeaveHistory(
				LeaveHistoryID,
				PersonID,
				LeaveType.LookupID,
				LeaveReason,
				LeaveDate,
				ReturnDate,
				Notes,
				userID);
		}

		private void LoadLeaveHistory(SqlDataReader reader)
		{
			if (!reader.IsDBNull(reader.GetOrdinal("leave_history_id")))
				_leaveHistoryID = (int)reader["leave_history_id"];

			if (!reader.IsDBNull(reader.GetOrdinal("date_created")))
				_dateCreated = (DateTime)reader["date_created"];

			if (!reader.IsDBNull(reader.GetOrdinal("date_modified")))
				_dateModified = (DateTime)reader["date_modified"];

			_createdBy  = reader["created_by"].ToString();

			_modifiedBy  = reader["modified_by"].ToString();

			if (!reader.IsDBNull(reader.GetOrdinal("person_id")))
				_personID = (int)reader["person_id"];

			if (!reader.IsDBNull(reader.GetOrdinal("leave_type_luid")))
				_leaveTypeLuid = (int)reader["leave_type_luid"];

			_leaveReason  = reader["leave_reason"].ToString();

			if (!reader.IsDBNull(reader.GetOrdinal("leave_date")))
				_leaveDate = (DateTime)reader["leave_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("return_date")))
				_returnDate = (DateTime)reader["return_date"];

			_notes  = reader["notes"].ToString();

		}


		#endregion
		
		#region Constructors
		
		public LeaveHistory()
		{
			
		}

		public LeaveHistory(int leaveHistoryID)
		{
			SqlDataReader reader = new LeaveHistoryData().GetLeaveHistoryByID(leaveHistoryID);
			if (reader.Read())
				LoadLeaveHistory(reader);
			reader.Close();
		}

        public LeaveHistory(Guid personGuid)
        {
            SqlDataReader reader = new LeaveHistoryData().GetLeaveHistoryByPersonID(new Person(personGuid).PersonID);
            if (reader.Read())
                LoadLeaveHistory(reader);
            reader.Close();
        }

		public LeaveHistory(SqlDataReader reader)
		{
			LoadLeaveHistory(reader);
		}

		#endregion
	}
}





