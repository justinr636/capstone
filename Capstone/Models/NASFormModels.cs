using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Capstone.App_Start;
using System.Data;
using System.Globalization;

namespace Capstone.Models
{
    public class NASFormModels
    {
        #region Properties
        public int nas_form_id { get; set; }
        private float? _birth_weight;
        public float? birth_weight
        {
            get
            {
                //DataTable dt = Select("birth_weight");

                //if (dt.Rows.Count > 0)
                //    //return (float)dt.Rows[0][0];
                //    return float.Parse(dt.Rows[0][0].ToString(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                //else
                //    return 0;
                return _birth_weight;
            }
            set
            {
                // Perform Validation Checks?
                _birth_weight = value;
                Update("birth_weight", value);
            }
        }
        private float? _gest_age;
        public float? gest_age
        {
            get
            {
                //DataTable dt = Select("gest_age");

                //if (dt.Rows.Count > 0)
                //    return (float)dt.Rows[0][0];
                //else
                //    return 0;
                return _gest_age;
            }
            set
            {
                // Perform Validation Checks
                _gest_age = value;
                Update("gest_age", value);
            }
        }
        private int? _gest_age_weeks;
        public int? gest_age_weeks
        {
            get
            {
                //DataTable dt = Select("gest_age_weeks");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _gest_age_weeks;
            }
            set
            {
                _gest_age_weeks = value;
                Update("gest_age_weeks", value);
            }
        }
        private int? _gest_age_days;
        public int? gest_age_days
        {
            get
            {
                //DataTable dt = Select("gest_age_days");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _gest_age_days;
            }
            set
            {
                _gest_age_days = value;
                Update("gest_age_days", value);
            }
        }
        private DateTime? _birth_date;
        public DateTime? birth_date
        {
            get
            {
                //DataTable dt = Select("birth_date");

                //if (dt.Rows.Count > 0)
                //    return (DateTime)dt.Rows[0][0];
                //else
                //    return DateTime.MinValue;
                return _birth_date;
            }
            set
            {
                _birth_date = value;
                UpdateDate("birth_date", value);
            }
        }
        private int? _birth_location;
        public int? birth_location
        {
            get
            {
                //DataTable dt = Select("birth_location");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _birth_location;
            }
            set
            {
                _birth_location = value;
                Update("birth_location", value);
            }
        }
        private int? _outborn_days;
        public int? outborn_days
        {
            get
            {
                //DataTable dt = Select("outborn_days");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _outborn_days;
            }
            set
            {
                _outborn_days = value;
                Update("outborn_days", value);
            }
        }
        private int? _ant_consult;
        public int? ant_consult
        {
            get
            {
                //DataTable dt = Select("ant_consult");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _ant_consult;
            }
            set
            {
                _ant_consult = value;
                Update("ant_consult", value);
            }
        }
        private int? _mother_tox;
        public int? mother_tox
        {
            get
            {
                //DataTable dt = Select("mother_tox");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _mother_tox;
            }
            set
            {
                _mother_tox = value;
                Update("mother_tox", value);
            }
        }
        private bool? _mother_tox_res;
        public bool? mother_tox_res
        {
            get
            {
                //DataTable dt = Select("mother_tox_res");

                //if (dt.Rows.Count > 0)
                //    return SetBool(dt.Rows[0][0]);
                //else
                //    return false;
                return _mother_tox_res;
            }
            set
            {
                _mother_tox_res = value;
                Update("mother_tox_res", value);
            }
        }
        private int? _infant_tox;
        public int? infant_tox
        {
            get
            {
                //DataTable dt = Select("infant_tox");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _infant_tox;
            }
            set
            {
                _infant_tox = value;
                Update("infant_tox", value);
            }
        }
        private bool? _infant_tox_res;
        public bool? infant_tox_res
        {
            get
            {
                //DataTable dt = Select("infant_tox_res");

                //if (dt.Rows.Count > 0)
                //    return SetBool(dt.Rows[0][0]);
                //else
                //    return false;
                return _infant_tox_res;
            }
            set
            {
                _infant_tox_res = value;
                Update("infant_tox_res", value);
            }
        }
        private bool? _nas_bool;
        public bool? nas_bool
        {
            get
            {
                //DataTable dt = Select("nas_bool");

                //if (dt.Rows.Count > 0)
                //    return SetBool(dt.Rows[0][0]);
                //else
                //    return false;
                return _nas_bool;
            }
            set
            {
                _nas_bool = value;
                Update("nas_bool", value);
            }
        }
        private int? _nas_score;
        public int? nas_score
        {
            get
            {
                //DataTable dt = Select("nas_score");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _nas_score;
            }
            set
            {
                _nas_score = value;
                Update("nas_score", value);
            }
        }
        private bool? _infant_pharm_bool;           
        public bool? infant_pharm_bool
        {
            get
            {
                //DataTable dt = Select("infant_phram_bool");

                //if (dt.Rows.Count > 0)
                //    return SetBool(dt.Rows[0][0]);
                //else
                //    return false;
                return _infant_pharm_bool;
            }
            set
            {
                _infant_pharm_bool = value;
                Update("infant_pharm_bool", value);
            }
        }
        private int? _pharm_length;
        public int? pharm_length
        {
            get
            {
                //DataTable dt = Select("pharm_length");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _pharm_length;
            }
            set
            {
                _pharm_length = value;
                Update("pharm_length", value);
            }
        }
        private int? _pharm_interval; 
        public int? pharm_interval 
        { 
            get
            {
                //DataTable dt = Select("pharm_interval");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _pharm_interval;
            }
            set
            {
                _pharm_interval = value;
                Update("pharm_interval", value);
            }
        }
        private bool? _infant_meds_bool;
        public bool? infant_meds_bool 
        { 
            get
            {
                //DataTable dt = Select("infant_meds_bool");

                //if (dt.Rows.Count > 0)
                //    return SetBool(dt.Rows[0][0]);
                //else
                //    return false;
                return _infant_meds_bool;
            }
            set
            {
                _infant_meds_bool = value;
                Update("infant_meds_bool", value);
            }
        }
        private string _infant_other_meds;
        public string infant_other_meds 
        { 
            get
            {
                //DataTable dt = Select("infant_other_meds");

                //if (dt.Rows.Count > 0)
                //    return dt.Rows[0][0].ToString();
                //else
                //    return "";
                return _infant_other_meds;
            }
            set
            {
                _infant_other_meds = value;
                Update("infant_other_meds", value);
            }
        }
        private int? _milk_hosp;
        public int? milk_hosp 
        { 
            get
            {
                //DataTable dt = Select("milk_hosp");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _milk_hosp;
            }
            set
            {
                _milk_hosp = value;
                Update("milk_hosp", value);
            }
        }
        private int? _milk_dis;
        public int? milk_dis 
        { 
            get
            {
                //DataTable dt = Select("milk_dis");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _milk_dis;
            }
            set
            {
                _milk_dis = value;
                Update("milk_dis", value);
            }
        }
        private int? _formula_hosp;
        public int? formula_hosp 
        { 
            get
            {
                //DataTable dt = Select("formula_hosp");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _formula_hosp;
            }
            set
            {
                _formula_hosp = value;
                Update("formula_hosp", value);
            }
        }
        private int? _total_los;
        public int? total_los 
        { 
            get
            {
                //DataTable dt = Select("total_los");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _total_los;
            }
            set
            {
                _total_los = value;
                Update("total_los", value);
            }
        }
        private int? _scn_los;
        public int? scn_los 
        { 
            get
            {
                //DataTable dt = Select("scn_los");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _scn_los;
            }
            set
            {
                _scn_los = value;
                Update("scn_los", value);
            }
        }
        private int? _nicu_los;
        public int? nicu_los 
        { 
            get
            {
                //DataTable dt = Select("nicu_los");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _nicu_los;
            }
            set
            {
                _nicu_los = value;
                Update("nicu_los", value);
            }
        }
        private int? _nur_los;
        public int? nur_los 
        { 
            get
            {
                //DataTable dt = Select("nur_los");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _nur_los;
            }
            set
            {
                _nur_los = value;
                Update("nur_los", value);
            }
        }
        private int? _ped_los;
        public int? ped_los 
        { 
            get
            {
                //DataTable dt = Select("ped_los");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _ped_los;
            }
            set
            {
                _ped_los = value;
                Update("ped_los", value);
            }
        }
        private int? _early_int; 
        public int? early_int 
        { 
            get
            {
                //DataTable dt = Select("early_int");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _early_int;
            }
            set
            {
                _early_int = value;
                Update("early_int", value);
            }
        }
        private int? _fiveone_a; 
        public int? fiveone_a 
        { 
            get
            {
                //DataTable dt = Select("fiveone_a");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _fiveone_a;
            }
            set
            {
                _fiveone_a = value;
                Update("fiveone_a", value);
            }
        }
        private int? _fiveone_a_res; 
        public int? fiveone_a_res 
        { 
            get
            {
                //DataTable dt = Select("fiveone_a_res");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _fiveone_a_res;
            }
            set
            {
                _fiveone_a_res = value;
                Update("fiveone_a_res", value);
            }
        }
        private int? _dis_location;
        public int? dis_location 
        { 
            get
            {
                //DataTable dt = Select("dis_location");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _dis_location;
            }
            set
            {
                _dis_location = value;
                Update("dis_location", value);
            }
        }
        private bool? _transfer_bool;
        public bool? transfer_bool 
        { 
            get
            {
                //DataTable dt = Select("transfer_bool");

                //if (dt.Rows.Count > 0)
                //    return SetBool(dt.Rows[0][0]);
                //else
                //    return false;
                return _transfer_bool;
            }
            set
            {
                _transfer_bool = value;
                Update("transfer_bool", value);
            }
        }
        private int? _transfer_los; 
        public int? transfer_los 
        {
            get
            {
                //DataTable dt = Select("final_los");

                //if (dt.Rows.Count > 0)
                //    return SetInt(dt.Rows[0][0]);
                //else
                //    return 0;
                return _transfer_los;
            }
            set
            {
                _transfer_los = value;
                Update("transfer_los", value);
            }
        }
        private DateTime? _date_created;
        public DateTime? date_created
        {   
            get
            {
                return _date_created;
            }
            set
            {
                _date_created = value;
                UpdateDate("date_created", value);
            }
        }
        private int _hospital_id;
        public int hospital_id
        {
            get
            {
                return _hospital_id;
            }
            set
            {
                _hospital_id = value;
                Update("hospital_id", value);
            }
        }
        #endregion

