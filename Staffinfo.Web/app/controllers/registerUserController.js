app.controller('registerUserController', ['$scope', 'userService', 'messageService', '$mdToast', '$mdDialog', function ($scope, userService, messageService, $mdToast, $mdDialog) {

    $scope.employees = [];
    $scope.user = {
        roles: []
    };

    $scope.getEmployees = function () {
        $scope.promise = userService.getEmployees().then(function (response) {
            $scope.employees = response.data;
        }, function (error) {
            messageService.errors.setError({ errorText: error.data, errorTitle: 'Статус - ' + error.status + ': ' + error.statusText });
            $mdToast.show(messageService.errors.errorViewConfig);
        });
    }

    $scope.registerUser = function () {

        if ($scope.user.isEditor) $scope.user.roles.push("editor");
        if ($scope.user.isReader) $scope.user.roles.push("reader");

        $scope.promise = userService.registerUser($scope.user).then(function (response) {
            $scope.cancel();
        }, function (error) {
            messageService.errors.setError({ errorText: error.data, errorTitle: 'Статус - ' + error.status + ': ' + error.statusText });
            $mdToast.show(messageService.errors.errorViewConfig);
        });
    }

    $scope.hide = function () {
        $mdDialog.hide();
    };

    $scope.cancel = function () {
        $mdDialog.cancel();
    };

    $scope.goNext = function () {
        if ($scope.selectedTabIndex < 2)
            $scope.selectedTabIndex++;
    }

    $scope.goBack = function () {
        if ($scope.selectedTabIndex > 0)
            $scope.selectedTabIndex--;
    }

    $scope.getEmployees();
}]);