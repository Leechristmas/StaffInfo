'use strict';
app.factory('messageService', function() {

    var messageServiceFactory = {}

    var _error = {errorText: '', errorTitle: ''};   //private
    
    var _errors = {

        setError: function (error) {
            _error = error;
        },
        getError: function () {
            return _error;
        },
        errorViewConfig: {
            hideDelay: 5000,
            position: 'top right',
            controller: 'toastController',
            templateUrl: 'app/views/error-toast.html'
        }
    }

    messageServiceFactory.errors = _errors;

    return messageServiceFactory;
});