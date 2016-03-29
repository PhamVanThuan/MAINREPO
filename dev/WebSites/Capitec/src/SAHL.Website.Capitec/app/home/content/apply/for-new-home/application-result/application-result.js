angular.module('capitecApp.home.content.apply.for-new-home.application-result', [])

.config(['$stateProvider', function config($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-new-home.new-home.application-result', {
        url: "/application-result?applicationID",
        templateUrl: 'application-result.tpl.html',
        data: { breadcrumb: 'Application Submitted'},
        controller: 'applicationResultCtrl'
    });
}])