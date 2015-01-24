using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Capstone.App_Start;

namespace Capstone.Models
{
    public class ChartModels
    {
        public static DataTable BirthWeightVsNASScore()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry = "SELECT nas_form_id, birth_weight, nas_score FROM nas_form WHERE birth_weight IS NOT NULL AND nas_score IS NOT NULL";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        // Avg Length Of Stay vs Infant Pharmacology Agents (Infant Subs)
        public static DataTable AvgLosVsInfantSubs()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry =
                        "SELECT ipa.sub_id, s.sub_name, AVG(n.total_los) AS avg_los "
                      + "FROM nas_form n "
                      + "LEFT OUTER JOIN infant_pharm_agents ipa ON ipa.nas_form_id = n.nas_form_id "
                      + "LEFT OUTER JOIN substances s ON s.sub_id = ipa.sub_id "
                      + "WHERE ipa.sub_id IS NOT NULL "
                      + "GROUP BY ipa.sub_id, s.sub_name "
                      + "ORDER BY s.sub_name";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        // Avg Length Of Stay vs Infant Formulas
        public static DataTable AvgLosVsInfantFormulas()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry =
                        "SELECT ifs.formula_id, f.formula_name, AVG(n.total_los) AS avg_los "
                      + "FROM nas_form n "
                      + "LEFT OUTER JOIN infant_formulas ifs ON ifs.nas_form_id = n.nas_form_id "
                      + "LEFT OUTER JOIN formulas f ON f.formula_id = ifs.formula_id "
                      + "WHERE ifs.formula_id IS NOT NULL "
                      + "GROUP BY ifs.formula_id, f.formula_name "
                      + "ORDER BY f.formula_name";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        // Avg Length Of Stay vs Mother's Substances
        public static DataTable AvgLosVsMotherSubs()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry =
                        "SELECT ms.sub_id, s.sub_name, AVG(n.total_los) AS avg_los, COUNT(n.total_los) AS Num " +
                        "FROM nas_form n " +
                      	"LEFT OUTER JOIN mother_subs ms ON ms.nas_form_id = n.nas_form_id " +
                      	"LEFT OUTER JOIN substances s ON s.sub_id = ms.sub_id " +
                      	"WHERE ms.sub_id IS NOT NULL AND n.total_los IS NOT NULL " +
                      	"GROUP BY ms.sub_id, s.sub_name " +
                      	"ORDER BY s.sub_name";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        // Length Of Stay vs Mother's Substances
        public static DataTable LosVsMotherSubs()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry =
                      "SELECT n.nas_form_id, ms.sub_id, s.sub_name, n.total_los " +
                      "FROM nas_form n  " +
                      "LEFT OUTER JOIN mother_subs ms ON ms.nas_form_id = n.nas_form_id  " +
                      "LEFT OUTER JOIN substances s ON s.sub_id = ms.sub_id  " +
                      "WHERE ms.sub_id IS NOT NULL AND n.total_los IS NOT NULL " +
                      "ORDER BY s.sub_name ";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        public static DataTable IntVsInt(string col1, string col2)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry = string.Format("SELECT {0}, {1} FROM nas_form WHERE {0} IS NOT NULL AND {1} IS NOT NULL", col1, col2);

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }
    }
}