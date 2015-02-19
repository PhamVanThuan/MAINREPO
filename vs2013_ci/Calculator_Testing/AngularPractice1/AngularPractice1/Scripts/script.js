(function () {

    var app = angular.module("gitHubViewer", []);
    var MainController = function ($scope, $http) {

        var onUserComplete = function (response) {
            $scope.user = response.data;
            $http.get($scope.user.repos_url)
            .then(onRepos, onError);
        };

        var repoError = function (response) {
            $scope.repo_error = response.data;
        };


        var onError = function (reason) {
            $scope.error = "Could not fetch the data";
        };

        var onRepos = function (response, username) {
            $scope.repos = response.data;
            //$http.get("https://api.github.com/users/"+username+"/repos") //static address
            //.then(onUserComplete, onError);
        };

        $scope.search = function (username) {
            $http.get("https://api.github.com/users/" + username) //dynamic address
              .then(onUserComplete, onError);
        };

        $scope.errorMsg = "Unable to retrieve info, out of http requests";
        $scope.errorMsg_Pic = "Unable to retrieve portrait, out of http requests";
        $scope.message = "GitHub & Gravatar profile Spy";
    };
    app.controller("MainController", ["$scope", "$http", MainController]);
    //register the controller, must be registered outside the controller definition
})();