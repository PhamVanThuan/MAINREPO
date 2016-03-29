angular.module('capitecApp.home.still-looking.calculation-result', [
  'ui.router',
  'capitecApp.controllers'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.still-looking.calculation-result', {
        url: '/calculation-result',
        templateUrl: 'calculation-still-looking-result.tpl.html',
        data: { breadcrumb: 'Calculation Results', pageHeading: 'calculation results - subject to full credit assessment' },
        controller: 'CalculationResultCtrl'
    });
}])

