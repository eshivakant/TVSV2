'use strict';
app.directive('jqdatepicker', function ($filter) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {

            $(function () {
                element.datepicker({
                    dateFormat: 'dd/mm/yy',
                    onSelect: function (date) {
                        scope.$apply(function () {

                            var parts = date.split('/');
                            // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
                            var dateVal = new Date(parts[2], parts[1] - 1, parts[0]); // Note: months are 0-based

                            ngModelCtrl.$setViewValue(dateVal);
                        });
                    }
                });

            });

            ngModelCtrl.$formatters.unshift(function (v) {
                return $filter('date')(v, 'dd/MM/yyyy');
            });
        }
    }
});

