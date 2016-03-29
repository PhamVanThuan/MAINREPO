'use strict';

/* Services */

angular.module('sahl.tools.app.services', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands',
    'sahl.tools.app.services.documentManager',
    'sahl.tools.app.services.designSurfaceManager',
    'sahl.tools.app.services.undoManager',
    'sahl.tools.app.services.selectionManager',
    'sahl.tools.app.services.clipboardManager',
    'sahl.tools.app.services.codemirrorVariablesService',
    'sahl.tools.app.services.codeMirrorService',
    'sahl.tools.app.services.keyboardManager',
    'sahl.tools.app.services.variableDataManager',
    'sahl.tools.app.services.enumerationDataManager',
    'sahl.tools.app.services.messageDataManager',
    'sahl.tools.app.services.testDataManager',
    'sahl.tools.app.services.eventAggregatorService',
    'sahl.tools.app.services.eventDefinitions',
    'sahl.tools.app.services.notificationservice',
    'sahl.tools.app.services.documentVersionManager',
    'sahl.tools.app.services.recentDocumentsService',
    'sahl.tools.app.services.documentVersionProviders.version_0_1',
    'sahl.tools.app.services.documentVersionProviders.version_0_2',
    'sahl.tools.app.services.documentVersionProviders.version_0_3',
    'sahl.tools.app.services.documentVersionProviders.version_0_4',
    'sahl.tools.app.services.modalDialogManager',
    'sahl.tools.app.services.statusbarManager',
    'sahl.tools.app.services.signalRSvc',
    'sahl.tools.app.services.debugService',
    'sahl.tools.app.services.sessionLockService',
    'sahl.tools.app.services.startables',
    'sahl.tools.app.services.searchManager',
    'sahl.tools.app.services.breakpointService',
    'sahl.tools.app.services.rubyCodeValidatorServices',
    'sahl.tools.app.services.applicationService',
    'sahl.tools.app.services.globalDataManager',
    'sahl.tools.app.services.utils'
])
.service('ValidationForm', function () {
        function isEmpty(o) {
            for (var i in o) {
                if (o.hasOwnProperty(i)) {
                    return false;
                }
            }
            return true;
        }

        return {
            Form: {},
            isEmpty: isEmpty
        };
 })
.factory('$commandManager', ['$rootScope', '$http', '$serviceConfig', '$q', '$validationManager', '$activityManager', function ($rootScope, $http, $serviceConfig, $q, $validationManager, $activityManager) {
        /**
        *
        *   NB NEED TO PUT RETRY STUFF IN HERE AT SOME POINT
        *
        *   this can be called like so
        *    //var command = new $messages.TestCommand(12345);
        *    //$commandManager.sendCommandAsync(command).then(function (data) { alert('got data'); }, function (errorMessage) { alert('error'); });
        */
        return {
            url: $serviceConfig.CommandService,
            sendCommandAsync: function (commandToSend) {
                $activityManager.startActivity();

                var deferred = $q.defer();
                var json = angular.fromJson(commandToSend);

                var valid = $validationManager.Validate(commandToSend);
                if (valid) {
                    $http.post(this.url, json).success(function (data, status, headers, config) {
                        deferred.resolve({ data: data, status: status, headers: headers });
                        $activityManager.stopActivity();
                    }).error(function (data, status, headers, config) {
                        deferred.reject("An error occurred while accessing the service over http.");
                        $activityManager.stopActivity();
                    });
                }
                else {
                    deferred.reject("Not valid");
                    $activityManager.stopActivity();
                }
                return deferred.promise;
            }
        };
    }])
