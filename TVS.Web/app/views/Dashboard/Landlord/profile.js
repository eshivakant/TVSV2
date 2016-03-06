'use strict';
app.controller('landlordsProfileCtrl', ['$scope', '$http', '$location', '$routeParams', '$q', 'ngAuthSettings', 'personData', function ($scope, $http, $location, $routeParams, $q, ngAuthSettings, personData) {


    function init() {
        var personId = $routeParams.personId;

    }

    init();

}]);