function cleanupJS(js){
	var startingIndex = js.indexOf("var shared");
	var endingIndex = js.indexOf("return shared");
	var functionsStr = js.substring(startingIndex, endingIndex);
	functionsStr = functionsStr.replace("var shared = (","var shared = ");
	functionsStr = functionsStr.replace("}());","};");
	try
	{
		eval(functionsStr);
    	return shared();
	}
	catch(err) {
		throw err;
	}
}

function getCtorArgs(obj,functionName){
	var arguments = Object.keys(obj);

	var str = obj.constructor.toString()
					.replace(functionName,"")
					.replace("(","")
					.replace("function","");
	var strSplit = str.split(')');
	
	return strSplit[0].split(',');
}

function getFunctionName(fun) {
  	var ret = fun.toString();
  	ret = ret.substr('function '.length);
  	ret = ret.substr(0, ret.indexOf('('));

  	return ret;
};

function httpRequest(url, $http, callback, body){
	if (body != undefined){
		$http.post(url,body).success(function (data, status, headers, config) {
			callback(data);
		},function(err){
			callback(err);
		});
	}else{
		$http.get(url).success(function (data, status, headers, config) {
			callback(data);
		},function(err){
			callback(err);
		});
	}
};