/*
 * drawCharts.js
 *
 * by Justin Ratra
 */



// #######################
//   Bar Chart Functions 
// #######################

function DateToString(d) {
    return ((d.getMonth() + 1) + "/" + d.getFullYear());
}

function drawBarChart(data, max, dateLabels, hospitalLabels, titles) {
    $("#chartDiv").empty();

    var numHospitals = hospitalLabels.length;

    var tooltip = d3.select('body').append('div')
                                .attr('class', 'tooltip')
                                .style('opacity', 1e-6);

    var x0 = d3.scale.ordinal()
                       .rangeRoundBands([0, width], .1);

    var x1 = d3.scale.ordinal();

    var y = d3.scale.linear()
			          .range([height, 0]);

    // Need to randomly generate colors based on number of hospitals
    // Also differentiate My Hospital vs others
    var color = d3.scale.ordinal()
	                      .range(randomColor({ count: numHospitals, hue: 'random', luminosity: 'bright' }));
    //var color = d3.interpolateRgb("#ffd", "#056");
    //var color = d3.scale.ordinal()
    //              .range(["#98abc5", "#8a89a6", "#7b6888", "#6b486b", "#a05d56", "#d0743c", "#ff8c00"]);

    var xAxis = d3.svg.axis()
			              .scale(x0)
			    	      .orient("bottom");

    var yAxis = d3.svg.axis()
			              .scale(y)
			    		  .orient("left");
    //.tickFormat(d3.format(".2s"));
    //.tickFormat(d3.format(function (d) { return d + "%"; }));

    var svg = d3.select("div#chartDiv").append("svg")
			            .attr("width", width + margin.left + margin.right)
			    		.attr("height", height + margin.top + margin.bottom)
			    		.append("g")
			    		.attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    //var hospIdIndex = data.length - 1;
    //var dateIndex = data.length - 1;
    //var X_DATA_PARSE = d3.time.format("%Y-%m-%d").parse;

    // Get Each Month
    /*
    var columnNames = d3.keys(data[hospIdIndex]).filter(function (key) { return key; });

    data.forEach(function (d) {
    //d.ages = ageNames.map(function (name) { return { name: name, value: +d[name] }; });
    d.hospitals = hospitalNames.map(function (name) { return { name: name, value: +d.ratio }; });
    });
    */

    x0.domain(dateLabels);
    x1.domain(hospitalLabels).rangeRoundBands([0, x0.rangeBand()]);
    //y.domain([0, d3.max(data, function (d) { return d3.max(d.hospitals, function (d) { return d.value; }); })]);
    y.domain([0, max]);

    // draw x-axis
    svg.append("g")
    		    .attr("class", "x axis")
    		    .attr("transform", "translate(0," + height + ")")
    		    .call(xAxis)
                .append("text")
                .attr("x", width / 2)
                .attr('y', 30)
                .attr('dy', '.71em')
                .style('text-anchor', 'middle')
                .style('font-size', '14px')
                .text(titles.xAxis);

    // draw y-axis
    svg.append("g")
    		    .attr("class", "y axis")
    		    .call(yAxis)
    		    .append("text")
    		    .attr("transform", "rotate(-90)")
    		    .attr("y", "-50")
    		    .attr("x", -(height / 2))
    		    .attr("dy", ".71em")
    		    .style("text-anchor", "end")
    		    .style("font-size", "14px")
    		    .text(titles.yAxis);

    var state = svg.selectAll(".state")
    		    .data(data)
    		    .enter().append("g")
    		    .attr("class", "g")
    //.attr("transform", function (d) { return "translate(" + x0(d.State) + ",0)"; });
    		    .attr("transform", function (d) { return "translate(" + x0(d.date) + ",0)"; });

    state.selectAll("rect")
	            .data(function (d) { return d.hospitals; })
    //.data(function (d) { return d.ages; })
    //.data(function (d) { return d[dateLabels[i]][hospitalLabels[j]]; })
    //.data(data)
    //.data(td)
    		    .enter().append("rect")
    		    .attr("width", x1.rangeBand())
	            .attr("x", function (d) { return x1(d.hid); })
	            .attr("y", function (d) { return y(d.ratio); })
	            .attr("height", function (d) { return height - y(d.ratio); })
	            .style("fill", function (d, i) { return color(i / (numHospitals - 1)); })
                .on("mouseover", function (d, i) {
                    $("div.tooltop").show();
                    tooltip.transition().duration(100).style("opacity", 1);
                }).on("mousemove", function (d, i) {
                    //var divHtml = '<h5>' + d['date'] + '</h5>';
                    var divHtml = '<strong>Date: </strong>' + d['date'] + '<br/>';
                    divHtml += '<strong>Hospital ID: </strong> ' + d['hid'] + '<br/>';
                    //divHtml += '<strong>Ratio: </strong> ' + (d['ratio'] * 100).toFixed(2) + '%' + '<br/>';
                    divHtml += '<strong>Ratio: </strong> ' + (d['ratio']).toFixed(2) + '%' + '<br/>';
                    divHtml += '<strong>Sample Size: </strong> ' + d['sample_size'];

                    if ($(window).width() - d3.event.pageX < 160) {
                        var left_position = (d3.event.pageX - 155) + "px";
                    } else {
                        var left_position = (d3.event.pageX - 2) + "px";
                    }
                    tooltip.html(divHtml).style("left", left_position).style("top", (d3.event.pageY - 80) + "px");
                }).on("mouseout", function (d, i) {
                    tooltip.transition().duration(100).style("opacity", 1e-6);
                });

    //yAxis.tickFormat(d3.format(function (d) { return d + "%"; }));
    //.tickFormat(d3.format(function (d) { return d + "%"; }));

    /*
    for (var i = 0; i < dateLabels.length; i++) {
    for (var j = 0; j < hospitalLabels.length; j++) {
    //console.log("data datelabels hosplabels");
    //console.log(data[dateLabels[i]][hospitalLabels[j]]);

    //var td = data[dateLabels[i]][hospitalLabels[j]];

    state.selectAll("rect")
    //.data(function (d) { return d.ages; })
    //.data(function (d) { return d[dateLabels[i]][hospitalLabels[j]]; })
    .data(data)
    //.data(td)
    .enter().append("rect")
    .attr("width", x1.rangeBand())
    //.attr("x", function (d) { return x1(d.hid); })
    //.attr("y", function (d) { return y(d.ratio); })
    .attr("x", function (d) { return x1(d[dateLabels[i]][hospitalLabels[j]].hid); })
    .attr("y", function (d) { return y(d[dateLabels[i]][hospitalLabels[j]].ratio); })
    .attr("height", function (d) { return height - y(d[dateLabels[i]][hospitalLabels[j]].ratio); })
    .style("fill", function (d) { return color(d[dateLabels[i]][hospitalLabels[j]].hid); });
    //.attr("x", function (d) { return x1(d.hid); })
    //.attr("y", function (d) { return y(d.ratio); })
    //.attr("height", function (d) { return height - y(d.ratio); })
    //.style("fill", function (d) { return color(d.hid); });
    }
    }

    console.log('state = ', state);
    */

    // draw title
    svg.append('text')
	                    .attr("x", width / 2)
	                    .attr("y", 0 + (margin.top / 2))
	                    .attr("text-anchor", "middle")
	                    .style("font-size", '18px')
	                    .text(titles.chartTitle);

    var legend = svg.selectAll(".legend")
    //.data(ageNames.slice().reverse())
    		    .data(hospitalLabels.slice().reverse())
    		    .enter().append("g")
    		    .attr("class", "legend")
    		    .attr("transform", function (d, i) { return "translate(0," + i * 20 + ")"; });

    legend.append("rect")
    		    .attr("x", width - 18)
    		    .attr("width", 18)
    		    .attr("height", 18)
    		    .style("fill", color);

    legend.append("text")
    		    .attr("x", width - 24)
    		    .attr("y", 9)
    		    .attr("dy", ".35em")
    		    .style("text-anchor", "end")
    		    .text(function (d) { return d; });
}

