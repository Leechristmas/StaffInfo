'use strict';
app.controller('loginController', ['$state', '$scope', '$mdToast', 'authService', function ($state, $scope, $mdToast, authService) {

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {
            $state.go('dashboard');

            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                template: '<md-toast class="md-toast-success">' +
                                '<div class="md-toast-content">' +
                                  'Вы вошли в систему.' +
                                '</div>' +
                            '</md-toast>'
            });
            },
         function (err) {
             $scope.message = err.error_description;
         });
    };

}]);