'use strict';

angular.module('sahl.websites.halo.services.entityBreadcrumbManagement', [])
    .service('$entityBreadcrumbManagerService',
        ['$rootScope',
        function($rootScope) {
            var breadcrumbObject = function() {
                this.crumbs = [];
                var bo = this;
                this.reset = function() {
                    bo.crumbs = [];
                };

                this.push = function (name, state, data,isRoot) {
                    bo.crumbs.push({
                        name: name,
                        state: state,
                        data: data,
                        isRoot:isRoot
                    });
                };

                this.pop = function () {
                    bo.crumbs.pop();
                };

                this.goToIndex = function(index) {
                    bo.crumbs.splice(index + 1);
                    return bo.crumbs[index];
                };

                return this;
            };

            return {
                start: function() {
                    $rootScope.breadcrumbs = {
                        forEntity: function (entity) {
                            if (entity){
                                if (!entity.breadcrumbs) {
                                    entity.breadcrumbs = new breadcrumbObject();
                                } else {
                                    if (!entity.breadcrumbs.crumbs[entity.breadcrumbs.crumbs.length - 1].isRoot) {
                                        entity.breadcrumbs.pop();
                                    }
                                }
                            }
                        }
                    };
                }
            };
        }]);
