'use strict';
angular.module('sahl.halo.app.organisationStructure.organisationStructureChart.directives', [])
    .directive('organisationStructureChart', ['$rootScope',
        function ($rootScope) {
            
            return {
                restrict: 'E',
                scope: {
                    organisationStructureData: '=',
                    two: '=',
                    organisationStructureModel: '='
                },
                template: '<div id="oscDiagram" style="background-color: #696969; border: solid 1px black; height: 600px"></div>',
                link: function (scope, element, attrs, ngModel) {
                    //organisationStructure.init($rootScope);
                }
            };
        }
    ]);


