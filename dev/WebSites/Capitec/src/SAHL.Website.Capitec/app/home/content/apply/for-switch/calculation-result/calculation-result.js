angular.module('capitecApp.home.content.apply.for-switch.calculation-result', [])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-switch.switch.calculation-result', {
        url: '/calculation-result',
        templateUrl: 'calculation-result.tpl.html',
        data: {
            breadcrumb: 'Calculation Results',
            pageHeading: 'you may qualify for a home loan - subject to full credit assessment',
            icon: 'success',
            next: 'home.content.apply.application-precheck-for-switch.switch.client-capture.personal'
        },
        controller: 'CalculationResultCtrl'
    });
}]);