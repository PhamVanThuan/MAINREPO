'use strict';

angular.module('sahl.js.core.documentManagement.types.userprofile')
    .service('$userprofileDocumentVersion_0_1', ['$documentVersionManagerService',
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
                        "documentFormatVersion": 0.1,
                        "document": {
                            "defaultLandingPage": ""
                        }
                    };



                },
                loadDocument: function (document) {
                    return document;
                }
            };

            $documentVersionManagerService.registerProvider("0.1", provider);

            return {
                start: function () {}
            };
        }
    ]);
