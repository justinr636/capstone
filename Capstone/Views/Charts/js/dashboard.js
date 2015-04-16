var store = new Persist.Store('nas');

var margin = { top: 20, right: 50, bottom: 30, left: 50 };
var width = 960;
var height = 450;
//var width = 960;
//var height = 927

var csvArray = [];  // Stores CSV
var titles = [];    // Stores CSV Column Headers
var allHids = [];   // Stores Unique Hospital IDs
var chartOptions = [];

function isNumeric(n) { return !isNaN(parseFloat(n)) && isFinite(n); }

function drawChart(opts) {
    var chartType = opts.options.chartType;
    
    if (chartType == 1) {           // Draw Run Chart
        drawRunChart(customizeCSVData(opts.options, opts.Y_COL, opts.X_COL, opts.HID_COL, opts.START_DATE, opts.END_DATE),
                 opts.titles, opts.width, opts.height, opts.selector);
    } else if (chartType == 2) {    // Draw Box Plot
        // Customize Report - Box and Whisker Plot
        drawBoxPlot(customizeCSVData({ chartType: chartType, byMonth: true }, opts.Y_COL, opts.X_COL, opts.HID_COL, opts.START_DATE, opts.END_DATE),
                    opts.titles, opts.width, opts.height, opts.selector);
                             //{ yAxis: $yAxisTitleText.val(), xAxis: $xAxisTitleText.val(), chartTitle: $chartTitleText.val() },
                             //width - margin.left - margin.right,
                             //height - margin.top - margin.bottom,
                             //"div#custChartDiv");
    } else if (chartType == 3) {    // Draw Funnel Plot
        drawFunnelPlot(customizeCSVData(opts.options, opts.Y_COL, opts.X_COL, opts.HID_COL, opts.START_DATE, opts.END_DATE),
                   opts.titles, opts.width, opts.height, opts.selector);
    } else if (chartType == 4) {    // Draw Bar Chart
    }
    
}

var global_hid = "AVG";

//var global_hid_col = 105;
//var global_date_col = 1;

//var global_date_col = -1;
//var global_losstart_col = -1;
//var global_milk_col = -1;
//var global_pharm_col = -1;
//var global_dischargemeds_col = -1;
//var global_losend_col = -1;
//var global_hid_col = -1;
//var global_date_col_text = "";
//var global_losstart_col_text = "";
//var global_milk_col_text = "";
//var global_pharm_col_text = "";
//var global_dischargemeds_col_text = "";
//var global_losend_col_text = "";
//var global_hid_col_text = "";

//var col_titles = [];
//col_titles.push("global_date_col");
//col_titles.push("global_losstart_col");
//col_titles.push("global_milk_col");
//col_titles.push("global_pharm_col");
//col_titles.push("global_dischargemeds_col");
//col_titles.push("global_losend_col");
//col_titles.push("global_hid_col");

var col_titles = [ "global_date_col", "global_losstart_col", "global_milk_col",
                   "global_pharm_col", "global_dischargemeds_col", "global_losend_col",
                   "global_hid_col", "global_weight_col", "global_maxcal_col",
                   "global_locations_cols", "global_mfdrugs_cols", "global_formulas_cols",
                   "global_dismeds_cols" ];

var global_cols = { };

