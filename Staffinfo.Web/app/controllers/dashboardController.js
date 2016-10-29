'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', function($scope, dashboardService) {
        $scope.employees = [];

        dashboardService.getData().then(function(results) {
            $scope.employees = results.data;
        }, function(error) {
            //alert
        });
    }
]);