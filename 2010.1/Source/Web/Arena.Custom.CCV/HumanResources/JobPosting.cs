using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Arena.Core;
using Arena.Exceptions;
using Arena.Custom.CCV.DataLayer.HumanResources;

namespace Arena.Custom.CCV.HumanResources
{
    public class JobPosting : ArenaObjectBase
    {
        #region Private Members

        private int _jobPostingID = -1;
        private DateTime _dateCreated = DateTime.Now;
        private DateTime _dateModified = DateTime.Now;
        private string _createdBy = string.Empty;
        private string _modifiedBy = string.Empty;
        private Guid _jobPostingGuid = Guid.NewGuid();
        private string _title = string.Empty;
        private bool _fullTime = true;
        private bool _paidPosition = true;
        private DateTime _datePosted = DateTime.Now;
        private bool _showExternal = false;
        private string _description = string.Empty;

        #endregion

        #region Public Properties

        public int JobPostingID
        {
            get { return _jobPostingID; }
            set { _jobPostingID = value; }
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

        public Guid JobPostingGuid
        {
            get { return _jobPostingGuid; }
            set { _jobPostingGuid = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public bool FullTime
        {
            get { return _fullTime; }
            set { _fullTime = value; }
        }

        public bool PaidPosition
        {
            get { return _paidPosition; }
            set { _paidPosition = value; }
        }

        public DateTime DatePosted
        {
            get { return _datePosted; }
            set { _datePosted = value; }
        }

        public bool ShowExternal
        {
            get { return _showExternal; }
            set { _showExternal = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        #region Public Methods

        public void Save(string userID)
        {
            SavePosting(userID);
        }

        static public void Delete(int jobPostingID)
        {
            new JobPostingData().DeleteJobPosting(jobPostingID);
        }

        public void Delete()
        {
            JobPostingData postingData = new JobPostingData();
            postingData.DeleteJobPosting(_jobPostingID);
            _jobPostingID = -1;
        }

        #endregion

        #region Private Methods

        private void SavePosting(string userID)
        {
            _jobPostingID = new JobPostingData().SaveJobPosting(
                JobPostingID,
                JobPostingGuid,
                Title,
                FullTime,
                PaidPosition,
                DatePosted,
                ShowExternal,
                Description,
                userID);
        }

        private void LoadPosting(SqlDataReader reader)
        {
            if (!reader.IsDBNull(reader.GetOrdinal("job_posting_id")))
                _jobPostingID = (int)reader["job_posting_id"];

            if (!reader.IsDBNull(reader.GetOrdinal("guid")))
                _jobPostingGuid = (Guid)reader["guid"];

            if (!reader.IsDBNull(reader.GetOrdinal("date_created")))
                _dateCreated = (DateTime)reader["date_created"];

            if (!reader.IsDBNull(reader.GetOrdinal("date_modified")))
                _dateModified = (DateTime)reader["date_modified"];

            _createdBy = reader["created_by"].ToString();
            _modifiedBy = reader["modified_by"].ToString();
            _title = reader["title"].ToString();

            if (!reader.IsDBNull(reader.GetOrdinal("full_time")))
                _fullTime = (bool)reader["full_time"];

            if (!reader.IsDBNull(reader.GetOrdinal("paid_position")))
                _paidPosition = (bool)reader["paid_position"];

            if (!reader.IsDBNull(reader.GetOrdinal("date_posted")))
                _datePosted = (DateTime)reader["date_posted"];

            if (!reader.IsDBNull(reader.GetOrdinal("show_external")))
                _showExternal = (bool)reader["show_external"];

            _description = reader["description"].ToString();
        }

        #endregion

        #region Constructors

        public JobPosting()
        {
        }

        public JobPosting(int jobPostingID)
        {
            SqlDataReader reader = new JobPostingData().GetJobPostingByID(jobPostingID);
            if (reader.Read())
                LoadPosting(reader);
            reader.Close();
        }

        public JobPosting(Guid jobPostingGuid)
        {
            SqlDataReader reader = new JobPostingData().GetJobPostingByGuid(jobPostingGuid);
            if (reader.Read())
                LoadPosting(reader);
            reader.Close();
        }

        public JobPosting(string title)
        {
            SqlDataReader reader = new JobPostingData().GetJobPostingByTitle(title);
            if (reader.Read())
                LoadPosting(reader);
            reader.Close();
        }

        #endregion
    }
}
