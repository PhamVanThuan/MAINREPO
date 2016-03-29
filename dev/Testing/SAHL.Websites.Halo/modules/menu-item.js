var menu = {};
menu.create = function(text,menuItem,link){
	var item = {
		name: text,
		menuElement: menuItem,
		menuElementLink: link,
		isActive: false,
		click: function() {
			item.menuElementLink.click();
			return item.update();
		},
		update:  function() {
			var deferred = protractor.promise.defer();
			item.menuElement.getAttribute('class').then(function(attVal) {
					if (attVal=='ng-scope active'){
						item.isActive = true;
					}else {
						item.isActive = false;
					};
					deferred.fulfill(item);
				});
				return deferred.promise;
		}
	};
	return item.update();
};
module.exports = menu;
