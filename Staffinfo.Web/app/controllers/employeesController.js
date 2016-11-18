﻿'use strict';

app.controller('employeesController', [
    '$scope', 'employeesService', '$mdToast', 'messageService', '$mdDialog', '$state', function ($scope, employeesService, $mdToast, messageService, $mdDialog, $state) {
        //$scope.employees = [
        //{
        //    id: 1,
        //    employeeLastname: "Иванов",
        //    employeeFirstname: "Петр",
        //    employeeMiddlename: "Геннадьевич",
        //    actualPost: "Спасатель-водолаз",
        //    actualRank: "Ст. Сержант",
        //    birthDate: new Date(1989, 11, 1)
        //}, {
        //    id: 2,
        //    employeeLastname: "Иванов",
        //    employeeFirstname: "Петр",
        //    employeeMiddlename: "Геннадьевич",
        //    actualPost: "Спасатель-водолаз",
        //    actualRank: "Ст. Сержант",
        //    birthDate: new Date(1989, 11, 1)
        //}, {
        //    id: 3,
        //    employeeLastname: "Иванов",
        //    employeeFirstname: "Петр",
        //    employeeMiddlename: "Геннадьевич",
        //    actualPost: "Спасатель-водолаз",
        //    actualRank: "Ст. Сержант",
        //    birthDate: new Date(1989, 11, 1)
        //}, {
        //    id: 4,
        //    employeeLastname: "Иванов",
        //    employeeFirstname: "Петр",
        //    employeeMiddlename: "Геннадьевич",
        //    actualPost: "Спасатель-водолаз",
        //    actualRank: "Ст. Сержант",
        //    birthDate: new Date(1989, 11, 1)
        //}, {
        //    id: 5,
        //    employeeLastname: "Иванов",
        //    employeeFirstname: "Петр",
        //    employeeMiddlename: "Геннадьевич",
        //    actualPost: "Спасатель-водолаз",
        //    actualRank: "Ст. Сержант",
        //    birthDate: new Date(1989, 11, 1)
        //}, {
        //    id: 6,
        //    employeeLastname: "Иванов",
        //    employeeFirstname: "Петр",
        //    employeeMiddlename: "Геннадьевич",
        //    actualPost: "Спасатель-водолаз",
        //    actualRank: "Ст. Сержант",
        //    birthDate: new Date(1989, 11, 1)
        //}, {
        //    id: 7,
        //    employeeLastname: "Иванов",
        //    employeeFirstname: "Петр",
        //    employeeMiddlename: "Геннадьевич",
        //    actualPost: "Спасатель-водолаз",
        //    actualRank: "Ст. Сержант",
        //    birthDate: new Date(1989, 11, 1)
        //}];

        $scope.selected = [];

        //options for queries to API and pagination
        $scope.query = {
            order: 'employeeLastname',
            limit: 10,
            page: 1,
            label: {
                of: "из",
                page: 'Текущая',
                rowsPerPage: 'Кол-во на странице'
            },
            filter: ''
        };

        //returns employees with pagination
        $scope.getEmployees = function () {
            $scope.promise = employeesService.getEmployees($scope.query).then(function (response) {
                $scope.employees = response.data;
                $scope.total = response.headers('X-Total-Count');
            }, function (data) {
                messageService.setError(data);
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    templateUrl: 'app/views/error-toast.html'
                });
            });
        }

        //returns $promise with employee by id
        $scope.getEmployeeById = function (id) {
            return employeesService.getEmployeeById(id);
        };

        //opens the dialog window with detailed information about specified employee
        $scope.showDetails = function (ev, id) {
            $scope.getEmployeeById(id).then(function(response) {

                //var _employee = {
                //    id: 2,
                //    employeeLastname: "Иванов",
                //    employeeFirstname: "Петр",
                //    employeeMiddlename: "Геннадьевич",
                //    actualPost: "Спасатель-водолаз",
                //    actualRank: "Ст. Сержант",
                //    birthDate: new Date(1989, 11, 1),
                //    passportId: 1,
                //    passportNumber: 'HB2234598',
                //    passportOrganization: 'Гомельский РОВД Гомельской области',
                //    addressId: 1,
                //    city: 'Гомель',
                //    area: 'Гомельская',
                //    detailedAddress: 'ул. Советская, д.33 кв.99',
                //    zipCode: '247023'
                //};

                //TODO: set JSON parser for data
                var employee = response.data;
                employee.birthDate = new Date(employee.birthDate);

                employeesService.setActualEmployee(employee);

                $state.go('details');
            });
        };

        //returns date from string
        $scope.getDate = function (date) {
            return new Date(date);
        }

        //shows the window with form for adding new employee
        $scope.showAddingView = function (ev) {
            $mdDialog.show({
                controller: 'addEmployeeController',
                templateUrl: 'app/views/addEmployeeView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            })
                .then(function () {
                });
        }

        //deletes the specified employee
        var _deleteEmployee = function (id) {
            //TODO: deleting
            employeesService.deleteEmployeeById(id).then(function (response) {
                $scope.employees = $scope.getEmployees();
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      'Информация о сотруднике была удалена.' +
                                    '</div>' +
                                '</md-toast>'
                });
            },function(error) {
                messageService.setError(error);
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    templateUrl: 'app/views/error-toast.html'
                });
            });
        }

        //shows confirmation of employee deletion 
        $scope.confirmDeleting = function (ev, id) {
            var confirm = $mdDialog.confirm()
                    .title('Удаление')
                    .textContent('Вы уверены, что хотите удалить информацию о сотруднике? \nВосстановить утерянную информацию будет невозможно!')
                    .ariaLabel('Deleting')
                    .targetEvent(ev)
                    .ok('Удалить')
                    .cancel('Отмена');
            $mdDialog.show(confirm).then(function () {
                //delete the employee
                _deleteEmployee(id);
            }, function () {
                //cancel
            });
        }

        $scope.refreshEmployees = function()
        {
            $scope.employees = $scope.getEmployees();
        }

        $scope.employees = $scope.getEmployees();

    }]).controller('toastController', ['$scope', '$mdDialog', 'messageService', function ($scope, $mdDialog, messageService) {

        var isDlgOpen = false;

        $scope.closeToast = function () {
            if (isDlgOpen) return;

            $mdToast
                .hide()
                .then(function () {
                    isDlgOpen = false;
                });
        };

        $scope.openMoreInfo = function (e) {
            if (isDlgOpen) return;
            isDlgOpen = true;

            $mdDialog
                .show($mdDialog
                    .alert()
                    .title('Информация об ошибке')
                    .textContent(messageService.getError())
                    .ariaLabel('More info')
                    .ok('OK')
                    .targetEvent(e)
                )
                .then(function () {
                    isDlgOpen = false;
                });
        };

    }]).controller('detailsController', ['$scope', '$mdDialog', 'employeesService', '$timeout', '$mdToast', '$state', function ($scope, $mdDialog, employeesService, $timeout, $mdToast, $state) {
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.maxDate = employeesService.maxDate;

        $scope.showAddMesView = function (ev) {
            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addMesView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            });
        };

        $scope.showAddServiceTimeView = function (ev) {
            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addServiceTimeView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            });
        };

        //shows confirmation for transferring employee
        $scope.askForTransfer = function (type, ev) {
            var confirm = {};
            if (type === 'P') {
                confirm = $mdDialog.confirm()
                    .title('Перевод в "Пенсионеры"')
                    .textContent('Вы уверены, что хотите перенести информацию о сотруднике в базу данных "Пенсионеры"? \nВосстановить утерянную информацию будет невозможно!')
                    .ariaLabel('Transfer')
                    .targetEvent(ev)
                    .ok('Перевести')
                    .cancel('Отмена');
                $mdDialog.show(confirm).then(function () {
                    //transfer to pensioners
                }, function () {
                    //cancel
                });
            }
            else if (type === 'F') {
                confirm = $mdDialog.confirm()
                    .title('Перевод в "Уволенные"')
                    .textContent('Вы уверены, что хотите перенести информацию о сотруднике в базу данных "Уволенные"? \nВосстановить утерянную информацию будет невозможно!')
                    .ariaLabel('Transfer')
                    .targetEvent(ev)
                    .ok('Перевести')
                    .cancel('Отмена');
                $mdDialog.show(confirm).then(function () {
                    //transfer to fired
                }, function () {
                    //cancel
                });
            }
        };

        //save specified changes for the employee
        $scope.saveChanges = function () {
            //save the changes
            $state.go('employees');
            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                template: '<md-toast class="md-toast-success">' +
                                '<div class="md-toast-content text-center">' +
                                  'Изменения приняты.' +
                                '</div>' +
                            '</md-toast>'
            });
        }

        //reset all changes
        $scope.resetChanges = function () {
            $scope.changeable = employeesService.getClone($scope.employee);
        }

        $scope.mesAchievements = [];
        $scope.serviceAchievements = [];

        //returns MES achievements for employee
        $scope.getMesAchievements = function () {
            $scope.mesAchievements = [
                {
                    startDate: new Date(2001, 12, 12),
                    finishDate: new Date(2002, 12, 12),
                    locationId: 1,
                    location: "location1",
                    rankId: 1,
                    rank: 'rank1',
                    postId: 1,
                    post: 'post1'
                }, {
                    startDate: new Date(2003, 12, 12),
                    finishDate: new Date(2004, 12, 12),
                    locationId: 1,
                    location: "location1",
                    rankId: 1,
                    rank: 'rank1',
                    postId: 1,
                    post: 'post1'
                }
            ];
            $scope.promise = $timeout(function () {
                // ...
            }, 2000);
        }

        //returns service achievements for employee
        $scope.getServiceAchievements = function () {
            $scope.serviceAchievements = [
                {
                    startDate: new Date(2001, 12, 12),
                    finishDate: new Date(2002, 12, 12),
                    locationId: 1,
                    location: "location1",
                    rankId: 1,
                    rank: 'rank1'
                }, {
                    startDate: new Date(2003, 12, 12),
                    finishDate: new Date(2004, 12, 12),
                    locationId: 1,
                    location: "location1",
                    rankId: 1,
                    rank: 'rank1'
                }
            ];
            $scope.promise = $timeout(function () {
                // ...
            }, 2000);
        }

        $scope.employee = employeesService.getActualEmployee();

        $scope.changeable = employeesService.getClone($scope.employee);

    }]).controller('addEmployeeItemsController', ['$scope', '$mdDialog', 'employeesService', function ($scope, $mdDialog, employeesService) {
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.mesAchItem = {};

        $scope.locations = [
            { id: 1, name: 'location_1' },
            { id: 2, name: 'location_2' },
            { id: 3, name: 'location_3' }
        ];

        $scope.ranks = [
            { id: 1, name: 'rank_1' },
            { id: 2, name: 'rank_2' },
            { id: 3, name: 'rank_3' }
        ];

        $scope.posts = [
            { id: 1, name: 'post_1' },
            { id: 2, name: 'post_2' },
            { id: 3, name: 'post_3' }
        ];

    }]).controller('addEmployeeController', ['$scope', '$mdDialog', 'employeesService', '$mdToast', function ($scope, $mdDialog, employeesService, $mdToast) {

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.maxDate = employeesService.maxDate;

        var employee = {    //testing
            employeeLastname: "Тестовый",
            employeeFirstname: "Тест",
            employeeMiddlename: "Тестович",
            birthDate: new Date(1989, 11, 1),
            description: 'тестовое описание',
            detailedAddress: 'тестовый точный адрес',
            city: 'тестовый город',
            area: 'тестовая область',
            zipCode: '123456',
            passportNumber: 'HB1234567',
            passportOrganization: 'тестовая организация',
            firstPhone: 'первый номер',
            secondPhone: 'второй номер'
        }

        $scope.newEmployee = employee;

        $scope.saveNewEmployee = function () {
            //ToDo: 
            var t = employeesService.addNewEmployee($scope.newEmployee);
            $scope.cancel();

            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                template: '<md-toast class="md-toast-success">' +
                                '<div class="md-toast-content">' +
                                  'Сотрудник успешно зарегистрирован.' +
                                '</div>' +
                            '</md-toast>'
            });
        };

    }]);