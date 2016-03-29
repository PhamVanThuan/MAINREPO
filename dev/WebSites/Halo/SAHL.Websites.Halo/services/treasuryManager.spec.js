'use strict';

describe('[sahl.websites.halo.services.treasuryManager]', function () {

	beforeEach(module('sahl.websites.halo.services.treasuryManager'));
	beforeEach(module('SAHL.Services.Query.rest'));

	var $treasuryManagerService, 
		$q, 
		$queryServiceRest = {}, 
		$queryWebService = {},
		rootScope,
		treasuryPromise,
		getQueryAsync = {};
	    var deferred = null;

	
	    beforeEach(inject(function ($injector,$rootScope) {
	        $q = $injector.get('$q');
	        deferred = $q.defer();
	        rootScope = $rootScope;
	        $queryServiceRest = $injector.get('$queryServiceRest');
	        $queryWebService = $injector.get('$queryWebService');
	        $treasuryManagerService = $injector.get('$treasuryManagerService');

	        spyOn($queryWebService, 'getQueryAsync').and.returnValue(deferred.promise);
	    }));

	    describe('when retrieving spv list',function(){
	    	 beforeEach(function () {
                treasuryPromise = $treasuryManagerService.getSPVs();
             });

	    	it("should call the query service to get a list of SPV's", function(){
				expect($queryWebService.getQueryAsync).toHaveBeenCalledWith($queryServiceRest.api.treasury.spvs);
	    	});

	    	it("should return the SPV's list when the promise is resolved", function(){
	    		var expectedResult = "Spv result data";
	    		treasuryPromise.then(function(data){
	    			expect(data.results).toBe(expectedResult);
	    		});

	    		deferred.resolve({data:expectedResult});
	    		rootScope.$apply();
	    	});
	    });
});