using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Arena.DataLib;
using Arena.DataLayer;

namespace Arena.Custom.CCV.DataLayer.HumanResources
{
    public class JobApplicantData : SqlData
    {
        public JobApplicantData()
        {
        }

        /// <summary>
        /// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
        /// containing the Job Applicant with the ID specified
        /// </summary>
        /// <param name="jobPostingID">Job Applicant ID</param>
        /// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
        public SqlDataReader GetJobApplicantByID(int jobApplicantID)
        {
            ArrayList lst = new ArrayList();
            lst.Add(new SqlParameter("@ApplicantID", jobApplicantID));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_applicantByID", lst);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                lst = null;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
        /// containing the Job Applicant with the GUID specified
        /// </summary>
        /// <param name="jobPostingID">Job Applicant GUID</param>
        /// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
        public SqlDataReader GetJobApplicantByGuid(Guid jobAppliantGuid)
        {
            ArrayList lst = new ArrayList();
            lst.Add(new SqlParameter("@Guid", jobAppliantGuid));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_applicantByGuid", lst);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                lst = null;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.Data.DataTable">DataTable</see> object
        /// containing the Job Applicants with Job Posting ID specified
        /// </summary>
        /// <param name="jobPostingID">Job Posting ID</param>
        /// <param name="jobPostingID">Rejection Notification Sent</param>
        /// <returns><see cref="System.Data.DataTable">DataTable</see></returns>
        public DataTable GetJobApplicantList_DT(int jobPostingID, bool rejectionNotificationSent, bool reviewedByHR)
        {
            ArrayList lst = new ArrayList();
            lst.Add(new SqlParameter("@JobPostingID", jobPostingID));
            lst.Add(new SqlParameter("@RejectionSent", rejectionNotificationSent));
            lst.Add(new SqlParameter("@ReviewedByHR", reviewedByHR));

            try
            {
                return this.ExecuteDataTable("cust_ccv_sp_hr_get_applicantList", lst);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                lst = null;
            }
        }

        /// <summary>
        /// Deletes a job application record.
        /// </summary>
        /// <param name="jobPostingID">The job_applicant_id key to delete.</param>
        public void DeleteJobApplicant(int jobApplicantID)
        {
            ArrayList lst = new ArrayList();
            lst.Add(new SqlParameter("@ApplicantID", jobApplicantID));

            try
            {
                this.ExecuteNonQuery("cust_ccv_sp_hr_del_applicant", lst);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                lst = null;
            }
        }

        /// <summary>
        /// Saves job application record
        /// </summary>
        /// <returns><see cref="int">int</see></returns>
        public int SaveJobApplicant(int jobApplicantID, Guid jobApplicantGuid, int personID, int jobPostingID, string firstName, 
                                    string lastName, string email, string howHeard, string howLongChristian, bool class100, 
                                    DateTime class100Date, bool churchMember, bool neighorhoodGroup, bool serving, 
                                    string servingMinistry, bool baptized, bool tithing, string experience, string ledToApply, 
                                    string coverletter, int resumeBlobID,  bool rejectionNoticeSent, bool reviewedByHR, string userID)
        {
            ArrayList lst = new ArrayList();
            lst.Add(new SqlParameter("@ApplicantID", jobApplicantID));
            lst.Add(new SqlParameter("@Guid", jobApplicantGuid));
            lst.Add(new SqlParameter("@PersonID", FKHelper.SaveForeignKey(personID)));
            lst.Add(new SqlParameter("@JobPostingID", FKHelper.SaveForeignKey(jobPostingID)));
            lst.Add(new SqlParameter("@FirstName", firstName));
            lst.Add(new SqlParameter("@LastName", lastName));
            lst.Add(new SqlParameter("@Email", email));
            lst.Add(new SqlParameter("@HowHeard", howHeard));
            lst.Add(new SqlParameter("@HowLongChristian", howLongChristian));
            lst.Add(new SqlParameter("@Class100", class100));
            lst.Add(new SqlParameter("@Class100Date", class100Date));
            lst.Add(new SqlParameter("@Churchmember", churchMember));
            lst.Add(new SqlParameter("@NeighborhoodGroup", neighorhoodGroup));
            lst.Add(new SqlParameter("@Serving", serving));
            lst.Add(new SqlParameter("@ServingMinistry", servingMinistry));
            lst.Add(new SqlParameter("@Baptized", baptized));
            lst.Add(new SqlParameter("@Tithing", tithing));
            lst.Add(new SqlParameter("@Experience", experience));
            lst.Add(new SqlParameter("@LedToApply", ledToApply));
            lst.Add(new SqlParameter("@Coverletter", coverletter));
            lst.Add(new SqlParameter("@ResumeBlobID", FKHelper.SaveForeignKey(resumeBlobID)));
            lst.Add(new SqlParameter("@RejectionNoticeSent", rejectionNoticeSent));
            lst.Add(new SqlParameter("@ReviewedByHR", reviewedByHR));
            lst.Add(new SqlParameter("@UserID", userID));

            SqlParameter paramOut = new SqlParameter();
            paramOut.ParameterName = "@ID";
            paramOut.Direction = ParameterDirection.Output;
            paramOut.SqlDbType = SqlDbType.Int;
            lst.Add(paramOut);

            try
            {
                this.ExecuteNonQuery("cust_ccv_sp_hr_save_applicant", lst);
                return (int)((SqlParameter)(lst[lst.Count - 1])).Value;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique Key Violation
                    return -1;
                else
                    throw ex;
            }
            finally
            {
                lst = null;
            }
        }
    }
}
