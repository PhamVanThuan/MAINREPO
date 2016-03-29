'use strict';

angular.module('sahl.tools.app.home.design.file-menu.export.png', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.export.png', {
        url: "/png",
        templateUrl: "./app/home/design/file-menu/export/png/png.tpl.html",
        controller: 'FileExportPngCtrl'
    });
})

.controller('FileExportPngCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$designSurfaceManager', '$eventAggregatorService', '$eventDefinitions',
    function FileExportPngCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $designSurfaceManager,$eventAggregatorService,$eventDefinitions) {

        $scope.imageSizes = {
            available: ["800 x 600", "1024 x 768", "1280 x 1024", "actual size"],
            current: "800 x 600"
        };

        var init = function () {

            $scope.exportfilename = $rootScope.document.data.name.replace(/\s+/g, '').toLowerCase() + ".png";

            $scope.updateImageExport = function () {
                if ($scope.imageSizes.current == "800 x 600") {
                    // export to PNG preview
                    var png = $designSurfaceManager.exportToPNG(800, 600);

                    $("#preview").empty();
                    $("#preview").append(png);
                }
                else
                    if ($scope.imageSizes.current == "1024 x 768") {
                        // export to PNG preview
                        var png = $designSurfaceManager.exportToPNG(1024, 768);

                        $("#preview").empty();
                        $("#preview").append(png);
                    }
                    else
                        if ($scope.imageSizes.current == "1280 x 1024") {
                            // export to PNG preview
                            var png = $designSurfaceManager.exportToPNG(1280, 1024);

                            $("#preview").empty();
                            $("#preview").append(png);
                        }
                        else
                            if ($scope.imageSizes.current == "actual size") {
                                // export to PNG preview
                                var png = $designSurfaceManager.exportToPNG();

                                $("#preview").empty();
                                $("#preview").append(png);
                            }
                $scope.exportdata = png.src;
            }

            $scope.updateImageExport();
        }

        if ($rootScope.document !== undefined) {
            init();
        }

        var eventHandlers = {
            onDocumentLoaded: function (loadedDocument) {
                init();
            }
        }

        $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
    }]);