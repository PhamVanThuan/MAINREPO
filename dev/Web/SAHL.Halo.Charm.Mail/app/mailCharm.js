'use strict';
angular.module('sahl.halo.charm.mail.app', [])
.service('$mailCharmService', ['$charmManagerService',
function ($charmManagerService) {
    return {
        start: function () {
            $charmManagerService.register('user', 'mail', 'sahl.ui.charms.mail', 'mail.png', 'view', 'mail.tpl.html');
        }
    };
}]);

