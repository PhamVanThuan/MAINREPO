'use strict';

/* Services */

angular.module('sahl.tools.app.services.rubyCodeValidatorServices', [])
.service('$rubyCodeValidatorServices', ['$decisionTreeDesignQueries', '$queryManager', '$codeMirrorService', '$q', '$documentManager', '$eventAggregatorService','$eventDefinitions','$notificationService','$enumerationDataManager','$variableDataManager','messageDataManager','$rootScope','$codemirrorVariablesService',
function ($decisionTreeDesignQueries, $queryManager, $codeMirrorService, $q, $documentManager, $eventAggregatorService, $eventDefinitions, $notificationService, enumerationDataManager, variableDataManager, messageDataManager, $rootScope,$codemirrorVariablesService) {

        var enums, globals, msgs, globalsRaw;
        var getVariables = function () {
            enumerationDataManager.GetLatestEnumerationSetQueryAsync().then(function (data) {
                enums = angular.fromJson(data.data.ReturnData.Results.$values[0].Data);
                variableDataManager.GetLatestVariableSetQueryAsync().then(function (data) {
                    globalsRaw = angular.fromJson(data.data.ReturnData.Results.$values[0].Data);
                    //globals = $codemirrorVariablesService.getGlobalVariables(globalsRaw, enums);
                });
            });
        };

        var getMessages = function () {
            messageDataManager.GetLatestMessageSetQueryAsync().then(function (data) {
                msgs = data.data.ReturnData.Results.$values[0].Data;
            });
        }

        var validateRubyCode = function () {
            var deferred = $q.defer();

            var variableObj = {
                inputs: $codemirrorVariablesService.packageInputs($documentManager.getInputVariablesForTree()),
                outputs: $codemirrorVariablesService.packageOutputs($documentManager.getOutputVariablesForTree()),
                globals: globals
            };

            var variablesJson = angular.toJson(variableObj);
           
            var validateQuery = new $decisionTreeDesignQueries.ValidateRubyCodeQuery($codeMirrorService.getText(), angular.toJson(enums), msgs, variablesJson, angular.toJson(globalsRaw));
            $queryManager.sendQueryAsync(validateQuery).then(function (data) {
                deferred.resolve(data);
            });
            return deferred.promise;
        };
        
        var getExpecting = function (msg) {
            var a = msg.indexOf("expecting");
            if (a != -1) {
                var b = msg.indexOf("`", a);
                if (b!=-1)
                {
                    return msg.substring(b + 1, msg.indexOf("'", b));
                }
            }
            return "";
        };
        var internalEventHandlers = {
            onKeyUpCodeEditor: function (eventObj) {
                internalEventHandlers.validate(eventObj);
            },
            validate: function (eventObj) {
                return; //disabled for now
                validateRubyCode().then(function (data) {
                    if (data.data.ReturnData.Results.$values[0].Valid == true) {
                        $codeMirrorService.removeUnderline();
                    }
                    else {
                        var ErrorString = data.data.ReturnData.Results.$values[0].ErrorString;
                        var Message = data.data.ReturnData.Results.$values[0].Message;
                        $codeMirrorService.removeUnderline();
                        if (Message.indexOf("unexpected") != -1) { //missing something
                            var expecting = getExpecting(Message);
                            if (expecting == "else") {
                                $notificationService.notifyError("", Message);
                                $codeMirrorService.underlineRed("if");
                            }
                            if (expecting == "end") {
                                $notificationService.notifyError("", Message);
                                $codeMirrorService.underlineRed("if");
                                $codeMirrorService.underlineRed("else");
                                $codeMirrorService.underlineRed("elsif");
                            }
                            if (expecting == ")") {
                                eventObj.codeEditor.matchBrackets();
                                $codeMirrorService.underlineRed("(");
                                $notificationService.notifyError("", "Missing close bracket: ')'");
                            }
                            
                        }
                        else if (Message.indexOf("undefined method") != -1) { 
                            var MessageOut = "undefined";
                            if (ErrorString.slice(-1) == '=') {
                                ErrorString = ErrorString.slice(0, -1);
                                MessageOut = "This variable is read only or not defined"
                            }
                            else {
                                MessageOut = "Cannot read outputs"
                            }
                            $codeMirrorService.underlineRed(ErrorString);
                            $notificationService.notifyError("", MessageOut);
                        }
                        else
                        {
                            $notificationService.notifyError("", Message);
                            if (ErrorString != "") {
                                $codeMirrorService.underlineRed(ErrorString);
                            }
                        }
                        /*$codeMirrorService.underlineRed($codeMirrorService.getText());
                        var msg = { "message": data.data.ReturnData.Results.$values[0].Message, "type": "error" }
                        $documentManager.addOutputMessage(msg)*/
                    }
                });
            }
        }

        var debouncedEventHandlers = {
            debouncedonKeyUpCodeEditor: $.debounce(1000, false, internalEventHandlers.onKeyUpCodeEditor),
        }

        var eventHandlers = {
            onKeyUpCodeEditor: function (eventObj) {
                //$codeMirrorService.removeUnderline();
                debouncedEventHandlers.debouncedonKeyUpCodeEditor(eventObj);
            }
        }

        //subscribe to code editor keyup event
        $eventAggregatorService.subscribe($eventDefinitions.onKeyUpCodeEditor, eventHandlers.onKeyUpCodeEditor);
        getVariables();
        getMessages();
        return {
            start: function () { }
        }
    }]);