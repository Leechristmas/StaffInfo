'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', function ($scope, dashboardService) {
        $scope.employees = [];

        //dashboardService.getEmployees().then(function (results) {
        //    $scope.employees = results.data;
        //}, function(error) {
        //    //alert
        //});;

        dashboardService.getEmployees().then(function(results) {
            $scope.employees = results.data;
        }, function(error) {
            //alert
        });

        

        //chart config
        $scope.data = [10, 27, 16, 22, 11, 8, 5];
        $scope.labels = ["Водолазная", "Пожарная", "Медицинская", "Химическая", "Взрывотехническая", "Аппарат отряда", "Отдел кадрового делопроизводства"];
        $scope.options = {};

    }
]);