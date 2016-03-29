'use strict';

angular.module('sahl.js.core.documentManagement.types.userprofile')
    .service('$userprofileDocumentVersion_0_3', ['$documentVersionManagerService',
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
                        "documentFormatVersion": 0.3,
                        "document": {
                            "defaultLandingPage": "",
                            "searchFilters": "",
                            "defaultRole": ""
                        }
                    };
                },
                loadDocument: function (document) {
                    document.document.defaultRole = "";
                    return document;
                }
            };

            $documentVersionManagerService.registerProvider("0.3", provider);

            return {
                start: function () {}
            };
        }
    ]);
