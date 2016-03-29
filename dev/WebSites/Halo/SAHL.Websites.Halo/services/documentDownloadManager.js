'use strict';
angular.module('sahl.websites.halo.services.documentDownloadManager', [
        'halo.webservices',
        'SAHL.Services.Interfaces.DocumentManager.queries',
        'SAHL.Services.Interfaces.DocumentManager.sharedmodels'
])
.service('$documentDownloadManagerService', ['$documentManagerQueries', '$documentManagerWebService', '$activityManager','$toastManagerService',
    function ($documentManagerQueries, $documentManagerWebService, $activityManager, $toastManagerService) {

        var operations = {
            downloadDocumentFromStor: function (documentGuid, storId) {
                var query = new $documentManagerQueries.GetDocumentFromStorByDocumentGuidQuery(storId, documentGuid);
                $activityManager.startActivityWithKey("loading");

                $documentManagerWebService.sendQueryAsync(query).then(function (data) {

                    var base64Content = data.data.ReturnData.Results.$values[0].FileContentAsBase64,
                        fileExtension = '.pdf',
                        filename = data.data.ReturnData.Results.$values[0].FileName.toLowerCase();
                    filename = filename.indexOf(fileExtension) === -1 ? filename+fileExtension : filename; 
                    
                    if (base64Content) {
                        var anchor = document.createElement("a");
                        anchor.href = "data:application/octet-stream;charset=utf-16le;base64," + base64Content;
                        anchor.download = filename;
                        anchor.click();
                    }else{
                        $toastManagerService.error({
                              title: 'Error',
                              text: 'The document could not be located for download.'
                            });
                    };
                    $activityManager.stopActivityWithKey("loading");
                }, function error(data) {
                    $activityManager.stopActivityWithKey("loading");
                    angular.forEach(data.data.SystemMessages.AllMessages.$values, function(systemMessage){
                        $toastManagerService.error({
                              title: 'Error',
                              text: systemMessage.Message
                            });
                    });
                    
                });
            }
        };

        return {
            downloadDocumentFromStor: operations.downloadDocumentFromStor
        };
    }
]);