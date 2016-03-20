'use strict';
app.directive("personPartialView", function ($http) {
    return {
        restrict: 'A',
        templateUrl: '/app/partialViews/personPartial.html',
        link: function (scope, element, attr) {
            // Do linking
        }
    };
});