// #######################
//   Run Chart Functions 
// #######################

function drawRunChart(dataObj, label, width, height, selector) {
    var data = dataObj.data;
    var avg = dataObj.avg;
    var ucl = dataObj.ucl;
    var lcl = dataObj.lcl;

    var X_DATA_PARSE = d3.time.format("%Y-%m-%d").parse;
    //var X_DATA_PARSE = d3.time.format("%m/%d/%Y").parse;

    var Y_DATA_PARSE = 0;

    // This is the key in the data object passed to draw function
    var X_AXIS_COLUMN = "date";

    // This is the key in the data object passed to draw function
    var Y_AXIS_COLUMN = "val";

    var x = d3.time.scale()
	     	             .range([0, width]);

    var y = d3.scale.linear()
	     	             .range([height, 0]);

    var xAxis = d3.svg.axis()
	     	                 .scale(x)
	     	   			     .orient("bottom");

    var yAxis = d3.svg.axis()
	     	                 .scale(y)
	     	   			     .orient("left");

    var line = d3.svg.line()
    //.interpolate("linear")
	     	                .interpolate("linear")
	     	   	    		.x(function (d) { return x(d.x_axis); })
	     	   				.y(function (d) { return y(d.y_axis); });

    var svg = d3.select(selector).append("svg")
	     	                .attr("width", width + margin.left + margin.right)
	     	   	    		.attr("height", height + margin.top + margin.bottom)
	     	   				.append("g")
	     	   				.attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    var tooltip = d3.select('body').append('div')
                                .attr('class', 'tooltip')
                                .style('opacity', 1e-6);

    data.forEach(function (d) {
        d.x_axis = X_DATA_PARSE(d[X_AXIS_COLUMN]);
        d.y_axis = d[Y_AXIS_COLUMN] + 0;
    });

    // Set X Domain
    x.domain(d3.extent(data, function (d) { return d.x_axis; }));

    // Set Y Domain (including Control Limits)
    data.push({ x_axis: "", y_axis: ucl + 0 });
    data.push({ x_axis: "", y_axis: lcl + 0 });
    y.domain(d3.extent(data, function (d) { return d.y_axis; }));
    data.pop();
    data.pop();

    // draw x-axis
    svg.append("g")
	               .attr("class", "x axis")
	        	   .attr("transform", "translate(0," + height + ")")
	        	   .call(xAxis)
                   .append('text')
                   .attr('x', width / 2)
                   .attr('y', 30)
                   .attr('dy', '.71em')
                   .style('text-anchor', 'middle')
                   .style('font-size', '14px')
                   .text(label.xAxis);

    // draw y-axis
    svg.append("g")
	               .attr("class", "y axis")
	      		   .call(yAxis)
	               .append("text")
	               .attr("transform", "rotate(-90)")
	      		   .attr("y", "-50")
	      		   .attr("x", -(height / 2))
	      		   .attr("dy", ".71em")
	      		   .style("text-anchor", "end")
                   .style('font-size', '14px')
	      		   .text(label.yAxis);

    // draw run chart line
    svg.append("path")
	               .datum(data)
	      		   .attr("class", "line")
	      		   .attr("d", line);

    // draw data points on line
    svg.selectAll('circle')
                    .data(data)
                    .enter()
                    .append("circle")
    //.attr("fill", "rgba(22, 68, 81, 0.6)")
    // Flag data points >= UCL or <= LCL
                    .attr("fill", function (d) { return ((d.val >= ucl) || (d.val <= lcl) ? "rgba(220, 55, 41, 0.8)" : "rgba(22, 68, 81, 0.6)"); })
                    .attr("cx", function (d) {
                        return x(d.x_axis);
                    })
                    .attr("cy", function (d) {
                        return y(d.y_axis);
                    })
                    .attr("r", 3)
                    .on("mouseover", function (d, i) {
                        $("div.tooltip").show();
                        tooltip.transition().duration(100).style("opacity", 1);
                    }).on("mousemove", function (d, i) {
                        var divHtml = '<h5><strong>Date:</strong>&emsp;' + d.date + '</h5>';
                        divHtml += '<h5><strong>Value:</strong>&emsp;' + d.val + '</h5>';

                        if ($(window).width() - d3.event.pageX < 160) {
                            var left_position = (d3.event.pageX - 155) + "px";
                        } else {
                            var left_position = (d3.event.pageX - 2) + "px";
                        }
                        tooltip.html(divHtml).style("left", left_position).style("top", (d3.event.pageY - 80) + "px");
                    }).on("mouseout", function (d, i) {
                        tooltip.transition().duration(100).style("opacity", 1e-6);
                    });

    // upper limit line
    //var upper_limit = 15;
    //console.log("ucl = ", ucl);
    var upper_limit = ucl;
    svg.append("line")
	               .attr("class", "limit-line")
	    		   .attr({ x1: 0, y1: y(upper_limit), x2: width, y2: y(upper_limit) });
    svg.append("text")
	               .attr({ x: width + 5, y: y(upper_limit) + 4 })
	    		   .text("Upper Limit: " + parseInt(upper_limit));

    // lower limit line
    //var lower_limit = 6;
    var lower_limit = lcl;
    svg.append("line")
	               .attr("class", "limit-line")
	    		   .attr({ x1: 0, y1: y(lower_limit), x2: width, y2: y(lower_limit) });
    svg.append("text")
	               .attr({ x: width + 5, y: y(lower_limit) + 4 })
	    		   .text("Lower Limit: " + parseInt(lower_limit));

    // draw title
    svg.append('text')
	                    .attr("x", width / 2)
	                    .attr("y", 0 - (margin.top / 2))
	                    .attr("text-anchor", "middle")
	                    .style("font-size", '18px')
	                    .text(label.chartTitle);
}

