var page = require('../../page.js');
var trackingPage = new page();

trackingPage.idNumber = element(by.id("identityNumber"));
trackingPage.applicationNumber = element(by.id("applicationNumber"));
trackingPage.url = "/capitec/#/track";
trackingPage.btnTrack = element(by.id("submitButton"));
trackingPage.pageTitle = "track your application\'s progress";
trackingPage.pageHeader = 'track';

trackingPage.search = function(searchCriteria){
	var trackingResultPage = require('./tracking-result-page.js');
	if(searchCriteria["applicationNumber"] !== undefined){
		trackingPage.clearInputAndPopulate(trackingPage.applicationNumber, searchCriteria.applicationNumber);			
	}
	if(searchCriteria["idNumber"] !== undefined){
		trackingPage.clearInputAndPopulate(trackingPage.idNumber, searchCriteria.idNumber);
	}
	trackingPage.btnTrack.click();
	return trackingResultPage;
}; 	

module.exports = trackingPage;