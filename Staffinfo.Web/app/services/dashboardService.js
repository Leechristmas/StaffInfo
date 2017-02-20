'use strict';
app.factory('dashboardService', ['$http', 'ngAuthSettings','authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var dashboardServiceFactory = {};

    //calendar properties and methods
    var _calendar = {
        selectedNotification: {},
        getSelectedNotification: function () {
            return this.selectedNotification;
        },
        setSelectedNotification: function (notification) {
            this.selectedNotification = notification;
        },
        getNotifications: function (options) {
            return $http.get(serviceBase + 'api/dashboard/notifications?includeCustomNotifications=' + options.includeCustomNotifications +
                '&includeSertification=' + options.includeSertification + '&includeBirthDates=' + options.includeBirthDates + '&includeRanks=' + options.includeRanks);
        },
        saveNotification: function (notification) {
            notification.author = authService.authentication.userName;
            return $http.post(serviceBase + 'api/dashboard/notifications', notification, {});
        },
        deleteNotification: function (id) {
            return $http.delete(serviceBase + 'api/dashboard/notifications?notificationId=' + id);
        }
    }

    //charts properties and methods
    var _charts = {
        getTotalSeniorityStatistic: function () {
            return $http.get(serviceBase + 'api/employees/seniority/statistic/total');
        },
        getActualSeniorityStatistic: function () {
            return $http.get(serviceBase + 'api/employees/seniority/statistic/actual');
        },
        getServicesStruct: function () {
            return $http.get(serviceBase + 'api/employees/servicesstruct');
        }
    }

    dashboardServiceFactory.calendar = _calendar;
    dashboardServiceFactory.charts = _charts;

    return dashboardServiceFactory;

}]);