'use strict';

app.controller('employeesController', ['$scope', 'employeesService', '$mdToast', function ($scope, employeesService, $mdToast) {
    $scope.employees = [
    {
        employeeLastname: "Иванов",
        employeeFirstname: "Петр",
        employeeMiddlename: "Геннадьевич",
        actualPost: "Спасатель-водолаз",
        actualRank: "Ст. Сержант",
        birthDate: new Date(1989, 11, 1).toISOString().slice(0,10)
}, {
        employeeLastname: "Иванов",
        employeeFirstname: "Петр",
        employeeMiddlename: "Геннадьевич",
        actualPost: "Спасатель-водолаз",
        actualRank: "Ст. Сержант",
        birthDate: new Date(1989, 11, 1).toISOString().slice(0, 10)
    }, {
        employeeLastname: "Иванов",
        employeeFirstname: "Петр",
        employeeMiddlename: "Геннадьевич",
        actualPost: "Спасатель-водолаз",
        actualRank: "Ст. Сержант",
        birthDate: new Date(1989, 11, 1).toISOString().slice(0, 10)
    }, {
        employeeLastname: "Иванов",
        employeeFirstname: "Петр",
        employeeMiddlename: "Геннадьевич",
        actualPost: "Спасатель-водолаз",
        actualRank: "Ст. Сержант",
        birthDate: new Date(1989, 11, 1).toISOString().slice(0, 10)
    }, {
        employeeLastname: "Иванов",
        employeeFirstname: "Петр",
        employeeMiddlename: "Геннадьевич",
        actualPost: "Спасатель-водолаз",
        actualRank: "Ст. Сержант",
        birthDate: new Date(1989, 11, 1).toISOString().slice(0, 10)
    }, {
        employeeLastname: "Иванов",
        employeeFirstname: "Петр",
        employeeMiddlename: "Геннадьевич",
        actualPost: "Спасатель-водолаз",
        actualRank: "Ст. Сержант",
        birthDate: new Date(1989, 11, 1).toISOString().slice(0, 10)
    }, {
        employeeLastname: "Иванов",
        employeeFirstname: "Петр",
        employeeMiddlename: "Геннадьевич",
        actualPost: "Спасатель-водолаз",
        actualRank: "Ст. Сержант",
        birthDate: new Date(1989, 11, 1).toISOString().slice(0, 10)
    }];
}]);