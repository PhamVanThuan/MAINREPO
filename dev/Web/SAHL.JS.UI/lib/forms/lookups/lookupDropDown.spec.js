'use strict';
describe('[sahl.js.ui.forms.lookup]', function() {
    beforeEach(module('sahl.js.core.lookup'));
    beforeEach(module('sahl.js.ui.forms.lookup'));

    var lookupService = {};

    beforeEach(module(function($provide) {
        $provide.value('$lookupService', lookupService);
    }));

    beforeEach(inject(function($q) {
        lookupService.getByLookupType = function() {
            var deferred = $q.defer();
            
            var data ={'data':{'_links':{'self':{'href':'/QueryService/api/lookup/{lookupType}','templated':true},'parent':{'href':'/QueryService/api/lookup'},'legalentitytype':[{'href':'/QueryService/api/lookup/legalentitytype/1'},{'href':'/QueryService/api/lookup/legalentitytype/2'},{'href':'/QueryService/api/lookup/legalentitytype/3'},{'href':'/QueryService/api/lookup/legalentitytype/4'},{'href':'/QueryService/api/lookup/legalentitytype/5'}]},'_embedded':{'legalentitytype':[{'id':1,'description':'Unknown','_links':{'self':{'href':'/QueryService/api/lookup/legalentitytype/1'},'parent':{'href':'/QueryService/api/lookup/legalentitytype'}}},{'id':2,'description':'Natural Person','_links':{'self':{'href':'/QueryService/api/lookup/legalentitytype/2'},'parent':{'href':'/QueryService/api/lookup/legalentitytype'}}},{'id':3,'description':'Company','_links':{'self':{'href':'/QueryService/api/lookup/legalentitytype/3'},'parent':{'href':'/QueryService/api/lookup/legalentitytype'}}},{'id':4,'description':'Close Corporation','_links':{'self':{'href':'/QueryService/api/lookup/legalentitytype/4'},'parent':{'href':'/QueryService/api/lookup/legalentitytype'}}},{'id':5,'description':'Trust','_links':{'self':{'href':'/QueryService/api/lookup/legalentitytype/5'},'parent':{'href':'/QueryService/api/lookup/legalentitytype'}}}]}},'status':200} ; 

            deferred.resolve(data);
            return deferred.promise;
        };

    }));


    var $compile, $RootScope;
    beforeEach(inject(function(_$compile_, $rootScope) {
        $compile = _$compile_;
        $RootScope = $rootScope;
    }));

    var compileAndDigest = function(target, scope) {
        var $element = $compile(target)(scope);
        $RootScope.$digest();
        return $element;
    };

    describe(' - (Directive: lookup)-', function() {
        describe('given that the lookup data has loaded', function() {
            var $scope, element;

            var inputElement = angular.element('<div lookup-dropdown lookup-id="lookupId" lookup-type="legalentitytype"></div>');

            beforeEach(inject(function() {
                $scope = $RootScope.$new();
                $scope.lookupId = 2;
                element = compileAndDigest(inputElement, $scope);

            }));

            it('should have a isolated scope ', function() {
              var isolated = element.isolateScope();
                expect(isolated).toBeDefined();
            });

            it('should have a lookupId defined', function() {
                var scope = element.scope();
                expect(scope.lookupId).toEqual(2);
            });

            it('should have a select element', function() {
                expect(element.find('select')).toBeDefined();
            });

            it('should have Natural Person as the selected item', function() {
                expect(element.find('select')[0].options.selectedIndex).toEqual(1);
                expect(element.find('select')[0].options[1].innerText).toBe('Natural Person');
            });
        });
    });
});
