app.controller('changePasswordController',
    [
        '$scope', 'messageService', '$mdToast', '$mdDialog', 'userService', 'userId', function ($scope, messageService, $mdToast, $mdDialog, userService, userId) {

            $scope.userId = userId;
            $scope.confirmPassword = '';
            $scope.newPassword = '';
            $scope.currentPassword = '';
            
            $scope.hide = function () {
                $mdDialog.hide();
            };

            $scope.cancel = function () {
                $mdDialog.cancel();
            };

            $scope.changePassword = function () {
                if ($scope.confirmPassword !== $scope.newPassword)
                    $mdToast.show({
                        hideDelay: 3000,
                        position: 'top right',
                        controller: 'toastController',
                        template: '<md-toast class="md-toast-success">' +
                        '<div class="md-toast-content">' +
                        'Пароли не совпадают' +
                        '</div>' +
                        '</md-toast>'
                    });
                else {
                    $scope.promise = userService
                        .changePassword($scope.userId, $scope.currentPassword, $scope.newPassword)
                        .then(function (response) {
                            $mdToast.show({
                                hideDelay: 3000,
                                position: 'top right',
                                controller: 'toastController',
                                template: '<md-toast class="md-toast-success">' +
                                '<div class="md-toast-content">' +
                                'Пароль изменен.' +
                                '</div>' +
                                '</md-toast>'
                            });
                                $mdDialog.cancel();
                            },
                        function (error) {
                            messageService.errors.setError({ errorText: error.data, errorTitle: 'Статус - ' + error.status + ': ' + error.statusText });
                            $mdToast.show(messageService.errors.errorViewConfig);
                        });
                }
            };

        }
    ]);