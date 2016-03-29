'use strict';
angular.module('sahl.js.ui.notifications')
.provider('$toastManagerService', [function () {
    this.configure = function (configuration) {
        for (var key in configuration) {
            if (PNotify.prototype.options && configuration.hasOwnProperty(key) && PNotify.prototype.options.hasOwnProperty(key) && configuration[key]) {
                PNotify.prototype.options[key] = configuration[key];
            }
        }
    };

    this.$get = ['$q',function ($q) {
        var operations = {
            notices: function () {
                return PNotify.notices;
            },
            closeAll: function () {
                PNotify.removeAll();
            },
            notify: function (params) {
                var deferred = $q.defer();
                params.after_close = function (toast, time) {
                    deferred.resolve(toast, time);
                };
                var toast = new PNotify(params);
                return { promise: deferred.promise, toast: toast };
            },
            notice: function (params) {
                return operations.notify(params);
            },
            info: function (params) {
                params.type = 'info';
                return operations.notify(params);
            },
            error: function (params) {
                params.type = 'error';
                params.hide = false;
                return operations.notify(params);
            },
            success: function (params) {
                params.type = 'success';
                return operations.notify(params);
            },
            tooltip: function (text) {
                var params = {
                    text: text,
                    hide: false,
                    buttons: {
                        closer: false,
                        sticker: false
                    },
                    history: { history: false },
                    animate_speed: 100,
                    opacity: .9,
                    icon: false,
                    stack: false,
                    addclass: '',
                    auto_display: false
                };
                return new PNotify(params);
            }

        };
        return {
            notices: operations.notices,
            closeAll: operations.closeAll,
            notify: operations.notify,

            notice: operations.notice,
            info: operations.info,
            error: operations.error,
            success: operations.success,

            tooltip: operations.tooltip
        };
    }];
}]);
