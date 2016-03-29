'use strict';
angular.module('sahl.js.core.applicationModules', [])
.service('$charmManagerService', [function () {
    var charms = {};
    var internal = {
        registerCharm: function (group, name, module, icon, action, verb) {
            if (!charms[group]) {
                charms[group] = [];
            }
            charms[group].push({ 'name': name, 'icon': icon, 'module': module, 'action': action, 'verb': verb });
        },
        getCharmsForGroup: function (group) {
            return charms[group];
        }
    };
    return {
        registerCharm: internal.registerCharm,
        getCharmsForGroup: internal.getCharmsForGroup
    };
}]);
