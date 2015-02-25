using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Capstone.Models;
using System.IO;
using Capstone.App_Start;

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

        public ActionResult StandardCharts()
        {
            return View();
        }

        [HttpPost()]
        public ActionResult UploadCSV(HttpPostedFileBase file)
        {
            //string filePath = "";
            //if (file.ContentLength > 0)
            //{
            //    string fileName = Path.GetFileName(file.FileName);
            //    filePath = Path.Combine(Server.MapPath("~/App_Data/upload"), fileName);
            //    file.SaveAs(filePath);
            //}
            // if fails should return to Redirect to error page

            string fileSavePath = "";
            var httpPostedFile = System.Web.HttpContext.Current.Request.Files["test"];

            if (httpPostedFile != null)
            {
                fileSavePath = Path.Combine(Server.MapPath("~/App_Data/upload"), httpPostedFile.FileName);
                httpPostedFile.SaveAs(fileSavePath);
            }


            //StreamReader reader = new StreamReader(file.InputStream);
            //DataTable dt = dbUtil.GetDataTableFromCsv(filePath, true);
            DataTable dt = dbUtil.GetDataTableFromCsv(fileSavePath, true);

            return Json(new { });
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
            // Box Plots to Create
            // Mother Subs vs LoS - already existing
            // Breast Milk vs LoS

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
        public ActionResult CustomizeBoxPlot(int data, int data2)
        {
            string title = "";

            bool intBool = true;

            int minInt = int.MaxValue;
            int maxInt = int.MinValue;

            double minDouble = double.MaxValue;
            double maxDouble = double.MinValue;

            List<string> keys = new List<string>();
            List<List<int>> intValues = new List<List<int>>();
            List<List<double>> doubleValues = new List<List<double>>();

            DataTable dt = ChartModels.GetBoxPlotData(data, data2, ref title);

            switch (data)
            {
                case 0:     // Birth Weight
                    intBool = false;
                    ChartModels.FormatBoxPlotData_Double(dt, 2, 3, ref keys, ref doubleValues, ref minDouble, ref maxDouble);
                    break;
                case 1:     // Length of Stay
                    ChartModels.FormatBoxPlotData_Int(dt, 2, 3, ref keys, ref intValues, ref minInt, ref maxInt);
                    break;
                case 2:     // Outborn Days
                    ChartModels.FormatBoxPlotData_Int(dt, 2, 3, ref keys, ref intValues, ref minInt, ref maxInt);
                    break;
                case 3:     // NAS Score
                    ChartModels.FormatBoxPlotData_Int(dt, 2, 3, ref keys, ref intValues, ref minInt, ref maxInt);
                    break;
                case 4:     // Total Pharmacology Length
                    ChartModels.FormatBoxPlotData_Int(dt, 2, 3, ref keys, ref intValues, ref minInt, ref maxInt);
                    break;
                case 5:     // Pharmacology Last Dose Interval
                    ChartModels.FormatBoxPlotData_Int(dt, 2, 3, ref keys, ref intValues, ref minInt, ref maxInt);
                    break;
                default:
                    break;
            }

            if (intBool)
                return Json(new { title = title, headers = keys, data = intValues, max = maxInt, min = minInt });
            else
                return Json(new { title = title, headers = keys, data = doubleValues, max = maxDouble, min = minDouble });
        }

        [HttpPost()]
        public ActionResult CustomizeRunChart(int data)
        {
            bool intBool = true;
            string yLabel = "";

            int uclInt, lclInt;
            double avg, variance, stdev;
            double UCL, LCL; 

            List<string> x_data = new List<string>();
            List<int> y_data_int = new List<int>();
            List<double> y_data_double = new List<double>();

            DataTable dt;

            switch (data)
            {
                case 0:     // Birth Weight
                    yLabel = "Birth Weight";

                    dt = ChartModels.RunChartVsTime("birth_weight");
                    x_data = ChartModels.CreateXData_Date(dt, 0);
                    y_data_double = ChartModels.CreateYData_Double(dt, 1);

                    avg = y_data_double.Average();
            		variance = y_data_double.Select(val => (val - avg) * (val - avg)).Sum();
            		stdev = Math.Sqrt(variance / y_data_double.Count);

            		UCL = avg + (3 * stdev);
            		LCL = avg - (3 * stdev);

            		uclInt = Convert.ToInt32(Math.Round(UCL));
            		lclInt = Convert.ToInt32(Math.Round(LCL));

                    intBool = false;

                    break;
                case 1:     // Length of Stay
                    yLabel = "Length Of Stay";

                    dt = ChartModels.RunChartVsTime("total_los");
                    x_data = ChartModels.CreateXData_Date(dt, 0);
                    y_data_int = ChartModels.CreateYData_Int(dt, 1);

                    avg = y_data_int.Average();
            		variance = y_data_int.Select(val => (val - avg) * (val - avg)).Sum();
            		stdev = Math.Sqrt(variance / y_data_int.Count);

            		UCL = avg + (3 * stdev);
            		LCL = avg - (3 * stdev);

            		uclInt = Convert.ToInt32(Math.Round(UCL));
            		lclInt = Convert.ToInt32(Math.Round(LCL));

                    break;
                case 2:     // Outborn Days
                    yLabel = "Outborn Days";

                    dt = ChartModels.RunChartVsTime("outborn_days");
                    x_data = ChartModels.CreateXData_Date(dt, 0);
                    y_data_int = ChartModels.CreateYData_Int(dt, 1);

                    avg = y_data_int.Average();
            		variance = y_data_int.Select(val => (val - avg) * (val - avg)).Sum();
            		stdev = Math.Sqrt(variance / y_data_int.Count);

            		UCL = avg + (3 * stdev);
            		LCL = avg - (3 * stdev);

            		uclInt = Convert.ToInt32(Math.Round(UCL));
            		lclInt = Convert.ToInt32(Math.Round(LCL));

                    break;
                case 3:     // NAS Score
                    yLabel = "NAS Score";

                    dt = ChartModels.RunChartVsTime("nas_score");
                    x_data = ChartModels.CreateXData_Date(dt, 0);
                    y_data_int = ChartModels.CreateYData_Int(dt, 1);

                    avg = y_data_int.Average();
            		variance = y_data_int.Select(val => (val - avg) * (val - avg)).Sum();
            		stdev = Math.Sqrt(variance / y_data_int.Count);

            		UCL = avg + (3 * stdev);
            		LCL = avg - (3 * stdev);

            		uclInt = Convert.ToInt32(Math.Round(UCL));
            		lclInt = Convert.ToInt32(Math.Round(LCL));

                    break;
                case 4:     // Total Pharmacology Length
                    yLabel = "Total Pharmacology Length";

                    dt = ChartModels.RunChartVsTime("pharm_length");
                    x_data = ChartModels.CreateXData_Date(dt, 0);
                    y_data_int = ChartModels.CreateYData_Int(dt, 1);

                    avg = y_data_int.Average();
            		variance = y_data_int.Select(val => (val - avg) * (val - avg)).Sum();
            		stdev = Math.Sqrt(variance / y_data_int.Count);

            		UCL = avg + (3 * stdev);
            		LCL = avg - (3 * stdev);

            		uclInt = Convert.ToInt32(Math.Round(UCL));
            		lclInt = Convert.ToInt32(Math.Round(LCL));

                    break;
                case 5:     // Pharmacology Last Dose Interval
                    yLabel = "Last Dose Interval";

                    dt = ChartModels.RunChartVsTime("pharm_interval");
                    x_data = ChartModels.CreateXData_Date(dt, 0);
                    y_data_int = ChartModels.CreateYData_Int(dt, 1);

                    avg = y_data_int.Average();
            		variance = y_data_int.Select(val => (val - avg) * (val - avg)).Sum();
            		stdev = Math.Sqrt(variance / y_data_int.Count);

            		UCL = avg + (3 * stdev);
            		LCL = avg - (3 * stdev);

            		uclInt = Convert.ToInt32(Math.Round(UCL));
            		lclInt = Convert.ToInt32(Math.Round(LCL));

                    break;
                default:
                    lclInt = 0;
                    uclInt = 0;
                    /*
                    double avg = y_data.Average();
            		double variance = y_data.Select(val => (val - avg) * (val - avg)).Sum();
            		double stdev = Math.Sqrt(variance / y_data.Count);

            		double ucl = avg + (3 * stdev);
            		double lcl = avg - (3 * stdev);

            		int uclInt = Convert.ToInt32(Math.Round(ucl));
            		int lclInt = Convert.ToInt32(Math.Round(lcl));

            		if (lclInt < 0)
            		    lclInt = 0;
            		        break;
                    */
                    break;
            }

            if (lclInt < 0)
                lclInt = 0;

            if (intBool)
            {
                var list = new[] { new { date = "", dta = 1 } }.ToList();

                for (int i = 0; i < x_data.Count; i++)
                    list.Add(new { date = x_data[i], dta = y_data_int[i] });

                list.RemoveAt(0);

                return Json(new { list = list, ucl = uclInt, lcl = lclInt, label = yLabel });
            }
            else
            {
                var list = new[] { new { date = "", dta = 1.0 } }.ToList();

                for (int i = 0; i < x_data.Count; i++)
                    list.Add(new { date = x_data[i], dta = y_data_double[i] });

                list.RemoveAt(0);

                return Json(new { list = list, ucl = uclInt, lcl = lclInt, label = yLabel });
            }
        }

        [HttpPost()]
        public ActionResult GetRunChartData()
        {
            DataTable dt = ChartModels.LosVsTime();

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