// #######################
//    Box Plot Functions
// #######################

// Function to Calculate Error Bars for Box and Whisker Plot
function iqr(k) {
    return function (d, i) {
        var q1 = d.quartiles[0];
        var q3 = d.quartiles[2];
        var iqr = (q3 - q1) * k;
        var i = -1;
        var j = d.length;

        while (d[++i] < q1 - iqr);
        while (d[--j] > q3 + iqr);
        return [i, j];
    }
}

function drawBoxPlot(boxData, title, width, height, selector) {
    var data = boxData.data;
    var min = boxData.min;
    var max = boxData.max;

    var labels = true;

    var chart = d3.box()
                  .whiskers(iqr(1.5))
        	      .height(height)
        		  .domain([0, max])
        		  .showLabels(labels);

    var svg = d3.select(selector)
                .append("svg")
                .attr('width', width + margin.left + margin.right)
                .attr('height', height + margin.top + margin.bottom)
                .attr('class', 'box')
                .append('g')
                .attr('transform', 'translate(' + margin.left + ',' + margin.top + ')');

    var x = d3.scale.ordinal()
                    .domain(data.map(function (d) { return d[0] }))
                    .rangeRoundBands([0, width], 0.7, 0.3);

    var xAxis = d3.svg.axis()
                      .scale(x)
                      .orient('bottom')

    var y = d3.scale.linear()
                    .domain([0, max])
                    .range([height + margin.top, 0 + margin.top]);

    var yAxis = d3.svg.axis()
                      .scale(y)
                      .orient('left');

    // draw box plots
    svg.selectAll('.box')
            .data(data)
            .enter()
            .append('g')
            .attr("transform", function (d) { return "translate(" + x(d[0]) + "," + margin.top + ")"; })
            .call(chart.width(x.rangeBand()));

    // draw title
    svg.append('text')
            .attr("x", width / 2)
            .attr("y", 0 + (margin.top / 2))
            .attr("text-anchor", "middle")
            .style("font-size", '18px')
            .text(title.chartTitle);

    // draw y-axis
    svg.append('g')
            .attr('class', 'y axis')
            .call(yAxis)
            .append('text')
            .attr('transform', 'rotate(-90)')
            .attr('y', -50)
            .attr('x', -(height / 2))
            .attr('dy', '.71em')
            .style('text-anchor', 'end')
            .style('font-size', '14px')
            .text(title.yAxis);

    // draw x-axis
    svg.append('g')
            .attr('class', 'x axis')
            .attr('transform', 'translate(0,' + (height + margin.top + 15) + ')')
            .call(xAxis)
            .append('text')
            .attr('x', width / 2)
            .attr('y', 85)
            .attr('dy', '.71em')
            .style('text-anchor', 'middle')
            .style('font-size', '14px')
            .text(title.xAxis);


    //$(selector + " g.x g.tick text").map(function () {
    $("g.x g.tick text").map(function () {
        // translation formula 1:
        // y = 0.4932x + 11.422
        // translation formula 2:
        // y = 0.3961x + 14.865
        var text = $("#hidden-span").text($(this).text());

        //var translation = text.width() * 0.4932 + 11.422;
        var translation = text.width() * 0.3961 + 14.865;
        $(this).attr("transform", "rotate(90), translate(" + translation + ", -14)");
        return;
    });

    //var chartHeight = $chartDiv.height();
    //$body.height(bodyHeight + (chartHeight / 2));
}

