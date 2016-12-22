'use strict';
app.factory('messageService', function() {

    var _error = {errorText: '', errorTitle: ''};

    var messageServiceFactory = {
        setError: function(error) {
            _error = error;
        },
        getError: function() {
            return _error;
        },
        errorViewConfig: {
            hideDelay: 5000,
            position: 'top right',
            controller: 'toastController',
            templateUrl: 'app/views/error-toast.html'
        }
    };

    return messageServiceFactory;
});