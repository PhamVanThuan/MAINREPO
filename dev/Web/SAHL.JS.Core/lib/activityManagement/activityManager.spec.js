'use strict';
describe('[sahl.js.core.activityManagement]', function () {
    beforeEach(module('sahl.js.core.activityManagement'));
    var $activityManager;
    beforeEach(inject(function ($injector, $q) {
        $activityManager = $injector.get('$activityManager');
    }));

    describe(' - (Factory: activityManager)-', function () {
        describe("startActivityWithKey function", function () {
            beforeEach(function () {
                spyOn($activityManager, 'startActivity');
            });
            it("should start activity", function () {
                $activityManager.startActivityWithKey("Loading");
                expect($activityManager.startActivity).toHaveBeenCalled();

            });
        });

        describe("stopActivityWithKey function", function () {
            beforeEach(function () {
                spyOn($activityManager, 'stopActivity');
            });
            it("should stop activity", function () {
                $activityManager.stopActivityWithKey("Loading");
                expect($activityManager.stopActivity).toHaveBeenCalled();

            });
        });

        describe("getActivityWithKey function", function () {
            var key = "Test";
            beforeEach(function () {
                $activityManager.startActivityWithKey(key);
            });
            it("should stop activity", function () {
                var runningKey = $activityManager.getActivityWithKey(key);
                expect(_.first(runningKey)).toEqual(key);

            });
        });
    });

});
