 var  modules = modules || [];
(function () {
    'use strict';
    modules.push('Person');

    angular.module('Person',['ngRoute'])
    .controller('Person_list', ['$scope', '$http', function($scope, $http){

        $http.get('/Api/Person/')
        .then(function(response){$scope.data = response.data;});

    }])
    .controller('Person_details', ['$scope', '$http', '$routeParams', function($scope, $http, $routeParams){

        $http.get('/Api/Person/' + $routeParams.id)
        .then(function(response){$scope.data = response.data;});

    }])
    .controller('Person_create', ['$scope', '$http', '$routeParams', '$location', function($scope, $http, $routeParams, $location){

        $scope.data = {};
        
        $scope.save = function(){
            $http.post('/Api/Person/', $scope.data)
            .then(function(response){ $location.path("Person"); });
        }

    }])
    .controller('Person_edit', ['$scope', '$http', '$routeParams', '$location', function($scope, $http, $routeParams, $location){

        $http.get('/Api/Person/' + $routeParams.id)
        .then(function(response){$scope.data = response.data;});

        
        $scope.save = function(){
            $http.put('/Api/Person/' + $routeParams.id, $scope.data)
            .then(function(response){ $location.path("Person"); });
        }

    }])
    .controller('Person_delete', ['$scope', '$http', '$routeParams', '$location', function($scope, $http, $routeParams, $location){

        $http.get('/Api/Person/' + $routeParams.id)
        .then(function(response){$scope.data = response.data;});
        $scope.save = function(){
            $http.delete('/Api/Person/' + $routeParams.id, $scope.data)
            .then(function(response){ $location.path("Person"); });
        }

    }])

    .config(['$routeProvider', function ($routeProvider) {
            $routeProvider
            .when('/Person', {
                title: 'Person - List',
                templateUrl: '/Static/Person_List',
                controller: 'Person_list'
            })
            .when('/Person/Create', {
                title: 'Person - Create',
                templateUrl: '/Static/Person_Edit',
                controller: 'Person_create'
            })
            .when('/Person/Edit/:id', {
                title: 'Person - Edit',
                templateUrl: '/Static/Person_Edit',
                controller: 'Person_edit'
            })
            .when('/Person/Delete/:id', {
                title: 'Person - Delete',
                templateUrl: '/Static/Person_Delete',
                controller: 'Person_delete'
            })
            .when('/Person/:id', {
                title: 'Person - Details',
                templateUrl: '/Static/Person_Details',
                controller: 'Person_details'
            })
    }])
;

})();
