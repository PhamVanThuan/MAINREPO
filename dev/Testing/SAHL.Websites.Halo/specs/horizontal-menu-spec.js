var horizontalMenu = require('../html/horizontal-menu.js');

describe('Halo Search Horizontal Menu Specifications -->', function(){

	beforeEach(function(){
		browser.refresh();
	});

	describe('when searching', function(){

		it('should have a Clients tab and should get focus when clicked on', function() {
			horizontalMenu.getMenuItems().then(function(menuItems){
				expect(menuItems.length).toBeGreaterThan(0);
				menuItems.forEach(function(item) {
					if (item.name=="Clients"){
						item.click().then(function(updatedItem){
							expect(updatedItem.isActive).toBe(true);
						});
					};
				});
			});
		});

		it('should have a Third Parties tab and should get focus when clicked on', function() {
			horizontalMenu.getMenuItems().then(function(menuItems){
				expect(menuItems.length).toBeGreaterThan(0);
				menuItems.forEach(function(item) {
					if (item.name=="Third Parties"){
						item.click().then(function(updatedItem){
							expect(updatedItem.isActive).toBe(true);
						});
					};
				});
			});
		});

		it('should have a Tasks tab and should get focus when clicked on', function() {
			horizontalMenu.getMenuItems().then(function(menuItems){
				expect(menuItems.length).toBeGreaterThan(0);
				menuItems.forEach(function(item) {
					if (item.name=="Tasks"){
						item.click().then(function(updatedItem){
							expect(updatedItem.isActive).toBe(true);
						});
					};
				});
			});
		});

		it('should have a Search Tools tab and should not get focus when clicked on', function() {
			horizontalMenu.getMenuItems().then(function(menuItems){
				expect(menuItems.length).toBeGreaterThan(0);
				menuItems.forEach(function(item) {
					if (item.name=="Search Tools"){
						item.click().then(function(updatedItem){
							expect(updatedItem.isActive).toBe(false);
						});
					};
				});
			});
		});
	});
});