var randomScalingFactor = function () {
    return Math.round(Math.random() * 50 + 10);
};
var randomColorFactor = function () {
    return Math.round(Math.random() * 255);
};
var randomColor = function (opacity) {
    return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',' + (opacity || '.8') + ')';
};

var rr = [
    randomScalingFactor(),
				randomScalingFactor(),
				randomScalingFactor(),
				randomScalingFactor(),
                randomScalingFactor(),
				randomScalingFactor(),
				randomScalingFactor(),
				randomScalingFactor()];

//New type Chart for labels with % near slice
Chart.defaults.pieLabels = Chart.helpers.clone(Chart.defaults.pie);

var helpers = Chart.helpers;
var defaults = Chart.defaults;

Chart.controllers.pieLabels = Chart.controllers.pie.extend({
    updateElement: function (arc, index, reset) {
        var _this = this;
        var chart = _this.chart,
            chartArea = chart.chartArea,
            opts = chart.options,
            animationOpts = opts.animation,
            arcOpts = opts.elements.arc,
            centerX = (chartArea.left + chartArea.right) / 2,
            centerY = (chartArea.top + chartArea.bottom) / 2,
            startAngle = opts.rotation, // non reset case handled later
            endAngle = opts.rotation, // non reset case handled later
            dataset = _this.getDataset(),
            circumference = reset && animationOpts.animateRotate ? 0 : arc.hidden ? 0 : _this.calculateCircumference(dataset.data[index]) * (opts.circumference / (2.0 * Math.PI)),
            innerRadius = reset && animationOpts.animateScale ? 0 : _this.innerRadius,
            outerRadius = reset && animationOpts.animateScale ? 0 : _this.outerRadius,
            custom = arc.custom || {},
            valueAtIndexOrDefault = helpers.getValueAtIndexOrDefault;

        helpers.extend(arc, {
            // Utility
            _datasetIndex: _this.index,
            _index: index,

            // Desired view properties
            _model: {
                x: centerX + chart.offsetX,
                y: centerY + chart.offsetY,
                startAngle: startAngle,
                endAngle: endAngle,
                circumference: circumference,
                outerRadius: outerRadius,
                innerRadius: innerRadius,
                label: valueAtIndexOrDefault(dataset.label, index, chart.data.labels[index])
            },

            draw: function () {
                var ctx = this._chart.ctx,
                                vm = this._view,
                                sA = vm.startAngle,
                                eA = vm.endAngle,
                                opts = this._chart.config.options;

                var labelPos = this.tooltipPosition();
                var segmentLabel = vm.circumference / opts.circumference * 100;

                ctx.beginPath();

                ctx.arc(vm.x, vm.y, vm.outerRadius, sA, eA);
                ctx.arc(vm.x, vm.y, vm.innerRadius, eA, sA, true);

                ctx.closePath();
                ctx.strokeStyle = vm.borderColor;
                ctx.lineWidth = vm.borderWidth;

                ctx.fillStyle = vm.backgroundColor;

                ctx.fill();
                ctx.lineJoin = 'bevel';

                if (vm.borderWidth) {
                    ctx.stroke();
                }

                var total = 0; //sum of all data
                var total = _this._data.reduce(function (a, b) {
                    return a + b;
                });
                //for (var val of _this._data) { total += val; }

                if (vm.circumference > 0) { // Trying to hide label when it doesn't fit in segment (0= always show, even overwriting)
                    ctx.beginPath();
                    var per = segmentLabel.toFixed(2);
                    ctx.font = helpers.fontString(opts.defaultFontSize, opts.defaultFontStyle, opts.defaultFontFamily);
                    ctx.fillStyle = "#000";
                    ctx.textBaseline = "top";
                    ctx.textAlign = "right";

                    var sliceBefore;  //percantage before slice
                    var sliceAfter;//percantage before slice
                    var labelRadiusKoeff = 1.05;
                    var limitPerc = 2;

                    if (index == 0)
                        sliceBefore = (_this._data[_this._data.length - 1]) / total * 100;
                    else
                        sliceBefore = (_this._data[index - 1]) / total * 100;


                    if (_this._data.length == (index + 1))
                        sliceAfter = (_this._data[0]) / total * 100;
                    else
                        sliceAfter = (_this._data[index + 1]) / total * 100;

                    var dx = 2 * (labelPos.x - vm.x) * labelRadiusKoeff + vm.x;
                    var dy = 2 * (labelPos.y - vm.y) * labelRadiusKoeff + vm.y;
                    if (Math.abs(dy - vm.y) > vm.outerRadius * 0.5) {
                        ctx.textAlign = "center";
                        if ((labelPos.y - vm.y) > 0) { //bottom of pie
                            ctx.textBaseline = "top";
                            dy -= 5;
                            if (sliceBefore < limitPerc && per < limitPerc) {
                                ctx.textAlign = "right";
                            }
                            if (sliceAfter < limitPerc && per < limitPerc) {
                                ctx.textAlign = "left";
                            }
                            if (sliceAfter < limitPerc && per < limitPerc && sliceBefore < limitPerc) {
                                ctx.textAlign = "center";
                            }

                        }
                        else { //top of pie
                            ctx.textBaseline = "bottom";
                            dy += 5;
                            if (sliceBefore < limitPerc && per < limitPerc) {
                                ctx.textAlign = "left";
                            }
                            if (sliceAfter < limitPerc && per < limitPerc) {
                                ctx.textAlign = "right";
                            }
                            if (sliceAfter < limitPerc && per < limitPerc && sliceBefore < limitPerc) {
                                ctx.textAlign = "center";
                            }
                        }
                    }
                    if (Math.abs(dx - vm.x) > vm.outerRadius * 0.5) {
                        if ((labelPos.x - vm.x) > 0) { //right side of pie
                            ctx.textAlign = "left";
                            dx -= 5;
                            if (sliceBefore < limitPerc && per < limitPerc) {
                                ctx.textBaseline = "top";
                            }
                            if (sliceAfter < limitPerc && per < limitPerc) {
                                ctx.textBaseline = "bottom";
                            }
                            if (sliceAfter < limitPerc && per < limitPerc && sliceBefore < limitPerc) {
                                ctx.textBaseline = "middle";
                            }
                        }
                        else { //left side of pie
                            ctx.textAlign = "right";
                            dx += 5;
                            if (sliceBefore < 2 && per < 2) {
                                ctx.textBaseline = "bottom";
                            }
                            if (sliceAfter < 2 && per < 2) {
                                ctx.textBaseline = "top";
                            }
                            if (sliceAfter < 2 && per < 2 && sliceBefore < 2) {
                                ctx.textBaseline = "middle";
                            }
                        }
                    }


                    ctx.fillText(per + "%", dx, dy);
                }
            }
        });

        var model = arc._model;
        model.backgroundColor = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(dataset.backgroundColor, index, arcOpts.backgroundColor);
        model.hoverBackgroundColor = custom.hoverBackgroundColor ? custom.hoverBackgroundColor : valueAtIndexOrDefault(dataset.hoverBackgroundColor, index, arcOpts.hoverBackgroundColor);
        model.borderWidth = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(dataset.borderWidth, index, arcOpts.borderWidth);
        model.borderColor = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(dataset.borderColor, index, arcOpts.borderColor);

        // Set correct angles if not resetting
        if (!reset || !animationOpts.animateRotate) {
            if (index === 0) {
                model.startAngle = opts.rotation;
            } else {
                model.startAngle = _this.getMeta().data[index - 1]._model.endAngle;
            }

            model.endAngle = model.startAngle + model.circumference;
        }

        arc.pivot();
    }
});

