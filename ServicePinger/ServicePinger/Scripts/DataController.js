(function () {

    var ServiceI = angular.module("ServiceI", []);
    var DataController = function ($scope, $http) {

        var onUserComplete = function (response) {
            $scope.user = response.data;
            $http.get($scope.user.service_url)
            .then(serviceList, onError);
        };

        var repoError = function (response) {
            $scope.error_context = response.data;
        };

        var onError = function (reason) {
            $scope.error = "Could not connect.";
        };

        var serviceList = function (response, machineName) {
            $scope.serviceList = response.data;
            $http.get() //static address
            .then(onUserComplete, onError);
        };

        $scope.search = function (machineName) {
            $http.get(machineName+":3000") //dynamic address
              .then(onUserComplete, onError);
        };

        $scope.errorMsg = "Unable to connect, out of http requests";
        $scope.message = "Service Center";
    };
    app.controller("DataController", ["$scope", "$http", DataController]);
    //register the controller, must be registered outside the controller definition
})();