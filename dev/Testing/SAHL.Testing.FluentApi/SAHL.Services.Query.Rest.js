'use strict';

angular.module('SAHL.Services.Query.rest', ['sahl.js.core.fluentRestQuery']).
    factory('$queryServiceRest', ['$fluentRestQuery',function ($fluentRestQuery) {

		 var settingsToPass = {
			route: '',
			parameters: [],
			pushParameter: function (param, inPosition) {
				settingsToPass.parameters[inPosition] = param;
			}
		};

		var internals = {
			builder: new $fluentRestQuery(settingsToPass),
			extendIt: function (objectToExtend, routeToPass) {
				settingsToPass.route = routeToPass;
				angular.extend(objectToExtend, new $fluentRestQuery(settingsToPass));
				return objectToExtend;
			}
		};

        var routes = {
            api : internals.extendIt({accounts : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({processes : {getByProcess : function(process) {   settingsToPass.pushParameter(process, 1);  return {stage : internals.extendIt({getByValue : function(value) {   settingsToPass.pushParameter(value, 2);  return internals.extendIt({}, '/api/accounts/{0}/processes/{1}/stage/{2}');}}, '/api/accounts/{0}/processes/{1}/stage')};}},
				spv : internals.extendIt({}, '/api/accounts/{0}/spv'),
				spvs : {getById : function(Id) {   settingsToPass.pushParameter(Id, 1);  return internals.extendIt({}, '/api/accounts/{0}/spvs/{1}');}}}, '/api/accounts/{0}');}}, '/api/accounts'),
		lookup : internals.extendIt({InvoiceLineItemCategory : {getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return {InvoiceLineItemDescription : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 1);  return internals.extendIt({}, '/api/lookup/InvoiceLineItemCategory/{0}/InvoiceLineItemDescription/{1}');}}, '/api/lookup/InvoiceLineItemCategory/{0}/InvoiceLineItemDescription')};}},
			getByLookupType : function(lookupType) {   settingsToPass.pushParameter(lookupType, 0);  return internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 1);  return internals.extendIt({}, '/api/lookup/{0}/{1}');}}, '/api/lookup/{0}');}}, '/api/lookup'),
		attorneys : internals.extendIt({contactinformation : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/attorneys/contactinformation/{0}');}}, '/api/attorneys/contactinformation'),
			contacts : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/attorneys/contacts/{0}');}}, '/api/attorneys/contacts'),
			count : internals.extendIt({}, '/api/attorneys/count'),
			getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/attorneys/{0}');}}, '/api/attorneys'),
		finance : {thirdpartyinvoices : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({lineItems : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 1);  return internals.extendIt({}, '/api/finance/thirdpartyinvoices/{0}/lineItems/{1}');}}, '/api/finance/thirdpartyinvoices/{0}/lineItems'),
					documents : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 1);  return internals.extendIt({}, '/api/finance/thirdpartyinvoices/{0}/documents/{1}');}}, '/api/finance/thirdpartyinvoices/{0}/documents')}, '/api/finance/thirdpartyinvoices/{0}');}}, '/api/finance/thirdpartyinvoices')},
		organisation : internals.extendIt({types : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/organisation/types/{0}');}}, '/api/organisation/types'),
			structure : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/organisation/structure/{0}');}}, '/api/organisation/structure')}, '/api/organisation'),
		thirdparties : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({bankaccounts : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 1);  return internals.extendIt({}, '/api/thirdparties/{0}/bankaccounts/{1}');}}, '/api/thirdparties/{0}/bankaccounts'),
				contactinformation : internals.extendIt({}, '/api/thirdparties/{0}/contactinformation')}, '/api/thirdparties/{0}');},
			contactinformation : internals.extendIt({}, '/api/thirdparties/contactinformation')}, '/api/thirdparties'),
		thirdparty : {paymentbankaccount : internals.extendIt({}, '/api/thirdparty/paymentbankaccount'),
			getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return {paymentbankaccount : internals.extendIt({}, '/api/thirdparty/{0}/paymentbankaccount')};}},
		treasury : {spvs : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/treasury/spvs/{0}');}}, '/api/treasury/spvs')},
		TestDynamic : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/TestDynamic/{0}');}}, '/api/TestDynamic'),
		Fake : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/Fake/{0}');}}, '/api/Fake'),
		test : internals.extendIt({getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/test/{0}');}}, '/api/test'),
		test2 : {getById : function(Id) {   settingsToPass.pushParameter(Id, 0);  return internals.extendIt({}, '/api/test2/{0}');}}}, '/api')
            };
        return routes;
        
        
    }]);