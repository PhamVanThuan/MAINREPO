'use strict';
angular.module('sahl.js.ui.forms.lookup', ['sahl.js.core.lookup'])
    .directive('lookupDropdown', function() {
        return {
            restrict: 'A',
            scope: {
                lookupId: '=',
                lookupType: '@'
            },
            template: '<div class="input-control select">' +
                '<select ng-model="selectedValue" ng-options="value.description for value in values"></select>' +
                '</div>',
            controller: ['$scope', '$lookupService', function($scope, $lookupService) {
                $scope.getLookupByTypeId = function(lookupId, lookupType) {
                    $lookupService.getByLookupTypeId(lookupType, lookupId).then(function(data) {
                        $scope.selectedValue = data.data.description;
                    });
                };

                $scope.getLookupByType = function(lookupType, selectedId) {
                    $lookupService.getByLookupType(lookupType).then(function(data) {
                        $scope.values = data.data._embedded[lookupType];
                        $scope.selectedValue = _.find($scope.values, function(item) {
                            return item.id === selectedId;
                        });
                    });
                };

            }],
            link: function(scope, element, attrs, ctrls) {
                scope.getLookupByType(scope.lookupType, scope.lookupId);
            }
        };
    });
