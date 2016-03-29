'use strict';
angular.module('sahl.ui.halo.views.pages.common.invoices.invoiceDocumentPageState.tpl.html',['halo.webservices','SAHL.Services.Interfaces.DocumentManager.queries','sahl.js.core.activityManagement'])
	.controller('Common_Invoices_InvoiceDocumentPageStateCtrl',['$scope', '$stateParams', '$activityManager' ,'$documentManagerWebService','$documentManagerQueries', '$invoiceDocumentPageStateService',
		function($scope, $stateParams, $activityManager,$documentManagerWebService,$documentManagerQueries,$invoiceDocumentPageStateService){
			$scope.pdfDataAvailable = true;
			

			$scope.pageData = $invoiceDocumentPageStateService.pageData;
			$scope.currentPageNumber = $scope.pageData.currentPageNumber;
			$scope.totalPages = $scope.pageData.totalPages;
			
			var query = new $documentManagerQueries.GetDocumentFromStorByDocumentGuidQuery(44,$stateParams.tileData.DocumentGuid);
			$activityManager.startActivityWithKey("loading");
			$documentManagerWebService.sendQueryAsync(query).then(function(data){
				
				var base64Pdf = data.data.ReturnData.Results.$values[0].FileContentAsBase64;

				if (base64Pdf) {
					// $invoiceDocumentPageStateService.loadDocument(base64Pdf,1).then(function(pageData) {
					// 	$scope.pageData = pageData;
					// 	// $scope.currentPageNumber = $scope.pageData.currentPageNumber;
					// 	// $scope.totalPages = $scope.pageData.totalPages;
					// 	$scope.$apply();
					// });	
					download("data:application/pdf;base64,"+base64Pdf,"test.pdf","application/pdf");
					$scope.pdfDataAvailable = false;
				};
				$activityManager.stopActivityWithKey("loading");
			});

			$scope.onPrevPage = function() {
				$invoiceDocumentPageStateService.onPrevPage();			
			};

			$scope.onNextPage = function() {
				$invoiceDocumentPageStateService.onNextPage();				
			};

			$scope.$watch('pageData', function(newValue, oldValue) {
				$scope.currentPageNumber = $scope.pageData.currentPageNumber;
				$scope.totalPages = $scope.pageData.totalPages;
			}, true);			
	}])
	.service('$invoiceDocumentPageStateService', function() {
		var _pdfDoc = null;
		var _canvas = null;
		var pageRendering = false;
		var pageNumPending = null;
		var pageData = {
			currentPageNumber: 1,
			totalPages: 1
		};
		
		function convertBase64ToBinary(base64) {
		    var raw = window.atob(base64);
		    var rawLength = raw.length;
		    var array = new Uint8Array(new ArrayBuffer(rawLength));

		    for(var i = 0; i < rawLength; i++) {
		    array[i] = raw.charCodeAt(i);
		    }
		    return array;
		};

		function renderPage(page) {
			var scale = 1.0;
			var viewport = page.getViewport(scale);
			
		  	_canvas.height = viewport.height;
		  	_canvas.width = viewport.width;

		  	var renderContext = {
		    	canvasContext: _canvas.getContext('2d'),
		    	viewport: viewport
		  	};
		  	var renderTask = page.render(renderContext);

		  	// Wait for rendering to finish
			renderTask.promise.then(function () {
			  pageRendering = false;
			  if (pageNumPending !== null) {
			    // New page rendering is pending
			    renderPage(pageNumPending);
			    pageNumPending = null;
			  }
			});
		};

		function loadPage(pageNum) {
			if (!_pdfDoc) { 
				return;
			};
			pageData.currentPageNumber = pageNum;
			_pdfDoc.getPage(pageNum).then(renderPage);
		};

		function loadDocument(base64Pdf, pageNum, canvas) {			
			if (!canvas) {
				canvas = document.getElementById('documentCanvas');
			};		
			_canvas = canvas;	

			return new Promise(function(resolve, reject) {
				try {
					PDFJS.getDocument(convertBase64ToBinary(base64Pdf)).then(function (pdfDoc) {		
					    pageData.totalPages = pdfDoc.numPages;
					    _pdfDoc = pdfDoc;
					    pageData.currentPageNumber = pageNum;
						_pdfDoc.getPage(pageNum).then(renderPage);
						resolve(pageData);
				  	});	
				} catch(ex) {
					reject();
				};				
			});				
		};

		function onNextPage() {
			if (pageData.currentPageNumber >= pageData.totalPages) {
		      return;
		    }
		    pageData.currentPageNumber++;
		    queueRenderPage(pageData.currentPageNumber);
		};

		function onPrevPage() {
			if (pageData.currentPageNumber <= 1) {
			  return;
			}
			pageData.currentPageNumber--;
			queueRenderPage(pageData.currentPageNumber);
		};

		function queueRenderPage(num) {
			if (pageRendering) {
			  pageNumPending = num;
			} else {
			  loadPage(num);
			}
		};

		return {
			loadDocument: loadDocument,
			onNextPage: onNextPage,
			onPrevPage: onPrevPage,
			pageData: pageData
		};
	});