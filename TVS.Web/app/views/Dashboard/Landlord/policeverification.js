var LandlordsPoliceVerificationCtrl = (function () {
    function LandlordsPoliceVerificationCtrl(scope, http, location, $routeParams, q, ngAuthSettings, personData, upload, timeout, Notification) {
        var _this = this;
        this.scope = scope;
        this.http = http;
        this.location = location;
        this.$routeParams = $routeParams;
        this.q = q;
        this.ngAuthSettings = ngAuthSettings;
        this.personData = personData;
        this.upload = upload;
        this.timeout = timeout;
        this.Notification = Notification;
        this.error = "";
        this.log = "";
        this.serverFileNames = new Array();
        this.success = false;
        this.model = new TVS.API.Models.VerificationRequestDto;
        this.model.person = new TVS.API.Entities.Person();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.files = new Array();
        this.scope.$watch(function () { return _this.files; }, function () {
            _this.uploadFile(_this.files);
        });
        this.scope.$watch(function () { return _this.file; }, function () {
            if (_this.scope != null)
                _this.uploadFile(_this.file);
        });
        if (this.$routeParams.initString.indexOf('police') > -1)
            this.policeCheckRequired = true;
        if (this.$routeParams.initString.indexOf('civil') > -1)
            this.civilCheckRequired = true;
        if (this.$routeParams.initString.indexOf('credit') > -1)
            this.creditCheckRequired = true;
    }
    LandlordsPoliceVerificationCtrl.prototype.uploadFile = function (files) {
        var _this = this;
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                if (!file.$error) {
                    this.upload.upload({
                        url: serviceBase + '/api/file',
                        file: file
                    }).then(function (resp) {
                        _this.timeout(function () {
                            _this.serverFileNames.push(resp.data.returnData);
                        });
                    }, null, function (evt) {
                        var progressPercentage = parseInt((100.0 * evt.loaded / evt.total).toString());
                        _this.log = 'progress: ' + progressPercentage + '% ' + evt.config.file.name;
                        if (progressPercentage === 100)
                            _this.log = 'Upload Successful!';
                    });
                }
            }
        }
    };
    LandlordsPoliceVerificationCtrl.prototype.saveRequest = function () {
        var _this = this;
        var req = this.model;
        req.verificationRequest = new TVS.API.Entities.VerificationRequest();
        req.verificationRequest.documents = new Array();
        req.verificationRequest.crimeCheck = this.policeCheckRequired;
        req.verificationRequest.creditCheck = this.creditCheckRequired;
        req.verificationRequest.civilCheck = this.civilCheckRequired;
        angular.forEach(this.serverFileNames, function (f) {
            var doc = new TVS.API.Entities.VerificationDocument();
            doc.url = f;
            doc.description = "Verification Request";
            req.verificationRequest.documents.push(doc);
        });
        this.http.post(serviceBase + '/api/misc/lverify', req)
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
    LandlordsPoliceVerificationCtrl.$inject = ['$scope', '$http', '$location', '$routeParams', '$q', 'ngAuthSettings', 'personData', 'Upload', '$timeout', 'Notification'];
    return LandlordsPoliceVerificationCtrl;
})();
app.controller('LandlordsPoliceVerificationCtrl', LandlordsPoliceVerificationCtrl);
