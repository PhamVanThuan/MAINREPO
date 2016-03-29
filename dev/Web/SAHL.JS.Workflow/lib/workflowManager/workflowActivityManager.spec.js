'use strict';
describe('[sahl.js.workflow.workflowManager]', function() {
  var rootscope,
      workflowActivityManager,
      workflowService,
      guidService,
      q,
      startDeferred,
      cancelDeferred,
      completeDeferred,
      rawMessages,
      toastManagerService,
      wizardFactory;
      
  beforeEach(module('sahl.js.workflow.mocks'));
  beforeEach(module('sahl.js.core.primitives'));
  beforeEach(module('sahl.js.workflow.workflowManager'));
  beforeEach(module('sahl.js.ui.pages'));
  beforeEach(module('ui.router'));

  beforeEach(inject(function($rootScope,
                             $workflowActivityManager,
                             $workflowService,
                             $guidService,
                             $q,
                             $rawMessages,
                             $httpBackend,
                             $toastManagerService,
                             $wizardFactory){
    rootscope = $rootScope;
    workflowActivityManager = $workflowActivityManager;
    workflowService = $workflowService;
    guidService = $guidService;
    q = $q;
    rawMessages = $rawMessages;
    toastManagerService = $toastManagerService;
    wizardFactory = $wizardFactory;
      
    startDeferred = q.defer();
    cancelDeferred = q.defer();
    completeDeferred = q.defer();

    spyOn(workflowService,'startActivity').and.returnValue(startDeferred.promise);
    spyOn(workflowService,'cancelActivity').and.returnValue(cancelDeferred.promise);
    spyOn(workflowService,'completeActivity').and.returnValue(completeDeferred.promise);

    spyOn(wizardFactory, 'setCanCancelWorkFlowActivity').and.callThrough();
    //spyOn(wizardFactory.internal, 'canCancelWorkFlow').and.callThrough();
      
    spyOn(toastManagerService,'notice');
    spyOn(toastManagerService,'error');
    spyOn(toastManagerService,'info');
  }));

  describe(' - workflowActivityManager - ',function(){
    describe('when starting an activity',function(){
      describe('and instance successfuly locked',function(){
        var returnedPromise;
        beforeEach(function(){
          returnedPromise = workflowActivityManager.start(guidService.newComb(), 1, 'test', false, {}, {});
          wizardFactory.setCanCancelWorkFlowActivity(true);
          spyOn(wizardFactory.internal, 'canCancelWorkFlow').and.returnValue(true);
        });

        it('should return non null value',function(){
          expect(returnedPromise).not.toBeNull();
        });

        it('should set the can cancel workflow activity to true',function(){
          expect(wizardFactory.setCanCancelWorkFlowActivity).toHaveBeenCalled();
        });
          
        it('should return promise with then method',function(){
          expect(returnedPromise.then).not.toBeNull();
          expect(typeof returnedPromise.then).toBe('function');
        });

        it('should not call the toast manager service notice',function(){
          expect(toastManagerService.notice).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service error',function(){
          expect(toastManagerService.error).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service info',function(){
          expect(toastManagerService.info).not.toHaveBeenCalled();
        });
      });

      describe('and instance already locked',function(){
        var returnedPromise;
        var result;
        beforeEach(function(){
          returnedPromise = workflowActivityManager.start(guidService.newComb(), 1, 'test', false, {}, {});
          returnedPromise.then(function(_result){
            result = _result;
          });
          startDeferred.resolve(rawMessages.startActivity.nodeDown);
          rootscope.$digest();
          wizardFactory.setCanCancelWorkFlowActivity(false);
          spyOn(wizardFactory.internal, 'canCancelWorkFlow').and.returnValue(false);
        });

        it('should return non null value promise object',function(){
          expect(returnedPromise).not.toBeNull();
        });

        it('should set the can cancel workflow activity to false',function(){
          expect(wizardFactory.setCanCancelWorkFlowActivity).toHaveBeenCalled();
        });
          
        it('should return promise with then method',function(){
          expect(returnedPromise.then).not.toBeNull();
          expect(typeof returnedPromise.then).toBe('function');
        });

        it('should not call the toast manager service notice',function(){
          expect(toastManagerService.notice).not.toHaveBeenCalled();
        });

        it('should call the toast manager service error',function(){
          expect(toastManagerService.error).toHaveBeenCalled();
        });

        it('should not call the toast manager service info',function(){
          expect(toastManagerService.info).not.toHaveBeenCalled();
        });
      });

      describe('and node is down',function(){
        var returnedPromise;
        var result;
        beforeEach(function(){
          returnedPromise = workflowActivityManager.start(guidService.newComb(), 1, 'test', false, {}, {});
          returnedPromise.then(function(_result){
            result = _result;
          });
          startDeferred.resolve(rawMessages.startActivity.nodeDown);
          rootscope.$digest();
        });

        it('should return non null value promise object',function(){
          expect(returnedPromise).not.toBeNull();
        });

        it('should return promise with then method',function(){
          expect(returnedPromise.then).not.toBeNull();
          expect(typeof returnedPromise.then).toBe('function');
        });

        it('should not call the toast manager service notice',function(){
          expect(toastManagerService.notice).not.toHaveBeenCalled();
        });

        it('should call the toast manager service error',function(){
          expect(toastManagerService.error).toHaveBeenCalled();
        });

        it('should not call the toast manager service info',function(){
          expect(toastManagerService.info).not.toHaveBeenCalled();
        });
      });
    });

    describe('when cancelling an activity',function(){
      describe('and instance successfuly locked',function(){
        var returnedPromise;
        beforeEach(function(){
          returnedPromise = workflowActivityManager.cancel(guidService.newComb(), 1, 'test', false, {}, {});
          spyOn(wizardFactory.internal, 'canCancelWorkFlow').and.returnValue(false);
        });

        it('should return non null value',function(){
          expect(returnedPromise).not.toBeNull();
        });

        it('should return promise with then method',function(){
          expect(returnedPromise.then).not.toBeNull();
          expect(typeof returnedPromise.then).toBe('function');
        });

        it('should not call the toast manager service notice',function(){
          expect(toastManagerService.notice).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service error',function(){
          expect(toastManagerService.error).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service info',function(){
          expect(toastManagerService.info).not.toHaveBeenCalled();
        });
      });
      describe('and node is down',function(){
        var returnedPromise;
        var result;
        beforeEach(function(){
          returnedPromise = workflowActivityManager.cancel(guidService.newComb(), 1, 'test', false, {}, {});
          returnedPromise.then(function(_result){
            result = _result;
          });
          cancelDeferred.resolve(rawMessages.cancelActivity.nodeDown);
          rootscope.$digest();
        });

        it('should return non null value promise object',function(){
          expect(returnedPromise).not.toBeNull();
        });

        it('should return promise with then method',function(){
          expect(returnedPromise.then).not.toBeNull();
          expect(typeof returnedPromise.then).toBe('function');
        });

        it('should not call the toast manager service notice',function(){
          expect(toastManagerService.notice).not.toHaveBeenCalled();
        });

        it('should call the toast manager service error',function(){
          expect(toastManagerService.error).toHaveBeenCalled();
        });

        it('should not call the toast manager service info',function(){
          expect(toastManagerService.info).not.toHaveBeenCalled();
        });
      });
      describe('and cannot cancel workflow activity', function(){
        var returnedPromise;
        beforeEach(function(){
          returnedPromise = workflowActivityManager.cancel(guidService.newComb(), 1, 'test', false, {}, {});
          spyOn(wizardFactory.internal, 'canCancelWorkFlow').and.returnValue(false);
        });
        
        it('should return promise with then method',function(){
          expect(returnedPromise.then).not.toBeNull();
          expect(typeof returnedPromise.then).toBe('function');
        });

        it('should not call the toast manager service notice',function(){
          expect(toastManagerService.notice).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service error',function(){
          expect(toastManagerService.error).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service info',function(){
          expect(toastManagerService.info).not.toHaveBeenCalled();
        });
      });
    });

    describe('when completing an activity',function(){
      describe('and instance successfuly locked',function(){
        var returnedPromise;
        beforeEach(function(){
          returnedPromise = workflowActivityManager.complete(guidService.newComb(), 1, 'test', false, {}, {});
        });

        it('should return non null value',function(){
          expect(returnedPromise).not.toBeNull();
        });

        it('should return promise with then method',function(){
          expect(returnedPromise.then).not.toBeNull();
          expect(typeof returnedPromise.then).toBe('function');
        });

        it('should not call the toast manager service notice',function(){
          expect(toastManagerService.notice).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service error',function(){
          expect(toastManagerService.error).not.toHaveBeenCalled();
        });

        it('should not call the toast manager service info',function(){
          expect(toastManagerService.info).not.toHaveBeenCalled();
        });
      });
    });
    describe('and node is down',function(){
      var returnedPromise;
      var result;
      beforeEach(function(){
        returnedPromise = workflowActivityManager.complete(guidService.newComb(), 1, 'test', false, {}, {});
        returnedPromise.then(function(_result){
          result = _result;
        });
        completeDeferred.resolve(rawMessages.completeActivity.nodeDown);
        rootscope.$digest();
      });

      it('should return non null value promise object',function(){
        expect(returnedPromise).not.toBeNull();
      });

      it('should return promise with then method',function(){
        expect(returnedPromise.then).not.toBeNull();
        expect(typeof returnedPromise.then).toBe('function');
      });

      it('should not call the toast manager service notice',function(){
        expect(toastManagerService.notice).not.toHaveBeenCalled();
      });

      it('should call the toast manager service error',function(){
        expect(toastManagerService.error).toHaveBeenCalled();
      });

      it('should not call the toast manager service info',function(){
        expect(toastManagerService.info).not.toHaveBeenCalled();
      });
    });
    describe('and you get an info message',function(){
      var returnedPromise;
      var result;
      beforeEach(function(){
        returnedPromise = workflowActivityManager.complete(guidService.newComb(), 1, 'test', false, {}, {});
        returnedPromise.then(function(_result){
          result = _result;
        });
        completeDeferred.resolve(rawMessages.completeActivity.infoMessage);
        rootscope.$digest();
      });

      it('should return non null value promise object',function(){
        expect(returnedPromise).not.toBeNull();
      });

      it('should return promise with then method',function(){
        expect(returnedPromise.then).not.toBeNull();
        expect(typeof returnedPromise.then).toBe('function');
      });

      it('should not call the toast manager service notice',function(){
        expect(toastManagerService.notice).not.toHaveBeenCalled();
      });

      it('should not call the toast manager service error',function(){
        expect(toastManagerService.error).not.toHaveBeenCalled();
      });

      it('should call the toast manager service info',function(){
        expect(toastManagerService.info).toHaveBeenCalled();
      });
    });

    describe('and you get a warning message',function(){
      var returnedPromise;
      var result;
      beforeEach(function(){
        returnedPromise = workflowActivityManager.complete(guidService.newComb(), 1, 'test', false, {}, {});
        returnedPromise.then(function(_result){
          result = _result;
        });
        completeDeferred.resolve(rawMessages.completeActivity.warningMessage);
        rootscope.$digest();
      });

      it('should return non null value promise object',function(){
        expect(returnedPromise).not.toBeNull();
      });

      it('should return promise with then method',function(){
        expect(returnedPromise.then).not.toBeNull();
        expect(typeof returnedPromise.then).toBe('function');
      });

      it('should call the toast manager service notice',function(){
        expect(toastManagerService.notice).toHaveBeenCalled();
      });

      it('should not call the toast manager service error',function(){
        expect(toastManagerService.error).not.toHaveBeenCalled();
      });

      it('should not call the toast manager service info',function(){
        expect(toastManagerService.info).not.toHaveBeenCalled();
      });
    });

  });
});
