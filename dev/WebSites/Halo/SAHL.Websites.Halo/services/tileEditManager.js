'use strict';
angular.module('sahl.websites.halo.services.tileEditManagement', [
    'SAHL.Services.Interfaces.Halo.commands',
    'halo.webservices'
])
.service('$tileEditManager', ['$q', '$haloCommands', '$haloWebService',
    function ($q, $haloCommands, $haloWebService) {
        var operations = {
            save: function (tileConfiguration, businessKey, tileDataModel) {
                var deferred = $q.defer();
                var command = new $haloCommands.TileEditorUpdateCommand(tileConfiguration, businessKey, tileDataModel);
                $haloWebService.sendCommandAsync(command).then(function () {
                    deferred.resolve();
                }, function () {
                    deferred.reject();
                });
                return deferred.promise;
            }
        };
        return {
            save: operations.save
        };
    }
]);
