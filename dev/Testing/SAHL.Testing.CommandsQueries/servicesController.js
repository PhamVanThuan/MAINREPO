servicesApp.controller('servicesController',['$scope','$http','$q',function($scope, $http, $q) {

		var createCmdsQueriesModels = function(service, callback){
			httpRequest(service.route,$http, function(jsFile) {
				var functions = cleanupJS(jsFile);
				for (f in functions) {
					var func = functions[f];
					var postUrl = "";
					if (service.route.indexOf("Models") > -1) {
						var model = new Model(func)
						callback(model);
					}
					if (service.route.indexOf("Commands") > -1)
					{
						var postUrl = 'http://'+service.host+'/'+service.name+'/api/CommandHttpHandler/performhttpcommand';
						var command = new Command(func, postUrl);
						callback(command);
					}
					if (service.route.indexOf("Queries") > -1)
					{
						var postUrl = 'http://'+service.host+'/'+service.name+'/api/QueryHttpHandler/PerformHttpQuery';	
						var query = new Query(func,postUrl);
						callback(query);
					}
				}
			});
		};

		var createServices = function(host, callback){
			httpRequest('/routeConfig',$http,function(scriptsRouteConfig){
				for (var i = scriptsRouteConfig.services.length - 1; i >= 0; i--) {
					var service = new Service(scriptsRouteConfig.services[i],host);
					callback(service);
				};
			});
		}

		$scope.executeCommandsQueries = function(commandsQueries, models) {
			
			for (var x = commandsQueries.length - 1; x >= 0; x--) {
				for (var y = commandsQueries[x].params.length - 1; y >= 0; y--) {
					var param =  commandsQueries[x].params[y];
					console.log('param value',param);
					try{
						param.value = JSON.parse(param.value);
					}catch(err){
						//intentional as there is no way to know what type the param value is.
					}
				};

				commandsQueries[x].func= commandsQueries[x].func.toString();


				httpRequest('/performCommandOrQuery', $http, function(data){

					var currentCommandsQuery = null;

					//find and set command response
					for (var r = commandsQueries.length - 1; r >= 0; r--) {
						if (commandsQueries[r].name == data.name){
							currentCommandsQuery = commandsQueries[r];
						}
					};
					
					data.response =  JSON.parse(data.response);
					console.log('data.response',data.response);

					//called with every property and it's value
					function process(key,value,indented) {
						var lineItem = {value: indented + key + " : "+value};
					   currentCommandsQuery.response.push(lineItem);
					}

					function traverse(o,func,indented) {
					    for (var i in o) {
					        func.apply(this,[i,o[i],indented]);  
					        if (o[i] !== null && typeof(o[i])=="object") {
					            //going on step down in the object tree!!
					            indented += "	";
					            traverse(o[i],func,indented);
					        }
					    }
					}

					//that's all... no magic, no bloated framework
					traverse(data.response, process, "");

				},commandsQueries[x]);
			};
		
		};
		
		$scope.serverName = "localhost";
		$scope.changeServer = function (serverName){
			$scope.paramValue = "";
			$scope.paramValue2 = "";
			$scope.selectedService = undefined;
			$scope.selectedCommandsQueries = new Array();
			$scope.services = new Array();
			createServices(serverName,function(service){
				$scope.services.push(service);
				createCmdsQueriesModels(service,function(obj){
					if (obj.name.indexOf("Model") == -1){
						service.commandsQueries.push(obj);
					}else{
						service.models.push(obj);
					}
				});
			});
		};
		$scope.changeServer($scope.serverName);
}]);
