using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace Capstone.App_Start
{
    public class dbUtil
    {
        public static string ConnectionString = "Data Source=J-PC\\SQLEXPRESS;Initial Catalog=NAS;Integrated Security=True";

        public static int GetUserID(string uname)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string qry = "SELECT UserId FROM UserProfile WHERE UserName = @uname";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@uname", uname);
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0][0]);
            else
                return 0;
        }
    }
}