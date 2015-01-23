using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;

namespace Capstone.Models
{
    public class FormulasModels
    {
        #region Properties
        public int formula_id { get; set; }
        public string formula_name { get; set; }
        #endregion

        #region Constructors
        public FormulasModels()
        {
            formula_id = 0;
            formula_name = "";
        }
        public FormulasModels(int id)
        {
            formula_id = id;

            DataTable dt = Select();

            if (dt.Rows.Count == 1)
                formula_name = (string)dt.Rows[0]["formula_name"];

        }
        public FormulasModels(string name)
        {
            // Check string length before adding
            if (name.Length <= 100)
            {
                string qry = "INSERT INTO formulas (formula_name) VALUES (@name) SELECT @@IDENTITY";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        conn.Open();
                        //formula_id = (int)cmd.ExecuteScalar();
                        formula_id = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                    }

                    formula_name = name;
                }
            }
            else
            {
                formula_id = 0;
                formula_name = "error";
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM formulas WHERE formula_id = @ID";
            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", formula_id);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }
        private void Update(string colName, object val)
        {
            string qry = string.Format("UPDATE formulas SET {0} = @val WHERE formula_id = @ID", colName);

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@val", val);
                    cmd.Parameters.AddWithValue("@ID", formula_id);
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

            string qry = "SELECT * FROM formulas";

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