'use strict';

app.factory('employeesService', [
    "$http", 'ngAuthSettings', function ($http, ngAuthSettings) {
        var employeesServiceFactory = {};
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var _getEmployees = function () {
            return $http.get(serviceBase + 'api/employees').then(function (results) {
                return results;
            });
        }

        var data = {
            Id: 4,
            employeeLastname: "Иванов",
            employeeFirstname: "Петр",
            employeeMiddlename: "Геннадьевич",
            actualPost: "Спасатель-водолаз",
            actualRank: "Ст. Сержант",
            birthDate: new Date(1989, 11, 1).toISOString().slice(0, 10)
        };

        var config = {};

        var _testPost = function() {
            $http.post(serviceBase + 'api/employees', data, config)
            .success(function (data, status, headers, config) {
                //$scope.PostDataResponse = data;
                alert("success");
            })
            .error(function (data, status, header, config) {
                $scope.ResponseDetails = "Data: " + data +
                    "<hr />status: " + status +
                    "<hr />headers: " + header +
                    "<hr />config: " + config;
            });
        }

        employeesServiceFactory.getEmployees = _getEmployees;
        employeesServiceFactory.testPost = _testPost;

        return employeesServiceFactory;
    }
]);