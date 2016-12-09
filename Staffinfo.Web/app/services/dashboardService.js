'use strict';
app.factory('dashboardService', ['$http', 'ngAuthSettings','authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var dashboardServiceFactory = {};

    var _getSeniorityStatisctic = function(options) {
        return $http.get(serviceBase + 'api/employees/seniority/statistic');
    }

    //returns promise for getting services struct
    var _getServicesStruct = function () {
        return $http.get(serviceBase + 'api/employees/servicesstruct');
    }

    dashboardServiceFactory.getSeniorityStatistic = _getSeniorityStatisctic;
    dashboardServiceFactory.getServicesStruct = _getServicesStruct;

    return dashboardServiceFactory;

}]);