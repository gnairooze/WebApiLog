using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebLog
{
    public class Log
    {
        public void Save(WebLogModel logModel)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebLogDB"].ConnectionString);

            SqlCommand cmd = new SqlCommand("WebLog_Add", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.AddWithValue("@ID", logModel.ID);
                cmd.Parameters["@ID"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@RequestContent", logModel.RequestContentText);
                cmd.Parameters.AddWithValue("@RequestHeaders", logModel.RequestHeadersText);
                cmd.Parameters.AddWithValue("@RequestMethod", logModel.RequestMethodText);
                cmd.Parameters.AddWithValue("@RequestTime", logModel.RequestTime);
                cmd.Parameters.AddWithValue("@Scheme", logModel.SchemeText);
                cmd.Parameters.AddWithValue("@Site", logModel.SiteText);
                cmd.Parameters.AddWithValue("@Path", logModel.PathText);
                cmd.Parameters.AddWithValue("@Query", logModel.QueryText);
                cmd.Parameters.AddWithValue("@RequestClientIP", logModel.RequestClientIP);
                cmd.Parameters.AddWithValue("@RequestHost", logModel.RequestHost);
                cmd.Parameters.AddWithValue("@Uri", logModel.RequestUriText);
                cmd.Parameters.AddWithValue("@RequestVersion", logModel.RequestVersionText);
                cmd.Parameters.AddWithValue("@ResponseContent", logModel.ResponseContentText);
                cmd.Parameters.AddWithValue("@ResponseHeaders", logModel.ResponseHeadersText);
                cmd.Parameters.AddWithValue("@StatusCode", logModel.ResponseStatusCodeText);
                cmd.Parameters.AddWithValue("@ResponseTime", logModel.ResponseTime);
                cmd.Parameters.AddWithValue("@ResponseVersion", logModel.ResponseVersionText);
                cmd.Parameters.AddWithValue("@Queries", getQueries(logModel.QueryText));
                cmd.Parameters["@Queries"].SqlDbType = SqlDbType.Structured;

                conn.Open();

                cmd.ExecuteNonQuery();

                logModel.ID = long.Parse(cmd.Parameters["@ID"].Value.ToString());
            }
            finally
            {
                conn.Close();

                conn.Dispose();

                conn = null;

                cmd.Dispose();
                cmd = null;
            }
        }

        private DataTable getQueries(string queryText)
        {
            if (string.IsNullOrEmpty(queryText))
            {
                return null;
            }

            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name");
            tbl.Columns.Add("Value");
            string query = string.Empty;

            if (queryText.StartsWith("?"))
            {
                query = queryText.Substring(1);
            }
            else
            {
                query = String.Copy(queryText);
            }

            string[] queries = query.Split("&".ToCharArray());
            foreach (var item in queries)
            {
                DataRow row = tbl.NewRow();
                row["Name"] = item.Split("=".ToCharArray())[0];
                row["Value"] = item.Split("=".ToCharArray())[1];

                tbl.Rows.Add(row);
            }

            return tbl;
        }
    }
}