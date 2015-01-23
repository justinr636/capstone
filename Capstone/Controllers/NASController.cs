using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using System.Web.Security;
using Capstone.App_Start;

namespace Capstone.Controllers
{
    [Authorize]
    public class NASController : Controller
    {
        //
        // GET: /NAS/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ViewAll()
        {
            ViewBag.NASData = NASFormModels.SelectAll();

            return View();
        }

        public ActionResult Edit(string id)
        {
            int ID = Convert.ToInt32(id);

            ViewBag.ID = ID;

            // OR
            //NASFormModels nfm = new NASFormModels(ID);
            //ViewBag.NASData = nfm;

            return View();
        }

        [HttpPost()]
        #region NewNASForm HTTP Post declaration
        public ActionResult NewNASForm(
            string birthWeightText,
            int? gestAgeWeeksText,
            int? gestAgeDaysText,
            int? birthDayDrop,
            int? birthMonthDrop,
            int? birthYearDrop,
            int? birthLocationRadio,
            int? birthLocationAgeText,
            int? consultRadio,
            int[] drugCheck,
            int? momToxRadio,
            bool? momToxResultsRadio,
            int? infantToxRadio,
            bool? infantToxResultsRadio,
            bool? nasRadio,
            int? nasScoreText,
            bool? infantDrugRadio,
            int[] infantDrugCheck,
            int? nasPharmTreatLengthText,
            int? nasIntervalText,
            bool? infantMedRadio,
            int[] infantMedCheck,
            bool? infantOtherMedRadio,
            string infantOtherMedText,
            int? momMilkDuringRadio,
            int? momMilkDischargeRadio,
            int? formulaRadio,
            int[] formulaTypeCheck,
            int[] hospLocationCheck,
            int? totalLengthOfStayText,
            int? scnLengthOfStayText,
            int? nicuLengthOfStayText,
            int? nurseryLengthOfStayText,
            int? pedLengthOfStayText,
            int? earlyInterventionRadio,
            int? fiveOneARadio,
            int? dcfRadio,
            int? dischargeRadio,
            bool? transferRadio,
            int? transferLengthText
            )
        #endregion
        #region NewNASForm HTTP Post function
        {
            NASFormModels nfm = new NASFormModels();

       // Refactor Code to Check variables before creating new entry!!!
            nfm.CreateNewEntry();

            string username = User.Identity.Name;
            int userId = dbUtil.GetUserID(username);

            nfm.hospital_id = userId;

            nfm.birth_weight = float.Parse(birthWeightText, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            nfm.gest_age_weeks = gestAgeWeeksText;
            nfm.gest_age_days = gestAgeDaysText;

            if (birthYearDrop != null && birthMonthDrop != null && birthDayDrop != null)
            {
                string bDayStr = birthYearDrop + "-" + birthMonthDrop + "-" + birthDayDrop + " 00:00:00";
                nfm.birth_date = DateTime.Parse(bDayStr);
            }

            nfm.birth_location = birthLocationRadio;
            if (birthLocationRadio == 2)
                nfm.outborn_days = birthLocationAgeText;

            nfm.ant_consult = consultRadio;

            // CHECK IF "none" (1) is checked. Then only add 1.
            // also use javascript to error check.....
            if (drugCheck == null)
            { }
            else if (drugCheck.Length != 0)
            {
                List<int> drugs = new List<int>();
                List<string> comments = new List<string>();

                foreach (int drug in drugCheck)
                {
                    drugs.Add(drug);
                    comments.Add("");
                }

                MotherSubsModels msm = new MotherSubsModels(nfm.nas_form_id, drugs, comments);
            }

            nfm.mother_tox = momToxRadio;
            if(momToxRadio == 1)
                nfm.mother_tox_res = momToxResultsRadio;

            nfm.infant_tox = infantToxRadio;
            if(infantToxRadio == 1)
                nfm.infant_tox_res = infantToxResultsRadio;

            nfm.nas_bool = nasRadio;
            if (nasRadio.HasValue)
            {
                if (nasRadio.Value)
                    nfm.nas_score = nasScoreText;
            }

            nfm.infant_pharm_bool = infantDrugRadio;
            if (infantDrugCheck == null || infantDrugRadio != true)
            { }
            else if (infantDrugCheck.Length != 0)
            {
                List<int> drugs = new List<int>();
                List<string> comments = new List<string>();

                foreach (int drug in infantDrugCheck)
                {
                    drugs.Add(drug);
                    comments.Add("");
                }

                InfantPharmAgentsModels ipam = new InfantPharmAgentsModels(nfm.nas_form_id, drugs, comments);
            }

            nfm.pharm_length = nasPharmTreatLengthText;
            nfm.pharm_interval = nasIntervalText;

            nfm.infant_meds_bool = infantMedRadio;
            if (infantMedCheck == null || infantMedRadio != true)
            { }
            else if (infantMedCheck.Length != 0)
            {
                List<int> meds = new List<int>();
                List<string> comments = new List<string>();

                foreach (int med in infantMedCheck)
                {
                    meds.Add(med);
                    comments.Add("");
                }

                InfantDisMedsModels idmm = new InfantDisMedsModels(nfm.nas_form_id, meds, comments);
            }

            if ((infantOtherMedRadio == true) && (infantOtherMedText.Length < 200))
                nfm.infant_other_meds = infantOtherMedText;
            else    // TEST if you can set as null like this
                nfm.infant_other_meds = null;

            nfm.milk_hosp = momMilkDuringRadio;
            nfm.milk_dis = momMilkDischargeRadio;

            nfm.formula_hosp = formulaRadio;
            if(formulaRadio == 1 && (formulaTypeCheck.Length != 0))
            {
                List<int> formulas = new List<int>();
                List<string> comments = new List<string>();

                foreach (int formula in formulaTypeCheck)
                {
                    formulas.Add(formula);
                    comments.Add("");
                }

                InfantFormulasModels idmm = new InfantFormulasModels(nfm.nas_form_id, formulas, comments);
            }

            if (hospLocationCheck == null)
            { }
            else if (hospLocationCheck.Length != 0)
            {
                List<int> locations = new List<int>();
                List<string> comments = new List<string>();

                foreach (int location in hospLocationCheck)
                {
                    locations.Add(location);
                    comments.Add("");
                }

                InfantLocationsModels ilm = new InfantLocationsModels(nfm.nas_form_id, locations, comments);
            }

            nfm.total_los = totalLengthOfStayText;
            nfm.scn_los = scnLengthOfStayText;
            nfm.nicu_los = nicuLengthOfStayText;
            nfm.nur_los = nurseryLengthOfStayText;
            nfm.ped_los = pedLengthOfStayText;

            nfm.early_int = earlyInterventionRadio;

            nfm.fiveone_a = fiveOneARadio;
            if (fiveOneARadio == 1)
                nfm.fiveone_a_res = dcfRadio;
            else
                nfm.fiveone_a_res = null;

            nfm.dis_location = dischargeRadio;

            nfm.transfer_bool = transferRadio;
            if (transferRadio == true)
                nfm.transfer_los = transferLengthText;

            return Redirect("/NAS/ViewAll/");
        }
        #endregion

        [HttpPost()]
        #region UpdateNASForm HTTP Post declaration
        public ActionResult UpdateNASForm(
            int ID,
            string birthWeightText,
            int? gestAgeWeeksText,
            int? gestAgeDaysText,
            int? birthDayDrop,
            int? birthMonthDrop,
            int? birthYearDrop,
            int? birthLocationRadio,
            int? birthLocationAgeText,
            int? consultRadio,
            int[] drugCheck,
            int? momToxRadio,
            bool? momToxResultsRadio,
            int? infantToxRadio,
            bool? infantToxResultsRadio,
            bool? nasRadio,
            int? nasScoreText,
            bool? infantDrugRadio,
            int[] infantDrugCheck,
            int? nasPharmTreatLengthText,
            int? nasIntervalText,
            bool? infantMedRadio,
            int[] infantMedCheck,
            bool? infantOtherMedRadio,
            string infantOtherMedText,
            int? momMilkDuringRadio,
            int? momMilkDischargeRadio,
            int? formulaRadio,
            int[] formulaTypeCheck,
            int[] hospLocationCheck,
            int? totalLengthOfStayText,
            int? scnLengthOfStayText,
            int? nicuLengthOfStayText,
            int? nurseryLengthOfStayText,
            int? pedLengthOfStayText,
            int? earlyInterventionRadio,
            int? fiveOneARadio,
            int? dcfRadio,
            int? dischargeRadio,
            bool? transferRadio,
            int? transferLengthText
            )
        #endregion
        #region UpdateNASForm HTTP Post function
        {
            // NEED TO SET *ID* from URL?
            NASFormModels nfm = new NASFormModels(ID);

            // Validate!!!!!!

            //  Difficult to compare floats due to precision.....
            //fltPlaceHolder = float.Parse(birthWeightText, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            nfm.birth_weight = float.Parse(birthWeightText, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            //nfm.gest_age = null;

            if(nfm.gest_age_days != gestAgeDaysText)
                nfm.gest_age_days = gestAgeDaysText;

            // Check how to compare DateTime when SQL only stores date
            if (birthYearDrop != null && birthMonthDrop != null && birthDayDrop != null)
            {
                string bDayStr = birthYearDrop + "-" + birthMonthDrop + "-" + birthDayDrop + " 00:00:00";
                nfm.birth_date = DateTime.Parse(bDayStr);
            }
            else
                nfm.birth_date = null;

            if(nfm.birth_location != birthLocationRadio)
                nfm.birth_location = birthLocationRadio;

            if (birthLocationRadio == 2)
            {
                if(nfm.outborn_days != birthLocationAgeText)
                    nfm.outborn_days = birthLocationAgeText;
            }
            else
                nfm.outborn_days = null;

            if(nfm.ant_consult != consultRadio)
                nfm.ant_consult = consultRadio;

            // CHECK IF "none" (1) is checked. Then only add 1.
            // also use javascript to error check.....
            if (drugCheck == null)
                MotherSubsModels.Delete(ID);
            else if (drugCheck.Length != 0)
            {
                List<int> drugs = new List<int>();
                List<string> comments = new List<string>();

                foreach (int drug in drugCheck)
                {
                    drugs.Add(drug);
                    comments.Add("");
                }

                MotherSubsModels oldSubs = new MotherSubsModels(ID);
                //bool subEqual = oldSubs.sub_id.SequenceEqual(drugs);
                bool subEqual = new HashSet<int>(oldSubs.sub_id).SetEquals(drugs);

                if (!subEqual)
                {
                    MotherSubsModels.Delete(ID);
                    MotherSubsModels msm = new MotherSubsModels(nfm.nas_form_id, drugs, comments);
                }
            }
            else
                MotherSubsModels.Delete(ID);

            if (nfm.mother_tox != momToxRadio)
                nfm.mother_tox = momToxRadio;

            if (momToxRadio == 1)
            {
                if (nfm.mother_tox_res != momToxResultsRadio)
                    nfm.mother_tox_res = momToxResultsRadio;
            }
            else
                nfm.mother_tox_res = null;

            if (nfm.infant_tox != infantToxRadio)
                nfm.infant_tox = infantToxRadio;

            if (infantToxRadio == 1)
            {
                if (nfm.infant_tox_res != infantToxResultsRadio)
                    nfm.infant_tox_res = infantToxResultsRadio;
            }
            else
                nfm.infant_tox_res = null;

            if(nfm.nas_bool != nasRadio)
                nfm.nas_bool = nasRadio;

            if (nasRadio == true)
            {
                if (nfm.nas_score != nasScoreText)
                    nfm.nas_score = nasScoreText;
            }
            else
                nfm.nas_score = null;


            if (nfm.infant_pharm_bool != infantDrugRadio)
                nfm.infant_pharm_bool = infantDrugRadio;

            if (infantDrugCheck == null || infantDrugRadio != true)
                InfantPharmAgentsModels.Delete(ID);
            else if (infantDrugCheck.Length != 0)
            {
                List<int> drugs = new List<int>();
                List<string> comments = new List<string>();

                foreach (int drug in infantDrugCheck)
                {
                    drugs.Add(drug);
                    comments.Add("");
                }

                InfantPharmAgentsModels oldPharmAgents = new InfantPharmAgentsModels(ID);
                //bool subEqual = oldSubs.sub_id.SequenceEqual(drugs);
                bool subEqual = new HashSet<int>(oldPharmAgents.sub_id).SetEquals(drugs);

                if (!subEqual)
                {
                    InfantPharmAgentsModels.Delete(ID);
                    InfantPharmAgentsModels ipam = new InfantPharmAgentsModels(nfm.nas_form_id, drugs, comments);
                }
            }
            else
                InfantPharmAgentsModels.Delete(ID);

            if(nfm.pharm_interval != nasPharmTreatLengthText)
                nfm.pharm_length = nasPharmTreatLengthText;

            if(nfm.pharm_interval != nasIntervalText)
                nfm.pharm_interval = nasIntervalText;

            if(nfm.infant_meds_bool != infantMedRadio)
                nfm.infant_meds_bool = infantMedRadio;

            if (infantMedCheck == null || infantMedRadio != true)
                InfantDisMedsModels.Delete(ID);
            else if (infantMedCheck.Length != 0)
            {
                List<int> meds = new List<int>();
                List<string> comments = new List<string>();

                foreach (int med in infantMedCheck)
                {
                    meds.Add(med);
                    comments.Add("");
                }

                InfantDisMedsModels oldDisMeds = new InfantDisMedsModels(ID);
                //bool subEqual = oldSubs.sub_id.SequenceEqual(drugs);
                bool subEqual = new HashSet<int>(oldDisMeds.sub_id).SetEquals(meds);

                if (!subEqual)
                {
                    InfantDisMedsModels.Delete(ID);
                    InfantDisMedsModels idmm = new InfantDisMedsModels(nfm.nas_form_id, meds, comments);
                }
            }
            else
                InfantDisMedsModels.Delete(ID);

            if (nfm.infant_other_meds != infantOtherMedText)
            {
                if ((infantOtherMedRadio == true) && (infantOtherMedText.Length < 200))
                    nfm.infant_other_meds = infantOtherMedText;
                else
                    nfm.infant_other_meds = null;
            }

            if(nfm.milk_hosp != momMilkDuringRadio)
                nfm.milk_hosp = momMilkDuringRadio;
            if(nfm.milk_dis != momMilkDischargeRadio)
                nfm.milk_dis = momMilkDischargeRadio;

            if(nfm.formula_hosp != formulaRadio)
                nfm.formula_hosp = formulaRadio;
            if (formulaTypeCheck == null)
                InfantFormulasModels.Delete(ID);
            else if (formulaRadio == 1 && (formulaTypeCheck.Length != 0))
            {
                List<int> formulas = new List<int>();
                List<string> comments = new List<string>();

                foreach (int formula in formulaTypeCheck)
                {
                    formulas.Add(formula);
                    comments.Add("");
                }

                InfantFormulasModels oldFormulas = new InfantFormulasModels(ID);
                //bool subEqual = oldSubs.sub_id.SequenceEqual(drugs);
                bool subEqual = new HashSet<int>(oldFormulas.formula_id).SetEquals(formulas);

                if (!subEqual)
                {
                    InfantFormulasModels.Delete(ID);
                    InfantFormulasModels idmm = new InfantFormulasModels(nfm.nas_form_id, formulas, comments);
                }
            }
            else
                InfantFormulasModels.Delete(ID);

            if (hospLocationCheck == null)
                InfantLocationsModels.Delete(ID);
            else if (hospLocationCheck.Length != 0)
            {
                List<int> locations = new List<int>();
                List<string> comments = new List<string>();

                foreach (int location in hospLocationCheck)
                {
                    locations.Add(location);
                    comments.Add("");
                }

                InfantLocationsModels oldLocations = new InfantLocationsModels(ID);
                //bool subEqual = oldSubs.sub_id.SequenceEqual(drugs);
                bool subEqual = new HashSet<int>(oldLocations.hosp_ward_id).SetEquals(locations);

                if (!subEqual)
                {
                    InfantLocationsModels.Delete(ID);
                    InfantLocationsModels idmm = new InfantLocationsModels(nfm.nas_form_id, locations, comments);
                }
            }
            else
                InfantLocationsModels.Delete(ID);

            if(nfm.total_los != totalLengthOfStayText)
                nfm.total_los = totalLengthOfStayText;
            if(nfm.scn_los != scnLengthOfStayText)
            	nfm.scn_los = scnLengthOfStayText;
            if(nfm.nicu_los != nicuLengthOfStayText)
            	nfm.nicu_los = nicuLengthOfStayText;
            if(nfm.nur_los != nurseryLengthOfStayText)
            	nfm.nur_los = nurseryLengthOfStayText;
            if(nfm.ped_los != pedLengthOfStayText)
            	nfm.ped_los = pedLengthOfStayText;

            if(nfm.early_int != earlyInterventionRadio)
                nfm.early_int = earlyInterventionRadio;

            if (nfm.fiveone_a != fiveOneARadio)
                nfm.fiveone_a = fiveOneARadio;
            if (fiveOneARadio == 1)
            {
                if (nfm.fiveone_a_res != dcfRadio)
                    nfm.fiveone_a_res = dcfRadio;
            }
            else
                nfm.fiveone_a_res = null;

            if(nfm.dis_location != dischargeRadio)
                nfm.dis_location = dischargeRadio;

            if (nfm.transfer_bool != transferRadio)
                nfm.transfer_bool = transferRadio;
            if (transferRadio == true)
            {
                if (nfm.transfer_los != transferLengthText)
                    nfm.transfer_los = transferLengthText;
            }
            else
                nfm.transfer_los = null;

            return Redirect("/NAS/ViewAll/");
        }
        #endregion
    }
}
