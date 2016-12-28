'use strict';

app.controller('dialogController', [
    '$scope', '$mdDialog', 'idleConfig', '$timeout', 'idleService', function ($scope, $mdDialog, idleConfig, $timeout, idleService) {
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };

        $scope.timeInSec = idleConfig.timeout;

        var countUp = function () {

            if (idleService.idleHasBeenStopped) //idle ended
            {
                $scope.cancel();
                return;
            }

            if ($scope.timeInSec <= 0) {
                return;
            };
            $scope.timeInSec -= 1;
            $timeout(countUp, 1000);
        }
        
        $timeout(countUp, 1000);

    }
]);