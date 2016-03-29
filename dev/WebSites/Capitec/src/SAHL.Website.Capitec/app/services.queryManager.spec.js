describe("capitecApp", function () {
    beforeEach(module('capitecApp.services'));
    beforeEach(module('SAHL.Services.Interfaces.Capitec.queries'));

    describe(' Capitec Query Manager Service ', function () {
        var $queryManager, $activityManager, $http,$deferred,$httpBackend;
      
        beforeEach(inject(function ($injector, $q) {                  
            $activityManager = $injector.get('$activityManager');
            $deferred = $q.defer();
            $http = $injector.get('$http');
            $httpBackend = $injector.get('$httpBackend');
            $queryManager = $injector.get('$queryManager');
            $capitecQueries = $injector.get('$capitecQueries');
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();
            $validationManager = $injector.get('$validationManager');
        }));

        describe('when running a query', function() {
            var queryToSend, paginationOptions, filterOptions, sortOptions;

            beforeEach(function() {
                    
                paginationOptions = {
                    'pageSize' : 10,
                    'currentPage' : 1
                };
                filterOptions = {
                    'filterDescription': '',
                    'filterOn': '',
                    'filterValue': ''
                };
                sortOptions = {
                    'orderBy': 'BranchName',
                    'sortDirection': 'ascending'
                };

                spyOn($activityManager, 'startActivity').and.callFake(function() {});
                spyOn($activityManager, 'stopActivity').and.callFake(function() {});
                spyOn($activityManager, 'clearRunningKeyedActivities').and.callFake(function() {});
                queryToSend = new $capitecQueries.GetCalculatorFeeQuery(1, 900000.00, 0);
            });

            afterEach(function() {
                $httpBackend.verifyNoOutstandingExpectation();
                $httpBackend.verifyNoOutstandingRequest();
            });

            it('it should set a queryString if parameter pageSize is not in the URL', function() {
                 spyOn($validationManager, 'Validate').and.returnValue(true);
                $httpBackend.whenPOST("http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?filterOn=&filterValue=&orderBy=BranchName&sortDirection=ascending").respond(200);                                                                                                            

                $queryManager.sendQueryAsync(queryToSend, undefined, filterOptions, sortOptions);
                $httpBackend.expect('POST','http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?filterOn=&filterValue=&orderBy=BranchName&sortDirection=ascending');
                $httpBackend.flush();
            });

            it('it should set a queryString if the filterOn parameter is not in the URL', function() {
                spyOn($validationManager, 'Validate').and.returnValue(true);
                $httpBackend.whenPOST("http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?orderBy=BranchName&sortDirection=ascending").respond(200);
                $queryManager.sendQueryAsync(queryToSend, undefined, undefined, sortOptions);
                $httpBackend.expect('POST','http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?orderBy=BranchName&sortDirection=ascending');
                $httpBackend.flush();
            });

            it('should call stopActivity() if the HTTP post fails', function() {
                spyOn($validationManager, 'Validate').and.returnValue(true);
                $httpBackend.whenPOST("http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?pageSize=10&currentPage=1&filterOn=&filterValue=&orderBy=BranchName&sortDirection=ascending").respond(500);
                $queryManager.sendQueryAsync(queryToSend, paginationOptions, filterOptions, sortOptions);
                $httpBackend.expect('POST','http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?pageSize=10&currentPage=1&filterOn=&filterValue=&orderBy=BranchName&sortDirection=ascending');
                $httpBackend.flush();
                expect($activityManager.stopActivity).toHaveBeenCalled();
            });

            it('should call clearRunningKeyedActivities() if the HTTP post fails', function() {
                spyOn($validationManager, 'Validate').and.returnValue(true);
                $httpBackend.whenPOST("http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?pageSize=10&currentPage=1&filterOn=&filterValue=&orderBy=BranchName&sortDirection=ascending").respond(500);
                $queryManager.sendQueryAsync(queryToSend, paginationOptions, filterOptions, sortOptions);
                $httpBackend.expect('POST','http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery?pageSize=10&currentPage=1&filterOn=&filterValue=&orderBy=BranchName&sortDirection=ascending');
                $httpBackend.flush();
                expect($activityManager.clearRunningKeyedActivities).toHaveBeenCalled();
            });

            it('should call stopActivity() if the validation fails for the query', function() {
                spyOn($validationManager, 'Validate').and.returnValue(false);
                $queryManager.sendQueryAsync(queryToSend, paginationOptions, filterOptions, sortOptions);
                expect($activityManager.stopActivity).toHaveBeenCalled();
            });
                      
        });
    });
});