app.controller(landlordsReportNewTenantCtrl);
var landlordsReportNewTenantCtrl = (function () {
    function landlordsReportNewTenantCtrl(scope, http, location, q, ngAuthSettings, personData, upload, timeout, Notification) {
        var _this = this;
        this.scope = scope;
        this.http = http;
        this.location = location;
        this.q = q;
        this.ngAuthSettings = ngAuthSettings;
        this.personData = personData;
        this.upload = upload;
        this.timeout = timeout;
        this.Notification = Notification;
        this.error = "";
        this.log = "";
        this.success = false;
        this.person = new TVS.API.Entities.Person();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.http.get(serviceBase + '/api/landlord/myaddresses?hash=' + Math.random())
            .then(function (response) {
            _this.addresses = response.data;
        }, function () {
            _this.Notification.error({ message: 'Could not retrieve your owned addresses. Please try later.', delay: 1000 });
        });
    }
    landlordsReportNewTenantCtrl.prototype.saveRequest = function () {
        var _this = this;
        if (this.selectedAddressId == undefined || this.selectedAddressId == 0) {
            this.Notification.error({ message: 'Please select the address for your new tenant', delay: 1000 });
            return;
        }
        var addressOccupy = new TVS.API.Entities.AddressOccupation();
        addressOccupy.address = this.addresses.filter(function (a) { return a.id === _this.selectedAddressId; })[0];
        addressOccupy.addressId = this.selectedAddressId;
        addressOccupy.occupiedFrom = new Date(Date.now());
        this.person.addressOccupations = new Array();
        this.person.addressOccupations.push(addressOccupy);
        this.http.post(serviceBase + '/api/landlord/newtenant', this.person)
            .then(function () {
            _this.success = true;
            _this.error = "";
            _this.Notification({ message: 'Success', title: 'Request submitted!' });
            _this.location.path('/postLogin');
        }, function () {
            _this.success = false;
            _this.error = "Oops, server error occurred!";
            _this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
        });
    };
    landlordsReportNewTenantCtrl.prototype.fullAddress = function (address) {
        //var add = address.addressLine1 + ', ' + address.addressLine2 + ', ' + address.addressLine3 + ', ' + address.city + ', ' + address.state;
        //if (address.postCode != undefined && address.postCode != null && address.postCode !== '') add = add + ', ' + address.postCode;
        //add = add.replace("  ", " ").replace(",,", ",").replace(", ,", ",");
        //return add;
        var commonFunc = new Helpers.CommonFunctions();
        return commonFunc.fullAddress(address);
    };
    landlordsReportNewTenantCtrl.$inject = ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', 'Upload', '$timeout', 'Notification'];
    return landlordsReportNewTenantCtrl;
})();
