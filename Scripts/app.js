console.log("I'm here");

var app = angular.module("app", ['ui.bootstrap']);
app.controller("appCtrl", function ($scope, $http, $uibModal) {
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

    $scope.jsDate = function (dateIn) {
        if (dateIn) {
            return new Date(parseInt(dateIn.substr(6)));
        } else {
            return "";
        }
        
    }



    $scope.open = function () {
        var modalInstance = $uibModal.open({
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: 'lg',
            resolve: {
                items: function () {
                    return $scope.test;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };

});