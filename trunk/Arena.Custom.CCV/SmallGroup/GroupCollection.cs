using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace Arena.Custom.CCV.SmallGroup
{
    public class GroupCollection : Arena.SmallGroup.GroupCollection
    {
        public void LoadByClusterType(int clusterTypeID)
        {
            SqlDataReader rdr = new Arena.Custom.CCV.DataLayer.SmallGroup.GroupData().GetGroupByClusterTypeID(clusterTypeID);
            while (rdr.Read())
                base.Add(new Arena.SmallGroup.Group(rdr));
            rdr.Close();
        }
    }
}
