'use strict';
app.controller('signUpLandlordController', [
    '$scope', '$http','$q', '$location', 'ngAuthSettings', function($scope, $http,$q, $location, ngAuthSettings) {

        $scope.newPerson = {};

        var serviceBase = ngAuthSettings.apiServiceBaseUri;

        $http.get(serviceBase + '/Api/Landlord/Template'+'?hash=' + Math.random()) //to avoid caching
            .then(function(response) {
                $scope.newPerson = response.data;
                $scope.getTenantTemplate($q).then(function(result) {
                    var tenantTemplate = result;
                    for (var i = 0; i < $scope.newPerson.addressOwnerships.length; i++) {
                        $scope.newPerson.addressOwnerships[i].previousTenant = JSON.parse(JSON.stringify(tenantTemplate));
                        $scope.newPerson.addressOwnerships[i].rent = 0;
                    }
                });
            });



        $scope.savePreviousTenants = function () {
            var person = $scope.newPerson;
            for (var i = 0; i < person.addressOwnerships.length; i++) {
                var tenant = person.addressOwnerships[i].previousTenant;
                tenant.placeOfBirth = "NA";
                $scope.addNewAddressOccupation(tenant, person.addressOwnerships[i].address, tenant.addressOccupations[0].occupiedFrom, tenant.addressOccupations[0].occupiedTo, person.addressOwnerships[i].rent);

                $http.post(serviceBase + '/Api/Landlord/SaveTenant', tenant)
                 .success(function (response) {
                 })
                 .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
        }


        $scope.addNewAddressOccupation = function (person, address, occupiedFrom, occupiedTo, rent) {
            var newOccupation = new function () {
                this.occupiedFrom = new Date(occupiedFrom);
                this.occupiedTo = new Date(occupiedTo);
                this.rent = rent;
                this.address = address;
            }
            person.addressOccupations = [];
            person.addressOccupations.push(newOccupation);

        }


        $scope.getTenantTemplate = function ($q) {
            var deferredObject = $q.defer();
            $http.get(serviceBase + '/Api/Tenant/Template' + '?hash=100').
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

                $http.post(serviceBase + '/Api/Landlord/'+action, $scope.newPerson)
                    .success(function (response) {
                        $scope.newPerson = response;
                        $location.path('/lsignup2');
                    })
                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
            if (step === 2) {
                $http.post(serviceBase + '/Api/Landlord/Update', $scope.newPerson)
                    .success(function (response) {
                        $scope.newPerson = response;
                        $location.path('/lsignup3');
                    })
                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
            if (step === 3) {
                $http.post(serviceBase + '/Api/Landlord/Update', $scope.newPerson)
                    .success(function (response) {
                        $scope.newPerson = response;
                        $location.path('');
                    })
                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
            }
        }

        $scope.addNewAddressOwnership = function () {
            var newAddress = new function () {
                this.addressLine1 = "";
                this.addressLine2 = "";
                this.addressLine3 = "";
                this.city = "";
                this.state = "";
                this.postCode = "";
            }

            var newOwnership = new function () {
                this.ownedFrom = new Date();
                this.ownedTo =  new Date();
                this.address = newAddress;
            }

            $scope.newPerson.addressOwnerships.push(newOwnership);

        }
    }
]);

