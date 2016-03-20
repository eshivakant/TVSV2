app.controller(LandlordsAddNewAddressCtrl);

class LandlordsAddNewAddressCtrl {
    public success: boolean;
    public error: string;
    public person: TVS.API.Entities.Person;
    public title: string;
    public log: string;
    public previewPhoto;
    private serviceBase: string;


    public addresses: Array<TVS.API.Entities.Address>;
    public newAddress: TVS.API.Entities.Address;
    public ownedFrom:Date;

    static $inject = ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', 'Upload', '$timeout', 'Notification'];
    constructor(
        private scope: ng.IScope,
        private http: ng.IHttpService,
        private location,
        private q,
        private ngAuthSettings,
        private personData,
        private upload,
        private timeout,
        private Notification
    ) {

        this.error = "";
        this.log = "";
        this.success = false;
        this.person = new TVS.API.Entities.Person();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;

        this.newAddress = new TVS.API.Entities.Address();

        this.http.get(serviceBase + '/api/landlord/myaddresses?hash='+Math.random())
            .then((response) => {
                this.addresses = <Array<TVS.API.Entities.Address>>response.data;
            },
            () => {
                this.Notification.error({ message: 'Could not retrieve your owned addresses. Please try later.', delay: 1000 });
            });

    }


    public saveRequest() {

        var addressOwnership = new TVS.API.Entities.AddressOwnership();
        addressOwnership.address = this.newAddress;
        addressOwnership.ownedFrom = this.ownedFrom;

        this.http.post(serviceBase + '/api/landlord/newaddress', addressOwnership)
            .then(() => {
                this.success = true;
                this.error = "";
                this.Notification({ message: 'Success', title: 'Request submitted!' });
                this.location.path('/postLogin');
            }, () => {
                this.success = false;
                this.error = "Oops, server error occurred!";
                this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
            });


    }

    public fullAddress(address: TVS.API.Entities.Address): string {
        //var add = address.addressLine1 + ', ' + address.addressLine2 + ', ' + address.addressLine3 + ', ' + address.city + ', ' + address.state;
        //if (address.postCode != undefined && address.postCode != null && address.postCode !== '') add = add + ', ' + address.postCode;
        //add = add.replace("  ", " ").replace(",,", ",").replace(", ,", ",");
        //return add;

        var commonFunc = new CommonFunctions();
        return commonFunc.fullAddress(address);
    }



}

