'use strict';

angular.module('halo.core', [
        'ui.router',
        'ngAnimate',
        'halo.startables',
        'halo.start',
        'halo.menu',
        'halo.charms.userCharms',
        'halo.error',
        'halo.logging',
        'halo.userProfile',
        'sahl.js.core.messaging',
        'sahl.js.core.logging',
        'sahl.js.core.activityManagement',
        'sahl.js.core.applicationManagement',
        'sahl.js.core.userManagement',
        'sahl.js.core.tileManager',
        'sahl.js.core.serviceManagement',
        'sahl.js.core.fluentDecorator',
        'sahl.js.core.eventAggregation',
        'sahl.js.core.userProfile',
        'sahl.js.core.routing',
        'sahl.js.core.transactionQueryService',
        'sahl.js.ui.activityManagement',
        'sahl.js.ui.filters',
        'sahl.services.config',
        'sahl.websites.halo.events',
        'sahl.websites.halo.services.haloService',
        'sahl.websites.halo.services.transactionQueryService',
        'truncate',
        'angularMoment',
        'ui.grid',
        'ui.grid.pagination',
        'ui.grid.autoResize',
        'ui.grid.edit',
        'ui.grid.selection',
        'sahl.js.ui.forms.lookup',
        'sahl.js.core.lookup',
        'sahl.websites.halo.services.lookup',
        'sahl.js.workflow.workflowManager',
        'sahl.js.core.workflowEngineManagement',
        'sahl.js.core.primitives',
        'sahl.websites.halo.services.thirdPartyInvoiceManager',
        'sahl.websites.halo.services.accountsManager',
        'sahl.websites.halo.services.documentDownloadManager',
        'sahl.websites.halo.services.attorneysManager',
        'sahl.websites.halo.services.thirdPartyManager',
        'sahl.websites.halo.services.acronymManager',
        '720kb.datepicker',
        'monospaced.elastic',
        'sahl.ui.halo.views.tiles.thirdparty.invoices',
        'sahl.js.ui.modalManager',
        'sahl.websites.halo.services.treasuryManager',

        'sahl.websites.halo.services.documentService',
        'sahl.js.core.documentManagement',
        'sahl.js.core.workflowAssignmentManagement'
])
    .config(['$urlRouterProvider',
             '$locationProvider',
             '$fluentDecoratorProvider',
             '$activityDecoratorProvider',
             '$haloStartablesProvider',
             '$loggingProvider',
             '$globalLoggerProvider',
             '$haloServiceProvider',
             '$uiStateDecoratorProvider',
             '$userProfileProvider',
             '$toastManagerServiceProvider',
             '$lookupProvider',
             '$httpProvider',
             '$workflowManagerDecorationProvider',
             '$transactionQueryDecorationProvider',
             '$documentServiceDecorationProvider',
             '$workflowAssignmentManagerDecorationProvider',
        function ($urlRouterProvider, $locationProvider, $fluentDecoratorProvider,
                  $activityDecoratorProvider, $haloStartablesProvider, $loggingProvider,
                  $globalLoggerProvider, $haloServiceProvider, $uiStateDecoratorProvider,
                  $userProfileProvider, $toastManagerServiceProvider, $lookupProvider,
                  $httpProvider, $workflowManagerDecorationProvider, $transactionQueryDecorationProvider,
                  $documentServiceDecorationProvider, $workflowAssignmentManagerDecorationProvider) {
            $urlRouterProvider.otherwise('/');
            $locationProvider.html5Mode(true);

            $fluentDecoratorProvider.decorate('$messageManager').with($activityDecoratorProvider.decoration);
            $fluentDecoratorProvider.decorate('$startableManagerService').with($haloStartablesProvider.decoration);
            $fluentDecoratorProvider.decorate('$loggingService').with($loggingProvider.decoration);
            $fluentDecoratorProvider.decorate('$tileManagerConfigurationService').with($haloServiceProvider.decoration);
            $fluentDecoratorProvider.decorate('$transactionQueryService').with($transactionQueryDecorationProvider.decoration);
            $fluentDecoratorProvider.decorate('$state').with($uiStateDecoratorProvider.decoration);
            $fluentDecoratorProvider.decorate('$userProfileService').with($userProfileProvider.decoration);
            $fluentDecoratorProvider.decorate('$lookupService').with($lookupProvider.decoration);
            $fluentDecoratorProvider.decorate('$workflowService').with($workflowManagerDecorationProvider.decoration);
            $fluentDecoratorProvider.decorate('$documentService').with($documentServiceDecorationProvider.decoration);
            $fluentDecoratorProvider.decorate('$workflowAssignmentService').with($workflowAssignmentManagerDecorationProvider.decoration);
            
            $httpProvider.interceptors.push('$authInterceptor');

            $toastManagerServiceProvider.configure({
                styling: 'fontawesome',
                animate_speed: 'fast',
                delay: 3000,
                stack: { "dir1": "up", "dir2": "left", "firstpos1": 25, "firstpos2": 25 },
                addclass: "stack-bottomright",
                mousereset: false
            });
        }
    ])
    .controller('AppCtrl', ['$rootScope', '$state', '$eventAggregatorService', '$applicationManagerService', '$userManagerService',
function ($rootScope, $state, $eventAggregatorService, $applicationManager, $userManagerService) {
    var eventHandlers = {
        onStarting: function () {
            $rootScope.application.state = "Starting";
        },
        onStarted: function () {
            $rootScope.application.state = "Started";
        },
        onFailed: function () {
            $rootScope.application.state = "Failed";
            $userManagerService.getAuthenticatedUser();
        },
        onUnload: function () {
            $eventAggregatorService.unsubscribe($applicationManager.applicationEvents.applicationStarting, eventHandlers.onStarting);
            $eventAggregatorService.unsubscribe($applicationManager.applicationEvents.applicationStarted, eventHandlers.onStarted);
            $eventAggregatorService.unsubscribe($applicationManager.applicationEvents.applicationFailed, eventHandlers.onFailed);
        }
    };
    // subscriptions
    $eventAggregatorService.subscribe($applicationManager.applicationEvents.applicationStarting, eventHandlers.onStarting);
    $eventAggregatorService.subscribe($applicationManager.applicationEvents.applicationStarted, eventHandlers.onStarted);
    $eventAggregatorService.subscribe($applicationManager.applicationEvents.applicationFailed, eventHandlers.onFailed);

    $rootScope.$on('$destroy', eventHandlers.onUnload);
    $applicationManager.startApp().then(function () {
        $userManagerService.getUserProfile().then(function (data) {
            var landingPage = data.defaultLandingPage;
            var params = data.params;
            if (_.isEmpty(landingPage)) {
                landingPage = "start.portalPages.search";
            }
            $state.go(landingPage, params);
        });
    });
}])
.run(['$rootScope', '$state', '$applicationManagerService', '$toastManagerService', '$logger', '$http',
    function ($rootScope, $state, $applicationManager, $toastManagerService, $logger, $http) {
        $rootScope.$on('$stateChangeSuccess', function (event, toState) {
            $logger.logInfo('State Change: State change success [' + toState.templateUrl + ']');
        });

        $rootScope.$on('$stateChangeError', function () {
            $logger.logError("State Change: Error!");
        });

        $rootScope.$on('$stateNotFound', function (event, toState) {
            $logger.logError("Error: invalid config - State Change: State not found!" + toState.to);
            event.preventDefault();
        });

        $rootScope.$on('$viewContentLoading', function () {
            $logger.logInfo("View Load: the view is loaded, and DOM rendered!");
        });

        $rootScope.$on('$viewcontentLoaded', function () {
            $logger.logInfo("View Load: the view is loaded, and DOM rendered!");
        });

        $rootScope.$on('$stateChangeStart', function (evt, toState) {
            $toastManagerService.closeAll();
            var appState = $applicationManager.getCurrentState();
            if (toState.name !== 'applicationError' &&
                (appState === $applicationManager.applicationStates.FAILED || appState === $applicationManager.applicationStates.NONE)) {
                evt.preventDefault();
                $state.transitionTo('applicationError');
            }
        });

        $rootScope.application = {
            state: 'none'
        };

        $http.defaults.withCredentials = true;

        
    }
]);
