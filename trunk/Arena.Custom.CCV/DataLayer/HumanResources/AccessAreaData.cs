

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
	public class AccessAreaData : SqlData
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public AccessAreaData()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
		/// containing the AccessArea with the ID specified
		/// </summary>
		/// <param name="accessAreaID">AccessArea ID</param>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public SqlDataReader GetAccessAreaByID(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_sp_hr_get_accessAreaByID", lst);
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
		/// deletes a AccessArea record.
		/// </summary>
		/// <param name="roleID">The poll_id key to delete.</param>
		public void DeleteAccessArea(int personID)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@PersonID", personID));

			try
			{
                this.ExecuteNonQuery("cust_ccv_sp_hr_del_accessArea", lst);
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
		/// saves AccessArea record
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public void SaveAccessArea(int personID, int areaLuid, string userID)
		{
			ArrayList lst = new ArrayList();
			
			lst.Add(new SqlParameter("@PersonID", FKHelper.SaveForeignKey(personID)));
			lst.Add(new SqlParameter("@AreaLuid", areaLuid));
			lst.Add(new SqlParameter("@UserID", userID));
			
			
			try
			{
                this.ExecuteNonQuery("cust_ccv_sp_hr_save_accessArea", lst);
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
	}
}


