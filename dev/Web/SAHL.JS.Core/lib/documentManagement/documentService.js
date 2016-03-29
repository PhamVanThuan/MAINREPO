'use strict';

angular.module('sahl.js.core.documentManagement')
.factory('$documentService', [
   function(){
      return {
         createOrUpdateDocument : function(){},
         getDocumentById : function(id){},
         getDocumentByNameAndType : function(name,type){}
      };
   }]);