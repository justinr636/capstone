using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;

namespace Capstone.Models
{
    public class InfantLocationsModels
    {
        #region Properties
        public int nas_form_id { get; set; }
        public List<int> hosp_ward_id { get; set; }
        public List<string> comment { get; set; }
        #endregion

        #region Constructors
        public InfantLocationsModels()
        {
            nas_form_id = 0;
            hosp_ward_id = new List<int>();
            comment = new List<string>();

            hosp_ward_id.Add(0);
            comment.Add("");
        }
        public InfantLocationsModels(int nas_id)
        {
            nas_form_id = nas_id;
            hosp_ward_id = new List<int>();
            comment = new List<string>();

            DataTable dt = Select();        // get all records associated with nas_form_id
            if (dt.Rows.Count < 1)
            {
                hosp_ward_id.Add(0);
                comment.Add("error finding stuff");
            }
            else
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    hosp_ward_id.Add(Convert.ToInt32(dtRow["hosp_ward_id"].ToString()));
                    comment.Add(dtRow["comment"].ToString());
                }
            }
        }
        public InfantLocationsModels(int nid, List<int> wids, List<string> notes)
        {
            nas_form_id = nid;
            hosp_ward_id = new List<int>();
            comment = new List<string>();

            if (wids.Count == notes.Count)
            {
                int numItems = wids.Count;

                for (int i = 0; i < numItems; i++)
                    AddLocation(wids[i], notes[i]);
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM infant_locations WHERE nas_form_id = @ID";
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
        public void AddLocation(int wardid, string note)
        {
            hosp_ward_id.Add(wardid);
            comment.Add(note);

            if(note == "")
            {
                string qry = "INSERT INTO infant_locations (nas_form_id, hosp_ward_id) VALUES ( @nasid , @wardid )";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@nasid", nas_form_id);
                        cmd.Parameters.AddWithValue("@wardid", wardid);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            else 
            {
                string qry = "INSERT INTO infant_locations (nas_form_id, hosp_ward_id, comment) VALUES ( @nasid , @wardid, @note )";

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@nasid", nas_form_id);
                        cmd.Parameters.AddWithValue("@wardid", wardid);
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
            string qry = "UPDATE infant_locations SET comment = @comm";

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

            string qry = "SELECT * FROM infant_locations";

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
            string qry = "DELETE FROM infant_locations WHERE nas_form_id = @id";

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