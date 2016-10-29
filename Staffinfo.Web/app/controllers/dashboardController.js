'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', function($scope, dashboardService) {
        $scope.employees = [];

        dashboardService.getEmployees().then(function (results) {
            $scope.employees = results.data;
        }, function(error) {
            //alert
        });
    }
]);