for (var i = 0; i < col_titles.length; i++) {
    var str = col_titles[i];
    global_cols[str] = {};
    global_cols[str].val = -1;          // Index of CSV Column
    global_cols[str].name = [];         // CSV Header Text
}
global_cols["global_date_col"].name.push("Date of Audit");
global_cols["global_losstart_col"].name.push("If outborn, what day of life was admission to your hospital?     Date of birth is day of life ONE.  ");
global_cols["global_milk_col"].name.push("Did infant receive any of his/her mother's own milk at any time during hospitalization?  ");
global_cols["global_pharm_col"].name.push("Did infant receive pharmacologic agents for NAS?");
global_cols["global_dischargemeds_col"].name.push("At time of discharge or transfer from  your hospital, was infant receiving medications for NAS?");
global_cols["global_losend_col"].name.push("What day of life was infant discharged from your hospital?    Day of birth is considered day of life ONE.  ");
global_cols["global_hid_col"].name.push("Your Hospital ID");
global_cols["global_weight_col"].name.push("Birth weight (Grams)");
global_cols["global_maxcal_col"].name.push("What was the maximum caloric density of human milk or formula given to infant during hospitalization?");
global_cols["global_locations_cols"].name.push("In what locations did the infant receive care during hospitalization?      Check all that apply. (choice=");
global_cols["global_mfdrugs_cols"].name.push("What were the maternal-fetal opioid exposures?      Check all that apply.  Information can come from maternal self-report, maternal, or neonatal toxicology screen.   Do not include if exposure was clearly ONLY in first trimester.  Short-acting opioids include codeine, oxycodone, hydrocodone, morphine, and hydromorphine.  Buprenorphine includes Subutex and Suboxone.  (choice=");
global_cols["global_mfdrugs_cols"].name.push("What were other maternal-fetal exposures of note?      Check all that apply.    Do not include if exposure was clearly only in first trimester.   (choice=");
global_cols["global_formulas_cols"].name.push("What types of formula did infant receive during hospitalization?      Check all that apply.   (choice=");
global_cols["global_dismeds_cols"].name.push("If yes, what medication was infant receiving at time of discharge or transfer?      Check all that apply.   (choice=");

//global_cols["global_date_col"].name.push("");
global_cols["global_losstart_col"].name.push("outborn_day_of_admission");
global_cols["global_milk_col"].name.push("hm_any");
global_cols["global_pharm_col"].name.push("pharm_tx_any");
global_cols["global_dischargemeds_col"].name.push("discharge_med");
global_cols["global_losend_col"].name.push("discharge_day");
global_cols["global_hid_col"].name.push("hospital_id");
global_cols["global_weight_col"].name.push("birth_weight");
global_cols["global_maxcal_col"].name.push("caloric_maximum");
global_cols["global_locations_cols"].name.push("care_locations_");
global_cols["global_mfdrugs_cols"].name.push("maternal_opioid_exposure_");
global_cols["global_formulas_cols"].name.push("formula_types_");
global_cols["global_dismeds_cols"].name.push("discharge_med");

