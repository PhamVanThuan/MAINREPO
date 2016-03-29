describe('Halo.Common Specs ->', function() {
	var common;
	var mocks;
	var constants;
	var flag=false;
	beforeEach(function(){
		require(["jquery", "halo.common", "halo.constants", "mockjax"], function($, haloCommon, haloConstants, mockJax){
			common = haloCommon;
			constants = haloConstants;
			mocks = mockJax;
			flag = true;
		});
		waitsFor(function(){
			return flag;
		});
	});
	afterEach(function(){
		$('#menubar-context').remove();
	})
	describe('when getting the context', function(){			
		it("should return menu_item_id", function() {
			spyOn($.fn, "attr").andReturn("menu_item_id");
			var context = common.getContext();
			expect(context).toBe("id");
		});
	});
	describe('when getting the business key given an element with the business key and business key type tags', function(){
		var element;
		var businessKey;
		beforeEach(function(){
			element = "<div data-business-key='12345' data-business-keytype='LegalEntity'/>";
			businessKey = common.getBusinessKey(element);
		});
			it("should return the business key", function() {
				expect(businessKey.businessKey).toBe(12345);	
			});
			it("should return the business key type", function(){
				expect(businessKey.businessKeyType).toBe("LegalEntity");		
			});
	});
	describe("when getting the action given an element with the data action and data action url tags", function(){
		var element;
		var actionAndURL;
		beforeEach(function(){
			element = "<div data-action-url='URL' data-action='drilldown'/>";
			actionAndURL = 	common.getAction(element);
		});
			it("should return the action to perform", function(){
				expect(actionAndURL.actionToPerform).toBe("drilldown");
			});
			it("should return the action URL", function(){
				expect(actionAndURL.actionUrl).toBe("URL");
			});
	});
	describe("when getting the tile model type and configuration", function(){
		var element;
		var tileModelAndConfig;
		beforeEach(function() {
			element = "<div data-tile-modeltype='LegalEntity.Default.LegalEntityMajorTile' data-tile-configtype='LegalEntityMajorTileConfiguration'/>";
			tileModelAndConfig = common.getTileTypeName(element);
		});
			it("should return the tile model type", function(){
				expect(tileModelAndConfig.tileModelType).toBe("LegalEntity.Default.LegalEntityMajorTile"); 
			});
			it("should return the tile mode configuration type", function(){
				expect(tileModelAndConfig.tileConfigType).toBe("LegalEntityMajorTileConfiguration");
			});
	});
	describe("when retrieving the business context", function() {
		var contextMenuUrl;
		var outputID;
		var contextMenuBar;
		var element = '<div id="menubar-context"></div>';
		beforeEach(function(){
			$('body').append(element);
			contextMenuUrl = "/tile/GetContextMenu";
			outputID = "#menubar-context";
			contextMenuBar = "Some Text";			
		});
		afterEach(function(){
			$.mockjaxClear();
		});

		it("should make the ajax call with the correct business context data", function(){
			$.mockjax({
						url: contextMenuUrl,
						dataType: 'json',
						responseText: contextMenuBar
						});
			spyOn($, "ajax").andCallThrough();
			//spyOn(constants, "GetContextURL").andReturn(contextMenuUrl);
			var businessContext = { context: "test" , businessKey: "12345", businessKeyType: "LegalEntity" };
			common.GetBusinessContext(businessContext);
			expect($.ajax.mostRecentCall.args[0]["data"]["context"]).toEqual(businessContext.context);
			expect($.ajax.mostRecentCall.args[0]["data"]["businessKey"]).toEqual(businessContext.businessKey);
			expect($.ajax.mostRecentCall.args[0]["data"]["businessKeyType"]).toEqual(businessContext.businessKeyType);
		});

		it("should set the html content of the menubar-context", function(){
			spyOn($, "ajax").andCallFake(function(req) {
				var d = $.Deferred();
				d.resolve(contextMenuBar);
				return d.promise();
			});
			var businessContext = { context: "test" , businessKey: "12345", businessKeyType: "LegalEntity" };
			common.GetBusinessContext(businessContext);
			expect($(outputID).text()).toBe(contextMenuBar);
		});
	});
});