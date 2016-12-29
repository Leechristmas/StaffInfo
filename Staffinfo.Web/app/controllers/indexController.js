'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', '$mdDialog', 'Idle', 'Keepalive', 'idleConfig', '$timeout', 'idleService', '$state', 'localStorageService', function ($scope, $location, authService, $mdDialog, Idle, Keepalive, idleConfig, $timeout, idleService, $state, localStorageService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    }

    var originatorEv;
    this.openMenu = function ($mdOpenMenu, ev) {
        originatorEv = ev;
        $mdOpenMenu(ev);
    };

    this.menuClick = function () {
        originatorEv = null;
    }

    $scope.actualDate = new Date(Date.now()).toLocaleString("ru", { year: 'numeric', month: 'long', day: 'numeric', weekday: 'long' });

    $scope.authentication = authService.authentication;
    $scope.isAuth = authService.isAuthenticated;

    //IDLE------------------------------------------------

    $scope.$on('IdleStart', function () {//user is enactive
        console.log('idle started');
        if (!authService.isAuthenticated()) return;

        idleService.idleHasBeenStopped = false;

        $mdDialog.show({
            controller: 'dialogController',
            templateUrl: 'app/views/idleWarning.html',
            parent: angular.element(document.body),
            clickOutsideToClose: true
        });
    });

    $scope.$on('IdleEnd', function () {//user has returned
        idleService.idleHasBeenStopped = true;
    });

    $scope.$on('IdleTimeout', function () {//time has expired
        $mdDialog.show({
            controller: 'dialogController',
            templateUrl: 'app/views/sessionExpiredView.html',
            parent: angular.element(document.body),
            clickOutsideToClose: true
        }).finally(function() {
            authService.logOut();
            $state.go('login');
        });
    });

    //$scope.$on('Keepalive', function() {
    //    console.log(localStorageService.get('authorizationData'));
    //    console.log(authService.isAuthenticated());
    //});

    //----------------------------------------------------s
}]);