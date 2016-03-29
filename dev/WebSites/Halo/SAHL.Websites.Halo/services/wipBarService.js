'use strict';
angular.module('sahl.websites.halo.services.wipbar', [
])
.service('$wipBarService', [
    function () {
    	
    	var operations = {
    		getSumOfNavTabWidths: function(documentParam){
    			var sumOfTabItemWidths = 0;
	         	var tabList = documentParam.find('#tabs').find('li');
	         	angular.forEach(tabList, function(li) { 
	           		sumOfTabItemWidths += li.clientWidth;
	         	});

	         	return sumOfTabItemWidths;
    		},
    		getOverflowingTabIndexes: function(documentParam, maximumWidth) {
    			var sumOfTabItemWidths = 0;
				var activeTab = documentParam.find('#tabs').find('li.active');
				var tabList = documentParam.find('#tabs').find('li');
				var overflowingTabs = [];

				if (activeTab[0]) {
					sumOfTabItemWidths += activeTab[0].clientWidth;
				};

				for (var i = 0; i < tabList.length; i++) {
					var li = tabList[i];
					if (li.className.indexOf('active') >= 0) {
					  continue;
					};

					sumOfTabItemWidths += li.clientWidth;
					if (sumOfTabItemWidths >= maximumWidth) {
					  overflowingTabs.push(i);
					};
    			};
    			return overflowingTabs;
    		}
    	};

    	return {
			getSumOfNavTabWidths: operations.getSumOfNavTabWidths,
			getOverflowingTabIndexes: operations.getOverflowingTabIndexes
    	};
	}
]);
