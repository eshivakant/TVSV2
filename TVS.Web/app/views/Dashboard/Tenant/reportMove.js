'use strict';
app.controller('tenantsReportMoveCtrl', ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', function ($scope, $http, $location, $q, ngAuthSettings, personData) {

    $scope.model = {};
    $scope.error = '';
    $scope.info = '';

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    //get all people who owned this address
    $http.get(serviceBase + '/api/Tenant/MoveTemplate?hash=' + Math.random())
      .then(function (response) {
          $scope.model = response.data;
      });



    $scope.save = function () {
        $http.post(serviceBase + '/api/Tenant/SaveMove', $scope.model)
         .then(function () {
                $scope.error = '';
                $scope.info = 'New Address has been updated successfully!';
                $location.path('/tenanthome');

            }, function() {
                $scope.error = 'Error occurred. Please try again.';
                $scope.info = '';
            });
    }


    $scope.addAnotherAddressOccupation = function() {

    };

}]);