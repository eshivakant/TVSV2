'use strict';

//reviews done by me
app.controller('tenantsMyReviewsCtrl', ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', function ($scope, $http, $location, $q, ngAuthSettings, personData) {

    $scope.addressRatings = [];
    $scope.ratingCount = 0;
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    function init() {

        //get all people who owned this address
        $http.get(serviceBase + '/api/Search/Tenant/MyReviews?hash=' + Math.random())
          .then(function (response) {
              $scope.addressRatings = response.data;
              $scope.ratingCount = $scope.addressRatings.length;
          });


        //get ratings

    }


    $scope.getScoreText = function (score) {

        if (score <= 1)
            return "Horrible";
        else if (score <= 2)
            return "Very Bad";
        else if (score <= 3)
            return "Bad";
        else if (score <= 4)
            return "Satisfactory";
        else if (score <= 5)
            return "Adequate";
        else if (score <= 6)
            return "Good";
        else if (score <= 7)
            return "Very Good";
        else if (score <= 8)
            return "Excellent";
        else if (score <= 9)
            return "Fanastic";
        else
            return "Exceptional!";

    }


    init();

}]);


