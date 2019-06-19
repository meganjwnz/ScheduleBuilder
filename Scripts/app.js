console.log("I'm here");

var app = angular.module("app", []);
app.controller("appCtrl", function ($scope, $http) {
    $scope.test = "hello";
    $scope.getShifts = function () {
        $http.get('Main/Test').then(function (response) {
            $scope.peeps = response;
        }), function (error) {
            console.log(error);
        };
    };

$scope.getShifts();
});