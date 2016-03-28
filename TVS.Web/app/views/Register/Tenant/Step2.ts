
class signUpTenantStep2Ctrl {

    public error: string;
    public info: string;
    public serviceBase: string;
    private searchResult: Array<TVS.API.Entities.Person>;
    
    
    public tRegModel: TVS.API.Models.TenantRegistration;
    public flatModels: Array<TVS.API.Models.PersonAddressFlatModel>;

    static $inject = ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', '$timeout', 'Notification', 'ModalService'];
        constructor(
        private scope: ng.IScope,
        private http: ng.IHttpService,
        private location,
        private q,
        private ngAuthSettings,
        private personData,
        private timeout,
        private Notification,
        private ModalService
    ) {

        this.error = "";
        this.info = "";

        this.flatModels = new Array<TVS.API.Models.PersonAddressFlatModel>();
        this.flatModels.push(this.newFlatModel());

        this.tRegModel = new TVS.API.Models.TenantRegistration();
        this.tRegModel.person = new TVS.API.Entities.Person();
        this.tRegModel.previousLandlords = new Array<TVS.API.Entities.Person>();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.init();

    }

    public init() {

        this.http.get(serviceBase + '/Api/Tenant/Template' + '?hash=' + Math.random()) //to avoid caching
            .then(response => {
                this.tRegModel.person = <TVS.API.Entities.Person>response.data;
            }, () => this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 }));
    }



    public showSearchPanel(m: TVS.API.Models.PersonAddressFlatModel) {
        this.ModalService.showModal({
            templateUrl: "app/views/ModalViews/personSrchRsltModal.html",
            controller: 'personSrchRsltModalCtrl',
            controllerAs: 'vm',
            inputs: {
                title: "Matching people found...",
                people: this.searchResult
            }
        }).then((modal)=> {
            modal.element.modal();
            modal.close.then((result)=> {
                if (result != null) {
                    var commonFunc = new Helpers.CommonFunctions();
                    commonFunc.copyNonEmptyPersonAttributes(result, m.person);
                }

                m.searchDone = true;
            });
        });
    }




    public searchAndSave() {
        var m: TVS.API.Models.PersonAddressFlatModel;
        for (var i = 0; i < this.flatModels.length; i++) {
            m = this.flatModels[i];
            if (m.searchDone) continue;
            m = this.flatModels[i];
        }

        this.http.post(serviceBase + '/api/Search/Landlord/Search', m.person)
            .then((response) => {
                this.searchResult = <Array<TVS.API.Entities.Person>>response.data;

                if (!m.searchDone && this.searchResult != undefined && this.searchResult != null && this.searchResult.length > 0) {
                    this.showSearchPanel(m);
                    return;
                } else {
                    this.savePerson();
                }
            },
            () => {
                this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
            });
    }
    

    private savePerson() {
        //push new address ownerships
        this.tRegModel.previousLandlords = new Array<TVS.API.Entities.Person>();
        for (var i = 0; i < this.flatModels.length; i++) {
            var o = new TVS.API.Entities.AddressOwnership();
            o.ownedFrom = this.flatModels[i].occupiedFrom; //owned  = occupied since tenant won't know about other period
            o.ownedTo = this.flatModels[i].occupiedTo;
            o.address = this.flatModels[i].address;
            this.flatModels[i].person.addressOwnerships = new Array<TVS.API.Entities.AddressOwnership>();
            this.flatModels[i].person.addressOwnerships.push(o);
            

            o.address.addressOccupations = new Array<TVS.API.Entities.AddressOccupation>();
            var oc = new TVS.API.Entities.AddressOccupation();
            oc.occupiedFrom = this.flatModels[i].occupiedFrom;
            oc.occupiedTo = this.flatModels[i].occupiedTo;
            oc.personId = this.tRegModel.person.id;
            o.address.addressOccupations.push(oc);

            this.tRegModel.previousLandlords.push(this.flatModels[i].person);

        }
        
        this.http.post(serviceBase + '/Api/Tenant/RegisterStep2', this.tRegModel)
            .success(()=> {
                this.Notification({ message: 'Success', title: 'profile updated successfully!' });
                this.location.path('/tenanthome');

            })
            .error(() => { this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 }) });
        
    }

    private newFlatModel() {
        var m = new TVS.API.Models.PersonAddressFlatModel();
        m.address = new TVS.API.Entities.Address();
        m.person = new TVS.API.Entities.Person();
        return m;
    }

    private addNewAddressOccupation() {
        this.flatModels.push(this.newFlatModel());
    }

    public searchAndAddNewAddressOccupation() {
        var m: TVS.API.Models.PersonAddressFlatModel;
        for (var i = 0; i < this.flatModels.length; i++) {
            m = this.flatModels[i];
            if (m.searchDone) continue;
            m = this.flatModels[i];
        }

        this.http.post(serviceBase + '/api/Search/Landlord/Search', m.person)
            .then((response) => {
                this.searchResult = <Array<TVS.API.Entities.Person>>response.data;

                if (!m.searchDone && this.searchResult != undefined && this.searchResult != null && this.searchResult.length > 0) {
                    this.showSearchPanel(m);
                    return;
                } else {
                    this.addNewAddressOccupation();
                }
            },
            () => {
                this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
            });
    }


}

app.controller('signUpTenantStep2Ctrl', signUpTenantStep2Ctrl);