.factory('$validationManager', ['ValidationForm', '$notificationService', function (ValidationForm, $notificationService) {
        return {
            Validate: function (toValidate) {
                var valid = false;
                var form = ValidationForm.Form;

                if (typeof (toValidate.Validate) === 'function') { //Check if the command/query requires validation.
                    var validationResults = toValidate.Validate();
                    var validationMessages = GetValidationMessages(validationResults);
                    valid = ValidateForm(form, validationMessages);
                    BindValidationMessagesToForm(form, validationMessages);
                }
                else {
                    valid = true;
                }
                if (valid === false) {
                    var validationNotification = GetValidationNotification(validationMessages);
                    $notificationService.notifyError("Validation errors", validationNotification);
                }
                ValidationForm.Form = {};
                return valid;
            }
        };

        function ValidateForm(form, validationMessages) {
            if (!ValidationForm.isEmpty(form)) {
                SetFormToValid(form);
                for (var field in form) {
                    if (field[0] != '$') {
                        var valMessage = validationMessages[field];
                        UpdateFieldWithValidation(form[field], valMessage);
                    }
                }
                return form.$valid;
            }
            else {
                // the the form was empty there is nothing to validate
                return true;
            }
        }

        function SetFormToValid(form) {
            for (var field in form) {
                if (field[0] != '$') {
                    form[field].$setValidity("custom", true);
                    form[field].$error = {};
                    form[field].$setPristine(); //Errors only show while the field is pristine
                }
            }
        }

        function UpdateFieldWithValidation(field, valMessage) {
            if (valMessage !== undefined) {
                field.$setValidity("custom", false);
                field.$valid = false;
            }
        }

        function GetValidationMessages(validationResults) {
            var messages = {};
            for (var i = 0; i < validationResults.length; i++) {
                for (var key in validationResults[i]) {
                    if (!messages[key]) {
                        messages[key] = [];
                    }
                    messages[key].push(validationResults[i][key]);
                }
            }
            return messages;
        }

        function BindValidationMessagesToForm(form, validationResults) {
            var htmlForm = angular.element("#" + form.$name);
            var inputs = htmlForm.find("input, select");

            for (var i = 0; i < inputs.length; i++) {
                var input = inputs[i];
                var msg = validationResults[input.name];
                if (msg) {
                    $(input).attr("title", msg);
                }
                else $(input).removeAttr("title");
            }
        }

        function GetValidationNotification(validationMessages) {
            var message = "<span>There were errors on the form : </span><ul>";
            for (var key in validationMessages) {
                for (var i = 0; i < validationMessages[key].length; i++) {
                    message = message + "<li>" + validationMessages[key][i] + "</li>";
                }
            }
            message = message + "</ul>";
            return message;
        }
    }])
.factory('$httpInterceptor', ['$injector', '$httpRetryQueue', '$location', '$notificationService', function ($injector, $retryQueue, $location, notificationService) {
        var startTime = null;

        function canRecover(rejection) {
            // Recoverable errors need to be defined, timeout as detected from browser is currently considered http status 0
            return rejection.status === 0 || rejection.status === 408;
        }

        return {
            'responseError': function (rejection) {
                if (canRecover(rejection)) {
                    notificationService.notifyError('Error ' + rejection.status, rejection.data.SystemMessages.AllMessages.$values[0].Message);
                    var promise = $retryQueue.pushRetryFn('Error ' + rejection.status, function retryRequest() {
                        // We must use $injector to get the $http service to prevent circular dependency
                        function retry(operation, maxTimes) {
                            return operation().then(undefined, function (reason) {
                                if (maxTimes === 0) {
                                    throw reason;
                                }
                                return retry(operation, maxTimes - 1);
                            });
                        }

                        function httpWithRetry(config, maxTimes) {
                            return retry(function () { $injector.get('$http')(config) }, maxTimes);
                        }
                        return httpWithRetry(rejection.config, 3);
                    });

                    return promise;
                }
                notificationService.notifyError('Error ', rejection.data.SystemMessages.AllMessages.$values[0].Message);
                return $injector.get('$q').reject(rejection);
            }
        }
    }])
.factory('$dropdownData', function () {
        return {
            OccupancyTypes: [
                             {
                                 name: 'Owner Occupied',
                                 value: '1'
                             },
                             {
                                 name: 'Investment Property',
                                 value: '5'
                             }
            ],

            IncomeTypes: [
                            {
                                name: 'Salaried',
                                value: '1'
                            },
                            {
                                name: 'Salaried with Commission',
                                value: '1'
                            },
                            {
                                name: 'Salaried with Housing Allowance',
                                value: '3'
                            },
                            {
                                name: 'Self Employed',
                                value: '2'
                            }

            ]
        }
    })
