'use strict';
app.factory('dashboardService', ['$http', 'ngAuthSettings','authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var dashboardServiceFactory = {};

    var _getTotalSeniorityStatisctic = function(options) {
        return $http.get(serviceBase + 'api/employees/seniority/statistic/total');
    }

    var _getActualSeniorityStatistic = function(options) {
        return $http.get(serviceBase + 'api/employees/seniority/statistic/actual');
    }

    //returns promise for getting services struct
    var _getServicesStruct = function () {
        return $http.get(serviceBase + 'api/employees/servicesstruct');
    }

    var _getNotifications = function(options) {
        return $http.get(serviceBase + 'api/dashboard/notifications?includeCustomNotifications=' + options.includeCustomNotifications +
            '&includeSertification=' + options.includeSertification + '&includeBirthDates=' + options.includeBirthDates);
    }

    var _saveNotification = function (notification) {
        notification.author = 'test';
        return $http.post(serviceBase + 'api/dashboard/notifications', notification, {});
    }

    dashboardServiceFactory.saveNotification = _saveNotification;
    dashboardServiceFactory.getNotifications = _getNotifications;
    dashboardServiceFactory.getActualSeniorityStatistic = _getActualSeniorityStatistic;
    dashboardServiceFactory.getTotalSeniorityStatistic = _getTotalSeniorityStatisctic;
    dashboardServiceFactory.getServicesStruct = _getServicesStruct;

    return dashboardServiceFactory;

}]);