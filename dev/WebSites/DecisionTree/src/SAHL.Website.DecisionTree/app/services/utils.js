'use strict';

/* Services */

angular.module('sahl.tools.app.services.utils', [
    'sahl.tools.app.services'
])
.factory('$utils', ['$rootScope', '$q',
    function ($rootScope, $q) {
        var string = {
            capitaliseFirstLetter: function (s, sanitise) {
                var doSanitise = false;
                if (sanitise !== undefined) {
                    doSanitise = sanitise;
                }
                if (doSanitise) {
                    s = string.sanitise(s)
                }
                return s.charAt(0).toUpperCase() + s.slice(1);
            },

            smallFirstLetter: function (s, sanitise) {
                var doSanitise = false;
                if (sanitise !== undefined) {
                    doSanitise = sanitise;
                }
                if (doSanitise) {
                    s = string.sanitise(s)
                }

                return s.charAt(0).toLowerCase() + s.slice(1);
            },

            sanitise: function (s) {
                s = s.replace(/[^A-Za-z0-9:]/g, '');
                return s.replace(/ /g, '').replace(/_/g, '').replace(/g-/, '').replace(/\(/, '').replace(/\)/, '');
            },
            endsWith: function(inputString,suffix){
                return inputString.indexOf(suffix, inputString.length - suffix.length) !== -1;
            },

        }

        var json = {
            replaceInvalidJSONCharacters: function (rawData) {
                return rawData.replace(/'/g, "_sgl_quote_").replace(/\n/g, "_newline_").replace(/\r/g, "_carriage_").replace(/\t/g, "_tab_").replace(/"/g, "_quote_")
            },
            removeJSONEncodedCharacters: function (rawData) {
                return rawData.replace(/_sgl_quote_/g, "'").replace(/_newline_/g, "\n").replace(/_carriage_/g, "\r").replace(/_tab_/g, "\t").replace(/_quote_/g, "\"")
            }
        };

        return {
            string: string,
            json:json
        }
    }]);