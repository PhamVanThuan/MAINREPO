'use strict';
angular.module('sahl.websites.halo.services.documentService', [
        'halo.webservices',
        'SAHL.Services.Interfaces.JsonDocument.queries',
        'SAHL.Services.Interfaces.JsonDocument.commands',
        'sahl.js.core.documentManagement'
])
.provider('$documentServiceDecoration', [function () {
    this.decoration = ['$delegate', '$jsonDocumentWebService', '$jsonDocumentQueries', '$jsonDocumentCommands'
    , function ($delegate, $jsonDocumentWebService, $jsonDocumentQueries, $jsonDocumentCommands) {
        var operations = {
            createOrUpdateDocument: function (document) {
                var jsonUserProfile = angular.toJson(document);
                var command = new $jsonDocumentCommands.CreateOrUpdateDocumentCommand(jsonUserProfile);
                return $jsonDocumentWebService.sendCommandAsync(command);
            },
            getDocumentById: function (id) {
                var query = new $jsonDocumentQueries.GetJsonDocumentByIdQuery(id);
                return $jsonDocumentWebService.sendQueryAsync(query);
            },
            getDocumentByNameAndType: function (name, type) {
                var query = new $jsonDocumentQueries.GetJsonDocumentByNameAndTypeQuery(type, name);
                return $jsonDocumentWebService.sendQueryAsync(query);
            }
        };

        return {
            createOrUpdateDocument: operations.createOrUpdateDocument,
            getDocumentById: operations.getDocumentById,
            getDocumentByNameAndType: operations.getDocumentByNameAndType
        };
    }];
    this.$get = [function () { }];
}]);
