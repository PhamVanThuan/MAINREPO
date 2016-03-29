'use strict';

/* Services */

angular.module('sahl.tools.app.services.searchManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$searchManager', ['$rootScope', '$codeMirrorService', function ($rootScope, $codeMirrorService) {

    var operations = {
        searchDocument: function (searchQuery, matchWholeWord, matchCase, filter) {
            $rootScope.searchResults = [];
            var searchIndex = -1;
            var filters = 'gi'
            if (matchCase) {
                filters = 'g';
            }
            if (matchWholeWord) {
                searchQuery = '\\b' + searchQuery + '\\b';
            }

            for (var i = 0; i < $rootScope.document.dataModel.nodeDataArray.length; i++) {
                var code = $rootScope.document.dataModel.nodeDataArray[i].code || '';
                var strippedCode = code.replace(/_sgl_quote_/g, "'").replace(/_newline_/g, "\n").replace(/_carriage_/g, "\r").replace(/_tab_/g, "\t").replace(/_quote_/g, "\"")
                                        
                var name = $rootScope.document.dataModel.nodeDataArray[i].text;
                var key = $rootScope.document.dataModel.nodeDataArray[i].key;

                var searchResult = {};

                if ((filter == null)||(filter == 'node')) {
                    var reNode = new RegExp(searchQuery, filters);

                    if (m = reNode.exec(name)) {
                        searchResult.nodeKey = key;
                        searchResult.nodeName = name;
                        searchResult.isCodeResult = false;
                        searchIndex++;
                        searchResult.searchIndex = searchIndex;
                        searchResult.searchQuery = searchQuery;
                        $rootScope.searchResults.push(searchResult);
                    }
                }

                if ((filter == null) || (filter == 'code')) {
                    var codeLines = strippedCode.split('\n');

                    var reCode = new RegExp(searchQuery, filters);
                    for (var lineNumber = 0; lineNumber < codeLines.length; lineNumber++) {
                        var searchResult = {};
                        if (codeLines[lineNumber] && codeLines[lineNumber].length != 0) {
                            var m = [];
                            while (m = reCode.exec(codeLines[lineNumber])) {
                                var searchResult = {};
                                searchResult.nodeKey = key;
                                searchResult.nodeName = name;
                                searchResult.nodeCodeIndex = m.index;
                                searchResult.nodeCodeLineNumber = lineNumber + 1;
                                searchResult.isCodeResult = true;
                                searchResult.searchQuery = searchQuery;
                                searchResult.codeLine = codeLines[lineNumber];
                                searchIndex++;
                                searchResult.searchIndex = searchIndex;
                                $rootScope.searchResults.push(searchResult);
                            }
                        }
                    }
                }

            }            

        },
        replaceAtSearchIndex: function (searchIndex, matchWholeWord, matchCase, replaceText) {
            var code = '';
            var searchResult = $rootScope.searchResults[searchIndex - 1];
            var searchLineNumber = $rootScope.searchResults[searchIndex - 1].nodeCodeLineNumber;
            var searchIndexNumber = $rootScope.searchResults[searchIndex - 1].nodeCodeIndex;
            var filters = 'gi'
            if (matchCase) {
                filters = 'g';
            }
            if (matchWholeWord) {
                searchQuery = '\\b' + searchResult.searchQuery + '\\b';
            }

            for (var i = 0; i < $rootScope.document.dataModel.nodeDataArray.length; i++) {
                if ($rootScope.document.dataModel.nodeDataArray[i].key == searchResult.nodeKey)
                {
                    var searchQuery = searchResult.searchQuery;
                    
                    var reCode = new RegExp(searchQuery, filters);
                    if (searchResult.isCodeResult) {
                        var codeLines = $rootScope.document.dataModel.nodeDataArray[i].code.split('\n');
                        var firstPartOfCode = codeLines[searchLineNumber - 1].substring(0, searchIndexNumber);
                        var codeToReplace = codeLines[searchLineNumber - 1].substring(searchIndexNumber, searchIndexNumber + searchQuery.length);
                        var restOfCode = codeLines[searchLineNumber - 1].substring(searchIndexNumber + searchQuery.length, codeLines[searchLineNumber - 1].length);
                        var replaceHalf =  codeToReplace.replace(reCode, replaceText);                        

                        codeLines[searchLineNumber - 1] = firstPartOfCode + replaceHalf + restOfCode;
                        for (var c = 0; c < codeLines.length; c++) {
                            if ((codeLines.length == 1) || (c == codeLines.length - 1))
                            {
                                code += codeLines[c];
                            } else
                            {
                                code += codeLines[c] + '\n';
                            }                            
                        }

                        $rootScope.document.dataModel.nodeDataArray[i].code = code;                        
                    } else {
                        $rootScope.document.dataModel.nodeDataArray[i].text = $rootScope.document.dataModel.nodeDataArray[i].text.replace(reCode, replaceText);
                    }
                }
            }
            return code;
        },
        highlightCode: function (index) {
            var lineNumber = $rootScope.searchResults[index].nodeCodeLineNumber;
            var codeIndex = $rootScope.searchResults[index].nodeCodeIndex;
            var searchQuerylength = $rootScope.searchResults[index].searchQuery.length;

            $codeMirrorService.setSelection(lineNumber, codeIndex, searchQuerylength);
            $codeMirrorService.scrollIntoView(lineNumber, codeIndex, searchQuerylength);     
        }

    }



    return {
        start: function () {

        },
        searchDocument: operations.searchDocument,
        replaceAtSearchIndex: operations.replaceAtSearchIndex,
        highlightCode: operations.highlightCode
    }

}])