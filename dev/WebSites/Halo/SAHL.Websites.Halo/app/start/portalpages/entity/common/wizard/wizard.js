angular.module('halo.start.portalpages.entity.common.wizard', ['sahl.js.ui.pages'])
.config(['$stateProvider', function ($stateProvider) {
    var stateSettings = function (urlType) {
        this.url = '/' + urlType;
        this.templateUrl = 'app/start/portalPages/entity/common/wizard/wizard.tpl.html';
        this.controller = 'WizardCtrl';
    };
    _.each(['client', 'task', 'thirdparty'], function (entity) {
        _.each(['wizard', 'workflow'], function (view) {
            $stateProvider.state('start.portalPages.' + entity + '.common.' + view, new stateSettings(view));
        });
    });
}])
.controller('WizardCtrl', ['$rootScope', '$scope', '$state', '$stateParams', '$viewManagerService', '$tileEditManager',
                           '$toastManagerService', '$activityManager', '$eventAggregatorService', '$tileManagerService',
                           '$workflowActivityManager', '$guidService', '$pageFactory', '$wizardFactory',
function WizardController($rootScope, $scope, $state, $stateParams, $viewManagerService, $tileEditManager,
                          $toastManagerService, $activityManager, $eventAggregatorService, $tileManagerService,
                          $workflowActivityManager, $guidService, $pageFactory, $wizardFactory) {
    var workflowName = 'workflow';
    var enabled = true;
    $scope.buttonsVisible = $wizardFactory.internal.buttonsVisible && enabled;
    $wizardFactory.internal.buttonsChanged(function (visible) {
        $scope.buttonsVisible = visible && enabled;
    });
    
    var isWorkFlow = $state.current.name.indexOf(workflowName, $state.current.name.length - workflowName.length) !== -1;

    $scope.guid = $guidService.newGuid();
  
   

    var viewData = {
        configuration: null,
        businessKey: null,
        previousState: null
    };

    var internal = {
        setViewData: function (input, targetConfiguration) {
            viewData.businessKey = input.businessKey;
            viewData.configuration = targetConfiguration;
        },
        setWizardConfiguration: function (wizardConfiguration) {
            $scope.wizardConfiguration = wizardConfiguration;
            $scope.wizardPages = wizardConfiguration.wizardPages.$values;
            $scope.$parent.wizardConfiguration = $scope.wizardConfiguration;
        }
    };

    var operations = {
        setPageView: function () {
            if ($scope.wizardPages.length === $scope.index) {
                actions.finish();
                return;
            }

            if ($scope.index !== 0) {
                $scope.wizardPages[$scope.index - 1].$canNavigate = true;
            }
            $scope.pageView = $viewManagerService.getWizardPage($scope.wizardPages[$scope.index].pageState);
            $scope.contentModel = $scope.wizardPages[$scope.index].contentModel;
            $scope.isLast = ($scope.index + 1) === $scope.wizardPages.length;
        },
        init: function () {
            $activityManager.startActivityWithKey("loading");
            internal.setViewData($stateParams.model, $stateParams.targetConfiguration);
            $pageFactory.setViewData($stateParams.previousState);

            $scope.item = $stateParams.model;
            operations.loadWizardConfiguration().then(function (data) {
                internal.setWizardConfiguration(data[0].WizardConfiguration);
                operations.setPageView();
                if (isWorkFlow) {
                    $scope.disabled = true;
                    $workflowActivityManager.start(
                        $stateParams.tileAction.instanceId,
                        $scope.guid,
                        $stateParams.tileAction.name,
                        false
                    ).then(function () {
                        $scope.disabled = false;
                    }, function () {
                        $scope.disabled = true;
                    });
                }
                $activityManager.stopActivityWithKey("loading");
            }, function () {
                $activityManager.stopActivityWithKey("loading");
            });
        },
        loadWizardConfiguration: function () {
            return $tileManagerService.loadWizardConfiguration(
                ($stateParams.tileAction.wizardTileConfiguration !== null ? $stateParams.tileAction.wizardTileConfiguration.Name : null),
                $stateParams.tileAction.processName,
                $stateParams.tileAction.workflowName,
                $stateParams.tileAction.name,
                $stateParams.model.businessContext
            );
        },
        moveNext: function () {
            $scope.index += 1;
            operations.setPageView();
            operations.recalcMovements();
        },
        movePrev: function () {
            $scope.index -= 1;
            operations.setPageView();
            operations.recalcMovements();
        },
        recalcMovements: function () {
            if ($scope.index !== 0 && $scope.wizardPages[$scope.index - 1].$canNavigate) {
                $scope.canNavigateBack = true;
            } else {
                $scope.canNavigateBack = false;
            }

            if (($scope.index !== ($scope.wizardPages.length - 1) && $scope.wizardPages[$scope.index + 1].$canNavigate) || $scope.wizardPages[$scope.index].$canNavigate) {
                $scope.canNavigateFoward = true;
            } else {
                $scope.canNavigateFoward = false;
            }
        }
    };

    $scope.index = 0;

    var actions = {
        cancel: function () {
            $eventAggregatorService.publish($wizardFactory.events.CANCEL_WIZARD_CLICKED);
            $pageFactory.back();
        },
        yes: function () {
            $scope.wizardPages[$scope.index].WizardInput = 'Yes';
            $eventAggregatorService.publish($wizardFactory.events.YES_WIZARD_CLICKED, { model: $scope.wizardPages[$scope.index] });
            operations.moveNext();
        },
        no: function () {
            $scope.wizardPages[$scope.index].WizardInput = 'No';
            $eventAggregatorService.publish($wizardFactory.events.NO_WIZARD_CLICKED, { model: $scope.wizardPages[$scope.index] });
            operations.moveNext();
        },
        next: function () {
            $eventAggregatorService.publish($wizardFactory.events.NEXT_WIZARD_CLICKED, { model: $scope.wizardPages[$scope.index] });
            operations.moveNext();
        },
        prev: function () {
            $eventAggregatorService.publish($wizardFactory.events.PREVIOUS_WIZARD_CLICKED, { model: $scope.wizardPages[$scope.index] });
            operations.movePrev();
        },
        finish: function () {
            $activityManager.startActivityWithKey("loading");
            $eventAggregatorService.publish($wizardFactory.events.FINISH_WIZARD_CLICKED, { model: $scope.wizardPages });
            $wizardFactory.internal.finished().then(function () {
                $wizardFactory.complete();
                $pageFactory.back(true);
                $activityManager.stopActivityWithKey("loading");
            });
        },
        submit: function () {
            $eventAggregatorService.publish($wizardFactory.events.SUBMIT_WIZARD_CLICKED, { model: $scope.wizardPages[$scope.index] });
            operations.moveNext();
        }
    };
    
   

    $scope.cancel = actions.cancel;
    $scope.yes = actions.yes;
    $scope.no = actions.no;
    $scope.next = actions.next;
    $scope.prev = actions.prev;
    $scope.finish = actions.finish;
    $scope.submit = actions.submit;
   

    $scope.canNavigateBack = false;
    $scope.canNavigateFoward = false;

    operations.init();

    $scope.$on('$destroy', function () {
        if (isWorkFlow && enabled && !$wizardFactory.internal.completed()) {
            $workflowActivityManager.cancel(
                $stateParams.tileAction.instanceId,
                $scope.guid,
                $stateParams.tileAction.name,
                false, null, null
            );
        }
    });
}]);
