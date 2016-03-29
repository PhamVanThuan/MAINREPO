'use strict';

angular.module('SAHL.DomainProcessManager.Models.Custom', []).
    factory('$domainProcessManagerModels', function () {
        var shared = (function () {
				function PayThirdPartyInvoiceProcessModel(invoiceCollection) {
					this.invoiceCollection = invoiceCollection;
					this._name = 'SAHL.DomainProcessManager.Models.PayThirdPartyInvoiceProcessModel,SAHL.DomainProcessManager.Models';
				}

				function PayThirdPartyInvoiceModel(thirdPartyInvoiceKey, instanceId, accountNumber, sahlReference) {
					this.thirdPartyInvoiceKey = thirdPartyInvoiceKey;
					this.instanceId = instanceId;
					this.accountNumber = accountNumber;
					this.sahlReference = sahlReference;
					this.stepInProcess = null;
					this._name = 'SAHL.DomainProcessManager.Models.PayThirdPartyInvoiceModel,SAHL.DomainProcessManager.Models';
				}

	            return {
	                PayThirdPartyInvoiceProcessModel: PayThirdPartyInvoiceProcessModel,
	                PayThirdPartyInvoiceModel: PayThirdPartyInvoiceModel
	            };
        }());
        return shared;
    });