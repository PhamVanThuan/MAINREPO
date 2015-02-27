(function() {

  var app = angular.module("gitHubViewer", []);
  var MainController = function($scope, $http, $interval, $log) {

    var onUserComplete = function(response) {
      $scope.user = response.data;
      $http.get($scope.user.repos_url)
      .then(onRepos, onError);
    };
    
    var repoError  = function(response){
      $scope.repo_error = response.data;
    };

    var onError = function(reason) {
      $scope.error = "Could not fetch the data";

    };

    var onRepos = function(response,username){
      $scope.repos = response.data;
      //$http.get("https://api.github.com/users/"+username+"/repos") //dynamic address
        //.then(onUserComplete, onError);
    };

    var countdownInterval = null;
    $scope.search = function(username) {
      $log.info("Searching for "+username);
      $http.get("https://api.github.com/users/"+username+"") //dynamic address
        .then(onUserComplete, onError);
        if(countdownInterval){
          $interval.cancel(countdownInterval);
          $scope.countdown = null;
        }
    };

    var decrementCountdown = function()
    {
        $scope.countdown-=1;
        if($scope.countdown < 1){
          $scope.search($scope.username);
        }
    };
    
    
    var startCountdown = function(){
      countdownInterval = $interval(decrementCountdown,1000,$scope.countdown);
    };
    
    $scope.username="VishavP";
    $scope.errorMsg = "Unable to retrieve info, out of http requests";
    $scope.errorMsg_Pic = "Unable to retrieve portrait, out of http requests";
    $scope.message = "GitHub profile Spy";
    $scope.countdown=10;
    startCountdown();

  };
  app.controller("MainController", ["$scope", "$http", "$interval", "$log", MainController]); //or app.controller("MainController",MainController);
  //register the controller, must be registered outside the controller definition
})();