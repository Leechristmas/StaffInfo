app.controller('settingsController', [
    '$scope', '$userSettings', 'settingsService', function ($scope, $userSettings, settingsService) {

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

        $scope.saveSettings = function() {
            $scope.allNotificationTypes.forEach(function (item, index) {
                //save changes of 'selected notification types' list to the service
                settingsService.calendarSettings.includedNotificatoinTypes = $scope.selectedNotifications;

                //save changes of 'selected notification types' list to the local storage
                if ($scope.selectedNotifications.includes(item))
                    $userSettings.set(item.key, true);
                else
                    $userSettings.set(item.key, false);
            });
        }

        $scope.resetChanges = function() {
            $scope.selectedNotifications = settingsService.calendarSettings.loadIncludedNotificatoinTypes();
        }

    }
]);