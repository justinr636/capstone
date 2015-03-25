var margin = { top: 20, right: 100, bottom: 30, left: 50 };
var width = 960 - margin.left - margin.right;
var height = 500 - margin.top - margin.bottom;

var csvArray = [];  // Stores CSV
var titles = [];    // Stores CSV Column Headers
var allHids = [];   // Stores Unique Hospital IDs 

var global_hid = 0;