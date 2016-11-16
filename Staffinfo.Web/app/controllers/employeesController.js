'use strict';

app.controller('employeesController', [
    '$scope', 'employeesService', '$mdToast', 'messageService', '$mdDialog', function ($scope, employeesService, $mdToast, messageService, $mdDialog) {
        $scope.employees = [
        {
            id: 1,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1)
        }, {
            id: 2,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1)
        }, {
            id: 3,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1)
        }, {
            id: 4,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1)
        }, {
            id: 5,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1)
        }, {
            id: 6,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1)
        }, {
            id: 7,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1)
        }];

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
            }
        };

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
            //$scope.getEmployeeById(id).then(function (response) {
            //employeesService.setActualEmployee(response.data);

            $mdDialog.show({
                controller: 'detailsController',
                templateUrl: 'app/views/employeeDetails.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            })
            .then(function (answer) {
                $scope.status = 'You said the information was "' + answer + '".';
            }, function () {
                $scope.status = 'You cancelled the dialog.';
            });

        }, function (data) {
            messageService.setError("Failed. " + data);
            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                templateUrl: 'app/views/error-toast.html'
            });
            //});


        };

        //returns date from string
        $scope.getDate = function (date) {
            return new Date(date);
        }

        $scope.showAddingView = function (ev) {
            $mdDialog.show({
                controller: 'addEmployeeController',
                templateUrl: 'app/views/addEmployee.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            })
                .then(function () {
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
                });
        }
        //$scope.employees = $scope.getEmployees();

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

    }]).controller('detailsController', ['$scope', '$mdDialog', 'employeesService', '$timeout', '$mdToast', function ($scope, $mdDialog, employeesService, $timeout, $mdToast) {
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
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
            $mdDialog.cancel();
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

        //$scope.employee = employeesService.getActualEmployee();
        $scope.employee = {
            id: 2,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1),
            passportId: 1,
            passportNumber: 'HB2234598',
            passportOrganization: 'Гомельский РОВД Гомельской области',
            addressId: 1,
            city: 'Гомель',
            area: 'Гомельская',
            detailedAddress: 'ул. Советская, д.33 кв.99',
            zipCode: '247023'
        };

        $scope.changeable = employeesService.getClone($scope.employee);

    }]).controller('addEmployeeController', ['$scope', '$mdDialog', 'employeesService', function ($scope, $mdDialog, employeesService) {

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };

        $scope.newEmployee = {};

        $scope.saveNewEmployee = function () {
            //ToDo: 
        };

    }]);