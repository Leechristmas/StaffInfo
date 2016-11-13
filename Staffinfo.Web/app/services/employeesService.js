﻿'use strict';

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

        //returns employee by id
        var _getEmployeeById = function(id) {
            return $http.get(serviceBase + 'api/employees/' + id).then(function (response) {
                return response;
            });
        }

        //returns clone of the specified object
        var _getClone = function clone(obj) {
            if (null == obj || "object" != typeof obj) return obj;
            var copy = obj.constructor();
            for (var attr in obj) {
                if (obj.hasOwnProperty(attr)) copy[attr] = obj[attr];
            }
            return copy;
        }

        //actual selected employee
        var _actualEmployee = {};
        
        var  _getActualEmployee = function() {
            return _actualEmployee;
        }

        var _setActualEmployee = function(employee) {
            _actualEmployee = employee;
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

        employeesServiceFactory.getClone = _getClone;
        employeesServiceFactory.getEmployeeById = _getEmployeeById;
        employeesServiceFactory.getActualEmployee = _getActualEmployee;
        employeesServiceFactory.setActualEmployee = _setActualEmployee;
        employeesServiceFactory.getEmployees = _getEmployees;
        employeesServiceFactory.testPost = _testPost;

        return employeesServiceFactory;
    }
]);