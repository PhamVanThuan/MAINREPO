'use strict';
angular.module('sahl.ui.halo.views.wizardPages.wizards.thirdParty.invoice.thirdPartyEscalateInvoiceWorkflowWizardPageState.tpl.html', [])
    .controller('Wizards_ThirdParty_Invoice_ThirdPartyEscalateInvoiceWorkflowWizardPageStateCtrl', ['$scope', '$stateParams', '$workflowActivityManager', '$wizardFactory', '$q', '$toastManagerService', '$thirdPartyInvoiceManagerService', ' $pageFactory',
  function ($scope, $stateParams, $workflowActivityManager, $wizardFactory, $q, $toastManagerService, $thirdPartyInvoiceManagerService, $pageFactory) {
            var mapVariables = {};
            var data = '';

            function initialiseController() {
                populateUserList();
                $scope.selectedItem = 0;
                mapVariables["ThirdPartyInvoiceKey"] = $stateParams.model.businessContext.BusinessKey.Key.toString();
            };

            function populateUserList() {
                var thirdPartyInvoiceKey = $stateParams.model.businessContext.BusinessKey.Key.toString();
                $thirdPartyInvoiceManagerService.getMandatedUserListForEscalation(thirdPartyInvoiceKey)
                    .then(function (data) {
                        $scope.mandatedUserList = data.results;
                    }, errorRetrievingData);
            };

            $scope.setSelectedUser = function (userOrgStructureKey) {
                data = userOrgStructureKey;
            };


            function escalateInvoiceToSelectedUser() {
                if(!data){
                    data = "0";
                }
                var deferred = $q.defer();
                $workflowActivityManager.complete($stateParams.tileAction.instanceId, $scope.guid, $stateParams.tileAction.name, false, mapVariables, data)
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

            var showUserNotSelectedToast = function () {
                $toastManagerService.error({
                    title: 'Error',
                    text: 'Please select a valid user to escalate the invoice to.'
                });
            };

            var showFailureToast = function () {
                $toastManagerService.error({
                    title: 'Error',
                    text: 'An error escalating this invoice has occured. Please try again later.'
                });
            };

            var showSuccessfulToast = function () {
                return $toastManagerService.success({
                    title: 'Success',
                    text: 'The invoice has been successfully escalated to the selected user.'
                }).promise;
            };

            var errorRetrievingData = function () {
                $toastManagerService.error({
                    title: 'Error',
                    text: 'An error occured retrieving the user options to select from.  Please try again later.'
                });
            };

            $wizardFactory.onComplete(escalateInvoiceToSelectedUser);
            
            initialiseController();
}]);