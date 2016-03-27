'use strict';
app.controller('signUpTenantController', [
    '$scope', '$http', '$q', '$location', 'ngAuthSettings', 'Notification', 'ModalService', function ($scope, $http, $q, $location, ngAuthSettings,Notification, ModalService) {

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
                //this.address = address;
                this.addressId = address.id;

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




        
        $scope.showSearchPanel = function() {
            ModalService.showModal({
                templateUrl: "app/views/ModalViews/personSrchRsltModal.html",
                controller: 'personSrchRsltModalCtrl',
                controllerAs: 'vm',
                inputs: {
                    title: "Matching people found...",
                    people: this.searchResult
                }
            }).then(function(modal){
                modal.element.modal();
                modal.close.then(function(result) {
                    if (result != null) {
                        var commonFunc = new Helpers.CommonFunctions();
                        commonFunc.copyNonEmptyPersonAttributes(result, $scope.newPerson);
                    }

                    $scope.proceedToSave = true;
                });
            });
        }


        $scope.searchResult = [];
        $scope.proceedToSave = false;

        $scope.searchAndSave = function (step) {
            if (step === 1) {
                var isNewPerson = $scope.newPerson.id === 0;
                if (!isNewPerson) {
                    $scope.savePerson(step);
                    return;
                }

                $http.post(serviceBase + '/api/Search/Landlord/Search', $scope.newPerson)
                    .then(function(response) {
                        $scope.searchResult = response.data;
                        if ($scope.proceedToSave === false && $scope.searchResult != undefined && $scope.searchResult != null && $scope.searchResult.length > 0) {
                            $scope.showSearchPanel();
                            return;
                        } else {
                            $scope.savePerson(step);
                        }
                    },
                    function(){
                        Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
                    });
            }
            else if (step === 2) {
                $scope.savePerson(step);
            }
            else if (step === 3) {
                $scope.savePerson(step);
            }

        }
       

    }
]);

