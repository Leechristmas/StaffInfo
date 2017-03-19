'use strict';

app.factory('userService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    var userServiceFactory = {};

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var getUsers = function () {
        return $http.get(serviceBase + 'api/users/all');
    }

    var getEmployees = function () {
        return $http.get(serviceBase + 'api/users/employees');
    }

    userServiceFactory.getUsers = getUsers;
    userServiceFactory.getEmployees = getEmployees;

    return userServiceFactory;
}]);