var tenantsReportMoveCtrl = (function () {
    function tenantsReportMoveCtrl(scope, http, location, q, ngAuthSettings, personData, timeout, Notification, ModalService) {
        this.scope = scope;
        this.http = http;
        this.location = location;
        this.q = q;
        this.ngAuthSettings = ngAuthSettings;
        this.personData = personData;
        this.timeout = timeout;
        this.Notification = Notification;
        this.ModalService = ModalService;
        this.error = "";
        this.info = "";
        this.model = new TVS.API.Models.ReportMoveModel();
        this.person = new TVS.API.Entities.Person();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.proceedToSave = false;
    }
    tenantsReportMoveCtrl.prototype.save = function () {
        var _this = this;
        this.http.post(serviceBase + '/api/Tenant/SaveMove', this.model)
            .then(function (r) {
            _this.error = '';
            _this.info = 'New Address has been updated successfully!';
            _this.location.path('/tenanthome');
            _this.Notification({ message: 'Success', title: 'New Address has been updated successfully!' });
        }, function () {
            _this.error = 'Error occurred. Please try again.';
            _this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
            _this.info = '';
        });
    };
    tenantsReportMoveCtrl.prototype.showSearchPanel = function () {
        var _this = this;
        this.ModalService.showModal({
            templateUrl: "app/views/ModalViews/personSrchRsltModal.html",
            controller: 'personSrchRsltModalCtrl',
            controllerAs: 'vm',
            inputs: {
                title: "Matching people found...",
                people: this.searchResult
            }
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result != null) {
                    _this.person = result;
                    _this.model.landlord.id = _this.person.id;
                    _this.model.landlord.initial = _this.person.initial;
                    _this.model.landlord.firstName = _this.person.firstName;
                    _this.model.landlord.middleName = _this.person.middleName;
                    _this.model.landlord.lastName = _this.person.lastName;
                }
                _this.proceedToSave = true;
            });
        });
    };
    tenantsReportMoveCtrl.prototype.searchAndSave = function () {
        var _this = this;
        this.person = this.model.landlord;
        this.http.post(serviceBase + '/api/Search/Landlord/Search', this.person)
            .then(function (response) {
            _this.searchResult = response.data;
            if (_this.proceedToSave === false && _this.searchResult != undefined && _this.searchResult != null && _this.searchResult.length > 0) {
                _this.showSearchPanel();
                return;
            }
            else {
                _this.save();
            }
        }, function () {
            _this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
        });
    };
    tenantsReportMoveCtrl.$inject = ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', '$timeout', 'Notification', 'ModalService'];
    return tenantsReportMoveCtrl;
})();
app.controller('tenantsReportMoveCtrl', tenantsReportMoveCtrl);
//# sourceMappingURL=reportMove.js.map