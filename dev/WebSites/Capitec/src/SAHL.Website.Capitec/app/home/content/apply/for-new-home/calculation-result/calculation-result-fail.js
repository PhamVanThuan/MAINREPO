angular.module('capitecApp.home.content.apply.for-new-home.calculation-result-fail', [])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.apply.application-precheck-for-new-home.new-home.calculation-result-fail', {
        url: '/calculation-result-fail',
        templateUrl: 'calculation-result-fail.tpl.html',
        data: { breadcrumb: 'Calculation Results', pageHeading: 'application declined', icon: 'failure' },
        controller: 'CalculationResultFailCtrl'
    });
}]);