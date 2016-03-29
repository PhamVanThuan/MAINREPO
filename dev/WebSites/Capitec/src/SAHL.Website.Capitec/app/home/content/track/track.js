angular.module('capitecApp.home.track', [
  'ui.router',  
  'capitecApp.home.track.result',
  'SAHL.Services.Interfaces.CapitecSearch.searchqueries',
  'capitecApp.services'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content.track', {
        url: '/track',
        templateUrl: 'track.tpl.html',
        controller: 'TrackCtrl',
        data: { title: 'Track', pageHeading: "track your application's progress" }
    });
}])

.controller('TrackCtrl', ['$scope', '$state', '$searchQueryManager', '$capitecSearchSearchQueries', '$activityManager', '$notificationService', '$queryOptionsService', '$templateCache', function TrackController($scope, $state, $searchQueryManager, $capitecSearchSearchQueries, $activityManager, $notificationService, $queryOptionsService, $templateCache) {
    $scope.controller = this;
    $queryOptionsService.setPaginationOptions($scope, 5, 1, 0, 0);

    this.setData = function (data) {
        if(data.data.ReturnData.Results.$values[0]) {
            $scope.application = data.data.ReturnData.Results.$values[0];
            $scope.application.Applicants = angular.fromJson($scope.application.ApplicantsJson).applicants;
            delete $scope.application.ApplicantsJson;
        } else {
            $scope.application = null;
        }
    }
    
    $scope.trackApplication = function () {
        $activityManager.startActivityWithKey('trackApplication');

        if (!this.applicationNumber &&
            !this.identityNumber) {
            $scope.trackApplicationForm.applicationNumber.$setValidity('custom', false);
            $scope.trackApplicationForm.applicationNumber.$setPristine();

            $scope.trackApplicationForm.identityNumber.$setValidity('custom', false);
            $scope.trackApplicationForm.identityNumber.$setPristine();

            $activityManager.stopActivityWithKey('trackApplication');
            $notificationService.notifyError('Invalid search ', 'At least one search criteria is required to search on.');
        }
        else {
            if (this.applicationNumber && !isValidNumber(this.applicationNumber)) {
                $scope.trackApplicationForm.applicationNumber.$setValidity('custom', false);
                $scope.trackApplicationForm.applicationNumber.$setPristine();

                $scope.trackApplicationForm.identityNumber.$setValidity('custom', true);
                $activityManager.stopActivityWithKey('trackApplication');
                $notificationService.notifyError('Invalid search ', 'Application Number must be a number.');
            }
            else if (this.identityNumber && (!isValidNumber(this.identityNumber) || this.identityNumber.length !== 13)) {
                var message;
                if(!isValidNumber(this.identityNumber)) { 
                    message = 'ID Number must be a number.'; 
                }
                else if (this.identityNumber.length !== 13) { 
                    message = 'ID Number must be 13 digits.';
                }
                if(message) {
                    $scope.trackApplicationForm.applicationNumber.$setValidity('custom', true);
                    $scope.trackApplicationForm.identityNumber.$setValidity('custom', false);
                    $scope.trackApplicationForm.identityNumber.$setPristine();

                    $activityManager.stopActivityWithKey('trackApplication');
                    $notificationService.notifyError('Invalid search ', message);
                }
            }           
            else {
                $scope.paginationOptions.currentPage = 1;
                $scope.trackApplicationForm.applicationNumber.$setValidity('custom', true);
                $scope.trackApplicationForm.identityNumber.$setValidity('custom', true);

                var searchQuery = new $capitecSearchSearchQueries.ApplicationStatusQuery($scope.applicationNumber, $scope.identityNumber)

                $searchQueryManager.sendQueryAsync(searchQuery, $scope.paginationOptions, $scope.filterOptions, $scope.sortOptions).then(function (data) {
                    $scope.controller.setData(data);
                    $scope.paginationOptions.totalPages = data.data.ReturnData.NumberOfPages;
                    $scope.paginationOptions.totalResults = data.data.ReturnData.ResultCountInAllPages;
                    $activityManager.stopActivityWithKey('trackApplication');
                    $state.go('home.content.track.result');
                }, function (errorMessage) {
                    $scope.errorMessage = 'search query failure.';
                    $activityManager.stopActivityWithKey('trackApplication');
                });
            }
        }
        
    };

    /** Numeric RegEx to validate only numbers are entered **/
    /** disclaimer : doing only client side validation is not best approach! **/ 
    function isValidNumber(fieldToValidate) {
        return /^[0-9]+$/.test(fieldToValidate);
    };
}]);