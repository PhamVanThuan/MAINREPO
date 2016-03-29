module.exports = function(applicationName){

	var sql = require('mssql');
	var ptor = protractor.getInstance();

	var config = {
	    user: ptor.params['dbConfig'].user,
	    password: ptor.params['dbConfig'].password,
	    server: ptor.params['dbConfig'].server,
	    database: ptor.params['dbConfig'].database
	};
	applicationName.queryDB = function(sqlQuery, tokens, callback){
		if(tokens){
			sqlQuery = tokenReplace(sqlQuery, tokens);
		};
		sql.connect(config, function(err) {
	    	var request = new sql.Request();
	    	request.on('done', function(){
	    		sql.close();
	    	});
	    	request.query(sqlQuery, function(err, recordset) {		
				if(recordset && !err)
				{
					callback(err, recordset);
				}
				else
				{
					expect("Query returned no records: " + sqlQuery).toBeFalsy();
				};
	    	});
		});
	};
	function tokenReplace(query, tokens){
		for(var i = 0; i < tokens.length; i++){
			query = query.replace(new RegExp(escapeRegExp("[" + tokens[i].name + "]"), 'g'), tokens[i].value);
		}
		return query;
	};
	function escapeRegExp(string) {
    return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
	}	
	applicationName.transaction = function(callback){
		var tran;
		sql.connect(config, function(err) {
			tran = new sql.Transaction();
			tran.on('commit',function(){
				console.log("tran commited");
				sql.close();
			});
			tran.on('rollback',function(){
				console.log("tran rolled back");
				sql.close();
			});
			tran.begin(function(err){
				console.log("begin tran");
				if(!err && callback)
					callback(err)
				else{
					console.log(err);
				};
			});
		});

		query = function(sqlQuery, tokens, callback){
			if(tokens){
				sqlQuery = tokenReplace(sqlQuery, tokens);
				console.log(sqlQuery);
			};			
			var request = new sql.Request(tran);
			request.on('done',function(returnValue){
				console.log("query complete: " + returnValue)
			});
			console.log("execute query");			
			request.query(sqlQuery, function(err, recordset){
				if(!err && callback)
					callback(err, recordset)
				else if(err){
					console.log(err);
				};
			});
		};

		commit = function(){
			tran.commit();
		};

		rollback = function(){
			tran.rollback();
		};
	};
};

