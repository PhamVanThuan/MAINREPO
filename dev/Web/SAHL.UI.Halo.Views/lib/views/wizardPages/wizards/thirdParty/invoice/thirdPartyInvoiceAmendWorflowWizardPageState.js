'use strict';
angular.module('sahl.ui.halo.views.wizardPages.wizards.thirdParty.invoice.thirdPartyInvoiceAmendWorflowWizardPageState.tpl.html',['ui.grid.edit'])
.controller('Wizards_ThirdParty_Invoice_ThirdPartyInvoiceAmendWorflowWizardPageStateCtrl',['$scope', '$stateParams',
 '$thirdPartyInvoiceManagerService','$financeDomainSharedModels','$accountsManagerService','$workflowActivityManager',
 '$documentDownloadManagerService','$attorneysManagerService','$toastManagerService','$q','$state','$rootScope',
 '$pageFactory','$activityManager','$wizardFactory', '$modalManagerService', '$thirdPartyManagerService',
   function($scope, $stateParams, $thirdPartyInvoiceManagerService,$financeDomainSharedModels, $accountsManagerService, 
    $workflowActivityManager, $documentDownloadManagerService, $attorneysManagerService, $toastManagerService, $q,$state,$rootScope, 
    $pageFactory, $activityManager, $wizardFactory, $modalManagerService, $thirdPartyManagerService){
            $wizardFactory.hideButtons();
            var invoiceParameters = {
              invoiceId : $stateParams.model.tileData.ThirdPartyInvoiceKey,
              LOSS_CONTROL_STORE_ID : 44
            }; 
            
            var _this = this;
            _this.editRow = function(grid, row, isNewRow){
                var title = isNewRow? 'Add Invoice Line Item' : 'Edit Invoice Line Item';
                var dialogParams = {
                          title: title,
                          overlay : true,
                          width : 500,
                          height : 347,
                          draggable : true,
                          templateUrl : 'invoiceLineItemModal.html',
                          controller : 'RowEditCtrl',
                          controllerAs: 'vm',
                          controllerParams: {
                            row: row,
                            $parentScope : $scope
                          }
                      };
                $modalManagerService.loadModalWindow(dialogParams);
            };

            function initialiseController(){
                intialiseScope();
                loadData();
            };

            function intialiseScope(){
                $scope.invoice = {}; 
                $scope.lineItems = {};
                $scope.lineItems.hasLineItems = false;
                $scope.stageInLossControl = "";
                $scope.spvDescription ="";
                $scope.thirdParties = {};
                $scope.selectedThirdParty = 0;
                $scope.accountStatusKey = "";
                $scope.canInvoiceBeCapitalised = false;
                $scope.cannotSave = true;
                $scope.invoiceDocument = null;
                $scope.isRowSelected = false;
                $scope.invoiceTotal = 0;

                var todaysDate = new Date();
                $scope.maximumCalendarDate = todaysDate.getFullYear()+"/"+ ("0"+(todaysDate.getMonth()+1)).slice(-2)+"/"+ ("0"+todaysDate.getDate()).slice(-2);

                $scope.invoiceLineItemCategories = [];
                $scope.invoiceLineItemDescriptions = [];
                $scope.invoiceGrid = gridUtility.gridSettings;
                $scope.updateSaveButtonState = gridUtility.updateSaveButtonState;
                $scope.updateGridTotal = gridUtility.updateGridTotal;

                $scope.addRow =  gridUtility.addRow;
                $scope.deleteSelected = gridUtility.deleteSelected;
                $scope.amendThirdPartyInvoice = invoiceUtility.amendThirdPartyInvoice;
                $scope.cancelThirdPartyInvoiceAmend = invoiceUtility.cancelThirdPartyInvoiceAmend;
                
                $scope.$watch(function(scope){
                    return scope.invoice.invoiceNumber;
                }, function(newValue,oldValue){
                  if($scope.invoice.paymentReference == oldValue || (!!$scope.invoice.paymentReference)==false){
                    $scope.invoice.paymentReference = newValue;
                  }
                });
                $scope.toggleVatCheckbox = function(row){
                  gridUtility.updateVatColumns(row.entity);
                };
                $scope.loadDocument = function(documentGuid){
                    $documentDownloadManagerService.downloadDocumentFromStor(documentGuid, invoiceParameters.LOSS_CONTROL_STORE_ID);
                };
            };

            function loadData(){
               $activityManager.startActivityWithKey("loading");
                invoiceUtility.getLineItemCategoriesAndDescriptions()
                    .then(invoiceUtility.getInvoiceDataById)
                    .then(invoiceUtility.setIfInvoiceCanBeCapitalised)
                    .then(getAccountAndlineItemDataAsynchronously)
                    .then( function() {                 
                      $(window).trigger('resize');
                      $activityManager.stopActivityWithKey("loading");
                      $scope.cannotSave = true;
                    });
            }; 

            /* private functions */
            var getAccountAndlineItemDataAsynchronously = function(){
                var waitingFor = [],
                    deferred = $q.defer();

                waitingFor.push(getThirdParties());
                waitingFor.push(invoiceUtility.getStageInLossControl($scope.invoice.accountKey));
                waitingFor.push(getAccountAndSPVByAccountNumber($scope.invoice.accountKey));
                waitingFor.push(invoiceUtility.getInvoiceLineItems());
                
                $q.all(waitingFor).then(function(){
                  deferred.resolve();
                  gridUtility.updateGridTotal();
                });

                return deferred.promise;
            };

            var getAccountAndSPVByAccountNumber = function(accountNumber){
              $accountsManagerService.getAccountAndSPVByAccountNumber(accountNumber)
                  .then(function(data){
                    $scope.spvDescription = data.results._embedded.sPV.spvDescription;
                    $scope.accountStatusKey = data.results.accountStatusKey;
                  });
            };

            var getThirdParties = function(){
                $thirdPartyManagerService.getThirdParties().then(function(data){
                    $scope.thirdParties = _.filter(data.results._embedded.thirdParty, 
                                                function(thirdParty){ return !thirdParty._embedded.PaymentBankAccount;})
                    if($scope.invoice.thirdPartyId != null){
                      $scope.selectedThirdParty = $scope.invoice.thirdPartyId;
                    }
                });
             };
            /* privatae functions end */

            /* gridFunctions */
            var gridUtility = {
              gridSettings: {
                        multiSelect: false,
                        enableRowSelection: true,                  
                        selectionRowHeaderWidth: 35,
                        enableFiltering: false,
                        enableCellEditOnFocus: true,
                        columnDefs: [
                          {
                            field: 'id',
                            name: '',
                            cellTemplate: '<div class="ui-grid-cell-contents">'+
                                          '<button type="button"'+
                                          '  class="btn btn-xs btn-primary transparentBg"'+
                                          '   ng-click="grid.appScope.vm.editRow(grid, row, false)">'+
                                          '  <i class="fa fa-edit"></i>'+
                                          '</button>'+
                                        '</div>',
                            width: 54
                          },
                          { 
                            name: 'rowNumber', 
                            displayName: 'Row Num.', 
                            cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>', 
                            enableCellEdit: false,
                            width: 95
                          },
                          {   name: 'lineItemType', 
                              displayName: 'Type', 
                              editDropdownOptionsArray : null,
                              cellEditableCondition : function(scope){
                                  var canEdit = true;
                                  scope.row.entity.lineItemType = scope.row.entity.invoiceLineItemCategoryKey;
                                  return canEdit;
                              },
                              editableCellTemplate: 'ui-grid/dropdownEditor', 
                              enableCellEdit: false, 
                              editType: 'dropdown',     
                              editDropdownValueLabel: 'description', 
                              editDropdownIdLabel : 'id'},

                         {   name: 'lineItemDesc', 
                              displayName: 'Description', 
                              editDropdownOptionsArray : null,
                              cellEditableCondition : function(scope){
                                  var canEdit = true;
                                  scope.row.entity.lineItemDesc = scope.row.entity.invoiceLineItemDescriptionKey;
                                  gridUtility.updateGripDescriptionDropdown(scope.row.entity); 
                                  return canEdit;
                              },
                              editableCellTemplate: 'ui-grid/dropdownEditor', 
                              enableCellEdit: false, 
                              editType: 'dropdown',     
                              editDropdownValueLabel: 'description', 
                              editDropdownIdLabel : 'id',
                              editDropdownRowEntityOptionsArrayPath : "descriptionOptions"   },

                          { name: 'lineItemAmount', displayName: 'Amount', cellFilter: 'currencyFilter', cellClass:'right-align-text', enableCellEdit: false },
                            { 
                              name: 'isVatable', 
                              displayName: 'Is Vatable', 
                              visible: true,
                              enableCellEdit: false,
                              cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity.isVatable? \'Yes\':\'No\'}}</div>'
                          },
                          { name: 'lineItemVatAmount', displayName: 'VAT', cellFilter: 'currencyFilter', enableCellEdit: false, cellClass:'right-align-text'},
                          { name: 'lineItemTotalAmtInclVAT', displayName: 'Amount Including VAT', cellFilter: 'currencyFilter', enableCellEdit: false, cellClass:'right-align-text',},
                          { name: 'invoiceLineItemCategoryKey', displayName: 'Type Key', visible: false },
                          { name: 'invoiceLineItemDescriptionKey', displayName: 'Description Key', visible: false }                 
                      ],
                      onRegisterApi: function(gridApi){             
                          $scope.invoiceGrid = gridApi;
                          gridApi.selection.on.rowSelectionChanged($scope,function(row){
                              $scope.isRowSelected = row.isSelected;       
                          });

                          gridApi.edit.on.afterCellEdit($scope,function(row, cellSettings, selectedValue, oldValue){
                              gridUtility.updateSaveButtonState();

                              if(cellSettings.name == "lineItemType" || cellSettings.name == "lineItemDesc"){
                                  gridUtility.setDropdownDescriptionStringValue(row, cellSettings, selectedValue, oldValue);   
                                
                                  if(cellSettings.name == "lineItemType"){
                                    var isNewValue = selectedValue != oldValue;
                                    gridUtility.updateGripDescriptionDropdown(row);

                                    if(isNewValue){
                                      gridUtility.resetDescriptionColumn(row);   
                                    }     
                                  }
                              }else if(cellSettings.name == "lineItemAmount" || cellSettings.name == "isVatable"){
                                  gridUtility.updateVatColumns(row);
                              }
                          });
                      }
              },
              updateVatColumns : function(row){
                      var amount = parseFloat(row['lineItemAmount']);
                      var vat = (row['isVatable'] == true)?  (amount * 0.14) : 0;
                      
                      row['lineItemVatAmount'] = vat;
                      row['lineItemTotalAmtInclVAT'] = vat+amount;
                      gridUtility.updateGridTotal();
              },
              updateGridTotal: function(){
                      var runningTotal = 0;
                      $scope.invoiceGrid.grid.api.data.forEach(function(data){
                          runningTotal  += data.lineItemTotalAmtInclVAT;
                      });
                      $scope.invoiceTotal = runningTotal;
              },
              updateSaveButtonState : function(){
                  if($scope.invoiceGrid.data){
                    $scope.cannotSave = $scope.invoiceGrid.data.length === 0;
                  }else{
                    $scope.cannotSave = true;
                  }
              },
              addRow: function() {       
                      _this.editRow($scope.invoiceGrid, {}, true);
              },
              deleteSelected: function(){
                      angular.forEach($scope.invoiceGrid.selection.getSelectedRows(), function (data, index) {
                        $scope.invoiceGrid.data.splice($scope.invoiceGrid.data.lastIndexOf(data), 1);
                      });
                      gridUtility.updateSaveButtonState();
                      gridUtility.updateGridTotal();
                      $scope.isRowSelected = false;
              },
              updateGripDescriptionDropdown: function(row){
                      var selectedCategory = _.where($scope.invoiceLineItemCategories, {description: row['lineItemType'] });
                      var selectedDescriptions = _.where($scope.invoiceLineItemDescriptions, { categoryId : selectedCategory[0].id });
                      row.descriptionOptions = selectedDescriptions[0].descriptionOptions;
              },
              setDropdownDescriptionStringValue: function(row, cellSettings, selectedValue, oldValue){

                      var newValue = parseInt(selectedValue, 10) || row.invoiceLineItemCategoryKey;

                      switch(cellSettings.name){
                          case "lineItemType" : 
                            var selectedOption = _.where(cellSettings.editDropdownOptionsArray, {id: newValue});
                            row[cellSettings.name] = selectedOption[0].description;
                            row['invoiceLineItemCategoryKey'] = newValue;
                          break;
                          case "lineItemDesc" : 
                            var selectedOption = _.where(row.descriptionOptions, {id: newValue});
                            row[cellSettings.name] = selectedOption[0].description;
                            row['invoiceLineItemDescriptionKey'] = newValue;
                          break;
                      }
              },
              resetDescriptionColumn: function(row){
                      var defaultDescriptionOption = row.descriptionOptions[0];
                      row["lineItemDesc"] = defaultDescriptionOption.description;
                      row['invoiceLineItemDescriptionKey'] = defaultDescriptionOption.id;
              }
            };
            /* gridFunctions End */


            /* invoice utility */
            var invoiceUtility = {
              getLineItemCategoriesAndDescriptions : function(){
                      var deferred = $q.defer();
                      invoiceUtility.getInvoiceLineItemCategories().then(function (data) {
                          var waitingFor = [];
                          
                          _.each($scope.invoiceLineItemCategories, function(category, key, invoiceLineItemCategories){
                              waitingFor.push(invoiceUtility.getInvoiceLineItemDescriptionByCategory(category.id));
                          });

                          $q.all(waitingFor).then(function(){
                              deferred.resolve();
                          });
                      });

                      return deferred.promise;
              },
              getInvoiceLineItemCategories: function(){
                      var deferred = $q.defer();
                      $thirdPartyInvoiceManagerService.getLookupLineItemTypes().then(function(data){
                          $scope.invoiceLineItemCategories = data.data._embedded.InvoiceLineItemCategory;
                          $scope.invoiceGrid.grid.options.columnDefs[1].editDropdownOptionsArray = $scope.invoiceLineItemCategories;
                          $scope.typeOptions = $scope.invoiceLineItemCategories[0].description;

                          deferred.resolve(data);
                      });
                      return deferred.promise;         
              },
              getInvoiceLineItemDescriptionByCategory: function(categoryId){
                      var deferred = $q.defer();
                      $thirdPartyInvoiceManagerService.getLookUpLineItemDescriptions(categoryId).then(function(data){
                          var descriptionOptions = data.data._embedded.invoiceLineItemDescription
                          $scope.invoiceLineItemDescriptions.push({ categoryId: categoryId, descriptionOptions : descriptionOptions});
                          deferred.resolve();
                      });
                      return deferred.promise;           
              },
              getStageInLossControl: function(accountNumber){
                      $accountsManagerService.getLossControlProcessStage(accountNumber,"losscontrol")
                          .then(function(data){
                            $scope.stageInLossControl = data.results.stage;
                          });
              },
              getInvoiceDataById: function(){
                      var deferred = $q.defer();
                      $thirdPartyInvoiceManagerService.getInvoiceDataById(invoiceParameters.invoiceId).then(function(data){
                        $scope.invoice = data.results; 
                       
                        if(!!$scope.invoice._embedded.documents.totalCount){              
                          $scope.invoiceDocument = $scope.invoice._embedded.documents._embedded.thirdPartyInvoiceDocument[0];
                        }else{
                          $toastManagerService.error({
                              title: 'Error',
                              text: "Failed to load the invoice attachment details."
                          });
                        }                        
                        var invoiceDate = new Date($scope.invoice.invoiceDate);
                        if(isNaN(invoiceDate.getYear())){
                          invoiceDate = new Date();
                        }
                        var month = invoiceDate.getMonth() + 1;
                        $scope.invoice.invoiceDate = ("0"+invoiceDate.getDate()).slice(-2)+'/'+ ("0"+month).slice(-2)+'/'+invoiceDate.getFullYear();
                                               
                        deferred.resolve();
                      });
                      return deferred.promise;
              },
              getInvoiceLineItems: function(){
                        var deferred = $q.defer();

                        $thirdPartyInvoiceManagerService.getInvoiceLineItems(invoiceParameters.invoiceId).then(function(data){                        
                          if(data.results.data.totalCount != 0){
                              $scope.lineItems = data.results.data._embedded.thirdPartyInvoiceLineItems;
                              $scope.invoiceGrid.invoiceHasLineItems = true;
                              $scope.invoiceGrid.data = data.results.data._embedded.thirdPartyInvoiceLineItems; 
                          }else{
                            $scope.invoiceGrid.data = [];
                          }
                          deferred.resolve();
                          
                        });
                        return deferred.promise;
              },
              cancelThirdPartyInvoiceAmend: function(){
                /**finish off workflow**/
                $workflowActivityManager.cancel(
                    $stateParams.tileAction.instanceId,
                    $scope.guid,
                    $stateParams.tileAction.name,
                    false
                );
                $pageFactory.back();
              },
              setIfInvoiceCanBeCapitalised: function(){
                        var accountStatusKey = parseInt($scope.accountStatusKey,10) || 0,
                            nonCapitalisableStatuses = [ 2, 3, 6 ];

                        $scope.canInvoiceBeCapitalised = accountStatusKey > 0 && nonCapitalisableStatuses.indexOf(accountStatusKey) === -1;
              },
              isInvoiceValid : function(){
                        var isValid = true;
                        if( $scope.invoice.invoiceDate == null || $scope.invoice.invoiceDate.trim().length === 0 || $scope.invoice.invoiceDate.split('/').length != 3){
                            $toastManagerService.error({
                              title: 'Invoice date not set',
                              text: "Please provide the invoice date."
                            });
                            isValid = false;
                        }

                        if( $scope.selectedThirdParty == null || $scope.selectedThirdParty == "" || $scope.selectedThirdParty.trim().length === 0){
                            $toastManagerService.error({
                              title: 'No attorney selected',
                              text: "Please select the attorney this invoice belongs to."
                            });
                            isValid = false;
                        }

                        if( $scope.invoice.invoiceNumber == null || $scope.invoice.invoiceNumber.trim().length === 0){
                            $toastManagerService.error({
                              title: 'Invoice number not set',
                              text: "Please provide an invoice number."
                            });
                            isValid = false;
                        }

                        if( $scope.invoice.paymentReference == null || $scope.invoice.paymentReference.trim().length === 0){
                            $toastManagerService.error({
                              title: 'Payment reference not set',
                              text: "Please provide the payment reference for this invoice."
                            });
                            isValid = false;
                        }

                        if( $scope.invoice.paymentReference != null && $scope.invoice.paymentReference.length > 30){
                            $toastManagerService.error({
                              title: 'Payment reference too long',
                              text: "The payment reference cannot be longer than 30 characters."
                            });
                            isValid = false;
                        }

                        if( $scope.invoice.invoiceNumber.length > 50){
                            $toastManagerService.error({
                              title: 'Invoice Number too long.',
                              text: "The invoice Number cannot be greater than 50 characters."
                            });
                            isValid = false;
                        }

                        return isValid;
              },
              amendThirdPartyInvoice: function(){              //- Do the below call for each line item 

                        if(!invoiceUtility.isInvoiceValid()){
                            return;
                        }

                        var datePieces = $scope.invoice.invoiceDate.split('/');
                        var invoiceDate = new Date(datePieces[2]+"/"+datePieces[1]+"/"+datePieces[0]);
                        var invoiceLineItemModels = [];
                        var errorMessages = [];

                        _.each($scope.invoiceGrid.data, function(element, index, list){
                            var lineItemId = $scope.lineItems[index]? $scope.lineItems[index].id : null;
                            var item = new $financeDomainSharedModels.InvoiceLineItemModel(lineItemId,
                                                        $scope.invoice.id,
                                                        $scope.invoiceGrid.data[index].invoiceLineItemDescriptionKey,
                                                        $scope.invoiceGrid.data[index].lineItemAmount, 
                                                        $scope.invoiceGrid.data[index].isVatable);
                            
                            var lineItemValidationMessages = { errorMessages: item.Validate() };
                            if(lineItemValidationMessages.errorMessages.length == 0){
                                invoiceLineItemModels.push(item);
                            }else{
                              lineItemValidationMessages['rowNumber'] = index+1;
                              errorMessages.push(lineItemValidationMessages);
                            }
                            
                        });            
                        if(errorMessages.length == 0){
                            var thirdPartyInvoiceModel = new $financeDomainSharedModels.ThirdPartyInvoiceModel($scope.invoice.id, 
                                                            $scope.selectedThirdParty, 
                                                            $scope.invoice.invoiceNumber, 
                                                            invoiceDate,
                                                            invoiceLineItemModels, 
                                                            $scope.invoice.capitaliseInvoice,
                                                            $scope.invoice.paymentReference
                                                            ); 
                            invoiceUtility.executeAmendThirdPartyInvoice(thirdPartyInvoiceModel);
                        }else{
                            invoiceUtility.displayLineItemValidationMessages(errorMessages);
                        }
              },
              displayLineItemValidationMessages : function(errorMessagesCollection){
                  _.each(errorMessagesCollection, function(rowErrorMessageCollection, index, errorMessageList){
                    _.each(rowErrorMessageCollection.errorMessages, function(rowErrorMessages, index, rowList){
                        _.map(rowErrorMessages, function(errorMessage){
                            $toastManagerService.error({
                                    title: 'Error on row: '+(rowErrorMessageCollection.rowNumber),
                                    text: errorMessage
                                });
                        });
                    });  
                  });
              },
              executeAmendThirdPartyInvoice: function (thirdPartyInvoiceModel) {
                        $thirdPartyInvoiceManagerService.amendThirdPartyInvoice(thirdPartyInvoiceModel)
                                        .then(invoiceUtility.events.invoiceAmendmentSuccess, 
                                          invoiceUtility.events.invoiceAmendmentFailure);
              },
              events: {
                      invoiceAmendmentSuccess: function(){
                          $toastManagerService.success({
                              title: 'Success',
                              text: 'Update complete'
                          }).promise.then(function() {
                              $rootScope.entitiesModel.activeEntity.breadcrumbs.pop();
                              /**finish off workflow**/
                              $workflowActivityManager.complete($stateParams.tileAction.instanceId,
                                $scope.guid,
                                $stateParams.tileAction.name,
                                false
                              );
							  $wizardFactory.complete();
                              $pageFactory.back(true);
                          });                          
                      },
                      invoiceAmendmentFailure: function(results){
                          $toastManagerService.error({
                              title: 'Error',
                              text: results.results[0].Message
                          });
                      }
              }

            };

           initialiseController();
}]).controller('RowEditCtrl', ['$parentScope', 'row','$scope',
    function($parentScope, row, $scope){
          var vm = this;
          vm.isNewRow = false;
          if($scope.modalTitle == 'Add Invoice Line Item'){
              vm.isNewRow = true;
              resetEntityData();
              
          }else{
              //copy the entity over so we don't update the grid without clicking the finish button
              vm.entity = angular.copy(row.entity);
          }

          vm.row = row;
          vm.save = save;
          vm.invoiceTypes = $parentScope.invoiceLineItemCategories;
          vm.invoiceDescriptions = $parentScope.invoiceLineItemDescriptions;
          vm.descriptionOptions = [];
          vm.description = "";
          vm.setDescriptions = setDescriptions;

          function validate(){
            var isValid = true;
            if((!!vm.type) == false){
                isValid = false;
                vm.invalidType = true;
            }

            if((!!vm.description) == false){
                isValid = false;
                vm.invalidDescription = true;
            }

            var regex  = /^[1-9]\d*(?:\.\d{0,2})?$/;
            if((!!vm.entity.lineItemAmount) == false || regex.test(vm.entity.lineItemAmount) == false){
                isValid = false;
                vm.invalidAmount = true;
            }

            return isValid;
          }

          function setCleanValidationFlags(){
                vm.invalidType = false;
                vm.invalidDescription = false;
                vm.invalidAmount = false;
          }

          function save(){
              setCleanValidationFlags();
              if(validate())
              {
                  row.entity = angular.extend(row.entity, vm.entity);
                  row.entity.invoiceLineItemCategoryKey = vm.type;
                  row.entity.invoiceLineItemDescriptionKey = vm.description;

                  row.entity.lineItemType = (_.where(vm.invoiceTypes, {id : vm.type}))[0].description;
                  row.entity.lineItemDesc = (_.where(vm.descriptionOptions, { id : vm.description}))[0].description;
                  updateVatColumns(row.entity);

                  if(vm.isNewRow){
                    $parentScope.invoiceGrid.data.push(row.entity);
                    $parentScope.updateSaveButtonState();
                    resetEntityData();
                    if(!vm.addAnotherNewRow){
                      $scope.closeDialog();
                    }

                  }else{
                    $scope.closeDialog();
                  }

                  $parentScope.updateGridTotal();
              }

          }

          function resetEntityData(){
              vm.entity = {
                        invoiceLineItemCategoryKey : null,
                      invoiceLineItemDescriptionKey : null,
                      lineItemAmount : 0
                    };
              row['entity'] = {};
              initialise();
          }

          function updateVatColumns(entity){
              var amount = parseFloat(entity['lineItemAmount']);
              var vat = (entity['isVatable'] == true)?  (amount * 0.14) : 0;
              entity.lineItemAmount = parseFloat(entity.lineItemAmount);
              entity['lineItemVatAmount'] = vat;
              entity['lineItemTotalAmtInclVAT'] = vat+amount;
          }          

          function setDescriptions(selectedCategory){
                var category = parseInt(selectedCategory, 10);
                if(category > 0)
                {
                  var descriptions = _.where(vm.invoiceDescriptions, {categoryId : category});

                  vm.descriptionOptions = descriptions[0].descriptionOptions;
                  vm.description = descriptions[0].descriptionOptions[0].id;
                }else{
                  vm.description = null;
                }

          }

          function initialise(){
            vm.type = vm.entity.invoiceLineItemCategoryKey;
            setDescriptions(vm.type);
            vm.description = vm.entity.invoiceLineItemDescriptionKey;
            setCleanValidationFlags();
          }
          initialise();

}]);