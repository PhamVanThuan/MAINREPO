angular.module('capitecApp.home.track.result', [
  'ui.router',  
  'SAHL.Services.Interfaces.CapitecSearch.searchqueries',
  'capitecApp.services'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.track.result', {
        url: '/results',
        templateUrl: 'trackResults.tpl.html',
        controller: 'TrackResultsCtrl',
        data: { title: 'Track Results', pageHeading: 'track application' }
    });
}])

.controller('TrackResultsCtrl', ['$scope', '$state','$templateCache', function TrackResultsController($scope, $state, $templateCache) {
    if ($scope.application) {
        if ($scope.application.ApplicationStatus === 'In Progress') {
            $state.current.data.pageHeading = 'your application is currently at stage ' + $scope.application.ApplicationStage;
        } else if ($scope.application.ApplicationStatus === 'NTU') {
            $state.current.data.pageHeading = 'client not proceeding with application';
        } else if ($scope.application.ApplicationStatus === 'Decline') {
            $state.current.data.pageHeading = 'application has been declined';
        }
    } else {
        $state.current.data.pageHeading = 'no results found';
    }

   $scope.formatDateString = function (inDate) {
        if(!inDate)
            return '';
        if (inDate.length > 10)
            return inDate.substring(0, 10);
        else
            return inDate;
    };

    $scope.back = function () {
        $state.transitionTo($state.$current.parent.name);
    };
}]);