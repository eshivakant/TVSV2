'use strict';
app.controller('landlordsSearchCtrl', ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', 'ModalService', function ($scope, $http, $location, $q, ngAuthSettings, personData, ModalService) {
    
    $scope.newPerson = {};
    $scope.searchResult = [];
    $scope.hideSearchPane = false;

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    $http.get(serviceBase + '/Api/Tenant/Template' + '?hash=100') //to avoid caching
        .then(function (response) {
            $scope.newPerson = response.data;
        });

    $scope.search = function() {
        $http.post(serviceBase + '/api/Search/Landlord/Search', $scope.newPerson)
            .then(function(response) {
                $scope.searchResult = response.data;
            });

        $scope.hideSearchPane = true;


      
    }

    $scope.fuckyou = function () {
        ModalService.showModal({
            templateUrl: "app/views/ModalViews/personSearchModal.html",
            controller: 'personSrchModalCtrl',
            inputs: {
                title: "A More Complex Example"
            }
    }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                $scope.yesNoResult = result;
            });
        });
    }

    $scope.showDetails=function(person) {
        $location.path('/tprofile/id/'+ person.id);
    }

    $scope.showPanel = function () {
        $scope.hideSearchPane = false;
    }

    $scope.fullName = function (person) {
        if (person.middleName == undefined) person.middleName = "";
        var name = person.initial + " " + person.firstName + " " + person.middleName + " " + person.lastName;
        return name.replace("  ", " ");
    }

    $scope.fullAddress = function (address) {
        var name = address.addressLine1 + ", " + address.addressLine2 + ", " + address.addressLine3 + ", " + address.city + ", " + address.state + " - " + address.postCode;
        return name.replace("  ", " ").replace(",,", ",");
    }


}]);