'use strict';

app.controller('userController',
    [
        '$scope', 'userService', '$mdToast', 'messageService', '$mdDialog', 'authService', 'employeesService', '$state', function ($scope, userService, $mdToast, messageService, $mdDialog, authService, employeesService, $state) {
            $scope.users = [];
            $scope.selected = null;
            $scope.selectedTabIndex = 0;
            $scope.authorizedUser = authService.authentication;

            $scope.selectUser = function (user) {
                $scope.selected = user;
                $scope.selected.isadmin = user.roles.includes("admin") > 0;
                $scope.selected.iseditor = user.roles.includes("editor") > 0;
                $scope.selected.isreader = user.roles.includes("reader") > 0;
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

            $scope.setPermission = function (userId, permission) {

                if ($scope.selected['is' + permission])
                    $scope.promise = userService.removePermission(userId, permission).then(function (response) {
                        $scope.selected['is' + permission] = false;
                        $mdToast.show({
                            hideDelay: 3000,
                            position: 'top right',
                            controller: 'toastController',
                            template: '<md-toast class="md-toast-success">' +
                            '<div class="md-toast-content">' +
                            'Права пользователя были успешно удалены.' +
                            '</div>' +
                            '</md-toast>'
                        });
                    }, function (data) {
                        messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                        $mdToast.show(messageService.errors.errorViewConfig);
                    });
                else
                    $scope.promise = userService.addPermission(userId, permission).then(function (response) {
                        $scope.selected['is' + permission] = true;
                        $mdToast.show({
                            hideDelay: 3000,
                            position: 'top right',
                            controller: 'toastController',
                            template: '<md-toast class="md-toast-success">' +
                            '<div class="md-toast-content">' +
                            'Права были успешно добавлены пользователю.' +
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
                }, function () {
                    console.log('registration view has been closed.');
                    $scope.getUsers();//update users list
                });
            }

            $scope.showChangePasswordView = function (ev) {
                $mdDialog.show({
                    controller: 'changePasswordController',
                    templateUrl: 'app/views/changePasswordView.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    locals: {
                        userId: $scope.selected.id
                    },
                    clickOutsideToClose: true
                }).then(function (answer) {
                    console.log('new employee has been added.');
                }, function () {
                    console.log('adding view has been closed.');
                });
            }

            $scope.getUsers();

        }
    ]);