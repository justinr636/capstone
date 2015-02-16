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

        // Get Box Plot Data
        public static DataTable GetBoxPlotData(int x1, int x2, ref string title)
        {
            string qry = "";
            string col = "";

            DataTable dt = new DataTable();

            switch (x1)
            {
                case 0:     // Birth Weight
                    col = "birth_weight";
                    title = "Birth Weight Vs. ";
                    break;
                case 1:     // Length of Stay
                    col = "total_los";
                    title = "Length of Stay Vs. ";
                    break;
                case 2:     // Outborn Days
                    col = "outborn_days";
                    title = "Outborn Days Vs. ";
                    break;
                case 3:     // NAS Score
                    col = "nas_score";
                    title = "NAS Score Vs. ";
                    break;
                case 4:     // Total Pharm Length
                    col = "pharm_length";
                    title = "Total Pharmacology Length Vs. ";
                    break;
                case 5:     // Last Dose Interval
                    col = "pharm_interval";
                    title = "Last Pharmacology Dose Interval Vs. ";
                    break;
            }

            switch(x2)
            {
                case 0:     // Mother Subs
                    title += "Mother's Substances";
            	    qry = string.Format(
            	          "SELECT n.nas_form_id, ms.sub_id, s.sub_name, n.{0} " +
            	          "FROM nas_form n  " +
            	          "LEFT OUTER JOIN mother_subs ms ON ms.nas_form_id = n.nas_form_id  " +
            	          "LEFT OUTER JOIN substances s ON s.sub_id = ms.sub_id  " +
            	          "WHERE ms.sub_id IS NOT NULL AND n.{0} IS NOT NULL " +
            	          "ORDER BY s.sub_name",
                            col);
            	    break;
            	case 1:     // Infant Subs
                    title += "Infant's Substances";
            	    qry = string.Format(
            	          "SELECT n.nas_form_id, iss.sub_id, s.sub_name, n.{0} " +
            	          "FROM nas_form n  " +
            	          "LEFT OUTER JOIN infant_subs iss ON iss.nas_form_id = n.nas_form_id  " +
            	          "LEFT OUTER JOIN substances s ON s.sub_id = iss.sub_id  " +
            	          "WHERE iss.sub_id IS NOT NULL AND n.{0} IS NOT NULL " +
            	          "ORDER BY s.sub_name",
                             col);
                    break;
                case 2:     // Infant Discharge Meds
                    title += "Infant's Discharge Medications";
                    qry = string.Format(
                        "SELECT n.nas_form_id, idm.sub_id, s.sub_name, n.{0} " +
					    "FROM nas_form n " +
						"LEFT OUTER JOIN infant_dis_meds idm ON idm.nas_form_id = n.nas_form_id " +
						"LEFT OUTER JOIN substances s ON s.sub_id = idm.sub_id " +
						"WHERE idm.sub_id IS NOT NULL AND n.{0} IS NOT NULL " +
						"ORDER BY s.sub_name",
                           col);
                    break;
                case 3:     // Infant Pharmacology Agents 
                    title += "Infant's Pharmacology Agents";
                    qry = string.Format(
                        "SELECT n.nas_form_id, ipa.sub_id, s.sub_name, n.{0} " + 
					    "FROM nas_form n " + 
						"LEFT OUTER JOIN infant_pharm_agents ipa ON ipa.nas_form_id = n.nas_form_id " + 
						"LEFT OUTER JOIN substances s ON s.sub_id = ipa.sub_id " + 
						"WHERE ipa.sub_id IS NOT NULL AND n.{0} IS NOT NULL " + 
						"ORDER BY s.sub_name",
                           col);
                    break;
                case 4:     // Infant Formulas
                    title += "Infant's Formulas";
                    qry = string.Format(
                        "SELECT n.nas_form_id, ifm.formula_id, f.formula_name, n.{0} " +
					    "FROM nas_form n " +
						"LEFT OUTER JOIN infant_formulas ifm ON ifm.nas_form_id = n.nas_form_id " +
						"LEFT OUTER JOIN formulas f ON f.formula_id = ifm.formula_id " +
						"WHERE ifm.formula_id IS NOT NULL AND n.{0} IS NOT NULL " +
						"ORDER BY f.formula_name",
                           col);
                    break;
                case 5:     // Infant Locations
                    title += "Infant's Hospital Ward Locations";
                    qry = string.Format(
                        "SELECT n.nas_form_id, il.hosp_ward_id, h.hosp_ward_name, n.{0} " + 
					    "FROM nas_form n " + 
						"LEFT OUTER JOIN infant_locations il ON il.nas_form_id = n.nas_form_id " + 
						"LEFT OUTER JOIN hosp_locations h ON h.hosp_ward_id = il.hosp_ward_id " + 
						"WHERE il.hosp_ward_id IS NOT NULL AND n.{0} IS NOT NULL " + 
						"ORDER BY h.hosp_ward_name",
                          col);
                    break;
            }

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        public static DataTable RunChartVsTime(string col)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry =  string.Format("SELECT birth_date, {0} "
                            + "FROM nas_form "
                            + "WHERE birth_date IS NOT NULL AND {0} IS NOT NULL "
                            + "ORDER BY birth_date", col);

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        public static List<string> CreateXData_Date(DataTable dt, int index)
        {
            List<string> x_data = new List<string>();

            foreach (DataRow dtRow in dt.Rows)
            {
                DateTime dateT = Convert.ToDateTime(dtRow[index].ToString());
                string dateStr = dateT.Date.ToString("MM/dd/yyyy");

                x_data.Add(dateStr);
            }

            return x_data;
        }

        public static List<int> CreateYData_Int(DataTable dt, int index)
        {
            List<int> y_data = new List<int>();

            foreach (DataRow dtRow in dt.Rows)
                y_data.Add(Convert.ToInt32(dtRow[index]));

            return y_data;
        }

        public static List<double> CreateYData_Double(DataTable dt, int index)
        {
            List<double> y_data = new List<double>();

            foreach (DataRow dtRow in dt.Rows)
                y_data.Add(Convert.ToDouble(dtRow[index]));

            return y_data;
        }

        public static void FormatBoxPlotData_Int(DataTable dt, int keyIndex, int valIndex, ref List<string> keys, ref List<List<int>> vals, ref int min, ref int max)
        {
            string curKey = dt.Rows[0][keyIndex].ToString();
            keys.Add(curKey);

            List<int> curVals = new List<int>();

            foreach (DataRow dtRow in dt.Rows)
            {
                string key = dtRow[keyIndex].ToString();
                int val = Convert.ToInt32(dtRow[valIndex]);

                if (key == curKey)
                {
                    curVals.Add(val);

                    if (val > max)
                        max = val;

                    if (val < min)
                        min = val;
                }
                else
                {
                    curKey = key;
                    keys.Add(key);

                    vals.Add(new List<int>(curVals));
                    curVals.Clear();
                    curVals.Add(val);
                }
            }

            vals.Add(new List<int>(curVals));
        }

        public static void FormatBoxPlotData_Double(DataTable dt, int keyIndex, int valIndex, ref List<string> keys, ref List<List<double>> vals, ref double min, ref double max)
        {
            string curKey = dt.Rows[0][keyIndex].ToString();
            keys.Add(curKey);

            List<double> curVals = new List<double>();

            foreach (DataRow dtRow in dt.Rows)
            {
                string key = dtRow[keyIndex].ToString();
                int val = Convert.ToInt32(dtRow[valIndex]);

                if (key == curKey)
                {
                    curVals.Add(val);

                    if (val > max)
                        max = val;

                    if (val < min)
                        min = val;
                }
                else
                {
                    curKey = key;
                    keys.Add(key);

                    vals.Add(new List<double>(curVals));
                    curVals.Clear();
                    curVals.Add(val);
                }
            }

            vals.Add(new List<double>(curVals));
        }

        public static DataTable LosVsTime()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry =  "SELECT birth_date, total_los "
                            + "FROM nas_form "
                            + "WHERE birth_date IS NOT NULL AND total_los IS NOT NULL "
                            + "ORDER BY birth_date";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }

        public static DataTable PercentInfantPharm()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry = 
                    "SELECT birth_date, infant_pharm_bool  "
                    + "FROM nas_form "
					+ "WHERE birth_date IS NOT NULL "
					    + "AND infant_pharm_bool IS NOT NULL "
					+ "ORDER BY birth_date";

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