var textSearch = require('../html/text-search.js');
var horizontalMenu = require('../html/horizontal-menu.js');
var getData = require('../modules/search-query-data-provider.js');

describe('Halo Third Party Search Specifications -->', function(){

	beforeEach(function(){
		horizontalMenu.getMenuItems().then(function(menuItems){
			menuItems.forEach(function(item) {
				if (item.name=="Third Parties"){
					item.click();
				};
			});
		});
	});

	describe('when searching for a Third Party', function(){

		it('should be able to search for a specific third party that should appear at the top of the search results', function(){
			textSearch.performSearch("Velile Tinto Cape Inc.").then(function(clients) {
				var client = clients[0];
				var expectedLegalName = "Velile Tinto Cape Inc. (Vredenburg)";
				var actualLegalName = client.legalName;
				expect(actualLegalName).toBe(expectedLegalName);
			});
		});

		it('should not return clients', function(){
			textSearch.performSearch("Papenfus").then(function(clients) {
				expect(clients.length).toBe(0);
			});
		});

		it('should return third party search results that match the search criteria', function(){
			getData("Velile Tinto Cape Inc.","SearchForThirdPartyQuery","ThirdParty").then(function(data){
 				textSearch.performSearch("Velile Tinto Cape Inc.").then(function(clients){
 					var leArray = data.ReturnData.Results.$values;
			
 					expect(clients.length).toBe(clients.length);
 					expect(leArray.length).toBe(clients.length);

 					for (var i = 0; i < leArray.length-1; i++) {
 						var expectedLegalEntity = leArray[i];
						var expectedLegalName = expectedLegalEntity.LegalName;
						var actualLegalName = clients[i].legalName;
						expect(actualLegalName).toBe(expectedLegalName);
 					};
				});
			});
		});
	});
});