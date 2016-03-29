'use strict';

angular.module('sahl.js.core.applicationManagement')
    .service('$navigationService', [
        function () {
            //empty service for now, gets decorated with actual implementation in app.
            //this is just an interface
            var internal = {
                empty: function () {
                }
            };
            return {
                goHome: internal.empty,
                goClient: internal.empty,
                goTasks: internal.empty,
                goApps: internal.empty,
                goTo: internal.empty
            };
        }]);
