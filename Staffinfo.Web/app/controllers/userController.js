'use strict';

app.controller('userController',
[
    '$scope', 'userService', '$mdToast', 'messageService', '$mdDialog', 'authService', 'employeesService', '$state', function ($scope, userService, $mdToast, messageService, $mdDialog, authService, employeesService, $state) {
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

        //deletes the specified employee
        var _deleteAccount = function (id) {
            $scope.promise = userService.deleteAccount(id).then(function (response) {
                $scope.getUsers();//refresh
                $scope.selected = null;
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                    '<div class="md-toast-content">' +
                    'Учетные данные успешно удалены.' +
                    '</div>' +
                    '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //shows confirmation of employee deletion 
        $scope.confirmDeleting = function (ev, id) {
            var confirm = $mdDialog.confirm()
                .title('Удаление')
                .textContent('Вы уверены, что хотите удалить учетные данные сотрудника ' +
                    $scope.selected.lastname +
                    ' ' +
                    $scope.selected.firstname +
                    ' ' +
                    $scope.selected.middlename +
                    '?')
                .ariaLabel('Deleting')
                .targetEvent(ev)
                .ok('Удалить')
                .cancel('Отмена');
            $mdDialog.show(confirm).then(function () {
                //delete the employee
                _deleteAccount(id);
            }, function () {
                //cancel
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

        //opens the dialog window with detailed information about specified employee
        $scope.showDetails = function (ev, id) {
            employeesService.employees.getEmployeeById(id).then(function (response) {

                //TODO: set JSON parser for data - date is parsed not correct
                var employee = response.data;
                employee.birthDate = new Date(employee.birthDate);

                employeesService.employees.setActualEmployee(employee);

                $state.go('details');
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        };

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