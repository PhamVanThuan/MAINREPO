var page = require('../../page.js');
var trackingResultPage = new page();

trackingResultPage.url = "/track/results";
trackingResultPage.pageHeader = 'track result';
trackingResultPage.btnBack = element(by.id('back'));
trackingResultPage.ApplicationNo = element(by.binding('application.ApplicationNumber'));
trackingResultPage.ApplicationDate = element(by.binding('application.ApplicationDate'));
trackingResultPage.SAHLConsultant = element(by.binding('application.ConsultantName'));
trackingResultPage.ContactNo = element(by.binding('application.ConsultantContactNumber'));
trackingResultPage.Applicants = element(by.binding('applicant.identitynumber'));
trackingResultPage.noResultsMessage = element(by.id('noMatchMessage'));

trackingResultPage.back = function() {
	var trackingPage = require('./tracking-page.js');
	trackingResultPage.btnBack.click();
	return trackingPage;
};

module.exports = trackingResultPage;