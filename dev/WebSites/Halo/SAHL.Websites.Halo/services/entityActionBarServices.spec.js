'use strict';
describe('[halo.core.start.entityActionBarServices]', function () {
    var rootScope, serviceTesting, eventAggregatorService;

    var largeGroups = {
        group1: {placements: [0], includedWithFilter: true},
        group2: {placements: [1, 4, 2, 6, 3, 5, 7, 9], includedWithFilter: true},
        group3: {placements: [8, 10, 11], includedWithFilter: true}
    };
    var masterArray = [{item: 0}, {item: 1}, {item: 2}, {item: 3}, {item: 4}, {item: 5}, {item: 6}, {item: 7}, {item: 8}, {item: 9}, {item: 10}, {item: 11}];

    beforeEach(module('halo.core.start.entityActionBarServices'));
    beforeEach(inject(function ($entityActionBarSlidingWindowService, $rootScope, $eventAggregatorService) {
        serviceTesting = $entityActionBarSlidingWindowService;
        eventAggregatorService = $eventAggregatorService
        rootScope = $rootScope;
    }));
    describe('starting service', function () {
        it('should not fail', function () {
            expect(serviceTesting).not.toBe(null);
        });
    });

    describe('when moving Scrolling Left', function () {
        describe('when cannot scroll left', function () {
            beforeEach(function () {
                spyOn(eventAggregatorService, 'publish');
            });
            beforeEach(function () {
                serviceTesting.scrollLeft(1);
            });
            it('should not publish any changes', function () {
                expect(eventAggregatorService.publish).not.toHaveBeenCalled();
            });
        });
        describe('when can scroll left', function () {
            beforeEach(function () {
                serviceTesting.recalculate(largeGroups, masterArray);
                serviceTesting.setMaxAmountOfActionsOnScreen(2);
                serviceTesting.scrollRight(1);
            });
            describe('as last left scroll', function () {
                beforeEach(function () {
                    spyOn(eventAggregatorService, 'publish');
                    serviceTesting.scrollLeft(1);
                });
                it('should scroll left and publish that it can no longer scroll left ', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarScrollLeftChanged', false);
                });
                it('should publish new window view', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarViewWindowChanged', jasmine.any(Object));
                });
            });
            describe('not as last left scroll', function () {
                beforeEach(function () {
                    serviceTesting.scrollRight(1);
                    spyOn(eventAggregatorService, 'publish');
                    serviceTesting.scrollLeft(1);
                });
                it('should scroll left and publish that it can no longer scroll left ', function () {
                    expect(eventAggregatorService.publish).not.toHaveBeenCalledWith('EntityActionBarScrollLeftChanged', false);
                });
                it('should publish new window view', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarViewWindowChanged', jasmine.any(Object));
                });
            });
        });
    });

    describe('when moving Scrolling Right', function () {
        describe('when cannot scroll Right', function () {
            beforeEach(function () {
                spyOn(eventAggregatorService, 'publish');
            });
            beforeEach(function () {
                serviceTesting.scrollRight(1);
            });
            it('should not publish any changes', function () {
                expect(eventAggregatorService.publish).not.toHaveBeenCalled();
            });
        });
        describe('when can scroll right', function () {
            beforeEach(function () {
                serviceTesting.recalculate(largeGroups, masterArray);
                serviceTesting.setMaxAmountOfActionsOnScreen(2);
            });
            describe('as last right scroll', function () {
                beforeEach(function () {
                    serviceTesting.setMaxAmountOfActionsOnScreen(11);
                    serviceTesting.recalculate();
                    spyOn(eventAggregatorService, 'publish');
                    serviceTesting.scrollRight(1);
                });
                it('should scroll right and publish that it can no longer scroll right ', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarScrollRightChanged', false);
                });
                it('should publish new window view', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarViewWindowChanged', jasmine.any(Object));
                });
            });
            describe('not as last right scroll', function () {
                beforeEach(function () {
                    spyOn(eventAggregatorService, 'publish');
                    serviceTesting.scrollRight(1);
                });
                it('should scroll left and publish that it can no longer scroll left ', function () {
                    expect(eventAggregatorService.publish).not.toHaveBeenCalledWith('EntityActionBarScrollRightChanged', false);
                });
                it('should publish new window view', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarViewWindowChanged', jasmine.any(Object));
                });
            });
            describe('should skip over groups no longer in view', function () {
                var groups;
                beforeEach(function () {

                    eventAggregatorService.subscribe('EntityActionBarViewWindowChanged', function (group) {
                        groups = group;
                    });
                    serviceTesting.recalculate(largeGroups, masterArray);
                    serviceTesting.scrollRight(3);
                });
                it('group should not be in returning window', function () {
                    expect(groups.group1).toBeFalsy();
                });
            });
        });
    });

    describe('when resetting view to beginning', function () {
        describe('when already at front', function () {
            beforeEach(function () {
                serviceTesting.setMaxAmountOfActionsOnScreen(5);
                serviceTesting.recalculate(largeGroups, masterArray);
                spyOn(eventAggregatorService, 'publish');
                serviceTesting.resetWindow();
            });
            it('should not publish any events', function () {
                expect(eventAggregatorService.publish).not.toHaveBeenCalled();
            });
        });
        describe('when not at front', function () {
            beforeEach(function () {
                serviceTesting.setMaxAmountOfActionsOnScreen(5);
                serviceTesting.recalculate(largeGroups, masterArray);
                serviceTesting.scrollRight(1);
                spyOn(eventAggregatorService, 'publish');
                serviceTesting.resetWindow();
            });
            it('should set out event that can no longer scroll right', function () {
                expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarScrollLeftChanged', false);
            });
            it('should send out event for new viewable window', function () {
                expect(eventAggregatorService.publish).toHaveBeenCalledWith('EntityActionBarViewWindowChanged', jasmine.any(Object));
            });
        });
    });

    describe('when filtering', function () {
        var groups;
        beforeEach(function () {
            eventAggregatorService.subscribe('EntityActionBarViewWindowChanged', function (group) {
                groups = group;
            });
            serviceTesting.setMaxAmountOfActionsOnScreen(5);
            largeGroups.group2.includedWithFilter = false;
            serviceTesting.recalculate(largeGroups, masterArray);
            serviceTesting.resetWindow();
        });
        it('should not be in viewable window', function () {
            expect(groups.group2).toBeFalsy();
        });
    });

});
