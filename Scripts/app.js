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

    $scope.checkDate = function (checkDate) {
        return checkDate < Date.now();
    }

    $scope.openModal = function (type, shift) {
        $scope.selectedShift = shift;
        $scope.type = type;
        $scope.modalInstance = $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'myModalContent.html',
            scope: $scope,
            size: 'lg',
            controller: 'ModalInstanceCtrl',
        }).result.then(function () { }, function () { });
        $scope.getPeople();
        $scope.getPositions();
    };

    $scope.getPeople = function () {
        $http.post('/Person/GetAllActivePeople').then(function (response) {
            $scope.activePeople = response.data;
        }), function (error) {
            console.log(error);
        };
    };

    $scope.getPositions = function () {
        $http.post('/Shift/ViewAllActivePositions').then(function (response) {
            $scope.activePositions = response.data;
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

    //Calendar PopUp Start
    $scope.popup1 = {
        opened: false
    };

    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

    $scope.popup2 = {
        opened: false
    };

    $scope.open2 = function () {
        $scope.popup2.opened = true;
    };

    $scope.popup3 = {
        opened: false
    };

    $scope.open3 = function () {
        $scope.popup3.opened = true;
    };

    $scope.popup4 = {
        opened: false
    };

    $scope.open4 = function () {
        $scope.popup4.opened = true;
    };
    //Calendar PopUp End
});

app.controller('ModalInstanceCtrl', function ($uibModalInstance, $scope, $http) {

    $scope.hell = $scope.selectedShift;
    console.log($scope.selectedShift);
    $scope.selected = {};
    $scope.selected.shiftID = $scope.selectedShift.shiftID;
    $scope.selected.scheduledShiftID = $scope.selectedShift.scheduleShiftID;
    $scope.selected.personID = $scope.selectedShift.personID;
    $scope.selected.positionID = $scope.selectedShift.positionID;
    $scope.selected.startdt = $scope.jsDate($scope.selectedShift.scheduledStartTime);
    $scope.selected.enddt = $scope.jsDate($scope.selectedShift.scheduledEndTime);
    $scope.selected.startlunchdt = $scope.jsDate($scope.selectedShift.scheduledLunchBreakStart);
    $scope.selected.lunchenddt = $scope.jsDate($scope.selectedShift.scheduledLunchBreakEnd);

    $scope.addShift = function (selected) {
        var personID = selected.personID;
        var positionID = selected.positionID;
        var startdt = selected.startdt.getTime();
        var enddt = selected.enddt.getTime();
        var startlunchdt = selected.startlunchdt ? selected.startlunchdt.getTime() : null;
        var endlunchdt = selected.lunchenddt ? selected.lunchenddt.getTime() : null;

        $http.post('/Shift/AddShift', { personID: personID, positionID: positionID, startdt: startdt, enddt: enddt, startlunchdt: startlunchdt, endlunchdt: endlunchdt }).then(function (response) {
            $scope.success = response.data;
            if ($scope.success) {
                alert("Shift added successfully");
                $scope.cancel();
                $scope.refreshView();
            } else {
                alert("There was an error adding your shift. Please try again.");
            }
        }), function (error) {
            alert(error);
        };
    };

    $scope.updateShift = function (selected) {
        var shiftID = selected.shiftID;
        var scheduleShiftID = selected.scheduledShiftID;
        var isDelete = selected.delete;
        var personID = selected.personID;
        var positionID = selected.positionID;
        var startdt = selected.startdt.getTime();
        var enddt = selected.enddt.getTime();
        var startlunchdt = selected.startlunchdt ? selected.startlunchdt.getTime() : null;
        var endlunchdt = selected.lunchenddt ? selected.lunchenddt.getTime() : null;

        $http.post('/Shift/UpdateShift', {
            personID: personID, positionID: positionID, startdt: startdt, enddt: enddt, startlunchdt: startlunchdt,
            endlunchdt: endlunchdt, isDelete: isDelete, shiftID: shiftID, scheduleshiftID: scheduleShiftID
        }).then(function (response) {
            $scope.success = response.data;
            if ($scope.success) {
                alert("Shift updated successfully");
                $scope.cancel();
                $scope.refreshView();
            } else {
                alert("There was an error updating your shift. Please try again.");
            }
        }), function (error) {
            alert(error);
        };
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.refreshView = function () {
        $http.post('/Home/Index').then(function () {
            //This is calling the view but it isn't refreshing it?
            console.log("Refresh View");
            document.location.reload();
        }), function (error) {
            alert(error);
        };
    }

});