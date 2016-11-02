var app = angular.module('StaffinfoApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'chart.js', 'ngMaterial']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/dashboard", {
        controller: "dashboardController",
        templateUrl: "/app/views/dashboard.html"
    });

    $routeProvider.when("/employees", {
        controller: "employeesController",
        templateUrl: "/app/views/employees.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.config(function($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

//http path to the API
var serviceBase = 'http://localhost:21200/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);