        #region Constructors
        public NASFormModels()
        {
            nas_form_id = 0;
            hospital_id = 0;
            //birth_weight = 0;
        	//gest_age = 0;
        	//birth_date = DateTime.Now;
        	//infant_other_meds = "";
        	//gest_age_weeks = 0;
        	//gest_age_days = 0;
        	//birth_location = 0;
        	//outborn_days = 0;
        	//ant_consult = 0;
        	//mother_tox = 0;
        	//infant_tox = 0;
        	//nas_score = 0;
        	//pharm_length = 0;
        	//pharm_interval = 0;
        	//milk_hosp = 0;
        	//milk_dis = 0;
        	//formula_hosp = 0;
        	//final_los = 0;
        	//total_los = 0;
        	//scn_los = 0;
        	//nicu_los = 0;
        	//nur_los = 0;
        	//ped_los = 0;
        	//early_int = 0;
        	//fiveone_a = 0;
        	//fiveone_a_res = 0;
        	//dis_location = 0;
        	//transfer_bool = false;
        	//infant_meds_bool = false;
        	//infant_phram_bool = false;     // purposely mispelled "pharm" to match DB mistake
        	//mother_tox_res = false;
        	//infant_tox_res = false;
        	//nas_bool = false;
        }
        public NASFormModels(int nasID)
        {
            nas_form_id = nasID;

            DataTable dt = Select();    // fetch all data for NAS_FORM_ID

            if (dt.Rows.Count > 0)
            {
            	_birth_weight = float.Parse(dt.Rows[0]["birth_weight"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                // Currently gest_age is always null
        		//gest_age = float.Parse(dt.Rows[0]["gest_age"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                _birth_date = SetDate(dt, "birth_date");
                _infant_other_meds = dt.Rows[0]["infant_other_meds"].ToString(); 
        		_gest_age_weeks = SetInt(dt, "gest_age_weeks");
        		_gest_age_days = SetInt(dt, "gest_age_days");
        		_birth_location = SetInt(dt, "birth_location");
        		_outborn_days = SetInt(dt, "outborn_days");
        		_ant_consult = SetInt(dt, "ant_consult");
        		_mother_tox = SetInt(dt, "mother_tox");
        		_infant_tox = SetInt(dt, "infant_tox");
        		_nas_score = SetInt(dt, "nas_score");
        		_pharm_length = SetInt(dt, "pharm_length");
        		_pharm_interval = SetInt(dt, "pharm_interval");
        		_milk_hosp = SetInt(dt, "milk_hosp");
        		_milk_dis = SetInt(dt, "milk_dis");
        		_formula_hosp = SetInt(dt, "formula_hosp");
                _transfer_los = SetInt(dt, "transfer_los");
        		_total_los = SetInt(dt, "total_los");
        		_scn_los = SetInt(dt, "scn_los");
        		_nicu_los = SetInt(dt, "nicu_los");
        		_nur_los = SetInt(dt, "nur_los");
        		_ped_los = SetInt(dt, "ped_los");
        		_early_int = SetInt(dt, "early_int");
        		_fiveone_a = SetInt(dt, "fiveone_a");
        		_fiveone_a_res = SetInt(dt, "fiveone_a_res");
        		_dis_location = SetInt(dt, "dis_location");
        		_transfer_bool = SetBool(dt, "transfer_bool");
        		_infant_meds_bool = SetBool(dt, "infant_meds_bool");
        		_infant_pharm_bool = SetBool(dt, "infant_pharm_bool");     // purposely mispelled "pharm" to match DB mistake
        		_mother_tox_res = SetBool(dt, "mother_tox_res");
        		_infant_tox_res = SetBool(dt, "infant_tox_res");
        		_nas_bool = SetBool(dt, "nas_bool");
                _date_created = SetDate(dt, "date_created");
                _hospital_id = Convert.ToInt32(dt.Rows[0]["hospital_id"]);
            }
            else
            {
                // Spit out Error because no results were found.
            }
        }
        #endregion

        #region Methods
        public DataTable Select()
        {
            DataTable dt = new DataTable();

            string qry = "SELECT * FROM nas_form WHERE nas_form_id = @ID";

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
        private DataTable Select(string colName)
        {
            DataTable dt = new DataTable();

            string qry = string.Format("SELECT {0} FROM nas_form WHERE nas_form_id = @ID", colName);

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
        private void UpdateDate(string colName, DateTime? val)
        {
            if (val != null && val.HasValue)
            {
                string dateStr = val.Value.Year.ToString() + "-" + val.Value.Month.ToString() + "-" + val.Value.Day.ToString();

                string qry = string.Format("UPDATE nas_form SET {0} = @dateStr WHERE nas_form_id = @ID", colName);

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@dateStr", dateStr);
                        cmd.Parameters.AddWithValue("@ID", nas_form_id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            else
            {
                string qry = string.Format("UPDATE nas_form SET {0} = NULL WHERE nas_form_id = @ID", colName);

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", nas_form_id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }
        private void Update(string colName, object val)
        {
            if (val != null)
            {
                string qry = string.Format("UPDATE nas_form SET {0} = @val WHERE nas_form_id = @ID", colName);

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@val", val);
                        cmd.Parameters.AddWithValue("@ID", nas_form_id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            else
            {
                string qry = string.Format("UPDATE nas_form SET {0} = NULL WHERE nas_form_id = @ID", colName);

                using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", nas_form_id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }
        private int? SetInt(object s)
        {
            bool isNull = false;

            if (s == DBNull.Value)
                isNull = true;

            if (isNull)
                return null;
            else
                return Convert.ToInt32(s);
        }
        private int? SetInt(DataTable d, string attribute)
        {
            bool isNull = false;

            if (d.Rows[0][attribute] == DBNull.Value)
                isNull = true;

            if (isNull)
                return null;
            else
                return Convert.ToInt32(d.Rows[0][attribute].ToString());
        }
        private bool? SetBool(object s)
        {
            bool isNull = false;

            if (s == DBNull.Value)
                isNull = true;

            if (isNull)
                return null;
            else
                return Convert.ToBoolean(s);
        }
        private bool? SetBool(DataTable d, string attribute)
        {
            bool isNull = false;

            if (d.Rows[0][attribute] == DBNull.Value)
                isNull = true;

            if (isNull)
                return null;
            else
                return Convert.ToBoolean(d.Rows[0][attribute].ToString());
        }
        private DateTime? SetDate(DataTable d, string attribute)
        {
            bool isNull = false;

            if (d.Rows[0][attribute] == DBNull.Value)
                isNull = true;

            if (isNull)
                return null;
            else
                return Convert.ToDateTime(d.Rows[0][attribute].ToString());
        }
        public void CreateNewEntry()
        {
            string qry = "INSERT INTO nas_form DEFAULT VALUES SELECT @@IDENTITY";

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    nas_form_id = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
        }
        #endregion

        #region Static Methods
        public static DataTable SelectAll()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(dbUtil.ConnectionString))
            {
                string qry = "SELECT * FROM nas_form ORDER BY nas_form_id";

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                }
            }

            return dt;
        }
        #endregion
    }
}