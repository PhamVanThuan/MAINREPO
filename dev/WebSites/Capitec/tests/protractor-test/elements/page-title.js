var pageTitle = function(){
 	this.page_Title = element(by.id('pageContent')).element(by.css('h1:first-child'));
 	if (this.page_Title == undefined) {
		throw new Error('Header was not found'); 
 	}
	this.getTitleText = function() {
		return this.page_Title.getInnerHtml();
	}
};

module.exports = new pageTitle();