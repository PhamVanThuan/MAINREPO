describe("CapitecApp", function () {
    beforeEach(module('capitecApp'));
    beforeEach(module('templates'));

    describe(" - (TrackController) - ", function () {
        var $httpBackend, $rootScope, $_q, $state, $searchQueryManager, $activityManager, createController, $httpInterceptor, ApplicationStatusResponse, $capitecSearchSearchQueries, $notificationService, $queryOptionsService,$compile;
        
        beforeEach(inject(function ($injector, $q) {
            // Setup the root scope
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $state = $injector.get('$state');
            $searchQueryManager = $injector.get('$searchQueryManager');
            $activityManager = $injector.get('$activityManager');
            $httpBackend = $injector.get('$httpBackend');
            $httpInterceptor = $injector.get('$httpInterceptor');
            $capitecSearchSearchQueries = $injector.get('$capitecSearchSearchQueries');
            $notificationService = $injector.get('$notificationService');
            $queryOptionsService = $injector.get('$queryOptionsService');
            $compile = $injector.get('$compile');

            $rootScope.authenticated = true;

            // Set up the controller under test
            var $controller = $injector.get('$controller');
            createController = function () {
                return $controller('TrackCtrl', {
                    '$scope': $scope,
                    '$state': $state,
                    '$searchQueryManager': $searchQueryManager,
                    '$capitecSearchSearchQueries': $capitecSearchSearchQueries,
                    '$activityManager': $activityManager,
                    '$notificationService': $notificationService, 
                    '$queryOptionsService': $queryOptionsService
                });
            };
        }));

        describe('when neither identification number nor application number are set', function () {
            beforeEach(function () {
                var trackController = createController();
                $scope.applicationNumber = undefined;
                $scope.identityNumber = undefined;
                // compiling to mimic elemet binding to $scope.trackApplicationForm and $scope.trackApplicationForm.applicationNumber objects
                $compile('<form name="trackApplicationForm"><input type="text" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.applicationNumber, '$setPristine');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setPristine');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($notificationService, 'notifyError');
                $scope.trackApplication();
                $scope.$digest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', false)
            });
            it('should set application number field to prestine', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setPristine).toHaveBeenCalledWith();
            });
            it('should set identification number field to invalid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', false);
            });
            it('should set identification number field to prestine', function () {
                expect($scope.trackApplicationForm.identityNumber.$setPristine).toHaveBeenCalledWith();
            });
            it('should stop activity with key', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should inform the user that at least one search criteria is required to search on', function () {
                expect($notificationService.notifyError).toHaveBeenCalledWith('Invalid search ', 'At least one search criteria is required to search on.');
            });
        });

        describe('when an invalid application number and an undefined identity number are submitted', function () {
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = 'abcd';
                $scope.identityNumber = undefined;
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.applicationNumber, '$setPristine');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setPristine');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($notificationService, 'notifyError');
                $scope.trackApplication();
                $scope.$digest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', false)
            });
            it('should set application number field to prestine', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setPristine).toHaveBeenCalledWith();
            });
            it('should set identification number field to valid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', true);
            });
            it('should stop activity with key', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should inform the user that at the application number must be a number', function () {
                expect($notificationService.notifyError).toHaveBeenCalledWith('Invalid search ', 'Application Number must be a number.');
            });
        });

        describe('when an invalid application number and a valid identity number are submitted', function () {
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = 'abcd';
                $scope.identityNumber = '1234567890123';
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.applicationNumber, '$setPristine');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setPristine');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($notificationService, 'notifyError');
                $scope.trackApplication();
                $scope.$digest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', false)
            });
            it('should set application number field to prestine', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setPristine).toHaveBeenCalledWith();
            });
            it('should set identification number field to valid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', true);
            });
            it('should stop activity with key', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should inform the user that at the application number must be a number', function () {
                expect($notificationService.notifyError).toHaveBeenCalledWith('Invalid search ', 'Application Number must be a number.');
            });
        });

        describe('when an undefined application number and an invalid identity number are submitted', function () {
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = undefined;
                $scope.identityNumber = 'abcd';
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.applicationNumber, '$setPristine');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setPristine');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($notificationService, 'notifyError');
                $scope.trackApplication();
                $scope.$digest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', true)
            });
            it('should set identification number field to invalid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', false);
            });
            it('should set identification number field to prestine', function () {
                expect($scope.trackApplicationForm.identityNumber.$setPristine).toHaveBeenCalledWith();
            });
            it('should stop activity with key', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should inform the user that the identity number must be a number', function () {
                expect($notificationService.notifyError).toHaveBeenCalledWith('Invalid search ', 'ID Number must be a number.');
            });
        });

        describe('when a valid application number and an invalid identity number are submitted', function () {
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = '1234';
                $scope.identityNumber = 'abcd';
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.applicationNumber, '$setPristine');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setPristine');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($notificationService, 'notifyError');
                $scope.trackApplication();
                $scope.$digest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', true)
            });
            it('should set identification number field to invalid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', false);
            });
            it('should set identification number field to prestine', function () {
                expect($scope.trackApplicationForm.identityNumber.$setPristine).toHaveBeenCalledWith();
            });
            it('should stop activity with key', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should inform the user that the identity number must be a number', function () {
                expect($notificationService.notifyError).toHaveBeenCalledWith('Invalid search ', 'ID Number must be a number.');
            });
        });

        describe('when a valid application number and an identity number with fewer than 13 digits are submitted', function () {
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = '1234';
                $scope.identityNumber = '1234';
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.applicationNumber, '$setPristine');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setPristine');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($notificationService, 'notifyError');
                $scope.trackApplication();
                $scope.$digest();
            });
            it('should set application number field to valid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', true)
            });
            it('should set identification number field to invalid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', false);
            });
            it('should set identification number field to prestine', function () {
                expect($scope.trackApplicationForm.identityNumber.$setPristine).toHaveBeenCalledWith();
            });
            it('should stop activity with key', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should inform the user that the identity number must be 13 digits', function () {
                expect($notificationService.notifyError).toHaveBeenCalledWith('Invalid search ', 'ID Number must be 13 digits.');
            });
        });

        describe('when a valid application number and an undefined identity number are submitted', function () {
            var getApplicationStatusQueryResponse
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = '1234';
                $scope.identityNumber = undefined;
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($scope.controller, 'setData');
                spyOn($state, 'go');
                spyOn($searchQueryManager, 'sendQueryAsync').and.callThrough();
                getApplicationStatusQueryResponse = capitecSearchsearchQueryFakes.ApplicationStatusQueryResult();
                spyOn($capitecSearchSearchQueries, 'ApplicationStatusQuery').and.returnValue({ fake: 'ApplicationStatusQuery' });
                $httpBackend.whenPOST("http://localhost/CapitecSearchService/api/QueryHttpHandler/PerformHttpQuery?pageSize=5&currentPage=1").respond(200, getApplicationStatusQueryResponse);
                $scope.trackApplication();
                $scope.$digest();
                $httpBackend.flush();
            });
            afterEach(function () {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', true)
            });
            it('should set identification number field to valid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', true);
            });
            it('should get application status query', function () {
                expect($capitecSearchSearchQueries.ApplicationStatusQuery).toHaveBeenCalledWith('1234', undefined)
            });
            it('should send query async', function () {
                expect($searchQueryManager.sendQueryAsync).toHaveBeenCalledWith({ fake: 'ApplicationStatusQuery' }, $scope.paginationOptions, undefined, undefined);
            });
            it('should set data', function () {
                expect($scope.controller.setData).toHaveBeenCalled();
            });
            it('should set totalPages', function () {
                expect($scope.paginationOptions.totalPages).toEqual(1);
            });
            it('should set totalResults', function () {
                expect($scope.paginationOptions.totalResults).toEqual(1);
            });
            it('should stop activity with key trackApplication', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should go to home.content.track.result', function () {
                expect($state.go).toHaveBeenCalledWith('home.content.track.result');
            });
        });

        describe('when a an undefined application number and a valid identification number are submitted', function () {
            var getApplicationStatusQueryResponse
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = undefined;
                $scope.identityNumber = '1234567890123';
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($scope.controller, 'setData');
                spyOn($state, 'go');
                spyOn($searchQueryManager, 'sendQueryAsync').and.callThrough();
                getApplicationStatusQueryResponse = capitecSearchsearchQueryFakes.ApplicationStatusQueryResult();
                spyOn($capitecSearchSearchQueries, 'ApplicationStatusQuery').and.returnValue({ fake: 'ApplicationStatusQuery' });
                $httpBackend.whenPOST("http://localhost/CapitecSearchService/api/QueryHttpHandler/PerformHttpQuery?pageSize=5&currentPage=1").respond(200, getApplicationStatusQueryResponse);
                $scope.trackApplication();
                $scope.$digest();
                $httpBackend.flush();
            });
            afterEach(function () {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', true)
            });
            it('should set identification number field to valid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', true);
            });
            it('should get application status query', function () {
                expect($capitecSearchSearchQueries.ApplicationStatusQuery).toHaveBeenCalledWith(undefined, '1234567890123');
            });
            it('should send query async', function () {
                expect($searchQueryManager.sendQueryAsync).toHaveBeenCalledWith({ fake: 'ApplicationStatusQuery' }, $scope.paginationOptions, undefined, undefined);
            });
            it('should set data', function () {
                expect($scope.controller.setData).toHaveBeenCalled();
            });
            it('should set totalPages', function () {
                expect($scope.paginationOptions.totalPages).toEqual(1);
            });
            it('should set totalResults', function () {
                expect($scope.paginationOptions.totalResults).toEqual(1);
            });
            it('should stop activity with key trackApplication', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should go to home.content.track.result', function () {
                expect($state.go).toHaveBeenCalledWith('home.content.track.result');
            });
        });

        describe('when a valid application number and identification number are submitted', function () {
            var getApplicationStatusQueryResponse
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = '1234';
                $scope.identityNumber = '1234567890123';
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($scope.controller, 'setData');
                spyOn($state, 'go');
                spyOn($searchQueryManager, 'sendQueryAsync').and.callThrough();
                getApplicationStatusQueryResponse = capitecSearchsearchQueryFakes.ApplicationStatusQueryResult();
                spyOn($capitecSearchSearchQueries, 'ApplicationStatusQuery').and.returnValue({ fake: 'ApplicationStatusQuery' });
                $httpBackend.whenPOST("http://localhost/CapitecSearchService/api/QueryHttpHandler/PerformHttpQuery?pageSize=5&currentPage=1").respond(200, getApplicationStatusQueryResponse);
                $scope.trackApplication();
                $scope.$digest();
                $httpBackend.flush();
            });
            afterEach(function () {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });
            it('should set application number field to invalid', function () {
                expect($scope.trackApplicationForm.applicationNumber.$setValidity).toHaveBeenCalledWith('custom', true)
            });
            it('should set identification number field to valid', function () {
                expect($scope.trackApplicationForm.identityNumber.$setValidity).toHaveBeenCalledWith('custom', true);
            });
            it('should get application status query', function () {
                expect($capitecSearchSearchQueries.ApplicationStatusQuery).toHaveBeenCalledWith('1234', '1234567890123');
            });
            it('should send query async', function () {
                expect($searchQueryManager.sendQueryAsync).toHaveBeenCalledWith({ fake: 'ApplicationStatusQuery' }, $scope.paginationOptions, undefined, undefined);
            });
            it('should set data', function () {
                expect($scope.controller.setData).toHaveBeenCalled();
            });
            it('should set totalPages', function () {
                expect($scope.paginationOptions.totalPages).toEqual(1);
            });
            it('should set totalResults', function () {
                expect($scope.paginationOptions.totalResults).toEqual(1);
            });
            it('should stop activity with key trackApplication', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
            it('should go to home.content.track.result', function () {
                expect($state.go).toHaveBeenCalledWith('home.content.track.result');
            });
        });

        describe('when send query async promise is rejected', function () {
            var getApplicationStatusQueryResponse
            beforeEach(function () {
                var trackController = createController();
                $compile('<form name="trackApplicationForm"><input type="text" id="applicationNumber" name="applicationNumber" ng-model="applicationNumber"/><input type="text" name="identityNumber" ng-model="identityNumber" /></form>')($scope);
                $scope.applicationNumber = '1234';
                $scope.identityNumber = '1234567890123';
                spyOn($scope.trackApplicationForm.applicationNumber, '$setValidity');
                spyOn($scope.trackApplicationForm.identityNumber, '$setValidity');
                spyOn($activityManager, 'stopActivityWithKey');
                spyOn($scope.controller, 'setData');
                spyOn($state, 'go');
                spyOn($searchQueryManager, 'sendQueryAsync').and.callThrough();
                getApplicationStatusQueryResponse = capitecSearchsearchQueryFakes.ApplicationStatusQueryResult();
                getApplicationStatusQueryResponse.SystemMessages.AddError('Test Error');
                spyOn($capitecSearchSearchQueries, 'ApplicationStatusQuery').and.returnValue({ fake: 'ApplicationStatusQuery' });
                //http response code 400 will cause the promise to be rejected
                $httpBackend.whenPOST("http://localhost/CapitecSearchService/api/QueryHttpHandler/PerformHttpQuery?pageSize=5&currentPage=1").respond(400, getApplicationStatusQueryResponse);
                $scope.trackApplication();
                $scope.$digest();
                $httpBackend.flush();
            });
            afterEach(function () {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });
            it('should not set data', function () {
                expect($scope.controller.setData.calls.count()).toEqual(0);
            });
            it('should set error message', function () {
                expect($scope.errorMessage).toBe('search query failure.');
            });
            it('should stop activity with key trackApplication', function () {
                expect($activityManager.stopActivityWithKey).toHaveBeenCalledWith('trackApplication');
            });
        });

        describe('when setting data', function () {
            var _data;
            var _resultLine1;
            var _resultLine2;
            var _resultLine3;
            beforeEach(function () {
                var trackController = createController();
                _resultLine1 = {
                    DocumentId: 'documentId1',
                    Score: 'score1',
                    ApplicationNumber: 'applicationNumber1',
                    ApplicationDate: 'applicationDate1',
                    ApplicationStage: 'applicationStage1',
                    ApplicationStatus: 'applicationStatus1',
                    ApplicantsJson: { applicants: { fake: 'applicants1' } },
                    ConsultantName: 'consultantName1',
                    ConsultantContactNumber: 'consultantContactNumber1'
                }
                _data = { data: capitecSearchsearchQueryFakes.ApplicationStatusQueryResult() };
                _data.data.Add(_resultLine1.DocumentId,
                    _resultLine1.Score,
                    _resultLine1.ApplicationNumber,
                    _resultLine1.ApplicationDate,
                    _resultLine1.ApplicationStage,
                    _resultLine1.ApplicationStatus,
                    _resultLine1.ApplicantsJson,
                    _resultLine1.ConsultantName,
                    _resultLine1.ConsultantContactNumber);
                var fakeCall = 0
                spyOn(angular, 'fromJson').and.callFake(function () {
                    return _data.data.ReturnData.Results.$values[fakeCall++].ApplicantsJson;
                });
                trackController.setData(_data);
                $scope.$digest();
            });
            it('should set the application', function () {
                expect($scope.application).toBeDefined();
            });
            it('should convert applicants json', function () {
                expect(angular.fromJson.calls.count()).toEqual(1);
            });
            it('should set the applicants', function () {
                    expect($scope.application.Applicants.length).toBeDefined;
            });
            it('should delete applications applicants json', function () {
                    expect($scope.application.ApplicantsJson).toBeUndefined;
            });
        });
    });
});