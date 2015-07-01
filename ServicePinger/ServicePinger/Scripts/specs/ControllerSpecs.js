'use strict'
describe("Controller : All controllers", function () {
    beforeEach(module('app'));
    beforeEach(module('app.controllers'));
    var $controller;

    beforeEach(inject(function (_$controller_) {
        // The injector unwraps the underscores (_) from around the parameter names when matching
        $controller = _$controller_;
    }));

    it('should ', function () {
        expect(0).toEqual(0);
    });
});