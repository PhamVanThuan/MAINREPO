'use strict';
angular.module('sahl.ui.halo.views.wizardPages.wizards.thirdParty.invoice.thirdPartyApproveInvoiceWorkflowWizardPageState.tpl.html',[])
.controller('Wizards_ThirdParty_Invoice_ThirdPartyApproveInvoiceWorkflowWizardPageStateCtrl',['$scope','$stateParams','$workflowActivityManager','$wizardFactory','$q','$toastManagerService',
  function($scope, $stateParams, $workflowActivityManager, $wizardFactory, $q, $toastManagerService){
  var mapVariables = {}; 
  mapVariables["ThirdPartyInvoiceKey"] = $stateParams.model.businessContext.BusinessKey.Key.toString();  
  
  function completeApproveInvoice (){
    var deferred = $q.defer();
      $workflowActivityManager.complete($stateParams.tileAction.instanceId, $scope.guid, $stateParams.tileAction.name, false, mapVariables)
      .then(function(message){
        if(message.data.ReturnData){
          if(message.data.ReturnData.IsErrorResponse || (message.data.ReturnData.SystemMessages && message.data.ReturnData.SystemMessages.HasErrors)){
            showFailureToast();
            deferred.reject(message);
          }else{
            showSuccessfulToast().then(deferred.resolve);
          }
        }else{
          deferred.reject(message);
        }
      });
      return deferred.promise;
  }
  
  var showFailureToast = function(){
    $toastManagerService.error({
          title: 'Error',
          text: 'An error approving this invoice has occured. Please try again later.'
    });
  };

  var showSuccessfulToast = function(){
    return $toastManagerService.success({
          title:'Success',
          text: 'The invoice has been successfully approved.'
    }).promise;
  };

  $wizardFactory.onComplete(completeApproveInvoice);

  $scope.$on('$destroy', function () {
  });
}]);