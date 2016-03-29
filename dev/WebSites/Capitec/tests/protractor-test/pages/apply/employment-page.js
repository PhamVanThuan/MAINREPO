var wizardPage = require('./client-capture-wizard-base-page');
var scripts = {};
var utils = require('../../utils')(scripts)
var employmentPage = new wizardPage();
var declarationsPage = require('./declarations-page');

employmentPage.employmentType = element(by.id('employmentTypeEnumId'));
employmentPage.employmentTypeSpan = element(by.id('employmentTypeEnumIdSpan'));
employmentPage.grossIncome = element(by.id('grossMonthlyIncome'));
employmentPage.url = '/client-capture/employment';
employmentPage.titleText = 'please enter employment details';
employmentPage.headerText = 'apply';
employmentPage.employmentTypes = element(by.id('employmentTypeEnumId')).element.all(by.tagName('option'));
employmentPage.lblGrossIncome = element(by.id('lblGrossIncome'));
employmentPage.lblGrossIncomeThreeMonthAvg = element(by.id('lblGrossIncomeThreeMonthAvg'));
employmentPage.lblGrossIncomeExCommission = element(by.id('lblGrossIncomeExCommission'));
employmentPage.lblGrossIncomeExHousingAllowance = element(by.id('lblGrossIncomeExHousingAllowance'));
employmentPage.lblMonthlyCommission = element(by.id('lblMonthlyCommission'));
employmentPage.lblHousingAllowance = element(by.id('lblHousingAllowance'));
//inputs
employmentPage.threeMonthAverageCommission = element(by.id('threeMonthAverageCommission'));
employmentPage.housingAllowance = element(by.id('housingAllowance'));

employmentPage.captureEmployment = function(type, income, housingAllowance, commission){
	employmentPage.selectOption(employmentPage.employmentType, type);
	if (type != 'Unemployed'){
		employmentPage.clearInputAndPopulate(employmentPage.grossIncome, income);
	}
	if (type === 'Salaried with Commission'){
		employmentPage.clearInputAndPopulate(employmentPage.threeMonthAverageCommission, commission);
	}
	if(type === 'Salaried with Housing Allowance'){
		employmentPage.clearInputAndPopulate(employmentPage.housingAllowance, housingAllowance);
	}
	employmentPage.goForward();
	return declarationsPage;
};

employmentPage.fillForm = function(){
	return employmentPage.captureEmployment('Salaried', '50000');
};

employmentPage.reset = function(){
	employmentPage.grossIncome.clear();
	employmentPage.selectOption(employmentPage.employmentType, '');
	if(scripts.common.checkElementExists(protractor.By.id('threeMonthAverageCommission'))){
		employmentPage.threeMonthAverageCommission.clear();
	}
	if(scripts.common.checkElementExists(protractor.By.id('housingAllowance'))){
		employmentPage.housingAllowance.clear();
	}
};

employmentPage.selectEmploymentType = function(employmentType){
	employmentPage.selectOption(employmentPage.employmentType, employmentType);
};
employmentPage.setGrossMonthlyIndividualIncome = function(value){
	employmentPage.clearInputAndPopulate(employmentPage.grossIncome, value);
};
module.exports = employmentPage;