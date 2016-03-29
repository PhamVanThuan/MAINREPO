'use strict';

describe('[sahl.websites.halo.services.accountsManager]', function () {

    beforeEach(module('sahl.websites.halo.services.accountsManager'));
    beforeEach(module('SAHL.Services.Query.rest'));
    beforeEach(module('sahl.js.core.fluentRestQuery'));

    var queryServiceRest, q, 
        queryWebService={},
        accountNumber=1456,
        accountsManagerService,
        rootScope,
        accountPromise,
        lossControlStagePromise,
        getAccountQuery,
        getLossControlQuery,
        businessProcess ='losscontrol',
        getQueryAsync = {};
        var deferred = null,
        $fluentRestQuery = null;

    beforeEach(inject(function ($injector, $rootScope) {
        accountsManagerService = $injector.get('$accountsManagerService');
        q = $injector.get('$q');
        deferred =  q.defer();
        rootScope = $rootScope;
        queryServiceRest = $injector.get('$queryServiceRest');
        queryWebService = $injector.get('$queryWebService');
        $fluentRestQuery = $injector.get('$fluentRestQuery');

        
        rootScope = $injector.get('$rootScope');
    }));


    describe('get account and spv', function () {
        beforeEach(function(){
        getAccountQuery = queryServiceRest.api.accounts.getById(accountNumber);   
        var includes = ['spv'];
        getAccountQuery.include(includes);   

         spyOn(queryWebService, 'getQueryAsync').and.returnValue(deferred.promise);
         spyOn(queryServiceRest.api.accounts, 'getById').and.returnValue(getAccountQuery);
         accountPromise =  accountsManagerService.getAccountAndSPVByAccountNumber(accountNumber);
        });

        it('should call the webservice ', function () {
            expect(queryWebService.getQueryAsync).toHaveBeenCalledWith(getAccountQuery);
        });

         it('should return account and spv ', function () {
           var expectedResult = {account:"1456", spv:"spv_test"};
            accountPromise.then(function(data){
              expect(data.results).toBe(expectedResult);
            });

            deferred.resolve({data:expectedResult});
            rootScope.$apply();
        });
    });

     describe('get losscontrol process stage', function () {
        beforeEach(function () {
             getLossControlQuery  = queryServiceRest.api.accounts.getById(accountNumber).processes.getByProcess(businessProcess).stage;
             spyOn(queryServiceRest.api.accounts.getById(accountNumber).processes.getByProcess(businessProcess), 'stage')
                    .and.returnValue(getLossControlQuery);

             spyOn(queryWebService,'getQueryAsync').and.returnValue(deferred.promise);
            lossControlStagePromise  =  accountsManagerService.getLossControlProcessStage(accountNumber,businessProcess);
        });
     
        it('should call the webservice ', function () {
            expect(queryWebService.getQueryAsync).toHaveBeenCalled();
        });

         it('should return losscontrol process stage ', function () {
           var expectedResult = "Duplicate Accounts Archived";
            lossControlStagePromise.then(function(data){
              expect(data.results).toBe(expectedResult);
            });

            deferred.resolve({data:expectedResult});
            rootScope.$apply();
        });
    });
});