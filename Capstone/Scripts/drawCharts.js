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

function drawRunChart(data, ucl, lcl, label) {
    $("#chartDiv").empty();

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

    var svg = d3.select("div#chartDiv").append("svg")
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
	                    .attr("y", 0 + (margin.top / 2))
	                    .attr("text-anchor", "middle")
	                    .style("font-size", '18px')
	                    .text(label.chartTitle);
}

// #######################
//    Box Plot Functions
// #######################

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

function drawBoxPlot(data, min, max, title) {
    $("#chartDiv").empty();

    var labels = true;

    // Parse through data
    // Convert all to integers Math.floor()

    var chart = d3.box()
        .whiskers(iqr(1.5))
        .height(height)
        .domain([min, max])
        .showLabels(labels);

    var svg = d3.select('div#chartDiv')
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
                    //.domain([min, max])
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


    $("g.x g.tick text").map(function () {
        // translation formula:
        // y = 0.4932x + 11.422
        var translation = $(this).width() * 0.4932 + 11.422;
        $(this).attr("transform", "rotate(90), translate(" + translation + ", -15)");

        //console.log("translation = ", translation);

        return;
    });

    // Transform X-Axis Labels
    //var xLabelsArray = $("g.x g.tick text").map(function () { return $(this).width(); }).get();
    //var len = xLabelsArray.length;
    //var xLabelTransform = [];

    //$("g.x g.tick text").attr("transform", "rotate(90), translate(" + val + ", -15)");

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

function drawFunnelPlot(dataset, sorted_names, mean_incidence_rate, title) {
    // Draw the graph
    $("#chartDiv").empty();

    //var w = width;
    //var h = height;
    var padding = 30;

    var svg = d3.select("#chartDiv").append("svg")
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

    // Draw axes and axis labels
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

    //$("#x-axis-label").text("X-Axis Label");
    //$("#y-axis-label").text("Y-Axis Label");
    //$("#x-axis-label").css('top', (height - 10) + 'px').css('left', (width - 100) + 'px');

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

    //$("#confidence-label").css('left', (w - 100) + 'px');


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

    // Select DropDown
    /*
    var select_html = '<option value=""></option>';
    $.each(sorted_names, function (i, v) {
    select_html += '<option value="' + i + '">' + v + '</option>';
    });
    $("#select-entry").html(select_html);
    $("#select-entry").change(function (e) {
    // Clear Existing Circles
    d3.select('circle').transition().transition(2).attr("r", 5);
    // Highlight the circle with the same name val
    var circle = d3.select('circle[name="' + $(this).val() + '"]');
    circle.transition().duration(800).attr("r", 10);
    });
    */

    // Show the graph
}