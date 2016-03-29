'use strict';
angular.module('sahl.halo.app.domainDocumentation.start', [
    'sahl.services.config'
])
.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('start.portalPages.apps.domainDocumentation', {
        url: 'domainDocumentation',
        templateUrl: 'app/start/start.tpl.html',
        controller: 'DomainDocumentationCtrl'
    });
}])
.controller('DomainDocumentationCtrl', ['$scope', '$configuration','$sce', function DomainDocumentationController($scope, $configuration, $sce) {
    $scope.domainServices = $configuration.domainServices;
    $scope.currentDomainService = Object.keys($scope.domainServices)[0];
    $scope.viewDomainDocumentation = function (index) {
        $scope.currentDomainService = index;
    }
    $scope.$on('$destroy', function () {
    });
	
	$scope.getUrl = function(value){
		return $sce.trustAsResourceUrl(value);
	};
}]);