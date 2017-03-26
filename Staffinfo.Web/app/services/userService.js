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

    var addPermission = function (userId, role) {
        return $http.post(serviceBase + 'api/users/all/permissions', { userId: userId, role: role, action: 1 }, {});
    }

    var removePermission = function (userId, role) {
        return $http.post(serviceBase + 'api/users/all/permissions', { userId: userId, role: role, action: 0 }, {});
    }

    var changePassword = function(userId, currentPassword, newPassword) {
        return $http.post(serviceBase + 'api/users/all/change-password',
            { userId: userId, currentPassword: currentPassword, newPassword: newPassword }, {});
    }

    userServiceFactory.getUsers = getUsers;
    userServiceFactory.getEmployees = getEmployees;
    userServiceFactory.registerUser = registerUser;
    userServiceFactory.deleteAccount = deleteAccount;
    userServiceFactory.addPermission = addPermission;
    userServiceFactory.removePermission = removePermission;
    userServiceFactory.changePassword = changePassword;

    return userServiceFactory;
}]);