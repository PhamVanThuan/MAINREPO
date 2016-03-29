module.exports = function(applicationName){
    var informationToolTip = function(){   
        var scripts = {};
        require('../utils')(scripts);

        var toolTips = element.all(by.css("div[validationtooltip] span:first-child"));

        this.closeAll =  function(){
            element.all(by.css("div[validationtooltip] span:first-child")).each(function(toolTip){
               	toolTip.isDisplayed().then(function(isDisplayed){
            		if(isDisplayed)
                		toolTip.click();
                });
            });
        };

        this.getAll =  function(){
            element.all(by.css("div[validationtooltip] span:first-child")).each(function(element){
            	element.getText().then(function(text){
	            		console.log(text);
	            });
            });
        };

        this.checkToolTipMessageIsDisplayed =function(expectedText){
            var _this = this;
            _this.messages= [];
            element.all(by.css("div[validationtooltip] span:first-child")).then(function(toolTips){
                toolTips.some(function(toolTip){
                    toolTip.isDisplayed().then(function(isDisplayed){
                        if(isDisplayed)
                        {
                            toolTip.getText().then(function(message){
                                _this.messages.push(message);
                            }); 
                        };
                    }); 
                });
            }).then(function(){
                expect(_this.messages).toContain(expectedText);
            });
        };
    };
applicationName.informationToolTip = new informationToolTip();
};