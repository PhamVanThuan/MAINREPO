angular.module('capitecApp.home.apply.client-capture', ['capitecApp.services', 'SAHL.Services.Interfaces.Capitec.sharedmodels', 'sahl.core.app.services'])

.controller('clientCaptureCtrl', ['$rootScope', '$scope', '$state', '$commandManager', '$capitecCommands', '$queryManager', '$capitecQueries', '$notificationService', '$capitecSharedModels', '$validationManager', '$calculatorDataService', '$validation', '$activityManager', '$serialization', '$templateCache', '$timeout',
    function ClientCaptureController($rootScope, $scope, $state, $commandManager, $capitecCommands, $queryManager, $capitecQueries, $notificationService, $capitecSharedModels, $validationManager, $calculatorDataService, $validation, $activityManager, $serialization, $templateCache, $timeout) {
        var self = this;
        $rootScope.applicantsValidSuburbs = [];

        var initialiseController = function () {
            self.initialiseFocusItems();
            self.initialiseScope();
            initializeApplication();
            setUpDateOfBirthWatches();
        };

        var numberOfApplicants = $serialization.deserialize(angular.fromJson($calculatorDataService.getDataValue('NumberOfApplicants')));

        function Wizard(steps, state, scope, validationManager) {
            var _this = {};
            _this.state = state;
            _this.scope = scope;
            _this.steps = steps;
            _this.validationManager = validationManager;
            _this.currentStepIndex = 0;

            return {
                next: function (dismiss) {
                    
                    if (_this.currentStepIndex === 1) //address details
                    {
                        /** cater for when an initially valid suburb is overwriten by an invalid one **/
                        var validSuburb = $rootScope.applicantsValidSuburbs[$scope.currentApplicant];
                        var currentSuburb = $scope.application.applicants[$scope.currentApplicant].residentialAddress.suburb;
                        if (currentSuburb && (currentSuburb !== validSuburb)) {
                            $notificationService.notifyError('Please enter a valid Suburb');
                            return;
                        }
                    }

                    if (_this.steps[_this.currentStepIndex].validator(_this)) {
                        if (this.isLastStep()) {
                            dismiss();
                        } else {
                            _this.currentStepIndex++;
                            _this.state.current.data.pageHeading = this.getCurrentStepTitle();
                            _this.state.transitionTo(_this.state.current.data.next);
                        }
                    }
                },
                previous: function () {
                    _this.currentStepIndex--;
                    $state.transitionTo($state.current.data.previous);
                },
                setStep: function (stepIndex) {
                    _this.currentStepIndex = stepIndex;
                    _this.state.current.data.pageHeading = this.getCurrentStepTitle();
                    setFocusForElement();
                },
                getCurrentStepTitle: function () {
                    var title = _this.steps[_this.currentStepIndex].title;
                    if (typeof title == 'function') {
                        return title(_this);
                    }
                    return title;
                },
                getCurrentStep: function () {
                    return _this.steps[_this.currentStepIndex].step;
                },
                isLastStep: function () {
                    return _this.steps.length - 1 === _this.currentStepIndex;
                },
                isFirstStep: function () {
                    return _this.currentStepIndex === 0;
                }
            };
        }

        /** Initialises the scope of the page on first entry  **/
        self.initialiseScope = function () {
            $scope.searchText = '';
            $scope.minBirthYear = new Date().getFullYear() - 100;
            $scope.maxBirthYear = new Date().getFullYear() - 1;
            $scope.isConfirmingDeletion = false;
            $scope.printTemplate = './app/home/content/apply/shared/client-capture/consent-form.tpl.html';
            $scope.application = $serialization.deserialize(angular.fromJson($calculatorDataService.getDataValue('application')));
            
            if (!$scope.application) {
                $state.transitionTo('home.content.apply');
            }
        };

        /** Initialises the scope focus items to false  **/
        self.initialiseFocusItems = function () {
            $scope.model = {
                identityNumberFocus: false,
                unitNumber: false,
                employmentTypeFocus: false,
                incomeContributorFocus: false
            };
        };

        /** Sets the focus on the first item as the user enters the form
        ** param {index}
        **/
        var setFocusForElement = function () {
            $timeout(function () {
                self.removeLastFocus();
                switch ($scope.wizard.getCurrentStep()) {
                    case 'one':
                        $scope.model.identityNumberFocus = true;
                        break;
                    case 'two':
                        $scope.model.unitNumber = true;
                        break;
                    case 'three':
                        $scope.model.employmentTypeFocus = true;
                        break;
                    case 'four':
                        $scope.model.incomeContributorFocus = true;
                        break;
                    default:
                        break;
                }
            });
        };

        /** Removes the last set focus of an item by setting to false. **/
        self.removeLastFocus = function () {
            $scope.model.identityNumberFocus = false;
            $scope.model.unitNumber = false;
            $scope.model.employmentTypeFocus = false;
            $scope.model.incomeContributorFocus = false;
        };

        /**
       ** Ensure that DOB is empty string so that it doesn't trigger the validator
       **/
        $timeout(function () {
            var dob = $scope.getCurrentApplicant().information.$dateOfBirthObject;
            var applicant = $scope.getCurrentApplicant();
            if (dob.day == '' && dob.month == '' && dob.year == '') {
                applicant.information.dateOfBirth = '';
            }
        });

        function setUpDateOfBirthWatches() {
            var dateOfBirthObjectWatch = $scope.$watch(function () {
                return $scope.getCurrentApplicant().information.$dateOfBirthObject;
            }, function (newValue, oldValue) {
                var applicant = $scope.getCurrentApplicant();
                var dob = $scope.getCurrentApplicant().information.$dateOfBirthObject;

                if (dob && dob.year && dob.month && dob.day) {
                    applicant.information.dateOfBirth = dob.year + '-' + dob.month + '-' + dob.day;
                }
            }, true);

            var dateOfBirthWatch = $scope.$watch(function () {
                return $scope.getCurrentApplicant().information.dateOfBirth;
            }, function () {
                if ($scope.getCurrentApplicant().information.dateOfBirth) {
                    var dateParts = $scope.getCurrentApplicant().information.dateOfBirth.split('-');
                    $scope.getCurrentApplicant().information.$dateOfBirthObject = {
                        year: dateParts[0],
                        month: dateParts[1],
                        day: dateParts[2]
                    };
                } else {
                    $scope.getCurrentApplicant().information.dateOfBirth = '';
                }

            }, true);

            $scope.$on('$destroy', function() {
                if(dateOfBirthWatch) { dateOfBirthWatch(); }
                if(dateOfBirthObjectWatch) { dateOfBirthObjectWatch(); }
            });
        }

        $scope.$on('$stateChangeSuccess', function () {
            $scope.wizard.setStep($state.current.data.step);
        });

           var baseUrl = (($state.current.name.match('for-switch') != null) ? 'home.content.apply.application-precheck-for-switch.switch.' : 'home.content.apply.application-precheck-for-new-home.new-home.');

        $scope.checkMainContactSelectedForApplication = function () {
            if ($scope.application.applicants[$scope.currentApplicant].information.mainContact == true) {
                var numberOfMainContacts = 0;
                for (var i = 0; i < $scope.application.applicants.length; i++) {
                    if ($scope.application.applicants[i].information.mainContact == true) {
                        numberOfMainContacts++;
                    }
                }
                if (numberOfMainContacts > 1) {
                    $scope.application.applicants[$scope.currentApplicant].information.mainContact = false;
                    $notificationService.notifyError('Only one applicant can be selected as the main contact on this application.');
                }
            }
        };

        function isIdentityNumberValidForCurrentApplicant() {
            if (!($scope.application.applicants[$scope.currentApplicant].information.identityNumber === null)) {
                var idNumberToValidate = $scope.application.applicants[$scope.currentApplicant].information.identityNumber;
                var isTheCorrectNumberOfDigits = idNumberToValidate.match(/\d{13}/);
                if(!isTheCorrectNumberOfDigits){
                    return false;
                }
                var isValidIDNumber = validateIDNumber(idNumberToValidate);
                if(!isValidIDNumber){
                    var errorMessage = 'Please provide a valid ID Number.';
                    $notificationService.notifySticky(errorMessage);
                    $scope.application.applicants[$scope.currentApplicant].errorValidationMessages.push(errorMessage);
                    return false;
                }else{
                    $notificationService.removeAll();
                    return true;
                }
            }
            return false;
        }

        $scope.checkExistingApplicationsForIdentityNumber = function () {
            $notificationService.removeAll();
            if (isIdentityNumberValidForCurrentApplicant()) {               
                var identityNumber = $scope.application.applicants[$scope.currentApplicant].information.identityNumber;
                var query = new $capitecQueries.GetApplicationByIdentityNumberQuery(identityNumber, 'In Progress');
                $queryManager.sendQueryAsync(query).then(function (data) {
                    if (data.data.ReturnData.Results.$values.length > 0) {
                        var errorMessage = "Application " + data.data.ReturnData.Results.$values[0].ApplicationNumber + " exists for applicant with identity number " + identityNumber + ".";
                        $scope.application.applicants[$scope.currentApplicant].errorValidationMessages.push(errorMessage);
                        $notificationService.notifySticky(errorMessage);
                    }
                    else {
                        $scope.application.applicants[$scope.currentApplicant].errorValidationMessages = [];
                        $notificationService.removeAll();
                    }
                });
                updateDateOfBirthToIdentityNumber();                
            }
        };

        function updateDateOfBirthToIdentityNumber() {
            var century = '19';
            var idNumber = $scope.application.applicants[$scope.currentApplicant].information.identityNumber;
            var year = century + idNumber.slice(0, 2);
            var month = idNumber.slice(2, 4);
            var day = idNumber.slice(4, 6);
            $scope.application.applicants[$scope.currentApplicant].information.$dateOfBirthObject = {
                day: day,
                month: month,
                year: year
            };
        }

        var searchTextWatch = $scope.$watch('searchText', function () {
            if ($scope.searchText.SuburbName) {
                $scope.application.applicants[$scope.currentApplicant].residentialAddress.suburb = $scope.searchText.SuburbName;
            } else {
                if ($scope.application.applicants[$scope.currentApplicant].residentialAddress.suburb) {
                    $scope.searchText = {};
                    $scope.searchText.SuburbName = $scope.application.applicants[$scope.currentApplicant].residentialAddress.suburb;
                    $scope.searchText.Id = $scope.application.applicants[$scope.currentApplicant].residentialAddress.suburbId;
                }
            }
            if ($scope.searchText.Id) {
                var query = new $capitecQueries.GetSuburbProvinceQuery($scope.searchText.Id);
                $queryManager.sendQueryAsync(query).then(searchTextOnSucess, searchTextOnFailure);
            }
            else {
                $scope.isKnownDatabaseSuburb = false;
            }
        });

        var applicationWatch = $scope.$watch('application', function () {
        
            $calculatorDataService.addData('application', $scope.application);
        }, true);

        $scope.$on('$destroy', function() {
            if(searchTextWatch) { searchTextWatch(); }
            if(applicationWatch) {applicationWatch(); }
        });

        function searchTextOnSucess(data) {
            
            if (data.data.ReturnData.Results.$values.length > 0) {
                $scope.isKnownDatabaseSuburb = true;
                $scope.application.applicants[$scope.currentApplicant].residentialAddress.suburbId = $scope.searchText.Id;
                $scope.application.applicants[$scope.currentApplicant].residentialAddress.city = data.data.ReturnData.Results.$values[0].CityName;
                $scope.application.applicants[$scope.currentApplicant].residentialAddress.province = data.data.ReturnData.Results.$values[0].ProvinceName;
                if (data.data.ReturnData.Results.$values[0].PostalCode) {
                    $scope.application.applicants[$scope.currentApplicant].residentialAddress.postalCode = data.data.ReturnData.Results.$values[0].PostalCode;
                }

                $rootScope.applicantsValidSuburbs = [];
                $rootScope.applicantsValidSuburbs[$scope.currentApplicant] = $scope.application.applicants[$scope.currentApplicant].residentialAddress.suburb;
            }
        }

        function searchTextOnFailure(result) {
            $scope.isKnownDatabaseSuburb = false;
        }

        $scope.wizard = Wizard([
             { step: 'one', title: 'please enter personal information', validator: function (_this) { return _this.validationManager.Validate(_this.scope.getCurrentApplicant().information); } },
             {
                 step: 'two', title: 'please enter your current residential address', validator: function (_this) {
                     return _this.validationManager.Validate(_this.scope.getCurrentApplicant().residentialAddress);
                 }
             },

             {
                 step: 'three', title: 'please enter employment details', validator: function (_this) {
                     var currentApplicant = _this.scope.getCurrentApplicant();

                     var employmentTypeId = currentApplicant.employmentDetails.employmentTypeEnumId;
                     if (!employmentTypeId) {
                         return $notificationService.notifyError('An employment type is required');
                     }

                     return _this.validationManager.Validate(_this.scope.getCurrentApplicant().employmentDetails.incomeDetail);
                 }
             },
             {
                 step: 'four', title: function (_this) {
                     var currentApplicant = _this.scope.getCurrentApplicant();
                     if (currentApplicant.information.salutationEnumId && currentApplicant.information.surname) {
                         var title = _this.scope.getLookupName(currentApplicant.information.salutationEnumId, _this.scope.salutationLookup);
                         return $.trim('declarations for ' + (title || ' ') + ' ' + (currentApplicant.information.firstName || ' ') + ' ' + (currentApplicant.information.surname || ' '));
                     }
                     return 'declarations for current applicant';
                 }, validator: function (_this) { return _this.validationManager.Validate(_this.scope.getCurrentApplicant().declarations); }
             }],
             $state, $scope, $validationManager);

        $scope.validate = function () {
            if ($scope.application.applicants.length == 0) {
                return true;
            }
            var isValid = true;

            for (var i = 0; i < $scope.application.applicants.length; i++) {
                var modelsToValidate = new Array();
                var applicant = $scope.application.applicants[i];
                var applicantName = (applicant.information.firstName || ' ') + ' ' + (applicant.information.surname || ' ');
                
                if (applicant.information) {
                    modelsToValidate.push(applicant.information);
                }
                if (applicant.residentialAddress) {
                    modelsToValidate.push(applicant.residentialAddress);
                }
                if (applicant.employmentDetails) {
                    modelsToValidate.push(applicant.employmentDetails);
                }
                if (applicant.employmentDetails.incomeDetail) {
                    modelsToValidate.push(applicant.employmentDetails.incomeDetail);
                }
                if (applicant.declarations) {
                    modelsToValidate.push(applicant.declarations);
                }

                var validationMessage = 'Validation errors for ' + ($.trim(applicantName).length == 0 ? 'Applicant ' + (i + 1) : applicantName);
                isValid &= $validationManager.ValidateModels(modelsToValidate, validationMessage);
            }
            return Boolean(isValid);
        };

        $scope.addApplicant = function () {
            /** Current rule is to only allow user to add 2 applicants. **/
            if ($scope.application.applicants.length < 2) {
                var newApplicant = new $capitecSharedModels.Applicant(
                new $capitecSharedModels.ApplicantInformation(),
                new $capitecSharedModels.ApplicantResidentialAddress(),
                new $capitecSharedModels.ApplicantEmploymentDetails(),
                new $capitecSharedModels.ApplicantDeclarations());

                newApplicant.errorValidationMessages = [];

                //initialise applicant data model variables
                newApplicant.information.mainContact = false;

                $scope.application.applicants.push(newApplicant);
            }
        };

        /** Determines if more then 2 applicants are added **/
        $scope.showAddApplicant = function () {
            return $scope.application.applicants.length < 2 ? true : false;
        };

        $scope.getCurrentApplicant = function () {
            return $scope.application.applicants[$scope.currentApplicant];
        };

        $queryManager.sendQueryAsync(new $capitecQueries.GetSalutationsQuery()).then(function (data) {
            $scope.salutationLookup = data.data.ReturnData.Results.$values;
        });
        $queryManager.sendQueryAsync(new $capitecQueries.GetEmploymentTypesQuery()).then(function (data) {
            $scope.employmentTypeLookup = data.data.ReturnData.Results.$values;
        });
        $queryManager.sendQueryAsync(new $capitecQueries.GetDeclarationTypesQuery()).then(function (data) {
            $scope.declarationAnswersLookup = data.data.ReturnData.Results.$values;
            $scope.application.applicants[$scope.currentApplicant].declarations.presentAtCapture = $scope.getLookupId('Yes', $scope.declarationAnswersLookup);
            
        });

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

        function initializeApplication() {
            var currentApplicantCount = $scope.application.applicants.length;
            if(currentApplicantCount < numberOfApplicants) {
                for(var i = 0; i < numberOfApplicants - currentApplicantCount; i++) {
                    $scope.addApplicant();
                }
            }
            setCurrentlySelectedApplicant();
            setDefaultMainContact();
        }

        function setDefaultMainContact() {
            var hasMainContact = false;
            angular.forEach($scope.application.applicants, function(applicant) {
                if(applicant.information.mainContact === true) {
                    hasMainContact = true;
                }
            });
            if(!hasMainContact) {
                $scope.application.applicants[0].information.mainContact = true;
            }
        }

        function setCurrentlySelectedApplicant() {
            $scope.currentApplicant = parseFloat($calculatorDataService.getDataValue('currentApplicant'));
            if(!$scope.currentApplicant || $scope.currentApplicant > $scope.application.applicants.length - 1) {
                $scope.currentApplicant = 0;
            }
            var currentApplicantWatch = $scope.$watch('currentApplicant', function() {
                $calculatorDataService.addData('currentApplicant', $scope.currentApplicant);
            });
            $scope.$on('$destroy', function() {
                currentApplicantWatch();
            });
        }

        function goToNextApplicant() {
            $scope.currentApplicant = $scope.currentApplicant + 1;
            $state.transitionTo(baseUrl + 'client-capture.personal');

            getValidSuburb($scope.currentApplicant);
        }

        $scope.submitApplication = function () {
            // If this is not the last applicant, got to the next applicant.
            if($scope.currentApplicant!=$scope.application.applicants.length -1){
                var currentApplicant = $scope.application.applicants[$scope.currentApplicant];

                if ($validation.compare(currentApplicant.declarations.allowCreditBureauCheck, $scope.declarationAnswersLookup, 'No')) {
                    $notificationService.notifyError('If an applicant is present at the time of capture, SA Home Loans requires their permission prior to performing a Credit Bureau Check.');
                } else {
                    goToNextApplicant();
                }
                $activityManager.stopActivity();
                return;
            }

            // model validation
            if ($scope.validate() === false) {
                $activityManager.stopActivity();
                return;
            }
            // custom validation
            var isValid = true;
            var needToAddApplicant = false;
            var applicationHasIncomeContributingApplicant = false;
            var applicationHasMainContact = false;         
            
            for (var i = 0; i < $scope.application.applicants.length; i++) {
             
                //apply defaults
                $scope.application.applicants[i].information.title = $scope.getLookupName($scope.application.applicants[i].information.salutationEnumId, $scope.salutationLookup);

                //check if can do an ITC for all applicants
                if ($validation.compare($scope.application.applicants[i].declarations.allowCreditBureauCheck, $scope.declarationAnswersLookup, 'No')) {
                    $notificationService.notifyError('If an applicant is present at the time of capture, SA Home Loans requires their permission prior to performing a Credit Bureau Check.');
                    isValid = false;
                }

                // check number of applicants to capture
                if($scope.application.applicants.length < numberOfApplicants) {
                    needToAddApplicant = true;
                    isValid = false;
                }

                /** check if the user has selected a valid suburb from the dropdown **/
                if (!$scope.isKnownDatabaseSuburb) {
                    $notificationService.notifyError('Please enter a valid suburb in the residential address detail.');
                    isValid = false;
                    $activityManager.stopActivity();
                    return;
                }

                // check married COP - add another applicant
                if ($validation.compare($scope.application.applicants[i].declarations.marriedInCommunityOfProperty, $scope.declarationAnswersLookup, 'Yes')) {
                    if ($scope.application.applicants.length == 1) {
                        $notificationService.notifyInfo('Please capture spouse details for applicants that are Married in Community of Property.');
                        needToAddApplicant = true;
                        isValid = false;
                    }
                }

                // check if there is an income contributor - add another applicant
                if ($validation.compare($scope.application.applicants[i].declarations.incomeContributor, $scope.declarationAnswersLookup, 'No')) {
                    if ($scope.application.applicants.length == 1) {
                        $notificationService.notifyInfo('Please capture details for an applicant that is an income contributor.');
                        needToAddApplicant = true;
                        isValid = false;
                    }
                }

                // check application contains at least one income contributor
                if ($validation.compare($scope.application.applicants[i].declarations.incomeContributor, $scope.declarationAnswersLookup, 'Yes')) {
                    applicationHasIncomeContributingApplicant = true;
                }

                // check if application has a main contact
                if ($scope.application.applicants[i].information.mainContact === true) {
                    applicationHasMainContact = true;
                }
            }

            if ($scope.application.applicants.length == 2) {
                if ($scope.application.applicants[0].information.identityNumber === $scope.application.applicants[1].information.identityNumber) {
                    $notificationService.notifyError('The application requires that the identity numbers be different if two applicants or more are applying.');
                    isValid = false;
                }
            }

            if (applicationHasIncomeContributingApplicant === false) {
                $notificationService.notifyError('The application requires that at least one applicant is an income contributor.');
                isValid = false;
            }

            if (applicationHasMainContact === false) {
                $notificationService.notifyError('The application requires that one applicant must be selected as the main contact.');
                isValid = false;
            }

            if (needToAddApplicant === true) {
                $scope.addApplicant();
                goToNextApplicant();
                $activityManager.stopActivity();
                return;
            }

            if (isValid === false) {
                $activityManager.stopActivity();
                return;
            }

            for (var i = 0; i < $scope.application.applicants.length; i++) {

                var applicant = $scope.application.applicants[i];
                var employmentTypeEnumId = applicant.employmentDetails.employmentTypeEnumId;
                var incomeDetail = applicant.employmentDetails.incomeDetail;
                var employmentDetails = {
                    incomeDetail: incomeDetail,
                    employmentTypeEnumId: employmentTypeEnumId
                };
                switch (incomeDetail.employmentTypeName) {
                    case 'Salaried':
                        employmentDetails['salariedDetails'] = incomeDetail;
                        break;
                    case 'Self Employed':
                        employmentDetails['selfEmployedDetails'] = incomeDetail;
                        break;
                    case 'Salaried with Housing Allowance':
                        employmentDetails['salariedWithHousingAllowanceDetails'] = incomeDetail;
                        break;
                    case 'Salaried with Commission':
                        employmentDetails['salariedWithCommissionDetails'] = incomeDetail;
                        break;
                    case 'Unemployed':
                        employmentDetails['unEmployedDetails'] = incomeDetail;
                        break;
                    default:
                        break;
                }
                applicant.employmentDetails = employmentDetails;
            }
            $calculatorDataService.addData('application', $scope.application);

            var data = {};
            $state.transitionTo($state.current.data.next, data);
        };

        $scope.isClientPresentAtCapture = function () {
            if (!$scope.getCurrentApplicant().declarations.presentAtCapture) {
                return true;
            }
            return $validation.compare($scope.getCurrentApplicant().declarations.presentAtCapture, $scope.declarationAnswersLookup, 'Yes');
        };

        $scope.requiresItcPermission = function () {
            if ($validation.compare($scope.getCurrentApplicant().declarations.allowCreditBureauCheck, $scope.declarationAnswersLookup, 'No')) {
                return true;
            }
            return false;
        };

        $scope.selectApplicant = function ($index) {
            getValidSuburb($index);
            
            $state.transitionTo(baseUrl + 'client-capture.personal');
        };

        function getValidSuburb(applicantIndex) {
            $scope.currentApplicant = applicantIndex;
            if ($scope.application.applicants[$scope.currentApplicant].residentialAddress.suburb) {
                $rootScope.applicantsValidSuburbs[$scope.currentApplicant] = $scope.application.applicants[$scope.currentApplicant].residentialAddress.suburb;
            }
        }

        $scope.notFirstApplicant = function () {
            return (($scope.application.applicants.length > 1) && ($scope.currentApplicant > 0));
        };

        $scope.getNextLabel = function () {
            return ($scope.wizard.isLastStep()) ? 'Submit' : 'Next';
        };

        $scope.onClickClientInfo = function () {
            $state.transitionTo(baseUrl + 'client-capture.personal');
        };

        $scope.onClickClientAddress = function () {
            $state.transitionTo(baseUrl + 'client-capture.address');
        };

        $scope.onClickClientFinancials = function () {
            $state.transitionTo(baseUrl + 'client-capture.employment');
        };

        $scope.onClickClientDeclarations = function () {
            $state.transitionTo(baseUrl + 'client-capture.declarations');
        };

        $scope.confirm = function () {
            $scope.isConfirmingDeletion = true;
        };

        $scope.onClickCancel = function () {
            $scope.isConfirmingDeletion = false;
        };

        $scope.defaultToPreviousCapturedAddress = function ($event) {
            if ($event.target.checked) {
                $scope.getCurrentApplicant().residentialAddress = $scope.application.applicants[$scope.application.applicants.length - 2].residentialAddress;
                $rootScope.applicantsValidSuburbs[$scope.currentApplicant] = $scope.application.applicants[$scope.application.applicants.length - 2].residentialAddress.suburb;
            } else {
                $scope.getCurrentApplicant().residentialAddress = new $capitecSharedModels.ApplicantResidentialAddress();
            }
        };

        /** Determines if more then 2 applicants are added **/
        $scope.showAddApplicant = function () {
            if ($scope.application.applicants.length < 2) {
                return true;
            } else {
                return false;
            }
        };

        $scope.onClickRemoveClient = function () {
            $scope.isConfirmingDeletion = false;
            numberOfApplicants = $scope.application.applicants.length;
            if (numberOfApplicants > 1) {
                var currentApplicant = $scope.application.applicants[$scope.currentApplicant];
                var index = $.inArray(currentApplicant, $scope.application.applicants);
                if (index > -1) {
                    $scope.application.applicants.splice(index, 1);
                    $scope.selectApplicant($scope.application.applicants.length - 1);
                }
                numberOfApplicants --;
                $calculatorDataService.addData('NumberOfApplicants', numberOfApplicants);
            }
            else {
                $notificationService.notifyInfo('The application requires at least one applicant.');
            }
        };

        $scope.onClickConfirmRemoveClient = function () {
            $scope.isConfirmingDeletion = false;
        };


        $scope.currentlySelectedEmployment = function () {
            return $scope.getLookupName($scope.getCurrentApplicant().employmentDetails.employmentTypeEnumId, $scope.employmentTypeLookup);
        };

        $scope.changeCurrentApplicantEmployment = function (employmentTypeName) {

            var currentCapturedGrossIncome = 0;
            if ($scope.getCurrentApplicant().employmentDetails.incomeDetail) {
                currentCapturedGrossIncome = $scope.getCurrentApplicant().employmentDetails.incomeDetail.grossMonthlyIncome;
            }

            switch (employmentTypeName) {
                case 'Salaried':
                    $scope.getCurrentApplicant().employmentDetails.incomeDetail = new $capitecSharedModels.SalariedDetails(currentCapturedGrossIncome || null);
                    break;
                case 'Self Employed':
                    $scope.getCurrentApplicant().employmentDetails.incomeDetail = new $capitecSharedModels.SelfEmployedDetails(currentCapturedGrossIncome || null, null);
                    break;
                case 'Salaried with Housing Allowance':
                    $scope.getCurrentApplicant().employmentDetails.incomeDetail = new $capitecSharedModels.SalariedWithHousingAllowanceDetails(currentCapturedGrossIncome || null, null);
                    break;
                case 'Salaried with Commission':
                    $scope.getCurrentApplicant().employmentDetails.incomeDetail = new $capitecSharedModels.SalariedWithCommissionDetails(currentCapturedGrossIncome || null, null);
                    break;
                case 'Unemployed':
                    $scope.getCurrentApplicant().employmentDetails.incomeDetail = new $capitecSharedModels.UnEmployedDetails(0); 
                    break;
                default:
                    break;
            }

            $scope.getCurrentApplicant().employmentDetails.incomeDetail.employmentTypeName = employmentTypeName;
        };

        function DropDown(name) {
            var spanname = name + $scope.currentApplicant + 'Span';

            var dropdown = document.getElementById(name + $scope.currentApplicant);
            var span = document.getElementById(spanname);

            if (span.innerText) {
                span.innerText = dropdown[dropdown.selectedIndex].text;
            } else {
                span.textContent = dropdown[dropdown.selectedIndex].text;
            }
        }

        $scope.onEmploymentTypeChanged = function () {
            var selectedEmploymentType = $scope.getLookupName($scope.getCurrentApplicant().employmentDetails.employmentTypeEnumId, $scope.employmentTypeLookup);

            $scope.changeCurrentApplicantEmployment(selectedEmploymentType);

            //DropDown('employmentTypeEnumId');
        };
        $scope.disableSubmitbutton = function () {
            return $scope.enableSubmitButton;
        };

        $scope.getSubmitLabel = function () {
            var canSubmit = true;
            // check if we have captured the total number of applicants
            if($scope.application.applicants.length < numberOfApplicants || $scope.currentApplicant != $scope.application.applicants.length - 1) {
                canSubmit = false;
            } else {
                for (var i = 0; i < $scope.application.applicants.length; i++) {
                    // check married COP - add another applicant
                    if ($validation.compare($scope.application.applicants[i].declarations.marriedInCommunityOfProperty, $scope.declarationAnswersLookup, 'Yes')) {
                        if ($scope.application.applicants.length == 1) {
                            canSubmit = false;
                            break;
                        }
                    }

                    // check if there is an income contributor - add another applicant
                    if ($validation.compare($scope.application.applicants[i].declarations.incomeContributor, $scope.declarationAnswersLookup, 'No')) {
                        if ($scope.application.applicants.length == 1) {
                            canSubmit = false;
                            break;
                        }
                    }
                }
            }
            return (canSubmit === true) ? 'Print Consent Form' : 'Next Applicant';
        };

        $scope.enableSubmitButton = function () {
            for (var i = 0; i < $scope.application.applicants.length; i++) {
                if ($scope.application.applicants[i].errorValidationMessages.length > 0) {
                    return false;
                }
            }
            return true;
        };

        function validateIDNumber(idNumber) {
            var correct = true;
            // SA ID Number have to be 13 digits, so check the length
            if (idNumber.length != 13 || !isNumber(idNumber)) {
                correct = false;
            }

            // get first 6 digits as a valid date
            var tempDate = new Date(idNumber.substring(0, 2), idNumber.substring(2, 4) - 1, idNumber.substring(4, 6));

            var id_date = tempDate.getDate();
            var id_month = tempDate.getMonth();
            var id_year = tempDate.getFullYear();

            var fullDate = id_date + "-" + (id_month + 1) + "-" + id_year;

            if (!((tempDate.getYear() == idNumber.substring(0, 2)) && (id_month == idNumber.substring(2, 4) - 1) && (id_date == idNumber.substring(4, 6)))) {
                correct = false;
            }

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
                correct = false;
            }

            return correct;
        }

        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        return initialiseController();
    }]);