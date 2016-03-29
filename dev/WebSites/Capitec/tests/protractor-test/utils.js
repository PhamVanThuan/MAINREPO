module.exports = function(applicationName){
	var utils = function(){

	  	this.selectOption = function(selectList, item){
		    var desiredOption;
		    selectList.click();
		    selectList.findElements(protractor.By.tagName('option'))
		        .then(function (options){
		            options.some(function(option){
		                option.getText().then(function (text){
		                    if (item === text){
		                        desiredOption = option;
		                        return true;
		                    }
		                });
		            });
		        })
		        .then(function (){
		            if (desiredOption){
		                desiredOption.click();
		            }
		        });
		};

		this.selectFromAutoComplete = function(element, item){
		  	var ptor = protractor.getInstance();
		  	ptor.sleep(2500);
			var desiredListItem;
			element.click();
			element.findElements(protractor.By.tagName('li'))
				.then(function (listitems){
					listitems.some(function(listitem){
						listitem.getText().then(function(text){
							if(item === text){
								desiredListItem = listitem;
								return true;
							}
						});		
					});
				})
				.then(function(){
					if(desiredListItem){
						desiredListItem.click();
					}
				});
		};

		this.generateGuid = function(){
			var guid ='xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
	    	var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
	    	return v.toString(16);
			});
			return guid;
		};

		this.clearInputAndPopulate = function(element, value){
			if(value == undefined)
				return;
			element.clear();
			element.sendKeys(value);
		};

		this.checkAllSelectOptionsExist = function(options, expectedOptions){
			var _this = this;
			_this.expectedOptions = expectedOptions;
			options.map(function (element){
				element.getText().then(function expectElementToBeInList(text){
					//ignore the empty one populated by default
					var countOfEmptyOptions;
					if(text.length > 0){
						expect(_this.expectedOptions).toContain(text);
						var index = _this.expectedOptions.indexOf(text);
						if(index != -1)
							_this.expectedOptions.splice(index, 1);
					} else {
						//we might want to count the no. of empty options.
						countOfEmptyOptions++;
						if(countOfEmptyOptions > 1){
							console.log("Too many empty options were found in the select list.");
							expect(true).toBeFalsy();
						}
					}
				});
			}).then(function expectAllElementsToBeFound(){				
				expect(_this.expectedOptions.length).toEqual(0);
			});
		};

		this.checkElementExists = function(locator){
            var _this = this;
            _this.exists = false;
            browser.driver.isElementPresent(locator).then(function(elementExists){
                _this.exists = elementExists;
            });
            return _this.exists;
		};

		this.ensureBrowserIsAtPage = function(page, navigationFunc){
			if(page === undefined){
				navigationFunc();
			} else if (page !== undefined){
				browser.getCurrentUrl().then(function(url){
					if(url.indexOf(page.url) === -1 || url.indexOf(page.url + '/') > 0){
						navigationFunc();
					}
				});
			}
		};

		this.randomString = function (len, charSet) {
    		charSet = charSet || 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
    		var randomString = '';
    		for (var i = 0; i < len; i++) {
    			var randomPosition = Math.floor(Math.random() * charSet.length);
    			randomString += charSet.substring(randomPosition,randomPosition+1);
    		}
    		return randomString;
		};

		this.isCheckboxChecked = function(element){
            var _this = this;
            _this.exists = false;
            element.isSelected().then(function(value){
                _this.exists = value;
            });
            return _this.exists;
		};

		this.checkSelectOptionExist = function(selectList, expectedOption){
			var _this = this;
			_this.exists = false;
			return selectList.element.all(by.css("option")).then(function(options){
				options.some(function(option){
					option.getText().then(function(text){
						if (text === expectedOption) {
							_this.exists = true;
						};						
					});
				});
			}).then(function(){
				return _this.exists;
			});
		};

		this.getSelectListOptions = function(selectList){
			var _this = this;
			_this.options = [];
			return selectList.element.all(by.css("option")).then(function(options){
				options.some(function(option){
					option.getText().then(function(text){
						_this.options.push(text);					
					});
				});
			}).then(function(){
				return _this.options;
			});
		};

		this.getDateOfBirthFromID = function(idNumber){
			return '19' + idNumber.substring(0,2) + '-' + idNumber.substring(2,2) + '-' + idNumber.substring(4,2);
		}
	};
	applicationName.common = new utils();
};





