angular.module('capitecApp.home.apply.application-precheck', [])

.controller('applicationprecheckCtrl', ['$rootScope', '$scope', '$state', '$commandManager', '$capitecCommands', '$queryManager', '$capitecQueries', '$notificationService', '$capitecSharedModels', '$validationManager', '$calculatorDataService', '$validation', '$activityManager', '$templateCache',
 function ApplicationPrecheckController($rootScope, $scope, $state, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService, $capitecSharedModels, $validationManager, $calculatorDataService, $validation, $activityManager, $templateCache) {
    var maxApplicants = 2;
     $scope.maxApplicantsRange = [
        {name: 'One', value:1},
        {name: 'Two', value:2}
     ];
     init();
     function init() {
         $queryManager.sendQueryAsync(new $capitecQueries.GetDeclarationTypesQuery()).then(function (data) {
             $scope.declarationAnswersLookup = data.data.ReturnData.Results.$values;
         });
         var date = new Date();
         var applicationCaptureStart = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds())).getTime();
         $calculatorDataService.addData('applicationCaptureStart', applicationCaptureStart);
     }

     $scope.getLookupId = function getLookupId(value, lookup) {
         if (value === undefined || lookup === undefined) {
             return '';
         }
         for (var i = 0; i < lookup.length; i++) {
             if (lookup[i].Name === value) {
                 return lookup[i].Id;
             }
         }
         return '';
     };

     $scope.getLookupName = function (item, lookup) {
         if (item === undefined || lookup === undefined) {
             return '';
         }
         for (var i = 0; i < lookup.length; i++) {
             if (lookup[i].Id === item) {
                 return lookup[i].Name;
             }
         }
         return '';
     };

     $scope.applicationDeclarations = {
         "isUnderDebtCounselling": null,
         "withinSpecifiedAge": null,
         "linkedToPropertySale": null,
         "propertyHasTitleDeed":null,
         "numberOfApplicants":null 
     };

     $scope.newPurchaseApplication = false;

     if ($state.current.data.next.indexOf("for-new-home") != -1) {
         $scope.newPurchaseApplication = true;
     }

     $scope.canShow = function () {
         if ($state.current.name != 'home.content.apply.application-precheck-for-switch' &&  $state.current.name != 'home.content.apply.application-precheck-for-new-home') {
             return true;
         }
         else {
             return false;
         }
     };

     $scope.validationPassed = false;

     $scope.allApplicantsArePresent = function(){
        if ($scope.applicationDeclarations.allApplicantsArePresent === $scope.getLookupId('Yes', $scope.declarationAnswersLookup)) {
             return true;
         }
         return false;
     };

     $scope.isUnderDebtCounselling = function () {
         if ($scope.applicationDeclarations.isUnderDebtCounselling === $scope.getLookupId('Yes', $scope.declarationAnswersLookup)) {
             return true;
         }
         return false;
     };

     $scope.isWithinSpecifiedAge = function () {
         if ($scope.applicationDeclarations.withinSpecifiedAge === $scope.getLookupId('No', $scope.declarationAnswersLookup)) {
             return true;
         }
         return false;
     };

     $scope.doesPropertyHaveTitleDeed = function() {
        if($scope.applicationDeclarations.propertyHasTitleDeed === $scope.getLookupId('No', $scope.declarationAnswersLookup)) {
            return true;
        }
        return false;
     };

     $scope.isNewPurchaseApplicationLinkedToPropertySale = function () {
         if ($scope.applicationDeclarations.linkedToPropertySale === $scope.getLookupId('No', $scope.declarationAnswersLookup)) {
             return true;
         }
         return false;
     };

     $scope.canproceed = function () {

         $scope.validationPassed = true;

         if($scope.applicationDeclarations.numberOfApplicants === null || parseFloat($scope.applicationDeclarations.numberOfApplicants) < 1 || parseFloat($scope.applicationDeclarations.numberOfApplicants > maxApplicants)) {
            $scope.validationPassed = false;
            return $scope.validationPassed;
         }
         if($scope.applicationDeclarations.allApplicantsArePresent ===null || $scope.applicationDeclarations.allApplicantsArePresent === $scope.getLookupId('No', $scope.declarationAnswersLookup)) {
            $scope.validationPassed = false;
            return $scope.validationPassed;
         }
         if ($scope.applicationDeclarations.isUnderDebtCounselling === null || $scope.applicationDeclarations.isUnderDebtCounselling === $scope.getLookupId('Yes', $scope.declarationAnswersLookup)) {
             $scope.validationPassed = false;
             return $scope.validationPassed;
         }
         if ($scope.applicationDeclarations.withinSpecifiedAge === null || $scope.applicationDeclarations.withinSpecifiedAge === $scope.getLookupId('No', $scope.declarationAnswersLookup)) {
             $scope.validationPassed = false;
             return $scope.validationPassed;
         }
         if($scope.applicationDeclarations.propertyHasTitleDeed === null || $scope.applicationDeclarations.propertyHasTitleDeed === $scope.getLookupId('No', $scope.declarationAnswersLookup)) {
            $scope.validationPassed = false;
            return $scope.validationPassed;
         }
         if ($scope.newPurchaseApplication && ($scope.applicationDeclarations.linkedToPropertySale === null || $scope.applicationDeclarations.linkedToPropertySale === ($scope.getLookupId('No', $scope.declarationAnswersLookup)))) {
             $scope.validationPassed = false;
             return $scope.validationPassed;
         }
         return $scope.validationPassed;
     };

     $scope.proceed = function () {
        $calculatorDataService.addData('NumberOfApplicants', parseFloat($scope.applicationDeclarations.numberOfApplicants));
        $state.transitionTo($state.current.data.next);
     };
 }]);