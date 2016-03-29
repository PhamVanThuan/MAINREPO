'use strict';
angular.module('sahl.js.ui.activityManagement', [
    'sahl.js.core.activityManagement'
])
.directive('spinLoader', ['$activityManager', function ($activityManager) {
    return {
        restrict: 'A',
        scope: {},
        link: function (scope, element, attr) {
            var spinner;
            var opts = {
                length: 12,
                radius: 12,
                width: 6,
                color: '#FF6600',
                trail: 40
            };
            var activityName = attr.spinLoader;
            var target = element;
            var timer;

            function onActivityStart() {
                timer = setTimeout(function () {
                    if (!spinner) {
                        spinner = new Spinner(opts);
                    }
                    spinner.spin(target[0]);
                    target.addClass('spinLoad');
                    target.find('*').attr('disabled', true);
                }, 200);
            }

            function onActivityStop() {
                if (timer) {
                    clearTimeout(timer);
                }
                if (spinner) {
                    spinner.stop();
                    target.removeClass('spinLoad');
                    target.find('*').attr('disabled', false);
                }
            }

            $activityManager.registerSpinListenerForKey(onActivityStart, onActivityStop, activityName, element[0].id);

            scope.$on('$destroy', function () {
                $activityManager.removeListenerForKey(element[0].id, activityName);
            });
        }
    };
}]);