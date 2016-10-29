'use strict';

app.factory('dashboardService', [
    '$http, ngAuthSettings', function ($http, ngAuthSettings) {
        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var dashboardServiceFactory = {};

        var _getData = function () {
            return $http.get(serviceBase + 'api/employee')
                .then(function (result) {
                    return result;
                });
        };

        dashboardServiceFactory.getData = _getData;

        return dashboardServiceFactory;
    }
]);