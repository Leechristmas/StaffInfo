'use strict';

app.controller('employeesController', ['$scope', 'employeesService', '$mdToast', '$q', function ($scope, employeesService, $mdToast, $q) {
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
        var defered = $q.defer();
        $scope.promise = defered.promise;

        employeesService.getEmployees($scope.query).then(function (response) {
            $scope.employees = response.data;
            $scope.total = response.headers('X-Total-Count');
        });

        defered.resolve();
    }

    $scope.getDate = function (date){
        return new Date(date);
    }

    $scope.employees = $scope.getEmployees();
    
}]);