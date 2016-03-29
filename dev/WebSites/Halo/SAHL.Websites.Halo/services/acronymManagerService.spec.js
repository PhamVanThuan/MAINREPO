'use strict';
describe('[sahl.websites.halo.services.acronymManagerService]', function () {
    beforeEach(module('sahl.websites.halo.services.acronymManager'));

    var isAcronym, acronymDisplayName;
    var acronymManager;

    beforeEach(inject(function ($injector) {
        acronymManager = $injector.get('$acronymManagerService');
    }));


    describe('When checking if a word is an acronym', function () {
        describe('when a provided word is not an acronym', function () {

            beforeEach(function () {
                isAcronym = acronymManager.isAcronym('NotAconym');
            });
            it('Confirms that the provided word is not an acronym', function () {
                expect(isAcronym).toBe(false);
            });
        });
        describe('when a provided word is an acronym', function () {
            beforeEach(function () {
                isAcronym = acronymManager.isAcronym('SPV');
            });
            it('Confirms that the provided word is an acronym', function () {
                expect(isAcronym).toBe(true);
            });
        });
    });


    describe('When getting display name for an acronym', function () {
        beforeEach(function () {
            acronymDisplayName = acronymManager.getDisplayName('spv');
        });
        it('returns the display name', function () {
            expect(acronymDisplayName).toBe("SPV");
        });
    });
        
});