'use strict';

app.factory('employeesService', [
    "$http", 'ngAuthSettings', 'localStorageService', function ($http, ngAuthSettings, localStorageService) {
        var employeesServiceFactory = {};

        //base address to API
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        //retirees properties and methods
        var _retirees = {
            //returns promise for getting retirees
            getRetirees: function (query) {
                return $http.get(serviceBase + 'api/retirees?offset=' + (query.page - 1) * query.limit + '&limit=' + query.limit + '&query=' + (query.filter ? query.filter : ''));
            }
        }

        //dismissed properties and methods
        var _dismissed = {
            //returns promise for getting dismissed
            getDismissed: function (query) {
                return $http.get(serviceBase + 'api/dismissed?offset=' + (query.page - 1) * query.limit + '&limit=' + query.limit + '&query=' + (query.filter ? query.filter : ''));
            },
            //deletes dismissed by id
            deleteDismissedById: function (id) {
                return $http.delete(serviceBase + 'api/dismissed/' + id);
            },
        }

        //employee properties and methods
        var _employees = {
            //actual selected employee
            actualEmployee: {},
            //returns actual employees with pagination 
            getEmployees: function (query) {
                return $http.get(serviceBase + 'api/employees?offset=' + (query.page - 1) * query.limit + '&limit=' + query.limit + '&query=' + (query.filter ? query.filter : ''));
            },
            //deletes employee by id
            deleteEmployeeById: function (id) {
                return $http.delete(serviceBase + 'api/employees/' + id);
            },
            //returns employee by id
            getEmployeeById: function (id) {
                return $http.get(serviceBase + 'api/employees/' + id).then(function (response) {
                    return response;
                });
            },
            //adds new employee
            addNewEmployee: function (employee) {
                return $http.post(serviceBase + 'api/employees', employee, {});
            },
            //edit employee
            saveChanges: function (employee) {
                return $http.put(serviceBase + 'api/employees/' + employee.id, employee, {});
            },
            setActualEmployee: function (employee) {
                localStorageService.set('actualEmployee', employee);
                this.actualEmployee = employee;
            },
            //returns actual(selected) employee
            getActualEmployee: function () {//
                if (!angular.equals(this.actualEmployee, {}))
                    return this.actualEmployee;
                else {
                    var empl = localStorageService.get('actualEmployee');
                    if (empl && !angular.equals(empl, {})) {
                        this.setActualEmployee(empl);
                        empl.birthDate = new Date(empl.birthDate);
                        return empl;
                    } else
                        return null; //TODO: redirect to the error page
                }
            },
            transferToRetirees: function (employee) {
                return $http.post(serviceBase + 'api/employees/retiredtransfer', employee, {});
            },
            transferToDismissed: function (dismissal) {
                return $http.post(serviceBase + 'api/employees/dismissedtransfer', dismissal, {});
            },
            //getting seniority by employee id
            getSeniorityById: function (employeeId) {
                return $http.get(serviceBase + 'api/employees/seniority/' + employeeId);
            }
        }

        //common properties and methods
        var _common = {
            //returns clone of the specified object
            getClone: function clone(obj) {
                if (null == obj || "object" != typeof obj) return obj;
                var copy = obj.constructor();
                for (var attr in obj) {
                    if (obj.hasOwnProperty(attr)) copy[attr] = obj[attr];
                }
                return copy;
            },
            //max/min birthDate
            maxBirthDate: new Date(new Date(Date.now()).getFullYear() - 18, new Date(Date.now()).getMonth(), new Date(Date.now()).getDate()),
            minBirthDate: new Date(new Date(Date.now()).getFullYear() - 60, new Date(Date.now()).getMonth(), new Date(Date.now()).getDate())
        }

        //activity items (locations, ranks, posts, works, etc.) properties and methods
        var _activityItems = {
            works: {
                selectedWork: null,
                saveWork: function (item) {
                    if (item.id)//update
                        return $http.put(serviceBase + 'api/employees/activity/work/' + item.id, item, {});
                    return $http.post(serviceBase + 'api/employees/activity/work', item, {});
                },
                deleteWork: function (id) {
                    return $http.delete(serviceBase + 'api/employees/activity/work/' + id);
                },
                getWorks: function () {
                    return $http.get(serviceBase + 'api/employees/activity/work/' + _employees.actualEmployee.id);
                }
            },
            military: {
                selectedMilitary: null,
                getMilitary: function () {
                    return $http.get(serviceBase + 'api/employees/activity/military/' + _employees.actualEmployee.id);
                },
                saveMilitary: function (item) {
                    if (item.id)//update
                        return $http.put(serviceBase + 'api/employees/activity/military/' + item.id, item, {});
                    return $http.post(serviceBase + 'api/employees/activity/military', item, {});//save a new item
                },
                deleteMilitary: function (id) {
                    return $http.delete(serviceBase + 'api/employees/activity/military/' + id);
                }
            },
            mesAchievements: {
                selectedMesAchievement: null,
                getMesAchievements: function () {
                    return $http.get(serviceBase + 'api/employees/activity/mesachievements/' + _employees.actualEmployee.id);
                },
                saveMesAchievement: function (item) {
                    if (item.id)//update
                        return $http.put(serviceBase + 'api/employees/activity/mesachievements/' + item.id, item, {});
                    return $http.post(serviceBase + 'api/employees/activity/mesachievements', item, {});
                },
                deleteMesAchievement: function (id) {
                    return $http.delete(serviceBase + 'api/employees/activity/mesachievements/' + id);
                }
            },
            getRanks: function () {
                return $http.get(serviceBase + 'api/employees/ranks');
            },
            getPosts: function (serviceId) {
                if (serviceId)
                    return $http.get(serviceBase + 'api/employees/postsforservice/' + serviceId);
                else
                    return $http.get(serviceBase + 'api/employees/posts');
            },
            getServices: function () {
                return $http.get(serviceBase + 'api/employees/services');
            },
            getLocations: function () {
                return $http.get(serviceBase + 'api/employees/locations');
            },
            //properties and methods of discipline items
            disciplineItems: {
                selectedDisciplineItem: null,
                actualDisciplineItemsType: '',
                getDisciplineItems: function (employeeId) {
                    return $http.get(serviceBase + 'api/employees/activity/discipline/' + employeeId);
                },
                deleteDisciplineItem: function (id) {
                    return $http.delete(serviceBase + 'api/employees/activity/discipline/' + id);
                },
                saveNewDisciplineItem: function (disciplineItem) {
                    if (disciplineItem.id)//update
                        return $http.put(serviceBase + 'api/employees/activity/discipline/' + disciplineItem.id, disciplineItem, {});
                    return $http.post(serviceBase + 'api/employees/activity/discipline', disciplineItem, {});
                }
            },
            //properties and methods of out from office
            outFromOffice: {
                selectedOutFromOfficeItem: null,
                actualOutFromOfficeType: '',
                getOutFromOffice: function (employeeId) {
                    return $http.get(serviceBase + 'api/employees/activity/out-from-office/' + employeeId);
                },
                deleteOutFromOffice: function (id) {
                    return $http.delete(serviceBase + 'api/employees/activity/out-from-office/' + id);
                },
                saveOutFromOffice: function (item) {
                    if (item.id)//update
                        return $http.put(serviceBase + 'api/employees/activity/out-from-office/' + item.id, item, {});
                    return $http.post(serviceBase + 'api/employees/activity/out-from-office/', item, {});
                }
            },
            sertification: {
                selectedSertification: null,
                getSertifications: function (employeeId) {
                    return $http.get(serviceBase + 'api/employees/activity/sertification/' + employeeId);
                },
                deleteSertification: function (id) {
                    return $http.delete(serviceBase + 'api/employees/activity/sertification/' + id);
                },
                saveSertification: function (item) {
                    if (item.id)//update
                        return $http.put(serviceBase + 'api/employees/activity/sertification/' + item.id, item, {});
                    return $http.post(serviceBase + 'api/employees/activity/sertification/', item, {});
                }
            },
            constants: {
                minDate: new Date(1960, 1, 1),
                maxDate: new Date(Date.now())
            }
        }

        employeesServiceFactory.retirees = _retirees;
        employeesServiceFactory.dismissed = _dismissed;
        employeesServiceFactory.employees = _employees;
        employeesServiceFactory.common = _common;
        employeesServiceFactory.activityItems = _activityItems;

        return employeesServiceFactory;
    }
]);