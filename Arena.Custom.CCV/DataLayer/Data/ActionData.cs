
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
	public class ActionData : SqlData
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
        public ActionData()
		{
		}

        public SqlDataReader GetActionByID(int actionId)
        {
			ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@ActionId", actionId));

			try
			{
				return this.ExecuteSqlDataReader("cust_ccv_data_sp_get_actionByID", lst);
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

        public SqlDataReader GetActions()
        {
            try
			{
                return this.ExecuteSqlDataReader("cust_ccv_data_sp_get_actions");
			}
			catch (SqlException ex)
			{
				throw ex;
			}
		}

        public DataTable GetActions_DT()
        {
            try
            {
                return this.ExecuteDataTable("cust_ccv_data_sp_get_actions");
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteAction(int actionId)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@ActionId", actionId));

            try
            {
                this.ExecuteNonQuery("cust_ccv_data_sp_del_action", lst);
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
		/// saves AssignmentTypeState record
		/// </summary>
		/// <returns><see cref="System.Data.SqlClient.SqlDataReader">SqlDataReader</see></returns>
        public int SaveAction(int actionId, int actionOrder, string name, string description, 
            string actionAssembly, string UserId)
		{
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@ActionId", actionId));
            lst.Add(new SqlParameter("@ActionOrder", actionOrder));
            lst.Add(new SqlParameter("@ActionName", name));
            lst.Add(new SqlParameter("@ActionDescription", description));
            lst.Add(new SqlParameter("@ActionAssembly", actionAssembly));
            lst.Add(new SqlParameter("@UserId", UserId));

            SqlParameter paramOut = new SqlParameter();
            paramOut.ParameterName = "@ID";
            paramOut.Direction = ParameterDirection.Output;
            paramOut.SqlDbType = SqlDbType.Int;
            lst.Add(paramOut);

            try
			{
                this.ExecuteNonQuery("cust_ccv_data_sp_save_action", lst);
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

        public SqlDataReader GetDataUpdatesForAction(int actionId, string dataType)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@ActionId", actionId));
            lst.Add(new SqlParameter("@DataType", dataType));
            
            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_data_sp_get_updatesForAction", lst);
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

        public void SaveDataActionProcessed(Guid dataUpdateGuid, int actionId)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@DataUpdateGuid", dataUpdateGuid));
            lst.Add(new SqlParameter("@ActionId", actionId));

            try
            {
                this.ExecuteNonQuery("cust_ccv_data_sp_save_action_processed", lst);
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


