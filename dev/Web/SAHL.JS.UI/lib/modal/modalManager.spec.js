'use strict';

angular.module('controller.fakes',[]).controller('myControllerName', ['$scope',function($scope){

}]);

describe('[sahl.js.ui.modalManager]', function () {

    beforeEach(module('sahl.js.ui.modalManager'));
    beforeEach(module('controller.fakes'));

    var rootScope, 
    	controller, 
    	compile = null,
    	modalManagerService = null,
    	dialogParams = null;
	        
	    


	var oldDollah = window.$;

    beforeEach(inject(function ($injector, $compile, $controller, $rootScope) {

        modalManagerService = $injector.get('$modalManagerService');
        controller = $controller;
        rootScope = $rootScope;
        compile = $compile;
    }));




    describe('load modal window', function () {

    	var extendedParams;
    	
        beforeEach(function(){
	        window.$ = { 
	        	Dialog: function(params){}
	    	};
	    	dialogParams = {
		        	overlay : true,
		        	width : 500,
		        	height : 300,
		        	draggable : true,
		        	title: 'My Modal Title',
		          	templateUrl : 'MyControllerTemplate.html',
		          	controller : 'myControllerName',
		          	controllerAs: '',
		          	controllerParams: {
		          		customParam1: "CustomParam1",
		          		customParam2: "CustomParam2"
		          	}
	        	};

	    	extendedParams = {
                        overlay: dialogParams.overlay,
                        shadow: true,
                        flat: true,
                        width: dialogParams.width,
                        height: dialogParams.height,
                        draggable: dialogParams.draggable,
                        title: dialogParams.title,
                        onShow: function(_dialog){
                        }
                    };

	        spyOn($, 'Dialog').and.callThrough();

         	modalManagerService.loadModalWindow(dialogParams);
        });

        it('should load the modal window', function() {
			      	
            expect($.Dialog).toHaveBeenCalled();
        });

        afterEach(function(){
	    	window.$ = oldDollah;
	    });
    });
});