var searchResultItem = require('../modules/search-result-item.js');
var textSearch = {};

textSearch.getSearchField = function(){
	return element(by.model('searchTerm'));
};

textSearch.getSearchButton= function(){
	return element(by.id('search-button'));
};

textSearch.getSearchResultsElements= function(){
	return element.all(by.repeater('result in searchResult.results'));
};

textSearch.performSearch = function(text){

	var buildLegalEntityArray = function(elementArray) {
		var legalEntities = [];
		var resultCount = 0;
		var pushed = false
		var deferred1 = protractor.promise.defer();

		var array = elementArray;

		browser.wait(function(){
			legalEntities = [];
			if (array.length == 0){
				pushed = true;
			};
			array.forEach(function(result){
				searchResultItem.create(result).then(function(searchItem){
					searchItem.update().then(function(updatedSearchItem){
						legalEntities.push(updatedSearchItem);
						pushed = true;
					});
				});
			});
			return pushed;

		},10000).then(function(){
			deferred1.fulfill(legalEntities);	
		});

		return deferred1.promise;
	};

	var buildElementArray = function() {
		var elementArray = [];
		var deferred2 = protractor.promise.defer();
	  	var pushed = false

		browser.wait(function(){
			elementArray = [];
			textSearch.getSearchResultsElements().count().then(function(count){
				if (count == 0){
				  	pushed = true;
				};
			});
			textSearch.getSearchResultsElements().each(function(result){
				result.isPresent().then(function(isPresent){
					if (isPresent){
						elementArray.push(result);
					  	pushed = true;
					};
				});
			});
			return pushed;
		},5000).then(function(){
			deferred2.fulfill(elementArray);	
		});
		return deferred2.promise;
	};

	textSearch.getSearchField().clear();
	textSearch.getSearchField().sendKeys(text);
  	textSearch.getSearchButton().click();

  	var count = 2000000000;
  	while(count > 0) {
  		count = count - 1;
  	};

	var deferred = protractor.promise.defer();
	var legalEntitiesBuilt = false;
	buildElementArray().then(function(eleArray){
		buildLegalEntityArray(eleArray).then(function(legalEntities){
			deferred.fulfill(legalEntities);
		});
	});

  	return deferred.promise;
};

module.exports = textSearch;