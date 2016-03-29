'use strict';
angular.module('sahl.ui.halo.views.wizardPages.wizards.thirdParty.invoice.thirdPartyAcceptInvoiceWorkflowWizardPageState.tpl.html',[])
.controller('Wizards_ThirdParty_Invoice_ThirdPartyAcceptInvoiceWorkflowWizardPageStateCtrl',['$scope','$stateParams','$workflowActivityManager','$wizardFactory','$q','$toastManagerService',
  function($scope, $stateParams, $workflowActivityManager, $wizardFactory, $q, $toastManagerService){
	var mapVariables = {}; 
	mapVariables["ThirdPartyInvoiceKey"] = $stateParams.model.businessContext.BusinessKey.Key.toString();  
  
  function completeAcceptSetup (){
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
          text: 'An error accepting this invoice has occured. Please try again later.'
    });
  };

  var showSuccessfulToast = function(){
    return $toastManagerService.success({
          title:'Success',
          text: 'The invoice has been successfully accepted.'
    }).promise;
  };

  $wizardFactory.onComplete(completeAcceptSetup);

	$scope.$on('$destroy', function () {
  });
}]);