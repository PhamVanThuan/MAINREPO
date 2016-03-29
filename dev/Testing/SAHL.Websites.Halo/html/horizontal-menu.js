var menu = require('../modules/menu-item.js');
var horizontalMenu = {
	root: element(by.id('search-nav'))
};

horizontalMenu.getMenuItems = function(){

	var menuItemAddCount = 0;
	var deferred = protractor.promise.defer();
	var menuItems = [];
	//add search tools not ppart of ng-repeat
	element(by.id('searchTools')).then(function(menuItem) {
		element(by.id('searchToolsButton')).then(function(btn) {
			btn.getText().then(function(text){
				menu.create(text,menuItem,btn).then(function(item){
					menuItems.push(item);
					menuItemAddCount+=1;
				});
			});
		});
	});
	
	element.all(by.repeater('searchType in searchFilters.searchTypes')).each(function(menuItem, index){
		menuItemAddCount+=1;
		menuItem.element(by.tagName('a')).then(function(link){
			link.getText().then(function(text){
				menu.create(text,menuItem,link).then(function(item){
					menuItems.push(item);
					if (menuItems.length == menuItemAddCount) {
						deferred.fulfill(menuItems);
					};
				});
			});
		});
	});
	return deferred.promise;
};
module.exports = horizontalMenu;