'use strict';

app.factory('idleService',[function() {
    var idleServiceFactory = {};

    var _idleHasBeenStopped = false;

    idleServiceFactory.idleHasBeenStopped = _idleHasBeenStopped;

    return idleServiceFactory;
}])