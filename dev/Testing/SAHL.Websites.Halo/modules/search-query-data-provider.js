var http = require('http');
module.exports = function(criteria,searchQueryClassName,indexName) {
	var deferred = protractor.promise.defer();
	var options = {
	  host: 'localhost',
	  path: '/searchService/api/QueryHttpHandler/PerformHttpQuery?&currentPage=1&pageSize=10',
	  method: 'POST',
	  headers: {'Accept': 'application/json, text/plain, */*'}
	};
	var req = http.request(options,function(response){
		var data = ''
  		response.on('data', function (chunk) {
  			data += chunk;
  		});
  		response.on('end', function () {
  			data = JSON.parse(data);
    		deferred.fulfill(data);
  		});
  		response.on('uncaughtException', function (err) {
    		deferred.reject(err);
		}); 
	});
	var requestData =JSON.stringify({"queryText":criteria,"filters":[],"indexName":indexName,"_name":"SAHL.Services.Interfaces.Search.Queries."+searchQueryClassName+",SAHL.Services.Interfaces.Search"});
	req.write(requestData);
	req.end();
	return deferred.promise;
};