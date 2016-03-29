'use strict';
angular.module('sahl.websites.halo.services.acronymManager', [
])
.service('$acronymManagerService', [
    function () {
        var acronyms = [{acronym:"SPV", displayName: "SPV"}];
        var operations = {
            isAcronym: function (item) { 
                ; 
                return _.size(_.where(acronyms, { acronym: item.toUpperCase() })) > 0;
            },
            getDisplayName: function (searchItem) {
                if (this.isAcronym(searchItem)) {
                    return _.where(acronyms, { acronym: searchItem.toUpperCase() })[0].displayName;
                }
                else {
                    return null;
                }
            }
            };

    	return {
    	    isAcronym: operations.isAcronym,
    	    getDisplayName: operations.getDisplayName
    	};
	}
]);
