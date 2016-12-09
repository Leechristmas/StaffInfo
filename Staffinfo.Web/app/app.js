'use strict';

var app = angular.module('StaffinfoApp', ['ui.router', 'ngMaterial', 'md.data.table', 'LocalStorageModule', 'angular-loading-bar', 'chart.js']);

app.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state('home', {
            url: "/",
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
            controller: "employeesController",
            templateUrl: "app/views/employees.html",
            noLogin: true
        }).state('retirees', {
            url: '/retirees',
            controller: 'retireesController',
            templateUrl: 'app/views/retirees.html',
            noLogin: false
        }).state('dismissed',{
            url: '/dismissed',
            controller: 'dismissedController',
            templateUrl: 'app/views/dismissed.html',
            noLogin: false
        }).state('details', {
            url: "/employees/details",
            controller: 'detailsController',
            templateUrl: 'app/views/employeeView.html',
            noLogin: false
        });

    $urlRouterProvider.otherwise("/");
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

    $rootScope.$on('$stateChangeStart',
      function (event, toState, toParams, fromState, fromParams) {
          if (!toState.noLogin && !authService.isAuthenticated()) {
              event.preventDefault();
              authService.authentication.isAuth = false;//test- it is maybe not working
              $rootScope.$state.go('login');
          }
      }
    );
}]);


