using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Arena.DataLib;

namespace Arena.Custom.CCV.DataLayer.HumanResources
{
    public class JobPostingData : SqlData
    {
        public JobPostingData()
        {
        }

        /// <summary>
        /// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
        /// containing the Job Posting with the ID specified
        /// </summary>
        /// <param name="jobPostingID">Job Posting ID</param>
        /// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
        public SqlDataReader GetJobPostingByID(int jobPostingID)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@JobPostingID", jobPostingID));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_positionByID", lst);
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
        /// containing the Job Posting with the GUID specified
        /// </summary>
        /// <param name="jobPostingID">Job Posting GUID</param>
        /// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
        public SqlDataReader GetJobPostingByGuid(Guid jobPostingGuid)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@Guid", jobPostingGuid));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_positionByGuid", lst);
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
        /// containing the Job Posting with the Title specified
        /// </summary>
        /// <param name="jobPostingID">Job Posting Title</param>
        /// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
        public SqlDataReader GetJobPostingByTitle(string title)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@Title", title));

            SqlParameter paramOut = new SqlParameter();
            paramOut.ParameterName = "@ID";
            paramOut.Direction = ParameterDirection.Output;
            paramOut.SqlDbType = SqlDbType.Int;
            lst.Add(paramOut);

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_positionByTitle", lst);
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
        /// Returns a <see cref="System.Data.DataTable">SqlDataReader</see> object
        /// containing the Job Posting with External Only specified
        /// </summary>
        /// <param name="jobPostingID">External Only</param>
        /// <returns><see cref="System.Data.DataTable">SqlDataReader</see></returns>
        public SqlDataReader GetJobPostingList(bool showExternal)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@ExternalOnly", showExternal));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_positionList", lst);
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
        /// containing the Job Posting with External Only specified
        /// </summary>
        /// <param name="jobPostingID">External Only</param>
        /// <returns><see cref="System.Data.DataTable">DataTable</see></returns>
        public DataTable GetJobPostingList_DT(bool showExternal)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@ExternalOnly", showExternal));

            try
            {
                return this.ExecuteDataTable("cust_ccv_sp_hr_get_positionList", lst);
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
        /// Deletes a job posting record.
        /// </summary>
        /// <param name="jobPostingID">The job_posting_id key to delete.</param>
        public void DeleteJobPosting(int jobPostingID)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@JobPostingID", jobPostingID));

            try
            {
                this.ExecuteNonQuery("cust_ccv_sp_hr_del_position", lst);
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
        /// Saves job posting record
        /// </summary>
        /// <returns><see cref="int">int</see></returns>
        public int SaveJobPosting(int jobPostingID, Guid jobPostingGuid, string title, bool fullTime, bool paidPosition, DateTime datePosted,
                                  bool showExternal, string description, string userID)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@JobPostingID", jobPostingID));
            lst.Add(new SqlParameter("@Guid", jobPostingGuid));
            lst.Add(new SqlParameter("@Title", title));
            lst.Add(new SqlParameter("@FullTime", fullTime));
            lst.Add(new SqlParameter("@PaidPosition", paidPosition));
            lst.Add(new SqlParameter("@DatePosted", datePosted));
            lst.Add(new SqlParameter("@ShowExternal", showExternal));
            lst.Add(new SqlParameter("@Description", description));
            lst.Add(new SqlParameter("@UserID", userID));

            SqlParameter paramOut = new SqlParameter();
            paramOut.ParameterName = "@ID";
            paramOut.Direction = ParameterDirection.Output;
            paramOut.SqlDbType = SqlDbType.Int;
            lst.Add(paramOut);

            try
            {
                this.ExecuteNonQuery("cust_ccv_sp_hr_save_position", lst);
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
