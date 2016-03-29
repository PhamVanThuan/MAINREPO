'use strict';

describe('[sahl.websites.halo.services.documentDownloadManager]', function () {

	beforeEach(module('sahl.websites.halo.services.documentDownloadManager'));
	beforeEach(module('sahl.js.core.activityManagement'));
	beforeEach(module('SAHL.Services.Interfaces.DocumentManager.queries'));
	beforeEach(module('SAHL.Services.Interfaces.DocumentManager.sharedmodels'));
	beforeEach(module('sahl.js.ui.notifications'));

	var $documentDownloadManagerService, 
		$documentManagerQueries, 
		$documentManagerWebService, 
		$activityManager,
		$toastManagerService, 
		getDocumentFromStorQuery,
		documentPromise,
		rootScope = "",
		storId = 44,
		documentGuid = "{013584E1-273F-4F75-9064-4E3A365E6AB2}",
		q,
		deferred = null,
		promise;

	
	    beforeEach(inject(function ($injector, $rootScope, $q) {
	        rootScope = $rootScope;
	        q = $q;
	        deferred = q.defer();
	        promise = deferred.promise;
	        $documentManagerQueries = $injector.get('$documentManagerQueries');
	        $documentManagerWebService = $injector.get('$documentManagerWebService');
	        $activityManager = $injector.get('$activityManager');
	        $documentDownloadManagerService = $injector.get('$documentDownloadManagerService');
	        $toastManagerService = $injector.get('$toastManagerService');

	        getDocumentFromStorQuery = new $documentManagerQueries.GetDocumentFromStorByDocumentGuidQuery(storId, documentGuid);
	        spyOn($documentManagerQueries, 'GetDocumentFromStorByDocumentGuidQuery').and.returnValue(getDocumentFromStorQuery);
	        spyOn($documentManagerWebService, 'sendQueryAsync').and.returnValue(promise);
	        spyOn($activityManager, 'startActivityWithKey').and.callThrough();
	        spyOn($activityManager, 'stopActivityWithKey').and.callThrough();
	        spyOn($toastManagerService, "error").and.returnValue(null);
	    }));

	    describe('downloading a document successfully',function(){
	    	 beforeEach(function () {
	    	     $documentDownloadManagerService.downloadDocumentFromStor(storId, documentGuid);
	    	     var dummyElement = document.createElement('a');
	    	     dummyElement.click = function () { };
	    	     document.createElement = jasmine.createSpy('HTML Element').and.returnValue(dummyElement);
             });

	    	it("should start the activty manager", function(){
				expect($activityManager.startActivityWithKey).toHaveBeenCalledWith("loading");
	    	});

	    	it("should retrieve the document from the document manager service", function(){
				expect($documentManagerWebService.sendQueryAsync).toHaveBeenCalled();
	    	});

	    	it("should stop the activty manager", function(){


				var expectedFileContents = "my file contents";
	    		promise.then(function(data){
					expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith("loading");
	    		});

	    		var fakeServerResponse = {
	    				data: { 
	    						ReturnData : { 
	    								Results : { 
    										$values: [
													{
													    FileContentAsBase64: expectedFileContents,
														FileName : "my file name" 
													}  
												] 
										} 
								} 
	    				}
	    			};
	    		deferred.resolve(fakeServerResponse);
	    		rootScope.$apply();
	    	});
	    });
		
	    describe('download document throws an exception during service call',function(){
	    	 beforeEach(function () {
                $documentDownloadManagerService.downloadDocumentFromStor(storId, documentGuid);
             });

             it("should stop the activty manager", function(){
	    		promise.then(null, function(data){
					expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith("loading");
	    		});

	    		deferred.reject({data: { 
	    				SystemMessages: { 
	    						AllMessages : { 
	    							$values: [ { Message: "An unexpected error has bee thrown." } ] 
	    						} 
	    					} 
	    				} 
	    			});
	    		rootScope.$apply();
	    	});

             it("should pushed display an error message to the user", function(){
             	var expectedFileContents = "my file contents";
	    		promise.then(function(data){
					expect($toastManagerService.error).toHaveBeenCalled();
	    		});

	    		var fakeServerResponse = {
	    				data: { 
	    						ReturnData : { 
	    								Results : { 
    										$values: [
													{ 	FileContentAsBase64: null,
														FileName : "" 
													}  
												] 
										} 
								} 
	    				}
	    			};
	    		deferred.resolve(fakeServerResponse);
	    		rootScope.$apply();
	    	});
    	});
});