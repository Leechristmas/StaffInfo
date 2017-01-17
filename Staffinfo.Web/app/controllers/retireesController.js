app.controller('retireesController', [
    '$scope', 'employeesService', '$mdToast', 'messageService', '$mdDialog', '$state', function ($scope, employeesService, $mdToast, messageService, $mdDialog, $state) {
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

        //gets retirees with pagination 
        $scope.getRetirees = function () {
            $scope.promise = employeesService.retirees.getRetirees($scope.query).then(function (response) {
                $scope.retirees = response.data;
                $scope.total = response.headers('X-Total-Count');
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //opens the dialog window with detailed information about specified employee
        $scope.showDetails = function (ev, id) {
            $scope.getEmployeeById(id).then(function (response) {

                //TODO: set JSON parser for data
                var employee = response.data;
                employee.birthDate = new Date(employee.birthDate);
                employee.retirementDate = new Date(employee.retirementDate);

                employeesService.employees.setActualEmployee(employee);

                $state.go('details');
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        };

        //returns $promise with employee by id
        $scope.getEmployeeById = function (id) {
            return employeesService.employees.getEmployeeById(id);
        };

        //returns date from string
        $scope.getDate = function (date) {
            var t = new Date(date);
            return new Date(t.getFullYear(), t.getMonth(), t.getDate(), 3);
        }

        //deletes the specified employee
        var _deleteEmployee = function (id) {
            //TODO: deleting
            $scope.promise = employeesService.employees.deleteEmployeeById(id).then(function (response) {
                $scope.getRetirees();//refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      'Информация о пенсионере была удалена.' +
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
                    .textContent('Вы уверены, что хотите удалить информацию об указанном пенсионере? \nВосстановить утерянную информацию будет невозможно!')
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

        $scope.retirees = $scope.getRetirees();
    }
])
.controller('transferController', ['$scope', 'employeesService', 'messageService', '$mdDialog', '$mdToast', '$state', '$rootScope', function ($scope, employeesService, messageService, $mdDialog, $mdToast, $state, $rootScope) {
    $scope.hide = function () {
        $mdDialog.hide();
    };

    $scope.cancel = function () {
        $mdDialog.cancel();
    };

    $scope.answer = function (answer) {
        $mdDialog.hide(answer);
    };

    $scope.dtpckrOnFocus = function (item, field) {//when ditetimepicker is focused and the model field is undefined
        if (!item[field])
            item[field] = new Date();
    }

    $scope.employee = employeesService.common.getClone(employeesService.employees.getActualEmployee());
    $scope.dismissal = {};

    $scope.transferToDismissed = function () {
        $scope.dismissal.employeeId = $scope.employee.id;
        $scope.promise = employeesService.employees.transferToDismissed($scope.dismissal).then(function (response) {//transfer to dismissed
            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                template: '<md-toast class="md-toast-success">' +
                                '<div class="md-toast-content">' +
                                  'Операция проведена успешно.' +
                                '</div>' +
                            '</md-toast>'
            });
            $scope.hide();
            $state.go('employees');
        }, function (data) {
            messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
            $mdToast.show(messageService.errors.errorViewConfig);
        });
    }

    $scope.transferToRetirees = function () {
        $scope.promise = employeesService.employees.transferToRetirees($scope.employee).then(function (response) {//transfer to pensioners
            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                template: '<md-toast class="md-toast-success">' +
                                '<div class="md-toast-content">' +
                                  'Операция проведена успешно.' +
                                '</div>' +
                            '</md-toast>'
            });
            $scope.hide();
            $state.go('employees');
        }, function (data) {
            messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
            $mdToast.show(messageService.errors.errorViewConfig);
        });
    }
}]);