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
        
        //deletes employee by id
        var _deleteEmployeeById = function(id) {
            return $http.delete(serviceBase + 'api/employees/' + id);
        }

        //deletes work by id
        var _deleteWork = function(id) {
            return $http.delete(serviceBase + 'api/employees/works/' + id);
        }

        //deletes military by id
        var _deleteMilitary = function (id) {
            return $http.delete(serviceBase + 'api/employees/military/' + id);
        }

        //deletes mes achievement by id
        var _deleteMesAchievement = function (id) {
            return $http.delete(serviceBase + 'api/employees/mesachievements/' + id);
        }

        //returns employee by id
        var _getEmployeeById = function(id) {
            return $http.get(serviceBase + 'api/employees/' + id).then(function (response) {
                return response;
            });
        }

        //adds new employee
        var _addNewEmployee = function (employee) {
            return $http.post(serviceBase + 'api/employees', employee, {});
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

        var _saveChanges = function(employee) {
            return $http.put(serviceBase + 'api/employees/' + employee.id, employee, {});
        }

        //actual selected employee
        var _actualEmployee = {};
        
        var  _getActualEmployee = function() {
            return _actualEmployee;
        }

        var _setActualEmployee = function(employee) {
            _actualEmployee = employee;
        }

        //returns promise for getting mes achievements
        var _getMesAchievements = function() {
            return $http.get(serviceBase + 'api/employees/mesachievements/' + _actualEmployee.id);
        }

        //returns promise for getting military
        var _getMilitary = function() {
            return $http.get(serviceBase + 'api/employees/military/' + _actualEmployee.id);
        }

        //returns promise for getting military
        var _getWorks = function () {
            return $http.get(serviceBase + 'api/employees/works/' + _actualEmployee.id);
        }

        //returns promise for getting ranks
        var _getRanks = function() {
            return $http.get(serviceBase + 'api/employees/ranks');
        }

        //returns promise for getting ranks
        var _getPosts = function () {
            return $http.get(serviceBase + 'api/employees/posts');
        }
        
        //returns promise for getting locations
        var _getLocations = function() {
            return $http.get(serviceBase + 'api/employees/locations');
        }

        //max birthDate
        var _now = new Date();
        var _maxDate = new Date(_now.getFullYear() - 18, _now.getMonth(), _now.getDate());

        //returns promise for getting mes achievements
        var _saveMesAchievement = function(item) {
            return $http.post(serviceBase + 'api/employees/mesachievements', item, {});
        }

        employeesServiceFactory.saveMesAchievement = _saveMesAchievement;
        employeesServiceFactory.getLocations = _getLocations;
        employeesServiceFactory.getRanks = _getRanks;
        employeesServiceFactory.getPosts = _getPosts;
        employeesServiceFactory.deleteMesAchievement = _deleteMesAchievement;
        employeesServiceFactory.deleteMilitary = _deleteMilitary;
        employeesServiceFactory.deleteWork = _deleteWork;
        employeesServiceFactory.getWorks = _getWorks;
        employeesServiceFactory.getMilitary = _getMilitary;
        employeesServiceFactory.getMesAchievements = _getMesAchievements;
        employeesServiceFactory.maxDate = _maxDate;
        employeesServiceFactory.getClone = _getClone;
        employeesServiceFactory.getEmployeeById = _getEmployeeById;
        employeesServiceFactory.getActualEmployee = _getActualEmployee;
        employeesServiceFactory.setActualEmployee = _setActualEmployee;
        employeesServiceFactory.getEmployees = _getEmployees;
        employeesServiceFactory.addNewEmployee = _addNewEmployee;
        employeesServiceFactory.deleteEmployeeById = _deleteEmployeeById;
        employeesServiceFactory.saveChanges = _saveChanges;

        return employeesServiceFactory;
    }
]);