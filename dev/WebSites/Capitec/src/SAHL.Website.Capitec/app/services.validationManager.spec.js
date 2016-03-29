describe("[capitecApp]", function () {
    beforeEach(module('capitecApp.services'));

    describe(' Capitec Validation Manager Service ', function () {
        var $validationManager, ValidationForm ,$notificationService,toValidate;
      
        beforeEach(inject(function ($injector) {
            $validationManager = $injector.get('$validationManager');
            $notificationService = $injector.get('$notificationService');
            ValidationForm = $injector.get('ValidationForm');           

        }));

        describe('when calling Validate on the validationManager', function() {

            beforeEach(function() {
                toValidate = {};
                toValidate.Validate =  function() { 
                    return [{
                        prop1:'This is a validation message'
                    }];
                };
            });

            it('should call the provided validation function', function() {
                var validated = $validationManager.Validate(toValidate);
                expect(validated).toBe(true);
            });

            it('should provide a notification if validation fails', function() {
                ValidationForm.Form = {
                    $valid: false,
                    $invalid: true,
                    prop1: {
                        $setValidity:function(){},
                        $setPristine:function(){},
                    }
                };

                spyOn(ValidationForm,'isEmpty').and.returnValue(false);
                spyOn($notificationService,'notifyError').and.callThrough();
               
                $validationManager.Validate(toValidate);

                expect($notificationService.notifyError).toHaveBeenCalled();
            });

            it('should not provide a notification if validation passes', function() {
                ValidationForm.Form = {
                    $valid: true,
                    $invalid: false,
                    prop1: {
                        $setValidity:function(){},
                        $setPristine:function(){},
                    }
                };

                spyOn(ValidationForm,'isEmpty').and.returnValue(false);
                spyOn($notificationService,'notifyError').and.callThrough();
               
                $validationManager.Validate(function() { 
                    return [{}];
                });

                expect($notificationService.notifyError).not.toHaveBeenCalled();
            });

            it('should be successful when validate on command or query is not required', function() {
                toValidate.Validate = undefined;
                var validated = $validationManager.Validate(toValidate);
                expect(validated).toBe(true);
            });            
        });

        describe('when calling validationOnly  on the validationManager', function() {
             
            beforeEach(function() {
                toValidate = {};
                toValidate.Validate =  function() { 
                    return [];
                };
            });

            it('should call the provided validation function', function() {
                var validated = $validationManager.ValidateOnly(toValidate);
                expect(validated).toBe(true);
            });

            it('should provide a notification if validation fails', function() {
                toValidate.Validate =  function() { 
                    return [{
                        prop1 : 'This is a validation message'
                    }];
                };
                spyOn($notificationService,'notifyError').and.callThrough();
                $validationManager.ValidateOnly(toValidate);
                expect($notificationService.notifyError).toHaveBeenCalled();
            });

            it('should not provide a notification if validation passes', function() {

                spyOn($notificationService,'notifyError').and.callThrough();
                $validationManager.ValidateOnly(function() { 
                    return [{
                        prop1 : 'This is a validation message'
                    }];
                });

                expect($notificationService.notifyError).not.toHaveBeenCalled();
            });

            it('should be successful when validate on command or query is not required', function() {
                toValidate.Validate = undefined;
                var validated = $validationManager.ValidateOnly(toValidate);
                expect(validated).toBe(true);
            }); 
        });

        describe('when calling ValidateModels on the validationManager', function() {
            var modelsToValidate = [];

            function FakeApplicant(name, number){
                this.name = name;
                this.number = number;
                this.Validate = function(){
                    var results = [];
                    if(isNaN(parseFloat(this.number))) {
                        results.push({'number': 'Number must be a number.'});
                    }
                    if(!this.name) {
                        results.push({'name': 'Name is required.'});
                    }
                    return results;
                };
            }

            afterEach(function() {
                modelsToValidate = [];
            });
            it('should return true if the model is valid', function() {
                modelsToValidate.push(new FakeApplicant('Bob Jones', 34));
                var valid = $validationManager.ValidateModels(modelsToValidate);
                expect(valid).toBe(true);
            });
            it('should return false if the model is invalid', function() {
                modelsToValidate.push(new FakeApplicant('', 'aaa'));
                var valid = $validationManager.ValidateModels(modelsToValidate);
                expect(valid).toBe(false);
            });
            it('should call the notification service with the validation message', function() {
                spyOn($notificationService, 'notifyError').and.callThrough();
                modelsToValidate.push(new FakeApplicant('Bob Jones', 'aaa'));
                $validationManager.ValidateModels(modelsToValidate);
                expect($notificationService.notifyError).toHaveBeenCalledWith('Validation errors', 
                    '<span>There were errors on the form : </span><ul><li>Number must be a number.</li></ul>');
            });
        });
    });
});