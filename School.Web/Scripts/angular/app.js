var studentsApp = angular.module('studentsApp', [
  'ngRoute',
  'controllers'
]);

studentsApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
    when('/list', {
        templateUrl: 'partials/list.html',
        controller: 'StudentsListingController'
    }).
    when('/details/:itemId', {
        templateUrl: 'partials/details.html',
        controller: 'StudentDetailsController'
    }).
    otherwise({
        redirectTo: '/list'
    });
}]);