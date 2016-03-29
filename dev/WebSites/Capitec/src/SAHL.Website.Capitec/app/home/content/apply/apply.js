angular.module('capitecApp.home.apply', [
  'ui.router',
  'capitecApp.controllers',  
  'SAHL.Services.Interfaces.Capitec.commands',
  'SAHL.Services.Interfaces.Capitec.queries',
  'capitecApp.services',
  'capitecApp.home.content.apply.for-new-home',
  'capitecApp.home.content.apply.for-switch',
  'capitecApp.home.content.apply.for-new-home.calculation-result',
  'capitecApp.home.content.apply.for-new-home.calculation-result-fail',
  'capitecApp.home.content.apply.for-new-home.application-precheck',
  'capitecApp.home.content.apply.for-new-home.application-submit',
  'capitecApp.home.content.apply.for-switch.calculation-result',
  'capitecApp.home.content.apply.for-switch.calculation-result-fail',
  'capitecApp.home.content.apply.for-switch.application-precheck',
  'capitecApp.home.content.apply.for-switch.application-submit',
  'capitecApp.home.apply.application-precheck',
  'capitecApp.home.apply.application-result',
  'capitecApp.home.apply.application-submit'
])
//.config(['$stateProvider',function ($stateProvider) {
//    $stateProvider.state('home.content.apply', {
//        url: '/apply',
//        templateUrl: './app/home/content/apply/application-precheck/application-precheck.tpl.html',
//        controller: 'applicationprecheckCtrl',
//        data: { title: 'Apply', pageHeading: 'please answer all questions' }
//    });
//})
.config(['$stateProvider', function config($stateProvider) {
    $stateProvider.state('home.content.apply', {
        url: '/apply',
        templateUrl: 'apply.tpl.html',
        controller: 'ApplyCtrl',
        data: { title: 'Apply', pageHeading: 'choose the type of home loan you need' },
    });
}])
.controller('ApplyCtrl', ['$rootScope', '$scope', '$state', '$commandManager', '$capitecCommands', '$queryManager', '$capitecQueries', '$notificationService', '$templateCache',
    function ApplyController($rootScope, $scope, $state, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService, $templateCache) {
    }]);