'use strict';
app.controller('homeController', ['$scope', '$location', 'authService', 'personData', function ($scope, $location, authService, personData) {
   
    $scope.authentication = authService.authentication;

    $scope.navigateToDashboard = function() {
        if (personData.role === 'Tenant')
            $location.path('/tenanthome');
        if (personData.role === 'Landlord')
            $location.path('/landlordhome');
        
    };

}]);