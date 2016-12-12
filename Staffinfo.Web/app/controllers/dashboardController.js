'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', 'messageService', '$mdToast', '$interval', function ($scope, dashboardService, messageService, $mdToast, $interval) {
        $scope.employees = [];
        $scope.servicesStruct = {};
        $scope.totalSeniorityStatistic = {};
        $scope.actualSeniorityStatistic = {};

        $scope.isLoading = true;
        $scope.determinateValue = 30;

        $scope.includeRetirees = true;//include/exclude retirees to the seniority chart

        // Iterate every 100ms, non-stop and increment
        // the Determinate loader.
        $interval(function () {

            $scope.determinateValue += 1;
            if ($scope.determinateValue > 100) {
                $scope.determinateValue = 30;
            }

        }, 100);

        //charts
        var pieChart = {};  //services
        var barChart = {};  //seniority

        //include/exclude reirees from the bar chart (seniority)
        $scope.toggleRetirees = function (includeRetirees) {
            if (!includeRetirees) {
                barChart.data.datasets[0].data = getData($scope.totalSeniorityStatistic);
                barChart.data.labels = getLabels($scope.totalSeniorityStatistic);
            } else {
                barChart.data.datasets[0].data = getData($scope.actualSeniorityStatistic);
                barChart.data.labels = getLabels($scope.actualSeniorityStatistic);
            }
            barChart.update();
            console.log(getData($scope.totalSeniorityStatistic));
            console.log(getData($scope.actualSeniorityStatistic));
        }

        //loads the data for the page
        $scope.loadData = function() {
            $scope.isLoading = true;

            var q1 = dashboardService.getServicesStruct().then(function (response) {
                $scope.servicesStruct = response.data;
            });
            var q2 = dashboardService.getTotalSeniorityStatistic().then(function (response) {
                $scope.totalSeniorityStatistic = response.data;
            });

            var q3 = dashboardService.getActualSeniorityStatistic().then(function(response) {
                $scope.actualSeniorityStatistic = response.data;
            });

            Promise.all([q1, q2, q3]).then(values => {
                $scope.isLoading = false;
                console.log('data has been loaded.');
            }, function (data) {
                messageService.setError(data);
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    templateUrl: 'app/views/error-toast.html'
                });
            });

        }

        $scope.loadData();
        
        //redraws the pie chart (services)
        $scope.reloadPie = function () {
            //clear the canvas
            document.getElementById("service-chart-container").innerHTML = "<canvas id='serviceChart' height='30' width='100'></canvas>";
            //get canvas
            var ctx = document.getElementById("serviceChart").getContext("2d");

            //replace to common
            //if (angular.equals($scope.servicesStruct, {})) {
            //}
            
            //build the chart
            pieChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: getLabels($scope.servicesStruct),
                    datasets: [
                        {
                            label: 'Службы',
                            data: getData($scope.servicesStruct),
                            backgroundColor: [
                                "#FF6384",
                                "#36A2EB",
                                "#FFCE56",
                                "#B61118",
                                "#CC7014",
                                "#6AB210",
                                "#1FB2AD",
                                "#0C2A7E",
                                "#5C11B6",
                                "#C332B2"

                            ],
                            hoverBackgroundColor: [
                                "#FF6384",
                                "#36A2EB",
                                "#FFCE56"
                            ]
                        }
                    ]
                },
                options: {
                    legend: {
                        display: true
                    }
                }
            });
        }
        
        //redraws the bar chart (seniority)
        $scope.reloadBar = function () {
            //clear the canvas
            document.getElementById("seniority-chart-container").innerHTML = "<canvas id='seniorityChart' height='30' width='100'></canvas>";
            //get canvas
            var ctx = document.getElementById("seniorityChart").getContext("2d");
            
            //build the chart
            barChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: getLabels($scope.totalSeniorityStatistic),
                    datasets: [
                        {
                            label: 'Выслуга (количество человек в диапазоне)',
                            data: getData($scope.totalSeniorityStatistic),
                            backgroundColor: 'rgba(25, 118, 210, 0.8)',
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    responsive: true,
                    legend: {
                        display: true
                    },
                    scales: {
                        yAxes: [{
                            display: true,
                            ticks: {
                                suggestedMin: 0,    // minimum will be 0, unless there is a lower value.
                                // OR //
                                beginAtZero: true   // minimum value will be 0.
                            }
                        }]
                    },
                    tooltips: {
                        callbacks: {
                            //label: function (tooltipItem, data) { //percentage

                            //    var sum = data.datasets[0].data.reduce(function (a, b) {
                            //        return a + b;
                            //    });
                            //    var value = data.datasets[0].data[tooltipItem.index];

                            //    var label = '';
                            //    if (data.labels[tooltipItem.index])
                            //        label = data.labels[tooltipItem.index].toString();//.split(/\s+/g)[0];
                            //    var percentage = (value / sum * 100).toFixed(2);

                            //    return label + ' ' + percentage + '%';
                            //}
                        },
                        yAlign: "top"
                    }
                }
            });
        }
        
        //returns labels for chart from the object
        var getLabels = function (obj) {
            return Object.keys(obj);
        }

        //returns data for chart from the object
        var getData = function (obj) {
            return Object.values(obj);
        }
    }
]);