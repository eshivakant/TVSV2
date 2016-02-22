'use strict';
app.controller('postLoginController', ['$scope', '$http', '$location', 'ngAuthSettings', function ($scope, $http, $location,ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $http.get(serviceBase+'/Api/Account/UserRegStatus?meta='+Math.random())
    .then(function(response) {
            if (response.data.indexOf("LandlordRegistered")>-1) {
                $location.path('/lsignup1');
            } else if (response.data.indexOf("LandlordUnRegistered")>-1) {
                $location.path('/lsignup1');
            } else if (response.data.indexOf("TenantRegistered") > -1) {
                $location.path('/tsignup1');
            } else if (response.data.indexOf("TenantUnRegistered") > -1) {
                $location.path('/tsignup1');
            } else {
                //todo: error handling
            }

        });

}])