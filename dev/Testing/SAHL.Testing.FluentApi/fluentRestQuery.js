angular.module('sahl.js.core.fluentRestQuery').factory('$fluentRestQuery', ['$fluentWhereBuilder', '$orderByDirectionBuilder', function ($fluentWhereBuilder, $orderByDirectionBuilder) {
    return function (controllerRoute) {

        var internals = {
            route: (function () {
                if (!controllerRoute.parameters) {
                    return controllerRoute.route;
                }
                return controllerRoute.route.replace(/{(\d+)}/g, function (match, number) {
                    return typeof(controllerRoute.parameters[number])? controllerRoute.parameters[number]: match;
                });
            })(),
            whereBuildingTemplate: $fluentWhereBuilder,
            filterJson: {},
            pagingJson: {},
            //from stack overflow http://stackoverflow.com/a/4994244
            isEmpty: function (obj) {
                for (var key in obj) {
                    if (hasOwnProperty.call(obj, key)) {
                        return false;
                    }
                }
                return true;
            },
            filterJsonIsEmpty: function () {
                return internals.isEmpty(internals.filterJson);
            },
            pagingJsonIsEmpty: function () {
                return internals.isEmpty(internals.pagingJson);
            },
            isOrdered: function () {
                return internals.filterJson.order;
            }
        };

        var queryStringCompiler = {
            queryString: '',
            compilingInQueryString: ['filter', 'paging'],
            compileNewQueryString: function () {
                queryStringCompiler.queryString = '';

                queryStringCompiler.compilingInQueryString.forEach(function (item) {
                    if (!internals[(item + 'JsonIsEmpty')]()) {
                        queryStringCompiler.appendToQueryString(item + '=' + angular.toJson(internals[item + 'Json']));
                    }
                });
            },
            appendToQueryString: function (additional) {
                queryStringCompiler.queryString += (queryStringCompiler.queryString === '' ? '?' : '&') + additional;
            }
        };


        var pagingBuilder = {
            paging: function (pageNumber, pageSize) {
                internals.pagingJson = {Paging: {currentPage: pageNumber, pageSize: pageSize}};
            }
        };

        var filterBuilder = {
            limit: function (num) {
                if (internals.isOrdered()) {
                    internals.filterJson.limit = num;
                }
                else {
                    throw new Error('Query must first be ordered');
                }
            },
            skip: function (num) {
                if (internals.isOrdered()) {
                    internals.filterJson.skip = num;
                }
                else {
                    throw new Error('Query must first be ordered');
                }
            },
            fields: function () {
                if (!internals.filterJson.fields) {
                    internals.filterJson.fields = {};
                }
                var counter = 0;
                var argumentsLength = arguments.length;
                for (; counter < argumentsLength; counter++) {
                    var argument = arguments[counter];
                    internals.filterJson.fields[argument] = true;
                }
            },
            include: function (arrayOfRelationships) {
                arrayOfRelationships.forEach(function (item) {
                    if (!internals.filterJson.include) {
                        internals.filterJson.include = '';
                    }
                    else {
                        internals.filterJson.include += ', ';
                    }
                    internals.filterJson.include += item;
                });
            }
        };

        var thisInstance = this;

        this.paging = function (pageNumber, pageSize) {
            pagingBuilder.paging(pageNumber, pageSize);
            return thisInstance;
        };
        this.limit = function (toAmount) {
            filterBuilder.limit(toAmount);
            return thisInstance;
        };
        this.skip = function (numberToSkip) {
            filterBuilder.skip(numberToSkip);
            return thisInstance;
        };
        this.order = function (placementInOrdering, fieldName) {
            return new $orderByDirectionBuilder(function (direction) {
                if (!internals.filterJson.order) {
                    internals.filterJson.order = {};
                }
                internals.filterJson.order[placementInOrdering] = fieldName + ' ' + direction;
                return thisInstance;
            });
        };
        this.where = function (fieldName) {
            return new internals.whereBuildingTemplate(fieldName, function (whereJson) {
                internals.filterJson.where = whereJson;
                return thisInstance;
            });
        };
        this.fields = function () {
            filterBuilder.fields.apply(this, arguments);
            return thisInstance;
        };
        this.include = function (arrayOfRelationships) {
            filterBuilder.include(arrayOfRelationships);
            return thisInstance;
        };
        this.compiledUrl = function () {
            queryStringCompiler.compileNewQueryString();

            return internals.route + queryStringCompiler.queryString;
        };
    };
}]);