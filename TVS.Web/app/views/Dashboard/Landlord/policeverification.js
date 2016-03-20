app.controller(LandlordsPoliceVerificationCtrl);
var LandlordsPoliceVerificationCtrl = (function () {
    function LandlordsPoliceVerificationCtrl(scope, http, $location, $q, ngAuthSettings, personData, upload, timeout) {
        var _this = this;
        this.scope = scope;
        this.http = http;
        this.$location = $location;
        this.$q = $q;
        this.ngAuthSettings = ngAuthSettings;
        this.personData = personData;
        this.upload = upload;
        this.timeout = timeout;
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
                            //this.log = 'file: ' + resp.config.file.name +', Response: ' + JSON.stringify(resp.data) +'\n' + this.log;
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
        }, function () {
            _this.success = false;
            _this.error = "Oops, server error occurred!";
        });
    };
    LandlordsPoliceVerificationCtrl.$inject = ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', 'Upload', '$timeout'];
    return LandlordsPoliceVerificationCtrl;
})();
