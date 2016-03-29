var page = require('../../page.js');
var newPurchaseResultsFailPage = new page();

newPurchaseResultsFailPage.url = "/for-new-home/calculation-result-fail";
newPurchaseResultsFailPage.headerText = "apply";
newPurchaseResultsFailPage.titleText = "application declined";
newPurchaseResultsFailPage.btnBack = element(by.id('calcResultBackButton'));

newPurchaseResultsFailPage.back = function() {
	var newPurchasePage = require('./new-purchase-page');
	newPurchaseResultsFailPage.btnBack.click();
	return newPurchasePage;
};

newPurchaseResultsFailPage.checkReasonIsDisplayed =function(expectedText){
            var _this = this;
            _this.messages= [];
            element.all(by.css("p[class='ng-binding']")).then(function(reasons){
                reasons.some(function(reason){
                    reason.isDisplayed().then(function(isDisplayed){
                        if(isDisplayed)
                        {
                            reason.getText().then(function(reason){
                                _this.messages.push(reason);
                            }); 
                        };
                    }); 
                });
            }).then(function(){
                expect(_this.messages).toContain(expectedText);
            });
        };

module.exports = newPurchaseResultsFailPage;