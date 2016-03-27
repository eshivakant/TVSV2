var landlordsMyRatingsCtrl = (function () {
    function landlordsMyRatingsCtrl(scope, http, location, $routeParams, q, ngAuthSettings, personData, upload, timeout) {
        this.scope = scope;
        this.http = http;
        this.location = location;
        this.$routeParams = $routeParams;
        this.q = q;
        this.ngAuthSettings = ngAuthSettings;
        this.personData = personData;
        this.upload = upload;
        this.timeout = timeout;
        this.error = "";
        this.log = "";
        this.success = false;
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.init();
    }
    landlordsMyRatingsCtrl.prototype.init = function () {
        var _this = this;
        //get all people who owned this address
        this.http.get(serviceBase + '/api/Rating/Landlord/MyRatings?hash=' + Math.random())
            .then(function (response) {
            _this.addressRatings = response.data;
            _this.ratingCount = _this.addressRatings.length;
        });
        //get ratings
    };
    landlordsMyRatingsCtrl.prototype.getScoreText = function (score) {
        var cf = new Helpers.CommonFunctions();
        return cf.getScoreText(score);
    };
    landlordsMyRatingsCtrl.$inject = ['$scope', '$http', '$location', '$routeParams', '$q', 'ngAuthSettings', 'personData', 'Upload', '$timeout'];
    return landlordsMyRatingsCtrl;
})();
app.controller('landlordsMyRatingsCtrl', landlordsMyRatingsCtrl);
//# sourceMappingURL=myRatings.js.map