//'use strict';

//app.factory('dashboardService', ['$http, ngAuthSettings', function ($http, ngAuthSettings) {
//    var serviceBase = ngAuthSettings.apiServiceBaseUri;
//    var dashboardServiceFactory = {};

//    var _getEmployees = function () {
//        return $http.get(serviceBase + 'api/employees')
//            .then(function (result) {
//                return result;
//            });
//    };

//    dashboardServiceFactory.getEmployees = _getEmployees;

//    return dashboardServiceFactory;
//}
//]);

'use strict';
app.factory('dashboardService', ['$http', 'ngAuthSettings','authService', function ($http, ngAuthSettings, authService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var dashboardServiceFactory = {};

    var _getEmployees = function () {
        var t = authService.isAuthenticated();
        console.log(t);
        return $http.get(serviceBase + 'api/employees').then(function(results) {
            return results;
        });
    }

    dashboardServiceFactory.getEmployees = _getEmployees;

    return dashboardServiceFactory;

}]);