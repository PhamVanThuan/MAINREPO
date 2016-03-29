describe("[capitecApp]", function () {
    beforeEach(module('capitecApp'));
    
    var $httpBackend, $rootScope;

    beforeEach(inject(function ($injector, $q) {
        // get the root scope
        $rootScope = $injector.get('$rootScope');
        $httpBackend = $injector.get('$httpBackend');
        $rootScope.authenticated = true;
        $httpBackend.whenGET('./app/home/home.tpl.html').respond({});
        $httpBackend.whenGET('./app/home/splashscreen/splashscreen.tpl.html').respond({});
    }));

    describe(' - (Directive: backImg) -', function () {
        // TBC
    });

    describe(' - (Directive: customCheckbox) - ', function () {
        var element, validTemplate;

        validTemplate = '<label ng-model="testCheckbox" custom-checkbox></label>';

        beforeEach(inject(function ($injector, $compile) {
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);
            $scope.$digest();
        }));
        describe('when initialised', function() {
            it('should create an input of type checkbox', function () {
                var input = element.find('input[type=checkbox]');
                expect(input[0]).not.toBeUndefined();
            });
        });
        describe('when the model value is set to true', function() {
            it('should add the checked class to the element', function() {
                $scope.testCheckbox = true;
                $scope.$apply();
                var isChecked = $(element).hasClass('checked');
                expect(isChecked).toEqual(true);
            });
        });
        describe('when the model value is set to false', function() {
            it('should remove the checked class from the element', function() {
                $scope.testCheckbox = false;
                $scope.$apply();
                var isChecked = $(element).hasClass('checked');
                expect(isChecked).toEqual(false);
            });
        });
    });

    describe(' - (Directive: toggleSwitch) - ', function () {
        var element, validTemplate;

        validTemplate = '<label ng-model="toggleSwitch" toggle-switch></label>';

        beforeEach(inject(function ($injector, $compile) {
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);
            $scope.$digest();
        }));

        it('should create a toggle switch', function () {
            var input = element.find('input[type=checkbox]');
            expect(input[0]).not.toBeUndefined();
        })
        describe('when the model value is set to true', function() {
            it('should add the toggle-on class to the element', function() {
                $scope.toggleSwitch = true;
                $scope.$apply();
                var isOn = $(element).hasClass('toggle-on');
                expect(isOn).toEqual(true);
            });
        });
        describe('when the model value is set to false', function() {
            it('should remove the toggle-on class from the element', function() {
                $scope.toggleSwitch = false;
                $scope.$apply();
                var isOn = $(element).hasClass('toggle-off');
                expect(isOn).toEqual(false);
            });
        });
    });

    describe(' - (Directive: commandValidator) -', function () {
        // TBC
    });

    describe(' - (Directive: validateOnSubmit) -', function () {
        var element, validTemplate, validationForm;

        validTemplate = '<form name="testForm">'
                            + '<input name="testInput" />'
                            + '<button name="testButton" validate-on-submit>Test</button>'
                        + '</form>';

        beforeEach(inject(function ($injector, $compile) {
            validationForm = $injector.get('ValidationForm');
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);
            $scope.$digest();
        }));

        describe('when the element is clicked', function() {
            beforeEach(function() {
                var button = element.find('button')[0];
                button.click();
            });
            it('should set the validationForm to the elements parent form', function() {
                expect(validationForm.Form).toEqual($scope.testForm);
            });
        });
    });

    describe(' - (Directive: spinClick) -', function () {
        var element, scope, $activityManager, compiler;

        beforeEach(inject(function($injector, $compile) {
            scope = $rootScope.$new();
            compiler = $compile;
            $activityManager = $injector.get('$activityManager');            
        }));

        function compileDirective(template) {
            if(!template) {
                template = '<button type="submit" style="width:100px;" id="spinButton" value="spin" spin-click="spinFunction()" spin-keep-width="true">Spin</button>';
            }
            element = (compiler(template))(scope);
            scope.$digest();
        }

        describe('when spin button is clicked', function() {
            beforeEach(function() {
                scope.spinFunction = function() {
                }
                
                compileDirective();
                spyOn($activityManager, 'registerSpinListener').and.callThrough();
                spyOn(scope, 'spinFunction').and.callThrough();
                element.click();
            });

            it('should register itself with the activity manager', function() {
                expect($activityManager.registerSpinListener).toHaveBeenCalled();
            });
            it('should create a spinner', function() {
                var spinner = element.find('.spinner')[0];
                expect(spinner).not.toBeUndefined();
            });
            it('should run the registered function', function() {
                expect(scope.spinFunction).toHaveBeenCalled();
            });
            it('should disable the spin button', function() {
                var disabled = $(element).attr('disabled');
                expect(disabled).toEqual('disabled');
            });
        });

        describe('when the activity is stopped', function() {
            beforeEach(function() {
                scope.spinFunction = function() {
                }
                compileDirective();
                element.click();
                $activityManager.stopActivity();
            });

            it('should remove the spinner', function() {
                var spinner = element.find('.spinner')[0];
                expect(spinner).toBeUndefined();
            });
            it('should enable the spin button', function() {
                var disabled = $(element).attr('disabled');
                expect(disabled).toBeUndefined();
            });
        });

        describe('when the keepWidth attribute is true', function() {
            var originalWidth 
            beforeEach(function() {
                scope.spinFunction = function() {};
                compileDirective();
                originalWidth = element.width();
            });
            it('should not change the width of the element', function() {
                element.click();
                var newWidth = element.width();
                expect(newWidth).toEqual(originalWidth);
            });
        });
        describe('when the keepWidth attribute is false', function() {
            var newTemplate = '<button type="submit" style="width:100px;" id="spinButton" value="spin" spin-click="spinFunction()" spin-keep-width="false">Spin</button>';
            var originalWidth;
            beforeEach(function() {
                scope.spinFunction = function() {};
                compileDirective(newTemplate);
                originalWidth = element.width();
            });
            it('should increase the width of the element', function() {
                element.click();
                var newWidth = element.width();
                expect(newWidth).toEqual(originalWidth + 50);
            });
        });
    });

    describe(' - (Directive: spinLoader) -', function () {
        var element, scope, $activityManager;
        var activityName = "load";
        var spinContainerId = "spinContainer";
        var compiler;

        beforeEach(inject(function($injector, $compile) {
            scope = $rootScope.$new();
            compiler = $compile;
            $activityManager = $injector.get('$activityManager');
        }));

        function compileDirective(template) {
            if(!template) {
                template = '<form style="width:100px;" id="' + spinContainerId + '" name="spinContainer" spin-loader="' + activityName + '" >'
                            + '<button id="childButton">Click here</button>'
                            + '</form>';
            }
            element = compiler(template)(scope);
            scope.$digest();
        }

        describe('when spin loader is initialised', function() {
            beforeEach(function() {
                spyOn($activityManager, 'registerSpinListenerForKey').and.callThrough();
                compileDirective();
            });

            it('should register its key with the activity manager', function() {
                expect($activityManager.registerSpinListenerForKey).toHaveBeenCalledWith(jasmine.any(Function), jasmine.any(Function), activityName, spinContainerId);
            });
        });

        describe('when an activity is started with the registered key', function () {
            var spinnerStarted = false;
            var asyncAwait = function (done) {
                setTimeout(function () {
                    spinnerStarted = true;
                    done();
                }, 300);

            };
            beforeEach(function(done) {
                compileDirective();
                $activityManager.startActivityWithKey(activityName);
                asyncAwait(done);
            });

            it('should create a spinner', function() {
                var spinner = element.find('.spinner')[0];
                expect(spinner).not.toBeUndefined();
            });
            it('should add the spinLoad class to the element', function() {
                var hasSpinLoad = $(element).hasClass('spinLoad');
                expect(hasSpinLoad).toEqual(true);
            });
            it('should disable the child element', function() {
                var child = $(element).find('#childButton')[0];
                var disabled = $(child).attr('disabled');
                expect(disabled).toEqual('disabled');
            });
        });

        describe('when an activity is stopped with the registered key', function () {
            var spinnerStarted = false;
            var asyncAwait = function (done) {
                setTimeout(function () {
                    spinnerStarted = true;
                    done();
                }, 300);

            };
            beforeEach(function(done) {
                compileDirective();
                $activityManager.startActivityWithKey(activityName);
                asyncAwait(done);
                
                $activityManager.stopActivityWithKey(activityName);
                
            });

            it('should remove the spinner', function() {
                var spinner = element.find('.spinner')[0];
                expect(spinner).toBeUndefined();
            });
            it('should remove the spinLoad class to the element', function() {
                var hasSpinLoad = $(element).hasClass('spinLoad');
                expect(hasSpinLoad).toBeFalsy();
            });
            it('should enable the child element', function() {
                var child = $(element).find('#childButton')[0];
                var disabled = $(child).attr('disabled');
                expect(disabled).toBeUndefined();
            });
        });

        describe('when the directive is destroyed', function() {
            beforeEach(function() {
                compileDirective();
                spyOn($activityManager, 'removeListenerForKey').and.callThrough();
                var directiveScope = $(element).scope();
                directiveScope.$destroy();
            });
            it('should remove the listener from the activity manager', function() {
                expect($activityManager.removeListenerForKey).toHaveBeenCalledWith(spinContainerId, activityName);
            });
        });
    });

    describe(' - (Directive: sahlAutoComplete) -', function () {
        var element, validTemplate, $queryManager, input, searchQuery;
        var autoCompleteId = 'autoCompleteInput';
        var targetId = 'searchText';
        var selectItem = 'item.SuburbName';
        var model = 'item.Suburb';
        var query = 'FilterSuburbsByNameQuery';

        validTemplate = '<div class="searchBoxContainer">'
                            + '<input type="text" name="' + autoCompleteId + '" sahl-autocomplete="' + selectItem + ' in-query ' + query + ' for-model '+ model + '" ng-model="' + model + '" />'
                            + '<ul id="' + targetId + '" class="searchBox"></ul>'
                        + '</div>';
        beforeEach(inject(function ($injector, $compile) {
            $queryManager = $injector.get('$queryManager');
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);

            searchQuery = new capitecqueryFakes.FilterSuburbsByNameQueryResult();
            searchQuery.Add('89036D34-53C3-43EB-976B-A2D500AB7410','Durban');
            searchQuery.Add('88CD55BF-ABBC-4394-B430-A2D500AB7414','Durbanville');
            $scope.GetTarget = function() {
                return $(element.find('#' + targetId)[0]);
            }
            $scope.$digest();
            input = $(element.find('input')[0]);
        }));
        describe('when a query is entered into the input', function() {
            beforeEach(function() {
                $httpBackend.whenPOST("http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery")
                    .respond(200, searchQuery);
                spyOn($queryManager, 'sendQueryAsync').and.callThrough();

                $(input).val('Dur');
                $(input).keyup();
                $scope.$digest(); 
                $httpBackend.flush();
            });

            it('should run the search query', function() {
                expect($queryManager.sendQueryAsync).toHaveBeenCalled();
            });
            it('should populate the target element with the results', function() {
                var target = element.find('#' + targetId)[0];
                var listItems = $(target).children();
                expect(listItems.length).toEqual(2);
            });
            it('should show the auto complete list', function() {
                var target = element.find('#' + targetId)[0];
                expect($(target).css('display')).toEqual('block');
            });
        });
        describe('when the search list is open', function() {
            beforeEach(function() {
                $httpBackend.whenPOST("http://localhost/CapitecService/api/QueryHttpHandler/PerformHttpQuery")
                    .respond(200, searchQuery);
                
                spyOn($queryManager, 'sendQueryAsync').and.callThrough();
                
                $(input).val('Dur');
                $(input).keyup();
                $scope.$digest(); 
                $httpBackend.flush();
            });           

            describe('when a list item is hovered over', function() {
                it('should set the item to be active', function() {
                    var liItem = element.find('li')[0];
                    $(liItem).mouseenter();
                    var isActive = $(liItem).hasClass('active');
                    expect(isActive).toEqual(true);
                });
            });
            describe('when a list item is no longer hovered over', function() {
                it('should remove the active class', function() {
                    var liItem = element.find('li')[0];
                    $(liItem).mouseenter();
                    $(liItem).mouseleave();
                    var isActive = $(liItem).hasClass('active');
                    expect(isActive).toEqual(false);
                });
            });
            describe('when a list item is clicked on', function() {
                it('should set the input to the value of the list item', function() {
                    var liItem = element.find('li')[0];
                    $(liItem).click();
                    expect(input.val()).toEqual($(liItem).text());
                });
            });
        });
    });

    describe(' - (Directive: qtip) - ', function () {
        var element, validTemplate;

        validTemplate = '<label ng-model="qtip" qtip></label>';

        beforeEach(inject(function ($injector, $compile) {
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);
            $scope.$digest();
        }));

        it('should create a qtip', function () {
            var input = element.find('div.toolTip');
            expect(input[0]).not.toBeUndefined();
        });
    });

    describe(' - (Directive: paginator) - ', function () {
       var element, validTemplate;

        validTemplate = '<label ng-model="paginator" paginator></label>';

        beforeEach(inject(function ($injector, $compile) {
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);
            $scope.$digest();
        }));

        it('should create a paginator', function () {
            var input = element.find('div#pages');
            expect(input.prevObject[0]).not.toBeUndefined();
        }); 
    });

    describe(' - (Directive: filterer) - ', function () {
        var element, validTemplate;

        validTemplate = '<label ng-model="filterer" filterer></label>';

        beforeEach(inject(function ($injector, $compile) {
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);
            $scope.$digest();
        }));

        it('should create a filterer', function () {
            var input = element.find('div#filterer');
            expect(input.prevObject[0]).not.toBeUndefined();
        }); 
    });

    describe(' - (Directive: sorter) - ', function () {
        var element, validTemplate;

        validTemplate = '<label ng-model="sorter" sorter></label>';

        beforeEach(inject(function ($injector, $compile) {
            $scope = $rootScope.$new();
            element = angular.element(validTemplate);
            $compile(element)($scope);
            $scope.$digest();
        }));

        it('should create a filterer', function () {
            var input = element.find('button.textButton');
            expect(input.prevObject[0]).not.toBeUndefined();
        }); 
    });

    describe('- (Directive: dropdown datepicker) - ', function() {
        var element, validTemplate, compiler;
        validTemplate = '<div dropdown-datepicker date="date"></div>';
        function compileDirective(template) {
            if(!template) {
                template = validTemplate;
            }
            element = compiler(template)($scope);
            
            $scope.$digest();
        }
        beforeEach(inject(function($injector, $compile) {
            $scope = $rootScope.$new();
            $scope.date = {};
            compiler = $compile;
        }));

        describe('when initialised', function() {
            it('should create 3 select inputs', function() {
                compileDirective();
                var selects = element.find('select');
                expect(selects.length).toEqual(3);
            });
        });
        describe('when maximum and minimum years are defined', function() {
            var minYear = '1985';
            var maxYear = '1990';
            var expectedYears = ['1985','1986','1987','1988','1989','1990',];
            beforeEach(function(){
                $scope.minYear = minYear;
                $scope.maxYear = maxYear;
                $scope.date.year = '1985';
                var template = '<div dropdown-datepicker date="date" min-year="minYear" max-year="maxYear"></div>'
                compileDirective(template);
            });
            it('should set the range of allowed years', function() {
                var options = $(element.find('select')[2]).find('option');
                var years = [];
                angular.forEach(options, function(opt) {
                    years.push($(opt).text());
                });
                expect(years.length).toEqual(6);
                expect(years).toEqual(expectedYears);
            });
        });

        describe('when day is greater than the number of days in the month', function() {
            beforeEach(function() {
                compileDirective();
                $scope.date.month = '02';
                $scope.date.day = '31';
                $scope.$apply();
            });
            it('should set the day to the maximum day of the month', function() {
                expect($scope.date.day).toEqual('28');
            });
        });
        describe('when year is less than the earliest allowed year', function() {

        });
    });
});
