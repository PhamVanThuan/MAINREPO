'use strict';

angular.module('SAHL.Services.Interfaces.CapitecSearch.searchqueries', []).
	factory('$capitecSearchSearchQueries', [function () {
		var shared = (function () {
						function ApplicationStatusQuery(applicationNumber, identityNumberList) {
				this.applicationNumber = applicationNumber || '';
				this.identityNumberList = identityNumberList || '';
				this._name = 'SAHL.Services.Interfaces.CapitecSearch.Queries.ApplicationStatusQuery,SAHL.Services.Interfaces.CapitecSearch';
				}

			return {
				ApplicationStatusQuery: ApplicationStatusQuery
			};
		}());
		return shared;
	}]);