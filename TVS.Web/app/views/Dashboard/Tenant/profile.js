'use strict';
app.controller('tenantsProfileCtrl', ['$scope', '$http', '$location', '$routeParams', '$q', 'ngAuthSettings', 'personData', function ($scope, $http, $location, $routeParams, $q, ngAuthSettings, personData) {
    
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $scope.person = {};
    $scope.isMyProlfile = false;

    function init() {

        var url = '';
        if ($routeParams.personId == undefined) {
            url = serviceBase + '/Api/Tenant/Template' + '?hash=' + Math.random();
            $scope.isMyProlfile = true;
        } else
            url = serviceBase + '/Api/Tenant/Profile/' + $routeParams.personId;
        $http.get(url)
            .then(function(response) {
                $scope.person = response.data;
            });
        
    }

    init();

}]);