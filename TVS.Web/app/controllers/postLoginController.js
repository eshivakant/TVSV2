'use strict';
app.controller('postLoginController', ['$scope', '$http', '$location', 'ngAuthSettings', 'personData', function ($scope, $http, $location, ngAuthSettings, personData) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $http.get(serviceBase + '/Api/Account/UserRegStatus?meta=' + Math.random())
    .then(function (response) {
        if (response.data.indexOf("LandlordRegistered") > -1) {
            personData.role = "Landlord";
            personData.isRegistered = true;
            $location.path('/landlordhome');
        } else if (response.data.indexOf("LandlordUnRegistered") > -1) {
            personData.role = "Landlord";
            personData.isRegistered = false;
            $location.path('/lsignup1');
        } else if (response.data.indexOf("TenantRegistered") > -1) {
            personData.role = "Tenant";
            personData.isRegistered = true;
            $location.path('/tenanthome');
        } else if (response.data.indexOf("TenantUnRegistered") > -1) {
            personData.role = "Tenant";
            personData.isRegistered = false;
            $location.path('/tsignup1');
        } else {
            //todo: error handling
        }

    });

}])