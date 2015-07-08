(function () {

    var app = angular.module("app",[]);

    var dateCtrl = function ($scope) {
        $scope.date = new Date();
    };

    app.controller("dateCtrl", ["$scope", dateCtrl]);
})();