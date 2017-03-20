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

    var registerUser = function (user) {
        return $http.post(serviceBase + 'api/users/register', user, {});
    }

    userServiceFactory.getUsers = getUsers;
    userServiceFactory.getEmployees = getEmployees;
    userServiceFactory.registerUser = registerUser;

    return userServiceFactory;
}]);