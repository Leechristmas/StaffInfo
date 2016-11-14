'use strict';

app.controller('dashboardController', [
    '$scope', 'dashboardService', function ($scope, dashboardService) {
        $scope.employees = [];

        //dashboardService.getEmployees().then(function(results) {
        //    $scope.employees = results.data;
        //}, function(error) {
        //    //alert
        //});

        //service chart data
        $scope.serviceChart = {
            data: [10, 27, 16, 22, 11, 8, 5],
            labels: ["Водолазная", "Пожарная", "Медицинская", "Химическая", "Взрывотехническая", "Аппарат отряда", "Отдел кадрового делопроизводства"],
            options: {
                legend: {
                    display: true
                }
            }
        };

        $scope.highRankChart = {
            labels: ["Лейтенант", "Мл. Лейтенант", "Ст. Лейтенант", "Капитан", "Майор", "Подполковник"],
            data: [
                [4, 5, 6, 8, 10, 6]
            ],
            options: {}
        };

        $scope.expChart = {
            labels: ['0-5', '5-10', '10-15', '15-20', '>20'],
            data: [31, 9, 4, 6, 11],
            options: {}
        }

    }
]);