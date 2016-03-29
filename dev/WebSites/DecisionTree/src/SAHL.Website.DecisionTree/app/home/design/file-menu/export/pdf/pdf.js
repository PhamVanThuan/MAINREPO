'e strict';

angular.module('sahl.tools.app.home.design.file-menu.export.pdf', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.export.pdf', {
        url: "/pdf",
        templateUrl: "./app/home/design/file-menu/export/pdf/pdf.tpl.html",
        controller: 'FileExportPdfCtrl'
    });
})

.controller('FileExportPdfCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$designSurfaceManager', '$eventAggregatorService', '$eventDefinitions',
    function FileExportPdfCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $designSurfaceManager,$eventAggregatorService, $eventDefinitions) {
        var init = function () {
            $scope.exportfilename = $rootScope.document.data.name.replace(/\s+/g, '').toLowerCase() + ".pdf";

            // use the PNG image as preview
            var png = $designSurfaceManager.exportToPNG(800, 600);
            $("#preview").empty();
            $("#preview").append(png);

            var pdf = $designSurfaceManager.exportToPDF();
            $scope.exportdata = pdf.output('datauristring');
        }

        if ($rootScope.document !== undefined) {
            init();
        }

        var eventHandlers = {
            onDocumentLoaded: function (loadedDocument) {
                init();
            }
        }
    }]);