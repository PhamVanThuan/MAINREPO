'use strict';

angular.module('SAHL.Services.Interfaces.DecisionTreeDesign.commands', []).
	factory('$decisionTreeDesignCommands', [function () {
		var shared = (function () {
						function CloseLockForEnumerationsCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.CloseLockForEnumerationsCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function CloseLockForMessagesCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.CloseLockForMessagesCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function CloseLockForTreeCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.CloseLockForTreeCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function CloseLockForVariablesCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.CloseLockForVariablesCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function OpenLockForVariablesCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.OpenLockForVariablesCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function OpenLockForMessagesCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.OpenLockForMessagesCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function DeleteDecisionTreeCommand(decisionTreeId, decisionTreeVersionId, username) {
				this.decisionTreeId = decisionTreeId;
				this.decisionTreeVersionId = decisionTreeVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.DeleteDecisionTreeCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.decisionTreeId!==0 && !this.decisionTreeId) {
							results.push({'decisionTreeId': 'Decision Tree Id is required'});
						}
					if(this.decisionTreeVersionId!==0 && !this.decisionTreeVersionId) {
							results.push({'decisionTreeVersionId': 'Decision Tree Version Id is required'});
						}
					if(this.username!==0 && !this.username) {
							results.push({'username': 'Username is required'});
						}
					return results;
			};
		}
			function OpenLockForEnumerationsCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.OpenLockForEnumerationsCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function OpenLockForTreeCommand(documentVersionId, username) {
				this.documentVersionId = documentVersionId;
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.OpenLockForTreeCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function SaveThumbnailCommand(decisionTreeId, thumbnail) {
				this.decisionTreeId = decisionTreeId;
				this.thumbnail = thumbnail || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveThumbnailCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.thumbnail!==0 && !this.thumbnail) {
							results.push({'thumbnail': 'Thumbnail is required'});
						}
					if(this.decisionTreeId!==0 && !this.decisionTreeId) {
							results.push({'decisionTreeId': 'Decision Tree Id is required'});
						}
					return results;
			};
		}
			function SaveMRUDecisionTreeCommand(userName, treeId) {
				this.userName = userName || '';
				this.treeId = treeId;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveMRUDecisionTreeCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function SaveNewDecisionTreeVersionCommand(decisionTreeVersionId, decisionTreeId, version, data, username) {
				this.decisionTreeVersionId = decisionTreeVersionId;
				this.decisionTreeId = decisionTreeId;
				this.version = version;
				this.data = data || '';
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveNewDecisionTreeVersionCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.decisionTreeVersionId!==0 && !this.decisionTreeVersionId) {
							results.push({'decisionTreeVersionId': 'Decision Tree Version Id is required'});
						}
					if(this.version!==0 && !this.version) {
							results.push({'version': 'Version is required'});
						}
					if(this.decisionTreeId!==0 && !this.decisionTreeId) {
							results.push({'decisionTreeId': 'Decision Tree Id is required'});
						}
					if(this.data!==0 && !this.data) {
							results.push({'data': 'Data is required'});
						}
					if(this.username!==0 && !this.username) {
							results.push({'username': 'Username is required'});
						}
					return results;
			};
		}
			function SaveMRUDecisionTreePinStatusCommand(userName, treeId, pinned) {
				this.userName = userName || '';
				this.treeId = treeId;
				this.pinned = pinned;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveMRUDecisionTreePinStatusCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function SaveAndPublishDecisionTreeCommand(decisionTreeId, name, description, isActive, thumbnail, treeVersionId, data, publisher, saveFirst) {
				this.decisionTreeId = decisionTreeId;
				this.name = name || '';
				this.description = description || '';
				this.isActive = isActive;
				this.thumbnail = thumbnail || '';
				this.treeVersionId = treeVersionId;
				this.data = data || '';
				this.publisher = publisher || '';
				this.saveFirst = saveFirst;
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveAndPublishDecisionTreeCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.decisionTreeId!==0 && !this.decisionTreeId) {
							results.push({'decisionTreeId': 'Decision Tree Id is required'});
						}
					if(this.name!==0 && !this.name) {
							results.push({'name': 'Name is required'});
						}
					if(this.description!==0 && !this.description) {
							results.push({'description': 'Description is required'});
						}
					if(this.isActive!==0 && !this.isActive) {
							results.push({'isActive': 'Is Active is required'});
						}
					if(this.treeVersionId!==0 && !this.treeVersionId) {
							results.push({'treeVersionId': 'Tree Version Id is required'});
						}
					if(this.data!==0 && !this.data) {
							results.push({'data': 'Data is required'});
						}
					if(this.publisher!==0 && !this.publisher) {
							results.push({'publisher': 'Publisher is required'});
						}
					if(this.saveFirst!==0 && !this.saveFirst) {
							results.push({'saveFirst': 'Save First is required'});
						}
					if(this.thumbnail!==0 && !this.thumbnail) {
							results.push({'thumbnail': 'Thumbnail is required'});
						}
					return results;
			};
		}
			function SaveAndPublishEnumerationSetCommand(id, version, data, publisher) {
				this.id = id;
				this.version = version;
				this.data = data || '';
				this.publisher = publisher || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveAndPublishEnumerationSetCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function SaveAndPublishMessageSetCommand(messageSetId, version, data, publisher) {
				this.messageSetId = messageSetId;
				this.version = version;
				this.data = data || '';
				this.publisher = publisher || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveAndPublishMessageSetCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.messageSetId!==0 && !this.messageSetId) {
							results.push({'messageSetId': 'Message Set Id is required'});
						}
					if(this.version!==0 && !this.version) {
							results.push({'version': 'Version is required'});
						}
					if(this.data!==0 && !this.data) {
							results.push({'data': 'Data is required'});
						}
					if(this.publisher!==0 && !this.publisher) {
							results.push({'publisher': 'Publisher is required'});
						}
					return results;
			};
		}
			function SaveAndPublishVariableSetCommand(variableSetID, version, data, publisher) {
				this.variableSetID = variableSetID;
				this.version = version;
				this.data = data || '';
				this.publisher = publisher || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveAndPublishVariableSetCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function SaveDecisionTreeAsCommand(decisionTreeId, name, description, isActive, decisionTreeVersionId, data, username, thumbnail) {
				this.decisionTreeId = decisionTreeId;
				this.name = name || '';
				this.description = description || '';
				this.isActive = isActive;
				this.decisionTreeVersionId = decisionTreeVersionId;
				this.data = data || '';
				this.username = username || '';
				this.thumbnail = thumbnail || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveDecisionTreeAsCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.decisionTreeId!==0 && !this.decisionTreeId) {
							results.push({'decisionTreeId': 'Decision Tree Id is required'});
						}
					if(this.name!==0 && !this.name) {
							results.push({'name': 'Name is required'});
						}
					if(this.description!==0 && !this.description) {
							results.push({'description': 'Description is required'});
						}
					if(this.isActive!==0 && !this.isActive) {
							results.push({'isActive': 'Is Active is required'});
						}
					if(this.thumbnail!==0 && !this.thumbnail) {
							results.push({'thumbnail': 'Thumbnail is required'});
						}
					if(this.decisionTreeVersionId!==0 && !this.decisionTreeVersionId) {
							results.push({'decisionTreeVersionId': 'Decision Tree Version Id is required'});
						}
					if(this.data!==0 && !this.data) {
							results.push({'data': 'Data is required'});
						}
					if(this.username!==0 && !this.username) {
							results.push({'username': 'Username is required'});
						}
					return results;
			};
		}
			function SaveDecisionTreeVersionCommand(decisionTreeId, name, description, isActive, thumbnail, decisionTreeVersionId, data, username) {
				this.decisionTreeId = decisionTreeId;
				this.name = name || '';
				this.description = description || '';
				this.isActive = isActive;
				this.thumbnail = thumbnail || '';
				this.decisionTreeVersionId = decisionTreeVersionId;
				this.data = data || '';
				this.username = username || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveDecisionTreeVersionCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.decisionTreeId!==0 && !this.decisionTreeId) {
							results.push({'decisionTreeId': 'Decision Tree Id is required'});
						}
					if(this.name!==0 && !this.name) {
							results.push({'name': 'Name is required'});
						}
					if(this.description!==0 && !this.description) {
							results.push({'description': 'Description is required'});
						}
					if(this.isActive!==0 && !this.isActive) {
							results.push({'isActive': 'Is Active is required'});
						}
					if(this.thumbnail!==0 && !this.thumbnail) {
							results.push({'thumbnail': 'Thumbnail is required'});
						}
					if(this.decisionTreeVersionId!==0 && !this.decisionTreeVersionId) {
							results.push({'decisionTreeVersionId': 'Decision Tree Version Id is required'});
						}
					if(this.data!==0 && !this.data) {
							results.push({'data': 'Data is required'});
						}
					if(this.username!==0 && !this.username) {
							results.push({'username': 'Username is required'});
						}
					return results;
			};
		}
			function SaveEnumerationSetCommand(id, version, data) {
				this.id = id;
				this.version = version;
				this.data = data || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveEnumerationSetCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}
			function SaveMessageSetCommand(messageSetid, version, data) {
				this.messageSetid = messageSetid;
				this.version = version;
				this.data = data || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveMessageSetCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				this.Validate = function() {
					var results = [];
					if(this.messageSetId!==0 && !this.messageSetId) {
							results.push({'messageSetId': 'Message Set Id is required'});
						}
					if(this.version!==0 && !this.version) {
							results.push({'version': 'Version is required'});
						}
					if(this.data!==0 && !this.data) {
							results.push({'data': 'Data is required'});
						}
					return results;
			};
		}
			function SaveVariableSetCommand(id, version, data) {
				this.id = id;
				this.version = version;
				this.data = data || '';
				this._name = 'SAHL.Services.Interfaces.DecisionTreeDesign.Commands.SaveVariableSetCommand,SAHL.Services.Interfaces.DecisionTreeDesign';
				}

			return {
				CloseLockForEnumerationsCommand: CloseLockForEnumerationsCommand,
				CloseLockForMessagesCommand: CloseLockForMessagesCommand,
				CloseLockForTreeCommand: CloseLockForTreeCommand,
				CloseLockForVariablesCommand: CloseLockForVariablesCommand,
				OpenLockForVariablesCommand: OpenLockForVariablesCommand,
				OpenLockForMessagesCommand: OpenLockForMessagesCommand,
				DeleteDecisionTreeCommand: DeleteDecisionTreeCommand,
				OpenLockForEnumerationsCommand: OpenLockForEnumerationsCommand,
				OpenLockForTreeCommand: OpenLockForTreeCommand,
				SaveThumbnailCommand: SaveThumbnailCommand,
				SaveMRUDecisionTreeCommand: SaveMRUDecisionTreeCommand,
				SaveNewDecisionTreeVersionCommand: SaveNewDecisionTreeVersionCommand,
				SaveMRUDecisionTreePinStatusCommand: SaveMRUDecisionTreePinStatusCommand,
				SaveAndPublishDecisionTreeCommand: SaveAndPublishDecisionTreeCommand,
				SaveAndPublishEnumerationSetCommand: SaveAndPublishEnumerationSetCommand,
				SaveAndPublishMessageSetCommand: SaveAndPublishMessageSetCommand,
				SaveAndPublishVariableSetCommand: SaveAndPublishVariableSetCommand,
				SaveDecisionTreeAsCommand: SaveDecisionTreeAsCommand,
				SaveDecisionTreeVersionCommand: SaveDecisionTreeVersionCommand,
				SaveEnumerationSetCommand: SaveEnumerationSetCommand,
				SaveMessageSetCommand: SaveMessageSetCommand,
				SaveVariableSetCommand: SaveVariableSetCommand
			};
		}());
		return shared;
	}]);