'use strict';
app.controller('loginController', ['$state', '$scope', '$location', 'authService', function ($state, $scope, $location, authService) {

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {
                $state.go('dashboard');
            },
         function (err) {
             $scope.message = err.error_description;
         });
    };

}]);