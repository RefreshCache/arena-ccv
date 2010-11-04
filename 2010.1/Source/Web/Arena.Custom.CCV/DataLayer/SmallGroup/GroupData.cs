using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

using Arena.DataLib;

namespace Arena.Custom.CCV.DataLayer.SmallGroup
{
    public class GroupData : SqlData
    {
        /// <summary>
        /// Inserts a record into the the Group Member Log table
        /// </summary>
        public void SaveGroupMemberLog(int personID, int clusterTypeID, int groupID, 
            string groupName, int leaderPersonID, int role)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@PersonID", personID));
            lst.Add(new SqlParameter("@ClusterTypeID", Arena.DataLayer.FKHelper.SaveForeignKey(clusterTypeID)));
            lst.Add(new SqlParameter("@GroupID", Arena.DataLayer.FKHelper.SaveForeignKey(groupID)));
            lst.Add(new SqlParameter("@GroupName", groupName));
            lst.Add(new SqlParameter("@LeaderPersonID", Arena.DataLayer.FKHelper.SaveForeignKey(leaderPersonID)));
            lst.Add(new SqlParameter("@Role",  Arena.DataLayer.FKHelper.SaveForeignKey(role)));

            try
            {
                this.ExecuteNonQuery("cust_ccv_smgp_sp_save_member_log", lst);
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
        /// containing all the active groups of a particular Cluster Type
        /// </summary>
        public SqlDataReader GetGroupByClusterTypeID(int clusterTypeID)
        {
            ArrayList lst = new ArrayList();

            lst.Add(new SqlParameter("@ClusterTypeID", clusterTypeID));

            try
            {
                return this.ExecuteSqlDataReader("cust_ccv_smgp_sp_get_groupByClusterTypeID", lst);
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
