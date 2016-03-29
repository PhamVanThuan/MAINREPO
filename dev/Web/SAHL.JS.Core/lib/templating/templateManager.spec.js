'use strict';
describe('[sahl.js.core.templating]', function () {
    beforeEach(module('sahl.js.core.templating'));
    var $templateManagerService, _$httpBackend, $templateCache;
    var expectedResult = '<div>template</div>';
    var url = './test.tpl.html';
    beforeEach(inject(function ($injector, $httpBackend) {
        $templateManagerService = $injector.get('$templateManagerService');
        $templateCache = $injector.get('$templateCache');
    }));

    describe(' - (Service: templateManagerService)-', function () {
        beforeEach(inject(function ($injector, $httpBackend) {
            _$httpBackend = $httpBackend;
            _$httpBackend.whenGET(url).respond(200, expectedResult);
        }));
        describe(' - when getting template - ', function () {
            it('should add template to cache', function () {
                $templateManagerService.get(url).then(function (result) {
                    expect(result).toEqual(expectedResult);
                    expect($templateCache.get(url)[1]).toEqual(expectedResult);
                });
                _$httpBackend.flush();
            });
        });
    });
});