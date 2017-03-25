'use strict';

app.factory('userService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    var userServiceFactory = {};

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var getUsers = function () {
        return $http.get(serviceBase + 'api/users/all');
    }

    var getEmployees = function () {
        return $http.get(serviceBase + 'api/users/not-registered');
    }

    var registerUser = function (user) {
        return $http.post(serviceBase + 'api/users/register', user, {});
    }

    var deleteAccount = function(accountId) {
        return $http.delete(serviceBase + 'api/users/all/' + accountId);
    }

    userServiceFactory.getUsers = getUsers;
    userServiceFactory.getEmployees = getEmployees;
    userServiceFactory.registerUser = registerUser;
    userServiceFactory.deleteAccount = deleteAccount;

    return userServiceFactory;
}]);