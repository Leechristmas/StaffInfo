'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', 'messageService', '$mdToast', '$interval', function ($scope, dashboardService, messageService, $mdToast, $interval) {
        $scope.employees = [];
        $scope.servicesStruct = {};
        $scope.getSeniorityStatistic = {};

        $scope.isLoading = true;
        $scope.determinateValue = 30;

        // Iterate every 100ms, non-stop and increment
        // the Determinate loader.
        $interval(function () {

            $scope.determinateValue += 1;
            if ($scope.determinateValue > 100) {
                $scope.determinateValue = 30;
            }

        }, 100);

        //charts
        var myPieChart = {};
        var barChart = {};

        $scope.loadData = function() {
            $scope.isLoading = true;

            var q1 = dashboardService.getServicesStruct().then(function (response) {
                $scope.servicesStruct = response.data;
            });
            var q2 = dashboardService.getSeniorityStatistic().then(function(response) {
                $scope.getSeniorityStatistic = response.data;
            });

            Promise.all([q1, q2]).then(values => {
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

        $scope.reloadPie = function () {
            //clear the canvas
            document.getElementById("service-chart-container").innerHTML = "<canvas id='serviceChart' height='30' width='100'></canvas>";
            //get canvas
            var ctx = document.getElementById("serviceChart").getContext("2d");

            //replace to common
            //if (angular.equals($scope.servicesStruct, {})) {
            //}

            var labels = Object.keys($scope.servicesStruct);
            var data = Object.values($scope.servicesStruct);

            //build the chart
            myPieChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Службы',
                            data: data,
                            backgroundColor: [
                                "#FF6384",
                                "#36A2EB",
                                "#FFCE56"
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

        $scope.reloadBar = function () {
            //clear the canvas
            document.getElementById("seniority-chart-container").innerHTML = "<canvas id='seniorityChart' height='30' width='100'></canvas>";
            //get canvas
            var ctx = document.getElementById("seniorityChart").getContext("2d");

            var labels = Object.keys($scope.getSeniorityStatistic);
            var data = Object.values($scope.getSeniorityStatistic);

            //build the chart
            barChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                    {
                        label: 'Выслуга (количество человек в диапазоне)',
                        data: data,
                        backgroundColor: 'rgba(25, 118, 210, 0.8)',
                        borderWidth: 1
                    }]
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
    }
]);