'use strict';
angular.module('sahl.js.core.lookup', [])
    .service('$lookupService', ['$q', function ($q) {
        var operations = {
            getByLookupType: function (lookupType) {},
            getByLookupTypeId: function (lookupType, id) {},
            getByLineItemCategoryId: function (id) {}
        };
        return {
            getByLookupType: operations.getByLookupType,
            getByLookupTypeId: operations.getByLookupTypeId,
            getByLineItemCategoryId:operations.getByLineItemCategoryId
        };
    }]);
