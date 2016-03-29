angular.module('sahl.ui.halo.views.tiles.thirdparty.invoices',[
	'SAHL.Services.Interfaces.DocumentManager.queries',
	'sahl.js.core.activityManagement',
	'halo.webservices',
	'sahl.js.ui.notifications'
])
.controller('PdfTileCtrl', ['$scope', '$documentManagerQueries', '$activityManager', '$documentManagerWebService', '$toastManagerService', '$documentDownloadManagerService',
function PdfTileCtrl($scope, $documentManagerQueries, $activityManager, $documentManagerWebService, $toastManagerService, $documentDownloadManagerService) {
    $scope.downloadPdf = function(documentGuid) {

    	var invoiceParameters = {        
          LOSS_CONTROL_STORE_ID : 44
        };

    	$documentDownloadManagerService.downloadDocumentFromStor(documentGuid, invoiceParameters.LOSS_CONTROL_STORE_ID);
    };
}])
.directive('PdfTileLayout',function() {
	return {
		controller: 'PdfTileCtrl',
		restrict: 'A',
		template: '<div style="height:100%;width:100%" ng-click="downloadPdf(item.tileData.DocumentGuid)"></div>'
	};
})
;