function validateCSVFile() {
// ensures hard-coded columns for Local and State Reports
// are in the proper location
    for (var property in global_cols) {
        if (global_cols.hasOwnProperty(property)) {
            if ((document.cookie.indexOf(property) >= 0) && (document.cookie.indexOf(property + "_text") >= 0)) {
                var val = getCookie(property);
                var name = getCookie(property+"_text");
                global_cols[property].val = val;
                global_cols[property].name = name;
                //if (titles[global_cols[property].val] !== global_cols[property].name) {     // Checks if CSV column header == column header stored in user's cookie
                //    global_cols[property].val = -1;
                //}
                var nameArr = global_cols[property].name;
                
                for (var i = 0; i < nameArr.length; i++) {      // Checks if CSV column header == column header stored in user's cookie
                    if (titles[val].indexOf(nameArr[i]) < 0) {
                        global_cols[property].val = -1;
                    }
                }
            }
        }
    }

    /*
    if ((document.cookie.indexOf("global_date_col") >= 0) && (document.cookie.indexOf("global_date_col_text") >= 0)) {
        global_date_col = getCookie("global_date_col");
        global_date_col_text = getCookie("global_date_col_text");
        if (titles[global_date_col] !== global_date_col_text) {
            global_date_col = -1;
        }
    }
    if ((document.cookie.indexOf("global_los_start_col") >= 0) && (document.cookie.indexOf("global_los_start_col_text") >= 0)) {
        global_los_start_col = getCookie("global_los_start_col");
        global_los_start_col_text = getCookie("global_los_start_col_text");
        if (titles[global_los_start_col] !== global_los_start_col_text) {
            global_los_start_col = -1;
        }
    }
    if ((document.cookie.indexOf("global_milk_col") >= 0) && (document.cookie.indexOf("global_milk_col_text") >= 0)) {
        global_milk_col = getCookie("global_milk_col");
        global_milk_col_text = getCookie("global_milk_col_text");
        if (titles[global_milk_col] !== global_milk_col_text) {
            global_milk_col = -1;
        }
    }
    if ((document.cookie.indexOf("global_pharm_col") >= 0) && (document.cookie.indexOf("global_pharm_col_text") >= 0)) {
        global_pharm_col = getCookie("global_pharm_col");
        global_pharm_col_text = getCookie("global_pharm_col_text");
        if (titles[global_pharm_col] !== global_pharm_col_text) {
            global_pharm_col = -1;
        }
    }
    if ((document.cookie.indexOf("global_discharge_meds_col") >= 0) && (document.cookie.indexOf("global_discharge_meds_col_text") >= 0)) {
        global_discharge_meds_col = getCookie("global_discharge_meds_col");
        global_discharge_meds_col_text = getCookie("global_discharge_meds_col_text");
        if (titles[global_discharge_meds_col] !== global_discharge_meds_col_text) {
            global_discharge_meds_col = -1;
        }
    }
    if ((document.cookie.indexOf("global_los_end_col") >= 0) && (document.cookie.indexOf("global_los_end_col_text") >= 0)) {
        global_los_end_col = getCookie("global_los_end_col");
        global_los_end_col_text = getCookie("global_los_end_col_text");
        if (titles[global_los_end_col] !== global_los_end_col_text) {
            global_los_end_col = -1;
        }
    }
    if ((document.cookie.indexOf("global_hid_col") >= 0) && (document.cookie.indexOf("global_hid_col_text") >= 0)) {
        global_hid_col = getCookie("global_hid_col");
        global_hid_col_text = getCookie("global_hid_col_text");
        if (titles[global_hid_col] !== global_hid_col_text) {
            global_hid_col = -1;
        }
    }
    */
    
    for (var i = 0; i < titles.length; i++) {
        
        var item = titles[i];
    
        for (var property in global_cols) {
            if (global_cols.hasOwnProperty(property)) {
                var propArr = property.split("_");
                var mult_cols = (propArr[propArr.length-1] == "cols");
                if (global_cols[property].val == -1 || mult_cols) {
                    //if (titles[i] == global_cols[property].name) {
                    //    global_cols[property].val = i;
                    //    setCookie(property, i, 365);
                    //    setCookie(property+"_text", titles[i], 365);
                    //}
                    var nameArr = global_cols[property].name;
                    for (var j = 0; j < nameArr.length; j++)
                    {
                        if (titles[i].indexOf(nameArr[j]) >= 0) {      // If global_header matches csv_header, set the global_col index number
                            if (mult_cols) {
                                if (typeof global_cols[property].val !== "number") global_cols[property].val.push(i);
                                else global_cols[property].val = [i];
                                
                                setCookie(property, global_cols[property].val, 365);
                                setCookie(property+"_text", nameArr[j], 365);
                            } else {
                                global_cols[property].val = i;
                                setCookie(property, i, 365);
                                setCookie(property+"_text", nameArr[j], 365);
                                break;
                            }
                            
                        }
                    }
                }
            }
        }
        
        /*
        if (item == "Date of Audit") {
            //global_date_col = i;
            global_cols["global_date_col"].val = i;
            setCookie("global_date_col", i, 365);
            setCookie("global_date_col_text", "Date of Audit", 365);
        }
        else if (item == "If outborn, what day of life was admission to your hospital?     Date of birth is day of life ONE.  ") {
            global_los_start_col = i;
            setCookie("global_los_start_col", i, 365);
            setCookie("global_los_start_col_text", "If outborn, what day of life was admission to your hospital?     Date of birth is day of life ONE.  ", 365);
        }
        else if (item == "Did infant receive any of his/her mother's own milk at any time during hospitalization?  ") {
            global_milk_col = i;
            setCookie("global_milk_col", i, 365);
            setCookie("global_milk_col_text", "Did infant receive any of his/her mother's own milk at any time during hospitalization?  ", 365);
        }
        else if (item == "Did infant receive pharmacologic agents for NAS?") {
            global_pharm_col = i;
            setCookie("global_pharm_col", i, 365);
            setCookie("global_pharm_col_text", "Did infant receive pharmacologic agents for NAS?", 365);
        }
        else if (item == "What day of life was infant discharged from your hospital?    Day of birth is considered day of life ONE.  ") {
            global_los_end_col = i;
            setCookie("global_los_end_col", i, 365);
            setCookie("global_los_end_col_text", "What day of life was infant discharged from your hospital?    Day of birth is considered day of life ONE.  ", 365);
        }
        else if (item == "At time of discharge or transfer from  your hospital, was infant receiving medications for NAS?") {
            global_discharge_meds_col = i;
            setCookie("global_discharge_meds_col", i, 365);
            setCookie("global_discharge_meds_col_text", "At time of discharge or transfer from  your hospital, was infant receiving medications for NAS?", 365);
        }
        else if (item == "Hospital.ID") {
            global_hid_col = i;
            setCookie("global_hid_col", i, 365);
            setCookie("global_hid_col", "Hospital.ID", 365);
        }
        */
        
    }
    
    /*
    console.log(global_date_col);
    console.log(global_los_start_col);
    console.log(global_milk_col);
    console.log(global_pharm_col);
    console.log(global_discharge_meds_col);
    console.log(global_los_end_col);
    console.log(global_hid_col);
    
    if (global_date_col  == -1) {
        //console.log("Date of Audit");
        $("#global-date-error").show();
    }
        
    if (global_los_start_col  == -1) {
        //console.log("If outborn, what day of life was admission to your hospital?     Date of birth is day of life ONE.  ");
        $("#global-los-start-error").show();
    }
        
    if (global_milk_col  == -1) {
        //console.log("Did infant receive any of his/her mother's own milk at any time during hospitalization?  ");
        $("#global-milk-error").show();
    }
        
    if (global_pharm_col  == -1) {
        //console.log("Did infant receive pharmacologic agents for NAS?");
        $("#global-pharm-error").show();
    }
        
    if (global_los_end_col  == -1) {
        //console.log("What day of life was infant discharged from your hospital?    Day of birth is considered day of life ONE.  ");
        $("#global-los-end-error").show();
    }
        
    if (global_discharge_meds_col  == -1) {
        //console.log("At time of discharge or transfer from  your hospital, was infant receiving medications for NAS?");
        $("#global-discharge-error").show();
    }
        
    if (global_hid_col  == -1) {
        //console.log("Hospital.ID");
        $("#global-hid-error").show();
    }
    */
    
    for (var property in global_cols) {
        if (global_cols.hasOwnProperty(property)) {
            if (global_cols[property].val == -1) {
                var propArr = property.split('_');
                $('#' + propArr[0] + '-' + propArr[1] + '-error').show();
            }
        }
    }
    
    console.log("global_cols = ", global_cols);
    //console.log("document.cookie = ", document.cookie);
}

