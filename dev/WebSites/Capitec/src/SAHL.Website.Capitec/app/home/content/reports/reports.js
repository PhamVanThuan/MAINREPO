angular.module('capitecApp.home.reports', [
  'ui.router',
  'sahl.core.app.services',
  'SAHL.Services.Interfaces.DecisionTree.queries',
  'SAHL.Services.Interfaces.DecisionTree.sharedmodels'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.reports', {
        url: '/reports',
        templateUrl: 'reports.tpl.html',
        controller: 'ReportsCtrl',
        data: { title: 'Reports', pageHeading: 'Capitec Reporting' }
    });
}])

.controller('ReportsCtrl', ['$scope', '$state', '$commandManager', '$capitecSharedModels', '$serialization', '$calculatorDataService', '$decisionTreeQueries', '$decisionTreeSharedModels', '$decisionTreeManager', '$templateCache',
  function ReportsController($scope, $state, $commandManager, $capitecSharedModels, $serialization, $calculatorDataService, $decisionTreeQueries, $decisionTreeSharedModels, $decisionTreeManager, $templateCache) {
    $scope.deserializeTest = function () {
        var tempApp = new $capitecSharedModels.NewPurchaseApplication(0, new Date(), new $capitecSharedModels.NewPurchaseLoanDetails(), [new $capitecSharedModels.Applicant()], [new $capitecSharedModels.ApplicantDeclarations()]);
        $calculatorDataService.addData('test', tempApp);
        var tmp = angular.fromJson($calculatorDataService.getDataValue('test'));
        tmp = $serialization.deserialize(tmp);
        var test = 1;
    };

    $scope.dcTest = function () {
        var dtTreeQuery = new $decisionTreeQueries.OriginationNewBusinessSPVDetermination_1Query(false, false, false, 75, false);
        $decisionTreeManager.sendQueryAsync(dtTreeQuery).then(function (result) {
            var x = result.data.ReturnData.Results.$values[0];
        }, function () {
            
        })
    }
}]);