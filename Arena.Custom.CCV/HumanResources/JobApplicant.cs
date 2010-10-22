using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

using Arena.Core;
using Arena.Exceptions;
using Arena.Custom.CCV.DataLayer.HumanResources;
using Arena.Utility;

namespace Arena.Custom.CCV.HumanResources
{
    public class JobApplicant : ArenaObjectBase
    {
        #region Private Members

        private int _applicantID = -1;
        private DateTime _dateCreated = DateTime.Now;
        private DateTime _dateModified = DateTime.Now;
        private string _createdBy = string.Empty;
        private string _modifiedBy = string.Empty;
        private Guid _guid = Guid.NewGuid();
        private int _personID = -1;
        private Person _person = null;
        private int _jobPostingID = -1;
        private JobPosting _jobPosting = null;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _howHeard = string.Empty;
        private string _howLongChristian = string.Empty;
        private bool _class100 = false;
        private DateTime _class100Date = DateTime.MinValue;
        private bool _churchMember = false;
        private bool _neighborhoodGroup = false;
        private bool _serving = false;
        private string _servingMinistry = string.Empty;
        private bool _baptized = false;
        private bool _tithing = false;
        private string _experience = string.Empty;
        private string _ledToApply = string.Empty;
        private string _coverletter = string.Empty;
        private int _resumeBlobID = -1;
        private ArenaDataBlob _resume = null;
        private bool _rejectionNoticeSent = false;
        private bool _reviewedByHR = false;

        #endregion

        #region Public Properties

