angular.module('capitecApp.home.content.apply.for-switch.application-result', [])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-switch.switch.application-result', {
        url: '/application-result?applicationID',
        templateUrl: 'application-result.tpl.html',
        data: { breadcrumb: 'Application Submitted', pageHeading: 'Your application has been submitted'},
        controller: 'applicationResultCtrl'
    });
}])