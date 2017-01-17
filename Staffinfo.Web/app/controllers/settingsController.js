app.controller('settingsController', [
    '$scope', '$userSettings', 'settingsService', '$mdToast', function ($scope, $userSettings, settingsService, $mdToast) {

        $scope.selectedNotifications = [];

        //getting items from the service
        $scope.allNotificationTypes = settingsService.calendarSettings.calendarNotificationTypes;

        //loading settings from local storage
        $scope.selectedNotifications = settingsService.calendarSettings.loadIncludedNotificatoinTypes();
        
        /////////////////////////////////////

        //calendar
        $scope.toggle = function (item, list) {
            var idx = list.indexOf(item);
            if (idx > -1) {
                list.splice(idx, 1);
            }
            else {
                list.push(item);
            }
        };

        $scope.exists = function (item, list) {
            return list.indexOf(item) > -1;
        };

        $scope.isIndeterminate = function () {
            return ($scope.selectedNotifications.length !== 0 &&
                $scope.selectedNotifications.length !== $scope.allNotificationTypes.length);
        };

        $scope.isChecked = function () {
            return $scope.selectedNotifications.length === $scope.allNotificationTypes.length;
        };

        $scope.toggleAll = function () {
            if ($scope.selectedNotifications.length === $scope.allNotificationTypes.length) {
                $scope.selectedNotifications = [];
            } else if ($scope.selectedNotifications.length === 0 || $scope.selectedNotifications.length > 0) {
                $scope.selectedNotifications = $scope.allNotificationTypes.slice(0);
            }
        };

        ////////////////////////////////////////

        $scope.saveSettings = function () {
            //save changes of 'selected notification types' list to the service
            settingsService.calendarSettings.includedNotificatoinTypes = $scope.selectedNotifications;

            $scope.allNotificationTypes.forEach(function (item, index) {
                //save changes of 'selected notification types' list to the local storage
                if ($scope.selectedNotifications.includes(item))
                    $userSettings.set(item.key, true);
                else
                    $userSettings.set(item.key, false);
            });
            $mdToast.show({
                hideDelay: 3000,
                position: 'top right',
                controller: 'toastController',
                template: '<md-toast class="md-toast-success">' +
                                '<div class="md-toast-content">' +
                                  'Настройки были сохранены.' +
                                '</div>' +
                            '</md-toast>'
            });
        }

        $scope.resetChanges = function() {
            $scope.selectedNotifications = settingsService.calendarSettings.loadIncludedNotificatoinTypes();
        }

    }
]);