        public int ApplicantID
        {
            get { return _applicantID; }
            set { _applicantID = value; }
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

        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        public Person Person
        {
            get
            {
                if (_person == null)
                    _person = new Person(_personID);
                return _person;
            }

            set
            {
                _person = value;
                if (_person == null)
                    _personID = -1;
                else
                    _personID = _person.PersonID;
            }
        }

        public JobPosting JobPosting
        {
            get
            {
                if (_jobPosting == null)
                    _jobPosting = new JobPosting(_jobPostingID);
                return _jobPosting;
            }

            set
            {
                _jobPosting = value;
                if (_jobPosting == null)
                    _jobPostingID = -1;
                else
                    _jobPostingID = _jobPosting.JobPostingID;
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FullName
        {
            get 
            {
                return _firstName + " " + _lastName;
            }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string HowHeard
        {
            get { return _howHeard; }
            set { _howHeard = value; }
        }

        public string HowLongChristian
        {
            get { return _howLongChristian; }
            set { _howLongChristian = value; }
        }

        public bool Class100
        {
            get { return _class100; }
            set { _class100 = value; }
        }

        public DateTime Class100Date
        {
            get { return _class100Date; }
            set { _class100Date = value; }
        }

        public bool ChurchMember
        {
            get { return _churchMember; }
            set { _churchMember = value; }
        }

        public bool NeighborhoodGroup
        {
            get { return _neighborhoodGroup; }
            set { _neighborhoodGroup = value; }
        }

        public bool Serving
        {
            get { return _serving; }
            set { _serving = value; }
        }

        public string ServingMinistry
        {
            get { return _servingMinistry; }
            set { _servingMinistry = value; }
        }

        public bool Baptized
        {
            get { return _baptized; }
            set { _baptized = value; }
        }

        public bool Tithing
        {
            get { return _tithing; }
            set { _tithing = value; }
        }

        public string Experience
        {
            get { return _experience; }
            set { _experience = value; }
        }

        public string LedToApply
        {
            get { return _ledToApply; }
            set { _ledToApply = value; }
        }

        public string Coverletter
        {
            get { return _coverletter; }
            set { _coverletter = value; }
        }

        public ArenaDataBlob Resume
        {
            get
            {
                if (_resume == null)
                    _resume = new ArenaDataBlob(_resumeBlobID);
                return _resume;
            }

            set
            {
                _resume = value;
                if (_resume == null)
                    _resumeBlobID = -1;
                else
                    _resumeBlobID = _resume.BlobID;
            }
        }

        public bool RejectionNoticeSent
        {
            get { return _rejectionNoticeSent; }
            set { _rejectionNoticeSent = value; }
        }

        public bool ReviewedByHR
        {
            get { return _reviewedByHR; }
            set { _reviewedByHR = value; }
        }

        #endregion

        #region Public Methods

        public void Save(string userID, bool findPossibleMatch)
        {
            SavePosting(userID, findPossibleMatch);
        }

        static public void Delete(int jobApplicantID)
        {
            new JobApplicantData().DeleteJobApplicant(jobApplicantID);
        }

        #endregion

        #region Private Methods

        public void SavePosting(string userID, bool findPossibleMatch)
        {
            if (findPossibleMatch && _personID == -1 && _email.Trim() != string.Empty)
            {
                // Attempt to find person based on first name, last name, and email address
                PersonCollection possibleMatches = new PersonCollection();
                possibleMatches.LoadByName(_firstName, _lastName);
                foreach (Person person in possibleMatches)
                    foreach (PersonEmail personEmail in person.Emails)
                        if (personEmail.Email.Trim().ToLower() == _email.Trim().ToLower())
                        {
                            _personID = person.PersonID;
                            break;
                        }
            }

            _applicantID = new JobApplicantData().SaveJobApplicant(
                    ApplicantID,
                    Guid,
                    Person.PersonID,
                    JobPosting.JobPostingID,
                    FirstName,
                    LastName,
                    Email,
                    HowHeard,
                    HowLongChristian,
                    Class100,
                    Class100Date,
                    ChurchMember,
                    NeighborhoodGroup,
                    Serving,
                    ServingMinistry,
                    Baptized,
                    Tithing,
                    Experience,
                    LedToApply,
                    Coverletter,
                    Resume.BlobID,
                    RejectionNoticeSent,
                    ReviewedByHR,
                    userID);
        }

        private void LoadPosting(SqlDataReader reader)
        {
           if (!reader.IsDBNull(reader.GetOrdinal("applicant_id")))
                _applicantID = (int)reader["applicant_id"];

            if (!reader.IsDBNull(reader.GetOrdinal("date_created")))
                _dateCreated = (DateTime)reader["date_created"];

            if (!reader.IsDBNull(reader.GetOrdinal("date_modified")))
                _dateModified = (DateTime)reader["date_modified"];

            _createdBy = reader["created_by"].ToString();
            _modifiedBy = reader["modified_by"].ToString();

            if (!reader.IsDBNull(reader.GetOrdinal("guid")))
                _guid = (Guid)reader["guid"];

            if (!reader.IsDBNull(reader.GetOrdinal("person_id")))
                _personID = (int)reader["person_id"];

            if (!reader.IsDBNull(reader.GetOrdinal("job_posting_id")))
                _jobPostingID = (int)reader["job_posting_id"];

            _firstName = reader["first_name"].ToString();
            _lastName = reader["last_name"].ToString();
            _email = reader["email"].ToString();
            _howHeard = reader["how_heard"].ToString();
            _howLongChristian = reader["how_long_christian"].ToString();

            if (!reader.IsDBNull(reader.GetOrdinal("class_100")))
                _class100 = (bool)reader["class_100"];

            if (!reader.IsDBNull(reader.GetOrdinal("class_100_date")))
                _class100Date = (DateTime)reader["class_100_date"];

            if (!reader.IsDBNull(reader.GetOrdinal("church_member")))
                _churchMember = (bool)reader["church_member"];

            if (!reader.IsDBNull(reader.GetOrdinal("neighborhood_group")))
                _neighborhoodGroup = (bool)reader["neighborhood_group"];

            if (!reader.IsDBNull(reader.GetOrdinal("serving")))
                _serving = (bool)reader["serving"];

            _servingMinistry = reader["serving_ministry"].ToString();

            if (!reader.IsDBNull(reader.GetOrdinal("baptized")))
                _baptized = (bool)reader["baptized"];

            if (!reader.IsDBNull(reader.GetOrdinal("tithing")))
                _tithing = (bool)reader["tithing"];

            _experience = reader["experience"].ToString();
            _ledToApply = reader["led_to_apply"].ToString();
            _coverletter = reader["coverletter"].ToString();

            if (!reader.IsDBNull(reader.GetOrdinal("resume_blob_id")))
                _resumeBlobID = (int)reader["resume_blob_id"];

            if (!reader.IsDBNull(reader.GetOrdinal("rejection_notice_sent")))
                _rejectionNoticeSent = (bool)reader["rejection_notice_sent"];

            if (!reader.IsDBNull(reader.GetOrdinal("reviewed_by_hr")))
                _reviewedByHR = (bool)reader["reviewed_by_hr"];
        }

        #endregion

        #region Constructors

        public JobApplicant()
        {
        }

        public JobApplicant(int jobApplicantID)
        {
            SqlDataReader reader = new JobApplicantData().GetJobApplicantByID(jobApplicantID);
            if (reader.Read())
                LoadPosting(reader);
            reader.Close();
        }

        public JobApplicant(Guid jobApplicantGuid)
        {
            SqlDataReader reader = new JobApplicantData().GetJobApplicantByGuid(jobApplicantGuid);
            if (reader.Read())
                LoadPosting(reader);
            reader.Close();
        }

        #endregion

    }
}
