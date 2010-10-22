
using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Arena.DataLib;

namespace Arena.Custom.CCV.DataLayer.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class ActionSettingData : SqlData
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public ActionSettingData()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see> object
		/// containing the ActionSetting with the ID specified
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public SqlDataReader GetActionSettingByID(int actionId, string name)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@ActionId", actionId));
			lst.Add(new SqlParameter("@Name", name));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_data_sp_get_ActionSettingByID", lst);
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
		
		public SqlDataReader GetActionSettingByActionId(int actionId)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@ActionId", actionId));

			try
			{
                return this.ExecuteSqlDataReader("cust_ccv_data_sp_get_actionSettingByActionId", lst);
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
		/// deletes a ActionSetting record.
		/// </summary>
		/// <param name="roleID">The poll_id key to delete.</param>
		public void DeleteActionSetting(int actionSettingId)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@ActionSettingId", actionSettingId));

			try
			{
                this.ExecuteNonQuery("cust_ccv_data_sp_del_actionSetting", lst);
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
		/// saves ActionSetting record
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public int SaveActionSetting(int actionSettingId, int actionId, string name, string value, int typeId)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@ActionSettingId", actionSettingId));
			lst.Add(new SqlParameter("@ActionId", actionId));
			lst.Add(new SqlParameter("@Name", name));
			lst.Add(new SqlParameter("@Value", value));
			lst.Add(new SqlParameter("@TypeId", typeId));

			SqlParameter paramOut = new SqlParameter();
			paramOut.ParameterName = "@ID";
			paramOut.Direction = ParameterDirection.Output;
			paramOut.SqlDbType = SqlDbType.Int;
			lst.Add(paramOut);
			
			try
			{
                this.ExecuteNonQuery("cust_ccv_data_sp_save_actionSettingByID", lst);
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

		/// <summary>
		/// saves ActionSetting record.  If personId is -1, then it deletes all of the system
		/// module settings.  If personId is not -1, then it deletes all of the module settings for
		/// that person.
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
		public void DeleteActionSettings(int actionId)
		{
			ArrayList lst = new ArrayList();

			lst.Add(new SqlParameter("@ActionId", actionId));

			try
			{
                this.ExecuteNonQuery("cust_ccv_data_sp_del_actionSettings", lst);
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


