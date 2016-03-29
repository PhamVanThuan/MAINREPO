'use strict';

/* Services */

angular.module('sahl.tools.app.services.notificationservice', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$notificationService', function () {

    return {
        notifySuccess: function (title, message) {
            $.Notify({
                caption: title,
                content: "<div class='success-notification'><i class='icon-checkmark'/><span>" + message + "</span></div>"
            });
        },
        notifyError: function (title, message) {
            $.Notify({
                caption: title,
                content: "<div class='error-notification'><i class='icon-warning'/><span>" + message + "</span></div>"
            });
        },
        notifyInfo: function (title, message) {
            $.Notify({
                caption: title,
                content: "<div class='info-notification'><i class='icon-info'/><span>" + message + "</span></div>"
            });
        },
        notify: function (title, message) {
            $.Notify({
                caption: title,
                content: message
            });
        }
    };
})