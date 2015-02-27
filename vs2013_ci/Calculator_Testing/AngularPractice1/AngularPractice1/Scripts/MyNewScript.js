(function(){
    //register the app
    var app = angular.module("app", []);

    //create the first controller
    var MainController = function ($scope) {
        $scope.message = "Hello, my name is Vishav";
        $scope.stuff = "I am a UKZN bred BSc Computer Science & ISTN graduate"; //define additional data on the scope like this
        $scope.hidden = "I am going to hack Industrial and Commercial Bank of China";

        var person = {
            firstname: "Vishav",
            surname: "Premlall",
            age: 22,
            height: 1.54,
            weight: 65
        };

        $scope.person = person; //this must come after the object is created
    };

    app.controller("MainController", ["$scope", MainController]);

    //create the second controller
    var SpareController = function ($scope) {

        $scope.message = "Please rate my site";
    };
    app.controller("SpareController", ["$scope", SpareController]);

    var ThirdController = function ($scope, $http) { //declare and initialize
        $scope.message = "1    -     2    -   3    -    4    -   5";
        $scope.date = new Date();

    };
    app.controller("ThirdController", ["$scope", ThirdController]); //register the controller with "app"

})();