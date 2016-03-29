'use strict';
/*
    document version manager
    use : keeping changes to the decision tree documents in line
*/
angular.module('sahl.tools.app.services.documentVersionManager', [
    'sahl.tools.app.serviceConfig',
])
.factory('$documentVersionManager', [function () {
    var versionProviders = new Array();
    function Keys() {
        return Object.keys(versionProviders).sort();
    }
    function LatestVersion() {
        var keys = Keys();
        return keys[keys.length - 1];
    }
    return {
        emptyDocument:function(){
            var key = LatestVersion();
            return versionProviders[key].emptyDocument();
        },
        loadDocument: function (document) {
            var loadedDocument = document;
            if (typeof (document) === "string") {
                loadedDocument = angular.fromJson(document);
            }

            var keys = Keys();
            var latestVersion = LatestVersion();
            var documentVersion = loadedDocument.jsonversion;
            if (documentVersion !== latestVersion) {
                var currentDocumentIndex = keys.indexOf(documentVersion);
                keys = keys.splice(currentDocumentIndex + 1, (keys.length - currentDocumentIndex));
                angular.forEach(keys, function (key) {
                    loadedDocument = versionProviders[key].loadDocument(loadedDocument);
                    loadedDocument = versionProviders[key].validate(loadedDocument);
                    loadedDocument.jsonversion = key;
                });
            }
            return loadedDocument
        },
        registerProvider: function (version, provider) {
            versionProviders[version] = provider;
        },
        $providers: function () {
            return versionProviders;
        }
    };
}]);