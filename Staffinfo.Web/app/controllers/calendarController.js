'use strict';

app.controller('calendarController', [
    '$scope', 'dashboardService', '$mdDialog', 'messageService', '$mdToast', function ($scope, dashboardService, $mdDialog, messageService, $mdToast) {
        $scope.notification = {};

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.saveNotification = function() {
            dashboardService.saveNotification($scope.notification).then(function (response) {
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    template: '<md-toast class="md-toast-success">' +
                                    '<div class="md-toast-content">' +
                                      'Запись успешно добавлена.' +
                                    '</div>' +
                                '</md-toast>'
                });
                $mdDialog.hide('save');
            }, function (error) {
                $mdDialog.hide('cancel');
                messageService.setError(error);
                $mdToast.show({
                    hideDelay: 3000,
                    position: 'top right',
                    controller: 'toastController',
                    templateUrl: 'app/views/error-toast.html'
                });
            });
        }

    }
]);