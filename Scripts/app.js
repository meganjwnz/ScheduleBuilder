﻿var app = angular.module("app", ['ui.bootstrap']);

app.controller("appCtrl", function ($scope, $http, $uibModal) {

    $scope.test = "hello";

    $scope.getSessionID = function () {
        $scope.sessionID = document.getElementById("sessionIDForAngular").value;
        return $scope.sessionID;
    };

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

    $scope.getTotalHours = function (shift) {
        var startTime = $scope.jsDate(shift.scheduledStartTime);  // schedule date start
        var endTime = $scope.jsDate(shift.scheduledEndTime);

        if (shift.scheduledLunchBreakStart && shift.scheduledLunchBreakEnd) {
            var lunchTime = $scope.jsDate(shift.scheduledLunchBreakStart);
            var lunchEnd = $scope.jsDate(shift.scheduledLunchBreakEnd);
            var firstHours = lunchTime.getTime() - startTime.getTime();
            var secondHours = endTime.getTime() - lunchEnd.getTime();
            var totalHours = (secondHours + firstHours) / (1000 * 60 * 60);

        } else {
            var totalHours = (endTime.getTime() - startTime.getTime()) / (1000 * 60 * 60);
        }

        return totalHours.toFixed(2);

    };

    $scope.checkDateOrder = function (start, end, lunch, lunchEnd) {
        if ((lunch && !lunchEnd) || (lunchEnd && !lunch)) {
            alert("Lunch Break must have a start and end date and time");
            return false;
        } else if (lunch && lunchEnd) {
            if (end > lunchEnd && lunchEnd > lunch && lunch > start) {
                return true;
            } else {
                alert("Lunch date start must be before lunch break end. The entire lunch break must be inbetween shift start and end.");
                return false;
            }
        } else {
            if (end > start) {
                return true;
            } else {
                alert("Start date and time must be before end date and time.");
                return false;
            }
        }
    }

});

app.controller('ModalInstanceCtrl', function ($uibModalInstance, $scope, $http) {

    $scope.hell = $scope.selectedShift;
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

        if ($scope.checkDateOrder(startdt, enddt, startlunchdt, endlunchdt) == false) {
            return;
        } else {
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

        if ($scope.checkDateOrder(startdt, enddt, startlunchdt, endlunchdt) == false) {
            return;
        } else {
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
        }
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.refreshView = function () {
        $http.post('/Home/Index').then(function () {
            $scope.getShifts();
        }), function (error) {
            alert(error);
        };
    }

});