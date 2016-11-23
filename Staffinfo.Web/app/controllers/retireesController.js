app.controller('retireesController', [
    '$scope', 'employeesService', '$mdToast', 'messageService', '$mdDialog', '$state', function($scope, employeesService, $mdToast, messageService, $mdDialog, $state) {
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
        $scope.getRetirees = function() {
            $scope.promise = employeesService.getRetirees($scope.query).then(function(response) {
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
            return new Date(date);
        }

        $scope.retirees = $scope.getRetirees();
    }
]);