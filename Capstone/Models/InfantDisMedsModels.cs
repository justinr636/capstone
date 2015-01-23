using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;

namespace Capstone.Models
{
    public class InfantDisMedsModels
    {
        #region Properties
        public int nas_form_id { get; set; }
        public List<int> sub_id { get; set; }
        public List<string> comment { get; set; }
        #endregion

        #region Constructors
        public InfantDisMedsModels()
        {
            nas_form_id = 0;
            sub_id = new List<int>();
            comment = new List<string>();

            sub_id.Add(0);
            comment.Add("");
        }
        public InfantDisMedsModels(int nas_id)
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
                    sub_id.Add(Convert.ToInt32(dtRow["sub_id"].ToString()));
                    comment.Add(dtRow["comment"].ToString());
                }
            }
        }
        public InfantDisMedsModels(int nid, List<int> sids, List<string> notes)
        {
            nas_form_id = nid;

            sub_id = new List<int>();
            comment = new List<string>();

            if (sids.Count == notes.Count)
            {
                int numItems = sids.Count;

            /* UNSAFE ------ NOT PARAMETERIZED ------ NOT SANITIZING comment inputs */
                //string qry = "INSERT INTO infant_dis_meds (nas_form_id, sub_id, comment) VALUES ";
                //for (int i = 0; i < numItems - 1; i++)
                //    qry += string.Format(" ( {0} , {1} , {2} ),", nas_form_id, sids[i], notes[i]);

                //qry += string.Format(" ( {0} , {1} , {2} )", nas_form_id, sids[numItems], notes[numItems]);

                //using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                //{
                //    using (SqlCommand cmd = new SqlCommand(qry, conn))
                //    {
                //        conn.Open();
                //        cmd.ExecuteNonQuery();
                //        conn.Close();
                //    }
                //}

                for (int i = 0; i < numItems; i++)
                {
                    AddSubstance(sids[i], notes[i]);
                /* REDUNDANT CODE */
                    //if (notes[i] == "")
                    //{
                    //    string qry = "INSERT INTO infant_dis_meds (nas_form_id, sub_id) VALUES (@nid, @sids)";

                    //    using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                    //    {
                    //        using (SqlCommand cmd = new SqlCommand(qry, conn))
                    //        {
                    //            cmd.Parameters.AddWithValue("@nid", nid);
                    //            cmd.Parameters.AddWithValue("@sids", sids[i]);
                    //            conn.Open();
                    //            cmd.ExecuteNonQuery();
                    //            conn.Close();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    string qry = "INSERT INTO infant_dis_meds (nas_form_id, sub_id, comment) VALUES (@nid, @sids, @notes)";

                    //    using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                    //    {
                    //        using (SqlCommand cmd = new SqlCommand(qry, conn))
                    //        {
                    //            cmd.Parameters.AddWithValue("@nid", nid);
                    //            cmd.Parameters.AddWithValue("@sids", sids[i]);
                    //            cmd.Parameters.AddWithValue("@notes", notes);
                    //            conn.Open();
                    //            cmd.ExecuteNonQuery();
                    //            conn.Close();
                    //        }
                    //    }
                    //}

                    //sub_id.Add(sids[i]);
                    //comment.Add(notes[i]);
                }
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM infant_dis_meds WHERE nas_form_id = @ID";
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
                string qry = "INSERT INTO infant_dis_meds (nas_form_id, sub_id) VALUES ( @nasid , @subid )";

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
                string qry = "INSERT INTO infant_dis_meds (nas_form_id, sub_id, comment) VALUES ( @nasid , @subid, @note )";

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
        {
            string qry = "UPDATE infant_dis_meds SET comment = @comm";

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

            string qry = "SELECT * FROM infant_dis_meds";

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
            string qry = "DELETE FROM infant_dis_meds WHERE nas_form_id = @id";

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