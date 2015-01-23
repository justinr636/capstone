using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;

namespace Capstone.Models
{
    public class MotherSubsModels
    {
        #region Properties
        public int nas_form_id { get; set; }
        public List<int> sub_id { get; set; }
        public List<string> comment { get; set; }
        #endregion

        #region Constructors
        public MotherSubsModels()
        {
            nas_form_id = 0;

            sub_id = new List<int>();
            comment = new List<string>();

            sub_id.Add(0);
            comment.Add("");
        }
        public MotherSubsModels(int nas_id)
        {
            nas_form_id = nas_id;
            sub_id = new List<int>();
            comment = new List<string>();

            DataTable dt = Select();        // get all records associated with nas_form_id
            if (dt.Rows.Count < 1)
            {
                sub_id.Add(0);
                comment.Add("error finding stuff");
            }
            else
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    sub_id.Add(Convert.ToInt32(dtRow["sub_id"]));
                    comment.Add(dtRow["comment"].ToString());
                }
            }
        }
        public MotherSubsModels(int nid, List<int> sids, List<string> notes)
        {
            nas_form_id = nid;
            sub_id = new List<int>();
            comment = new List<string>();

            if (sids.Count == notes.Count)
            {
                int numItems = sids.Count;

                for (int i = 0; i < numItems; i++)
                    AddSubstance(sids[i], notes[i]);
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM mother_subs WHERE nas_form_id = @ID";
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
        public void AddSubstance(int subid, string note)
        {
            sub_id.Add(subid);
            comment.Add(note);

            if(note == "")
            {
                string qry = "INSERT INTO mother_subs (nas_form_id, sub_id) VALUES ( @nasid , @subid )";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@nasid", nas_form_id);
                        cmd.Parameters.AddWithValue("@subid", subid);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            else 
            {
                string qry = "INSERT INTO mother_subs (nas_form_id, sub_id, comment) VALUES ( @nasid , @subid, @note )";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@nasid", nas_form_id);
                        cmd.Parameters.AddWithValue("@subid", subid);
                        cmd.Parameters.AddWithValue("@note", note);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }
        public void UpdateComment(int subName, string comm)
        {   // WARNING: This updates all comments. Unspecifc to ID
            string qry = "UPDATE mother_subs SET comment = @comm";

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

            string qry = "SELECT * FROM mother_subs";

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
            string qry = "DELETE FROM mother_subs WHERE nas_form_id = @id";

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