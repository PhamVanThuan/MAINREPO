var textSearch = require('../html/text-search.js');
var horizontalMenu = require('../html/horizontal-menu.js');

describe('Halo Landing Page Specifications -->', function(){

	beforeEach(function(){
		browser.refresh();
	});
	
	describe('when arriving at the landing page', function(){
		
		it('should have a search field', function() {
			expect(textSearch.getSearchField().isDisplayed()).toBe(true);
		});

		it('should default on search', function() {
			browser.refresh();
			browser.getCurrentUrl().then(function(url) {
				expect(url).toContain('home/search/');
			});
		});

		it('should default on client search', function() {
			browser.refresh();
			horizontalMenu.getMenuItems().then(function(menuItems){
				expect(menuItems.length).toBeGreaterThan(0);
				menuItems.forEach(function(item) {
					if (item.name=="Clients"){
						expect(item).not.toBeNull();
						expect(item.isActive).toBe(true);
					};
				});
			});
		});
	});
});