'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', '$mdDialog', function ($scope, $location, authService, $mdDialog) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    var originatorEv;
    this.openMenu = function ($mdOpenMenu, ev) {
        originatorEv = ev;
        $mdOpenMenu(ev);
    };

    $scope.authentication = authService.authentication;

}]);