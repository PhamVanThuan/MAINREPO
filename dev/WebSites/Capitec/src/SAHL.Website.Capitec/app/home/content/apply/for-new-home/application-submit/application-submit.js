angular.module('capitecApp.home.content.apply.for-new-home.application-submit', [])

.config(['$stateProvider', function config($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-new-home.new-home.application-submit', {
        url: "/application-submit",
        templateUrl: 'application-submit.tpl.html',
        data: { breadcrumb: 'Client Consent', pageHeading: "Please sign the client consent form",
            next: 'home.content.apply.application-precheck-for-new-home.new-home.application-result',
        	previous: 'home.content.apply.application-precheck-for-new-home.new-home.client-capture.declarations'},
        controller: 'applicationSubmitCtrl'
    });
}])