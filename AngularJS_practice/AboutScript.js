(function(){
	
	var app = angular.module("About",[]);
	var MainController = function($scope,$log){
			$scope.array_message = "Outputing an array of integers";
			$scope.ints = [5,4,6,2,1,3];
			$log.info("My name is Vishav");
			$scope.title = "These are all the ways you can use loops";
			$scope.strings = ["Vishav","Taka","Jessica","Vincent","BB","Clinton"];
			//var sortedInts = $scope.ints.sort(); //private copy of ints
			//$scope.SortedInt = $scope.ints.sort();

			var sortedStrings = function(){
					return $scope.strings.sort();
			};

			//how to make this work?
			var sortPoints = function(){

				for(var i=0; i<$scope.points.length; i++)
				{
					for(var j=0; j<$scope.points.length; j++)
					{
						if($scope.points[i]>$scope.points[j])
						{
							$scope.temp = $scope.points[i];
							$scope.points[i] = $scope.points[j];
							$scope.points[j] = $scope.temp;
						}
					}
				}
			};

			
			$scope.temp= $scope.points[0];
			$scope.points = [40, 100, 1, 5, 25, 10];
			$scope.points.sort(function(a, b){return b-a}); //sort in descending
			$scope.points.sort(function(a, b){return a-b}); //sort in ascending
			$scope.sortedStrings = sortedStrings();
			
		};		
		app.controller("MainController",MainController);
})();