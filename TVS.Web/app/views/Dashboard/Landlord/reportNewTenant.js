var landlordsReportNewTenantCtrl = (function () {
    function landlordsReportNewTenantCtrl(scope, http, location, q, ngAuthSettings, personData, upload, timeout, Notification, ModalService) {
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
        this.ModalService = ModalService;
        this.error = "";
        this.log = "";
        this.success = false;
        this.person = new TVS.API.Entities.Person();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.proceedToSave = false;
        this.http.get(serviceBase + '/api/landlord/myaddresses?hash=' + Math.random())
            .then(function (response) {
            _this.addresses = response.data;
        }, function () {
            _this.Notification.error({ message: 'Could not retrieve your owned addresses. Please try later.', delay: 1000 });
        });
    }
    landlordsReportNewTenantCtrl.prototype.showSearchPanel = function () {
        var _this = this;
        this.ModalService.showModal({
            templateUrl: "app/views/ModalViews/personSrchRsltModal.html",
            controller: 'personSrchRsltModalCtrl',
            controllerAs: 'vm',
            inputs: {
                title: "Search Tenants...",
                people: this.searchResult
            }
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result != null)
                    _this.person = result;
                _this.proceedToSave = true;
            });
        });
    };
    landlordsReportNewTenantCtrl.prototype.searchAndSave = function () {
        var _this = this;
        this.http.post(serviceBase + '/api/Search/Landlord/Search', this.person)
            .then(function (response) {
            _this.searchResult = response.data;
            if (_this.proceedToSave === false && _this.searchResult != undefined && _this.searchResult != null && _this.searchResult.length > 0) {
                _this.showSearchPanel();
                return;
            }
            else {
                _this.saveRequest();
            }
        });
    };
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
        var commonFunc = new Helpers.CommonFunctions();
        return commonFunc.fullAddress(address);
    };
    landlordsReportNewTenantCtrl.prototype.clearForm = function () {
        this.person = new TVS.API.Entities.Person();
        this.proceedToSave = false;
        this.selectedAddressId = undefined;
        this.scope.$broadcast('show-errors-reset');
    };
    landlordsReportNewTenantCtrl.$inject = ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', 'Upload', '$timeout', 'Notification', 'ModalService'];
    return landlordsReportNewTenantCtrl;
})();
app.controller('landlordsReportNewTenantCtrl', landlordsReportNewTenantCtrl);
//# sourceMappingURL=reportNewTenant.js.map