function setCookie(cname, cval, cexdays) {
    var d = new Date();
    d.setTime(d.getTime() + (cexdays*24*60*60*1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cval + "; " + expires;
    
    //console.log("document.cookie = ", document.cookie);
    
    var isChromium = window.chrome,
        vendorName = window.navigator.vendor;
    if(isChromium !== null && isChromium !== "undefined" && vendorName === "Google Inc.") {
        // is Google chrome
        store.set(cname, cval);
        //if (window.localStorage !== "undefined") {
        //    //try {
        //    //    localStorage.setItem(cname, cval);
        //    //    //console.log("set cookie, name = ", name);
        //    //    //console.log("set cookie, val = ", val);
        //    //} catch (e) {
        //    //    console.log("Exceeded localStorage quota");
        //    //}
        //}
    }
}

function getCookie(cname) {
    var val = "";
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) val = c.substring(name.length, c.length);
    }
    
    var isChromium = window.chrome,
        vendorName = window.navigator.vendor;
    if(isChromium !== null && isChromium !== undefined && vendorName === "Google Inc.") {
        // is Google chrome
        val = store.get(cname);
        //if (window.localStorage !== "undefined") {
        //    val = localStorage.getItem(cname);
        //}
    }
    
    //console.log("got cookie, val = ", val);
    return val;
}