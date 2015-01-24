using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Capstone.Models;

namespace Capstone.Controllers
{
    public class ChartsController : Controller
    {
        //
        // GET: /Charts/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Customize()
        {
            return View();
        }

        public ActionResult BoxPlot()
        {
            return View();
        }

        [HttpPost()]
        // Birth Weight vs NAS Score
        public ActionResult GetChartData1()
        {
            List<int> xAxisData = new List<int>();
            List<float> yAxisData = new List<float>();
            //List<object> data = new List<object>();

            DataTable dt = ChartModels.BirthWeightVsNASScore();

            foreach (DataRow dtRow in dt.Rows)
            {
                xAxisData.Add(Convert.ToInt32(dtRow["nas_score"]));
                yAxisData.Add(float.Parse(dtRow["birth_weight"].ToString(), System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            }

            //return Json(new { labels = xAxisLabels, datasets = data });
            return Json(new { xData = xAxisData, yData = yAxisData });
        }

        [HttpPost()]
        // Avg Length Of Stay vs Infant Pharmacology Agents (Infant Subs)
        public ActionResult GetChartData2()
        {
            List<string> xAxisData = new List<string>();
            List<int> yAxisData = new List<int>();

            DataTable dt = ChartModels.AvgLosVsInfantSubs();

            foreach (DataRow dtRow in dt.Rows)
            {
                xAxisData.Add(dtRow["sub_name"].ToString());
                yAxisData.Add(Convert.ToInt32(dtRow["avg_los"].ToString()));
            }

            return Json(new { xData = xAxisData, yData = yAxisData });
        }

        [HttpPost()]
        // Avg Length Of Stay vs Infant Formulas
        public ActionResult GetChartData3()
        {
            List<string> xAxisData = new List<string>();
            List<int> yAxisData = new List<int>();

            DataTable dt = ChartModels.AvgLosVsInfantFormulas();

            foreach (DataRow dtRow in dt.Rows)
            {
                xAxisData.Add(dtRow["formula_name"].ToString());
                yAxisData.Add(Convert.ToInt32(dtRow["avg_los"].ToString()));
            }

            return Json(new { xData = xAxisData, yData = yAxisData });
        }

        [HttpPost()]
        // Avg Length Of Stay vs Mother's Substances
        public ActionResult GetChartData4()
        {
            List<string> xAxisData = new List<string>();
            List<int> yAxisData = new List<int>();

            DataTable dt = ChartModels.AvgLosVsMotherSubs();

            foreach (DataRow dtRow in dt.Rows)
            {
                xAxisData.Add(dtRow["sub_name"].ToString());
                yAxisData.Add(Convert.ToInt32(dtRow["avg_los"].ToString()));
            }

            return Json(new { xData = xAxisData, yData = yAxisData });
        }

        [HttpPost()]
        public ActionResult GetBoxPlotData()
        {
            List<string> xAxisData = new List<string>();
            List<int> yAxisData = new List<int>();

            DataTable dt = ChartModels.LosVsMotherSubs();

            foreach (DataRow dtRow in dt.Rows)
            {
                xAxisData.Add(dtRow["sub_name"].ToString());
                yAxisData.Add(Convert.ToInt32(dtRow["total_los"].ToString()));
            }

            return Json(new { xData = xAxisData, yData = yAxisData });
        }

        [HttpPost()]
        public ActionResult GetChartIntvsInt(string col1, string col2)
        {
            List<int> xAxisData = new List<int>();
            List<int> yAxisData = new List<int>();

            //DataTable dt = ChartModels.AvgLosVsSub();
            DataTable dt = ChartModels.IntVsInt(col1, col2);

            foreach (DataRow dtRow in dt.Rows)
            {
                xAxisData.Add(Convert.ToInt32(dtRow["sub_name"].ToString()));
                yAxisData.Add(Convert.ToInt32(dtRow["avg_los"].ToString()));
            }

            return Json(new { xData = xAxisData, yData = yAxisData });
        }
    }
}
