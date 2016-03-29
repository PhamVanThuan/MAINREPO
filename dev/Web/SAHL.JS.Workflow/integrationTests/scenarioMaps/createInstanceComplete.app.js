angular.module('scenarioMaps.completeCreateInstanceActivity', ['sahl.js.workflow.workflowManager', 'sahl.js.core.fluentDecorator', 'mock.workflow.workflowManagerDecorator'])
    .config(['$fluentDecoratorProvider', '$workflowManagerDecorationProvider', function($fluentDecoratorProvider, $workflowManagerDecorationProvider) {
        $fluentDecoratorProvider.decorate('$workflowManager').with($workflowManagerDecorationProvider.decoration);
    }])
    .controller('completeCreateInstanceActivityCtrl', ['$rootScope', '$scope', '$workflowManager', function($rootScope, $scope, $workflowManager) {

        $scope.describe = replaceAll('complete create instance activity', ' ', '_');
        $scope.it = replaceAll('should complete a create instance activity and move the case to Application Created', ' ', '_');
        $scope.testTitle = replaceAll($scope.describe + ' ' + $scope.it, '_', ' ');

        $scope.error = '';
        $scope.success = '';

        $scope.because = function() {
            console.log('because');

            var instanceId = $rootScope.testData.create_instance.data.ReturnData.InstanceId;
            var correlationID = guid(),
                activityName = 'Create Case',
                ignoreWarnings = false,
                mapVariables = {
                    'ApplicationKey': '1565621',
                    'Reference': 'Some test case'
                },
                data = null;
            $workflowManager.completeCreateInstanceRequest(instanceId, correlationID, activityName, ignoreWarnings, mapVariables, data)
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
