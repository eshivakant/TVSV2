'use strict';
app.controller('landlordsRateTenantCtrl', ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', function ($scope, $http, $location, $q, ngAuthSettings, personData) {

    $scope.myTenants = {};
    $scope.selectedTenant = {};
    $scope.ratingTemplate = {};
    $scope.tenantSelected = false;
    $scope.ratingTemplate = {};
    $scope.ratingError = "";

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $http.get(serviceBase + '/Api/Rating/Landlord/MyTenants' + '?hash=' + Math.random()) //to avoid caching
        .then(function (response) {
            $scope.myTenants = response.data;
        }, function (response) {
            $scope.ratingError = response;
        });


    $scope.getTemplate= function() {
        $http.get(serviceBase + '/Api/Rating/Landlord/GetRatingTemplate' + '?hash=' + Math.random())
            .then(function(response) {
                $scope.ratingTemplate = response.data;
            }, function (response) {
                $scope.ratingError = response;
            });
    }


    $scope.submitTenantReview = function (personRating) {
        personRating.personId = $scope.selectedTenant.id;

        $http.post(serviceBase + '/Api/Rating/Landlord/Submit', personRating) 
       .then(function (response) {
           $scope.selectedTenant = {};
           $scope.tenantSelected = false;
       }, function (response) {
           $scope.ratingError = response;
       });

    };

    $scope.fullName=function(person) {
        var name = person.initial + " " + person.firstName + " " + person.middleName + " " + person.lastName;
        return  name.replace("  ", " ");
    }

    $scope.fullAddress = function (address) {
        var name = address.addressLine1 + ", " + address.addressLine2 + ", " + address.addressLine3 + ", " + address.city + ", " + address.state + " - " + address.postCode;
        return name.replace("  ", " ").replace(",,", ",");
    }


    $scope.startRating = function (tenant) {
        $scope.getTemplate();
        $scope.selectedTenant = tenant;
        $scope.tenantSelected = true;
    };

    $scope.cancel = function() {
        $scope.selectedTenant = {};
        $scope.tenantSelected = false;
    }


    $scope.getScoreText=function(score) {
        switch (score) {
            case 1:
                return "Horrible";
            case 2:
                return "Very Bad";
            case 3:
                return "Bad";
            case 4:
                return "Satisfactory";
            case 5:
                return "Adequate";
            case 6:
                return "Good";
            case 7:
                return "Very Good";
            case 8:
                return "Excellent";
            case 9:
                return "Fanastic";
            case 10:
                return "Couldn't be better!";

        default:
        }
    }


}]);