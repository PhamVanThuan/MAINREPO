'use strict';
describe('[sahl.websites.halo.services.wipbar]',function() {
	beforeEach(module('sahl.websites.halo.services.wipbar'));

	var wipBarService;

	beforeEach(inject(function($wipBarService){		
		wipBarService = $wipBarService;		
	}));
	
	it('should have a wipBarService',function(){
		expect(wipBarService).not.toBeNull();
	});

	var documentParams = {
		find: function(idSelector) {
		    return {
		        find: function (liSelector) {
		            return [{
		                clientWidth: 150,
		                className: ''
		            },
					{
					    clientWidth: 200,
					    className: 'active'
					}];
		        }
		    };
		}
	};
	var maxWidth = 205;

	it('should get sum of all displayed navigation tab widths', function() {
		var navigationTabWidthSum = wipBarService.getSumOfNavTabWidths(documentParams);
		expect(navigationTabWidthSum).toEqual(350);
	});

	it('should get navigation tab indexes that currently exceed window width', function() {		
		var overflowingTabIndexes = wipBarService.getOverflowingTabIndexes(documentParams, maxWidth);
		expect(overflowingTabIndexes.length).toEqual(1);
	});

	it('should determine that overflowing tab is not the right-most tab when that tab is active', function() {
		var overflowingTabIndexes = wipBarService.getOverflowingTabIndexes(documentParams, maxWidth);
		expect(overflowingTabIndexes[0]).toEqual(0);
	});
});