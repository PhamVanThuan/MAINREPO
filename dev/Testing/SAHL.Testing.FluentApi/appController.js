app.controller('AppCtrl',function (metadataUIElements,$http,$scope) {
	$scope.metadataUI = metadataUIElements;
	console.log('metadataUI',$scope.metadataUI);
	$scope.execute = function(){
		console.log('executing...');
		traverseMetadataUI($scope.metadataUI,function(metadataUI){
			if (metadataUI.enabled ) {
				if (metadataUI.metadata.isFunction){
					var parameterArray = new Array();
			        for (var i = 0; i < metadataUI.children.length; i++) {
			            parameterArray.push(metadataUI.children[i].user.inputOutput);
			        };
			        var funcToCall = metadataUI.metadata.description;
			        var results =  funcToCall.apply(null,parameterArray);
			       
			        metadataUI.user.inputOutput  = JSON.stringify(results);
			        if (metadataUI.canAppend(results)){
			        	metadataUI.append(results);
			        }
				}
			}
		});
     }
});