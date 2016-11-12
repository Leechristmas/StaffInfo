'use strict';

app.factory('employeesService', [
    "$http", 'ngAuthSettings', function ($http, ngAuthSettings) {
        var employeesServiceFactory = {};

        //base address to API
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        //returns actual employees with pagination 
        var _getEmployees = function (query) {
            return $http.get(serviceBase + 'api/employees?offset=' + (query.page-1)*query.limit + '&limit=' + query.limit).then(function (response) {
                return response;
            });
        }

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