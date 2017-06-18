'use strict';

var app = angular.module('StaffinfoApp', ['ui.router', 'ui.mask', 'ngMaterial', 'angular-page-loader', 'material.components.expansionPanels', 'ngMessages', 'md.data.table', 'fixed.table.header', 'LocalStorageModule', 'angular-loading-bar', 'chart.js', 'ui.rCalendar', 'angularUserSettings', 'ngIdle']);

app.config(function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state('login', {
            url: "/login",
            controller: "loginController",
            templateUrl: "app/views/login.html",
            noLogin: true
        }).state('signup', {
            url: "/signup",
            controller: "signupController",
            templateUrl: "app/views/signup.html",
            noLogin: true,
            allowedRoles: ['admin']
        }).state('dashboard', {
            url: "/dashboard",
            controller: "dashboardController",
            templateUrl: "app/views/dashboard.html",
            noLogin: false,
            allowedRoles: ['admin', 'editor', 'reader']
        }).state('employees', {
            url: "/employees",
            controller: "employeesController",
            templateUrl: "app/views/employees.html",
            noLogin: false,
            allowedRoles: ['admin', 'editor', 'reader']
        }).state('retirees', {
            url: '/retirees',
            controller: 'retireesController',
            templateUrl: 'app/views/retirees.html',
            noLogin: false,
            allowedRoles: ['admin', 'editor', 'reader']
        }).state('dismissed', {
            url: '/dismissed',
            controller: 'dismissedController',
            templateUrl: 'app/views/dismissed.html',
            noLogin: false,
            allowedRoles: ['admin', 'editor', 'reader']
        }).state('details', {
            url: "/employees/details",
            controller: 'detailsController',
            templateUrl: 'app/views/employeeView.html',
            noLogin: false,
            allowedRoles: ['admin', 'editor']
        }).state('settings', {
            url: "/settings",
            controller: 'settingsController',
            templateUrl: 'app/views/settingsView.html',
            noLogin: false,
            allowedRoles: ['admin', 'editor', 'reader']
        }).state('reporting', {
            url: "/reporting",
            controller: "reportingController",
            templateUrl: 'app/views/reportingView.html',
            noLogin: false,
            allowedRoles: ['admin', 'editor', 'reader']
        }).state('users', {
            url: "/users",
            controller: "userController",
            templateUrl: 'app/views/usersView.html',
            noLogin: false,
            allowedRoles: ['admin']
        });

    $urlRouterProvider.otherwise("/dashboard");
});

//http path to the API
var serviceBase = 'http://localhost:21200/';//'http://staffinfoapi.azurewebsites.net/'; 
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase
});
//idle config
app.constant('idleConfig', {
    idle: 120,
    timeout: 10,
    interval: 50
});

//idle
app.config([
    'KeepaliveProvider', 'IdleProvider', 'idleConfig', function (KeepaliveProvider, IdleProvider, idleConfig) {
        IdleProvider.idle(idleConfig.idle); //idle time
        IdleProvider.timeout(idleConfig.timeout);
        KeepaliveProvider.interval(idleConfig.interval);
    }
]);

//mask
app.config([
    'uiMask.ConfigProvider', function(uiMaskConfigProvider) {
        uiMaskConfigProvider.maskDefinitions({ '9': /\d/, 'A': /[a-z]/, '*': /[a-zA-Z0-9]/ });
        uiMaskConfigProvider.clearOnBlur(false);
        uiMaskConfigProvider.eventsToHandle(['input', 'keyup', 'click']);
    }
]);

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
app.config(function ($mdDateLocaleProvider) {
    $mdDateLocaleProvider.parseDate = function (dateString) {
        var m = moment(dateString, 'DD.MM.YYYY', true);
        return m.isValid() ? m.toDate() : new Date(NaN);
    }
});

//interceptors
app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['$rootScope', '$state', '$stateParams', 'authService', 'employeesService', 'localStorageService', 'Idle', '$mdToast', function ($rootScope, $state, $stateParams, authService, employeesService, localStorageService, Idle, $mdToast) {
    //Idle.watch();

    authService.fillAuthData();

    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    $rootScope.user = null;

    //checks if the user has permission for moving to the state
    var checkRoles = function (userRoles, allowedRoles) {
        userRoles = angular.fromJson(userRoles);
        var isAllowed = false;

        userRoles.forEach(r => {
            if (allowedRoles.includes(r)) {
                isAllowed = true;
                return;
            }
        });
        return isAllowed;
    }

    $rootScope.$on('$stateChangeStart',
      function (event, toState, toParams, fromState, fromParams) {

          if (toState.name !== 'login') {

              Idle.watch();
              if (!checkRoles(authService.authentication.roles, toState.allowedRoles)) {
                  event.preventDefault();
                  $mdToast.show({
                      hideDelay: 3000,
                      position: 'top right',
                      controller: 'toastController',
                      template: '<md-toast class="md-toast-success">' +
                                      '<div class="md-toast-content">' +
                                        'У Вас недостаточно прав!' +
                                      '</div>' +
                                  '</md-toast>'
                  });
              }
          }
          else {
              if (!authService.isAuthenticated() && !Idle.isExpired())
                  Idle.unwatch();
          }

          if (!toState.noLogin && !authService.isAuthenticated()) {
              event.preventDefault();
              $rootScope.$state.go('login');
          }

          if (toState.name === 'login') {//SERVER session time has expired!
              if (!localStorageService.get('authorizationData'))
                  authService.logOut();
          }
      }
    );
}]);

Date.prototype.withoutTime = function () {
    var d = new Date(this);
    d.setHours(0, 0, 0, 0, 0);
    return d;
}


