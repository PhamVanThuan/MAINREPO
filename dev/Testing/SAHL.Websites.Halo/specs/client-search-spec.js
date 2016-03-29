var textSearch = require('../html/text-search.js');
var getData = require('../modules/search-query-data-provider.js');
var horizontalMenu = require('../html/horizontal-menu.js');

describe('Halo Client Search Specifications -->', function(){

	//set to search tab
	beforeEach(function(){
		browser.refresh();
		horizontalMenu.getMenuItems().then(function(menuItems){
			expect(menuItems.length).toBeGreaterThan(0);
			menuItems.forEach(function(item) {
				if (item.name=="Clients"){
					item.click();
				};
			});
		});
	});

	describe('when searching for a client', function(){

		it('should not return third parties', function(){
			textSearch.performSearch("Randles Inc").then(function(clients) {
				expect(clients.length).toBe(0);
			});
		});

		it('should display at least one search results', function(){
			textSearch.performSearch("17438/2002").then(function(clients) {
				expect(clients.length).toBeGreaterThan(0);
			});
		});

		it('should be able to search for a specific client that should appear at the top of the search results', function(){
			textSearch.performSearch("17438/2002").then(function(clients) {
				var client = clients[0];
				var expectedLegalName = "Eben Luus Testamentere Trust trading as Eben Luus Testamentere Trust";
				var actualLegalName = client.legalName;
				expect(actualLegalName).toBe(expectedLegalName);
			});
		});

		it('should return client search results that match the search criteria', function(){
			getData("susan","SearchForClientQuery","Client").then(function(data){
 				textSearch.performSearch("susan").then(function(clients){
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

	describe('when searching for a client by id number', function(){

		it('should return a single result', function(){
			getData("7006165147083","SearchForClientQuery","Client").then(function(data){
 				textSearch.performSearch("7006165147083").then(function(clients){
 					var leArray = data.ReturnData.Results.$values;
 					expect(clients.length).toBe(1);
 					expect(leArray.length).toBe(1);
 					expect(leArray[0].LegalIdentity).toBe(clients[0].LegalIdentity);
				});
			});
		});

	});
});