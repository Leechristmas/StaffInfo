'use strict';

app.factory('employeesService', [
    "$http", 'ngAuthSettings', function ($http, ngAuthSettings) {
        var employeesServiceFactory = {};

        var _disciplineItems = {
            actualDisciplineItemsType: ''
        };

        //base address to API
        var serviceBase = ngAuthSettings.apiServiceBaseUri;


        //returns actual employees with pagination 
        var _getEmployees = function (query) {
            return $http.get(serviceBase + 'api/employees?offset=' + (query.page-1)*query.limit + '&limit=' + query.limit + '&query=' + (query.filter ? query.filter : ''));
        }

        //returns promise for getting retirees
        var _getRetirees = function (query) {
            return $http.get(serviceBase + 'api/retirees?offset=' + (query.page - 1) * query.limit + '&limit=' + query.limit + '&query=' + (query.filter ? query.filter : ''));
        }

        //returns promise for getting dismissed
        var _getDismissed = function(query) {
            return $http.get(serviceBase + 'api/dismissed?offset=' + (query.page - 1) * query.limit + '&limit=' + query.limit + '&query=' + (query.filter ? query.filter : ''));
        }

        //deletes employee by id
        var _deleteEmployeeById = function(id) {
            return $http.delete(serviceBase + 'api/employees/' + id);
        }

        //deletes dismissed by id
        var _deleteDismissedById = function (id) {
            return $http.delete(serviceBase + 'api/dismissed/' + id);
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

        //returns promise for getting services
        var _getServices = function() {
            return $http.get(serviceBase + 'api/employees/services');
        }

        //returns promise for getting ranks
        var _getPosts = function (serviceId) {
            if (serviceId)
                return $http.get(serviceBase + 'api/employees/postsforservice/' + serviceId);
            else
                return $http.get(serviceBase + 'api/employees/posts');
        }
        
        //returns promise for getting locations
        var _getLocations = function() {
            return $http.get(serviceBase + 'api/employees/locations');
        }

        //max birthDate
        var _now = new Date();
        var _maxDate = new Date(_now.getFullYear() - 18, _now.getMonth(), _now.getDate());

        //returns promise for adding mes achievements
        var _saveMesAchievement = function(item) {
            return $http.post(serviceBase + 'api/employees/mesachievements', item, {});
        }

        //returns promise for adding military
        var _saveMilitary = function(item) {
            return $http.post(serviceBase + 'api/employees/military', item, {});
        }

        //returns promise for adding work
        var _saveWork = function(item) {
            return $http.post(serviceBase + 'api/employees/works', item, {});
        }

        //returns promise for transferring the employee to retired
        var _transferToRetirees = function(employee) {
            return $http.post(serviceBase + 'api/employees/retiredtransfer', employee, {});
        }

        //returns promise for transferring the employee to retired
        var _transferToDismissed = function(dismissal) {
            return $http.post(serviceBase + 'api/employees/dismissedtransfer', dismissal, {});
        }

        //returns promise for getting seniority by employee id
        var _getSeniorityById = function(employeeId) {
            return $http.get(serviceBase + 'api/employees/seniority/' + employeeId);
        }

        //returns promise for getting discipline items by employee id
        var _getDisciplineItems = function(employeeId) {
            return $http.get(serviceBase + 'api/employees/discipline/' + employeeId);
        }

        //returns promise for deleting discipline item by id
        var _deleteDisciplineItem = function(id) {
            return $http.delete(serviceBase + 'api/employees/discipline/' + id);
        }

        //returns promise for saving new discipline item
        var _saveNewDisciplineItem = function (disciplineItem) {
            return $http.post(serviceBase + 'api/employees/discipline', disciplineItem, {});
        }

        employeesServiceFactory.DisciplineItems = _disciplineItems;

        employeesServiceFactory.saveNewDisciplineItem = _saveNewDisciplineItem;
        employeesServiceFactory.deleteDisciplineItem = _deleteDisciplineItem;
        employeesServiceFactory.getDisciplineItems = _getDisciplineItems;
        employeesServiceFactory.getServices = _getServices;
        employeesServiceFactory.getSeniorityById = _getSeniorityById;
        employeesServiceFactory.deleteDismissedById = _deleteDismissedById;
        employeesServiceFactory.getDismissed = _getDismissed;
        employeesServiceFactory.trasnferToDismissed = _transferToDismissed;
        employeesServiceFactory.transferToRetirees = _transferToRetirees;
        employeesServiceFactory.getRetirees = _getRetirees;
        employeesServiceFactory.saveWork = _saveWork;
        employeesServiceFactory.saveMilitary = _saveMilitary;
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