'use strict';

app.factory('reportingService', [
    "$http", "ngAuthSettings", function($http, ngAuthSettings) {
        var reportingServiceFactory = {};
        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        var _reports = {
            getTotalEmployeesReport: function (format) {
                window.open(serviceBase + 'api/reports/total-employees/' + format, '_blank', '');
            }
        }

        reportingServiceFactory.reports = _reports;

        return reportingServiceFactory;
    }
]);