// #######################
//  Funnel Plot Functions
// #######################

//  --- Sorts dataset by population size
function compare(a, b) {
    if (a.sample_size < b.sample_size)
        return -1;
    if (a.sample_size > b.sample_size)
        return 1;
    return 0;
}

// Sort By Multiple Attributes helper functions
function dynamicSort(property) {
    return function (obj1, obj2) {
        return obj1[property] > obj2[property] ? 1
                        : obj1[property] < obj2[property] ? -1 : 0;
    }
}

function dynamicSortMultiple(props) {
    return function (obj1, obj2) {
        var i = 0, result = 0, numProps = props.length;

        while (result === 0 && i < numProps) {
            result = dynamicSort(props[i])(obj1, obj2);
            i++;
        }
        return result;
    }
}

function compareGen(a, b) {
    if (a < b)
        return -1;
    if (a > b)
        return 1;
    return 0;
}

function drawFunnelPlot(data, title, width, height, selector) {
    var dataset = data.data;
    var sorted_names = data.names;
    var mean_incidence_rate = data.rate;

    var padding = 30;

    var svg = d3.select(selector).append("svg")
            	            .attr("width", width)
            	            .attr("height", height)
    var tooltip = d3.select('body').append('div')
            	            .attr('class', 'tooltip')
            	            .style('opacity', 1e-6);
    var max_x = d3.max(dataset, function (d) {
        return d['sample_size'];
    });
    var min_x = d3.min(dataset, function (d) {
        return d['sample_size'];
    });

    // Create scale functions
    var xScale = d3.scale.linear()
            	               .domain([0, max_x])
            	               .range([padding, width - padding * 2]);
    var yScale = d3.scale.linear()
            	               .domain([0, d3.max(dataset, function (d) { return d3.max([d['ratio'], d['plus_3sd']]); })])
            	               .range([height - padding, padding]);

    // Set up axes format
    var formatAsPercentage = d3.format("%");
    var xAxis = d3.svg.axis()
            	  .scale(xScale)
            	  .orient("bottom")
            	  .ticks(5)
            	  .tickSize(4, 0, 0);
    var yAxis = d3.svg.axis()
            	  .scale(yScale)
            	  .orient("left")
            	  .ticks(5)
            	  .tickSize(4, 0, 0)
            	  .tickFormat(formatAsPercentage);

    // Draw X - Axis
    svg.append("g")
       .attr("class", "axis")
       .attr("transform", "translate(0," + (height - padding) + ")")
       .call(xAxis)
       .append('text')
       .attr('x', width / 2)
       .attr('y', 30)
       .attr('dy', '.71em')
       .style('text-anchor', 'middle')
       .style('font-size', '14px')
       .text(title.xAxis);

    // Draw Y - Axis
    svg.append("g")
       .attr("class", "axis")
       .attr("transform", "translate(" + padding + ", 0)")
       .call(yAxis)
       .append('text')
       .attr('transform', 'rotate(-90)')
       .attr('y', '-50')
       .attr('x', -(height / 2))
       .attr('dy', '.71em')
       .style('text-anchor', 'end')
       .style('font-size', '14px')
       .text(title.yAxis);

    // draw title
    svg.append('text')
	   .attr("x", width / 2)
	   .attr("y", 0 + (margin.top / 2))
	   .attr("text-anchor", "middle")
	   .style("font-size", '18px')
	   .text(title.chartTitle);


    // Draw Confidence Intervals
    var confidence3sd_lower = d3.svg.line()
            	                .x(function (d) { return xScale(d['sample_size']); })
            	                .y(function (d) {
            	                    if (d['minus_3sd'] < 0.0) {
            	                        return yScale(0);
            	                    } else {
            	                        return yScale(d['minus_3sd']);
            	                    }
            	                })
            	                .interpolate("linear");
    var confidence3sd_upper = d3.svg.line()
            	                .x(function (d) {
            	                    return xScale(d['sample_size']);
            	                })
            	                .y(function (d) {
            	                    return yScale(d['plus_3sd']);
            	                })
            	                .interpolate("linear");
    var confidence2sd_lower = d3.svg.line()
            	                .x(function (d) {
            	                    return xScale(d['sample_size']);
            	                })
            	                .y(function (d) {
            	                    if (d['minus_2sd'] < 0.0) {
            	                        return yScale(0);
            	                    } else {
            	                        return yScale(d['minus_2sd']);
            	                    }
            	                })
            	                .interpolate("linear");
    var confidence2sd_upper = d3.svg.line()
            	                .x(function (d) {
            	                    return xScale(d['sample_size']);
            	                })
            	                .y(function (d) {
            	                    return yScale(d['plus_2sd']);
            	                })
            	                .interpolate("linear");
    svg.append("svg:path")
       .attr("d", confidence2sd_upper(dataset))
       .attr("class", "confidence95");
    svg.append("svg:path")
       .attr("d", confidence2sd_lower(dataset))
       .attr("class", "confidence95");
    svg.append("svg:path")
       .attr("d", confidence3sd_upper(dataset))
       .attr("class", "confidence99");
    svg.append("svg:path")
       .attr("d", confidence3sd_lower(dataset))
       .attr("class", "confidence99");

    // Draw line to indicate mean.
    svg.append("svg:line")
       .attr("x1", xScale(min_x))
       .attr("y1", yScale(mean_incidence_rate))
       .attr("x2", xScale(max_x))
       .attr("y2", yScale(mean_incidence_rate))
       .style("stroke", "rgba(6, 120, 155, 0.6)")
       .style("stroke-width", 1)
       .on("mouseover", function (d, i) {
           $('div.tooltip').show();
           tooltip.transition().duration(100).style("opacity", 1);
       }).on("mousemove", function () {
           var divHtml = '<h4>Mean Value</h4>';
           var left_position = (d3.event.pageX - 2) + "px";
           tooltip.html(divHtml).style("left", left_position).style('top', (d3.event.pageY - 80) + "px");
       }).on("mouseout", function (d, i) {
           tooltip.transition().duration(100).style("opacity", 1e-6);
       });

    // Create Circles
    svg.selectAll("circle")
       .data(dataset)
       .enter()
       .append("circle")
       .attr("fill", "rgba(22, 68, 81, 0.6)")
       .attr("cx", function (d) {
           return xScale(d['sample_size']);
       })
       .attr("cy", function (d) {
           return yScale(d['ratio']);
       })
       .attr("name", function (d) {
           return $.inArray(d['date'], sorted_names);
       })
       .attr("r", 5)
       .on("mouseover", function (d, i) {
           $("div.tooltip").show();
           tooltip.transition().duration(100).style("opacity", 1);
       }).on("mousemove", function (d, i) {
           var divHtml = '<h4>' + d['date'] + '</h4>';
           divHtml += '<strong>Population: </strong> ' + d['sample_size'] + '<br/>';
           divHtml += '<strong>Percentage: </strong> ' + (d['ratio'] * 100).toFixed(2) + '%';

           if ($(window).width() - d3.event.pageX < 160) {
               var left_position = (d3.event.pageX - 155) + "px";
           } else {
               var left_position = (d3.event.pageX - 2) + "px";
           }
           tooltip.html(divHtml).style("left", left_position).style("top", (d3.event.pageY - 80) + "px");
       }).on("mouseout", function (d, i) {
           tooltip.transition().duration(100).style("opacity", 1e-6);
       });
}

