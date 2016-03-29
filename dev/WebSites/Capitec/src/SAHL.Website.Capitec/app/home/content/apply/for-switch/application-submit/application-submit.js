angular.module('capitecApp.home.content.apply.for-switch.application-submit', [])

.config(['$stateProvider', function config($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-switch.switch.application-submit', {
        url: "/application-submit",
        templateUrl: 'application-submit.tpl.html',
        data: { breadcrumb: 'Client Consent', pageHeading: "Please sign the client consent form",
            next: 'home.content.apply.application-precheck-for-switch.switch.application-result',
        	previous: 'home.content.apply.application-precheck-for-switch.switch.client-capture.declarations'},
        controller: 'applicationSubmitCtrl'
    });
}])