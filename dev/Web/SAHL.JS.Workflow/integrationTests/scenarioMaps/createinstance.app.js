'use strict'

angular.module('scenarioMaps.createInstance', ['sahl.js.workflow.workflowManager', 'sahl.js.core.fluentDecorator', 'mock.workflow.workflowManagerDecorator'])
    .config(['$fluentDecoratorProvider', '$workflowManagerDecorationProvider', function($fluentDecoratorProvider, $workflowManagerDecorationProvider) {
        $fluentDecoratorProvider.decorate('$workflowManager').with($workflowManagerDecorationProvider.decoration);
    }])
    .controller('createInstanceCtrl', ['$rootScope', '$scope', '$workflowManager', function($rootScope, $scope, $workflowManager) {



        $scope.describe = replaceAll('create instance', ' ', '_');
        $scope.it = replaceAll('should create an instance', ' ', '_');
        $scope.testTitle = replaceAll($scope.describe + ' ' + $scope.it, '_', ' ');

        $scope.error = '';
        $scope.success = '';


        $scope.because = function() {
            console.log('because');

            console.log('because', $rootScope);
            var correlationID = guid(),
                activityName = 'Create Case',
                processName = 'CreateInstanceV3',
                workflowName = 'Create Instance V3',
                ignoreWarnings = false,
                ReturnActivityId = null,
                SourceInstanceId = null,
                mapVariables = {
                    'ApplicationKey': '1565621',
                    'Reference': 'Some test case',

                },
                data = null;
            $workflowManager.createInstance(correlationID, activityName, processName, workflowName,
                     ignoreWarnings, ReturnActivityId, SourceInstanceId, mapVariables, data)
                .then(function(data) {
                    console.log('handle', data);
                    $rootScope.testData[$scope.describe] = data;
                    if (!data.data.ReturnData.IsErrorResponse) {
                        $scope.success = data;
                    } else {
                        $scope.error = data;
                    }

                }, function(error) {
                    console.log('error', error);
                    $rootScope.testData[$scope.describe] = data;
                    $scope.error = error;
                });

        };
    }]);
