'use strict';

angular.module('sahl.js.core.documentManagement')
   .service('$documentManagerService', ['$q','$documentService','$documentVersionManagerService','$guidService',
       function ($q,$documentService,$documentVersionManagerService,$guidService) {
           var operations = {
               newFromExistingDocument : function(document){
                   var newDocument = angular.fromJson(angular.toJson(document));
                   newDocument.id = $guidService.newComb();
                   return newDocument;
               },
               newDocument : function(documentType){
                   var document = new $documentVersionManagerService.emptyDocument(documentType);
                   document.id = $guidService.newComb();
                   return document;
               },
               openDocumentById : function(id){
                   var deferred = $q.defer();
                   $documentService.getDocumentById(id).then(function(data){
                        var jsonDoc = data.data.ReturnData.Results.$values[0];
                        if(_.isEmpty(jsonDoc)){
                            deferred.reject("failed to load document with id "+ id);
                        }else{
                            var loadedDocument = $documentVersionManagerService.loadDocument(angular.fromJson(jsonDoc.JsonDocument));
                            deferred.resolve(loadedDocument);
                        }
                   },deferred.reject);
                   return deferred.promise;
               },
               openDocumentByName : function(name,documentType){
                   var deferred = $q.defer();
                   $documentService.getDocumentByNameAndType(name,documentType).then(function(data){
                       var jsonDoc = data.data.ReturnData.Results.$values[0];
                       if(_.isEmpty(jsonDoc)){
                           var doc = operations.newDocument(documentType);
                           doc.name = name;
                           deferred.resolve(doc);
                       }else{
                            var loadedDoc = $documentVersionManagerService.loadDocument(angular.fromJson(jsonDoc.JsonDocument));
                            deferred.resolve(loadedDoc);
                       }
                   },deferred.reject);
                   return deferred.promise;
               },
               saveDocument:function(document){
                   var deferred = $q.defer();
                   $documentService.createOrUpdateDocument(document).then(deferred.resolve,deferred.reject);
                   return deferred.promise;
               },
               saveDocumentAs:function(document,name){
                   var newDocument = operations.newFromExistingDocument(document);
                   newDocument.name = name;
                   return operations.saveDocument(newDocument);
               }
           };

           return {
               newFromExistingDocument: operations.newFromExistingDocument,
               newDocument: operations.newDocument,
               openDocumentById: operations.openDocumentById,
               openDocumentByName: operations.openDocumentByName,
               saveDocument: operations.saveDocument,
               saveDocumentAs: operations.saveDocumentAs
           };
   }]);