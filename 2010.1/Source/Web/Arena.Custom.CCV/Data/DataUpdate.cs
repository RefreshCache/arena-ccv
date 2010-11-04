
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;

namespace Arena.Custom.CCV.Data
{
    /// <summary>
    /// Summary description for AssignmentTrigger.
    /// </summary>
    [Serializable]
    public class DataUpdate
    {
        #region Public Properties

        public Guid DataUpdateGuid { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string DataType { get; set; }
        public DataTable DataBefore { get; set; }
        public DataTable DataAfter { get; set; }

        #endregion

        #region Constructors

        public DataUpdate()
        {
        }

        public DataUpdate(SqlDataReader reader)
        {
            DataUpdateGuid = (Guid)reader["data_update_guid"];
            UpdateDateTime = (DateTime)reader["update_datetime"];
            DataType = reader["data_type"].ToString();

            DataSet DataBeforeDS = new DataSet();
            if (reader[3] != DBNull.Value)
                DataBeforeDS.ReadXml(reader.GetSqlXml(3).CreateReader());
            if (DataBeforeDS.Tables.Count == 1)
                DataBefore = DataBeforeDS.Tables[0];

            DataSet DataAfterDS = new DataSet();
            if (reader[4] != DBNull.Value)
            DataAfterDS.ReadXml(reader.GetSqlXml(4).CreateReader());
            if (DataAfterDS.Tables.Count == 1)
                DataAfter = DataAfterDS.Tables[0];
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class MonitorDataUpdate : System.Attribute
    {
        private string _DataType = string.Empty;

        public string DataType { get { return _DataType; } }

        public MonitorDataUpdate(string DataType)
        {
            _DataType = DataType;
        }
    }

}