.factory('$queryManager', ['$rootScope', '$http', '$serviceConfig', '$q', '$validationManager', '$activityManager', function ($rootScope, $http, $serviceConfig, $q, $validationManager, $activityManager) {
        /**
        *
        *   NB NEED TO PUT RETRY STUFF IN HERE AT SOME POINT
        *
        *   this can be called like so
        *    //var command = new $messages.TestCommand(12345);
        *    //$commandManager.sendCommandAsync(command).then(function (data) { alert('got data'); }, function (errorMessage) { alert('error'); });
        */

        return {
            url: $serviceConfig.QueryService,
            sendQueryAsync: function (queryToSend, pageSize, currentPage, filterOn, filterValue) {
                var query = "";
                switch (arguments.length) {
                    case 5:
                        query = "&filterOn=" + filterOn + "&filterValue=" + filterValue
                    case 3:
                        query = "?pageSize=" + pageSize + "&currentPage=" + currentPage + query
                        break;
                    default:
                        break;
                }
                $activityManager.startActivity();
                var deferred = $q.defer();
                var json = angular.fromJson(queryToSend);

                var valid = $validationManager.Validate(queryToSend);
                if (valid) {
                    $http.post(this.url + query, json).success(function (data, status, headers, config) {
                        deferred.resolve({ data: data, status: status, headers: headers });
                        $activityManager.stopActivity();
                    }).error(function () {
                        deferred.reject("An error occurred while accessing the service over http.");
                        $activityManager.stopActivity();
                    });
                }
                else {
                    deferred.reject("Validation errors on the page.");
                    $activityManager.stopActivity();
                }

                return deferred.promise;
            }
        };
    }])
.factory('$searchQueryManager', ['$rootScope', '$http', '$serviceConfig', '$q', '$validationManager', '$activityManager', function ($rootScope, $http, $serviceConfig, $q, $validationManager, $activityManager) {
        /**
        *
        *   NB NEED TO PUT RETRY STUFF IN HERE AT SOME POINT
        *
        *   this can be called like so
        *    //var command = new $messages.TestCommand(12345);
        *    //$commandManager.sendCommandAsync(command).then(function (data) { alert('got data'); }, function (errorMessage) { alert('error'); });
        */

        return {
            url: $serviceConfig.SearchService,
            sendQueryAsync: function (queryToSend, pageSize, currentPage, filterOn, filterValue) {
                var query = "";
                switch (arguments.length) {
                    case 5:
                        query = "&filterOn=" + filterOn + "&filterValue=" + filterValue
                    case 3:
                        query = "?pageSize=" + pageSize + "&currentPage=" + currentPage + query
                        break;
                    default:
                        break;
                }
                $activityManager.startActivity();
                var deferred = $q.defer();
                var json = angular.fromJson(queryToSend);
                var config = { headers: { 'CAPITEC-AUTH': $rootScope.userAuthToken } };

                var valid = $validationManager.Validate(queryToSend);
                if (valid) {
                    $http.post(this.url + query, json, config).success(function (data, status, headers, config) {
                        deferred.resolve({ data: data, status: status, headers: headers, config: config });
                        $activityManager.stopActivity();
                    }).error(function () {
                        deferred.reject("An error occurred while accessing the service over http.");
                        $activityManager.stopActivity();
                    });
                }
                else {
                    deferred.reject("Validation errors on the page.");
                    $activityManager.stopActivity();
                }

                return deferred.promise;
            }
        };
    }])