function customizeCSVData(chartType, Y_COL, X_COL, HID_COL, START_DATE, END_DATE) {
    // Returns data object formatted for SPC chart 
    //      depending on chartType value
    // 
    // chartType 
    //  1 = Run Chart                   Y_COL = number
    //  2 = Box and Whisker Plot
    //  3 = Funnel Plot                 Y_COL = "Yes" / "No", add functionality for "Checked" / "Unchecked"
    //  4 = Bar Chart
    // 

    if (chartType == 1) {           // Draw Run Chart
        var dataset = [];

        var avg_sum = 0;
        var variance_sum = 0;

        _.each(csvArray, function (item, i) {

            // Get Y-Axis indicator
            // This assumes y-axis = numbers (not "Yes" or "Checked")
            var num = parseInt(item[Y_COL]);

            // Get Date
            // This assumes x-axis = time    (YYYY-MM-DD)
            var dte = item[X_COL];
            var jsDte = new Date(item[X_COL]);

            // Get Hospital ID
            var hid = item[HID_COL];

            // if all items are defined, jsDte is between START_DATE and END_DATE
            //    and matches Hospital ID
            if ((num !== '') && (typeof num !== "undefined") && (jsDte !== '') && (typeof jsDte !== "undefined") && (jsDte > START_DATE) && (jsDte < END_DATE) && (hid == global_hid)) {
                var dataObj = { date: dte, val: num, hid: hid };
                dataset.push(dataObj);

                avg_sum += num;     // get total sum
            }
        });

        var avg = avg_sum / dataset.length;

        _.each(dataset, function (o, i) {
            variance_sum = variance_sum + ((o.val - avg) * (o.val - avg));
        });

        var variance = variance_sum / dataset.length;

        var stdev = Math.sqrt(variance);

        var ucl = avg + (3 * stdev);
        var lcl = avg - (3 * stdev);
        if (lcl < 0) lcl = 0;       // assumes lcl cannot be negative

        // Sort data by date
        dataset = _.sortBy(dataset, function (o) { var dt = new Date(o.date); return dt; });

        return { data: dataset, avg: avg, ucl: ucl, lcl: lcl };

    } else if (chartType == 2) {    // Draw Box Plot 
        var dataset = [];

        _.each(X_COL, function (item, i) {
            // Create X-Axis Label
            var labelText = $("#xDataDrop2").find("option[value='" + item + "']").text();
            var labelTextArr = labelText.split(/\(choice=(.*)\)/);
            labelText = labelTextArr[1];

            // Format dataset
            dataset[i] = [];
            dataset[i][0] = labelText;
            dataset[i][1] = [];
        });

        var max = -Infinity;
        var min = Infinity;

        // format data
        _.each(csvArray, function (item, i) {
            var num = parseInt(item[Y_COL]);
            var hid = item[HID_COL];

            if ((num !== '') && (typeof num !== "undefined") && (hid == global_hid)) {
                _.each(X_COL, function (header, i) {
                    if (item[header] == "Checked") {        // Assumes item[header] always = "Checked" / "Unchecked"
                        dataset[i][1].push(num);
                        if (num > max) max = num;
                        if (num < min) min = num;
                    }
                });
            }
        });
        console.log("dataset = ", dataset);

        return { data: dataset, min: min, max: max };
    } else if (chartType == 3) {	// Draw Funnel Plot 
        var funnelData = [];

        var total_population = 0;
        var incidence_population = 0;
        var sample_size = 0;
        var incidences = 0;

        var jsFirstDate = new Date(csvArray[0][X_COL]);
        var firstDate = (jsFirstDate.getMonth() + 1) + "/" + jsFirstDate.getFullYear();

        funnelData[0] = { sample_size: 0, indicator: 0, date: firstDate, ratio: 0 };

        _.each(csvArray, function (item, i) {
            var indicator = item[Y_COL];
            var jsDate = new Date(item[X_COL]);
            var dte = (jsDate.getMonth() + 1) + "/" + jsDate.getFullYear();
            var size = funnelData.length;
            var hid = item[HID_COL];

            if ((indicator !== '') && (typeof indicator !== "undefined") && (jsDate !== '') && (typeof jsDate !== "undefined") && (hid == global_hid)) {
                if (dte == funnelData[size - 1].date) {
                    sample_size++;
                    total_population++;
                    if (indicator == "Yes") {       // Assumes Indicator is always Yes/No
                        incidences++;
                        incidence_population++;
                    }
                }
                else {
                    funnelData[size - 1].sample_size = sample_size;
                    funnelData[size - 1].indicator = incidences;
                    funnelData[size - 1].ratio = incidences / sample_size;

                    sample_size = 1;
                    incidences = 0;
                    funnelData.push({ sample_size: 0, indicator: 0, date: dte, ratio: 0 });

                    if (indicator == "Yes") {
                        incidences++;
                        incidence_population++;
                    }
                    total_population++;
                }
            }
        });

        var size = funnelData.length;

        funnelData[size - 1].sample_size = sample_size;
        funnelData[size - 1].indicator = incidences;
        funnelData[size - 1].ratio = incidences / sample_size;

        // Calculate mean incidence over entire population
        var mean_incidence_rate = incidence_population / total_population;
        var mean_incidence = incidence_population / size;

        var sigma_squared = mean_incidence_rate * (1 - mean_incidence_rate);

        // Create Sorted List of Labels
        var sorted_names = [];
        _.each(funnelData, function (item) {
            sorted_names.push(item["date"]);
        });

        // Sorts dataset by population size for drawing confidence intervals
        funnelData.sort(compare);

        // Calculate standard error for each value: SE = SD / sqrt(n)
        //  ---Creating Control Limits/Confidence Intervals
        _.each(funnelData, function (item) {
            item['std_error'] = Math.sqrt(sigma_squared / item['sample_size']);
            item['plus_2sd'] = mean_incidence_rate + (2 * item['std_error']);
            item['minus_2sd'] = mean_incidence_rate - (2 * item['std_error']);
            item['plus_3sd'] = mean_incidence_rate + (3 * item['std_error']);
            item['minus_3sd'] = mean_incidence_rate - (3 * item['std_error']);
        });

        return { data: funnelData, names: sorted_names, rate: mean_incidence_rate };
    } else if (chartType == 4) {    // Draw Bar Chart
        var barData = [];

        //var Y_COL = $yDataDrop.val();
        //var X_COL = $xDataDrop.val();
        var Y_COL = 63;         // If Infant Received Pharm treatment 
        var X_COL = 1;          // Date
        var HID_COL = 105;      // Hospital ID column
        var YEAR = 2014;      // Date Range
        //var byMonth = true;
        //var START_DATE = new Date("2014-01-01");      // Date Range
        //var END_DATE = new Date("2014-12-31");      // Date Range

        var startDateText = $("#startDateText").val();
        var endDateText = $("#endDateText").val();
        var byMonth = true;

        if ($("#yearRadio:checked").length > 0)
            byMonth = false;

        //var START_DATE = new Date("2011-01-01");      // Date Range
        //var END_DATE = new Date("2015-12-31");      // Date Range
        var START_DATE = new Date(startDateText);      // Date Range
        var END_DATE = new Date(endDateText);      // Date Range

        /*
        var incidences = 0;
        var sample_size = 0;
        var id = 0;
        var hospitalLabels = [];
        */

        // Sort CSV by Hospital ID and Time
        //csvArray.sort(dynamicSortMultiple([HID_COL, X_COL]));
        // Sort CSV by Time and Hospital ID
        csvArray.sort(dynamicSortMultiple([X_COL, HID_COL]));
        console.log("sorted csvArray", csvArray);

        /*
        var jsFirstDate = new Date(csvArray[0][X_COL]);
        var firstDate = (jsFirstDate.getMonth() + 1) + "/" + jsFirstDate.getFullYear();
        id = csvArray[0][HID_COL];
        hospitalLabels.push(id);

        barData[0] = { sample_size: 0, indicator: 0, date: firstDate, ratio: 0, hid: id };
        */

        var idLabels = [];    // d.State
        var dateLabels = [];    // d.State
        //var testData = {};
        var testData2 = [];

        // Need to get each month (and possibly year) between START_DATE and END_DATE

        var startMonth = START_DATE.getMonth() + 1;
        var endMonth = END_DATE.getMonth() + 1;
        var startYear = START_DATE.getFullYear();
        var endYear = END_DATE.getFullYear();

        for (startYear; startYear <= endYear; startYear++) {
            if (byMonth) {
                if (startYear < endYear) {
                    for (startMonth; startMonth <= 12; startMonth++) {
                        var dateStr = startMonth + "/" + startYear;
                        dateLabels.push(dateStr);
                        //testData[dateStr] = {};

                        testData2.push({ date: dateStr, hospitals: [] });
                    }
                    startMonth = 1;
                } else if (startYear == endYear) {
                    for (startMonth; startMonth <= endMonth; startMonth++) {
                        var dateStr = startMonth + "/" + startYear;
                        dateLabels.push(dateStr);
                        //testData[dateStr] = {};

                        testData2.push({ date: dateStr, hospitals: [] });
                    }
                } else { }
            } else {
                var dateStr = startYear;
                dateLabels.push(dateStr);
                testData2.push({ date: dateStr, hospitals: [] });
            }
        }
        //console.log("testData = ", testData);

        _.each(csvArray, function (item, i) {
            var item_date = new Date(item[X_COL]);
            var item_id = item[HID_COL];

            if (item_date > START_DATE && item_date < END_DATE && typeof item_id !== "undefined" && item_id !== '') {
                if (byMonth)
                    item_date = DateToString(item_date);
                else
                    item_date = item_date.getFullYear();

                var newId = $.inArray(item_id, idLabels);
                if (newId == -1) {
                    idLabels.push(item_id);
                    //barData[barData.length - 1].values[newId][0]
                    //testData[item_date][2]++;
                    //if (item[Y_COL] === "Yes") testData[item_date][0]++;
                    /*
                    testData[item_date][item_id] = {
                    sample_size: 0,
                    indicator: 0,
                    ratio: 0
                    }
                    */
                    for (var i = 0; i < dateLabels.length; i++) {

                        //testData[dateLabels[i]][item_id] = {
                        //    indicator: 0,
                        //    sample_size: 0,
                        //    ratio: 0,
                        //    date: dateLabels[i],
                        //    hid: item_id
                        //};

                        //testData[i][1].indicator = 0;
                        //testData[i][1].push([item_id, []]);

                        testData2[i].hospitals.push({
                            hid: item_id,
                            date: dateLabels[i],
                            indicator: 0,
                            sample_size: 0,
                            ratio: 0
                        });
                    }

                }

                //testData[item_date][item_id].sample_size++;

                var newDate = $.inArray(item_date, dateLabels);
                newId = $.inArray(item_id, idLabels);

                //console.log("newDate = ", newDate);
                //console.log("newId = ", newId);
                //console.log("testData2 = ", testData2);
                if (newDate !== -1 && newId !== -1) {
                    //if (item[Y_COL] == "Yes") testData2[newDate][item_id].indicator++;
                    testData2[newDate].hospitals[newId].sample_size++;
                    if (item[Y_COL] == "Yes") testData2[newDate].hospitals[newId].indicator++;
                }


                /*
                var newDate = $.inArray(item_date, dateLabels);

                if (newDate == -1) {
                dateLabels.push(item_date);
                //barData.push({
                //    date: (item_date.getMonth() + 1) + "/" + item_date.getFullYear(),
                //    values: [[]]     // [[]]
                //    // [hopsitalIds][3] 
                //    // [indicator, ratio, sample_size]
                //});
                //testData[item_date] = {};
                //for (var i = 0; i < idLabels.length; i++)
                //    barData[barData.length - 1].values[i][2] = 0;
                }

                if (typeof testData[item_date][item_id] == 'object') {
                testData[item_date][item_id].sample_size++;
                if (item[Y_COL] === "Yes") testData[item_date][item_id].indicator++;
                } else {
                testData[item_date][item_id] = {
                sample_size: 0,
                indicator: 0,
                ratio: 0
                }
                }
                */
            }
        });

        var maxRatio = 0;

        //_.each(testData, function (item) {
        //    for (var i = 0; i < idLabels.length; i++) {
        //        var hospItem = item[idLabels[i]];
        //        if (hospItem.sample_size !== 0) {
        //            var ratio = hospItem.indicator / hospItem.sample_size;
        //            hospItem.ratio = ratio;

        //            if (ratio > maxRatio) maxRatio = ratio;
        //        }
        //    }
        //});

        _.each(testData2, function (item) {
            var hospItems = item.hospitals;

            for (var i = 0; i < idLabels.length; i++) {
                if (hospItems[i].sample_size !== 0) {
                    var ratio = (hospItems[i].indicator / hospItems[i].sample_size) * 100;
                    hospItems[i].ratio = ratio;

                    if (ratio > maxRatio) maxRatio = ratio;
                }
            }
        });

        //testData.hospLabels = idLabels;
        //testData.dateLabels = dateLabels;

        console.log("idLabels = ", idLabels);
        console.log("dateLabels = ", dateLabels);
        //console.log("testData = ", testData);
        console.log("testData2 = ", testData2);

        /*
        _.each(csvArray, function (item, i) {
        var indicator = item[Y_COL];        // Possibly parseInt or parseFloat
        var jsDate = new Date(item[X_COL]);
        var dte = (jsDate.getMonth() + 1) + "/" + jsDate.getFullYear();
        var itemID = item[HID_COL];
        var size = barData.length;

        if ((indicator !== '') && (typeof indicator !== "undefined") && (jsDate !== '') && (typeof jsDate !== "undefined") && (jsDate.getFullYear() == YEAR)) {
        if ((dte == barData[size - 1].date) && (id == itemID)) {
        sample_size++;
        //total_population++;
        if (indicator == "Yes") {
        incidences++;
        //incidence_population++;
        }
        }
        else {
        barData[size - 1].sample_size = sample_size;
        barData[size - 1].indicator = incidences;
        barData[size - 1].ratio = (incidences / sample_size) * 100;
        //barData[size - 1].hid = id;

        if (itemID !== id) hospitalLabels.push(itemID);

        sample_size = 1;
        incidences = 0;
        id = itemID;
        barData.push({ sample_size: 0, indicator: 0, date: dte, ratio: 0, hid: id });

        if (indicator == "Yes") {
        incidences++;
        //incidence_population++;
        }
        //total_population++;
        }
        }
        });

        var size = barData.length;

        barData[size - 1].sample_size = sample_size;
        barData[size - 1].indicator = incidences;
        barData[size - 1].ratio = incidences / sample_size;

        var zeroDate = barData[0].date;
        if (zeroDate.substr(zeroDate.length - 4, 4) !== YEAR.toString())
        barData.splice(0, 1);

        //_.sortBy(barData, function (item) { return item.date; });

        console.log("barData = ", barData);

        var testData = [];

        testData[0] = [];
        testData[0][0] = "Date";
        testData[0][1] = [];

        _.each(hospitalLabels, function (item, i) {
        // Format dataset
        testData[i + 1] = [];
        testData[i + 1][0] = item;
        testData[i + 1][1] = [];
        });

        _.each(barData, function (item, i) {
        });

        */

        var titleObj = { yAxis: $yAxisTitleText.val(), xAxis: $xAxisTitleText.val(), chartTitle: $chartTitleText.val() };

        drawBarChart(testData2, maxRatio, dateLabels, idLabels, titleObj);
    }
}