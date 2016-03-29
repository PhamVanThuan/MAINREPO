'use strict';

/* Services */

angular.module('sahl.tools.app.services.statusbarManager', [

])
.factory('$statusbarManager', ['$rootScope', '$eventAggregatorService', '$eventDefinitions', '$compile', '$http', '$q',
    function ($rootScope, $eventAggregatorService, $eventDefinitions, $compile, $http, $q) {
        function getPanelName(name) {
            return "sbp-" + name.toLowerCase();
        }

        function getSegmentIdName(name) {
            return "sbps-" + name.toLowerCase() + "-id";
        }

        function getSegmentValueName(name) {
            return "sbps-" + name.toLowerCase() + "-value";
        }

        function getContent(templateURL) {
            var deferred = $q.defer();
            $http.get(templateURL).then(function (result) {
                deferred.resolve(result.data);
            }, function (error) {
                deferred.reject(error);
            })

            return deferred.promise;
        }

        return {
            addStatusbarSegmentedPanel: function (name, segments, position) {
                var panelHTML = "<div class='statusbar-panel place-" + position.toLowerCase() + "' id='" + getPanelName(name) + "'>";
                if (segments !== undefined) {
                    for (var i = 0; i < segments.length; i++) {
                        panelHTML += "<span class='statusbar-span' id=" + getSegmentIdName(segments[i].name) + ">" + segments[i].name + " :</span><span class='statusbar-span' id=" + getSegmentValueName(segments[i].name) + ">" + segments[i].value + "</span>";
                    }
                }

                panelHTML += "</div>";

                var statusbar = $("#statusbar");
                statusbar.append(panelHTML);
            },
            addStatusbarPanelWidget: function (name, widgetTemplateURL) {
                var selector = "#statusbar #" + getPanelName(name);
                var statusbarPanel = $(selector);
                getContent(widgetTemplateURL).then(function (templateContent) {
                    statusbarPanel.html(templateContent);
                    $compile(statusbarPanel.contents())($rootScope);
                })
            },
            removeStatusPanel: function (name) {
                var selector = "#statusbar #" + getPanelName(name);
                var statusbarPanel = $(selector);
                statusbarPanel.remove();
            },
            setStatusbarSegmentedPanelValue: function (panelName, segmentName, newSegmentValue) {
                var selector = "#statusbar #" + getPanelName(panelName);
                var selector1 = "#" + getSegmentValueName(segmentName);
                var statusbarPanel = $(selector).find(selector1);
                statusbarPanel.html(newSegmentValue);
            },
            addStatusbarTextPanel: function (name, position, initialText) {
                var panelHTML = "<div class='statusbar-panel place-" + position.toLowerCase() + "' id='" + getPanelName(name) + "'>";

                var newTextValue = initialText || '';
                panelHTML += "<span class='statusbar-span' id=" + getSegmentIdName("text") + ">" + newTextValue + "</span>";

                panelHTML += "</div>";

                var statusbar = $("#statusbar");
                statusbar.append(panelHTML);
            },
            setStatusbarTextPanelValue: function (panelName, newTextValue) {
                var selector = "#statusbar #" + getPanelName(panelName);
                var selector1 = "#" + getSegmentIdName("text");
                var statusbarPanel = $(selector).find(selector1);
                statusbarPanel.html(newTextValue);
            }
        }
    }]);