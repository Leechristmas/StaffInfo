app.directive("tooltip", function($compile) {
    var contentContainer;

    return {
        restrict: "A",
        scope: {
            tooltipScope: "="
        },
        link: function(scope, element, attrs) {
            var templateUrl = attrs.tooltip;

            scope.hidden = true;

            var tooltipElement = angular.element("<div ng-hide='hidden'>");
            tooltipElement.append("<div ng-include='\"" + templateUrl + "\"'></div>");

            element.parent().append(tooltipElement);
            element
              .on('mouseenter', function () { scope.hidden = false; scope.$digest(); })
              .on('mouseleave', function () { scope.hidden = true; scope.$digest(); });

            var toolTipScope = scope.$new(true);
            angular.extend(toolTipScope, scope.myTooltipScope);
            $compile(tooltipElement.contents())(toolTipScope);
            $compile(tooltipElement)(scope);
        }
    }


})