﻿'use strict';
app.factory('authInterceptorService', ['$q', '$location', 'localStorageService', 'messageService', function ($q, $location, localStorageService, messageService) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

        config.headers = config.headers || {};

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
    }

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
            $location.path('/login');

            if (localStorageService.get('authorizationData')) {
                console.log('server session has expired.');
                //messageService.errors.setError({ errorText: 'Время серверной сессии истекло. Пожалуйста, авторизуйтесь заново, чтобы продолжить работу', errorTitle: 'Внимание' });
            }
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}]);