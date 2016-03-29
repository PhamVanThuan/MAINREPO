var decisionTreeDesignqueryFakes = function(){
	function SystemMessages(){
		return {
			"$type" : "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core",
			"HasErrors" : false,
			"HasWarnings" : false,
			"ErrorMessages" : function(){
				var retObj = this.AllMessages;
				retObj.$values = $.grep(this.AllMessages.$values,function(msg){
					return ((msg.Severity === 1)||(msg.Severity === 3));
				});
				return retObj;
			},
			"WarningMessages" : function(){
				var retObj = this.AllMessages;
				retObj.$values = $.grep(this.AllMessages.$values,function(msg){
					return msg.Severity === 0;
				});
				return retObj;
			},
			"AllMessages" : {
				"$id" : "99",
				"$type" : "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core",
				"$values" : []
			},
			"AddError": function(message){
				var message = {"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null","Severity" : 1,"Message" : message};
				this.AllMessages.$values.push(message);
				this.HasErrors = true;
			},
			"AddWarning": function(message){
				var message = {"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null","Severity" : 0,"Message" : message};
				this.AllMessages.$values.push(message);
				this.HasWarnings = true;
			},
			"AddInfo": function(message){
				var message = {
					"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
					"Severity" : 2,
					"Message" : message
				};
				this.AllMessages.$values.push(message);
			}
		}
	}
	this.GetAllMessageVersionsQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAllMessageVersionsQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAllMessageVersionsQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,version,isPublished,publishDate,publisher){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Version": version,
					"IsPublished": isPublished,
					"PublishDate": publishDate,
					"Publisher": publisher
					}
				)
			}
					}
	}
	this.GetAllVariableVersionsQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAllVariableVersionsQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAllVariableVersionsQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,version,isPublished,publishDate,publisher){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Version": version,
					"IsPublished": isPublished,
					"PublishDate": publishDate,
					"Publisher": publisher
					}
				)
			}
					}
	}
	this.GetCodeEditorKeywordsQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetCodeEditorKeywordsQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetCodeEditorKeywordsQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(data){
				this.ReturnData.Results.$values.push(
					{
						"Data": data
					}
				)
			}
					}
	}
	this.GetCurrentlyOpenDecisionTreeQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetCurrentlyOpenDecisionTreeQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetCurrentlyOpenDecisionTreeQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,documentVersionId,username,openDate,documentTypeId){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"DocumentVersionId": documentVersionId,
					"Username": username,
					"OpenDate": openDate,
					"DocumentTypeId": documentTypeId
					}
				)
			}
					}
	}
	this.GetDecisionTreeByNameAndVersionNumberQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeByNameAndVersionNumberQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeByNameAndVersionNumberQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(decisionTreeId,decisionTreeVersionId,data,version,name){
				this.ReturnData.Results.$values.push(
					{
						"DecisionTreeId": decisionTreeId,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"Data": data,
					"Version": version,
					"Name": name
					}
				)
			}
					}
	}
	this.GetDecisionTreeByNameQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeByNameQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeByNameQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,name,description,isActive){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Name": name,
					"Description": description,
					"IsActive": isActive
					}
				)
			}
					}
	}
	this.GetAllEnumerationVersionsQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAllEnumerationVersionsQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAllEnumerationVersionsQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,version,isPublished,publishDate,publisher){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Version": version,
					"IsPublished": isPublished,
					"PublishDate": publishDate,
					"Publisher": publisher
					}
				)
			}
					}
	}
	this.GetAuthenticatedUserDetailsResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAuthenticatedUserDetailsResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetAuthenticatedUserDetailsResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(username,displayName,emailAddress,isSuperUser){
				this.ReturnData.Results.$values.push(
					{
						"Username": username,
					"DisplayName": displayName,
					"EmailAddress": emailAddress,
					"IsSuperUser": isSuperUser
					}
				)
			}
					}
	}
	this.GetDecisionTreeHistoryInfoQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeHistoryInfoQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeHistoryInfoQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,decisionTreeVersionId,decisionTreeVersion,isPublished,modificationDate,modificationUser){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"DecisionTreeVersion": decisionTreeVersion,
					"IsPublished": isPublished,
					"ModificationDate": modificationDate,
					"ModificationUser": modificationUser
					}
				)
			}
					}
	}
	this.GetDecisionTreeQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetDecisionTreeQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(decisionTreeId,decisionTreeVersionId,data,version,name,description,isPublished,maxVersion){
				this.ReturnData.Results.$values.push(
					{
						"DecisionTreeId": decisionTreeId,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"Data": data,
					"Version": version,
					"Name": name,
					"Description": description,
					"IsPublished": isPublished,
					"MaxVersion": maxVersion
					}
				)
			}
					}
	}
	this.GetEnumerationSetQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetEnumerationSetQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetEnumerationSetQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,version,data,isPublished,publisher,lockedBy){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Version": version,
					"Data": data,
					"IsPublished": isPublished,
					"Publisher": publisher,
					"LockedBy": lockedBy
					}
				)
			}
					}
	}
	this.GetLatestDecisionTreesQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(decisionTreeId,name,description,isActive,thumbnail,decisionTreeVersionId,thisVersion,isPublished,currentlyOpenBy){
				this.ReturnData.Results.$values.push(
					{
						"DecisionTreeId": decisionTreeId,
					"Name": name,
					"Description": description,
					"IsActive": isActive,
					"Thumbnail": thumbnail,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"ThisVersion": thisVersion,
					"IsPublished": isPublished,
					"CurrentlyOpenBy": currentlyOpenBy
					}
				)
			}
					}
	}
	this.GetLatestDecisionTreesPubOnlyQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesPubOnlyQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesPubOnlyQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(decisionTreeId,name,description,isActive,thumbnail,decisionTreeVersionId,latestVersion,isPublished){
				this.ReturnData.Results.$values.push(
					{
						"DecisionTreeId": decisionTreeId,
					"Name": name,
					"Description": description,
					"IsActive": isActive,
					"Thumbnail": thumbnail,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"LatestVersion": latestVersion,
					"IsPublished": isPublished
					}
				)
			}
					}
	}
	this.GetLatestDecisionTreesMaxOnlyQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesMaxOnlyQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesMaxOnlyQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(decisionTreeId,name,description,isActive,thumbnail,decisionTreeVersionId,latestVersion,isPublished){
				this.ReturnData.Results.$values.push(
					{
						"DecisionTreeId": decisionTreeId,
					"Name": name,
					"Description": description,
					"IsActive": isActive,
					"Thumbnail": thumbnail,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"LatestVersion": latestVersion,
					"IsPublished": isPublished
					}
				)
			}
					}
	}
	this.GetLatestDecisionTreesUnpubOnlyQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesUnpubOnlyQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestDecisionTreesUnpubOnlyQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(decisionTreeId,name,description,isActive,thumbnail,decisionTreeVersionId,latestVersion,isPublished){
				this.ReturnData.Results.$values.push(
					{
						"DecisionTreeId": decisionTreeId,
					"Name": name,
					"Description": description,
					"IsActive": isActive,
					"Thumbnail": thumbnail,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"LatestVersion": latestVersion,
					"IsPublished": isPublished
					}
				)
			}
					}
	}
	this.MessageSetQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.MessageSetQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.MessageSetQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,version,data,isPublished,lockedBy){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"Version": version,
					"Data": data,
					"IsPublished": isPublished,
					"LockedBy": lockedBy
					}
				)
			}
					}
	}
	this.PublishedMessageSetQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.PublishedMessageSetQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.PublishedMessageSetQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(messageSetId,version,data,publishedMessageSetId,publishDate,publisher,publishStatus){
				this.ReturnData.Results.$values.push(
					{
						"MessageSetId": messageSetId,
					"Version": version,
					"Data": data,
					"PublishedMessageSetId": publishedMessageSetId,
					"PublishDate": publishDate,
					"Publisher": publisher,
					"PublishStatus": publishStatus
					}
				)
			}
					}
	}
	this.GetLatestVariableSetQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestVariableSetQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetLatestVariableSetQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(iD,version,data,isPublished,publisher,lockedBy){
				this.ReturnData.Results.$values.push(
					{
						"ID": iD,
					"Version": version,
					"Data": data,
					"IsPublished": isPublished,
					"Publisher": publisher,
					"LockedBy": lockedBy
					}
				)
			}
					}
	}
	this.GetMessageSetByMessageSetIdQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetMessageSetByMessageSetIdQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetMessageSetByMessageSetIdQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(messageSetId,version,data){
				this.ReturnData.Results.$values.push(
					{
						"MessageSetId": messageSetId,
					"Version": version,
					"Data": data
					}
				)
			}
					}
	}
	this.GetMRUDecisionTreeQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetMRUDecisionTreeQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetMRUDecisionTreeQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id,userName,treeId,modifiedDate,decisionTreeId,name,description,isActive,thumbnail,decisionTreeVersionId,version,isPublished,pinned,currentlyOpenBy){
				this.ReturnData.Results.$values.push(
					{
						"Id": id,
					"UserName": userName,
					"TreeId": treeId,
					"ModifiedDate": modifiedDate,
					"DecisionTreeId": decisionTreeId,
					"Name": name,
					"Description": description,
					"IsActive": isActive,
					"Thumbnail": thumbnail,
					"DecisionTreeVersionId": decisionTreeVersionId,
					"Version": version,
					"IsPublished": isPublished,
					"Pinned": pinned,
					"CurrentlyOpenBy": currentlyOpenBy
					}
				)
			}
					}
	}
	this.GetNewGuidQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetNewGuidQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetNewGuidQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(id){
				this.ReturnData.Results.$values.push(
					{
						"Id": id
					}
				)
			}
					}
	}
	this.GetNonTreeDocumentLockStatusQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetNonTreeDocumentLockStatusQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetNonTreeDocumentLockStatusQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(documentType,username){
				this.ReturnData.Results.$values.push(
					{
						"DocumentType": documentType,
					"Username": username
					}
				)
			}
					}
	}
	this.GetVariableSetByVariableSetIdQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetVariableSetByVariableSetIdQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.GetVariableSetByVariableSetIdQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(iD,version,data,isPublished,publisher){
				this.ReturnData.Results.$values.push(
					{
						"ID": iD,
					"Version": version,
					"Data": data,
					"IsPublished": isPublished,
					"Publisher": publisher
					}
				)
			}
					}
	}
	this.ValidateRubyCodeQueryResult = function(){
		return {
			"$type" : "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
			"ReturnData" : {
				"$type" : "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.ValidateRubyCodeQueryResult]]",
				"NumberOfPages": 1,
				"ResultCountInAllPages": 1,
				"ResultCountInPage": 1,
				"Results": {
					"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.DecisionTreeDesign.Models.ValidateRubyCodeQueryResult]]",
					"$values" : []
				}
			},
			"SystemMessages" : SystemMessages(),
						"Add" : function(valid,message,errorString){
				this.ReturnData.Results.$values.push(
					{
						"Valid": valid,
					"Message": message,
					"ErrorString": errorString
					}
				)
			}
					}
	}
	return {
		SystemMessages : SystemMessages,
		GetAllMessageVersionsQueryResult: GetAllMessageVersionsQueryResult,
		GetAllVariableVersionsQueryResult: GetAllVariableVersionsQueryResult,
		GetCodeEditorKeywordsQueryResult: GetCodeEditorKeywordsQueryResult,
		GetCurrentlyOpenDecisionTreeQueryResult: GetCurrentlyOpenDecisionTreeQueryResult,
		GetDecisionTreeByNameAndVersionNumberQueryResult: GetDecisionTreeByNameAndVersionNumberQueryResult,
		GetDecisionTreeByNameQueryResult: GetDecisionTreeByNameQueryResult,
		GetAllEnumerationVersionsQueryResult: GetAllEnumerationVersionsQueryResult,
		GetAuthenticatedUserDetailsResult: GetAuthenticatedUserDetailsResult,
		GetDecisionTreeHistoryInfoQueryResult: GetDecisionTreeHistoryInfoQueryResult,
		GetDecisionTreeQueryResult: GetDecisionTreeQueryResult,
		GetEnumerationSetQueryResult: GetEnumerationSetQueryResult,
		GetLatestDecisionTreesQueryResult: GetLatestDecisionTreesQueryResult,
		GetLatestDecisionTreesPubOnlyQueryResult: GetLatestDecisionTreesPubOnlyQueryResult,
		GetLatestDecisionTreesMaxOnlyQueryResult: GetLatestDecisionTreesMaxOnlyQueryResult,
		GetLatestDecisionTreesUnpubOnlyQueryResult: GetLatestDecisionTreesUnpubOnlyQueryResult,
		MessageSetQueryResult: MessageSetQueryResult,
		PublishedMessageSetQueryResult: PublishedMessageSetQueryResult,
		GetLatestVariableSetQueryResult: GetLatestVariableSetQueryResult,
		GetMessageSetByMessageSetIdQueryResult: GetMessageSetByMessageSetIdQueryResult,
		GetMRUDecisionTreeQueryResult: GetMRUDecisionTreeQueryResult,
		GetNewGuidQueryResult: GetNewGuidQueryResult,
		GetNonTreeDocumentLockStatusQueryResult: GetNonTreeDocumentLockStatusQueryResult,
		GetVariableSetByVariableSetIdQueryResult: GetVariableSetByVariableSetIdQueryResult,
		ValidateRubyCodeQueryResult: ValidateRubyCodeQueryResult
	};
}();