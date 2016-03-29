'use strict';

/* Services */

angular.module('capitecApp.services', ['capitecApp.serviceConfig']).
    service('ValidationForm', function () {
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
    .factory('$commandManager', ['$rootScope', '$http', '$serviceConfig', '$q', '$validationManager', '$activityManager', '$cookieService', function ($rootScope, $http, $serviceConfig, $q, $validationManager, $activityManager, $cookieService) {
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
                var json = angular.toJson(commandToSend);
                var config = { headers: { 'CAPITEC-AUTH': $cookieService.getItem('authToken') }, timeout: $serviceConfig.timeout };

                var valid = $validationManager.Validate(commandToSend);
                if (valid) {
                    $http.post(this.url, json, config).success(function (data, status, headers, config) {
                        $rootScope.userAuthToken = headers('CAPITEC-AUTH');
                        $cookieService.setItem('authToken', $rootScope.userAuthToken); // $cookieService.authToken & $rootScope.userAuthToken are set at the same time
                        deferred.resolve({ data: data, status: status, headers: headers, config: config });
                        $activityManager.stopActivity();
                    }).error(function (data, status, headers, config) {
                        deferred.reject('An error occurred while accessing the service over http.');
                        $activityManager.clearRunningKeyedActivities();
                        $activityManager.stopActivity();
                    });
                }
                else {
                    deferred.reject('Not valid');
                    $activityManager.stopActivity();
                }
                return deferred.promise;
            }
        };
    }])
    .factory('$validation', [function () {
        return {
            isValidZAIDNumber: function (idNumber) {
                // SA ID Number have to be 13 digits, so check the length
                if (idNumber.length != 13 || !typeof parseInt(idNumber) == 'number') {
                    return false;
                }

                // get first 6 digits as a valid date
                var tempDate = new Date(idNumber.substring(0, 2), idNumber.substring(2, 4) - 1, idNumber.substring(4, 6));

                var id_date = tempDate.getDate();
                var id_month = tempDate.getMonth();
                var id_year = tempDate.getFullYear();

                var fullDate = id_date + "-" + id_month + 1 + "-" + id_year;

                if (!((tempDate.getYear() == idNumber.substring(0, 2)) && (id_month == idNumber.substring(2, 4) - 1) && (id_date == idNumber.substring(4, 6)))) {
                    return false;
                }

                // get the gender
                var genderCode = idNumber.substring(6, 10);
                var gender = parseInt(genderCode) < 5000 ? "Female" : "Male";

                // get country ID for citzenship
                var citzenship = parseInt(idNumber.substring(10, 11)) == 0 ? "Yes" : "No";

                // apply Luhn formula for check-digits
                var tempTotal = 0;
                var checkSum = 0;
                var multiplier = 1;
                for (var i = 0; i < 13; ++i) {
                    tempTotal = parseInt(idNumber.charAt(i)) * multiplier;
                    if (tempTotal > 9) {
                        tempTotal = parseInt(tempTotal.toString().charAt(0)) + parseInt(tempTotal.toString().charAt(1));
                    }
                    checkSum = checkSum + tempTotal;
                    multiplier = (multiplier % 2 == 0) ? 1 : 2;
                }
                if ((checkSum % 10) != 0) {
                    return false;
                };
                return true;
            },
            compare: function (item, lookup, value) {
                if (item === undefined || lookup === undefined || value === undefined) {
                    return false;
                }
                for (var i = 0; i < lookup.length; i++) {
                    if (lookup[i].Name === value && lookup[i].Id == item) {
                        return true;
                    }
                }
                return false;
            }
        }
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
                    $notificationService.notifyError('Validation errors', validationNotification);
                }
                ValidationForm.Form = {};
                return valid;
            },
            ValidateOnly: function (toValidate) {
                var valid = false;
                if (typeof (toValidate.Validate) === 'function') {
                    var validationResults = toValidate.Validate();
                    var validationMessages = GetValidationMessages(validationResults);
                    valid = (validationResults == false || validationResults.length === 0);
                }
                else {
                    valid = true;
                }
                if (valid === false) {
                    var validationNotification = GetValidationNotification(validationMessages);
                    $notificationService.notifyError("Validation errors", validationNotification);
                }
                return valid;
            },
            ValidateModels: function (modelsToValidate, validationMessage) {
                var valid = false;
                var combinedValidationResults = new Array();

                for (var i = 0; i < modelsToValidate.length; i++) {
                    if (typeof (modelsToValidate[i].Validate) === 'function') {
                        var validationResults = modelsToValidate[i].Validate();
                        combinedValidationResults = combinedValidationResults.concat(validationResults);
                    }
                }

                var validationMessages = GetValidationMessages(combinedValidationResults);
                valid = (!combinedValidationResults || combinedValidationResults.length === 0);

                if (valid === false) {
                    var validationNotification = GetValidationNotification(validationMessages);
                    var message = validationMessage || "Validation errors";
                    $notificationService.notifyError(message, validationNotification);
                }
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
                    form[field].$setValidity('custom', true);
                    form[field].$error = {};
                    form[field].$setPristine(); //Errors only show while the field is pristine
                }
            }
        }

        function UpdateFieldWithValidation(field, valMessage) {
            if (valMessage !== undefined) {
                field.$setValidity('custom', false);
                field.$valid = false;
            }
        }

        function GetValidationMessages(validationResults) {
            var messages = new Array();
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
            var htmlForm = angular.element('#' + form.$name);
            var inputs = htmlForm.find('input, select, .toggle-switch');

            for (var i = 0; i < inputs.length; i++) {
                var input = inputs[i];
                var msg = validationResults[input.name];
                if (msg) {
                    $(input).attr('title', msg);
                }
                else $(input).removeAttr('title');
            }
        }

        function GetValidationNotification(validationMessages) {
            var message = '<span>There were errors on the form : </span><ul>';
            for (var key in validationMessages) {
                for (var i = 0; i < validationMessages[key].length; i++) {
                    message = message + '<li>' + validationMessages[key][i] + '</li>';
                }
            }
            message = message + '</ul>';
            return message;
        }
    }])
    .factory('$httpInterceptor', ['$q', '$notificationService', '$serviceConfig', '$injector', function ($q, $notificationService, $serviceConfig, $injector) {
        var startTime = null;

        var can_recover = function (rejection) {
            // Recoverable errors need to be defined, timeout as detected from browser is currently considered http status 0
            return rejection.status === 0 || rejection.status === 408;
        }

        var retry_async_request = function (config) {
            var retryResult = $q.defer();

            var retryHttp = function (config) {
                $injector.get('$http')(config).success(function (data, status, headers, config) {
                    retryResult.resolve({ data: data, status: status, headers: headers, config: config });
                }).error(function (data, status, headers, config) {
                    retryResult.reject({ data: data, status: status, headers: headers, config: config });
                });
            }
            retryHttp(config);

            return retryResult.promise;
        }

        return {
            'responseError': function (rejection) {

                if ('$retry_ttl' in rejection.config) {
                    rejection.config.$retry_ttl -= 1;
                }
                else {
                    rejection.config.$retry_ttl = $serviceConfig.timeoutRetryCount;
                }

                if (can_recover(rejection) && rejection.config.$retry_ttl > 0) {
                    $notificationService.notifyInfo('retrying');
                    return retry_async_request(rejection.config);
                } else {
                    $notificationService.notifyError('Error ' + "", "Unable to Contact Service. Please retry.");
                }

                if (rejection.data && 'SystemMessages' in rejection.data) {
                    for (var i = 0; i < rejection.data.SystemMessages.AllMessages.$values.length; i++) {
                        if (rejection.data.SystemMessages.AllMessages.$values[i].Severity == 1) {
                            $notificationService.notifyError('Error ', rejection.data.SystemMessages.AllMessages.$values[i].Message);
                        }
                    }
                }

                return $q.reject(rejection);
            },
            'response': function (response) {
                if (response.data.SystemMessages && response.data.SystemMessages.AllMessages.$values.length > 0) {
                    angular.forEach(response.data.SystemMessages.AllMessages.$values, function (message) {
                        if (message.Severity == 2) {
                            $notificationService.notifyInfo('Info', message.Message);
                        }
                        else if(message.Severity == 0){
                            $notificationService.notifyWarning('Warning', message.Message);
                        }
                    });
                }
                return response;
            }
        }
    }])
    .factory('$calculatorDataService', ['$cookieService', function ($cookieService) {
        var dataList = {};
        return {
            addData: function (name, newObj) {
                dataList[name] = angular.toJson(newObj);
                var dataString = angular.toJson(dataList);
                if (localStorage) {
                    localStorage.setItem('dataList', dataString);
                }
            },
            getData: function () {
                var data = {};
                if (localStorage) {
                    var localStorageData = angular.fromJson(localStorage.getItem('dataList'));
                    if (localStorageData) {
                        data = localStorageData;
                    } else {
                        data = dataList;
                    }
                } else {
                    data = dataList;
                }
                return data;
            },
            getDataValue: function (value) {
                var data = {};
                if (localStorage) {
                    var localStorageData = angular.fromJson(localStorage.getItem('dataList'));
                    if (localStorageData) {
                        data = localStorageData;
                    } else {
                        data = dataList;
                    }
                } else {
                    data = dataList;
                }
                return angular.fromJson(data[value]);
            },
            clearData: function () {
                dataList = {};
                if (localStorage) {
                    localStorage.clear();
                }
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
    .factory('$queryManager', ['$rootScope', '$http', '$serviceConfig', '$q', '$validationManager', '$activityManager', '$cookieService', function ($rootScope, $http, $serviceConfig, $q, $validationManager, $activityManager, $cookieService) {
        /**
        *
        *   this can be called like so
        *    //var command = new $messages.TestCommand(12345);
        *    //$commandManager.sendCommandAsync(command).then(function (data) { alert('got data'); }, function (errorMessage) { alert('error'); });
        */

        // Keeps track of how many requests are in play.
        var openRequestCounter = 0;

        var requestStarted = function (skipActivityManager) {
            if (openRequestCounter == 0 && !skipActivityManager) {
                $activityManager.startActivity();
            }
            openRequestCounter++;
        };

        var requestEnded = function (skipActivityManager) {
            openRequestCounter--;
            if (openRequestCounter == 0 && !skipActivityManager) {
                $activityManager.stopActivity();
            }
        };

        return {
            url: $serviceConfig.QueryService,
            sendQueryAsync: function (queryToSend, paginationOptions, filterOptions, sortOptions, skipActivityManager) {
                var query = '';

                if (paginationOptions != undefined) {
                    query = '?pageSize=' + paginationOptions.pageSize + '&currentPage=' + paginationOptions.currentPage;
                }

                if (filterOptions != undefined && filterOptions.filterValue != undefined) {
                    if (query.indexOf('?pageSize') != -1) {
                        query = query + '&filterOn=' + filterOptions.filterOn + '&filterValue=' + filterOptions.filterValue
                    } else {
                        query = '?filterOn=' + filterOptions.filterOn + '&filterValue=' + filterOptions.filterValue
                    }
                }

                if (sortOptions != undefined) {
                    if (query.indexOf('?pageSize') != -1 || query.indexOf('?filterOn') != -1) {
                        query = query + '&orderBy=' + sortOptions.orderBy + '&sortDirection=' + sortOptions.sortDirection
                    } else {
                        query = '?orderBy=' + sortOptions.orderBy + '&sortDirection=' + sortOptions.sortDirection
                    }
                }

                requestStarted(skipActivityManager);
                var deferred = $q.defer();
                var json = angular.toJson(queryToSend);
                var config = { headers: { 'CAPITEC-AUTH': $cookieService.getItem('authToken') }, timeout: $serviceConfig.timeout };

                var valid = $validationManager.Validate(queryToSend);
                if (valid) {
                    $http.post(this.url + query, json, config).success(function (data, status, headers, config) {
                        requestEnded(skipActivityManager);
                        deferred.resolve({ data: data, status: status, headers: headers, config: config });
                    }).error(function (data, status, headers, config) {
                        requestEnded(skipActivityManager);
                        $activityManager.clearRunningKeyedActivities();
                        $activityManager.stopActivity();
                        deferred.reject('An error occurred while accessing the service over http.');
                    });
                }
                else {
                    requestEnded(skipActivityManager);
                    deferred.reject('Validation errors on the page.');
                }
                return deferred.promise;
            }
        };
    }])
    .factory('$decisionTreeManager', ['$rootScope', '$http', '$serviceConfig', '$q', '$validationManager', '$activityManager', '$cookieService', function ($rootScope, $http, $serviceConfig, $q, $validationManager, $activityManager, $cookieService) {
        /**
        *
        *   this can be called like so
        *    //var command = new $messages.TestCommand(12345);
        *    //$commandManager.sendCommandAsync(command).then(function (data) { alert('got data'); }, function (errorMessage) { alert('error'); });
        */

        return {
            url: $serviceConfig.DecisionTreeService,
            sendQueryAsync: function (queryToSend) {
                $activityManager.startActivity();
                var deferred = $q.defer();
                var json = angular.toJson(queryToSend);
                var config = { headers: { 'CAPITEC-AUTH': $cookieService.getItem('authToken') }, timeout: $serviceConfig.timeout };

                var valid = $validationManager.Validate(queryToSend);
                if (valid) {
                    $http.post(this.url, json, config).success(function (data, status, headers, config) {
                        deferred.resolve({ data: data, status: status, headers: headers, config: config });
                        $activityManager.stopActivity();
                    }).error(function (data, status, headers, config) {
                        deferred.reject('An error occurred while accessing the service over http.');
                        $activityManager.stopActivity();
                        $activityManager.clearRunningKeyedActivities();
                    });
                }
                else {
                    deferred.reject('Validation errors on the page.');
                    $activityManager.stopActivity();
                }

                return deferred.promise;
            }
        };
    }])
    .factory('$searchQueryManager', ['$rootScope', '$http', '$serviceConfig', '$q', '$validationManager', '$activityManager', '$cookieService', function ($rootScope, $http, $serviceConfig, $q, $validationManager, $activityManager, $cookieService) {
        /**
        *
        *   this can be called like so
        *    //var command = new $messages.TestCommand(12345);
        *    //$commandManager.sendCommandAsync(command).then(function (data) { alert('got data'); }, function (errorMessage) { alert('error'); });
        */

        return {
            url: $serviceConfig.SearchService,

            sendQueryAsync: function (queryToSend, paginationOptions, filterOptions, sortOptions) {
                var query = '';

                if (paginationOptions != undefined) {
                    query = '?pageSize=' + paginationOptions.pageSize + '&currentPage=' + paginationOptions.currentPage;
                }

                if (filterOptions != undefined && filterOptions.filterValue != undefined) {
                    if (query.indexOf('?pageSize') != -1) {
                        query = query + '&filterOn=' + filterOptions.filterOn + '&filterValue=' + filterOptions.filterValue
                    } else {
                        query = '?filterOn=' + filterOptions.filterOn + '&filterValue=' + filterOptions.filterValue
                    }
                }

                if (sortOptions != undefined) {
                    if (query.indexOf('?pageSize') != -1 || query.indexOf('?filterOn') != -1) {
                        query = query + '&orderBy=' + sortOptions.orderBy + '&sortDirection=' + sortOptions.sortDirection
                    } else {
                        query = '?orderBy=' + sortOptions.orderBy + '&sortDirection=' + sortOptions.sortDirection
                    }
                }
                var deferred = $q.defer();
                var json = angular.toJson(queryToSend);
                var config = { headers: { 'CAPITEC-AUTH': $cookieService.getItem('authToken') }, timeout: $serviceConfig.timeout };

                var valid = $validationManager.Validate(queryToSend);
                if (valid) {
                    $http.post(this.url + query, json, config).success(function (data, status, headers, config) {
                        deferred.resolve({ data: data, status: status, headers: headers, config: config });
                        $activityManager.stopActivity();
                    }).error(function () {
                        deferred.reject('An error occurred while accessing the service over http.');
                        $activityManager.stopActivity();
                        $activityManager.clearRunningKeyedActivities();
                    });
                }
                else {
                    deferred.reject('Validation errors on the page.');
                    $activityManager.stopActivity();
                }

                return deferred.promise;
            }
        };
    }])
    .factory('$notificationService', function () {
        $.pnotify.defaults.history = false;
        var stack_bottomright = { 'dir1': 'up', 'dir2': 'left', 'firstpos1': 25, 'firstpos2': 25, 'addpos2': 0, 'animation': true };
        var opts = {
            addclass: 'stack-bottomright',
            stack: stack_bottomright,
            styling: 'bootstrap',
            animation: 'none'
        }
        return {
            notifySuccess: function (title, message) {
                opts.type = 'success';
                opts.title = title;
                opts.text = message;
                opts.hide = true;
                opts.sticker = true;
                $.pnotify(opts);
            },
            notifyError: function (title, message) {
                opts.type = 'error';
                opts.title = title;
                opts.text = message;
                opts.hide = true;
                opts.sticker = true;
                $.pnotify(opts);
            },
            notifyWarning: function (title, message) {
                opts.type = 'warning';
                opts.title = title;
                opts.text = message;
                opts.hide = true;
                opts.sticker = true;
                $.pnotify(opts);
            },
            notifyInfo: function (title, message) {
                opts.type = 'info';
                opts.title = title;
                opts.text = message;
                opts.hide = true;
                opts.sticker = true;
                $.pnotify(opts);
            },
            notify: function (title, message) {
                opts.title = title;
                opts.text = message;
                opts.hide = true;
                opts.sticker = true;
                $.pnotify(opts);
            },
            notifySticky: function (title, message) {
                opts.type = 'error';
                opts.title = title;
                opts.text = message;
                opts.hide = false;
                opts.sticker = false;
                $.pnotify(opts);
            },
            removeAll: function () {
                $.pnotify_remove_all();
            }
        };
    })
    .factory('$activityManager', [function () {
        var activityListeners = [];
        var keyedActivityListeners = {};
        var runningKeys = [];

        var startActivity = function () {
        };
        var stopActivity = function () {
            while (activityListeners.length > 0) {
                if (activityListeners[0]) {
                    activityListeners[0]();
                }
                activityListeners.shift();
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
                keyedActivityListeners[key].push({ 'onStartCallback': start, 'onStopCallback': stop, 'id': id });
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
    .factory('$feesService', function () {
        var feesService = {
            CalculateFees: function (calculationRequest, $queryManager, $activityManager, $capitecQueries, done) {
                if (!calculationRequest) { done('Please provide valid information for the application'); return; };
                var offerType = 7;
                if (calculationRequest.calculator == 'switch') {
                    offerType = 6;
                }

                var query = new $capitecQueries.GetCalculatorFeeQuery(offerType, calculationRequest.loanAmount, calculationRequest.cashOut);
                $queryManager.sendQueryAsync(query, undefined, undefined, undefined, true).then(function (data) {
                    if (data.data.ReturnData.Results.$values.length === 1) {
                        var calculationResult = calculationRequest;
                        calculationResult = calculationRequest;
                        calculationResult.interestRate = calculationRequest.interestRate;
                        calculationResult.cancellationFee = 0;
                        calculationResult.initiationFee = 0;
                        calculationResult.registrationFee = 0;
                        calculationResult.cancellationFee = data.data.ReturnData.Results.$values[0].CancellationFee.toFixed(2);
                        calculationResult.initiationFee = data.data.ReturnData.Results.$values[0].InitiationFee.toFixed(2);
                        calculationResult.registrationFee = data.data.ReturnData.Results.$values[0].RegistrationFee.toFixed(2);
                        calculationResult.InterimInterest = data.data.ReturnData.Results.$values[0].InterimInterest.toFixed(2);
                        calculationResult.total = (parseFloat(calculationResult.cancellationFee) + parseFloat(calculationResult.initiationFee) + parseFloat(calculationResult.registrationFee)).toFixed(2);

                        done(null, calculationResult);
                    } else {
                        done('The fees could not be calculated.');
                    }
                }, function (errorMessage) {
                    done('The fees could not be calculated. Please try again later.');
                });
            }
        };

        return feesService;
    })
    .factory('$queryOptionsService', function () {
        var service = {
            setPaginationOptions: function ($scope, pageSize, currentPage, totalPages, totalResults) {
                $scope.paginationOptions =
                {
                    'pageSize': pageSize,
                    'currentPage': currentPage,
                    'totalPages': totalPages,
                    'totalResults': totalResults
                }
            },
            setFilterOptions: function ($scope, filterDescription, filterOn, filterValue) {
                $scope.filterOptions =
                    {
                        'filterDescription': filterDescription,
                        'filterOn': filterOn,
                        'filterValue': filterValue
                    }
            },
            setSortOptions: function ($scope, orderBy, sortDirection) {
                $scope.sortOptions =
                    {
                        'orderBy': orderBy,
                        'sortDirection': sortDirection
                    }
            }
        };

        return service;
    })
    .factory('$cookieService', function () {
        return {
            getItem: function (sKey) {
                return decodeURIComponent(document.cookie.replace(new RegExp('(?:(?:^|.*;)\\s*' + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, '\\$&') + '\\s*\\=\\s*([^;]*).*$)|^.*$'), '$1')) || '';
            },
            setItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {
                if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/i.test(sKey)) { return false; }
                var sExpires = '';
                if (vEnd) {
                    switch (vEnd.constructor) {
                        case Number:
                            sExpires = vEnd === Infinity ? '; expires=Fri, 31 Dec 9999 23:59:59 GMT' : '; max-age=' + vEnd;
                            break;
                        case String:
                            sExpires = '; expires=' + vEnd;
                            break;
                        case Date:
                            sExpires = '; expires=' + vEnd.toUTCString();
                            break;
                    }
                }
                document.cookie = encodeURIComponent(sKey) + '=' + encodeURIComponent(sValue) + sExpires + (sDomain ? '; domain=' + sDomain : '') + (sPath ? '; path=' + sPath : '') + (bSecure ? '; secure' : '');
                return true;
            },
            removeItem: function (sKey, sPath, sDomain) {
                if (!sKey || !this.hasItem(sKey)) { return false; }
                document.cookie = encodeURIComponent(sKey) + '=; expires=Thu, 01 Jan 1970 00:00:00 GMT' + (sDomain ? '; domain=' + sDomain : '') + (sPath ? '; path=' + sPath : '');
                return true;
            },
            hasItem: function (sKey) {
                return (new RegExp('(?:^|;\\s*)' + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, '\\$&') + '\\s*\\=')).test(document.cookie);
            },
            keys: function () {
                var aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, '').split(/\s*(?:\=[^;]*)?;\s*/);
                for (var nIdx = 0; nIdx < aKeys.length; nIdx++) { aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]); }
                return aKeys;
            }
        };
    })
.factory('$printingService', ['$rootScope', '$compile', '$window', '$q', '$timeout', function ($rootScope, $compile, $window, $q, $timeout) {
    var printScope = {};
    function print(printTemplate) {
        var deferred = $q.defer();
        $('html').append('<iframe id="printf" name="printf"></iframe>');
        var printWindow = $window.frames['printf'];
        printWindow.document.write(printTemplate);

        var printContents = $(printWindow.document).find('#printContents');
        var printElement = angular.element(printContents);
        var compiled = $compile(printElement.contents());
        var scoped = compiled(printScope);
        printScope.$$phase || printScope.$apply();

        $timeout(function () {
            printWindow.focus();
            printWindow.document.execCommand('print', false, null);
            printWindow.close();
            $('iframe#printf').remove();
        }, 500);
    }
    function createPrintTemplate(template) {
        return '<!DOCTYPE html><html><head><title></title></head><body><div id="printContents">'
                + template
                + '</div></body></html>';
    }
    function createPrintTemplateFromUrl(templateUrl) {
        return '<!DOCTYPE html>'
                + '<html><head>'
                + '<title></title></head><body>'
                + '<div id="printContents"><div ng-include="\'' + templateUrl + '\'"></div>'
                + '</div></body></html>';
    }
    return {
        printFromTemplateUrl: function (templateUrl, printData) {
            var printTemplate = createPrintTemplateFromUrl(templateUrl);
            printScope = $rootScope.$new();
            printScope.printData = printData;
            print(printTemplate);
        }
    }
}]);