'use strict';

angular.module('sahl.ui.halo.views.wizardPages.wizards.thirdParty.invoice.thirdPartyReassignProcessorWorkflowWizardPageState.tpl.html', ['sahl.js.workflow.workflowManager'])
    .controller('Wizards_ThirdParty_Invoice_ThirdPartyReassignProcessorWorkflowWizardPageStateCtrl', ['$scope', '$workflowAssignmentService', '$workflowActivityManager', '$stateParams', '$wizardFactory', '$q', '$toastManagerService', '$pageFactory','$activityManager',
function ($scope, $workflowAssignmentService, $workflowActivityManager, $stateParams, $wizardFactory, $q, $toastManagerService, $pageFactory ,$activityManager) {
            var capabilityKey = 1; //invoice processor
            $scope.curentUserAssignedTo = {};
            $scope.activeUsersWithCapability = [];

            var loadCurentUserAssignedTo = function () {
				//PB
				$activityManager.startActivityWithKey("loading");
                $workflowAssignmentService.getCurrentlyAssignedUserForInstance($stateParams.tileAction.instanceId).then(function (data) {
                    if (data.length) {
                        var currentUser = data[0];
                        $scope.curentUserAssignedTo = currentUser;
                        
                        loadActiveUsersWithCapability();
                    }
				$activityManager.stopActivityWithKey("loading");
                });
            };
            var loadActiveUsersWithCapability = function () {
                var currentUserOGStructureKey = $scope.curentUserAssignedTo.UserOrganisationStructureKey;
                $workflowAssignmentService.getActiveUsersWithCapability(capabilityKey).then(function (data) {
                    var activeUsersWithCapability = _.filter(data, function (user) {
                        return user.UserOrganisationStructureKey !== currentUserOGStructureKey;
                    });

                    $scope.activeUsersWithCapability = activeUsersWithCapability;
					
                });
            }

            loadCurentUserAssignedTo();

            var mapVariables = {};
            mapVariables["ThirdPartyInvoiceKey"] = $stateParams.model.businessContext.BusinessKey.Key.toString();

            function completeReassignProcessor() {
                var deferred = $q.defer();
                var selectedUserOGStuctureKey = $scope.userToReAssignTo;
                if (!selectedUserOGStuctureKey)
                    selectedUserOGStuctureKey = "0";

                $workflowActivityManager.complete($stateParams.tileAction.instanceId, $scope.guid, $stateParams.tileAction.name, false, mapVariables, selectedUserOGStuctureKey)
                    .then(function (message) {
                        if (message.data.ReturnData) {
                            if (message.data.ReturnData.IsErrorResponse || (message.data.ReturnData.SystemMessages && message.data.ReturnData.SystemMessages.HasErrors)) {
                                showFailureToast();
                                deferred.reject(message);
                            } else {
                                showSuccessfulToast().then(deferred.resolve);
                                $wizardFactory.complete();
                                $pageFactory.back(true);
                            }
                        } else {
                            deferred.reject(message);
                        }
                    });
                return deferred.promise;

            }

            var showUserToReassignNotSelectedToast = function () {
                $toastManagerService.error({
                    title: 'Error',
                    text: 'Please select a user to reassign to.'
                });
            };

            var showCannotReassignToSameUserToast = function () {
                $toastManagerService.error({
                    title: 'Error',
                    text: 'You cannot reassign to the same user.'
                });
            };

            var showFailureToast = function () {
                $toastManagerService.error({
                    title: 'Error',
                    text: 'An error reassigning this invoice has occured. Please try again later.'
                });
            };

            var showSuccessfulToast = function () {
                return $toastManagerService.success({
                    title: 'Success',
                    text: 'The invoice has been successfully reassigned.'
                }).promise;
            };

            $wizardFactory.onComplete(completeReassignProcessor);

            $scope.$on('$destroy', function () {});

}]);