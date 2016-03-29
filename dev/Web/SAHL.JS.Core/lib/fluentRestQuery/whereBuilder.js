angular.module('sahl.js.core.fluentRestQuery').factory('$fluentWhereBuilder', [function () {
    return function (fieldName, originalFluentBuilderCallback) {

        var internals = {
            checkingFieldName: fieldName,
            logicalOperatorOptions: {AND: {AND: {}}, OR: {OR: {}}},
            chosenLogicalOperatorOption: null,
            innerWhereJson: {},
            addToFilter: function (type, name, value) {
                if (!internals.innerWhereJson[type]) {
                    internals.innerWhereJson[type] = {};
                }
                internals.innerWhereJson[type][name] = value;
            }

        };


        var functionsForWhereBuilder = {
            greaterThan: function (comparedValue) {
                internals.addToFilter('gt', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            lessThan: function (comparedValue) {
                internals.addToFilter('lt', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            greaterThanOrEquals: function (comparedValue) {
                internals.addToFilter('gte', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            lessThanOrEquals: function (comparedValue) {
                internals.addToFilter('lte', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            startsWith: function (comparedValue) {
                internals.addToFilter('startswith', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            endsWith: function (comparedValue) {
                internals.addToFilter('endswith', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            contains: function (comparedValue) {
                internals.addToFilter('contains', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            isLike: function (comparedValue) {
                internals.addToFilter('like', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            isEqual: function (comparedValue) {
                internals.addToFilter('eq', internals.checkingFieldName, comparedValue);
                return functionsForWhereBuilder;
            },
            andWhere: function (fieldName) {
                if (internals.chosenLogicalOperatorOption === null) {
                    internals.chosenLogicalOperatorOption = internals.logicalOperatorOptions.AND;
                }
                else if (internals.chosenLogicalOperatorOption === internals.logicalOperatorOptions.OR) {
                    throw new Error("can currently only build with and OR or operations exclusively");
                }
                internals.checkingFieldName = fieldName;
                return functionsForWhereBuilder;
            },
            orWhere: function (fieldName) {
                if (internals.chosenLogicalOperatorOption === null) {
                    internals.chosenLogicalOperatorOption = internals.logicalOperatorOptions.OR;
                }
                else if (internals.chosenLogicalOperatorOption === internals.logicalOperatorOptions.AND) {
                    throw new Error("can currently only build with and OR or operations exclusively");
                }
                internals.checkingFieldName = fieldName;
                return functionsForWhereBuilder;
            },
            endWhere: function () {
                if (!internals.chosenLogicalOperatorOption) {
                    return originalFluentBuilderCallback(internals.innerWhereJson);
                }
                var returningJson = internals.chosenLogicalOperatorOption;
                returningJson[Object.keys(returningJson)[0]] = internals.innerWhereJson;
                return originalFluentBuilderCallback(returningJson);
            }
        };

        return functionsForWhereBuilder;

    };
}]);
