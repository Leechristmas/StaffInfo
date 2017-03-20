'use strict';

app.controller('userController',
[
    '$scope', 'userService', '$mdToast', 'messageService', '$mdDialog', function($scope, userService, $mdToast, messageService, $mdDialog) {
        $scope.users = [];
        $scope.selected = null;
        $scope.selectedTabIndex = 0;

        $scope.selectUser = function(user)
        {
            $scope.selected = user;
            $scope.selected.isAdmin = user.roles.includes("admin") > 0;
            $scope.selected.isEditor = user.roles.includes("editor") > 0;
            $scope.selected.isReader = user.roles.includes("reader") > 0;
        }

        $scope.getUsers = function () {
            $scope.promise = userService.getUsers().then(function (response) {
                $scope.users = response.data;
            }, function (error) {
                messageService.errors.setError({ errorText: error.data, errorTitle: 'Статус - ' + error.status + ': ' + error.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //TODO
        $scope.isAdmin = function () {
            if ($scope.selected.isAdmin && !$scope.selected.roles.includes("admin"))
                $scope.selected.roles.push("admin");
            //else if ($scope.selected.isAdmin && !$scope.selected.roles.includes("admin"))
        }

        $scope.showUserRegistrationForm = function (ev) {
            $mdDialog.show({
                controller: 'registerUserController',
                templateUrl: 'app/views/registerUserView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            }).then(function (answer) {
                console.log('new user has been registered.');
            }, function () {
                console.log('registration view has been closed.');
            });
        }

        $scope.getUsers();
        
    }
]);