
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
	/// Summary description for SalaryHistory.
	/// </summary>
	[Serializable]
	public class SalaryHistory : ArenaObjectBase
	{
		#region Private Members

		private int _salaryHistoryID = -1; 
		private DateTime _dateCreated = DateTime.Now; 
		private DateTime _dateModified = DateTime.Now; 
		private string _createdBy = string.Empty; 
		private string _modifiedBy = string.Empty; 
		private int _personID = -1;
		private bool _fullTime = false; 
		private decimal _hourlyRate = 0; 
		private decimal _salary = 0; 
		private decimal _housing = 0; 
		private decimal _fuel = 0;
        private DateTime _raiseDate = DateTime.Parse("1/1/1900");
		private decimal _raiseAmount = 0; 
		private int _reviewScoreLuid = -1;
        private Lookup _reviewScore = null;
        private DateTime _reviewDate = DateTime.Parse("1/1/1900"); 
		private int _reviewerID = -1;
        private Person _reviewer = null;
        private DateTime _nextReview = DateTime.Parse("1/1/1900");

		#endregion

		#region Public Properties
		
		public int SalaryHistoryID
		{
			get{ return _salaryHistoryID; }
			set{ _salaryHistoryID = value; }
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

        public int PersonID
        {
            get { return _personID; }
            set { _personID = value; }
        }

		public bool FullTime
		{
            get { return _fullTime; }
            set { _fullTime = value; }
		}

		public decimal HourlyRate
		{
            get { return _hourlyRate; }
            set { _hourlyRate = value; }
		}

		public decimal Salary
		{
            get { return _salary; }
            set { _salary = value; }
		}

		public decimal Housing
		{
            get { return _housing; }
            set { _housing = value; }
		}

		public decimal Fuel
		{
            get { return _fuel; }
            set { _fuel = value; }
		}

		public DateTime RaiseDate
		{
            get { return _raiseDate; }
            set { _raiseDate = value; }
		}

		public decimal RaiseAmount
		{
            get { return _raiseAmount; }
            set { _raiseAmount = value; }
		}

        public Lookup ReviewScore
        {
            get
            {
                if (_reviewScore == null)
                    _reviewScore = new Lookup(_reviewScoreLuid);
                return _reviewScore;
            }

            set
            {
                _reviewScore = value;
                if (_reviewScore == null)
                    _reviewScoreLuid = -1;
                else
                    _reviewScoreLuid = _reviewScore.LookupID;
            }
        }

		public DateTime ReviewDate
		{
            get { return _reviewDate; }
            set { _reviewDate = value; }
		}

        public int ReviewerID
        {
            get { return _reviewerID; }
            set { _reviewerID = value; }
        }

        public Person Reviewer
        {
            get
            {
                if (_reviewer == null || _reviewerID != _reviewer.PersonID)
                    _reviewer = new Person(_reviewerID);
                return _reviewer;
            }
        }

		public DateTime NextReview
		{
            get { return _nextReview; }
            set { _nextReview = value; }
		}


		#endregion

		#region Public Methods
		public void Save(string userID)
		{
			SaveSalaryHistory(userID);	
		}


		public static void Delete(int salaryHistoryID)
		{
			new SalaryHistoryData().DeleteSalaryHistory(salaryHistoryID);
		}
		
		public void Delete()
		{
			// delete record
			SalaryHistoryData salaryhistoryData = new SalaryHistoryData();
			salaryhistoryData.DeleteSalaryHistory(_salaryHistoryID);
												
			_salaryHistoryID = -1;
		}
		
		#endregion

		#region Private Methods

		private void SaveSalaryHistory(string userID)
		{
			_salaryHistoryID = new SalaryHistoryData().SaveSalaryHistory(
				SalaryHistoryID,
				PersonID,
				FullTime,
				HourlyRate,
				Salary,
				Housing,
				Fuel,
				RaiseDate,
				RaiseAmount,
				ReviewScore.LookupID,
				ReviewDate,
				Reviewer.PersonID,
				NextReview,
				userID);
		}

		private void LoadSalaryHistory(SqlDataReader reader)
		{
			if (!reader.IsDBNull(reader.GetOrdinal("salary_history_id")))
				_salaryHistoryID = (int)reader["salary_history_id"];

			if (!reader.IsDBNull(reader.GetOrdinal("date_created")))
				_dateCreated = (DateTime)reader["date_created"];

			if (!reader.IsDBNull(reader.GetOrdinal("date_modified")))
				_dateModified = (DateTime)reader["date_modified"];

			_createdBy  = reader["created_by"].ToString();

			_modifiedBy  = reader["modified_by"].ToString();

			if (!reader.IsDBNull(reader.GetOrdinal("person_id")))
				_personID = (int)reader["person_id"];

			if (reader.IsDBNull(reader.GetOrdinal("full_time")))
				_fullTime = false;
			else
				_fullTime = (bool)reader["full_time"];

			if (!reader.IsDBNull(reader.GetOrdinal("hourly_rate")))
				_hourlyRate = (decimal)reader["hourly_rate"];

			if (!reader.IsDBNull(reader.GetOrdinal("salary")))
				_salary = (decimal)reader["salary"];

			if (!reader.IsDBNull(reader.GetOrdinal("housing")))
				_housing = (decimal)reader["housing"];

			if (!reader.IsDBNull(reader.GetOrdinal("fuel")))
				_fuel = (decimal)reader["fuel"];

			if (!reader.IsDBNull(reader.GetOrdinal("raise_date")))
				_raiseDate = (DateTime)reader["raise_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("raise_amount")))
				_raiseAmount = (decimal)reader["raise_amount"];

			if (!reader.IsDBNull(reader.GetOrdinal("review_score_luid")))
				_reviewScoreLuid = (int)reader["review_score_luid"];

			if (!reader.IsDBNull(reader.GetOrdinal("review_date")))
				_reviewDate = (DateTime)reader["review_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("reviewer_id")))
				_reviewerID = (int)reader["reviewer_id"];

			if (!reader.IsDBNull(reader.GetOrdinal("next_review")))
				_nextReview = (DateTime)reader["next_review"];

		}


		#endregion

        #region Constructors

        public SalaryHistory()
		{
			
		}

		public SalaryHistory(int salaryHistoryID)
		{
			SqlDataReader reader = new SalaryHistoryData().GetSalaryHistoryByID(salaryHistoryID);
			if (reader.Read())
				LoadSalaryHistory(reader);
			reader.Close();
		}

        public SalaryHistory(Guid personGuid)
        {
            SqlDataReader reader = new SalaryHistoryData().GetSalaryHistoryByPersonID(new Person(personGuid).PersonID);
            if (reader.Read())
                LoadSalaryHistory(reader);
            reader.Close();
        }

		public SalaryHistory(SqlDataReader reader)
		{
			LoadSalaryHistory(reader);
		}

		#endregion
	}
}