.factory('$httpRetryQueue', ['$q', '$notificationService', function ($q, $notificationService) {
        var retryQueue = [];
        var service = {
            // The retry service puts its own handler in here!
            onItemAddedCallbacks: [],

            hasMore: function () {
                return retryQueue.length > 0;
            },
            push: function (retryItem) {
                retryQueue.push(retryItem);
                // Call all the onItemAdded callbacks
                angular.forEach(service.onItemAddedCallbacks, function (cb) {
                    try {
                        cb(retryItem);
                    } catch (e) {
                        notificationService.notifyError('httpRetryQueue.push(retryItem): callback threw an error', e);
                    }
                });
            },
            pushRetryFn: function (reason, retryFn) {
                // The reason parameter is optional
                if (arguments.length === 1) {
                    retryFn = reason;
                    reason = undefined;
                }

                // The deferred object that will be resolved or rejected by calling retry or cancel
                var deferred = $q.defer();
                var retryItem = {
                    reason: reason,
                    retry: function () {
                        // Wrap the result of the retryFn into a promise if it is not already
                        $q.when(retryFn()).then(function (value) {
                            // If it was successful then resolve our deferred
                            deferred.resolve(value);
                        }, function (value) {
                            // Othewise reject it
                            deferred.reject(value);
                        });
                    },
                    cancel: function () {
                        // Give up on retrying and reject our deferred
                        deferred.reject();
                    }
                };
                service.push(retryItem);
                return deferred.promise;
            },
            retryReason: function () {
                return service.hasMore() && retryQueue[0].reason;
            },
            cancelAll: function () {
                while (service.hasMore()) {
                    retryQueue.shift().cancel();
                }
            },
            retryAll: function () {
                while (service.hasMore()) {
                    retryQueue.shift().retry();
                }
            }
        };
        return service;
    }])
.factory('$activityManager', [function () {
        var activityListeners = [];
        var keyedActivityListeners = {};
        var runningKeys = [];
        var startCount = 0;

        var startActivity = function () {
            startCount++;
        };
        var stopActivity = function () {
            startCount--;

            if (startCount <= 0) {
                while (activityListeners.length > 0) {
                    if (activityListeners[0]) {
                        activityListeners[0]();
                    }
                    activityListeners.shift();
                }
            }
        };
        var stopActivityWithKey = function (key) {
            var listeners = keyedActivityListeners[key];
            if (listeners) {
                for (var i = 0; i <= listeners.length; i++) {
                    if (listeners[i]) {
                        listeners[i].onStopCallback();
                    }
                }
            }
            runningKeys = $.grep(runningKeys, function (value) {
                return value != key;
            });
            stopActivity();
        };

        var service = {
            startActivity: startActivity,

            stopActivity: stopActivity,

            startActivityWithKey: function (key) {
                var listeners = keyedActivityListeners[key];
                if (listeners) {
                    for (var i = 0; i <= listeners.length; i++) {
                        if (listeners[i]) {
                            listeners[i].onStartCallback();
                        }
                    }
                }
                runningKeys.push(key);
                startActivity();
            },

            stopActivityWithKey: stopActivityWithKey,

            registerSpinListener: function (onStopCallback) {
                activityListeners.push(onStopCallback);
            },

            registerSpinListenerForKey: function (start, stop, key, id) {
                if (!keyedActivityListeners[key]) {
                    keyedActivityListeners[key] = [];
                }
                keyedActivityListeners[key].push({ "onStartCallback": start, "onStopCallback": stop, "id": id });
                for (var i = 0; i < runningKeys.length; i++) {
                    if (runningKeys[i] === key) {
                        start();
                    }
                }
            },

            removeListenerForKey: function (id, key) {
                var listeners = keyedActivityListeners[key];
                var remainingListeners = [];
                if (listeners) {
                    remainingListeners = $.grep(listeners, function (value) {
                        value.id === id;
                    });
                }
                keyedActivityListeners[key] = remainingListeners;
            },
            clearRunningKeyedActivities: function () {
                for (var i = 0; i < runningKeys.length; i++) {
                    stopActivityWithKey(runningKeys[i]);
                }
            }
        };

        return service;
    }])
.factory('$parseHttpService', ['$http', '$q', function ($http, $q) {
        return {
            getString: function (url) {
                var deferred = $q.defer();

                $http.get(url).then(function (response) {
                    deferred.resolve(response.data);
                });

                return deferred.promise;
            }
        }
    }])
.factory('$panelService', ['$rootScope', function ($rootScope) {
        return {
        }
    }])

angular.module('sahl.core.app.messaging', [])
.factory("$jsonPropertyValidator", [function () {
    return {
        isValidName: function (name) {
            return name.match(/^[a-zA-Z0-9 ]+$/); // add a space character to regex
        }
    }
}]);