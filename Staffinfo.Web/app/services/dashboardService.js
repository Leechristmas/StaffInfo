'use strict';
app.factory('dashboardService', ['$http', 'ngAuthSettings','authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var dashboardServiceFactory = {};

    //var _getEmployees = function () {
    //    var t = authService.isAuthenticated();
    //    console.log(t);
    //    return $http.get(serviceBase + 'api/employees').then(function(results) {
    //        return results;
    //    });
    //}

    //dashboardServiceFactory.getEmployees = _getEmployees;


    //returns promise for getting services struct
    var _getServicesStruct = function () {
        return $http.get(serviceBase + 'api/employees/servicesstruct');
    }

    dashboardServiceFactory.getServicesStruct = _getServicesStruct;

    return dashboardServiceFactory;

}]);