'use strict';

angular.module('SAHL.Services.Interfaces.DecisionTree.sharedmodels', []).
	factory('$decisionTreeSharedModels', [function () {
		var shared = (function () {
					function QueryGlobalsVersion(VariablesVersion, MessagesVersion, EnumerationsVersion) {
				this.VariablesVersion = VariablesVersion;
				this.MessagesVersion = MessagesVersion;
				this.EnumerationsVersion = EnumerationsVersion;
				this._name = 'SAHL.DecisionTree.Shared.QueryGlobalsVersion,SAHL.DecisionTree.Shared';
				}

			return {
				QueryGlobalsVersion: QueryGlobalsVersion
			};
		}());
		return shared;
	}]).
	factory('$decisionTreeSharedModelsContainer', ['$decisionTreeSharedModels',function ($decisionTreeSharedModels) {
		var shared = (function () {
			var Container = {
				'SAHL.DecisionTree.Shared.QueryGlobalsVersion,SAHL.DecisionTree.Shared': $decisionTreeSharedModels.QueryGlobalsVersion
			}
			return {
				Container : Container
			};
		}());
		return shared;
	}]);