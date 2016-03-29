module.exports = function(applicationName){
    var notifications = function(){
        var scripts = {};
        require('../utils')(scripts);
        var async = require('async');

        this.notificationBox = element(by.css("div[class~='ui-pnotify']"));
        this.successMessagesBox = element(by.css("div[class~='alert-success']"));
        this.infoMessagesBox = element(by.css("div[class~='alert-info']"));
        this.errorMessagesBox = element(by.css("div[class~='alert-error']"));
        this.errorElement = element(by.className("errorMessage"));
        this.allMessages = [];

        this.closeAll =  function(){
            var ptor = protractor.getInstance();
            ptor.executeScript('$.pnotify_remove_all()');
        };
        
        this.validationMessages =  function(){
            return this.notificationBox.getInnerHtml();
        };

        this.successMessages = function(){
            return this.successMessagesBox.getInnerHtml();
        };

        this.errorMessage = function() {
            return this.errorElement.getText();
        };

        this.errorMessages = function(){
            return this.errorMessagesBox.getInnerHtml();
        }

        this.infoMessages = function(){
            return this.infoMessagesBox.getInnerHtml();
        };

        this.notificationsExists = function(){
            return scripts.common.checkElementExists(protractor.By.css("div[class~='ui-pnotify-container']"));
        };

        this.getAllMessages = function(callback){
            getAll(function(items){
                this.allMessages = items;
                callback(items);
            });  
        }
        //
        function getAll(callback){
            var _this = this;
            _this.pnotifyPopUps = element.all(by.css("div[class~='ui-pnotify-container']"));
            _this.pnotifyPopUps.map(function findAllListItems(element){
                element.findElements(protractor.By.tagName('li')).then(function(listItems){
                    var items = [];
                    async.each(listItems, function(item, done){
                        if (item){
                                item.getText().then(function(text){
                                items.push(text);
                                done();
                            }); 
                        }
                    }, function(err){
                        callback(items);
                    });
                });
            });
        };

        this.checkIfValidationMessageExists = function(expectedText){
            var _this = this;
            _this.exists = false;
            if (!this.notificationsExists)
                expect(this.notificationsExists).toBeTruthy();
            _this.pnotifyPopUps = element.all(by.css("div[class~='ui-pnotify-container']"));
            _this.pnotifyPopUps.map(function findAllListItems(element){
                element.findElements(protractor.By.tagName('li')).then(function(listItems){
                    listItems.some(function checkForValidationMessage(listitem){
                        listitem.getText().then(function(text){
                            if(expectedText === text){
                                _this.exists = true;        
                            }
                        });     
                    });
                });
            }).then(function checkForSingleMessage(){
                _this.pnotifyPopUps.map(function(element){
                    element.getInnerHtml().then(function(text){
                        if (text.indexOf(expectedText) !== -1){
                            _this.exists = true;
                        }
                    });
                });
            }).then(function assert(){
                expect(_this.exists).toBeTruthy();
            });     
        };
    };

    applicationName.notifications = new notifications();
};