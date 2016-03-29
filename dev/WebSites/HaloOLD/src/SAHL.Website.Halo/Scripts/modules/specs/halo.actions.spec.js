describe("Halo.Actions Specs ->", function () {
    var common;
    var actions;
    var notifications;
    var tile;
    var notify;
    var mocks;
    var flag = false;
    var setupSpies = function (businessKey, context, tileType, action) {
        spyOn(common, "getContext").andReturn(context);
        spyOn(common, "getBusinessKey").andReturn(businessKey);
        spyOn(common, "getTileTypeName").andReturn(tileType);
        spyOn(common, "getAction").andReturn(action);
    };
    beforeEach(function () {
        require(["jquery", "halo.common", "halo.actions", "halo.notifications", "halo.tile", "notify", "mockjax"],
        function ($, haloCommon, haloActions, haloNotifications, haloTile, haloNotify, mockJax) {
            common = haloCommon;
            actions = haloActions;
            notifications = haloNotifications;
            tile = haloTile;
            notify = haloNotify;
            mocks = mockJax;
            flag = true;
        });
        waitsFor(function () {
            return flag;
        });
    });
    describe("when performing an action and the method cannot be found", function () {
        it("the callback should not be called", function () {
            var action = { actionToPerform: "Action", actionUrl: "Url" };
            spyOn(common, "getAction").andReturn(action);
            spyOn(actions, "getMethod").andReturn(undefined);
            var spyCallback = jasmine.createSpy('mySpy');
            var e = { clickedElement: "Tile1" };
            actions.performAction(e, spyCallback);
            expect(spyCallback.callCount).toEqual(0);
        });
    });
    describe("when performing an action and the method is found", function () {
        it("the callback should be called", function () {
            var action = { actionToPerform: "Action", actionUrl: "Url" };
            var mySpy = jasmine.createSpy('mySpy');
            spyOn(common, "getAction").andReturn(action);
            spyOn(actions, "getMethod").andCallFake(function () {
                return function myFunc(item, callback) { callback(); };
            });
            var e = { clickedElement: "Tile1" };
            actions.performAction(e, mySpy);
            expect(mySpy).toHaveBeenCalled();
        });
    });
    describe("when loading tile data given the businessKey is undefined", function () {
        var tile;
        beforeEach(function () {
            tile = "<div></div>";
            spyOn($, "ajax").andCallThrough();
        });
        it("should not make an ajax call", function () {
            var context = "id";
            var businessKey = { businessKey: undefined, businessKeyType: "businessKeyType" };
            var tileType = { tileModelType: "tileModelType", tileConfigType: "tileConfigType" };
            var action = { actionToPerform: "actionToPerform", actionUrl: "actionUrl" };
            setupSpies(businessKey, context, tileType, action);
            actions.loadTileData(tile)
            expect($.ajax.calls.length).toEqual(0);
        });
    });
    describe("when loading tile data given the businessKeyType is undefined", function () {
        var tile;
        beforeEach(function () {
            tile = "<div></div>";
            spyOn($, "ajax").andCallThrough();
        });
        it("should not make an ajax call", function () {
            var context = "id";
            var businessKey = { businessKey: 12345, businessKeyType: undefined };
            var tileType = { tileModelType: "tileModelType", tileConfigType: "tileConfigType" };
            var action = { actionToPerform: "actionToPerform", actionUrl: "actionUrl" };
            setupSpies(businessKey, context, tileType, action);
            actions.loadTileData(tile)
            expect($.ajax.calls.length).toEqual(0);
        });
    });
    describe("when loading tile data given the context is undefined", function () {
        var tile;
        beforeEach(function () {
            tile = "<div></div>";
            spyOn($, "ajax").andCallThrough();
        });
        it("should not make an ajax call", function () {
            var context = undefined;
            var businessKey = { businessKey: 12345, businessKeyType: "businessKeyType" };
            var tileType = { tileModelType: "tileModelType", tileConfigType: "tileConfigType" };
            var action = { actionToPerform: "actionToPerform", actionUrl: "actionUrl" };
            setupSpies(businessKey, context, tileType, action);
            actions.loadTileData(tile)
            expect($.ajax.calls.length).toEqual(0);
        });
    });
    describe("when loading tile data given the tile is undefined", function () {
        var tile;
        beforeEach(function () {
            tile = "<div></div>";
            spyOn($, "ajax").andCallThrough();
        });
        it("should not make an ajax call", function () {
            var context = "id";
            var businessKey = { businessKey: 12345, businessKeyType: "businessKeyType" };
            var tileType = undefined;
            var action = { actionToPerform: "actionToPerform", actionUrl: "actionUrl" };
            setupSpies(businessKey, context, tileType, action);
            actions.loadTileData(tile)
            expect($.ajax.calls.length).toEqual(0);
        });
    });

    describe("when loading tile data given all configuration is correctly loaded", function () {
        it("should make the ajax call with the arguments returned", function () {
            var tile = "<div></div>";
            var DrillDownTileURL = "/tile/GetTileData";
            $.mockjax({
                url: DrillDownTileURL,
                dataType: 'json',
                responseText: "some text"
            });
            spyOn($, "ajax").andCallThrough();
            var context = "id";
            var businessKey = { businessKey: 12345, businessKeyType: "businessKeyType" };
            var tileType = { tileModelType: "tileModelType", tileConfigType: "tileConfigType" };
            var action = { actionToPerform: "actionToPerform", actionUrl: "actionUrl" };
            setupSpies(businessKey, context, tileType, action);
            actions.loadTileData(tile);
            expect($.ajax.mostRecentCall.args[0]["data"]["context"]).toEqual(context);
            expect($.ajax.mostRecentCall.args[0]["data"]["businessKey"]).toEqual(businessKey.businessKey);
            expect($.ajax.mostRecentCall.args[0]["data"]["businessKeyType"]).toEqual(businessKey.businessKeyType);
            expect($.ajax.mostRecentCall.args[0]["data"]["tileModelTypeName"]).toEqual(tileType.tileModelType);
        });
    });
    describe("", function () {
        it("should", function () {

        });
    })
});
