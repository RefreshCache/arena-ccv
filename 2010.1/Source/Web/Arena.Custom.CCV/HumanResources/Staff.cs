
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
	/// Summary description for Staff.
	/// </summary>
	[Serializable]
	public class Staff : Person
	{
		#region Private Members

        private int _classLuid = -1; 
		private Lookup _class = null; 
		private int _subDepartmentLuid = -1; 
		private Lookup _subDepartment = null; 
		private string _title = string.Empty;
        private DateTime _hireDate = DateTime.Parse("1/1/1900");
        private DateTime _terminationDate = DateTime.Parse("1/1/1900");
		private int _terminationTypeLuid = -1; 
		private Lookup _terminationType = null;
        private int _noticeTypeLuid = -1;
        private Lookup _noticeType = null;
        private DateTime _exitInterview = DateTime.Parse("1/1/1900");
		private bool _rehireEligible = false;
        private DateTime _rehireDate = DateTime.Parse("1/1/1900");
		private string _rehireNote = string.Empty;
        private DateTime _ministerDate = DateTime.Parse("1/1/1900");
		private int _badgeFob = -1;
        private DateTime _badgeIssued = DateTime.Parse("1/1/1900");
		private string _keys = string.Empty; 
		private int _supervisorID = -1;
        private Person _supervisor = null;
		private int _benefitEligibleLuid = -1; 
		private Lookup _benefitEligible = null;
        private DateTime _benefitStartDate = DateTime.Parse("1/1/1900");
        private DateTime _benefitEndDate = DateTime.Parse("1/1/1900");
		private int _medicalChoiceLuid = -1; 
		private Lookup _medicalChoice = null; 
		private int _dentalChoiceLuid = -1; 
		private Lookup _dentalChoice = null; 
		private int _visionChoiceLuid = -1; 
		private Lookup _visionChoice = null; 
		private int _lifeInsuranceClassLuid = -1; 
		private Lookup _lifeInsuranceClass = null; 
		private bool _retirementParticipant = false; 
		private decimal _retirementContribution = 0;
        private decimal _retirementMatch = 0;
		private decimal _hsa = 0; 
		private bool _electCobra = false;
        private DateTime _cobraLetterSent = DateTime.Parse("1/1/1900");
        private DateTime _cobraEndDate = DateTime.Parse("1/1/1900");
		private decimal _fica = 0; 
		private int _weeklyHours = -1; 
		private SalaryHistoryCollection _salaryHistory = null;
		private LeaveHistoryCollection _leaveHistory = null;
        private AccessAreaCollection _accessAreas = null;

		#endregion

		#region Public Properties
		
        public Lookup Class
		{
			get
			{
				if (_class == null)
					_class = new Lookup(_classLuid);
				return _class;
			}
			set
			{
				_class = value;
				if (_class == null)
					_classLuid = -1;
				else
					_classLuid = _class.LookupID;
			}
		}

		public Lookup SubDepartment
		{
			get
			{
				if (_subDepartment == null)
					_subDepartment = new Lookup(_subDepartmentLuid);
				return _subDepartment;
			}
			set
			{
				_subDepartment = value;
				if (_subDepartment == null)
					_subDepartmentLuid = -1;
				else
					_subDepartmentLuid = _subDepartment.LookupID;
			}
		}

		public string JobTitle
		{
			get{ return _title; }
			set{ _title = value; }
		}

		public DateTime HireDate
		{
			get{ return _hireDate; }
			set{ _hireDate = value; }
		}

		public DateTime TerminationDate
		{
			get{ return _terminationDate; }
			set{ _terminationDate = value; }
		}

		public Lookup TerminationType
		{
			get
			{
				if (_terminationType == null)
					_terminationType = new Lookup(_terminationTypeLuid);
				return _terminationType;
			}
			set
			{
				_terminationType = value;
				if (_terminationType == null)
					_terminationTypeLuid = -1;
				else
					_terminationTypeLuid = _terminationType.LookupID;
			}
		}

        public Lookup NoticeType
        {
            get
            {
                if (_noticeType == null)
                    _noticeType = new Lookup(_noticeTypeLuid);
                return _noticeType;
            }

            set
            {
                _noticeType = value;
                if (_noticeType == null)
                    _noticeTypeLuid = -1;
                else
                    _noticeTypeLuid = _noticeType.LookupID;
            }
        }

		public DateTime ExitInterview
		{
			get{ return _exitInterview; }
			set{ _exitInterview = value; }
		}

		public bool RehireEligible
		{
			get{ return _rehireEligible; }
			set{ _rehireEligible = value; }
		}

		public DateTime RehireDate
		{
			get{ return _rehireDate; }
			set{ _rehireDate = value; }
		}

		public string RehireNote
		{
			get{ return _rehireNote; }
			set{ _rehireNote = value; }
		}

		public DateTime MinisterDate
		{
			get{ return _ministerDate; }
			set{ _ministerDate = value; }
		}

		public int BadgeFob
		{
			get{ return _badgeFob; }
			set{ _badgeFob = value; }
		}

		public DateTime BadgeIssued
		{
			get{ return _badgeIssued; }
			set{ _badgeIssued = value; }
		}

		public string Keys
		{
			get{ return _keys; }
			set{ _keys = value; }
		}

        public int SupervisorID
        {
            get { return _supervisorID; }
            set { _supervisorID = value; }
        }

		public Person Supervisor
        {
            get
            {
                if (_supervisor == null || _supervisorID != _supervisor.PersonID)
                    _supervisor = new Person(_supervisorID);
                return _supervisor;
            }
        }

		public Lookup BenefitEligible
		{
			get
			{
				if (_benefitEligible == null)
					_benefitEligible = new Lookup(_benefitEligibleLuid);
				return _benefitEligible;
			}

			set
			{
				_benefitEligible = value;
				if (_benefitEligible == null)
					_benefitEligibleLuid = -1;
				else
					_benefitEligibleLuid = _benefitEligible.LookupID;
			}
		}

		public DateTime BenefitStartDate
		{
			get{ return _benefitStartDate; }
			set{ _benefitStartDate = value; }
		}

		public DateTime BenefitEndDate
		{
			get{ return _benefitEndDate; }
			set{ _benefitEndDate = value; }
		}

		public Lookup MedicalChoice
		{
			get
			{
				if (_medicalChoice == null)
					_medicalChoice = new Lookup(_medicalChoiceLuid);
				return _medicalChoice;
			}

			set
			{
				_medicalChoice = value;
				if (_medicalChoice == null)
					_medicalChoiceLuid = -1;
				else
					_medicalChoiceLuid = _medicalChoice.LookupID;
			}
		}

		public Lookup DentalChoice
		{
			get
			{
				if (_dentalChoice == null)
					_dentalChoice = new Lookup(_dentalChoiceLuid);
				return _dentalChoice;
			}

			set
			{
				_dentalChoice = value;
				if (_dentalChoice == null)
					_dentalChoiceLuid = -1;
				else
					_dentalChoiceLuid = _dentalChoice.LookupID;
			}
		}

		public Lookup VisionChoice
		{
			get
			{
				if (_visionChoice == null)
					_visionChoice = new Lookup(_visionChoiceLuid);
				return _visionChoice;
			}

			set
			{
				_visionChoice = value;
				if (_visionChoice == null)
					_visionChoiceLuid = -1;
				else
					_visionChoiceLuid = _visionChoice.LookupID;
			}
		}

		public Lookup LifeInsuranceClass
		{
			get
			{
				if (_lifeInsuranceClass == null)
					_lifeInsuranceClass = new Lookup(_lifeInsuranceClassLuid);
				return _lifeInsuranceClass;
			}

			set
			{
				_lifeInsuranceClass = value;
				if (_lifeInsuranceClass == null)
					_lifeInsuranceClassLuid = -1;
				else
					_lifeInsuranceClassLuid = _lifeInsuranceClass.LookupID;
			}
		}

		public bool RetirementParticipant
		{
			get{ return _retirementParticipant; }
			set{ _retirementParticipant = value; }
		}

		public Decimal RetirementContribution
		{
			get{ return _retirementContribution; }
			set{ _retirementContribution = value; }
		}

		public Decimal RetirementMatch
		{
			get{ return _retirementMatch; }
			set{ _retirementMatch = value; }
		}

		public decimal Hsa
		{
			get{ return _hsa; }
			set{ _hsa = value; }
		}

		public bool ElectCobra
		{
			get{ return _electCobra; }
			set{ _electCobra = value; }
		}

		public DateTime CobraLetterSent
		{
			get{ return _cobraLetterSent; }
			set{ _cobraLetterSent = value; }
		}

		public DateTime CobraEndDate
		{
			get{ return _cobraEndDate; }
			set{ _cobraEndDate = value; }
		}

		public decimal Fica
		{
			get{ return _fica; }
			set{ _fica = value; }
		}

		public int WeeklyHours
		{
			get{ return _weeklyHours; }
			set{ _weeklyHours = value; }
		}

		public SalaryHistoryCollection SalaryHistory
		{
			get
			{
				if (_salaryHistory == null)
				{
					_salaryHistory = new SalaryHistoryCollection();
					_salaryHistory.LoadByPersonID(PersonID);
				}
				return _salaryHistory;
			}

			set{ _salaryHistory = value; }
		}

		public LeaveHistoryCollection LeaveHistory
		{
			get
			{
				if (_leaveHistory == null)
				{
					_leaveHistory = new LeaveHistoryCollection(PersonID);
				}
				return _leaveHistory;
			}

			set{ _leaveHistory = value; }
		}

        public AccessAreaCollection AccessAreas
        {
            get
            {
                if (_accessAreas == null)
                    _accessAreas = new AccessAreaCollection(PersonID);
                return _accessAreas;
            }

            set { _accessAreas = value; }
        }

		#endregion

		#region Public Methods

		public void Save(string userID)
		{
			SaveStaff(userID);	
		}


		public static void Delete(int personID)
		{
			new StaffData().DeleteStaff(personID);
		}
		
		#endregion

		#region Private Methods

		private void SaveStaff(string userID)
		{
			 new StaffData().SaveStaff(
				PersonID,
				Class.LookupID,
				SubDepartment.LookupID,
				JobTitle,
				HireDate,
				TerminationDate,
				TerminationType.LookupID,
                NoticeType.LookupID,
				ExitInterview,
				RehireEligible,
				RehireDate,
				RehireNote,
				MinisterDate,
				BadgeFob,
				BadgeIssued,
				Keys,
				Supervisor.PersonID,
				BenefitEligible.LookupID,
				BenefitStartDate,
				BenefitEndDate,
				MedicalChoice.LookupID,
				DentalChoice.LookupID,
				VisionChoice.LookupID,
				LifeInsuranceClass.LookupID,
				RetirementParticipant,
				RetirementContribution,
				RetirementMatch,
				Hsa,
				ElectCobra,
				CobraLetterSent,
				CobraEndDate,
				Fica,
				WeeklyHours,
				userID);
		}

		private void LoadStaff(SqlDataReader reader)
		{
            if (!reader.IsDBNull(reader.GetOrdinal("class_luid")))
				_classLuid = (int)reader["class_luid"];

			if (!reader.IsDBNull(reader.GetOrdinal("sub_department_luid")))
				_subDepartmentLuid = (int)reader["sub_department_luid"];

			_title  = reader["title"].ToString();

			if (!reader.IsDBNull(reader.GetOrdinal("hire_date")))
				_hireDate = (DateTime)reader["hire_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("termination_date")))
				_terminationDate = (DateTime)reader["termination_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("termination_type_luid")))
				_terminationTypeLuid = (int)reader["termination_type_luid"];

            if (!reader.IsDBNull(reader.GetOrdinal("notice_type_luid")))
                _noticeTypeLuid = (int)reader["notice_type_luid"];

			if (!reader.IsDBNull(reader.GetOrdinal("exit_interview")))
				_exitInterview = (DateTime)reader["exit_interview"];

			if (reader.IsDBNull(reader.GetOrdinal("rehire_eligible")))
				_rehireEligible = false;
			else
				_rehireEligible = (bool)reader["rehire_eligible"];

			if (!reader.IsDBNull(reader.GetOrdinal("rehire_date")))
				_rehireDate = (DateTime)reader["rehire_date"];

			_rehireNote  = reader["rehire_note"].ToString();

			if (!reader.IsDBNull(reader.GetOrdinal("minister_date")))
				_ministerDate = (DateTime)reader["minister_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("badge_fob")))
				_badgeFob = (int)reader["badge_fob"];

			if (!reader.IsDBNull(reader.GetOrdinal("badge_issued")))
				_badgeIssued = (DateTime)reader["badge_issued"];

			_keys  = reader["keys"].ToString();

			if (!reader.IsDBNull(reader.GetOrdinal("supervisor_id")))
				_supervisorID = (int)reader["supervisor_id"];

			if (!reader.IsDBNull(reader.GetOrdinal("benefit_eligible_luid")))
				_benefitEligibleLuid = (int)reader["benefit_eligible_luid"];

			if (!reader.IsDBNull(reader.GetOrdinal("benefit_start_date")))
				_benefitStartDate = (DateTime)reader["benefit_start_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("benefit_end_date")))
				_benefitEndDate = (DateTime)reader["benefit_end_date"];

			if (!reader.IsDBNull(reader.GetOrdinal("medical_choice_luid")))
				_medicalChoiceLuid = (int)reader["medical_choice_luid"];

			if (!reader.IsDBNull(reader.GetOrdinal("dental_choice_luid")))
				_dentalChoiceLuid = (int)reader["dental_choice_luid"];

			if (!reader.IsDBNull(reader.GetOrdinal("vision_choice_luid")))
				_visionChoiceLuid = (int)reader["vision_choice_luid"];

			if (!reader.IsDBNull(reader.GetOrdinal("life_insurance_class_luid")))
				_lifeInsuranceClassLuid = (int)reader["life_insurance_class_luid"];

			if (reader.IsDBNull(reader.GetOrdinal("retirement_participant")))
				_retirementParticipant = false;
			else
				_retirementParticipant = (bool)reader["retirement_participant"];

			if (!reader.IsDBNull(reader.GetOrdinal("retirement_contribution")))
				_retirementContribution = (decimal)reader["retirement_contribution"];

			if (!reader.IsDBNull(reader.GetOrdinal("retirement_match")))
				_retirementMatch = (decimal)reader["retirement_match"];

			if (!reader.IsDBNull(reader.GetOrdinal("hsa")))
				_hsa = (decimal)reader["hsa"];

			if (reader.IsDBNull(reader.GetOrdinal("elect_cobra")))
				_electCobra = false;
			else
				_electCobra = (bool)reader["elect_cobra"];

			if (!reader.IsDBNull(reader.GetOrdinal("cobra_letter_sent")))
				_cobraLetterSent = (DateTime)reader["cobra_letter_sent"];

			if (!reader.IsDBNull(reader.GetOrdinal("cobra_end_date")))
				_cobraEndDate = (DateTime)reader["cobra_end_date"];

            if (!reader.IsDBNull(reader.GetOrdinal("fica")))
                _fica = (decimal)reader["fica"];

			if (!reader.IsDBNull(reader.GetOrdinal("weekly_hours")))
				_weeklyHours = (int)reader["weekly_hours"];
		}

		#endregion
		
		#region Constructors
		
		public Staff() : base()
		{
		}

		public Staff(int personID) : base(personID)
		{
			SqlDataReader reader = new StaffData().GetStaffByID(personID);
			if (reader.Read())
				LoadStaff(reader);
			reader.Close();
		}

        public Staff(Guid guid) : base(guid)
        {
            SqlDataReader reader = new StaffData().GetStaffByID(new Person(guid).PersonID);
            if (reader.Read())
                LoadStaff(reader);
            reader.Close();
        }

		public Staff(SqlDataReader reader) : base(reader)
		{
			LoadStaff(reader);
		}

		#endregion
	}
}





