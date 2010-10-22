

using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

using Arena.DataLib;
using Arena.DataLayer;

namespace Arena.Custom.CCV.DataLayer.HumanResources

{
	/// <summary>
	/// 
	/// </summary>
	public class SalaryHistoryData : SqlData
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public SalaryHistoryData()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
		/// containing the SalaryHistory with the ID specified
		/// </summary>
		/// <param name="salaryHistoryID">SalaryHistory ID</param>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public SqlDataReader GetSalaryHistoryByID(int salaryHistoryID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@SalaryHistoryID", salaryHistoryID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_salaryHistoryByID", lst);
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
		
		public SqlDataReader GetSalaryHistoryByPersonID(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_salaryHistoryByPersonID", lst);
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

		public DataTable GetSalaryHistoryByPersonID_DT(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                return this.ExecuteDataTable("cust_ccv_sp_hr_get_salaryHistoryByPersonID", lst);
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

		public SqlDataReader GetSalaryHistoryByReviewerID(int reviewerID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@ReviewerID", reviewerID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_salaryHistoryByReviewerID", lst);
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

		public DataTable GetSalaryHistoryByReviewerID_DT(int reviewerID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@ReviewerID", reviewerID));

			try
			{
                return this.ExecuteDataTable("cust_ccv_sp_hr_get_salaryHistoryByReviewerID", lst);
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
		/// deletes a SalaryHistory record.
		/// </summary>
		/// <param name="roleID">The poll_id key to delete.</param>
		public void DeleteSalaryHistory(int salaryHistoryID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@SalaryHistoryID", salaryHistoryID));

			try
			{
				this.ExecuteNonQuery("cust_ccv_sp_hr_del_salaryHistory", lst);
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
		/// saves SalaryHistory record
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public int SaveSalaryHistory(int salaryHistoryID, int personID, bool fullTime, decimal hourlyRate, decimal salary, decimal housing, decimal fuel, 
                                     DateTime raiseDate, decimal raiseAmount, int reviewScoreLuid, DateTime reviewDate, int reviewerID, DateTime nextReview, string userID)
		{
			ArrayList lst = new ArrayList();
			
			lst.Add(new SqlParameter("@SalaryHistoryID", salaryHistoryID));
			lst.Add(new SqlParameter("@PersonID", FKHelper.SaveForeignKey(personID)));
			lst.Add(new SqlParameter("@FullTime", fullTime));
			lst.Add(new SqlParameter("@HourlyRate", hourlyRate));
			lst.Add(new SqlParameter("@Salary", salary));
			lst.Add(new SqlParameter("@Housing", housing));
			lst.Add(new SqlParameter("@Fuel", fuel));
			lst.Add(new SqlParameter("@RaiseDate", raiseDate));
			lst.Add(new SqlParameter("@RaiseAmount", raiseAmount));
			lst.Add(new SqlParameter("@ReviewScoreLuid", reviewScoreLuid));
			lst.Add(new SqlParameter("@ReviewDate", reviewDate));
			lst.Add(new SqlParameter("@ReviewerID", FKHelper.SaveForeignKey(reviewerID)));
			lst.Add(new SqlParameter("@NextReview", nextReview));
			lst.Add(new SqlParameter("@UserID", userID));
			
			SqlParameter paramOut = new SqlParameter();
			paramOut.ParameterName = "@ID";
			paramOut.Direction = ParameterDirection.Output;
			paramOut.SqlDbType = SqlDbType.Int;
			lst.Add(paramOut);
			
			try
			{
                this.ExecuteNonQuery("cust_ccv_sp_hr_save_salaryHistory", lst);
				return (int)((SqlParameter)(lst[lst.Count - 1])).Value;
			}
			catch (SqlException ex)
			{
				if (ex.Number == 2627) //Unique Key Violation
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


