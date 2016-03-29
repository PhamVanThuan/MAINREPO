angular.module('sahl.tools.app.services.codeMirrorService', [
    'sahl.tools.app.services.wordSearchService'
]).factory('$codeMirrorService', ['$q', '$rootScope', '$statusbarManager', '$eventAggregatorService', '$eventDefinitions', '$decisionTreeDesignQueries', '$queryManager', '$wordSearchService','$utils',
function ($q, $rootScope, $statusbarManager, $eventAggregatorService, $eventDefinitions, $decisionTreeDesignQueries, $queryManager, $wordSearchService,$utils) {
    var eventHandlers = {
        keyIgnoreList: [9, 13, 16, 17, 27, 32, 33, 34, 35, 36, 37, 38, 39, 40],
        onBeforeNodeSelectionChanged: function (eventMessage) {
            operations.updateNodeCode(eventMessage);
            editor.codeMirror.setValue("");
        },
        onNodeSelectionChanged: function (eventMessage) {
            var codeMirrorText = "";
            if (eventMessage && eventMessage.data) {
                if (!eventMessage.data.code)
                    eventMessage.data.code = "";
                codeMirrorText = $utils.json.removeJSONEncodedCharacters(eventMessage.data.code);
                editor.codeMirror.setOption('readOnly', false);
            }
            else {
                codeMirrorText = "";
                editor.codeMirror.setOption('readOnly', 'nocursor');
            }
            editor.codeMirror.setValue(codeMirrorText);
            editor.codeMirror.refresh();
        },
        onBeforeDocumentSaved: function (eventMessage) {
            var currentNode = $rootScope.document.selectionManager.current;
            operations.updateNodeCode(currentNode);
        },
        codeMirrorKeyEvent: function (sourceControl, keyboardEvent) {
            if (keyboardEvent.type == "keyup" && eventHandlers.keyIgnoreList.indexOf(keyboardEvent.keyCode) === -1) {
                CodeMirror.commands.autocomplete(sourceControl);
            }
        },
        onDocumentVariableChanged: function (updatedVariable) {
            var prefix = "Variables::" + updatedVariable.usage + "s.";
            if (updatedVariable.action == "change" || updatedVariable.action == "remove")
                operations.wordSearchService.remove(prefix + $utils.string.sanitise(updatedVariable.oldName));
            if (updatedVariable.action == "change" || updatedVariable.action == "add")
                operations.wordSearchService.add(prefix + $utils.string.sanitise(updatedVariable.name));
        }
    };
    var editor = {
        codeMirrorHasChanges:"",
        codeMirror: undefined,
        codeMirrorSettings: {
            lineNumbers: true,
            mode: "text/x-ruby",
            tabMode: "indent",
            matchBrackets: true,
            indentUnit: 2,
            onKeyEvent: eventHandlers.codeMirrorKeyEvent
        },
        setupCodeEditorAndRefresh :function (textAreaElement) {
            if(!editor.codeMirror){
                editor.codeMirror = CodeMirror.fromTextArea(textAreaElement, editor.codeMirrorSettings);
                editor.codeMirror.setSize('100%', '100%');
                editor.codeMirror.focus();
                editor.attachEvents();
                editor.setupCursorStatusBarUpdate();
            }
            editor.codeMirror.refresh();
        },
        setupCursorStatusBarUpdate: function () {
            editor.codeMirror.on("cursorActivity", function () {
                var cursorPos = editor.codeMirror.getCursor();
                $statusbarManager.setStatusbarSegmentedPanelValue("textPosition", "Ln", cursorPos.line);
                $statusbarManager.setStatusbarSegmentedPanelValue("textPosition", "Col", cursorPos.ch+1);
                $statusbarManager.setStatusbarSegmentedPanelValue("textSelection", "Sel", editor.codeMirror.getSelection().length);
            })
        },
        attachEvents: function () {
            $eventAggregatorService.subscribe($eventDefinitions.onBeforeNodeSelectionChanged, eventHandlers.onBeforeNodeSelectionChanged);
            $eventAggregatorService.subscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.onNodeSelectionChanged);
            $eventAggregatorService.subscribe($eventDefinitions.onBeforeDocumentSaved, eventHandlers.onBeforeDocumentSaved);
            $eventAggregatorService.subscribe($eventDefinitions.onDocumentVariableChanged, eventHandlers.onDocumentVariableChanged);
        },
        detachEvents: function () {
            $eventAggregatorService.unsubscribe($eventDefinitions.onBeforeNodeSelectionChanged, eventHandlers.onBeforeNodeSelectionChanged);
            $eventAggregatorService.unsubscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.onNodeSelectionChanged);
            $eventAggregatorService.unsubscribe($eventDefinitions.onBeforeDocumentSaved, eventHandlers.onBeforeDocumentSaved);
            $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentVariableChanged, eventHandlers.onDocumentVariableChanged);
        },
        customComplete: function (editorCtrl, options) {
            var cur = editorCtrl.getCursor();
            var token = editorCtrl.getTokenAt(cur);
            var list = [];
            if (token.type !== "string" || token.string.length === 1) {
                if (token.end !== 0){
                    var fullTokenStringWithContext = operations.getFullTokenText(editorCtrl, token, cur);
                    list = operations.findCurrentFullCodeToken(fullTokenStringWithContext);
                }
            }
            var replace = editor.getWordReplace(editorCtrl);
            return { list: list, from: CodeMirror.Pos(replace.line, replace.start), to: CodeMirror.Pos(replace.line, replace.end) };
        },
        getWordReplace: function (editorCtrl) {
            var cur = editorCtrl.getCursor();
            var token = editorCtrl.getTokenAt(cur);
            var replacement = {
                line: cur.line,
                start: token.start,
                end: token.end
            }
            if (token.string == "::" || token.string == ".") {
                replacement.start = token.end;
            }
            return replacement;
        },
        autoComplete: function (editorCtrl, isNested) {
            CodeMirror.showHint(editorCtrl, CodeMirror.hint.customComplete, { completeSingle: false });
        }
    };
    var operations = {
        wordSearchService: undefined,
        loadwordSearchService: function () {
            var deferred = $q.defer();
            var query = new $decisionTreeDesignQueries.GetCodeEditorKeywordsQuery();
            $queryManager.sendQueryAsync(query).then(function (data) {
                operations.wordSearchService = $wordSearchService.Initilized(angular.fromJson(data.data.ReturnData.Results.$values[0].Data));
                deferred.resolve();
            },deferred.reject);
            return deferred.promise;
        },
        findCurrentFullCodeToken: function (tokenString) {
            var array = operations.wordSearchService.startsWith(tokenString);
            return array;
        },
        getFullTokenText: function (editorCtrl, token, cur) {
            var tokenString = token.string;
            while (token.start !== 0 && token.className) {
                cur.ch = token.start;
                token = editorCtrl.getTokenAt(cur);
                if (token.className)
                    tokenString = token.string + tokenString;
            }
            return tokenString;
        },
        updateWordSearchService: function (wordArrayObject) {
            if (!wordArrayObject)
                return;
            for (var i = 0, c = wordArrayObject.length; i < c; i++) {
                operations.wordSearchService.add(wordArrayObject[i]);
            }
        },
        updateNodeCode: function (node) {
            if (node && node.data) {
                node.data.code = $utils.json.replaceInvalidJSONCharacters(editor.codeMirror.getValue());
            }
        }
    };

    return {
        attachEditor: function (textAreaElement) {
            editor.setupCodeEditorAndRefresh(textAreaElement);
        },
        detachEditor:function(){
            editor.detachEvents();
            editor.codeMirror = undefined;
            operations.wordSearchService = undefined;
        },
        initEditor: function (treeVariables) {
            CodeMirror.registerHelper("hint", "customComplete",editor.customComplete);
            CodeMirror.commands.autocomplete = editor.autoComplete;
            operations.loadwordSearchService().then(function () {
                operations.updateWordSearchService(treeVariables);
            });            
        },
        refresh: function () {
            if (editor.codeMirror) {
                editor.codeMirror.refresh();
            }
        },
        setReadOnly: function (isReadOnly) {
            if(editor.codeMirror)
                editor.codeMirror.setOption('readOnly', isReadOnly);
        },
        setSelection: function (gotoLine, gotoChr, length) {
            if (editor.codeMirror != null && gotoLine != null && gotoChr != null && length != null) {
                editor.codeMirror.setSelection({ line: gotoLine - 1, ch: gotoChr }, { line: gotoLine - 1, ch: gotoChr + length });
            }
        },
        scrollIntoView: function (gotoLine, gotoChr, length) {
            if (editor.codeMirror != null && gotoLine != null && gotoChr != null && length != null) {
                editor.codeMirror.scrollIntoView({ what: { line: gotoLine, ch: gotoChr } });
            }
        },
        getText: function () {
            return editor.codeMirror.getValue();
        },
        setText: function (text) {
            editor.codeMirror.setValue(text);
        }
    };
}]);
