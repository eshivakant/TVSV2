'use strict';
app.factory('personData', ['$http',function ($http) {
    var personData = {};
    
    personData.role = "";
    personData.isRegistered = false;


    $http.get(serviceBase + '/Api/Account/UserRegStatus?meta=' + Math.random())
   .then(function (response) {
       if (response.data.indexOf("LandlordRegistered") > -1) {
           personData.role = "Landlord";
           personData.isRegistered = true;
       } else if (response.data.indexOf("LandlordUnRegistered") > -1) {
           personData.role = "Landlord";
           personData.isRegistered = false;
       } else if (response.data.indexOf("TenantRegistered") > -1) {
           personData.role = "Tenant";
           personData.isRegistered = true;
       } else if (response.data.indexOf("TenantUnRegistered") > -1) {
           personData.role = "Tenant";
           personData.isRegistered = false;
       } else {
           //todo: error handling
       }

   });

    return personData;
}]);