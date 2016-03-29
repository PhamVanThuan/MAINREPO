'use strict';
describe('[sahl.js.core.documentManagement]', function () {
    beforeEach(module('sahl.js.core.documentManagement'));
    beforeEach(inject(function ($injector, $q) {
    }));

    describe(' - (Service: documentVersionManagement)-', function () {
        var $rootScope, $documentVersionManagerService;
        beforeEach(inject(function ($injector, $q) {
            $rootScope = $injector.get('$rootScope');
            $documentVersionManagerService = $injector.get('$documentVersionManagerService');
        }));

        describe('when registering a document version provider for a new document type', function () {
            describe('given a provider for the document type has not yet been registered', function () {
                it('it should add the document type to the list of document version provider types keys', function () {
                    var versionProvider = {
                        documentType: 'sometype',
                        emptyDocument: function () {
                        },
                        loadDocument: function (document) {
                            return document;
                        }
                    };

                    $documentVersionManagerService.registerProvider('0.1', versionProvider);
                    var providers = $documentVersionManagerService.$providers();

                    var provs = _.filter(providers, function (item) {
                        return item.documentType === 'sometype';
                    });
                    expect(provs.length).toEqual(1);
                });
            });

            describe('given a provider for the document type with an earlier version has already been registered', function () {
                it('it should add the provider to the list of versioned document providers', function () {
                    var versionProvider = {
                        documentType: 'sometype',
                        emptyDocument: function () {
                        },
                        loadDocument: function (document) {
                            return document;
                        }
                    };

                    $documentVersionManagerService.registerProvider('0.1', versionProvider);
                    $documentVersionManagerService.registerProvider('0.2', versionProvider);
                    var providers = $documentVersionManagerService.$providers();

                    expect(providers.length).toEqual(1);
                    expect(providers.$forDocumentType('sometype').versions.length).toEqual(2);
                });
            });

            describe('given a provider for the document type with the same version has already been registered', function () {
                it('it should not add the provider to the list of versioned document providers', function () {
                    var versionProvider = {
                        documentType: 'sometype',
                        emptyDocument: function () {
                        },
                        loadDocument: function (document) {
                            return document;
                        }
                    };

                    $documentVersionManagerService.registerProvider('0.1', versionProvider);
                    $documentVersionManagerService.registerProvider('0.2', versionProvider);
                    $documentVersionManagerService.registerProvider('0.2', versionProvider);
                    var providers = $documentVersionManagerService.$providers();

                    expect(providers.length).toEqual(1);
                    expect(providers.$forDocumentType('sometype').versions.length).toEqual(2);
                });
            });
        });

        describe('when requesting an empty document for a document type', function () {
            var versionProvider = {
                documentType: 'sometype',
                emptyDocument: function () {
                    return {ver: '0.1'};
                },
                loadDocument: function (document) {
                    return document;
                }
            };

            var versionProvider2 = {
                documentType: 'sometype',
                emptyDocument: function () {
                    return {ver: '0.2'};
                },
                loadDocument: function (document) {
                    return document;
                }
            };

            var result = undefined;

            describe('given the document type does not exist', function () {
                beforeEach(function () {
                    $documentVersionManagerService.registerProvider('0.1', versionProvider);
                });

                it('it should return the empty document', function () {
                    result = $documentVersionManagerService.emptyDocument('sometype23');
                    expect(result).toBeUndefined();
                });
            });

            describe('given the document type exists and a single version has been configured', function () {
                beforeEach(function () {
                    $documentVersionManagerService.registerProvider('0.1', versionProvider);
                });

                it('it should return the empty document', function () {
                    result = $documentVersionManagerService.emptyDocument('sometype');
                    expect(result).not.toBeUndefined();
                });
            });

            describe('given the document type exists and multiple versions have been configured', function () {
                beforeEach(function () {
                    $documentVersionManagerService.registerProvider('0.1', versionProvider);
                    $documentVersionManagerService.registerProvider('0.2', versionProvider2);
                });

                it('it should return the empty document of the latest version provider', function () {
                    result = $documentVersionManagerService.emptyDocument('sometype');

                    expect(result).not.toBeUndefined();
                    expect(result.ver).toEqual('0.2');
                });
            });
        });

        describe('when loading a document', function () {
            var versionProvider = {
                documentType: 'sometype',
                emptyDocument: function () {
                    return {ver: '0.1'};
                },
                loadDocument: function (document) {
                    return document;
                }
            };

            var versionProvider2 = {
                documentType: 'sometype',
                emptyDocument: function () {
                    return {ver: '0.2'};
                },
                loadDocument: function (document) {
                    return document;
                }
            };

            var doc = {
                name: '',
                description: '',
                version: 1,
                documentType: 'sometype',
                documentFormatVersion: '0.1',
                document: {}
            };

            var doc1 = {
                name: '',
                description: '',
                version: 1,
                documentType: 'someothertype',
                documentFormatVersion: '0.1',
                document: {}
            };

            var doc3 = {
                name: '',
                description: '',
                version: 1,
                documentType: 'sometype',
                documentFormatVersion: '0.3',
                document: {}
            };

            beforeEach(function () {
                $documentVersionManagerService.registerProvider('0.1', versionProvider);
            });
            describe('given the document is older than the latest document version', function () {
                it('it should upconvert the document', function () {
                    $documentVersionManagerService.registerProvider('0.2', versionProvider2);
                    var result = $documentVersionManagerService.loadDocument(doc);

                    expect(result.documentFormatVersion).toEqual('0.2');
                });
            });

            describe('given the document is newer than the latest document version', function () {
                it('it should upconvert the document', function () {
                    $documentVersionManagerService.registerProvider('0.2', versionProvider2);
                    var result = $documentVersionManagerService.loadDocument(doc3);

                    expect(result).toBeUndefined();
                });
            });

            describe('given the document type has not been registered', function () {
                it('it should return the document unmodified', function () {
                    var result = $documentVersionManagerService.loadDocument(doc1);

                    expect(result).toEqual(doc1);
                });
            });
        });
    });
});
