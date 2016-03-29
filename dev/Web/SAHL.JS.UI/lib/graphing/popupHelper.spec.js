'use strict';
describe('[sahl.js.ui.graphing]', function () {
    beforeEach(module('sahl.js.ui.graphing'));
    var $popupHelperService, paper;
    
    beforeEach(inject(function ($injector, $q) {
        $popupHelperService = $injector.get('$popupHelperService');
        paper = Raphael(0,0,800,600);
    }));

    describe(' - (Service: popupHelper)-', function () {
        var popupHelper;
        beforeEach(inject(function () {
            popupHelper = $popupHelperService.createPopup(paper);
        }));
        describe('when creating a new popupHelper', function () {
            it('should create new object with which we can update and hide', function () {
                expect(popupHelper).not.toBeUndefined();
            });

            describe('when updating empty text', function () {
                var text;
                it('should not throw errors with empty text',function(){
                    expect(function () { popupHelper.update([], 0, 0, 0, 800, 600); }).not.toThrow();
                });
            });

            describe('when updating actual text that is top left', function () {
                var text;
                it('should not throw errors with empty text', function () {
                    expect(function () {
                        popupHelper.update([{ text: 'test',attr: { font: '12px Helvetica, Arial', fill: '#fff' } }], 0, 0, 0, 800, 600);
                    }).not.toThrow();
                });
            });


            describe('when updating actual text top right', function () {
                var text;
                it('should not throw errors with empty text', function () {
                    expect(function () {
                        popupHelper.update([{ text: 'test', attr: { font: '12px Helvetica, Arial', fill: '#fff' } }], 800, 0, 0, 800, 600);
                    }).not.toThrow();
                });
            });

            describe('when updating actual text top right', function () {
                var text;
                it('should not throw errors with empty text', function () {
                    expect(function () {
                        popupHelper.update([{ text: 'test', attr: { font: '12px Helvetica, Arial', fill: '#fff' } }], 0, 600, 0, 800, 600);
                    }).not.toThrow();
                });
            });

            describe('when hiding text', function () {
                beforeEach(function () {
                    popupHelper.hide();
                });
                it('should not throw any errors', function () {
                    expect(function () { popupHelper.hide(); }).not.toThrow();
                });
            });
        });
    });
});