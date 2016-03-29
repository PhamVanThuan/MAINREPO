'use strict';

angular.module('sahl.js.core.documentManagement.types.decisiontree', [
        'sahl.js.core.documentManagement'
    ])
    .service('$decisiontreeDocumentVersion_0_1', ['$documentVersionManagerService',
        function($documentVersionManagerService) {
            var provider = {
                documentType: "decisiontree",
                emptyDocument: function() {

                },
                loadDocument: function(document) {
                    return document;
                }
            };

            $documentVersionManagerService.registerProvider("0.1", provider);

            return {
                start: function() {}
            };
        }
    ]);
