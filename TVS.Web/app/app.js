
var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

app.config(function ($routeProvider) {
    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/landlordhome", {
        controller: "landlordHomeCtrl",
        templateUrl: "/app/views/Dashboard/Landlord/home.html"
    });


    $routeProvider.when("/tenanthome", {
        controller: "tenantHomeCtrl",
        templateUrl: "/app/views/dashboard/tenant/home.html"
    });


    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/tsignup", {
        controller: "signupController",
        templateUrl: "/app/views/tsignup.html"
    });

    $routeProvider.when("/lsignup", {
        controller: "signupController",
        templateUrl: "/app/views/lsignup.html"
    });

    $routeProvider.when("/lsignup1", {
        controller: "signUpLandlordController",
        templateUrl: "/app/views/Register/landlord/step1.html"
    });


    $routeProvider.when("/lsignup2", {
        controller: "signUpLandlordController",
        templateUrl: "/app/views/Register/landlord/step2.html"
    });


    $routeProvider.when("/lsignup3", {
        controller: "signUpLandlordController",
        templateUrl: "/app/views/Register/landlord/step3.html"
    });


    $routeProvider.when("/tsignup1", {
        controller: "signUpTenantController",
        templateUrl: "/app/views/Register/tenant/step1.html"
    });


    $routeProvider.when("/tsignup2", {
        controller: "signUpTenantController",
        templateUrl: "/app/views/Register/tenant/step2.html"
    });


    $routeProvider.when("/tsignup3", {
        controller: "signUpTenantController",
        templateUrl: "/app/views/Register/tenant/step3.html"
    });

    $routeProvider.when("/orders", {
        controller: "ordersController",
        templateUrl: "/app/views/orders.html"
    });

    $routeProvider.when("/postLogin", {
        controller: "postLoginController",
        templateUrl: "/app/views/postLogin.html"
    });

    $routeProvider.when("/refresh", {
        controller: "refreshController",
        templateUrl: "/app/views/refresh.html"
    });

    $routeProvider.when("/tokens", {
        controller: "tokensManagerController",
        templateUrl: "/app/views/tokens.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/app/views/associate.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

var serviceBase = 'http://localhost:26264/';
//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

