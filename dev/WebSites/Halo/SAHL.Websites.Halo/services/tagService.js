'use strict';
angular.module('sahl.websites.halo.services.tagManagement', [
    'halo.webservices',
    'sahl.js.core.logging',
    'SAHL.Services.Interfaces.WorkflowTask.commands',
    'SAHL.Services.Interfaces.WorkflowTask.queries',
    'sahl.js.core.userManagement',
    'sahl.js.core.eventAggregation'

]).service('$tagService', ['$q', '$workflowTaskWebService', '$workflowTaskCommands', '$workflowTaskQueries', '$userManagerService', '$eventAggregatorService','$toastManagerService',
    function ($q, $workflowTaskWebService, $workflowTaskCommands, $workflowTaskQueries, $userManagerService, $eventAggregatorService, $toastManagerService) {

        // colours are chosen in priority from zero index
        var colourList = [
        { 'color': '#2ecc71', 'background-color': 'transparent' },
        { 'color': '#3498db', 'background-color': 'transparent' },
        { 'color': '#9b59b6', 'background-color': 'transparent' },
        { 'color': '#f1c40f', 'background-color': 'transparent' },
        { 'color': '#e74c3c', 'background-color': 'transparent' },
        { 'color': '#1abc9c', 'background-color': 'transparent' },
        { 'color': '#34495e', 'background-color': 'transparent' },
        { 'color': '#16a085', 'background-color': 'transparent' },
        { 'color': '#27ae60', 'background-color': 'transparent' },
        { 'color': '#2980b9', 'background-color': 'transparent' },
        { 'color': '#8e44ad', 'background-color': 'transparent' },
        { 'color': '#2c3e50', 'background-color': 'transparent' },
        { 'color': '#e67e22', 'background-color': 'transparent' },
        { 'color': '#95a5a6', 'background-color': 'transparent' },
        { 'color': '#f39c12', 'background-color': 'transparent' },
        { 'color': '#d35400', 'background-color': 'transparent' },
        { 'color': '#c0392b', 'background-color': 'transparent' }
        ];

        var maxTagsPerUser = 5;

        var allUserTags = [];
        var defaultSearchTags = [
            { tag: "Favourite" },
            { tag: "Important" },
            { tag: "Minor" }];

        var validResponse = function (response) {
            return response &&
                response.data &&
                response.data.ReturnData &&
                response.data.ReturnData.Results &&
                response.data.ReturnData.Results.$values;
        };

        var internal = {
            tags: {},
            tagsLoaded: false,
            filteringTags: [],
            updateTags: function (tags) {
                internal.tags = tags || internal.tags;
                allUserTags = internal.tags;
                $eventAggregatorService.publish(internal.publishingEvents.TAGSUPDATED, internal.tags);
            },
            validResponse: function (response) {
                return response &&
                    response.data &&
                    response.data.ReturnData &&
                    response.data.ReturnData.Results &&
                    response.data.ReturnData.Results.$values;
            },
            getComb: function () {
                var deferred = $q.defer();
                var query = new $workflowTaskQueries.GetNewTagIdQuery();
                $workflowTaskWebService.sendQueryAsync(query).then(function (combResponse) {
                    if (internal.validResponse(combResponse) && combResponse.data.ReturnData.Results.$values[0].TagID) {
                        deferred.resolve(combResponse.data.ReturnData.Results.$values[0].TagID);
                    }
                    else {
                        deferred.reject();
                    }

                }, deferred.reject);
                return deferred.promise;
            },
            saveTag: function (id, caption, foreColour, backColour, username) {
                var command = new $workflowTaskCommands.CreateTagForUserCommand(id, caption, backColour, foreColour, username);
                return $workflowTaskWebService.sendCommandAsync(command);
            },
            DeleteTagForUser: function (id) {
                var command = new $workflowTaskCommands.DeleteTagForUserCommand(id);
                return $workflowTaskWebService.sendCommandAsync(command);
            },
            UpdateUserTag: function (id, caption, foreColour, backColour) {
                var command = new $workflowTaskCommands.UpdateUserTagCommand(id, foreColour, backColour, caption);
                return $workflowTaskWebService.sendCommandAsync(command);
            },
            GetAllTagsForUserQuery: function (username) {
                var deferred = $q.defer();
                var query = new $workflowTaskQueries.GetAllTagsForUserQuery(username);
                $workflowTaskWebService.sendQueryAsync(query).then(function (tagsResponse) {
                    if (validResponse(tagsResponse) && tagsResponse.data.ReturnData.Results.$values[0].Tags) {
                        var tagsUnMapped = tagsResponse.data.ReturnData.Results.$values[0].Tags;
                        internal.updateTags(tagsUnMapped);
                        internal.tagsLoaded = true;
                        deferred.resolve(internal.tags);
                    }
                    else {
                        deferred.reject();
                    }

                }, deferred.reject);
                return deferred.promise;
            },
            linkTagToGeneric: function (tagId, foreignKey, username) {
                var command = new $workflowTaskCommands.AddTagToWorkflowInstanceCommand(tagId, foreignKey, username);
                return $workflowTaskWebService.sendCommandAsync(command);
            },
            unlinkTagFromGeneric: function (tagId, foreignKey) {
                var deferred = $q.defer();
                var command = new $workflowTaskCommands.DeleteTagFromWorkflowInstanceCommand(tagId, foreignKey);
                $workflowTaskWebService.sendCommandAsync(command).then(deferred.resolve, deferred.reject);
                return deferred.promise;
            },
            updateFilter: function (filteringTags) {
                internal.filteringTags = filteringTags || internal.filteringTags;
                $eventAggregatorService.publish(internal.publishingEvents.FILTERUPDATED, internal.filteringTags);
            },
            publishingEvents: {
                TAGSUPDATED: 'TagArrayItemUpdated',
                FILTERUPDATED: 'FilterArrayItemsUpdated'
            }
        };

        var operations = {
            events: internal.publishingEvents,
            createTagObject: function (caption, foreColour, backColour, tagId) {
                caption = caption || "Text";
                var tagStyle = {};
                if (foreColour || backColour) {
                    foreColour = foreColour || "#000000";
                    backColour = backColour || "#FFFFFF";
                    tagStyle = { 'background-color': backColour, 'color': foreColour };
                } else {
                    tagStyle = _.filter(colourList, function (style) {
                        var tagss = internal.tags;
                        for (var tag in tagss) {
                            if (internal.tags[tag] && internal.tags[tag].Style && internal.tags[tag].Style.color && internal.tags[tag].Style.color == style.color) {
                                return false;
                            }
                        }
                        return true;
                    })[0];
                }
                return { Caption: caption, Style: tagStyle, Id: tagId };
            }, getTags: function () {
                var deferred = $q.defer();
                if (!internal.tagsLoaded) {
                    var username = $userManagerService.getAuthenticatedUser().fullAdName;
                    internal.GetAllTagsForUserQuery(username).then(function () {
                        deferred.resolve(internal.tags);
                    }, deferred.reject);

                } else {
                    deferred.resolve(internal.tags);
                }
                return deferred.promise;
            },
            addTag: function (caption, foreColour, backColour) {
                var deferred = $q.defer();
                var username = $userManagerService.getAuthenticatedUser().fullAdName;
                internal.getComb().then(function (comb) {
                    internal.saveTag(comb, caption, foreColour, backColour, username).then(function () {
                        internal.tags[comb] = operations.createTagObject(caption, foreColour, backColour, comb);
                        internal.updateTags();
                        deferred.resolve(comb, internal.tags);
                    }, deferred.reject);
                }, deferred.reject);
                return deferred.promise;
            },
            addTag2: function (caption, comb, foreColour, backColour) {
                var deferred = $q.defer();
                var username = $userManagerService.getAuthenticatedUser().fullAdName;

                internal.saveTag(comb, caption, foreColour, backColour, username).then(function () {
                    internal.tags[comb] = operations.createTagObject(caption, foreColour, backColour, comb);
                    internal.updateTags();

                    deferred.resolve(comb, internal.tags);
                }, function (errorMsg) {
                    var errorMsgs = errorMsg.data.SystemMessages.AllMessages.$values;
                    for (var i = 0; i < errorMsgs.length; i++) {
                        $toastManagerService.error({
                            title: 'Error',
                            text: errorMsgs[i].Message
                        });
                    }
                    deferred.reject(errorMsg);
                });

                return deferred.promise;
            },
            createNewTag: function (foreColour, backColour, tagArrayLength, caption) {
                var deferred = $q.defer();
                var username = $userManagerService.getAuthenticatedUser().fullAdName;

                if ((Object.keys(internal.tags).length - 1) >= maxTagsPerUser) {
                    deferred.reject({ tags: internal.tags, newTag: null });
                };

                internal.getComb().then(function (comb) {
                    var tag = operations.createTagObject('', foreColour, backColour, comb);
                    deferred.resolve({ tags: internal.tags, newTag: tag });
                }, deferred.reject);
                return deferred.promise;
            },
            updateTag: function (id, caption, foreColour, backColour) {
                var deferred = $q.defer();
                internal.UpdateUserTag(id, caption, foreColour, backColour).then(function () {
                    //update tag in tags
                    internal.tags[id] = operations.createTagObject(caption, foreColour, backColour, id);
                    internal.updateTags();
                    deferred.resolve(internal.tags);
                }, deferred.reject);
                return deferred.promise;
            },
            deleteTag: function (id) {
                var deferred = $q.defer();
                internal.DeleteTagForUser(id).then(function () {
                    operations.filterWithout(id);
                    delete internal.tags[id];
                    internal.updateTags();
                    deferred.resolve(internal.tags);
                }, deferred.reject);

                return deferred.promise;
            },
            linkTag: function (tagId, foreignKey, pushToArray) {
                var deferred = $q.defer();
                var username = $userManagerService.getAuthenticatedUser().fullAdName;
                if (_.contains(pushToArray, tagId)) {
                    deferred.resolve();
                    return deferred.promise;
                }
                internal.linkTagToGeneric(tagId, foreignKey, username).then(function () {
                    pushToArray.push(tagId);
                    deferred.resolve(pushToArray);
                }, deferred.reject);
                return deferred.promise;
            },
            unlinkTag: function (tagId, foreignKey, takeFromArray) {
                var deferred = $q.defer();
                internal.unlinkTagFromGeneric(tagId, foreignKey).then(function () {
                    takeFromArray = _.filter(takeFromArray, function (item) {
                        return item !== tagId;
                    });
                    deferred.resolve(takeFromArray);
                }, deferred.reject);
                return deferred.promise;
            },
            filterWith: function (tagId) {
                internal.filteringTags.push(tagId);
                internal.updateFilter();
            },
            filterWithout: function (tagId) {
                var index = internal.filteringTags.indexOf(tagId);
                if (index > -1) {
                    internal.filteringTags.splice(index, 1);
                    internal.updateFilter();
                }
            },
            clearFilter: function () {
                internal.updateFilter([]);
            },
            filteringTags: function (filtering) {
                if (!filtering) {
                    return internal.filteringTags;
                } else {
                    internal.updateFilter(filtering);
                }
            },
            getMaxTagsPerUser: function () {
                return maxTagsPerUser;
            },
            getDefaultSearchTags: function () {
                return defaultSearchTags;
            },
            getAllUserTags: function () {
                return allUserTags;
            }
        };

        return {
            createTagAsObject: operations.createTagObject,
            predefinedColours: colourList,
            getTags: operations.getTags,
            addTag: operations.addTag,
            addTag2: operations.addTag2,
            createNewTag: operations.createNewTag,
            updateTag: operations.updateTag,
            deleteTag: operations.deleteTag,
            linkTag: operations.linkTag,
            unlinkTag: operations.unlinkTag,
            addTagToFiltering: operations.filterWith,
            removeTagFromFiltering: operations.filterWithout,
            clearTagFiltering: operations.clearFilter,
            allTags: internal.tags,
            getFilter: operations.filteringTags,
            getEvents: operations.events,
            getMaxTagsPerUser: operations.getMaxTagsPerUser,
            getDefaultSearchTags: operations.getDefaultSearchTags,
            getAllUserTags: operations.getAllUserTags
        };
    }]);