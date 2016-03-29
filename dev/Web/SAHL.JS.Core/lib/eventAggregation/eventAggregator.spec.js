'use strict';
describe('[sahl.js.core.eventAggregation]', function () {
    beforeEach(module('sahl.js.core.eventAggregation'));
    beforeEach(inject(function ($injector, $q) {
    }));

    describe(' - (Service: eventAggregator)-', function () {
        var $rootScope, $eventAggregator;
        beforeEach(inject(function ($injector, $q) {
            $rootScope = $injector.get('$rootScope');
            $eventAggregator = $injector.get('$eventAggregatorService');
        }));

        describe('when a new event aggregator has been created', function () {
            it('it should have no event subscribers', function () {
                expect($eventAggregator.getNumberOfRegisteredSubscribers()).toEqual(0);
            });

            it('it should be startable', function () {
                expect($eventAggregator.start).not.toBeNull();
                $eventAggregator.start();
            });
        });

        describe('when subscribing to an event message', function () {
            var eventName = 'SomeEvent';
            var handler1 = {
                onSomeMessage: function (message) {

                }
            };
            describe('given there are currently no subscriptions', function () {

                it('should have a single event subscription to that specific event', function () {
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    expect($eventAggregator.getNumberOfRegisteredSubscribersForEvent(eventName)).toEqual(1);
                });
            });

            describe('given the same subscription already exists', function () {
                it('should not add a new subscription', function () {
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    expect($eventAggregator.getNumberOfRegisteredSubscribersForEvent(eventName)).toEqual(1);
                });
            });
        });

        describe('when publishing an event message', function () {
            var eventName = 'SomeEvent';
            var handler1 = {
                onSomeMessage: function (message) {

                }
            };
            var handler2 = {
                onSomeMessage: function (message) {
                    $eventAggregator.unsubscribe(eventName, handler1.onSomeMessage);
                }
            };

            describe('given there is a single subscription for the event with a valid handler', function () {
                it('should publish the event to the subscriber', function () {
                    spyOn(handler1, 'onSomeMessage');
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    $eventAggregator.publish(eventName, 'somedata');
                    expect(handler1.onSomeMessage).toHaveBeenCalled();
                });
            });

            describe('given there is a single subscription for the event with an invalid handler', function () {
                it('should publish the event to the subscriber', function () {
                    spyOn(handler1, 'onSomeMessage');
                    $eventAggregator.subscribe(eventName, handler1.HandlerThatDoesntExist);
                    $eventAggregator.publish(eventName, 'somedata');
                    expect(handler1.onSomeMessage).not.toHaveBeenCalled();
                });
            });

            describe('given there are no subscriptions for the event', function () {
                it('should publish the event to the subscriber', function () {
                    spyOn(handler1, 'onSomeMessage');
                    $eventAggregator.publish(eventName, 'somedata');
                    expect(handler1.onSomeMessage).not.toHaveBeenCalled();
                });
            });

            describe('given that the handler removes a previous subscription', function () {
                it('should correctly collect the removal during publish and remove it after complete', function () {
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    $eventAggregator.subscribe(eventName, handler2.onSomeMessage);
                    $eventAggregator.publish(eventName, 'somedata');
                    expect($eventAggregator.getNumberOfRegisteredSubscribersForEvent(eventName)).toEqual(1);
                });
            });
        });

        describe('when unsubscribing for an event', function () {
            var eventName = 'SomeEvent';
            var handler1 = {
                onSomeMessage: function (message) {

                }
            };
            var handler2 = {
                onSomeOtherMessage: function (message) {

                }
            };

            describe('given there is a single subscription for the event', function () {
                it('should remove the subscription to the subscriber', function () {
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    $eventAggregator.unsubscribe(eventName, handler1.onSomeMessage);
                    expect($eventAggregator.getNumberOfRegisteredSubscribersForEvent(eventName)).toEqual(0);
                });
            });

            describe('given there is more than one subscription for the event', function () {
                it('should remove the subscription to the paticular subscriber but leave the others', function () {
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    $eventAggregator.subscribe(eventName, handler2.onSomeMessage);
                    $eventAggregator.unsubscribe(eventName, handler1.onSomeMessage);
                    expect($eventAggregator.getNumberOfRegisteredSubscribersForEvent(eventName)).toEqual(1);
                });
            });

            describe('given that the subscription does not exist', function () {
                it('should leave the subscriptions unchanged', function () {
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    $eventAggregator.unsubscribe(eventName, handler2.onSomeOtherMessage);
                    expect($eventAggregator.getNumberOfRegisteredSubscribersForEvent(eventName)).toEqual(1);
                });
            });
        });

        describe('when clearing all event subscriptions', function () {
            var eventName = 'SomeEvent';
            var handler1 = {
                onSomeMessage: function (message) {

                }
            };
            var handler2 = {
                onSomeMessage: function (message) {

                }
            };

            describe('given there is more than one subscription to an event', function () {
                it('should remove all the subscriptions', function () {
                    $eventAggregator.subscribe(eventName, handler1.onSomeMessage);
                    $eventAggregator.subscribe(eventName, handler2.onSomeMessage);
                    $eventAggregator.clear();
                    expect($eventAggregator.getNumberOfRegisteredSubscribersForEvent(eventName)).toEqual(0);
                });

            });
        });
    });
});