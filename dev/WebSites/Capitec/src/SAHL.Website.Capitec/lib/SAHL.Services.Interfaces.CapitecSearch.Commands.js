'use strict';

angular.module('SAHL.Services.Interfaces.CapitecSearch.commands', []).
	factory('$capitecSearchCommands', [function () {
		var shared = (function () {
						function RefreshLuceneIndexCommand() {
				this._name = 'SAHL.Services.Interfaces.CapitecSearch.Commands.RefreshLuceneIndexCommand,SAHL.Services.Interfaces.CapitecSearch';
				}

			return {
				RefreshLuceneIndexCommand: RefreshLuceneIndexCommand
			};
		}());
		return shared;
	}]);