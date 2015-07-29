(function () {
    var app = angular.module("app", []);
    var MainController = function (name, $scope) {
        $scope.name = name; //username
        alert("Welcome " + name);
    }

    app.controller("MainController", MainController);

    var DateController = function ($scope) {

        $scope.date = new Date();
    };

    app.controller("DateController", DateController);

})();