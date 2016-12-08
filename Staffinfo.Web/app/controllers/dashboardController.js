'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', 'messageService', '$mdToast', function ($scope, dashboardService, messageService, $mdToast) {
        $scope.employees = [];
        $scope.servicesStruct = {};

        //charts
        var myPieChart = {};
        var barChart = {};

        $scope.reloadPie = function () {
            //clear the canvas
            document.getElementById("service-chart-container").innerHTML = "<canvas id='serviceChart' height='30' width='100'></canvas>";
            //get canvas
            var ctx = document.getElementById("serviceChart").getContext("2d");

            //replace to common
            //if (angular.equals($scope.servicesStruct, {})) {
            //}

            dashboardService.getServicesStruct().then(function (response) {
                $scope.servicesStruct = response.data;

                var labels = Object.keys($scope.servicesStruct);
                var data = Object.values($scope.servicesStruct);

                //build the chart
                myPieChart = new Chart(ctx, {
                    type: 'pie',
                    data: {
                        labels: labels,
                        datasets: [
                        {
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
                        }]
                    },
                    options: {
                        legend: {
                            display: true
                        }
                    }
                });
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

        $scope.reloadBar = function () {
            //clear the canvas
            document.getElementById("seniority-chart-container").innerHTML = "<canvas id='seniorityChart' height='30' width='100'></canvas>";
            //get canvas
            var ctx = document.getElementById("seniorityChart").getContext("2d");

            dashboardService.getServicesStruct().then(function (response) {
                $scope.servicesStruct = response.data;

                var labels = Object.keys($scope.servicesStruct);
                var data = Object.values($scope.servicesStruct);

                //build the chart
                myPieChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                        {
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
                        }]
                    },
                    options: {
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
                        }
                    }
                });
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

    }
]);