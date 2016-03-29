var page = require('../../page');

var switchResultsFailPage = new page();

switchResultsFailPage.url = "/for-switch/calculation-result-fail";
switchResultsFailPage.headerText = "apply";
switchResultsFailPage.titleText = "application declined";
switchResultsFailPage.btnBack = element(by.id('calcResultBackButton'));

switchResultsFailPage.back = function(){
	var switchPage = require('./switch-page');
	switchResultsFailPage.btnBack.click();
	return switchPage;
}

switchResultsFailPage.checkReasonIsDisplayed =function(expectedText){
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

module.exports = switchResultsFailPage;