//'use strict';
//app.controller('signUpTenantController', [
//    '$scope', '$http', '$q', '$location', 'ngAuthSettings', 'Notification', 'ModalService', function ($scope, $http, $q, $location, ngAuthSettings,Notification, ModalService) {

//        $scope.newPerson = {};

//        var serviceBase = ngAuthSettings.apiServiceBaseUri;

//        $http.get(serviceBase + '/Api/Tenant/Template'+'?hash=' + Math.random()) //to avoid caching
//            .then(function(response) {
//                $scope.newPerson = response.data;
//                $scope.getLandlordTemplate($q).then(function (result) {
//                    var landlordTemplate = result;
//                    for (var i = 0; i < $scope.newPerson.addressOccupations.length; i++) {
//                        $scope.newPerson.addressOccupations[i].previousLandlord = JSON.parse(JSON.stringify(landlordTemplate));
//                    }
//                });
//            });



//        $scope.savePreviousLandlords = function () {
//            var person = $scope.newPerson;
//            for (var i = 0; i < person.addressOccupations.length; i++) {
//                var landlord = person.addressOccupations[i].previousLandlord;
//                landlord.placeOfBirth = "NA";
//                $scope.addNewAddressOwnership(landlord, person.addressOccupations[i].address, person.addressOccupations[i].occupiedFrom, person.addressOccupations[i].occupiedTo);

//                $http.post(serviceBase + '/Api/Tenant/SaveLandlord', landlord)
//                 .success(function (response) {
//                        $location.path("");
//                    })
//                 .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
//            }
//        }


//        $scope.addNewAddressOwnership = function (person, address, ownedFrom, ownedTo) {
//            var newOwnership = new function () {
//                this.ownedFrom = new Date(ownedFrom);
//                this.ownedTo = new Date(ownedTo);
//                //this.address = address;
//                this.addressId = address.id;

//            }
//            person.addressOwnerships = [];
//            person.addressOwnerships.push(newOwnership);

//        }


//        $scope.getLandlordTemplate = function ($q) {
//            var deferredObject = $q.defer();
//            $http.get(serviceBase + '/Api/Landlord/Template' + '?hash=100').
//            success(function (data) {
//                deferredObject.resolve(data);
//            }).
//            error(function () {
//                deferredObject.resolve(data);
//            });

//            return deferredObject.promise;
//        }



//        $scope.registrationError = "";
        
        
//        $scope.savePerson = function (step) {
//            if (step === 1) {

//                var action = $scope.newPerson.id === 0 ? "Save" : "Update";

//                $http.post(serviceBase + '/Api/Tenant/'+action, $scope.newPerson)
//                    .success(function (response) {
//                        $scope.newPerson = response;
//                        $location.path('/tsignup2');
//                    })
//                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
//            }
//            if (step === 2) {
//                $http.post(serviceBase + '/Api/Tenant/Update', $scope.newPerson)
//                    .success(function (response) {
//                        $scope.newPerson = response;
//                        $location.path('/tsignup3');
//                    })
//                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
//            }
//            if (step === 3) {
//                $http.post(serviceBase + '/Api/Tenant/Update', $scope.newPerson)
//                    .success(function (response) {
//                        $scope.newPerson = response;
//                        $location.path('');
//                    })
//                    .error(function () { $scope.registrationError = "failed to save due to errors in the form" });
//            }
//        }

//        $scope.addNewAddressOccupation = function () {
//            var newAddress = new function () {
//                this.addressLine1 = "";
//                this.addressLine2 = "";
//                this.addressLine3 = "";
//                this.city = "";
//                this.state = "";
//                this.postCode = "";
//            }

//            var newOccpuation = new function () {
//                this.occupiedFrom = new Date();
//                this.occupiedTo = new Date();
//                this.address = newAddress;
//                this.rent = "";
//            }

//            $scope.newPerson.addressOccupations.push(newOccpuation);

//        }




        
//        $scope.showSearchPanel = function() {
//            ModalService.showModal({
//                templateUrl: "app/views/ModalViews/personSrchRsltModal.html",
//                controller: 'personSrchRsltModalCtrl',
//                controllerAs: 'vm',
//                inputs: {
//                    title: "Matching people found...",
//                    people: this.searchResult
//                }
//            }).then(function(modal){
//                modal.element.modal();
//                modal.close.then(function(result) {
//                    if (result != null) {
//                        var commonFunc = new Helpers.CommonFunctions();
//                        commonFunc.copyNonEmptyPersonAttributes(result, $scope.newPerson);
//                    }

//                    $scope.proceedToSave = true;
//                });
//            });
//        }


//        $scope.searchResult = [];
//        $scope.proceedToSave = false;

//        $scope.searchAndSave = function (step) {
//            if (step === 1) {
//                var isNewPerson = $scope.newPerson.id === 0;
//                if (!isNewPerson) {
//                    $scope.savePerson(step);
//                    return;
//                }

//                $http.post(serviceBase + '/api/Search/Landlord/Search', $scope.newPerson)
//                    .then(function(response) {
//                        $scope.searchResult = response.data;
//                        if ($scope.proceedToSave === false && $scope.searchResult != undefined && $scope.searchResult != null && $scope.searchResult.length > 0) {
//                            $scope.showSearchPanel();
//                            return;
//                        } else {
//                            $scope.savePerson(step);
//                        }
//                    },
//                    function(){
//                        Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
//                    });
//            }
//            else if (step === 2) {
//                $scope.savePerson(step);
//            }
//            else if (step === 3) {
//                $scope.savePerson(step);
//            }

//        }
       

//    }
//]);





