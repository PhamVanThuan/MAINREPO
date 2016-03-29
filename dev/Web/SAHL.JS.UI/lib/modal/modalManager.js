'use strict'

angular.module('sahl.js.ui.modalManager',[])
.service('$modalManagerService', ['$rootScope','$controller', '$compile', function($rootScope, $controller, $compile){
        var defaultOptions = {
        	overlay : false,
        	width : 500,
        	height : 300,
        	draggable : true,
          	templateUrl : '',
          	controller : '',
          	controllerAs: null,
          	controllerParams: {}
        };

        var internal = {
            loadAngularTemplateData : function(templateUrl){
                  var modalBody = '<div class="modal-body" ng-include="\'' + templateUrl + '\'"></div>';
                  return angular.element(modalBody);
            },
            prepareAngularViewData : function(dialogParams){
                  var modalEl = internal.loadAngularTemplateData(dialogParams.templateUrl);

                  var scope = $rootScope.$new();
                  scope['modalTitle'] = dialogParams.title;
                  scope["closeDialog"] = function(){
                      $.Dialog.close();
                  };

                  var locals = angular.extend({$scope: scope}, dialogParams.controllerParams);
                  var ctrl = $controller(dialogParams.controller, locals);

                  if(typeof(dialogParams.controllerAs) == 'string'){
                    scope[dialogParams.controllerAs] = ctrl;
                  }

                  // Yes, ngControllerController is not a typo
                  modalEl.contents().data('$ngControllerController', ctrl);

                  $compile(modalEl)(scope);
                  return modalEl;
            }
        };

        var operations = {
              loadModalWindow : function(dialogParams){
              		dialogParams = angular.extend(defaultOptions, dialogParams);
                  var modalEl = internal.prepareAngularViewData(dialogParams);
                    $.Dialog({
                        overlay: dialogParams.overlay,
                        shadow: true,
                        flat: true,
                        width: dialogParams.width,
                        height: dialogParams.height,
                        draggable: dialogParams.draggable,
                        title: dialogParams.title,
                        onShow: function(_dialog){
                          $(_dialog.children('.content'))
                          .css("padding","32px 10px 10px 10px")
                          .append(modalEl);
                        }
                    });
            }
        };

        return {
            loadModalWindow : operations.loadModalWindow
        };
}]);