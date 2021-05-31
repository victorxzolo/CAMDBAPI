using IBM.Data.DB2.iSeries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CampaignAPI.Models
{
    public class DB2Context
    {
        static string ConnectionString = ConfigurationManager.ConnectionStrings["IbmIConnectionString"].ConnectionString;
        static string sqlConnect = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;


        public DataTable GetDataTableSQL(string queryString)
        {
            try
            {
                string connect = sqlConnect;

                using (SqlConnection connection = new SqlConnection(connect))
                {
                    //String cmd = sql;

                    SqlCommand cmd = new SqlCommand(queryString, connection);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];

                    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                    foreach (DataColumn column in dt.Columns)
                        column.ColumnName = ti.ToTitleCase(column.ColumnName).ToLower();

                    DataColumn[] stringColumns = dt.Columns.Cast<DataColumn>()
                       .Where(c => c.DataType == typeof(string))
                       .ToArray();

                    foreach (DataRow row in dt.Rows)
                        foreach (DataColumn col in stringColumns)
                            row.SetField<string>(col, row.Field<string>(col).Trim());
                    connection.Close();

                    return dt;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetDataTableAS400(string sql)
        {
            try
            {
                string connect = ConnectionString;

                using (iDB2Connection connection = new iDB2Connection(connect))
                {
                    //String cmd = sql;
                    
                        connection.Open();
                        iDB2Command cmd = connection.CreateCommand();
                        cmd.CommandText = sql;
                        //adapter.SelectCommand = new iDB2Command(cmd, connection);
                        iDB2DataAdapter da = new iDB2DataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        DataTable dt = ds.Tables[0];

                        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                        foreach (DataColumn column in dt.Columns)
                        column.ColumnName = ti.ToTitleCase(column.ColumnName).ToLower();

                        DataColumn[] stringColumns = dt.Columns.Cast<DataColumn>()
                           .Where(c => c.DataType == typeof(string))
                           .ToArray();

                        foreach (DataRow row in dt.Rows)
                        foreach (DataColumn col in stringColumns)
                        row.SetField<string>(col, row.Field<string>(col).Trim());
                        connection.Close();

                        return dt; 
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string ExecuteSqlStr(string pStrSql)
        {
            string connect = ConnectionString;

            using (iDB2Connection Con = new iDB2Connection(connect))
            {
                if (Con.State == ConnectionState.Closed) Con.Open();
                string ReturnValue;
                if (Con.State == ConnectionState.Closed)
                {
                    Con.Open();
                }
                iDB2Command Cmd = new iDB2Command(pStrSql, Con);
                ReturnValue = Convert.ToString(Cmd.ExecuteScalar());
                Cmd.Dispose();
                if (ReturnValue == null)
                {
                    ReturnValue = "";
                }
                Con.Close();
                Con.Dispose();
                return ReturnValue;
            };
                 
        }
        public string ConvertDataTabletoString(DataTable dt)
        {

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);

        }
        
    }
}