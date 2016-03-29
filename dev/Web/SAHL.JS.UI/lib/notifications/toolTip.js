'use strict';
angular.module('sahl.js.ui.notifications')
.directive('toolTip', ['$toastManagerService', '$parse', function ($toastManagerService, $parse) {
  return {
    link: function (scope, element, attrs, ngModel) {
	 var length = attrs.minLength != null ? attrs.minLength : 0;
      var text = $parse(attrs.toolTip)(scope);
	   if(text.length >= length){
	    var toolTip = $toastManagerService.tooltip(text);
		 element.bind("mouseover", function (e) {
		 toolTip.open();
	     toolTip.get().css({ 'top': e.originalEvent.clientY + 12, 'left': e.originalEvent.clientX + 12 });
	     });
	     element.bind("mousemove", function (e) {
		 toolTip.get().css({ 'top': e.originalEvent.clientY + 12, 'left': e.originalEvent.clientX + 12 });
	     });
	     element.bind("mouseout", function (e) {
		 toolTip.remove();
		 });
	     scope.$on('$destroy', function() {
	     toolTip.remove();
	     });
	    }
       }
     };
}]);