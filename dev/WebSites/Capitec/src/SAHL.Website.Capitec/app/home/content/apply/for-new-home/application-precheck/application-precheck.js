angular.module('capitecApp.home.content.apply.for-new-home.application-precheck', [])

.config(['$stateProvider', function config($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-new-home', {
        url: '/new-home/application-precheck',
        templateUrl: 'application-precheck.tpl.html',
        data: { breadcrumb: 'Minimum Requirements', pageHeading: 'Please answer all questions', next: 'home.content.apply.application-precheck-for-new-home.new-home', step: 0 },
        controller: 'applicationprecheckCtrl'
    });
}]);