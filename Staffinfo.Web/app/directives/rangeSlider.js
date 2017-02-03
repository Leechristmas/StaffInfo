app.directive('rangeSlider', function($compile) {
    return {
        restrict: "E",
        replace: true,
        scope: {
            max: '=',
            min: '=',
            minGap: '=',
            step: '=',
            lowerValue: "=lowerValue",
            upperValue: "=upperValue"
        },
        controller: function($scope) {
            $scope.lowerMax = $scope.max - $scope.step;
            $scope.upperMin = $scope.lowerValue + $scope.step;

            $scope.lowerValue = $scope.min;
            $scope.upperValue = $scope.max;

            $scope.$watch('lowerValue', function() {
                $scope.upperMin = $scope.lowerValue + $scope.step;
                $scope.upperWidth = ((($scope.max - ($scope.lowerValue + $scope.step)) / $scope.max) * 100) + "%";
                if ($scope.lowerValue > ($scope.upperValue - $scope.minGap) && $scope.upperValue < $scope.max) {
                    $scope.upperValue = $scope.lowerValue + $scope.minGap;
                }
            });
        },
        templateUrl: 'app/common/sliders/range-slider.tpl.html',
        link: function(scope, element, attrs) {

        }
    }
});