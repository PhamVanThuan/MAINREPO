'use strict';

angular.module('sahl.tools.app.home.design.file-menu.export.svg', [
  'ui.router',
  'sahl.tools.app.services'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu.export.svg', {
        url: "/svg",
        templateUrl: "./app/home/design/file-menu/export/svg/svg.tpl.html",
        controller: 'FileExportSvgCtrl'
    });
})

.controller('FileExportSvgCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$designSurfaceManager','$eventAggregatorService','$eventDefinitions',
    function FileExportSvgCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $designSurfaceManager, $eventAggregatorService, $eventDefinitions) {
        var  init = function () {

                $scope.imageSizes = {
                    available: ["800 x 600", "1024 x 768", "1280 x 1024", "actual size"],
                    current: "800 x 600"
                };

                $scope.exportfilename = $rootScope.document.data.name.replace(/\s+/g, '').toLowerCase() + ".svg";
                
                $scope.updateImageExport = function () {
                    if ($scope.imageSizes.current == "800 x 600") {
                        // export to PNG preview
                        var svg = $designSurfaceManager.exportToSVG(800, 600);

                        $("#preview").empty();
                        $("#preview").append(svg);
                    }
                    else
                        if ($scope.imageSizes.current == "1024 x 768") {
                            // export to SVG preview
                            var svg = $designSurfaceManager.exportToSVG(1024, 768);

                            $("#preview").empty();
                            $("#preview").append(svg);
                        }
                        else
                            if ($scope.imageSizes.current == "1280 x 1024") {
                                // export to SVG preview
                                var svg = $designSurfaceManager.exportToSVG(1280, 1024);

                                $("#preview").empty();
                                $("#preview").append(svg);
                            }
                            else
                                if ($scope.imageSizes.current == "actual size") {
                                    // export to SVG preview
                                    var svg = $designSurfaceManager.exportToSVG();

                                    $("#preview").empty();
                                    $("#preview").append(svg);
                                }
                    $scope.exportdata = "data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg'>" + svg.outerHTML.replace(/fill=\"transparent\"/gi, "fill=\"transparent\" style=\"stroke:none;opacity:0\"") + "</svg>";
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