'use strict'
angular.module('scenarioMaps.completeActivity', ['sahl.js.workflow.workflowManager', 'sahl.js.core.fluentDecorator', 'mock.workflow.workflowManagerDecorator'])
    .config(['$fluentDecoratorProvider', '$workflowManagerDecorationProvider', function($fluentDecoratorProvider, $workflowManagerDecorationProvider) {
        $fluentDecoratorProvider.decorate('$workflowManager').with($workflowManagerDecorationProvider.decoration);
    }])
    .controller('completeActivityCtrl', ['$rootScope', '$scope', '$workflowManager', function($rootScope, $scope, $workflowManager) {

        $scope.describe = replaceAll('complete activity', ' ', '_');
        $scope.it = replaceAll('should complete an activity', ' ', '_');
        $scope.testTitle = replaceAll($scope.describe + ' ' + $scope.it, '_', ' ');


        $scope.error = '';
        $scope.success = '';

        $scope.because = function() {
            console.log('because');

            var instanceId = $rootScope.testData.create_instance.data.ReturnData.InstanceId;
            var correlationID = guid(),
                activityName = 'Move Application',
                processName = 'CreateInstanceV3',
                workflowName = 'Create Instance V3',
                ignoreWarnings = false,
                ReturnActivityId = null,
                SourceInstanceId = null,
                mapVariables = {
                    'ApplicationKey': '1565621',
                    'Reference': 'Some test case',
                    'Move': false

                },
                data = null;
            $workflowManager.completeActivity(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data)
                .then(function(data) {
                    console.log('handle', data);

                    if (!data.data.ReturnData.IsErrorResponse) {
                        $scope.success = data;
                    } else {
                        $scope.error = data;
                    }

                }, function(error) {
                    console.log('error', error);
                    $scope.error = error;
                });

        };
    }]);
