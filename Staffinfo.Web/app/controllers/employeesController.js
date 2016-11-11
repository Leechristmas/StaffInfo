'use strict';

app.controller('employeesController', ['$scope', 'employeesService', '$mdToast', function ($scope, employeesService, $mdToast) {
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

    $scope.testPost = function() {
        employeesService.testPost();
    }

}]);