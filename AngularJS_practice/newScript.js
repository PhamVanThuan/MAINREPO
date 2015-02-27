(function() {

	var app2 = angular.module("app2", []);
	var serviceFunction = function($log,$scope,$interval){
		
	var sayHi = function(){
		$log.info("Hi");
	};

	var decreasingForLoop = function(){
		for (var i = 5; i >= 0; i--) 
		{
			$log.info(i);
		};
		$log.info("End of loop");
	};

	var whileLoop = function(number){
		$log.info("Begin while loop");
		var i=0;
		while(i<number)
		{
			$log.info(i);
			i+=1;
		}
		$log.info("End of while loop");
	};

	var displayMethod = function(text){ //takes in a paramter named text
		alert(text);
		decreasingForLoop();
		whileLoop(5);
	};

	sayHi(); //write to log via sayHi function

	$scope.button_clicked = function(){
		var d = new Date();
		alert("Button was clicked on "+d);
	};		

	var shuffleArray = function(array) {
    var randomNum,temp;
    for(var i=0; i<array.length; i++)
     {
     	randomNum = Math.random()*array.length-1;
     	temp=array[i];
     	array[i] = array[randomNum];
     	temp = array[randomNum];
     }
     viewArray(array);
  	};

	var startCountUp = function(){
      //countupInterval = $interval($scope.incrementCountUp,1000,$scope.countUp);
    	$scope.incrementCountUp();
    };

    $scope.incrementCountUp = function()
    {
        if($scope.countuP<11){
        	$scope.countUp+=1;
    		$log.info($scope.countUp);
    	}
    };
    
	var viewArray = function(array){
		for(var i=0; i < array.length; i++){
			$log.info(array[i]+" at position "+(i+1));
		}
	};

	//var textDisplay = "AlertString";
	//displayMethod(textDisplay); //passing a predefined argument
	//$scope.button_clicked();
	$scope.firstname = "Vishav";
	$scope.surname = "Premlall";
	$scope.countUp = 1;
	startCountUp();
	$scope.countries = ['USA','Germany','United Kingdom','Russia','Spain'];
	$scope.countries.sort();
	$scope.MoreCountries = ['Cyberia','Finland','Britian','Australia','France'];
	
	//shuffleArray($scope.MoreCountries); error in shuffle 
	//viewArray($scope.countries);
	//viewArray($scope.MoreCountries);
	};

	app2.controller("serviceFunction", ["$log","$scope", "$interval", serviceFunction]);

	var NewTestController = function($scope){

		$scope.message = "This is from an additional controller";

	};
	app2.controller("NewTestController", ["$scope", NewTestController]);
})();