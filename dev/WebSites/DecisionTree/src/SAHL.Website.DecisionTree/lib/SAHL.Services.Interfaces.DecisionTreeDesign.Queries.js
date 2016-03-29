'use strict';

angular.module('SAHL.Services.Interfaces.DecisionTreeDesign.queries', []).
	factory('$decisionTreeDesignQueries', [function () {
		var shared = (function () {
						function GetAllMessageVersionsQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetAllMessageVersionsQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetAllVariableVersionsQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetAllVariableVersionsQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetCodeEditorKeywordsQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetCodeEditorKeywordsQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetCurrentlyOpenDecisionTreeQuery(treeVersionId) {
				this.treeVersionId = treeVersionId;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetCurrentlyOpenDecisionTreeQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetDecisionTreeByNameAndVersionNumberQuery(decisionTreeName, versionNumber) {
				this.decisionTreeName = decisionTreeName || '';
				this.versionNumber = versionNumber;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetDecisionTreeByNameAndVersionNumberQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetDecisionTreeByNameQuery(decisionTreeName) {
				this.decisionTreeName = decisionTreeName || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetDecisionTreeByNameQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetAllEnumerationVersionsQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetAllEnumerationVersionsQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetAuthenticatedUserDetailsQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetAuthenticatedUserDetailsQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetDecisionTreeHistoryInfoQuery(decisionTreeId) {
				this.decisionTreeId = decisionTreeId;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetDecisionTreeHistoryInfoQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetDecisionTreeQuery(decisionTreeVersionId) {
				this.decisionTreeVersionId = decisionTreeVersionId;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetDecisionTreeQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetEnumerationAtVersionQuery(id) {
				this.id = id;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetEnumerationAtVersionQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestDecisionTreesQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestDecisionTreesQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestDecisionTreesPubOnlyQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestDecisionTreesPubOnlyQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestDecisionTreesMaxOnlyQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestDecisionTreesMaxOnlyQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestDecisionTreesUnpubOnlyQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestDecisionTreesUnpubOnlyQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestEnumerationSetQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestEnumerationSetQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestMessageSetQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestMessageSetQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestPublishedMessageSetQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestPublishedMessageSetQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetLatestVariableSetQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetLatestVariableSetQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetMessageSetByMessageSetIdQuery(messageSetId) {
				this.messageSetId = messageSetId;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetMessageSetByMessageSetIdQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetMRUDecisionTreeQuery(userName) {
				this.userName = userName || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetMRUDecisionTreeQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetNewGuidQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetNewGuidQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetNonTreeDocumentLockStatusQuery() {
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetNonTreeDocumentLockStatusQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetPublishedMessageSetByMessageSetIdQuery(messageSetId) {
				this.messageSetId = messageSetId;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetPublishedMessageSetByMessageSetIdQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function GetVariableSetByVariableSetIdQuery(variableSetId) {
				this.variableSetId = variableSetId;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.GetVariableSetByVariableSetIdQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function ValidateRubyCodeQuery(rubyCode, enumJson, messages, localVars, globalVars) {
				this.rubyCode = rubyCode || '';
				this.enumJson = enumJson || '';
				this.messages = messages || '';
				this.localVars = localVars || '';
				this.globalVars = globalVars || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Queries.ValidateRubyCodeQuery,SAHL.Services.Interfaces.DecisionTreeDesign';
				}

			return {
				GetAllMessageVersionsQuery: GetAllMessageVersionsQuery,
				GetAllVariableVersionsQuery: GetAllVariableVersionsQuery,
				GetCodeEditorKeywordsQuery: GetCodeEditorKeywordsQuery,
				GetCurrentlyOpenDecisionTreeQuery: GetCurrentlyOpenDecisionTreeQuery,
				GetDecisionTreeByNameAndVersionNumberQuery: GetDecisionTreeByNameAndVersionNumberQuery,
				GetDecisionTreeByNameQuery: GetDecisionTreeByNameQuery,
				GetAllEnumerationVersionsQuery: GetAllEnumerationVersionsQuery,
				GetAuthenticatedUserDetailsQuery: GetAuthenticatedUserDetailsQuery,
				GetDecisionTreeHistoryInfoQuery: GetDecisionTreeHistoryInfoQuery,
				GetDecisionTreeQuery: GetDecisionTreeQuery,
				GetEnumerationAtVersionQuery: GetEnumerationAtVersionQuery,
				GetLatestDecisionTreesQuery: GetLatestDecisionTreesQuery,
				GetLatestDecisionTreesPubOnlyQuery: GetLatestDecisionTreesPubOnlyQuery,
				GetLatestDecisionTreesMaxOnlyQuery: GetLatestDecisionTreesMaxOnlyQuery,
				GetLatestDecisionTreesUnpubOnlyQuery: GetLatestDecisionTreesUnpubOnlyQuery,
				GetLatestEnumerationSetQuery: GetLatestEnumerationSetQuery,
				GetLatestMessageSetQuery: GetLatestMessageSetQuery,
				GetLatestPublishedMessageSetQuery: GetLatestPublishedMessageSetQuery,
				GetLatestVariableSetQuery: GetLatestVariableSetQuery,
				GetMessageSetByMessageSetIdQuery: GetMessageSetByMessageSetIdQuery,
				GetMRUDecisionTreeQuery: GetMRUDecisionTreeQuery,
				GetNewGuidQuery: GetNewGuidQuery,
				GetNonTreeDocumentLockStatusQuery: GetNonTreeDocumentLockStatusQuery,
				GetPublishedMessageSetByMessageSetIdQuery: GetPublishedMessageSetByMessageSetIdQuery,
				GetVariableSetByVariableSetIdQuery: GetVariableSetByVariableSetIdQuery,
				ValidateRubyCodeQuery: ValidateRubyCodeQuery
			};
		}());
		return shared;
	}]);