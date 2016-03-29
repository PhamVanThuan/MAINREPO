'use strict';

describe('[SAHL.DomainProcessManager.Models.Custom]', function () {
	beforeEach(module('SAHL.DomainProcessManager.Models.Custom'));

	var dpmModels;

	beforeEach(inject(function($injector){
		dpmModels = $injector.get("$domainProcessManagerModels");

	}));

	describe("when instantiating the pay third party invoice model",function(){
		var payTP, 
		thirdPartyInvoiceKey = 22, 
		instanceId = 12500, 
		accountNumber = 140500, 
		sahlReference = "sahl reference", 
		stepInProcess = null, 
		_name = "SAHL.DomainProcessManager.Models.PayThirdPartyInvoiceModel,SAHL.DomainProcessManager.Models";

		beforeEach(function(){
			payTP = new dpmModels.PayThirdPartyInvoiceModel(thirdPartyInvoiceKey, instanceId, accountNumber, sahlReference);
		});

		it("should set the third party invoice key",function(){
			expect(payTP.thirdPartyInvoiceKey).toEqual(thirdPartyInvoiceKey);
		});

		it("should set the instance id",function(){
			expect(payTP.instanceId).toEqual(instanceId);
		});

		it("should set the account number",function(){
			expect(payTP.accountNumber).toEqual(accountNumber);
		});

		it("should set the sahl reference",function(){
			expect(payTP.sahlReference).toEqual(sahlReference);
		});

		it("should set the step in progress to null",function(){
			expect(payTP.stepInProcess).toEqual(stepInProcess);
		});

		it("should set the _name to the fully qualified name of the C# model",function(){
			expect(payTP._name).toEqual(_name);
		});
	});



	describe("when instantiating the pay third party invoice process model",function(){
		var payTP, 
		invoiceCollection = "invoiceCollection", 
		_name = "SAHL.DomainProcessManager.Models.PayThirdPartyInvoiceProcessModel,SAHL.DomainProcessManager.Models";

		beforeEach(function(){
			payTP = new dpmModels.PayThirdPartyInvoiceProcessModel(invoiceCollection);
		});

		it("should set the third party invoice collection",function(){
			expect(payTP.invoiceCollection).toEqual(invoiceCollection);
		});

		it("should set the _name to the fully qualified name of the C# model",function(){
			expect(payTP._name).toEqual(_name);
		});
	});
});