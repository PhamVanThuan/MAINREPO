'use strict';

describe('[sahl.websites.halo.services.thirdPartyInvoiceManager]', function () {

	beforeEach(module('sahl.websites.halo.services.thirdPartyInvoiceManager'));
	beforeEach(module('SAHL.Services.Query.rest'));
	beforeEach(module('SAHL.Services.Interfaces.FinanceDomain.sharedmodels'));
	beforeEach(module('sahl.js.core.lookup'));
	beforeEach(module('sahl.websites.halo.services.lookup'));
	beforeEach(module('SAHL.Services.Interfaces.DocumentManager.queries'));
	beforeEach(module('SAHL.Services.Interfaces.DocumentManager.sharedmodels'));

		var $thirdPartyInvoiceManagerService, 
			$financeDomainWebService,
			$financeDomainCommands, 
			$q,
	        $queryServiceRest, 
	        $queryWebService, 
	        $activityManager, 
	        $documentManagerQueries, 
	        $documentManagerWebService, 
	        $lookupService,
	        rootScope;

	    var deferred,
	    	promise = null,
	    	invoiceId = 100;
	
	    beforeEach(inject(function ($injector,$rootScope) {
	        $q = $injector.get('$q');
	        deferred = $q.defer();
	        promise = deferred.promise;
	        rootScope = $rootScope;

	        $financeDomainCommands = $injector.get('$financeDomainCommands');
	        $financeDomainWebService = $injector.get('$financeDomainWebService');
	        $queryServiceRest = $injector.get('$queryServiceRest'); 
	        $queryWebService = $injector.get('$queryWebService'); 
	        $activityManager = $injector.get('$activityManager'); 
	        $documentManagerQueries = $injector.get('$documentManagerQueries'); 
	        $documentManagerWebService = $injector.get('$documentManagerWebService'); 
	        $lookupService = $injector.get('$lookupService');

	        $thirdPartyInvoiceManagerService = $injector.get('$thirdPartyInvoiceManagerService');
	    }));

	    describe('get invoice data',function(){
	    	this.getInvoiceDataQuery = "";
	    	this.getInvoiceDataQueryPromise = "";

	    	beforeEach(function () {
	    		this.getInvoiceDataQuery = $queryServiceRest.api.finance.thirdpartyinvoices.getById(invoiceId);

		        spyOn($queryServiceRest.api.finance.thirdpartyinvoices, 'getById').and.returnValue(this.getInvoiceDataQuery);
	    	 	spyOn($queryWebService,'getQueryAsync').and.returnValue(deferred.promise);

                this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoiceDataById(invoiceId);
             });

	    	it("should call the query service to get the invoice data", function(){
				expect($queryWebService.getQueryAsync).toHaveBeenCalled();
	    	});

	    	it("should return the invoice data when the promise is resolved", function(){
	    		var expectedResult = "invoice metadata";
	    		this.getInvoiceDataQueryPromise.then(function(data){
	    			expect(data.results).toBe(expectedResult);
	    		});

	    		deferred.resolve({data:expectedResult});
	    		rootScope.$apply();
	    	});
	    });

	    describe('get invoice line items',function(){
	    	this.getInvoiceLineItemsQuery = "";
	    	this.getInvoiceLineItemsPromise = "";

	    	beforeEach(function () {
	    		this.getInvoiceLineItemsQuery = $queryServiceRest.api.finance.thirdpartyinvoices.getById(invoiceId).lineItems;

		        spyOn($queryServiceRest.api.finance.thirdpartyinvoices.getById(invoiceId), 'lineItems').and.returnValue(this.getInvoiceLineItemsQuery);
	    	 	spyOn($queryWebService,'getQueryAsync').and.returnValue(deferred.promise);

                this.getInvoiceLineItemsPromise = $thirdPartyInvoiceManagerService.getInvoiceLineItems(invoiceId);
             });

	    	it("should call the query service to get the invoice line items", function(){
				expect($queryWebService.getQueryAsync).toHaveBeenCalled();
	    	});

	    	it("should return the invoice line items when the promise is resolved", function(){
	    		var expectedResult = "line item data";
	    		this.getInvoiceLineItemsPromise.then(function(data){
	    			expect(data.results).toBe(expectedResult);
	    		});

	    		deferred.resolve(expectedResult);
	    		rootScope.$apply();
	    	});
	    });

	    describe('get invoice lookup categories',function(){
	    	this.getLookUpLineItemTypesPromise = "";

	    	beforeEach(function () {

	    	 	spyOn($lookupService,'getByLookupType').and.returnValue(deferred.promise);

                this.getLookUpLineItemTypesPromise = $thirdPartyInvoiceManagerService.getLookupLineItemTypes();
             });

	    	it("should call the lookup service to get the looks up categories", function(){
				expect($lookupService.getByLookupType).toHaveBeenCalled();
	    	});

	    	it("should return the lookup categories when the promise is resolved", function(){
	    		var expectedResult = "lookup types data";
	    		this.getLookUpLineItemTypesPromise.then(function(data){
	    			expect(data).toBe(expectedResult);
	    		});

	    		deferred.resolve(expectedResult);
	    		rootScope.$apply();
	    	});
	    });

	    describe('get invoice lookup category description items',function(){
	    	this.getLookUpLineItemDescriptionsPromise = "";
	    	this.categoryId = 1;

	    	beforeEach(function () {

	    	 	spyOn($lookupService,'getByLineItemCategoryId').and.returnValue(deferred.promise);

                this.getLookUpLineItemDescriptionsPromise = $thirdPartyInvoiceManagerService.getLookUpLineItemDescriptions(this.categoryId);
             });

	    	it("should call the lookup service to get the looks category descriptsoins", function(){
				expect($lookupService.getByLineItemCategoryId).toHaveBeenCalledWith(this.categoryId);
	    	});

	    	it("should return the lookup category descriptions when the promise is resolved", function(){
	    		var expectedResult = "lookup category descriptions data";
	    		this.getLookUpLineItemDescriptionsPromise.then(function(data){
	    			expect(data).toBe(expectedResult);
	    		});

	    		deferred.resolve(expectedResult);
	    		rootScope.$apply();
	    	});
	    });

	    describe('amend third party invoice',function(){
	    	this.amendThirdPartyInvoicePromise = "";
	    	this.command = null;
	    	this.thirdPartyInvoiceModel = { id : 100, lineItems : [ {id: 1, category:2, description: 'Attorney Fees'} ] };

	    	beforeEach(function () {
	    		this.command = new $financeDomainCommands.AmendThirdPartyInvoiceCommand(this.thirdPartyInvoiceModel);

	    	 	spyOn($financeDomainCommands,'AmendThirdPartyInvoiceCommand').and.returnValue(this.command);
	    	 	spyOn($financeDomainWebService,'sendCommandAsync').and.returnValue(deferred.promise);

                this.amendThirdPartyInvoicePromise = $thirdPartyInvoiceManagerService.amendThirdPartyInvoice(this.thirdPartyInvoiceModel);
             });

	    	it("should send a command to the finance domain to amend the invoice", function(){
				expect($financeDomainCommands.AmendThirdPartyInvoiceCommand).toHaveBeenCalledWith(this.thirdPartyInvoiceModel);
	    	});

	    	it("should return without errors", function(){
	    		var expectedResult = { data : { SystemMessages : "Success" }};
	    		this.amendThirdPartyInvoicePromise.then(function(data){
	    			expect(data.results).toBe(expectedResult.data.SystemMessages);
	    		});

	    		deferred.resolve(expectedResult);
	    		rootScope.$apply();
	    	});
	    });

	    describe('amend third party invoice returns error messages',function(){
	    	this.amendThirdPartyInvoicePromise = "";
	    	this.command = null;
	    	this.thirdPartyInvoiceModel = { id : 100, lineItems : [ {id: 1, category:2, description: 'Attorney Fees'} ] };

	    	beforeEach(function () {
	    		this.command = new $financeDomainCommands.AmendThirdPartyInvoiceCommand(this.thirdPartyInvoiceModel);

	    	 	spyOn($financeDomainCommands,'AmendThirdPartyInvoiceCommand').and.returnValue(this.command);
	    	 	spyOn($financeDomainWebService,'sendCommandAsync').and.returnValue(deferred.promise);

                this.amendThirdPartyInvoicePromise = $thirdPartyInvoiceManagerService.amendThirdPartyInvoice(this.thirdPartyInvoiceModel);
             });

	    	it("should send a command to the finance domain to amend the invoice", function(){
				expect($financeDomainCommands.AmendThirdPartyInvoiceCommand).toHaveBeenCalledWith(this.thirdPartyInvoiceModel);
	    	});

	    	it("should return reject the promise and return the error messages", function(){
	    		var expectedResult = { data : { SystemMessages : { AllMessages  : { $values : "Errors" } } } };
	    		this.amendThirdPartyInvoicePromise.then(null,function(data){
	    			expect(data.results).toBe(expectedResult.data.SystemMessages.AllMessages.$values);
	    		});

	    		deferred.reject(expectedResult);
	    		rootScope.$apply();
	    	});
	    });

	    describe('capture third party invoice',function(){
	    	this.CaptureThirdPartyInvoicePromise = "";
	    	this.command = null;
	    	this.thirdPartyInvoiceModel = { id : 100, lineItems : [ {id: 1, category:2, description: 'Attorney Fees'} ] };

	    	beforeEach(function () {
	    		this.command = new $financeDomainCommands.CaptureThirdPartyInvoiceCommand(this.thirdPartyInvoiceModel);

	    	 	spyOn($financeDomainCommands,'CaptureThirdPartyInvoiceCommand').and.returnValue(this.command);
	    	 	spyOn($financeDomainWebService,'sendCommandAsync').and.returnValue(deferred.promise);

                this.CaptureThirdPartyInvoicePromise = $thirdPartyInvoiceManagerService.captureThirdPartyInvoice(this.thirdPartyInvoiceModel);
             });

	    	it("should send a command to the finance domain to capture the invoice", function(){
				expect($financeDomainCommands.CaptureThirdPartyInvoiceCommand).toHaveBeenCalledWith(this.thirdPartyInvoiceModel);
	    	});

	    	it("should return without errors", function(){
	    		var expectedResult = { data : { SystemMessages : "Success" }};
	    		this.CaptureThirdPartyInvoicePromise.then(function(data){
	    			expect(data.results).toBe(expectedResult.data.SystemMessages);
	    		});

	    		deferred.resolve(expectedResult);
	    		rootScope.$apply();
	    	});
	    });

	    describe('capture third party invoice returns errors',function(){
	    	this.captureThirdPartyInvoicePromise = "";
	    	this.command = null;
	    	this.thirdPartyInvoiceModel = { id : 100, lineItems : [ {id: 1, category:2, description: 'Attorney Fees'} ] };

	    	beforeEach(function () {
	    		this.command = new $financeDomainCommands.CaptureThirdPartyInvoiceCommand(this.thirdPartyInvoiceModel);

	    	 	spyOn($financeDomainCommands,'CaptureThirdPartyInvoiceCommand').and.returnValue(this.command);
	    	 	spyOn($financeDomainWebService,'sendCommandAsync').and.returnValue(deferred.promise);

                this.captureThirdPartyInvoicePromise = $thirdPartyInvoiceManagerService.captureThirdPartyInvoice(this.thirdPartyInvoiceModel);
             });

	    	it("should send a command to the finance domain to capture the invoice", function(){
				expect($financeDomainCommands.CaptureThirdPartyInvoiceCommand).toHaveBeenCalledWith(this.thirdPartyInvoiceModel);
	    	});

	    	it("should return reject the promise and return the error messages", function(){
	    		var expectedResult = { data : { SystemMessages : { AllMessages  : { $values : "Errors" } } } };
	    		this.captureThirdPartyInvoicePromise.then(null,function(data){
	    			expect(data.results).toBe(expectedResult.data.SystemMessages.AllMessages.$values);
	    		});

	    		deferred.reject(expectedResult);
	    		rootScope.$apply();
	    	});
	    });


	    describe('when retrieving invoices by status key',function(){
	    	this.getInvoicesDataQuery = "";
	    	this.getInvoiceDataQueryPromise = "";
	    	this.getInvoiceQueryWhere ="";
	    	var invoiceStatusKey = 4;
	    	beforeEach(function () {
	    		this.getInvoicesDataQuery = $queryServiceRest.api.finance.thirdpartyinvoices;
	    		this.getInvoiceQueryWhere = $queryServiceRest.api.finance.thirdpartyinvoices.where('invoiceStatusKey');

		        spyOn($queryServiceRest.api.finance, 'thirdpartyinvoices').and.returnValue(this.getInvoicesDataQuery);
		        spyOn($queryServiceRest.api.finance.thirdpartyinvoices, 'where').and.returnValue(this.getInvoiceQueryWhere);
		        spyOn($queryServiceRest.api.finance.thirdpartyinvoices, 'include').and.callThrough();
		        spyOn(this.getInvoiceQueryWhere, 'isEqual').and.callThrough();
	    	 	spyOn($queryWebService,'getQueryAsync').and.returnValue(deferred.promise);

                this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoicesByStatusKey(invoiceStatusKey);
             });

	    	it("should include filter invoices by status", function(){
	    		expect($queryServiceRest.api.finance.thirdpartyinvoices.where).toHaveBeenCalledWith('invoiceStatusKey');
	    		expect(this.getInvoiceQueryWhere.isEqual).toHaveBeenCalledWith(invoiceStatusKey);
	    	});


	    	it("should call the query service to get the invoice data", function(){
				expect($queryWebService.getQueryAsync).toHaveBeenCalled();
	    	});

	    	it("should return the invoices when the promise is resolved", function(){
	    		var expectedResult = "invoices data list";
	    		this.getInvoiceDataQueryPromise.then(function(data){
	    			expect(data.results).toBe(expectedResult);
	    		});

	    		deferred.resolve({data:expectedResult});
	    		rootScope.$apply();
	    	});
	    });


	    describe('when retrieving a break down of Invoices Paid in the Previous Month', function () {
	        this.getInvoiceDataQueryPromise = "";
	        beforeEach(function () {
	            spyOn($financeDomainWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

	            this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoicesPaidPreviousMonthBreakDown();
	        });

	        it("should call the finance domain service to get the invoice data break down", function () {
	            expect($financeDomainWebService.sendQueryAsync).toHaveBeenCalled();
	        });

	        it("should return the invoices break down when the promise is resolved", function () {
	            var expectedResult = "invoices data break down";
	            this.getInvoiceDataQueryPromise.then(function (data) {
	                expect(data.results).toBe(expectedResult);
	            });

	            deferred.resolve({ data: { ReturnData: { Results: { $values: expectedResult } } } });
	            rootScope.$apply();
	        });
	    });

	    describe('when retrieving a break down of Invoices Paid this Month', function () {
	        this.getInvoiceDataQueryPromise = "";
	        beforeEach(function () {
	            spyOn($financeDomainWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

	            this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoicesPaidThisMonthBreakDown();
	        });

	        it("should call the finance domain service to get the invoice data break down", function () {
	            expect($financeDomainWebService.sendQueryAsync).toHaveBeenCalled();
	        });

	        it("should return the invoices break down when the promise is resolved", function () {
	            var expectedResult = "invoices paid this month data break down";
	            this.getInvoiceDataQueryPromise.then(function (data) {
	                expect(data.results).toBe(expectedResult);
	            });

	            deferred.resolve({ data: { ReturnData: { Results: { $values: expectedResult } } } });
	            rootScope.$apply();
	        });
	    });

	    describe('when retrieving a break down of Invoices Paid this Year', function () {
	        this.getInvoiceDataQueryPromise = "";
	        beforeEach(function () {
	            spyOn($financeDomainWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

	            this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoicesPaidThisYearBreakDown();
	        });

	        it("should call the finance domain service to get the invoice data break down", function () {
	            expect($financeDomainWebService.sendQueryAsync).toHaveBeenCalled();
	        });

	        it("should return the invoices break down when the promise is resolved", function () {
	            var expectedResult = "invoices paid this year data break down";
	            this.getInvoiceDataQueryPromise.then(function (data) {
	                expect(data.results).toBe(expectedResult);
	            });

	            deferred.resolve({ data: { ReturnData: { Results: { $values: expectedResult } } } });
	            rootScope.$apply();
	        });
	    });

	    describe('when retrieving a break down of Invoices Not Processed Last Month', function () {
	        this.getInvoiceDataQueryPromise = "";
	        beforeEach(function () {
	            spyOn($financeDomainWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

	            this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoicesNotProcessedPreviousMonthBreakDown();
	        });

	        it("should call the finance domain service to get the invoice data break down", function () {
	            expect($financeDomainWebService.sendQueryAsync).toHaveBeenCalled();
	        });

	        it("should return the invoices break down when the promise is resolved", function () {
	            var expectedResult = "invoices not processed last month data break down";
	            this.getInvoiceDataQueryPromise.then(function (data) {
	                expect(data.results).toBe(expectedResult);
	            });

	            deferred.resolve({ data: { ReturnData: { Results: { $values: expectedResult } } } });
	            rootScope.$apply();
	        });
	    });

	    describe('when retrieving a break down of Invoices Not Processed This Month', function () {
	        this.getInvoiceDataQueryPromise = "";
	        beforeEach(function () {
	            spyOn($financeDomainWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

	            this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoicesNotProcessedThisMonthBreakDown();
	        });

	        it("should call the finance domain service to get the invoice data break down", function () {
	            expect($financeDomainWebService.sendQueryAsync).toHaveBeenCalled();
	        });

	        it("should return the invoices break down when the promise is resolved", function () {
	            var expectedResult = "invoices not processed this month data break down";
	            this.getInvoiceDataQueryPromise.then(function (data) {
	                expect(data.results).toBe(expectedResult);
	            });

	            deferred.resolve({ data: { ReturnData: { Results: { $values: expectedResult } } } });
	            rootScope.$apply();
	        });
	    });

	    describe('when retrieving a break down of Invoices Not Processed This Month', function () {
	        this.getInvoiceDataQueryPromise = "";
	        beforeEach(function () {
	            spyOn($financeDomainWebService, 'sendQueryAsync').and.returnValue(deferred.promise);

	            this.getInvoiceDataQueryPromise = $thirdPartyInvoiceManagerService.getInvoicesNotProcessedThisYearBreakDown();
	        });

	        it("should call the finance domain service to get the invoice data break down", function () {
	            expect($financeDomainWebService.sendQueryAsync).toHaveBeenCalled();
	        });

	        it("should return the invoices break down when the promise is resolved", function () {
	            var expectedResult = "invoices not processed this year data break down";
	            this.getInvoiceDataQueryPromise.then(function (data) {
	                expect(data.results).toBe(expectedResult);
	            });

	            deferred.resolve({ data: { ReturnData: { Results: { $values: expectedResult } } } });
	            rootScope.$apply();
	        });
	    });
});