// plugin for tooltips
Chart.pluginService.register({
    beforeRender: function (chart) {
        if (chart.config.options.showAllTooltips) {
            // create an array of tooltips
            // we can't use the chart tooltip because there is only one tooltip per chart
            chart.pluginTooltips = [];
            chart.config.data.datasets.forEach(function (dataset, i) {
                chart.getDatasetMeta(i).data.forEach(function (sector, j) {
                    chart.pluginTooltips.push(new Chart.Tooltip({
                        _chart: chart.chart,
                        _chartInstance: chart,
                        _data: chart.data,
                        _options: chart.options.tooltips,
                        _active: [sector]
                    }, chart));
                });
            });

            // turn off normal tooltips
            chart.options.tooltips.enabled = false;
        }
    },
    afterDraw: function (chart, easing) {
        if (chart.config.options.showAllTooltips) {
            // we don't want the permanent tooltips to animate, so don't do anything till the animation runs atleast once
            if (!chart.allTooltipsOnce) {
                if (easing !== 1)
                    return;
                chart.allTooltipsOnce = true;
            }

            // turn on tooltips
            chart.options.tooltips.enabled = true;
            Chart.helpers.each(chart.pluginTooltips, function (tooltip) {
                tooltip.initialize();
                tooltip.update();
                // we don't actually need this since we are not animating tooltips
                tooltip.pivot();
                tooltip.transition(easing).draw();
            });
            chart.options.tooltips.enabled = false;
        }
    }
})