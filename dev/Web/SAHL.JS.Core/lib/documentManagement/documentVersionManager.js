'use strict';

angular.module('sahl.js.core.documentManagement')
    .factory('$documentVersionManagerService', ['$q', '$eventAggregatorService', '$logger', '$timeout', '$activityManager',
        function($q, $eventAggregatorService, $logger, $timeout, $activityManager) {

            var versionProviders = [];
            versionProviders.$forDocumentType = function(documentType) {
                return _.find(versionProviders, function(item) {
                    return item.documentType === documentType;
                });
            };

            function getLatestVersion(documentType) {
                var docType = versionProviders.$forDocumentType(documentType);
                if (!_.isUndefined(docType)) {
                    var sorted = _.sortBy(docType.versions, function(item) {
                        return item.version;
                    });
                    return _.last(sorted);
                }
                return undefined;
            }

            return {
                emptyDocument: function(documentType) {
                    var latestVersion = getLatestVersion(documentType);
                    if (!_.isUndefined(latestVersion)) {
                        var latestProvider = latestVersion.provider;
                        return latestProvider.emptyDocument();
                    } else {
                        return undefined;
                    }
                },
                loadDocument: function(document) {
                    var loadedDocument = document;

                    var documentDataVersion = loadedDocument.documentFormatVersion;
                    var documentVersions = versionProviders.$forDocumentType(loadedDocument.documentType);

                    if (_.isUndefined(documentVersions)) {
                        return loadedDocument;
                    }
                    var sortedDocumentVersions = _.sortBy(documentVersions.versions, function(item) {
                        return item.version;
                    });

                    var reachedStart = false;
                    _.each(sortedDocumentVersions, function(documentFormatVersion) {
                        if (documentFormatVersion.version === documentDataVersion) {
                            reachedStart = true;

                        } else {

                            if (reachedStart){

                                loadedDocument = documentFormatVersion.provider.loadDocument(loadedDocument);
                                loadedDocument.documentFormatVersion = documentFormatVersion.version;
                            }
                        }
                    });

                    if (reachedStart) {
                        return loadedDocument;
                    } else {
                        return undefined;
                    }
                },
                registerProvider: function(version, provider) {
                    var docType = versionProviders.$forDocumentType(provider.documentType);

                    if (_.isUndefined(docType)) {
                        versionProviders.push({
                            documentType: provider.documentType,
                            versions: [{
                                "version": version,
                                "provider": provider
                            }]
                        });
                    } else {
                        var provVersion = _.find(docType.versions, function(item) {
                            return item.version === version;
                        });

                        if (_.isUndefined(provVersion)) {
                            docType.versions.push({
                                "version": version,
                                "provider": provider
                            });
                        }
                    }
                },
                $providers: function() {
                    return versionProviders;
                }

            };
        }
    ]);
