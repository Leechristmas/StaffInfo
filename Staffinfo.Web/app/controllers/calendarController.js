'use strict';

app.controller('calendarController', [
    '$scope', 'dashboardService', function ($scope, dashboardService) {
        $scope.eventSource = [];

        $scope.changeMode = function (mode) {
            $scope.mode = mode;
        };

        $scope.today = function () {
            $scope.currentDate = new Date();
        };

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
            $scope.eventSource.push({
                title: 'TEST - 1',
                startTime: new Date($scope.currentDate),
                endTime: new Date($scope.currentDate),
                allDay: true,
                details: 'test details'
            });
            $scope.updateCalendar();
        }

        //updates the calendar
        $scope.updateCalendar = function () {
            $scope.$broadcast('eventSourceChanged', $scope.eventSource);
        }

        $scope.onTimeSelected = function (selectedTime) {
            console.log('Selected time: ' + selectedTime);
        };

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

        $scope.loadEvents = function () {
            //$scope.eventSource = createRandomEvents();
            $scope.promise = dashboardService.getNotifications({
                includeCustomNotifications: true,
                includeSertification: true,
                includeBirthDates: true
            }). then(function(response) {
                var events = response.data;
                events.forEach(function (item) {
                    item.allDay = true;
                    item.dueDate = new Date(item.dueDate);
                    item.startTime = new Date(item.dueDate.getFullYear(), item.dueDate.getMonth(), item.dueDate.getDate(), 3);
                    item.endTime = new Date(item.dueDate.getFullYear(), item.dueDate.getMonth(), item.dueDate.getDate(), 4);
                });
                var t = createRandomEvents();
                $scope.eventSource = events;
                $scope.updateCalendar();
            });
        };

        $scope.loadEvents();
    }
]);