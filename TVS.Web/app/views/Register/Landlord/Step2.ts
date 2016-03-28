
class signUpLandlordStep2Ctrl {

    public error: string;
    public info: string;
    public serviceBase: string;
    private searchResult: Array<TVS.API.Entities.Person>;
    public lRegModel: TVS.API.Models.LandlordRegistration;
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

        this.lRegModel = new TVS.API.Models.LandlordRegistration();
        this.lRegModel.person = new TVS.API.Entities.Person();
        this.lRegModel.ownedAddresses = new Array<TVS.API.Entities.Address>();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.init();

    }

    public init() {

        this.http.get(serviceBase + '/Api/Landlord/Template' + '?hash=' + Math.random()) //to avoid caching
            .then(response => {
                this.lRegModel.person = <TVS.API.Entities.Person>response.data;
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
        this.lRegModel.ownedAddresses = new Array<TVS.API.Entities.Address>();

        for (var i = 0; i < this.flatModels.length; i++) {

            var a = this.flatModels[i].address;
            a.addressOccupations = new Array<TVS.API.Entities.AddressOccupation>();
            var o = new TVS.API.Entities.AddressOccupation();
            o.occupiedFrom = this.flatModels[i].occupiedFrom;
            o.occupiedTo = this.flatModels[i].occupiedTo;
            o.person = this.flatModels[i].person;
            a.addressOccupations.push(o);

            
            var myOwnership = new TVS.API.Entities.AddressOwnership();
            myOwnership.personId = this.lRegModel.person.id;
            myOwnership.ownedFrom = this.flatModels[i].ownedFrom;
            myOwnership.ownedTo = this.flatModels[i].ownedTo;
            a.addressOwnerships = new Array<TVS.API.Entities.AddressOwnership>();
            a.addressOwnerships.push(myOwnership);


            this.lRegModel.ownedAddresses.push(a);
        }
        
        this.http.post(serviceBase + '/Api/Landlord/RegisterStep2', this.lRegModel)
            .success(()=> {
                this.Notification({ message: 'Success', title: 'profile updated successfully!' });
                this.location.path('/landlordhome');

            })
            .error(() => { this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 }) });
        
    }

    private newFlatModel() {
        var m = new TVS.API.Models.PersonAddressFlatModel();
        m.address = new TVS.API.Entities.Address();
        m.person = new TVS.API.Entities.Person();
        return m;
    }

    private addNewAddressOwnership() {
        this.flatModels.push(this.newFlatModel());
    }

    public searchAndAddNewAddressOwnership() {
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
                    this.addNewAddressOwnership();
                }
            },
            () => {
                this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
            });
    }


}

app.controller('signUpLandlordStep2Ctrl', signUpLandlordStep2Ctrl);



