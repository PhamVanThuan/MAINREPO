function Service(serviceConf,host){

	return {
		"name":serviceConf.name,
		"commandsQueries": new Array(),
		"error":"",
		"models": new Array(),
		"route": serviceConf.route,
		"host":host
	};

}

function Command(func,postUrl) {
	
	var params = {
		func: func,
		postUrl :postUrl
	};
	return new Instance(params);
}
function Query(func,postUrl) {

	var params = {
		func: func,
		postUrl :postUrl
	};
	return new Instance(params);
}
function Model(func,postUrl) {

	var params = {
		func: func,
		postUrl :postUrl
	};
	return new Instance(params);
}

function Instance(params){
	var funcName = getFunctionName(params.func);
	var obj =  new params.func();
	var typeInfo = obj._name.split(',')[0];
	
	var ctorArgs = getCtorArgs(obj, funcName);
	for (var i = ctorArgs.length - 1; i >= 0; i--) {
		ctorArgs[i] = { name:ctorArgs[i]};
	};

	return {
		"name":funcName,
		"func":params.func,
		"params":ctorArgs,
		"postUrl":params.postUrl,
		"type":typeInfo.type,
		"response":[]
	};
}
