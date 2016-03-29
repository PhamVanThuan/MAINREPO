var searchResultItem = {};

searchResultItem.create = function(ele){
	var deferred2 = protractor.promise.defer();
	
	var item = {
		legalName: "",
		LegalIdentity:"",
		registrationNumber:"",
		homePhoneNumber:"",
		workPhoneNumber:"",
		cellPhoneNumber:"",
		emailAddress:"",
		Element: ele,
		update: function() {
			var deferred = protractor.promise.defer();
			item = this;
			item.Element.element(by.tagName('h5')).then(function(header){
				header.getText().then(function(headerText) {
					item.legalName = headerText;
					item.Element.element(by.tagName('td')).then(function(td){
						td.getText().then(function(text) {
							var expression = "Id No. - ";
							var startIndex = text.indexOf(expression);
							var subStr = "";
							if (startIndex > 0){
								subStr = text.substring((startIndex+expression.length),(startIndex + expression.length + 13));
							};
							item.LegalIdentity = subStr;
							deferred.fulfill(item);
						});
					});
				});
			});
			return deferred.promise;
		}
	};
	deferred2.fulfill(item);
	return deferred2.promise;
};

module.exports = searchResultItem;
