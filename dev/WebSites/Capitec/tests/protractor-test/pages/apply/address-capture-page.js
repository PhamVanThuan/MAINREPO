var wizardPage = require('./client-capture-wizard-base-page');
var employmentPage = require('./employment-page');
var addressCapturePage = new wizardPage();

//headers/urls
addressCapturePage.url = "/client-capture/address";
addressCapturePage.headerText = "apply";
addressCapturePage.titleText = "please enter your current residential address";
//elements
addressCapturePage.unitNumber = element(by.id("unitNumber"));
addressCapturePage.buildingNumber = element(by.id("buildingNumber"));
addressCapturePage.buildingName = element(by.id("buildingName"));
addressCapturePage.streetName = element(by.id("streetName"));
addressCapturePage.streetNumber = element(by.id("streetNumber"));
addressCapturePage.suburb = element(by.name("suburb"));
addressCapturePage.province = element(by.id("province"));
addressCapturePage.postalCode = element(by.id("postalCode"));
addressCapturePage.city = element(by.id("city"));
addressCapturePage.suburbSearchField = element(by.id("searchText"));
addressCapturePage.selectPreviousAddressCheckbox = element(by.css('label[for="selectPreviousAddress"]>input'))
//div containing the address capture fields
addressCapturePage.containingDiv = element(by.id("clientpage2"));
//valid address for filling the form
addressCapturePage.validAddress = {
	streetName: "Rock Road",
	streetNumber: "99",
	suburb: "Hillcrest",
	city: "Hillcrest",
	province:"Kwazulu-natal"
};

addressCapturePage.captureAddress = function(address, goForward){
	addressCapturePage.clearInputAndPopulate(addressCapturePage.unitNumber, address.unitNumber);
	addressCapturePage.clearInputAndPopulate(addressCapturePage.buildingNumber, address.buildingNumber);
	addressCapturePage.clearInputAndPopulate(addressCapturePage.buildingName, address.buildingName);
	addressCapturePage.clearInputAndPopulate(addressCapturePage.streetNumber, address.streetNumber);
	addressCapturePage.clearInputAndPopulate(addressCapturePage.streetName, address.streetName);
	addressCapturePage.selectSuburb(address.suburb, address.province);
	if(goForward)
		addressCapturePage.goForward();
	return employmentPage;
};

addressCapturePage.fillForm = function(){
	addressCapturePage.captureAddress(addressCapturePage.validAddress);
	addressCapturePage.goForward();
	return employmentPage;
};

addressCapturePage.selectSuburb = function(suburbName, province){
	addressCapturePage.clearInputAndPopulate(addressCapturePage.suburb,suburbName);
	addressCapturePage.selectFromAutoComplete(addressCapturePage.suburbSearchField, suburbName+'('+province+')');
};

addressCapturePage.selectInvalidSuburb = function(){
	addressCapturePage.clearInputAndPopulate(addressCapturePage.suburb,"Rubish Suburb");
	addressCapturePage.goForward();
};

addressCapturePage.reset = function(){
	addressCapturePage.unitNumber.clear();
	addressCapturePage.buildingNumber.clear();
	addressCapturePage.buildingName.clear();
	addressCapturePage.streetNumber.clear();
	addressCapturePage.streetName.clear();
	addressCapturePage.suburb.clear();
};

addressCapturePage.selectPreviousAddress = function(){
	addressCapturePage.selectPreviousAddressCheckbox.click();
	addressCapturePage.goForward();
	return employmentPage;
};

module.exports = addressCapturePage;
