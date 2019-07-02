var app = angular.module("app", ['ui.bootstrap']);

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

    $scope.getPeople = function () {
        $http.post('/Person/GetAllActivePeople').then(function (response) {
            $scope.activePeople = response.data;
        }), function (error) {
            console.log(error);
        };
    };

    $scope.getAllPositions = function () {
        $http.post('/Position/GetAllPositions').then(function (response) {
            $scope.allPositions = response.data;
            console.log($scope.allPositions);
        }), function (error) {
            console.log(error);
        };
    };
    $scope.getAllPositions();

    $scope.getAllTasks = function () {
        $http.post('/Position/GetAllTasks').then(function (response) {
            $scope.allTasks = response.data;
            console.log($scope.allPositions);
        }), function (error) {
            console.log(error);
        };
    };
    $scope.getAllTasks();

    $scope.dateOptions = {
        formatYear: 'yy',
        maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
    };

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

    //Calculation to get total hours scheduled with or without lunch break
    $scope.getTotalHours = function (shift) {
        var startTime = $scope.jsDate(shift.scheduledStartTime);
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

    //Verify that all scheduled dates are in order thus valid
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

    $scope.openPositionModal = function (type, position) {
        $scope.selectedPosition = position;
        $scope.typePosition = type;
        $scope.modalInstance = $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'positionModalContent.html',
            scope: $scope,
            size: 'lg',
            controller: 'PositionModalInstanceCtrl',
        }).result.then(function () { }, function () { });
    };

    $scope.openTaskModal = function (type, task) {
        $scope.selectedTask = task;
        $scope.typeTask = type;
        $scope.modalInstance = $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'taskModalContent.html',
            scope: $scope,
            size: 'lg',
            controller: 'TaskModalInstanceCtrl',
        }).result.then(function () { }, function () { });
    };

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
                    $scope.getShifts();
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
                    $scope.getShifts();
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

});

app.controller('PositionModalInstanceCtrl', function ($uibModalInstance, $scope, $http) {

    $scope.selected = {};
    $scope.selected.positionID = $scope.selectedPosition.positionID;
    $scope.selected.pTitle = $scope.selectedPosition.positionTitle;
    $scope.selected.pDesc = $scope.selectedPosition.positionDescription;
    $scope.selected.pActive = $scope.selectedPosition.isActive ? $scope.selectedPosition.isActive : true;

    $scope.addPosition = function (selected) {
        var positionTitle = selected.pTitle;
        var positionDescription = selected.pDesc;
        var pIsActive = selected.pActive;

        $http.post('/Position/AddPosition', { positionTitle: positionTitle, positionDescription: positionDescription, isActive: pIsActive }).then(function (response) {
            $scope.success = response.data;
            if ($scope.success) {
                alert("Position added successfully");
                $scope.cancel();
                $scope.getAllPositions();
            } else {
                alert("There was an error adding your position. Please try again.");
            }
        }), function (error) {
            alert(error);
        };

    };

    $scope.updatePosition = function (selected) {
        var positionID = selected.positionID;
        var positionTitle = selected.pTitle;
        var positionDescription = selected.pDesc;
        var pIsActive = selected.pActive;

        $http.post('/Position/UpdatePosition', { positionTitle: positionTitle, positionDescription: positionDescription, isActive: pIsActive, id: positionID }).then(function (response) {
            $scope.success = response.data;
            if ($scope.success) {
                alert("Position updated successfully");
                $scope.cancel();
                $scope.getAllPositions();
            } else {
                alert("There was an error updating your position. Please try again.");
            }
        }), function (error) {
            alert(error);
        };
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

 });

app.controller('TaskModalInstanceCtrl', function ($uibModalInstance, $scope, $http) {

    $scope.selected = {};
    $scope.selected.taskID = $scope.selectedTask.TaskId;
    $scope.selected.positionID = $scope.selectedTask.PositionID;
    $scope.selected.tTitle = $scope.selectedTask.Task_title;
    $scope.selected.tDesc = $scope.selectedTask.Task_description;
    $scope.selected.tActive = $scope.selectedTask.IsActive ? $scope.selectedTask.IsActive : true;

    $scope.addTask = function (selected) {
        var tTitle = selected.tTitle;
        var tDescription = selected.tDesc;
        var tIsActive = selected.tActive;
        var positionID = selected.positionID;

        $http.post('/Position/AddTaskPosition', { taskTitle: tTitle, taskDescription: tDescription, isActive: tIsActive, positionID: positionID }).then(function (response) {
            $scope.success = response.data;
            if ($scope.success) {
                alert("Task added successfully");
                $scope.cancel();
                $scope.getAllTasks();
            } else {
                alert("There was an error adding your task. Please try again.");
            }
        }), function (error) {
            alert(error);
        };

    };

    $scope.updateTask = function (selected) {
        var tId = selected.taskID;
        var tTitle = selected.tTitle;
        var tDescription = selected.tDesc;
        var tIsActive = selected.tActive;
        var positionID = selected.positionID;

        $http.post('/Position/UpdateTaskPosition', { id: tId, taskTitle: tTitle, taskDescription: tDescription, isActive: tIsActive, positionID: positionID }).then(function (response) {
            $scope.success = response.data;
            if ($scope.success) {
                alert("Task updated successfully");
                $scope.cancel();
                $scope.getAllTasks();
            } else {
                alert("There was an error adding your task. Please try again.");
            }
        }), function (error) {
            alert(error);
        };
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

});