'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', 'messageService', '$mdToast', '$interval', '$mdDialog', 'settingsService', function ($scope, dashboardService, messageService, $mdToast, $interval, $mdDialog, settingsService) {
        
//COMMON----------------------------------------------
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

        //loads the data for the page
        $scope.loadData = function () {
            $scope.isLoading = true;

            var q1 = dashboardService.charts.getServicesStruct().then(function (response) {
                $scope.servicesStruct = response.data;
            });

            var q2 = dashboardService.charts.getTotalSeniorityStatistic().then(function (response) {
                $scope.totalSeniorityStatistic = response.data;
            });

            var q3 = dashboardService.charts.getActualSeniorityStatistic().then(function (response) {
                $scope.actualSeniorityStatistic = response.data;
            });

            Promise.all([q1, q2, q3]).then(values => {
                $scope.isLoading = false;
                console.log('data has been loaded.');
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });

        }

        $scope.loadData();

//----------------------------------------------------

//CHARTS----------------------------------------------
        $scope.servicesStruct = {};
        $scope.totalSeniorityStatistic = {};
        $scope.actualSeniorityStatistic = {};

        var pieChart = {};  //services
        var barChart = {};  //seniority

        $scope.includeRetirees = true;//include/exclude retirees to the seniority chart

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

        //redraws the pie chart (services)
        $scope.reloadPie = function () {

            if (getData($scope.servicesStruct).length <= 0) {
                document.getElementById("service-chart-container").innerHTML = "<h3 class='no-data-label'>Нет данных</h3>";
                return;
            }

            //clear the canvas
            document.getElementById("service-chart-container").innerHTML = "<canvas class=\"unselectable\" id='serviceChart' height='30' width='100'></canvas>";
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
            document.getElementById("seniority-chart-container").innerHTML = "<canvas class=\"unselectable\" id='seniorityChart' height='30' width='100'></canvas>";
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

//----------------------------------------------------

        
//CALENDAR--------------------------------------------
        //calendar
        $scope.eventSource = [];
        $scope.notification = {};

        $scope.showEventDetailsView = function (ev) {
            dashboardService.calendar.setSelectedNotification($scope.event);
            $mdDialog.show({
                controller: 'calendarController',
                templateUrl: 'app/views/eventDetailsView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            }).then(function(answer) {
                $scope.loadEvents();
                console.log('notification has been deleted.');
            }, function() {
                console.log('event details view has been closed.');
            });
        }

        $scope.showAddNotificationView = function (ev) {
            dashboardService.calendar.setSelectedNotification({});//set empty model for adding view
            $mdDialog.show({
                controller: 'calendarController',
                templateUrl: 'app/views/addNotificationView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            }).then(function (answer) {
                $scope.loadEvents();
                console.log('new notification has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        }

        //change mode: month/week/day
        $scope.changeMode = function (mode) {
            $scope.mode = mode;
        };

        //current day
        $scope.today = function () {
            $scope.currentDate = new Date();
        };

        //true if is today
        $scope.isToday = function () {
            var today = new Date(),
                currentCalendarDate = new Date($scope.currentDate);

            today.setHours(0, 0, 0, 0);
            currentCalendarDate.setHours(0, 0, 0, 0);
            return today.getTime() === currentCalendarDate.getTime();
        };

        //when event has been selected
        $scope.onEventSelected = function (event) {
            $scope.event = event;
        };

        //when event has been dbl clicked
        $scope.onDblClicked = function (event) {
            $scope.showEventDetailsView();
        }

        //updates the calendar
        $scope.updateCalendar = function () {
            $scope.$broadcast('eventSourceChanged', $scope.eventSource);
        }

        //prints selected date and time to console
        $scope.onTimeSelected = function (selectedTime) {
            console.log('Selected time: ' + selectedTime);
        };

        //random generation events
        var createRandomEvents = function () {
            var events = [];
            for (var i = 0; i < 50; i += 1) {
                var date = new Date();
                var eventType = Math.floor(Math.random() * 2);
                var startDay = Math.floor(Math.random() * 90) - 45;
                var endDay = Math.floor(Math.random() * 2) + startDay;
                var startTime;
                var endTime;
                var details = 'GHVHADHJADHVASHJDVGJASHDHGASVDGVAGHSDVGHAVSDHAGD';
                if (eventType === 0) {
                    startTime = new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate() + startDay));
                    if (endDay === startDay) {
                        endDay += 1;
                    }
                    endTime = new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate() + endDay));
                    events.push({
                        title: 'All Day - ' + i,
                        startTime: startTime,
                        endTime: endTime,
                        allDay: true,
                        details: details
                    });
                } else {
                    var startMinute = Math.floor(Math.random() * 24 * 60);
                    var endMinute = Math.floor(Math.random() * 180) + startMinute;
                    startTime = new Date(date.getFullYear(), date.getMonth(), date.getDate() + startDay, 0, date.getMinutes() + startMinute);
                    endTime = new Date(date.getFullYear(), date.getMonth(), date.getDate() + endDay, 0, date.getMinutes() + endMinute);
                    events.push({
                        title: 'Event - ' + i,
                        startTime: startTime,
                        endTime: endTime,
                        allDay: false,
                        details: details
                    });
                }
            }
            return events;
        }

        //loads all events
        $scope.loadEvents = function () {

            //load settings for calendar notifications
            settingsService.calendarSettings.includedNotificatoinTypes = settingsService.calendarSettings.loadIncludedNotificatoinTypes();

            //load notifications
            $scope.promise = dashboardService.calendar.getNotifications({
                includeCustomNotifications: settingsService.calendarSettings.customAreIncluded(),
                includeSertification: settingsService.calendarSettings.sertificationsAreIncluded(),
                includeBirthDates: settingsService.calendarSettings.birthdaysAreIncluded()
            }).then(function (response) {//success
                var events = response.data;
                events.forEach(function (item) {
                    item.allDay = true;
                    item.dueDate = new Date(item.dueDate);
                    item.startTime = new Date(item.dueDate.getFullYear(), item.dueDate.getMonth(), item.dueDate.getDate(), 3);
                    item.endTime = new Date(item.dueDate.getFullYear(), item.dueDate.getMonth(), item.dueDate.getDate(), 4);
                });
                $scope.eventSource = events;
                $scope.updateCalendar();
            }, function(data) {//error
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        };

        $scope.loadEvents();
    }
//----------------------------------------------------
]);