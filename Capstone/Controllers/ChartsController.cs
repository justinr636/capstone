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

        public ActionResult RunChart()
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
            //List<string> xAxisData = new List<string>();
            //List<int> yAxisData = new List<int>();

            List<string> keys = new List<string>();
            List<List<int>> values = new List<List<int>>();

            DataTable dt = ChartModels.LosVsMotherSubs();

            string currentSub = dt.Rows[0]["sub_name"].ToString();
            keys.Add(currentSub);
            //string currentSub = "";

            List<int> currentValues = new List<int>();

            int min = int.MaxValue;
            int max = int.MinValue;

            foreach (DataRow dtRow in dt.Rows)
            {
                string sub = dtRow["sub_name"].ToString();
                int val = Convert.ToInt32(dtRow["total_los"]);

                if (sub == currentSub)
                {
                    currentValues.Add(val);

                    if (val > max)
                        max = val;

                    if (val < min)
                        min = val;
                }
                else
                {
                    currentSub = sub;
                    keys.Add(sub);

                    values.Add(new List<int>(currentValues));
                    currentValues.Clear();
                    currentValues.Add(val);
                }

                //xAxisData.Add(dtRow["sub_name"].ToString());
                //yAxisData.Add(Convert.ToInt32(dtRow["total_los"].ToString()));
            }

            values.Add(new List<int>(currentValues));

            //return Json(new { xData = xAxisData, yData = yAxisData });
            return Json(new { headers = keys, data = values, max = max, min = min });
        }

        [HttpPost()]
        public ActionResult GetRunChartData()
        {
            DataTable dt = ChartModels.LosVsTime();

            // Try returning the data in one object d.x_axis and d.y_axis
            // debug in console.log javascript to see what X_DATA_PARSE does.
            List<int> y_data = new List<int>();
            List<string> x_data = new List<string>();

            foreach (DataRow dtRow in dt.Rows)
            {
                y_data.Add(Convert.ToInt32(dtRow["total_los"]));

                DateTime dateT = Convert.ToDateTime(dtRow["birth_date"].ToString());
                string dateStr = dateT.Date.ToString("MM/dd/yyyy");

                x_data.Add(dateStr);

                //x_data.Add(dtRow["birth_date"].ToString());
            }

            double avg = y_data.Average();
            double variance = y_data.Select(val => (val - avg) * (val - avg)).Sum();
            double stdev = Math.Sqrt(variance / y_data.Count);

            double ucl = avg + (3 * stdev);
            double lcl = avg - (3 * stdev);

            int uclInt = Convert.ToInt32(Math.Round(ucl));
            int lclInt = Convert.ToInt32(Math.Round(lcl));

            if (lclInt < 0)
                lclInt = 0;

            //return Json(new { x_axis = x_data, y_axis = y_data, ucl = uclInt, lcl = lclInt });

            // Return data in same format as website
            var list = new[] { new { date = "", los = 1 } }.ToList();

            for (int i = 0; i < x_data.Count; i++)
            {
                list.Add(new { date = x_data[i], los = y_data[i] });
            }

            //foreach (DataRow dtRow in dt.Rows)
            //    list.Add(new { date = dtRow["birth_date"].ToString(), los = Convert.ToInt32(dtRow["total_los"]) });

            list.RemoveAt(0);

            return Json(new { list = list, ucl = uclInt, lcl = lclInt });
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
