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
        $scope.getPeople();
    };

    $scope.cancel = function () {
        $scope.modalInstance.dismiss('cancel');
    };

    $scope.getPeople = function () {
        $http.post('/Person/GetAllActivePeople').then(function (response) {
            console.log(response.data);
            $scope.activePeople =response.data;
        }), function (error) {
            console.log(error);
        };
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
    };

    $scope.popup1 = {
        opened: false
    };

    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

    $scope.setDate = function (year, month, day) {
        $scope.dt = new Date(year, month, day);
    };



    

});