var wizardPage = require('./client-capture-wizard-base-page');
var addressPage = require('./address-capture-page');
var clientCapturePage = new wizardPage();

//headers/urls
clientCapturePage.url = "/client-capture/personal";
clientCapturePage.headerText = "apply";
clientCapturePage.titleText = "please enter personal information";

//input fields
clientCapturePage.containingDiv = element(by.id("clientpage1"));
clientCapturePage.identityNumber = element(by.id("identityNumber"));
clientCapturePage.dateOfBirth = { 
	day: element(by.model('date.day')), 
	month: element(by.model('date.month')), 
	year: element(by.model('date.year')),
	selectedDay: element(by.css("select[ng-model='date.day'] + span")),
	selectedMonth: element(by.css("select[ng-model='date.month'] + span")),
	selectedYear: element(by.css("select[ng-model='date.year'] + span"))
};
clientCapturePage.firstname = element(by.id("firstName"));
clientCapturePage.surname = element(by.id("surname"));
clientCapturePage.salutation = element(by.id("salutationEnumId"));
clientCapturePage.salutationSpan = element(by.id("salutationEnumIdSpan"));
clientCapturePage.workPhoneNumber = element(by.id("workPhoneNumber"));
clientCapturePage.cellPhoneNumber = element(by.id("cellPhoneNumber"));
clientCapturePage.emailAddress = element(by.id("emailAddress"));
clientCapturePage.salutations = clientCapturePage.salutation.element.all(by.tagName('option'));
clientCapturePage.mainContact = element(by.name("mainContact"));


clientCapturePage.addClientDetails = function(client, goForward){
	clientCapturePage.clearInputAndPopulate(clientCapturePage.identityNumber, client.idNumber);
	clientCapturePage.selectOption(clientCapturePage.salutation, client.salutation);
	clientCapturePage.clearInputAndPopulate(clientCapturePage.firstname, client.firstName);
	clientCapturePage.clearInputAndPopulate(clientCapturePage.surname, client.surname);
	clientCapturePage.clearInputAndPopulate(clientCapturePage.emailAddress, client.contactDetails.emailAddress);
	clientCapturePage.clearInputAndPopulate(clientCapturePage.cellPhoneNumber, client.contactDetails.cellPhoneNumber);
	clientCapturePage.clearInputAndPopulate(clientCapturePage.workPhoneNumber, client.contactDetails.workPhoneNumber);
	clientCapturePage.selectMainContact(client.mainContact);
	if(goForward)
		clientCapturePage.goForward();
	return addressPage;
};

clientCapturePage.Next = function(){
	clientCapturePage.goForward();
	return addressPage;
}

clientCapturePage.fillForm = function(idNumber){
	var clientDetails =	{ 
				idNumber: idNumber, 
				surname: "Speed", 
				firstName: "Clinton", 
				dateOfBirth: "1980-01-01",  
				contactDetails: { 
					emailAddress: "test@sahomeloans.com",
					workPhoneNumber: "0315713036",
					cellPhoneNumber: "0827702444"
				},
				salutation: "Mr",
				mainContact: true
		};
	clientCapturePage.addClientDetails(clientDetails);
	clientCapturePage.goForward();
	return addressPage;
};

clientCapturePage.reset = function(){
	clientCapturePage.clearInputs(clientCapturePage.containingDiv);
};

clientCapturePage.setSalutationTo = function(salutation){
	clientCapturePage.selectOption(clientCapturePage.salutation, salutation);
};

clientCapturePage.setCellPhoneNumberTo = function(cellPhoneNumber){
	clientCapturePage.clearInputAndPopulate(clientCapturePage.cellPhoneNumber, cellPhoneNumber)
};

clientCapturePage.setWorkPhoneNumberTo = function(workPhoneNumber){
	clientCapturePage.clearInputAndPopulate(clientCapturePage.workPhoneNumber, workPhoneNumber)
};

clientCapturePage.setSurnameTo = function(surname){
	clientCapturePage.clearInputAndPopulate(clientCapturePage.surname, surname);
};

clientCapturePage.setFirstNameTo = function(firstName){
	clientCapturePage.clearInputAndPopulate(clientCapturePage.firstname, firstName);	
};

clientCapturePage.setIdentityNumber = function(identityNumber){
	clientCapturePage.clearInputAndPopulate(clientCapturePage.identityNumber, identityNumber);	
};

clientCapturePage.setEmailAddress = function(emailAddress){
	clientCapturePage.clearInputAndPopulate(clientCapturePage.emailAddress, emailAddress);	
};

clientCapturePage.selectMainContact = function(value){
	clientCapturePage.mainContact.isSelected().then(function(selected){
		if(value && !selected){
			clientCapturePage.mainContact.click();
		} 
		else if (!value && selected) {
			clientCapturePage.mainContact.click();
		}
	});	
};

clientCapturePage.selectDOB = function(date){
	var dateParts = date.split('-');
	clientCapturePage.selectOption(clientCapturePage.dateOfBirth.day, dateParts[2]);
	clientCapturePage.selectOption(clientCapturePage.dateOfBirth.month, dateParts[1]);
	clientCapturePage.selectOption(clientCapturePage.dateOfBirth.year, dateParts[0]);
};

module.exports = clientCapturePage;