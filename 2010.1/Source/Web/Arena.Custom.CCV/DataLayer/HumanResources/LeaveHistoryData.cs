

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
	public class LeaveHistoryData : SqlData
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public LeaveHistoryData()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
		/// containing the LeaveHistory with the ID specified
		/// </summary>
		/// <param name="leaveHistoryID">LeaveHistory ID</param>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public SqlDataReader GetLeaveHistoryByID(int leaveHistoryID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@LeaveHistoryId", leaveHistoryID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_leaveHistoryByID", lst);
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
		
		public SqlDataReader GetLeaveHistoryByPersonID(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_leaveHistoryByPersonID", lst);
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

		public DataTable GetLeaveHistoryByPersonID_DT(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                return this.ExecuteDataTable("cust_ccv_sp_hr_get_leaveHistoryByPersonID", lst);
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
		/// deletes a LeaveHistory record.
		/// </summary>
		/// <param name="roleID">The poll_id key to delete.</param>
		public void DeleteLeaveHistory(int leaveHistoryID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@LeaveHistoryID", leaveHistoryID));

			try
			{
                this.ExecuteNonQuery("cust_ccv_sp_hr_del_leaveHistory", lst);
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
		/// saves LeaveHistory record
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public int SaveLeaveHistory(int leaveHistoryID, int personID, int leaveTypeLuid, string leaveReason, DateTime leaveDate, DateTime returnDate, string notes, string userID)
		{
			ArrayList lst = new ArrayList();
			
			lst.Add(new SqlParameter("@LeaveHistoryID", leaveHistoryID));
			lst.Add(new SqlParameter("@PersonID", FKHelper.SaveForeignKey(personID)));
			lst.Add(new SqlParameter("@LeaveTypeLuid", leaveTypeLuid));
			lst.Add(new SqlParameter("@LeaveReason", leaveReason));
			lst.Add(new SqlParameter("@LeaveDate", leaveDate));
			lst.Add(new SqlParameter("@ReturnDate", returnDate));
			lst.Add(new SqlParameter("@Notes", notes));
			lst.Add(new SqlParameter("@UserID", userID));
			
			SqlParameter paramOut = new SqlParameter();
			paramOut.ParameterName = "@ID";
			paramOut.Direction = ParameterDirection.Output;
			paramOut.SqlDbType = SqlDbType.Int;
			lst.Add(paramOut);
			
			try
			{
                this.ExecuteNonQuery("cust_ccv_sp_hr_save_leaveHistory", lst);
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


