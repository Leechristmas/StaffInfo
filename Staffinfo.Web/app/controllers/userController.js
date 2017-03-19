'use strict';

app.controller('userController',
[
    '$scope', function($scope) {

        $scope.selected = null;

        $scope.selectUser = function(user)
        {
            $scope.selected = user;
        }

        $scope.users = [
            {
                lastname: 'Иванов',
                firstname: 'Иван',
                middlename: 'Иванович'
            },
            {
                lastname: 'Петров',
                firstname: 'Петр',
                middlename: 'Петрович'
            },
            {
                lastname: 'Сидоров',
                firstname: 'Алексей',
                middlename: 'Сергеевич'
            },
            {
                lastname: 'Александров',
                firstname: 'Петр',
                middlename: 'Геннадьевич'
            }
        ];



    }
]);