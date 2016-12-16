app.controller('settingsController', [
    '$scope', '$userSettings', 'ngSettingItems', function ($scope, $userSettings, ngSettingItems) {

        $scope.selected = [];
        //var birthdays = false, sertification = false, custom = false;

        //loading settings
        $scope.items = ngSettingItems.calendarNotificationTypes;
        $scope.items.forEach(function(item, index) {
            if ($userSettings.get(item))
                $scope.selected.push(item);
        });

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
            return ($scope.selected.length !== 0 &&
                $scope.selected.length !== $scope.items.length);
        };

        $scope.isChecked = function () {
            return $scope.selected.length === $scope.items.length;
        };

        $scope.toggleAll = function () {
            if ($scope.selected.length === $scope.items.length) {
                $scope.selected = [];
            } else if ($scope.selected.length === 0 || $scope.selected.length > 0) {
                $scope.selected = $scope.items.slice(0);
            }
        };

        ////////////////////////////////////////

        $scope.saveSettings = function() {
            $scope.items.forEach(function(item, index) {
                if ($scope.selected.includes(item))
                    $userSettings.set(item, true);
                else
                    $userSettings.set(item, false);
            });
        }

    }
]);