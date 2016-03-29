'use strict';
describe('[halo.start.portalpages.entity.common.page]', function () {
    beforeEach(module('ui.router'));
    beforeEach(module('halo.start.portalpages.entity.common.page'));

    var injectorOptions = function ($scope, $rootScope, $viewManagerService, $state, $stateParams,$pageFactory) {
        this.$scope = $scope;
        this.$rootScope = $rootScope;
        this.$viewManagerService = $viewManagerService;
        this.$state = $state;
        this.$stateParams = $stateParams;
        this.$pageFactory = $pageFactory;
    };
    var scope, rootScope, viewManagerService, state, stateParams,pageFactory;

    scope = {
        $on: function () {}
    };
    rootScope = {};
    viewManagerService = {
        getPageView: function () {
            return 'page';
        }
    };
    state = {};
    stateParams = {
        model:{
            nonTilePageSate: true
        }
    };
    pageFactory = {
        back: function () {},
        setViewData:function(){}
    };

    beforeEach(inject(function ($rootScope, $controller, $q) {
        spyOn(pageFactory, 'setViewData');
        spyOn(pageFactory, 'back');
        $controller('PageCtrl', new injectorOptions(scope, rootScope, viewManagerService, state,stateParams,pageFactory));
    }));
    
    describe('PageCtrl', function () {
        describe('when page is loading', function () {
            it('should set the pageView', function () {
                expect(scope.pageView).not.toBe(null);
            });
        });

        describe('when cancelling', function () {
            beforeEach(function () {
                scope.cancel();
            });

            it('should set page factory data', function () {
                expect(pageFactory.setViewData).toHaveBeenCalled();
            });

            it('should tell page factory to go back', function () {
                expect(pageFactory.back).toHaveBeenCalled();
            });
        });
    });
});
