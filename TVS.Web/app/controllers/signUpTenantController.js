'use strict';
app.controller('signUpTenantController', [
    '$scope', '$http', '$q', '$location', 'ngAuthSettings', function ($scope, $http, $q, $location, ngAuthSettings) {

        $scope.newPerson = {};

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        $http.get(serviceBase + '/Api/Tenant/Template'+'?hash=' + Math.random()) //to avoid caching
            .then(function(response) {
                $scope.newPerson = response.data;
                $scope.getLandlordTemplate($q).then(function (result) {
                    var landlordTemplate = result;
                    for (var i = 0; i < $scope.newPerson.addressOccupations.length; i++) {
                        $scope.newPerson.addressOccupations[i].previousLandlord = JSON.parse(JSON.stringify(landlordTemplate));
                    }
                });
            });



        $scope.savePreviousLandlords = function () {
            var person = $scope.newPerson;
            for (var i = 0; i < person.addressOccupations.length; i++) {
                var landlord = person.addressOccupations[i].previousLandlord;
                landlord.placeOfBirth = "NA";
                $scope.addNewAddressOwnership(landlord, person.addressOccupations[i].address, person.addressOccupations[i].occupiedFrom, person.addressOccupations[i].occupiedTo);

                $http.post(serviceBase + '/Api/Tenant/SaveLandlord', landlord)
                 .success(function (response) {
                        $location.path("");
                    })
                 .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
        }


        $scope.addNewAddressOwnership = function (person, address, ownedFrom, ownedTo) {
            var newOwnership = new function () {
                this.ownedFrom = new Date(ownedFrom);
                this.ownedTo = new Date(ownedTo);
                this.address = address;
            }
            person.addressOwnerships = [];
            person.addressOwnerships.push(newOwnership);

        }


        $scope.getLandlordTemplate = function ($q) {
            var deferredObject = $q.defer();
            $http.get(serviceBase + '/Api/Landlord/Template' + '?hash=100').
            success(function (data) {
                deferredObject.resolve(data);
            }).
            error(function () {
                deferredObject.resolve(data);
            });

            return deferredObject.promise;
        }



        $scope.registrationError = "";
        
        
        $scope.savePerson = function (step) {
            if (step === 1) {
                var action = $scope.newPerson.id === 0 ? "Save" : "Update";

                $http.post(serviceBase + '/Api/Tenant/'+action, $scope.newPerson)
                    .success(function (response) {
                        $scope.newPerson = response;
                        $location.path('/tsignup2');
                    })
                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
            if (step === 2) {
                $http.post(serviceBase + '/Api/Tenant/Update', $scope.newPerson)
                    .success(function (response) {
                        $scope.newPerson = response;
                        $location.path('/tsignup3');
                    })
                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
            if (step === 3) {
                $http.post(serviceBase + '/Api/Tenant/Update', $scope.newPerson)
                    .success(function (response) {
                        $scope.newPerson = response;
                        $location.path('');
                    })
                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
        }

        $scope.addNewAddressOccupation = function () {
            var newAddress = new function () {
                this.addressLine1 = "";
                this.addressLine2 = "";
                this.addressLine3 = "";
                this.city = "";
                this.state = "";
                this.postCode = "";
            }

            var newOccpuation = new function () {
                this.occupiedFrom = new Date();
                this.occupiedTo = new Date();
                this.address = newAddress;
                this.rent = "";
            }

            $scope.newPerson.addressOccupations.push(newOccpuation);

        }
    }
]);

