'use strict';
app.controller('personSrchModalCtrl', ['$scope', '$element', 'title', 'close', function ($scope, $element, title, close) {
    $scope.name = null;
    $scope.age = null;
    $scope.title = title;
    $scope.person = new TVS.API.Entities.Person();
    //  This close function doesn't need to use jQuery or bootstrap, because
    //  the button has the 'data-dismiss' attribute.
    $scope.search = function () {
        close($scope.person, 500); // close, but give 500ms for bootstrap to animate
    };
    //  This cancel function must use the bootstrap, 'modal' function because
    //  the doesn't have the 'data-dismiss' attribute.
    $scope.cancel = function () {
        //  Manually hide the modal.
        $element.modal('hide');
        //  Now call close, returning control to the caller.
        close($scope.person, 500); // close, but give 500ms for bootstrap to animate
    };
}]);




