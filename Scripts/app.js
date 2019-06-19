console.log("I'm here");

var app = angular.module("app", []);
app.controller("appCtrl", function ($scope, $http) {
    $scope.test = "hello";
    $scope.getShifts = function () {
        $http.post('/Shift/ViewAllShifts').then(function (response) {
            console.log(response);
            $scope.shift = response.data;
        }), function (error) {
            console.log(error);
        };
    };
    $scope.getShifts();
});