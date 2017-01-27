'use strict';

app.controller('employeesController', [
    '$scope', 'employeesService', '$mdToast', 'messageService', '$mdDialog', '$state', function ($scope, employeesService, $mdToast, messageService, $mdDialog, $state) {

        //COMMON----------------------------------------------
        //options for queries to API and pagination
        $scope.query = {
            order: 'employeeLastname',
            limit: 10,
            page: 1,
            label: {
                of: "из",
                page: 'Текущая',
                rowsPerPage: 'Кол-во на странице'
            },
            filter: ''  //it would be better to refresh the list when filter has 3 chars at least.
        };

        //returns date from string
        $scope.getDate = function (date) {
            var t = new Date(date);
            return new Date(t.getFullYear(), t.getMonth(), t.getDate(), 3);
        }

        //----------------------------------------------------

        //EMPLOYEES-------------------------------------------

        //gets employees with pagination
        $scope.getEmployees = function () {
            $scope.promise = employeesService.employees.getEmployees($scope.query).then(function (response) {
                $scope.employees = response.data;
                $scope.total = response.headers('X-Total-Count');
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //employees list
        $scope.getEmployees();

        //refresh employees list
        $scope.refreshEmployees = function () {
            $scope.promise = $scope.getEmployees();
        }

        //returns $promise with employee by id
        $scope.getEmployeeById = function (id) {
            return employeesService.employees.getEmployeeById(id);
        };

        //deletes the specified employee
        var _deleteEmployee = function (id) {
            $scope.promise = employeesService.employees.deleteEmployeeById(id).then(function (response) {
                $scope.getEmployees();//refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      'Информация о сотруднике была удалена.' +
                                    '</div>' +
                                '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //opens the dialog window with detailed information about specified employee
        $scope.showDetails = function (ev, id) {
            $scope.getEmployeeById(id).then(function (response) {

                //TODO: set JSON parser for data - date is parsed not correct
                var employee = response.data;
                employee.birthDate = new Date(employee.birthDate);

                employeesService.employees.setActualEmployee(employee);

                $state.go('details');
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        };

        //shows the window with form for adding new employee
        $scope.showAddingView = function (ev) {
            $mdDialog.show({
                controller: 'addEmployeeController',
                templateUrl: 'app/views/addEmployeeView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true
            }).then(function (answer) {
                $scope.getEmployees();
                console.log('new employee has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        }

        //shows confirmation of employee deletion 
        $scope.confirmDeleting = function (ev, id) {
            var confirm = $mdDialog.confirm()
                    .title('Удаление')
                    .textContent('Вы уверены, что хотите удалить информацию о сотруднике? \nВосстановить утерянную информацию будет невозможно!')
                    .ariaLabel('Deleting')
                    .targetEvent(ev)
                    .ok('Удалить')
                    .cancel('Отмена');
            $mdDialog.show(confirm).then(function () {
                //delete the employee
                _deleteEmployee(id);
            }, function () {
                //cancel
            });
        }

        //----------------------------------------------------

    }]).controller('toastController', ['$scope', '$mdDialog', 'messageService', '$mdToast', function ($scope, $mdDialog, messageService, $mdToast) {

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
                    .title('Ошибка! ' + messageService.errors.getError().errorTitle)
                    .textContent(messageService.errors.getError().errorText)
                    .ariaLabel('More info')
                    .ok('OK')
                    .targetEvent(e)
                )
                .then(function () {
                    isDlgOpen = false;
                });
        };

    }]).controller('detailsController', ['$scope', '$mdDialog', 'employeesService', 'messageService', '$timeout', '$mdToast', '$state', function ($scope, $mdDialog, employeesService, messageService, $timeout, $mdToast, $state) {


//COMMON----------------------------------------------

        $scope.maxBirthDate = employeesService.common.maxBirthDate;
        $scope.minBirthDate = employeesService.common.minBirthDate;
        $scope.minRetirementDate = '';

        $scope.personalInfoTabConfig = function() {
            $scope.promise = employeesService.activityItems.mesAchievements.getMesAchievements().then(function (response) {//calculating constraint for retirement date
                $scope.mesAchievements = response.data;
                if ($scope.mesAchievements && $scope.mesAchievements.length > 0) {
                    $scope.minRetirementDate = new Date($scope.mesAchievements[0].startDate);
                    $scope.mesAchievements.forEach(function (item) {
                        var date = null;
                        if (item.finishDate)
                            date = new Date(item.finishDate);
                        else 
                            date = new Date(Date.now());

                        if (date > $scope.minRetirementDate) {
                            $scope.minRetirementDate = date; //maybe finish date should be here.
                        }
                    });
                }

            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        //returns date from string
        $scope.getDate = function (date) {
            if (date) {
                var t = new Date(date);
                return new Date(t.getFullYear(), t.getMonth(), t.getDate(), 3);
            } else
                return null;
        }

        //selected employee
        $scope.employee = employeesService.employees.getActualEmployee();
        //selected for changing
        $scope.changeable = employeesService.common.getClone($scope.employee);

        $scope.uploadFile = function (input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {

                    //Sets the Old Image to new New Image
                    document.getElementById('photo-id').setAttribute('src', e.target.result);

                    //Create a canvas and draw image on Client Side to get the byte[] equivalent
                    var canvas = document.createElement("canvas");
                    var imageElement = document.createElement("img");

                    imageElement.setAttribute('src', e.target.result);
                    canvas.width = imageElement.width;
                    canvas.height = imageElement.height;
                    var context = canvas.getContext("2d");
                    context.drawImage(imageElement, 0, 0);
                    var base64Image = canvas.toDataURL("image/jpeg");

                    //Removes the Data Type Prefix 
                    //And set the view model to the new value
                    $scope.changeable.employeePhoto = base64Image.replace(/data:image\/jpeg;base64,/g, '');
                }

                //Renders Image on Page
                reader.readAsDataURL(input.files[0]);
            }
        }

        $scope.upload = function () {
            angular.element(document.getElementById('photo-id')).click();
        };

        //reset all changes
        $scope.resetChanges = function () {
            $scope.changeable = employeesService.common.getClone($scope.employee);
        }

        $scope.isPensioner = function () {
            if ($scope.employee.retirementDate) return true;
            return false;
        }

        //----------------------------------------------------

        //HANDLERS--------------------------------------------

        //shows the window for adding new mes achievement
        $scope.showAddMesView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.mesAchievements.selectedMesAchievement = null;
            else if (mode === 'edit') {
                employeesService.activityItems.mesAchievements.selectedMesAchievement = employeesService.common.getClone(item);
                employeesService.activityItems.mesAchievements.selectedMesAchievement.startDate = $scope.getDate(item.startDate);
                employeesService.activityItems.mesAchievements.selectedMesAchievement.finishDate = $scope.getDate(item.finishDate);
            }

            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addMesView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getMesAchievements(); //refresh the list
                console.log('new achievement has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        $scope.showAddMilitaryView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.military.selectedMilitary = null;
            else if (mode === 'edit') {
                employeesService.activityItems.military.selectedMilitary = employeesService.common.getClone(item);
                employeesService.activityItems.military.selectedMilitary.startDate = $scope.getDate(item.startDate);
                employeesService.activityItems.military.selectedMilitary.finishDate = $scope.getDate(item.finishDate);
            }
                
            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addMilitaryView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getMilitary(); //refresh the list
                console.log('new military has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        $scope.showAddDisciplineItemView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.disciplineItems.selectedDisciplineItem = null;
            else if (mode === 'edit') {
                employeesService.activityItems.disciplineItems.selectedDisciplineItem = employeesService.common.getClone(item);
                employeesService.activityItems.disciplineItems.selectedDisciplineItem.date = $scope.getDate(item.date);
            }

            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addDisciplineItem.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getDisciplineItems(); //refresh the list
                console.log('new discipline item has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        $scope.showAddWorkView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.works.selectedWork = null;
            else if (mode === 'edit') {
                employeesService.activityItems.works.selectedWork = employeesService.common.getClone(item);
                employeesService.activityItems.works.selectedWork.startDate = $scope.getDate(item.startDate);
                employeesService.activityItems.works.selectedWork.finishDate = $scope.getDate(item.finishDate);
            }

            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addWorkView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getWorks(); //refresh the list
                console.log('new work has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        $scope.showAddContractView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.contracts.selectedContract= null;
            else if (mode === 'edit') {
                employeesService.activityItems.contracts.selectedContract = employeesService.common.getClone(item);
                employeesService.activityItems.contracts.selectedContract.startDate = $scope.getDate(item.startDate);
                employeesService.activityItems.contracts.selectedContract.finishDate = $scope.getDate(item.finishDate);
            }

            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addContractView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getContracts(); //refresh the list
                console.log('new work has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        $scope.showAddEducationView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.education.selectedEducation = null;
            else if (mode === 'edit') {
                employeesService.activityItems.education.selectedEducation = employeesService.common.getClone(item);
                employeesService.activityItems.education.selectedEducation.startDate = $scope.getDate(item.startDate);
                employeesService.activityItems.education.selectedEducation.finishDate = $scope.getDate(item.finishDate);
            }

            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addEducationView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getEducation(); //refresh the list
                console.log('new work has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        $scope.showAddOutFromOfficeView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.outFromOffice.selectedOutFromOfficeItem = null;
            else if (mode === 'edit') {
                employeesService.activityItems.outFromOffice.selectedOutFromOfficeItem = employeesService.common.getClone(item);
                employeesService.activityItems.outFromOffice.selectedOutFromOfficeItem.startDate = $scope.getDate(item.startDate);
                employeesService.activityItems.outFromOffice.selectedOutFromOfficeItem.finishDate = $scope.getDate(item.finishDate);
            }

            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addOutFromOfficeItem.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getOutFromOffice(); //refresh the list
                console.log('new "out from office" item has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        $scope.showAddSertificationView = function (ev, mode, item) {
            if (mode === 'add')
                employeesService.activityItems.sertification.selectedSertification = null;
            else if (mode === 'edit') {
                employeesService.activityItems.sertification.selectedSertification = employeesService.common.getClone(item);
                employeesService.activityItems.sertification.selectedSertification.dueDate = $scope.getDate(item.dueDate);
            }

            $mdDialog.show({
                controller: 'addEmployeeItemsController',
                templateUrl: 'app/views/addSertificationView.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                locals: {
                    mode: mode
                }
            }).then(function (answer) {
                $scope.getSertifications(); //refresh the list
                console.log('new sertification item has been added.');
            }, function () {
                console.log('adding view has been closed.');
            });
        };

        //shows confirmation for transferring employee
        $scope.askForTransfer = function (type, ev) {
            var confirm = {};
            if (type === 'P') {
                confirm = {
                    controller: 'transferController',
                    templateUrl: 'app/views/retiredTransferView.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    clickOutsideToClose: true
                };
                $mdDialog.show(confirm).then(function (response) {
                    console.log('');
                    //success
                }, function (error) {
                    //cancel
                });
            } else if (type === 'F') {
                confirm = {
                    controller: 'transferController',
                    templateUrl: 'app/views/dismissalView.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    clickOutsideToClose: true
                }
                $mdDialog.show(confirm).then(function () {
                    //transfer to fired
                }, function () {
                    //cancel
                });
            }
        };

        //confirmation deleting
        $scope.confirmDeleting = function (ev, id, type) { //type: W - works; M - military; A - achievements; D - discipline items; O- 'out from office' items; C - contracts

            var confirm = $mdDialog.confirm()
                .title('Удаление')
                .textContent('Вы уверены, что хотите удалить запись? \nВосстановить утерянную информацию будет невозможно!')
                .ariaLabel('Deleting')
                .targetEvent(ev)
                .ok('Удалить')
                .cancel('Отмена');
            $mdDialog.show(confirm).then(function () {
                //delete the item
                switch (type) {
                    case "W":   //works
                        _deleteWork(id);
                        break;
                    case "M":   //military
                        _deleteMilitary(id);
                        break;
                    case "A":   //MES achievements
                        _deleteMesAchievement(id);
                        break;
                    case "D":   //discipline
                        _deleteDisciplineItem(id);
                    case "O":   //out from office
                        _deleteOutFromOfficeItem(id);
                    case "S":   //sertification
                        _deleteSertification(id);
                    case "C":   //contract
                        _deleteContract(id);
                    case "E":   //education
                        _deleteEducation(id);
                    default:
                        break;
                }
            }, function () {
                //cancel
            });
        }

        //save specified changes for the employee
        $scope.saveChanges = function () {
            //save the changes
            employeesService.employees.saveChanges($scope.changeable).then(function (response) {
                if ($scope.isPensioner())
                    $state.go('retirees');
                else
                    $state.go('employees');

                $mdToast.show({
                    hideDelay: 5000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content text-center">' +
                        'Изменения приняты.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        $scope.getSeniority = function () {
            $scope.promise = employeesService.employees.getSeniorityById($scope.employee.id).then(function (response) {
                $scope.seniority = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }
        //----------------------------------------------------

        //OUT FROM OFFICE-------------------------------------

        $scope.outFromOfficeItems = [];
        $scope.outFromOfficeType = 'S';

        var _deleteOutFromOfficeItem = function (id) {
            $scope.promise = employeesService.activityItems.outFromOffice.deleteOutFromOffice(id).then(function (response) {
                $scope.getOutFromOffice(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        $scope.getOutFromOffice = function () {
            $scope.promise = employeesService.activityItems.outFromOffice.getOutFromOffice($scope.employee.id).then(function (response) { //success
                $scope.outFromOfficeItems = response.data.filter(function (item) { return item.cause === $scope.outFromOfficeType });
                employeesService.activityItems.outFromOffice.actualOutFromOfficeType = $scope.outFromOfficeType;
            }, function (data) { //if error
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

        //DISCIPLINE------------------------------------------

        $scope.disciplineItems = [];
        $scope.discType = 'G';  //gratitudes by default

        var _deleteDisciplineItem = function (id) {
            $scope.promise = employeesService.activityItems.disciplineItems.deleteDisciplineItem(id).then(function (response) {
                $scope.getDisciplineItems(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        $scope.getDisciplineItems = function () {
            $scope.promise = employeesService.activityItems.disciplineItems.getDisciplineItems($scope.employee.id).then(function (response) { //success
                $scope.disciplineItems = response.data.filter(function (item) { return item.itemType === $scope.discType });
                employeesService.activityItems.disciplineItems.actualDisciplineItemsType = $scope.discType;
            }, function (data) { //if error
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

        //MES-------------------------------------------------
        $scope.mesAchievements = [];

        //returns MES achievements for employee
        $scope.getMesAchievements = function () {
            $scope.promise = employeesService.activityItems.mesAchievements.getMesAchievements().then(function (response) {
                $scope.mesAchievements = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //deletes mes achievemnt by id
        var _deleteMesAchievement = function (id) {
            $scope.promise = employeesService.activityItems.mesAchievements.deleteMesAchievement(id).then(function (response) {
                $scope.getMesAchievements(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

        //WORKS-----------------------------------------------
        $scope.works = [];

        //returns works for employee
        $scope.getWorks = function () {
            $scope.promise = employeesService.activityItems.works.getWorks().then(function (response) {
                $scope.works = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //deletes work by id
        var _deleteWork = function (id) {
            $scope.promise = employeesService.activityItems.works.deleteWork(id).then(function (response) {
                $scope.getWorks(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

        //EDUCATION-----------------------------------------------
        $scope.educationItems = [];

        //returns education items for employee
        $scope.getEducation = function () {
            $scope.promise = employeesService.activityItems.education.getEducationItems().then(function (response) {
                $scope.educationItems = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //deletes work by id
        var _deleteEducation = function (id) {
            $scope.promise = employeesService.activityItems.education.deleteEducation(id).then(function (response) {
                $scope.getEducation(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

        //CONTRACTS-----------------------------------------------
        $scope.contracts = [];

        //returns contracts for employee
        $scope.getContracts = function () {
            $scope.promise = employeesService.activityItems.contracts.getContracts().then(function (response) {
                $scope.contracts = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //deletes contract by id
        var _deleteContract = function (id) {
            $scope.promise = employeesService.activityItems.contracts.deleteContract(id).then(function (response) {
                $scope.getContracts(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

        //MILITARY--------------------------------------------
        $scope.military = [];

        //returns military for employee
        $scope.getMilitary = function () {
            $scope.promise = employeesService.activityItems.military.getMilitary().then(function (response) {
                $scope.military = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //deletes military by id
        var _deleteMilitary = function (id) {
            $scope.promise = employeesService.activityItems.military.deleteMilitary(id).then(function (response) {
                $scope.getMilitary(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

        //SERTIFICATION---------------------------------------

        $scope.sertifications = [];

        //returns sertifications for employee
        $scope.getSertifications = function () {
            $scope.promise = employeesService.activityItems.sertification.getSertifications(employeesService.employees.actualEmployee.id).then(function (response) {
                $scope.sertifications = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //deletes sertifications by id
        var _deleteSertification = function (id) {
            $scope.promise = employeesService.activityItems.sertification.deleteSertification(id).then(function (response) {
                $scope.getSertifications(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Запись была успешно удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }


        //----------------------------------------------------

}]).controller('addEmployeeItemsController', ['$scope', '$mdDialog', 'employeesService', 'messageService', '$mdToast', 'mode', function ($scope, $mdDialog, employeesService, messageService, $mdToast, mode) {

        //COMMON----------------------------------------------
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.dtpckrOnFocus = function (item, field) {//when ditetimepicker is focused and the model field is undefined
            if (!item[field])
                item[field] = new Date();
        }

        $scope.minDate = employeesService.activityItems.constants.minDate;
        $scope.maxDate = employeesService.activityItems.constants.maxDate;
        $scope.mode = mode;

        //posts init
        $scope.getPosts = function (serviceId) {
            employeesService.activityItems.getPosts(serviceId).then(function (response) {
                $scope.posts = response.data;
            }, function (data) {
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //ranks init
        employeesService.activityItems.getRanks().then(function (response) {
            $scope.ranks = response.data;
        }, function (data) {
            messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
            $mdToast.show(messageService.errors.errorViewConfig);
        });

        //locations init
        employeesService.activityItems.getLocations().then(function (response) {
            $scope.locations = response.data;
        }, function (data) {
            messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
            $mdToast.show(messageService.errors.errorViewConfig);
        });

        //services init
        employeesService.activityItems.getServices().then(function (response) {
            $scope.services = response.data;
        }, function (data) {
            messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
            $mdToast.show(messageService.errors.errorViewConfig);
        });
        //----------------------------------------------------

        //OUT FROM OFFICE-------------------------------------

        $scope.outFromOffice = employeesService.activityItems.outFromOffice.selectedOutFromOfficeItem == null
            ? { employeeId: employeesService.employees.getActualEmployee().id, cause: employeesService.activityItems.outFromOffice.actualOutFromOfficeType }
            : employeesService.activityItems.outFromOffice.selectedOutFromOfficeItem;

        $scope.saveNewOutFromOffice = function () {
            $scope.promise = employeesService.activityItems.outFromOffice.saveOutFromOffice($scope.outFromOffice).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------
        
        //MES-------------------------------------------------
        $scope.mesAchItem = employeesService.activityItems.mesAchievements.selectedMesAchievement == null 
            ? { employeeId: employeesService.employees.getActualEmployee().id }
            : employeesService.activityItems.mesAchievements.selectedMesAchievement;

        if (mode === 'edit') $scope.getPosts($scope.mesAchItem.serviceId);

        //saves new mes achievement
        $scope.saveNewMesAchievement = function () {

            //if strat date is bigger than "today"
            if ($scope.mesAchItem.startDate.withoutTime().startDate > $scope.maxDate.withoutTime()) {
                messageService.errors.setError({ errorText: 'Дата указана неверно: проверьте диапазон дат.', errorTitle: "" });
                $mdToast.show(messageService.errors.errorViewConfig);
                return;
            }

            $scope.promise = employeesService.activityItems.mesAchievements.saveMesAchievement($scope.mesAchItem).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }
        //----------------------------------------------------

        //MILITARY--------------------------------------------
        $scope.military = employeesService.activityItems.military.selectedMilitary == null
            ? { employeeId: employeesService.employees.getActualEmployee().id }
            : employeesService.activityItems.military.selectedMilitary;

        //saves new military
        $scope.saveNewMilitary = function () {
            $scope.promise = employeesService.activityItems.military.saveMilitary($scope.military).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }
        //----------------------------------------------------

        //WORK------------------------------------------------
        $scope.work = employeesService.activityItems.works.selectedWork == null
            ? { employeeId: employeesService.employees.getActualEmployee().id }
            : employeesService.activityItems.works.selectedWork;

        //saves new work
        $scope.saveNewWork = function () {
            $scope.promise = employeesService.activityItems.works.saveWork($scope.work).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }
        //----------------------------------------------------

        //CONTRACT------------------------------------------------
        $scope.contract = employeesService.activityItems.contracts.selectedContract == null
            ? { employeeId: employeesService.employees.getActualEmployee().id }
            : employeesService.activityItems.contracts.selectedContract;

        //saves new work
        $scope.saveNewContract = function () {
            $scope.promise = employeesService.activityItems.contracts.saveContract($scope.contract).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }
        //----------------------------------------------------

        //EDUCATION------------------------------------------------
        $scope.education = employeesService.activityItems.education.selectedEducation == null
            ? { employeeId: employeesService.employees.getActualEmployee().id }
            : employeesService.activityItems.education.selectedEducation;

        //saves new work
        $scope.saveNewEducation = function () {
            $scope.promise = employeesService.activityItems.education.saveEducation($scope.education).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }
        //----------------------------------------------------

        //DISCIPLINE------------------------------------------
        $scope.newDisciplineItem = employeesService.activityItems.disciplineItems.selectedDisciplineItem == null
            ? { employeeId: employeesService.employees.getActualEmployee().id, itemType: employeesService.activityItems.disciplineItems.actualDisciplineItemsType }
            : employeesService.activityItems.disciplineItems.selectedDisciplineItem;

        //saves new discipline item
        $scope.saveNewDisciplineItem = function () {
            $scope.promise = employeesService.activityItems.disciplineItems.saveNewDisciplineItem($scope.newDisciplineItem).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }
        //----------------------------------------------------

        //SERTIFICATION---------------------------------------
        $scope.sertification = employeesService.activityItems.sertification.selectedSertification == null
            ? { employeeId: employeesService.employees.getActualEmployee().id }
            : employeesService.activityItems.sertification.selectedSertification;
        
        //saves new sertification item
        $scope.saveNewSertification = function () {
            $scope.promise = employeesService.activityItems.sertification.saveSertification($scope.sertification).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      (mode === 'edit' ? 'Запись успешно изменена.' : 'Запись успешно добавлена.') +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main controller to refresh or do not the list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        }

        //----------------------------------------------------

}]).controller('addEmployeeController', ['$scope', '$mdDialog', 'employeesService', 'messageService', '$mdToast', function ($scope, $mdDialog, employeesService, messageService, $mdToast) {

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };

        $scope.maxBirthDate = employeesService.common.maxBirthDate;
        $scope.minBirthDate = employeesService.common.minBirthDate;

        var employee = {    //testing
            employeeLastname: "Тестовый",
            employeeFirstname: "Тест",
            employeeMiddlename: "Тестович",
            birthDate: new Date(1989, 11, 1),
            description: 'тестовое описание',
            detailedAddress: 'тестовый точный адрес',
            city: 'тестовый город',
            area: 'тестовая область',
            zipCode: '123456',
            passportNumber: 'HB1234567',
            passportOrganization: 'тестовая организация',
            passportIdentityNumber: '102proit265743',
            firstPhone: 'первый номер',
            secondPhone: 'второй номер',
            gender: "M",
            personalNumber: "Г-00001"
        }

        $scope.newEmployee = employee;

        $scope.saveNewEmployee = function () {
            $scope.promise = employeesService.employees.addNewEmployee($scope.newEmployee).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      'Сотрудник успешно зарегистрирован.' +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save'); //throw the 'answer' to the main employee controller to refresh or do not the employee list
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errors.errorViewConfig);
            });
        };
    }]);