﻿app.controller('dismissedController', [
    '$scope', 'employeesService', '$mdToast', 'messageService', '$mdDialog', '$state', function($scope, employeesService, $mdToast, messageService, $mdDialog, $state) {
        //options for queries to API and pagination
        $scope.query = {
            order: 'lastname',
            limit: 10,
            page: 1,
            label: {
                of: "из",
                page: 'Текущая',
                rowsPerPage: 'Кол-во на странице'
            },
            filter: ''
        };

        //gets retirees with pagination 
        $scope.getDismissed = function() {
            $scope.promise = employeesService.getDismissed($scope.query).then(function (response) {
                $scope.dismissed = response.data;
                $scope.total = response.headers('X-Total-Count');
            }, function(data) {
                messageService.setError(data);
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    templateUrl: 'app/views/error-toast.html'
                });
            });
        }

        //returns date from string
        $scope.getDate = function(date) {
            return new Date(date);
        }

        //deletes the specified employee
        var _deleteDismissed = function(id) {
            //TODO: deleting
            $scope.promise = employeesService.deleteDismissedById(id).then(function(response) {
                $scope.getDismissed(); //refresh
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Информация об уволенном была удалена.' +
                        '</div>' +
                        '</md-toast>'
                });
            }, function(error) {
                messageService.setError(error);
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    templateUrl: 'app/views/error-toast.html'
                });
            });
        }

        //shows confirmation of employee deletion 
        $scope.confirmDeleting = function(ev, id) {
            var confirm = $mdDialog.confirm()
                .title('Удаление')
                .textContent('Вы уверены, что хотите удалить информацию об указанном уволенном сотруднике? \nВосстановить утерянную информацию будет невозможно!')
                .ariaLabel('Deleting')
                .targetEvent(ev)
                .ok('Удалить')
                .cancel('Отмена');
            $mdDialog.show(confirm).then(function() {
                //delete the employee
                _deleteDismissed(id);
            }, function() {
                //cancel
            });
        }

        $scope.dismissed = $scope.getDismissed();
    }
]);