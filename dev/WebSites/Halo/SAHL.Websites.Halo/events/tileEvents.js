'use strict';

angular.module('sahl.websites.halo.events.tileEvents', [])
.service('$tileEvents',
    function () {
        return {
            onRootTileLoaded: 'onRootTileLoaded',
            onTileDrillDown: 'onTileDrillDown',
            onViewStart: 'onViewStart'
        };
    });
