using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;

namespace Capstone.Models
{
    public class InfantFormulasModels
    {
        #region Properties
        public int nas_form_id { get; set; }
        public List<int> formula_id { get; set; }
        public List<string> comment { get; set; }
        #endregion

        #region Constructors
        public InfantFormulasModels()
        {
            nas_form_id = 0;
            formula_id = new List<int>();
            comment = new List<string>();

            formula_id.Add(0);
            comment.Add("");
        }
        public InfantFormulasModels(int nas_id)
        {
            nas_form_id = nas_id;
            formula_id = new List<int>();
            comment = new List<string>();

            DataTable dt = Select();        // get all records associated with nas_form_id
            if (dt.Rows.Count < 1)
            {
                formula_id.Add(0);
                comment.Add("error finding stuff");
            }
            else
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    formula_id.Add(Convert.ToInt32(dtRow["formula_id"].ToString()));
                    comment.Add(dtRow["comment"].ToString());
                }
            }
        }
        public InfantFormulasModels(int nid, List<int> fids, List<string> notes)
        {
            nas_form_id = nid;
            formula_id = new List<int>();
            comment = new List<string>();

            if (fids.Count == notes.Count)
            {
                int numItems = fids.Count;

                for (int i = 0; i < numItems; i++)
                    AddFormula(fids[i], notes[i]);
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM infant_formulas WHERE nas_form_id = @ID";
            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", nas_form_id);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }
        public void AddFormula(int formid, string note)
        {
            formula_id.Add(formid);
            comment.Add(note);

            if(note == "")
            {
                string qry = "INSERT INTO infant_formulas (nas_form_id, formula_id) VALUES ( @nasid , @formid )";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@nasid", nas_form_id);
                        cmd.Parameters.AddWithValue("@formid", formid);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            else 
            {
                string qry = "INSERT INTO infant_formulas (nas_form_id, formula_id, comment) VALUES ( @nasid , @formid, @note )";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@nasid", nas_form_id);
                        cmd.Parameters.AddWithValue("@formid", formid);
                        cmd.Parameters.AddWithValue("@note", note);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }
        public void UpdateComment(int subName, string comm)
        {
            string qry = "UPDATE infant_formulas SET comment = @comm";

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@comm", comm);
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

            string qry = "SELECT * FROM infant_formulas";

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                    dt.Load(cmd.ExecuteReader());
                conn.Close();
            }

            return dt;
        }
        public static void Delete(int id)
        {
            string qry = "DELETE FROM infant_formulas WHERE nas_form_id = @id";

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        #endregion

    }
}