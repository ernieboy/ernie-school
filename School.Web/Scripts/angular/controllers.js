var controllers = angular.module('controllers', []);

controllers.controller('StudentsListingController', ['$scope', '$http', function ($scope, $http) {
    $http.get('/AngularStudents/AllStudents').
        success(function (data, status, headers, config) {
            $scope.students = angular.fromJson(data.json);
        }).
    error(function (data, status, headers, config) {

    });
}]);

controllers.controller('StudentDetailsController', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
    $http.get('/AngularStudents/FindById/' + $routeParams.itemId).success(function (data) {
        $scope.student = angular.fromJson(data.json);
        $scope.whichItem = $routeParams.itemId;
    }).
    error(function (data, status, headers, config) {

    });
}]);