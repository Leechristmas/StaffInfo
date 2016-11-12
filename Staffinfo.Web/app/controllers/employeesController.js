'use strict';

app.controller('employeesController', ['$scope', 'employeesService', '$mdToast', 'messageService', function ($scope, employeesService, $mdToast, messageService) {
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
            })
        });
    }

    $scope.getDate = function (date){
        return new Date(date);
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
}]);