'use strict';

app.factory('employeesService', [
    "$http", 'ngAuthSettings', function($http, ngAuthSettings) {
        var employeesServiceFactory = {};
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        
        var _getEmployees = function () {
            return $http.get(serviceBase + 'api/employees').then(function (results) {
                return results;
            });
        }

        employeesServiceFactory.getEmployees = _getEmployees;
        
        return employeesServiceFactory;
    }
]);