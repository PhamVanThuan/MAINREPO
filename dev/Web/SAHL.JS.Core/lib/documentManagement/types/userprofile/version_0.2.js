'use strict';

angular.module('sahl.js.core.documentManagement.types.userprofile')
    .service('$userprofileDocumentVersion_0_2', ['$documentVersionManagerService',
        function ($documentVersionManagerService) {
            var provider = {
                documentType: "userprofile",
                emptyDocument: function () {
                    return {
                        "id": "",
                        "name": "",
                        "description": "Document containing user profile data",
                        "version": 0,
                        "documentType": "userprofile",
                        "documentFormatVersion": 0.2,
                        "document": {
                            "defaultLandingPage": "",
                            "searchFilters": ""
                        }
                    };
                },
                loadDocument: function (document) {
                    document.document.searchFilters = "";
                    return document;
                }
            };

            $documentVersionManagerService.registerProvider("0.2", provider);

            return {
                start: function () {}
            };
        }
    ]);
