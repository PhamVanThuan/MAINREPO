'use strict';
angular.module('sahl.js.ui.pages')
    .factory('$wizardFactory', ['$rootScope', '$q', function ($rootScope, $q) {
        var wizardFactory = function () {
            var buttonsVisible = true;
            var changeFn = null;
            var onCompleteFn = null;
			var completed = false;
            var canCancelWorkFlow = true;
            
            var events = {
                YES_WIZARD_CLICKED: 'YES_WIZARD_CLICKED',
                NO_WIZARD_CLICKED: 'NO_WIZARD_CLICKED',
                NEXT_WIZARD_CLICKED: 'NEXT_WIZARD_CLICKED',
                PREVIOUS_WIZARD_CLICKED: 'PREVIOUS_WIZARD_CLICKED',
                CANCEL_WIZARD_CLICKED: 'CANCEL_WIZARD_CLICKED',
                SUBMIT_WIZARD_CLICKED: 'SUBMIT_WIZARD_CLICKED',
                FINISH_WIZARD_CLICKED: 'FINISH_WIZARD_CLICKED'
            };

            var internal = {
                buttonsChanged: function (fnInput) {
                    changeFn = fnInput;
                },
                buttonsVisible: buttonsVisible,
                finished: function () {
                    var awaitables = [];
                    if (onCompleteFn) {
                        var result = onCompleteFn();
                        if (result && result.then) {
                            awaitables.push(result);
                        }
                    }
                    return $q.all(awaitables);
                },
				completed : function(){
					return completed;
				},
                canCancelWorkFlow: function(){
                    return canCancelWorkFlow;
                }
            };

            var operations = {
                hideButtons: function () {
                    buttonsVisible = false;
                    if (changeFn) {
                        changeFn(false);
                    }
                },
                showButtons: function () {
                    buttonsVisible = true;
                    if (changeFn) {
                        changeFn(true);
                    }
                },
                onComplete: function (fn) {
                    onCompleteFn = fn;
                },
				complete : function(){
					completed = true;
				},
                setCanCancelWorkFlowActivity : function(canCancel){
                    canCancelWorkFlow = canCancel;
                }
            };

            return {
                events: events,
                internal: internal,
                hideButtons: operations.hideButtons,
                showButtons: operations.showButtons,
                onComplete: operations.onComplete,
				complete: operations.complete,
                setCanCancelWorkFlowActivity: operations.setCanCancelWorkFlowActivity
            };
        };
        return new wizardFactory();
    }]);