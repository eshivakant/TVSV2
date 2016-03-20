app.controller(LandlordsProfileCtrl);
var LandlordsProfileCtrl = (function () {
    function LandlordsProfileCtrl(scope, http, $location, $routeParams, $q, ngAuthSettings, personData) {
        var _this = this;
        this.scope = scope;
        this.http = http;
        this.$location = $location;
        this.$routeParams = $routeParams;
        this.$q = $q;
        this.ngAuthSettings = ngAuthSettings;
        this.personData = personData;
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        var url = '';
        if ($routeParams.personId == undefined) {
            url = serviceBase + '/Api/Landlord/Template' + '?hash=' + Math.random();
            this.isMyProfile = true;
        }
        else
            url = serviceBase + '/Api/Landlord/Profile/' + $routeParams.personId;
        this.http.get(url)
            .then(function (response) {
            _this.person = response.data;
        });
    }
    LandlordsProfileCtrl.prototype.fullAddress = function (address) {
        var add = address.addressLine1 + ', ' + address.addressLine2 + ', ' + address.addressLine3 + ', ' + address.city + ', ' + address.state;
        if (address.postCode != undefined && address.postCode != null && address.postCode !== '')
            add = add + ', ' + address.postCode;
        add = add.replace("  ", " ").replace(",,", ",").replace(", ,", ",");
        return add;
    };
    LandlordsProfileCtrl.$inject = ['$scope', '$http', '$location', '$routeParams', '$q', 'ngAuthSettings', 'personData'];
    return LandlordsProfileCtrl;
})();
