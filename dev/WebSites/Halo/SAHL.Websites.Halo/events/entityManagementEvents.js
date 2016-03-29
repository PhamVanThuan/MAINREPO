'use strict';

angular.module('sahl.websites.halo.events.entityManagement', [])
.service('$entityManagementEvents',
    function () {
        return {
            onEntityAdded: 'onEntityAdded',
            onEntityRemoved: 'onEntityRemoved',
            onEntityActivated: 'onEntityActivated'
        };
    });
