'use strict';
describe('[halo.start.portalpages.entity.common.wizard]', function () {
    beforeEach(module('ui.router'));
    beforeEach(module('halo.start.portalpages.entity.common.wizard'));
    beforeEach(module('sahl.js.ui.pages'));

    var ctrl;
    var injectorOptions = function (rootScope, scope, state, stateParams,viewManagerService, tileEditManager, toastManagerService,
    activityManager, eventAggregatorService, tileManagerService, authenticatedUser, workflowActivityManager, guidService, pageFactory, wizardFactory) {
        this.$rootScope = rootScope;
        this.$scope=scope;
        this.$state = state;
        this.$stateParams = stateParams;
        this.$viewManagerService=viewManagerService;
        this.$tileEditManager=tileEditManager;
        this.$toastManagerService=toastManagerService;
        this.$activityManager=activityManager;
        this.$eventAggregatorService=eventAggregatorService;
        this.$tileManagerService=tileManagerService;
        this.$authenticatedUser=authenticatedUser;
        this.$workflowActivityManager = workflowActivityManager;
        this.$guidService = guidService;
        this.$pageFactory = pageFactory;
        this.$wizardFactory = wizardFactory;
    };
    var q, loadWizardConfigDefer;
    var wizardConfig = [{ WizardConfiguration: { wizardPages: { $values: [{}, {}, {}] } } }];
    var entitiesModel = {};
    var rootScope;
    var scope = { 
        $parent: {},
        $on: function () {} 
    };
    var state = {
        current: {
            name: '...wizard'
        },
        go: function () { }
    };
    var stateParams = {
        model: {
            tileData: {
                $type: ''
            }
        },
        targetConfiguration: '',
        previousState: 'oldState',
        tileAction: {
            wizardTileConfiguration : ''
        }
    };
    var viewManagerService = {
        getWizardPage: function () {}
    };
    var tileEditManager;
    var toastManagerService;
    var activityManager = {
        startActivityWithKey: function () {},
        stopActivityWithKey: function () {}
    };
    var eventAggregatorService = {
        publish:function(){}
    };
    var tileManagerService = {
        loadWizardConfiguration: function () {
            loadWizardConfigDefer = q.defer();
            return loadWizardConfigDefer.promise;
        }
    };
    var authenticatedUser;
    var workflowManager = {
        cancelActivity: function () { },
        startActivity: function () { }
    };
    var guidService = {
        newGuid: function () {
            return 'expectedResult';
        }
    };
    var pageFactory = {
        back: function () { },
        setViewData: function () { }
    };

    var wizardFactory = {
        buttonsVisible: true,
        hideButtons: function () { },
        showButtons: function () { },
        changed: function() { }
    };

    beforeEach(inject(function ($rootScope, $controller, $q, $wizardFactory) {
        q = $q;
        rootScope = $rootScope;
        rootScope.entitiesModel = entitiesModel;
        wizardFactory = $wizardFactory;
        
        spyOn(activityManager, 'startActivityWithKey').and.callThrough();
        spyOn(activityManager, 'stopActivityWithKey').and.callThrough();
        spyOn(tileManagerService, 'loadWizardConfiguration').and.callThrough();

        spyOn(workflowManager, 'cancelActivity').and.callThrough();
        spyOn(workflowManager, 'startActivity').and.callThrough();

        spyOn(eventAggregatorService, 'publish').and.callThrough();

        spyOn(viewManagerService, 'getWizardPage').and.callThrough();

        spyOn(pageFactory, 'back');
        spyOn(pageFactory, 'setViewData');
       
        spyOn(wizardFactory, 'hideButtons').and.callThrough();
        spyOn(wizardFactory.internal, 'buttonsChanged').and.callThrough();
        spyOn(wizardFactory, 'showButtons').and.callThrough();

        scope.index = 0;

        ctrl = $controller('WizardCtrl', new injectorOptions(rootScope, scope, state, stateParams, viewManagerService, tileEditManager, toastManagerService, activityManager, eventAggregatorService, tileManagerService, authenticatedUser, workflowManager, guidService, pageFactory,wizardFactory));
    }));

    describe('WizardCtrl', function () {
        describe('if all inputs are valid', function () {
            describe('should set all scope variables', function () {
                beforeEach(function () {
                    loadWizardConfigDefer.resolve(wizardConfig);
                    rootScope.$digest();
                });

                it('should start activity', function () {
                    expect(activityManager.startActivityWithKey).toHaveBeenCalledWith('loading');
                });

                it('should set cancel event', function () {
                    expect(scope.cancel).not.toBe(null);
                });
                it('should set yes event', function () {
                    expect(scope.yes).not.toBe(null);
                });
                it('should set cancel event', function () {
                    expect(scope.cancel).not.toBe(null);
                });
                it('should set no event', function () {
                    expect(scope.no).not.toBe(null);
                });
                it('should set next event', function () {
                    expect(scope.next).not.toBe(null);
                });
                it('should set prev event', function () {
                    expect(scope.prev).not.toBe(null);
                });
                it('should set finish event', function () {
                    expect(scope.finish).not.toBe(null);
                });
                it('should set submit event', function () {
                    expect(scope.submit).not.toBe(null);
                });

                it('should set canNavigateBack variable', function () {
                    expect(scope.canNavigateBack).not.toBe(null);
                });
                it('should set canNavigateFoward variable', function () {
                    expect(scope.canNavigateFoward).not.toBe(null);
                });

                it('should load wizard configuration', function () {
                    expect(tileManagerService.loadWizardConfiguration).toHaveBeenCalled();
                    expect(scope.wizardConfiguration).not.toBe(null);
                    expect(scope.wizardPages).not.toBe(null);
                });

                it('should stop activity', function () {
                    expect(activityManager.stopActivityWithKey).toHaveBeenCalledWith('loading');
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                });

                it('should start with wizard workflow buttons as being visible', function () {
                    expect(wizardFactory.internal.buttonsVisible).toBe(true);
                });

                it('should start with wizard factory ready to call changed function when buttonsVisible changes value', function () {
                    expect(wizardFactory.internal.buttonsChanged).toHaveBeenCalled();
                });

                it('should have buttonsVisible set to false after hiding the buttons', function() {
                    expect(scope.buttonsVisible).toBe(true);
                    wizardFactory.hideButtons();
                    expect(scope.buttonsVisible).toBe(false);
                });
            });

            describe('when canceling wizard action', function () {
                beforeEach(function () {
                    scope.cancel();
                });
                
                it('should publish to the event aggregator', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should go to previous state', function () {
                    expect(pageFactory.back).toHaveBeenCalled();
                });
            
            });

            describe('when moving to the next wizard screen', function () {
                beforeEach(function () {
                    scope.next();
                });

                it('should call the event publisher', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should update the wizard index', function () {
                    expect(scope.index).toBeGreaterThan(0);
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                    expect(scope.pageView).not.toBe(null);
                });
            });

            describe('when moving to the next wizard screen', function () {
                beforeEach(function () {
                    scope.next();
                });

                it('should call the event publisher', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should update the wizard index', function () {
                    expect(scope.index).toBeGreaterThan(0);
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                });
            });

            describe('when moving to the prev wizard screen', function () {
                beforeEach(function () {
                    scope.next();
                    scope.prev();
                });

                it('should call the event publisher', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should update the wizard index', function () {
                    expect(scope.index).toBe(0);
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                });
            });

            describe('when yes is selected on the wizard screen', function () {
                beforeEach(function () {
                    scope.yes();
                });

                it('should call the event publisher', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should update model to set wizard input to no', function () {
                    expect(scope.wizardPages[0].WizardInput).toBe('Yes');
                });

                it('should update the wizard index', function () {
                    expect(scope.index).toBeGreaterThan(0);
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                });
            });

            describe('when yes is selected on the wizard screen', function () {
                beforeEach(function () {
                    scope.no();
                });

                it('should call the event publisher', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should update model to set wizard input to no', function () {
                    expect(scope.wizardPages[0].WizardInput).toBe('No');
                });

                it('should update the wizard index', function () {
                    expect(scope.index).toBeGreaterThan(0);
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                });
            });

            describe('when yes is selected on the wizard screen', function () {
                beforeEach(function () {
                    scope.no();
                });

                it('should call the event publisher', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should update model to set wizard input to no', function () {
                    expect(scope.wizardPages[0].WizardInput).toBe('No');
                });

                it('should update the wizard index', function () {
                    expect(scope.index).toBeGreaterThan(0);
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                });
            });

            describe('when the wizard is finished', function () {
                beforeEach(function () {
                    scope.finish();
                });

                it('should publish to the event aggregator', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should not call workflow cancel activity', function () {
                    expect(workflowManager.cancelActivity).not.toHaveBeenCalled();
                });
            });

            describe('when submitting forms', function () {
                beforeEach(function () {
                    scope.submit();
                });

                it('should call the event publisher', function () {
                    expect(eventAggregatorService.publish).toHaveBeenCalled();
                });

                it('should update the wizard index', function () {
                    expect(scope.index).toBeGreaterThan(0);
                });

                it('should update the wizard view', function () {
                    expect(viewManagerService.getWizardPage).toHaveBeenCalled();
                    expect(scope.pageView).not.toBe(null);
                });
            });
        });
    });

});
