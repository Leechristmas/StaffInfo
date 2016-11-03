'use strict';

var app = angular.module('StaffinfoApp', ['ui.router', 'LocalStorageModule', 'angular-loading-bar', 'chart.js', 'ngMaterial']);

app.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state('home', {
            url: "/home",
            controller: "homeController",
            templateUrl: "app/views/home.html",
            noLogin: true
        }).state('login', {
            url: "/login",
            controller: "loginController",
            templateUrl: "app/views/login.html",
            noLogin: true
        }).state('signup', {
            url: "/signup",
            controller: "signupController",
            templateUrl: "app/views/signup.html",
            noLogin: false
        }).state('dashboard', {
            url: "/dashboard",
            controller: "dashboardController",
            templateUrl: "app/views/dashboard.html",
            noLogin: false
        }).state('employees', {
            url: "/employees",
            controller: "dashboardController",
            templateUrl: "app/views/employees.html",
            noLogin: false
        });

    $urlRouterProvider.otherwise("/home");
});

app.config(function ($mdThemingProvider) {
    $mdThemingProvider.theme('default')
        .primaryPalette('blue');
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

//http path to the API
var serviceBase = 'http://localhost:21200/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase
});

app.run(['$rootScope', '$state', '$stateParams', 'authService', function ($rootScope, $state, $stateParams, authService) {
    authService.fillAuthData();

    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    $rootScope.user = null;

    // Здесь мы будем проверять авторизацию
    $rootScope.$on('$stateChangeStart',
      function (event, toState, toParams, fromState, fromParams) {
          if (!toState.noLogin) {
              event.preventDefault();
              $rootScope.$state.go('login');
          }
      }
    );
}]);


