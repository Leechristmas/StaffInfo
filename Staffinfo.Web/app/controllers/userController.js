'use strict';

app.controller('userController',
[
    '$scope', 'userService', '$mdToast', 'messageService', '$mdDialog', 'authService', function ($scope, userService, $mdToast, messageService, $mdDialog, authService) {
        $scope.users = [];
        $scope.selected = null;
        $scope.selectedTabIndex = 0;
        $scope.authorizedUser = authService.authentication;

        $scope.selectUser = function(user)
        {
            $scope.selected = user;
            $scope.selected.isAdmin = user.roles.includes("admin") > 0;
            $scope.selected.isEditor = user.roles.includes("editor") > 0;
            $scope.selected.isReader = user.roles.includes("reader") > 0;
        }

        $scope.getUsers = function () {
            $scope.isLoading = true;
            $scope.promise = userService.getUsers().then(function (response) {
                $scope.users = response.data;
                $scope.isLoading = false;
            }, function (error) {
                messageService.errors.setError({ errorText: error.data, errorTitle: 'Статус - ' + error.status + ': ' + error.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
                $scope.isLoading = false;
            });
        }

        ////TODO
        //$scope.isAdmin = function () {
        //    var currentUserRoles = authService.authentication.roles;

        //    return currentUserRoles.includes("admin");

        //    //if ($scope.selected.isAdmin && !$scope.selected.roles.includes("admin"))
        //    //    $scope.selected.roles.push("admin");
        //    //else if ($scope.selected.isAdmin && !$scope.selected.roles.includes("admin"))
        //}



        $scope.showUserRegistrationForm = function (ev) {
            $mdDialog.show({
                controller: 'registerUserController',
                templateUrl: 'app/views/registerUserView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            }).then(function (answer) {
                console.log('new user has been registered.');
                $scope.getUsers();//update users list
            }, function () {
                console.log('registration view has been closed.');
            });
        }

        $scope.getUsers();
        
    }
]);