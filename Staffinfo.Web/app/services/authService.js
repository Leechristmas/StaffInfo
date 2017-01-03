'use strict';

app.factory('authService', [
    '$http', '$q', 'localStorageService', 'ngAuthSettings', 'Idle', function ($http, $q, localStorageService, ngAuthSettings, Idle) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: ""
        };

        var _saveRegistration = function (registration) {
            _logOut();

            return $http.post(serviceBase + "api/account/register", registration).then(function (response) {
                return response;
            });
        };

        var _login = function (loginData) {
            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
            var deferred = $q.defer();

            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                .success(function (response) {
                    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });

                    _authentication.isAuth = true;
                    _authentication.userName = loginData.userName;

                    deferred.resolve(response);
                    //Idle.watch();
                })
                .error(function (err, status) {
                    _logOut();
                    if(!err)
                        err = { error_description : "Неизвестная ошибка. Ваша сессия была закрыта." }
                    deferred.reject(err);
                });

            return deferred.promise;
        }
        var _logOut = function () {

            localStorageService.remove('authorizationData');

            _authentication.isAuth = false;
            _authentication.userName = "";

            //Idle.unwatch();
        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
            } else
                _logOut();
        }

        var _isAuthenticated = function() {
            return !!localStorageService.get('authorizationData');
        }

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;
        authServiceFactory.isAuthenticated = _isAuthenticated;

        return authServiceFactory;
    }
]);

