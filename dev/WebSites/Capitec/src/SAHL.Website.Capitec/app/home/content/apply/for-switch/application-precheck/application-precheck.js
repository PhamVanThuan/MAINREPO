angular.module('capitecApp.home.content.apply.for-switch.application-precheck', [])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-switch', {
        url: '/switch/application-precheck',
        templateUrl: 'application-precheck.tpl.html',
        data: { breadcrumb: 'Minimum Requirements', pageHeading: 'Please answer all questions', next: 'home.content.apply.application-precheck-for-switch.switch', step: 0 },
        controller: 'applicationprecheckCtrl'
    });
}]);