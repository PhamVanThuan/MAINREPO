'use strict';

angular.module('sahl.js.core.fluentDecorator', [])
    .provider('$fluentDecorator', ['$provide', function FluentDecoratorProvider($provide) {
        var fluentDecorator = function (targetProvider) {
            var base = this;
            var target = targetProvider;
            base.with = function (decoratorFunction) {
                $provide.decorator(target, decoratorFunction);
                return base;
            };
            return base;
        };

        this.decorate = function (targetProvider) {
            return new fluentDecorator(targetProvider);
        };

        this.$get = [function FluentDecoratorFactory() {
            return {};
        }];
    }]);