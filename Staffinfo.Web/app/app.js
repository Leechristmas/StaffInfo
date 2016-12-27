'use strict';

var app = angular.module('StaffinfoApp', ['ui.router', 'ngMaterial', 'md.data.table', 'fixed.table.header', 'LocalStorageModule', 'angular-loading-bar', 'chart.js', 'ui.rCalendar', 'angularUserSettings']);

app.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state('home', {
            url: "/",
            controller: "loginController",
            templateUrl: "app/views/login.html",
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
        }).state('dismissed', {
            url: '/dismissed',
            controller: 'dismissedController',
            templateUrl: 'app/views/dismissed.html',
            noLogin: false
        }).state('details', {
            url: "/employees/details",
            controller: 'detailsController',
            templateUrl: 'app/views/employeeView.html',
            noLogin: false
        }).state('settings', {
            url: "/settings",
            controller: 'settingsController',
            templateUrl: 'app/views/settingsView.html',
            noLogin: false
        });

    $urlRouterProvider.otherwise("/");
});

//color theme configuring
app.config(function ($mdThemingProvider) {
    $mdThemingProvider
      .theme('default')
      .primaryPalette('blue')
      .accentPalette('indigo')
      .warnPalette('red')
      .backgroundPalette('grey');
});

//datetimepicker format config
app.config(function ($mdDateLocaleProvider) {
    $mdDateLocaleProvider.formatDate = function (date) {
        return moment(date).format('DD.MM.YYYY');
    }
});

//interceptors
app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

//http path to the API
var serviceBase = 'http://localhost:21200/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase
});

app.run(['$rootScope', '$state', '$stateParams', 'authService', 'employeesService', function ($rootScope, $state, $stateParams, authService, employeesService) {
    authService.fillAuthData();

    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    $rootScope.user = null;

    $rootScope.$on('$stateChangeStart',
      function (event, toState, toParams, fromState, fromParams) {
          if (!toState.noLogin && !authService.isAuthenticated()) {
              event.preventDefault();
              $rootScope.$state.go('login');
          }
          if (toState.name === 'details' && !employeesService.employees.actualEmployee.hasOwnProperty('id')) {
              event.preventDefault();
              $rootScope.$state.go('employees');
          }
      }
    );
}]);


