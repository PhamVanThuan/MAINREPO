'use strict';

describe('[sahl.websites.halo.services.attorneysManager]', function () {

	beforeEach(module('sahl.websites.halo.services.attorneysManager'));
	beforeEach(module('SAHL.Services.Query.rest'));

	var $attorneysManagerService, 
		$q, 
		$queryServiceRest = {}, 
		$queryWebService = {},
		rootScope,
		attorneysPromise,
		getQueryAsync = {};
	    var deferred = null;

	
	    beforeEach(inject(function ($injector,$rootScope) {
	        $q = $injector.get('$q');
	        deferred = $q.defer();
	        rootScope = $rootScope;
	        $queryServiceRest = $injector.get('$queryServiceRest');
	        $queryWebService = $injector.get('$queryWebService');
	        $attorneysManagerService = $injector.get('$attorneysManagerService');

	        spyOn($queryWebService, 'getQueryAsync').and.returnValue(deferred.promise);
	    }));

	    describe('get attorneys',function(){
	    	 beforeEach(function () {
                attorneysPromise = $attorneysManagerService.getAttorneys();
             });

	    	it("should call the query service to get a list of attorneys", function(){
				expect($queryWebService.getQueryAsync).toHaveBeenCalledWith($queryServiceRest.api.attorneys);
	    	});

	    	it("should return the attorneys list when the promise is resolved", function(){
	    		var expectedResult = "1234";
	    		attorneysPromise.then(function(data){
	    			expect(data.results).toBe(expectedResult);
	    		});

	    		deferred.resolve({data:expectedResult});
	    		rootScope.$apply();
	    	});
	    });
});