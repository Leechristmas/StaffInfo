'use strict';
app.factory('messageService', function() {

    var _errorText = '';

    var messageServiceFactory = {
        setError: function(text) {
            _errorText = text;
        },
        getError: function() {
            return _errorText;
        }
    };

    return messageServiceFactory;
});