'use strict';
describe('[sahl.js.core.documentManagement]', function() {
    beforeEach(module('sahl.js.core.documentManagement'));
    beforeEach(module('sahl.js.core.documentManagement.types.decisiontree'));
    beforeEach(module('sahl.js.core.documentManagement.types.userprofile'));

    beforeEach(inject(function($injector, $q) {}));

    describe(' - (documentVersionManagement versioned providers) -', function() {
        var $rootScope, $documentVersionManagerService;
        var versionProviders = [];
        beforeEach(inject(function($injector, $q) {
            $rootScope = $injector.get('$rootScope');
            $documentVersionManagerService = $injector.get('$documentVersionManagerService');
            versionProviders.push($injector.get('$decisiontreeDocumentVersion_0_1'));
            versionProviders.push($injector.get('$userprofileDocumentVersion_0_1'));
            versionProviders.push($injector.get('$userprofileDocumentVersion_0_2'));
        }));

        it('each versioned provider should be startable', function(){
            _.each(versionProviders, function(item){
                expect(item["start"]).not.toBeUndefined();
            });
        });

        it('each versioned provider be capable of creating an empty document and loading one', function(){
            _.each($documentVersionManagerService.$providers(), function(item){
                _.each(item.versions, function(item2){
                    expect(item2.provider["emptyDocument"]).not.toBeUndefined();
                    expect(item2.provider["loadDocument"]).not.toBeUndefined();
                });
            });
        });     
    });

});
