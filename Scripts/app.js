console.log("I'm here");

var app = angular.module("app", ['ui.bootstrap']);
app.controller("appCtrl", function ($scope, $http, $uibModal) {
    $scope.test = "hello";
    $scope.getShifts = function () {
        $http.post('/Shift/ViewAllShifts').then(function (response) {
            $scope.shift = response.data;
        }), function (error) {
            console.log(error);
        };
    };
    $scope.getShifts();

    $scope.jsDate = function (dateIn) {
        if (dateIn) {
            return new Date(parseInt(dateIn.substr(6)));
        } else {
            return "";
        }   
    }

    $scope.openModal = function () {
        $scope.modalInstance = $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'myModalContent.html',
            scope: $scope,
            size: 'lg'
        }).result.then(function () { }, function () { });
    };

    $scope.cancel = function () {
        $scope.modalInstance.dismiss('cancel');
    };

    

});