'use strict';

app.controller('reportingController', [
    '$scope', 'reportingService', function($scope, reportingService) {
        $scope.getReport = function(name, format) {
            switch (name) {
            case 'totalEmployees':
                reportingService.reports.getTotalEmployeesReport(format);
            default:
            }
        }
    }
]);