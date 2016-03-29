'use strict';

angular.module('halo.start.portalpages.entity.common.edit', [
        'sahl.js.core.activityManagement',
        'sahl.js.ui.forms.validation',
        'sahl.js.ui.forms',
        'sahl.websites.halo.services.tileEditManagement',
        'sahl.js.ui.pages'
    ])
    .config([
        '$stateProvider', function($stateProvider) {
            var state = function () {
                this.url = '/edit';
                this.templateUrl = 'app/start/portalPages/entity/common/edit/edit.tpl.html';
                this.controller = 'EditCtrl';
            };
            _.each(['client', 'task', 'thirdparty'], function (entity) {
                $stateProvider.state('start.portalPages.' + entity + '.common.edit', new state());
            });
        }
    ])
    .controller('EditCtrl', [
        '$rootScope', '$scope', '$state', '$stateParams',
        '$viewManagerService', '$tileEditManager', '$toastManagerService', '$activityManager', '$pageFactory',
        function TileEditController($rootScope, $scope, $state, $stateParams,
            $viewManagerService, $tileEditManager, $toastManagerService, $activityManager, $pageFactory) {
            var viewData = {
                configuration: null,
                businessKey: null,
                previousState: null
            };

            var internal = {
                setViewData: function(input, targetConfiguration) {
                    viewData.businessKey = input.businessKey;
                    viewData.configuration = targetConfiguration;
                }
            };

            var operations = {
                init: function () {
                    internal.setViewData($stateParams.model, $stateParams.targetConfiguration);
                    $pageFactory.setViewData($stateParams.previousState);
                    $scope.cancel = $pageFactory.back;
                    $scope.editView = $viewManagerService.getEditView($stateParams.model.tileData.$type);
                    $scope.item = $stateParams.model;
                    $scope.save = operations.save;
                    $scope.submitting = false;
                },
                save: function() {
                    $scope.editForm.$dirty = true;
                    if ($scope.editForm.$valid) {
                        $activityManager.startActivityWithKey("saving");
                        $scope.submitting = true;
                        $tileEditManager.save({
                                    '$type': viewData.configuration
                                },
                                viewData.businessKey,
                                $stateParams.model.tileData)
                            .then(function() {
                                $activityManager.stopActivityWithKey("saving");
                                $toastManagerService.success({
                                    title: 'Success',
                                    text: 'save complete'
                                }).promise.then(function() {
                                    $pageFactory.back();
                                });
                            }, function() {
                                $activityManager.stopActivityWithKey("saving");
                                $toastManagerService.error({
                                    title: 'Error',
                                    text: 'An error has occured while saving this, please try again in a few minutes.'
                                });
                                $scope.submitting = false;
                            });
                    }
                }
            };

            operations.init();

            $scope.$on('$destroy', function () {
                $scope.editView = null;
                $scope.item = null;
            });
        }
    ]);
