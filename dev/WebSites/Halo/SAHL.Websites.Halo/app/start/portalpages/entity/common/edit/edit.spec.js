'use strict';
describe('[halo.start.portalpages.entity.common.edit]', function () {
    beforeEach(module('ui.router'));
    beforeEach(module('halo.start.portalpages.entity.common.edit'));
    var injectorOptions = function (rootScope,scope,state,stateParams,viewManagerService,tileEditorManager,toastManagerService,activityManager,pageFactory) {
        this.$rootScope = rootScope;
        this.$scope = scope;
        this.$state = state;
        this.$stateParams = stateParams;
        this.$viewManagerService = viewManagerService;
        this.$tileEditManager = tileEditorManager;
        this.$toastManagerService = toastManagerService;
        this.$activityManager = activityManager;
        this.$pageFactory  = pageFactory;
    };

    var controller,rootScope, scope, q, deferredSave, deferredToast;
    var state = { go: function () { }};
    var stateParams = {
        model: {
            tileData: {
                $type: ''
            }
        },
        targetConfiguration: '',
        previousState: ''
    };

    var entitiesModel = {};
    var viewManager = {
        getEditView: function () { return ''; }
    };

    var editForm = {
        $dirty: false,
        $valid: true
    };

    var tileEditManager = {
        save: function () {
            deferredSave = q.defer();
            return deferredSave.promise;
        }
    };

    var toastManagerService = {
        success: function () {
            deferredToast = q.defer();
            return {promise:deferredToast.promise};
        },
        error: function () {
        }
    };

    var activityManager = {
        startActivityWithKey: function () { },
        stopActivityWithKey: function () { }
    };


    var pageFactory = {
        back: function () { },
        setViewData: function () { }
    };

    beforeEach(inject(function ($rootScope, $controller,$q) {
        rootScope = $rootScope;
        scope = $rootScope.$new();
        scope.editForm = editForm;

        spyOn(state, 'go');
        spyOn(viewManager, 'getEditView');
        spyOn(tileEditManager, 'save').and.callThrough();
        spyOn(toastManagerService, 'success').and.callThrough();
        spyOn(toastManagerService, 'error');
        spyOn(activityManager, 'startActivityWithKey');
        spyOn(activityManager, 'stopActivityWithKey');

        spyOn(pageFactory, 'setViewData');
        spyOn(pageFactory, 'back');

        rootScope.entitiesModel = entitiesModel;
        q = $q;
        controller = $controller;
    }));
    
    describe('EditController', function () {
        describe('when initilizing the controller', function () {
            beforeEach(function () {
                var ctrl = controller('EditCtrl', new injectorOptions(rootScope, scope, state, stateParams, viewManager, tileEditManager, toastManagerService, activityManager, pageFactory));
            });

            it('should get edit view', function () {
                expect(viewManager.getEditView).toHaveBeenCalled();
            });

            it('should set scope variables', function () {
                expect(scope.editView).not.toBe(null);
                expect(scope.item).not.toBe(null);
                expect(scope.save).not.toBe(null);
                expect(scope.submitting).not.toBe(null);
            });

            it('should set the view data for the page factory', function () {
                expect(pageFactory.setViewData).toHaveBeenCalled();
            });
        });

        describe('when cancelling edit', function () {
            beforeEach(function () {
                var ctrl = controller('EditCtrl', new injectorOptions(rootScope, scope, state, stateParams, viewManager, tileEditManager, toastManagerService, activityManager,pageFactory));
                scope.cancel();
            });

            it('should pop the last item of the breadcrumb', function () {
                expect(pageFactory.back).toHaveBeenCalled();
            });
        });

        describe('when saving', function () {
            beforeEach(function () {
                scope.submitting = false;
            });

            describe('and the form has changed', function () {
                beforeEach(function () {
                    var ctrl = controller('EditCtrl', new injectorOptions(rootScope, scope, state, stateParams, viewManager, tileEditManager, toastManagerService, activityManager,pageFactory));
                    scope.save();
                });
                describe('and form is valid', function () {
                    it('should disable button by setting status', function () {
                        expect(scope.submitting).toBe(true);
                    });

                    it('should call the activity manager to indicate that the form is saving', function () {
                        expect(activityManager.startActivityWithKey).toHaveBeenCalledWith('saving');
                    });

                    it('should make call to save', function () {
                        expect(tileEditManager.save).toHaveBeenCalled();
                    });

                    describe('once save is successful', function () {
                        beforeEach(function () {
                            deferredSave.resolve();
                            rootScope.$apply();
                        });

                        it('should call the activity manager to indicate that the form is saved', function () {
                            expect(activityManager.stopActivityWithKey).toHaveBeenCalledWith('saving');
                        });

                        it('should call the toast manager to show successful save', function () {
                            expect(toastManagerService.success).toHaveBeenCalled();
                        });

                        describe('once success toast is shown', function () {
                            it('should close and go back', function () {
                                
                            });
                        });
                    });
                    describe('once save is unsuccessful', function () {
                        beforeEach(function () {
                            deferredSave.reject();
                            rootScope.$apply();
                        });

                        it('should call the activity manager to indicate that the form is saved', function () {
                            expect(activityManager.stopActivityWithKey).toHaveBeenCalledWith('saving');
                        });

                        it('should call the toast manager to show successful save', function () {
                            expect(toastManagerService.error).toHaveBeenCalled();
                        });

                        it('should re enable button by setting status', function () {
                            expect(scope.submitting).toBe(false);
                        });
                    });
                });
            });

            describe('and the form is invalid', function () {
                beforeEach(function () {
                    scope.editForm.$valid = false;
                    var ctrl = controller('EditCtrl', new injectorOptions(rootScope, scope, state, stateParams, viewManager, tileEditManager, toastManagerService, activityManager,pageFactory));
                    scope.save();
                });

                it('should disable button by setting status', function () {
                    expect(scope.submitting).toBe(false);
                });

                it('should call the activity manager to indicate that the form is saving', function () {
                    expect(activityManager.startActivityWithKey).not.toHaveBeenCalledWith('saving');
                });

                it('should make call to save', function () {
                    expect(tileEditManager.save).not.toHaveBeenCalled();
                });
            });
        });
    });
});
