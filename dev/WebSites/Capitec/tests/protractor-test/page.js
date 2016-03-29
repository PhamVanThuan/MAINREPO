var pageHeader = require('./elements/page-header.js');
var pageTitle = require('./elements/page-title.js');
var utils = {} 
require('./utils.js')(utils);

var pageObject = function(){

};

pageObject.prototype.header = function(){ 
	return pageHeader.getHeaderText();
};

pageObject.prototype.title = function(){ 
	return pageTitle.getTitleText();
};

pageObject.prototype.selectOption = function(selectList, optionValue){
	utils.common.selectOption(selectList, optionValue);
}

pageObject.prototype.selectFromAutoComplete = function(autoComplete, option){
	utils.common.selectFromAutoComplete(autoComplete, option);
}

pageObject.prototype.clearInputAndPopulate = function(element, value){
	if(typeof value != "undefined"){
		utils.common.clearInputAndPopulate(element, value);
	}
};

pageObject.prototype.clearInputs = function(container){
	container.element.all(by.tagName('input')).then(function(elements){
		for (var i = 0; i < elements.length; i++) {
				elements[i].clear();
			}
	});
};

pageObject.prototype.isElementReadonly = function(element){
	return utils.common.isElementReadonly(element);
};

pageObject.prototype.isCheckboxChecked = function(element){
	return	utils.common.isCheckboxChecked(element);
};

module.exports = pageObject;
