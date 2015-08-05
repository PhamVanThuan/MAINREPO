(function () {
    var app = angular.module("app", ['smart-table']);

    
    var MainController = function (name, $scope) {
        $scope.firstName = "Vishav";
        $scope.name = name; //username
        alert("Welcome " + name);
    }

    app.controller("MainController", MainController);

    var DateController = function ($scope) {
        $scope.date = new Date();
    };

    app.controller("DateController", DateController);

})();