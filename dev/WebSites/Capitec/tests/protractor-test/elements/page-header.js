var pageHeader = function(){
 	this.page_Header = element(by.id('pageHeader')).element(by.css('h1:first-child'));
 	if (this.page_Header == undefined) {
		throw new Error('Header was not found'); 
 	}
	this.getHeaderText = function() {
		return this.page_Header.getInnerHtml();
	}
};

module.exports = new pageHeader();