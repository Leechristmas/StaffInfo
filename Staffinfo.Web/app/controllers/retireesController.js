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
            $scope.promise = employeesService.getRetirees($scope.query).then(function (response) {
                $scope.retirees = response.data;
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

        //opens the dialog window with detailed information about specified employee
        $scope.showDetails = function (ev, id) {
            $scope.getEmployeeById(id).then(function (response) {

                //TODO: set JSON parser for data
                var employee = response.data;
                employee.birthDate = new Date(employee.birthDate);
                employee.retirementDate = new Date(employee.retirementDate);

                employeesService.setActualEmployee(employee);

                $state.go('details');
            }, function (data) {
                messageService.setError(data);
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    templateUrl: 'app/views/error-toast.html'
                });
            });
        };

        //returns $promise with employee by id
        $scope.getEmployeeById = function (id) {
            return employeesService.getEmployeeById(id);
        };

        //returns date from string
        $scope.getDate = function (date) {
            var t = new Date(date);
            return new Date(t.getFullYear(), t.getMonth(), t.getDate(), 3);
        }

        //deletes the specified employee
        var _deleteEmployee = function (id) {
            //TODO: deleting
            $scope.promise = employeesService.deleteEmployeeById(id).then(function (response) {
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
            }, function (error) {
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
.controller('transferController', ['$scope', 'employeesService', 'messageService', '$mdDialog', '$mdToast', '$state', function ($scope, employeesService, messageService, $mdDialog, $mdToast, $state) {
    $scope.hide = function () {
        $mdDialog.hide();
    };

    $scope.cancel = function () {
        $mdDialog.cancel();
    };

    $scope.answer = function (answer) {
        $mdDialog.hide(answer);
    };

    $scope.employee = employeesService.getClone(employeesService.getActualEmployee());
    $scope.dismissal = {};

    $scope.transferToDismissed = function () {
        $scope.dismissal.employeeId = $scope.employee.id;
        $scope.promise = employeesService.trasnferToDismissed($scope.dismissal).then(function (response) {
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
        }, function (error) {
            messageService.setError(error);
            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                templateUrl: 'app/views/error-toast.html'
            });
        });
        $scope.hide();
        $state.go('employees');
    }

    $scope.transferToRetirees = function () {
        $scope.promise = employeesService.transferToRetirees($scope.employee).then(function (response) {//transfer to pensioners
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
        }, function (error) {
            messageService.setError(error);
            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                templateUrl: 'app/views/error-toast.html'
            });
        });
        $scope.hide();
        $state.go('employees');
    }
}]);