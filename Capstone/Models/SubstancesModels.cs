using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;

namespace Capstone.Models
{
    public class SubstancesModels
    {
        #region Properties
        public int sub_id { get; set; }
        public string sub_name { get; set; }
        #endregion

        #region Constructors
        public SubstancesModels()
        {
            sub_id = 0;
            sub_name = "";
        }
        public SubstancesModels(int id)
        {
            sub_id = id;

            DataTable dt = Select();

            //using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            //{
            //    string qry = "SELECT * FROM substances WHERE sub_id = @ID";
            //    using (SqlCommand cmd = new SqlCommand(qry, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@ID", id);
            //        conn.Open();
            //        dt.Load(cmd.ExecuteReader());
            //        conn.Close();
            //    }
            //}

            if (dt.Rows.Count == 1)
                sub_name = (string)dt.Rows[0]["sub_name"];

        }
        public SubstancesModels(string name)
        {
            if (name.Length <= 50)
            {
                string qry = "INSERT INTO substances (sub_name) VALUES (@name) SELECT @@IDENTITY";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        conn.Open();
                        //sub_id = (int)cmd.ExecuteScalar();
                        sub_id = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                    }
                }

                sub_name = name;
            }
            else
            {
                sub_id = 0;
                sub_name = "error";
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM substances WHERE sub_id = @ID";
            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", sub_id);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }
        private void Update(string colName, object val)
        {
            string qry = string.Format("UPDATE substances SET {0} = @val WHERE sub_id = @ID", colName);

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@val", val);
                    cmd.Parameters.AddWithValue("@ID", sub_id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        #endregion

        #region Static Methods
        public static void newSubstance(string name)
        {
            // convert name to lowercase
            name = name.ToLower();
            // Add error checking to make sure substance name doesn't already exist?

            // Check string length before adding
            if (name.Length <= 50)
            {
                string qry = "INSERT INTO substances (sub_name) VALUES (@name)";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
        public static DataTable SelectAll()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM substances";

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