'use strict';

app.controller('calendarController', [
    '$scope', 'dashboardService', '$mdDialog', 'messageService', '$mdToast', function ($scope, dashboardService, $mdDialog, messageService, $mdToast) {
        $scope.notification = dashboardService.calendar.getSelectedNotification();

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

        $scope.deleteNotification = function () {
            if ($scope.notification.id > 0)
                dashboardService.calendar.deleteNotification($scope.notification.id).then(function (response) {
                    $mdToast.show({
                        hideDelay: 3000,
                        position: 'top right',
                        controller: 'toastController',
                        template: '<md-toast class="md-toast-success">' +
                                        '<div class="md-toast-content">' +
                                          'Запись успешно удалена.' +
                                        '</div>' +
                                    '</md-toast>'
                    });
                    $mdDialog.hide('save');
                }, function (data) {
                    $mdDialog.hide('cancel');
                    messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                    $mdToast.show(messageService.errorViewConfig);
                });
        }

        $scope.saveNotification = function () {
            dashboardService.calendar.saveNotification($scope.notification).then(function (response) {
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
            }, function (data) {
                $mdDialog.hide('cancel');
                messageService.errors.setError({ errorText: data.data, errorTitle: 'Статус - ' + data.status + ': ' + data.statusText });
                $mdToast.show(messageService.errorViewConfig);
            });
        }

    }
]);