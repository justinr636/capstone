using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;

namespace Capstone.Models
{
    public class HospLocationsModels
    {
        #region Properties
        public int hosp_ward_id { get; set; }
        public string hosp_ward_name { get; set; }
        #endregion

        #region Constructors
        public HospLocationsModels()
        {
            hosp_ward_id = 0;
            hosp_ward_name = "";
        }
        public HospLocationsModels(int id)
        {
            hosp_ward_id = id;

            DataTable dt = Select();

            if (dt.Rows.Count == 1)
                hosp_ward_name = (string)dt.Rows[0]["hosp_ward_name"];

        }
        public HospLocationsModels(string name)
        {
            // Check string length before adding
            if (name.Length <= 50)
            {
                string qry = "INSERT INTO hosp_locations (hosp_ward_name) VALUES (@name) SELECT @@IDENTITY";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        conn.Open();
                        //hosp_ward_id = (int)cmd.ExecuteScalar();
                        hosp_ward_id = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                    }

                    hosp_ward_name = name;
                }
            }
            else
            {
                hosp_ward_id = 0;
                hosp_ward_name = "error";
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM hosp_locations WHERE hosp_ward_id = @ID";
            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", hosp_ward_id);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }
        private void Update(string colName, object val)
        {
            string qry = string.Format("UPDATE hosp_locations SET {0} = @val WHERE hosp_ward_id = @ID", colName);

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@val", val);
                    cmd.Parameters.AddWithValue("@ID", hosp_ward_id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        #endregion

        #region Static Methods
        public static DataTable SelectAll()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM hosp_locations";

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                    dt.Load(cmd.ExecuteReader());
                conn.Close();
            }

            return dt;
        }
        #endregion
    }
}