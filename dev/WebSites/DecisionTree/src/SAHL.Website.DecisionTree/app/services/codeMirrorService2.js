angular.module('sahl.tools.app.services.codeMirrorService', [

]).factory('$codeMirrorService', ['$compile', '$parseHttpService', '$q', '$rootScope', '$statusbarManager', '$eventAggregatorService', '$eventDefinitions', '$decisionTreeDesignQueries', '$queryManager', '$designSurfaceManager',
function ($compile, $parseHttpService, $q, $rootScope, $statusbarManager, $eventAggregatorService, $eventDefinitions, $decisionTreeDesignQueries, $queryManager, $designSurfaceManager) {
    var mCodeMirror;
    var txtOut = "";
    var hasChanges = false;
    var Pos = CodeMirror.Pos;

    var globalvars, inputvars, outputvars, types, messages, enums;
    var mCodeMirror;

    var internalOperations =
    {
        loadWords : function(){

        }
    }

    var getKeywords = function () {
        return [
        "alias", "and", "BEGIN", "begin", "break", "case", "class", "def", "defined?", "do", "else",
        "elsif", "END", "end", "ensure", "false", "for", "if", "in", "module", "next", "not", "or",
        "redo", "rescue", "retry", "return", "self", "super", "then", "true", "undef", "unless",
        "until", "when", "while", "yield", "nil", "raise", "throw", "catch", "fail", "loop", "callcc",
        "caller", "lambda", "proc", "public", "protected", "private", "require", "load",
        "require_relative", "extend", "autoload", "__END__", "__FILE__", "__LINE__", "__dir__"
        ];
    }
    var stypes = null;

    var getTypes = function () {
        return stypes;
    }

    var getlist = function (presentString, startlists) {
        if (presentString.slice(-1) == ']')
            return [];
        // make sure we escape special regex characters
        presentString = presentString.trim().replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
        var tester = new RegExp('^' + presentString, "i");
        var filtered = [];
        for (var i = 0; i < startlists.length; ++i) {
            var filteredtemp = startlists[i].filter(function (item) {
                return tester.test(item);
            });
            filtered = filtered.concat(filteredtemp);
        }
        return filtered;
    }

    var getlistAbsolute = function (presentString, startlists) {
        // make sure we escape special regex characters
        presentString = presentString.trim().replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
        var tester = new RegExp('^' + presentString + '$', "i");
        var filtered = [];
        for (var i = 0; i < startlists.length; ++i) {
            var filteredtemp = startlists[i].filter(function (item) {
                return tester.test(item);
            });
            filtered = filtered.concat(filteredtemp);
        }
        return filtered;
    }

    var getTypeName = function (element) {
        var result = element.className;
        return result;
    }

    var getMemberName = function (element) {
        var result = element.memberName;
        return result;
    }

    var isGroup = function (element) {
        return element.memberTypes == "group";
    }

    var notGroup = function (element) {
        return element.memberTypes != "group";
    }
    //this must differentiate between messages and variables as the root
    var getSubTypes = function (lastone, baseone, groupsOnly) {
        var newlist = [];
        var results = getlistAbsolute(lastone, [getTypes().map(getTypeName)]);
        if (results.length > 0) {
            //find sub-members
            //NB! This can now be greater than 1 because of Messages and Variables L
            var checkhere = true;
            var usefilter = groupsOnly ? isGroup : notGroup;
            if (results.length === 1) {
                var index = (getTypes().map(getTypeName)).indexOf(results[0]);
                var members = getTypes()[index].classMembers.filter(usefilter).map(getMemberName); //just the names of the members
                newlist = members;
            }
        }
        return newlist;
    };

    var operations = {
        replaceInvalidJSONCharacters: function (rawData) {
            return rawData.replace(/'/g, "_sgl_quote_").replace(/\n/g, "_newline_").replace(/\r/g, "_carriage_").replace(/\t/g, "_tab_").replace(/"/g, "_quote_")
        },
        removeJSONEncodedCharacters: function (rawData) {
            return rawData.replace(/_sgl_quote_/g, "'").replace(/_newline_/g, "\n").replace(/_carriage_/g, "\r").replace(/_tab_/g, "\t").replace(/_quote_/g, "\"")
        },
        setupCursorStatusBarUpdate: function (editor) {
            editor.on("cursorActivity", function () {
                var cursorPos = editor.getCursor();
                $statusbarManager.setStatusbarSegmentedPanelValue("textPosition", "Ln", cursorPos.line);
                $statusbarManager.setStatusbarSegmentedPanelValue("textPosition", "Col", cursorPos.ch + 1);
                $statusbarManager.setStatusbarSegmentedPanelValue("textSelection", "Sel", editor.getSelection().length);
            })
        },
        updateNodeCode: function (newCode) {
            var currentNode = $rootScope.document.selectionManager.current;
            if (currentNode !== undefined || currentNode !== null) {
                if (hasChanges) {
                    currentNode.data.code = operations.replaceInvalidJSONCharacters(newCode);
                    $rootScope.document.hasChanges = true;
                    $designSurfaceManager.updateBindings();
                }
            }
        },
        refreshAutoCompleteVariables: function (Globals, Inputs, Outputs, Messages, Enums, SubTrees) {
            if (!Globals) { Globals = [] };
            if (!Inputs) { Inputs = [] };
            if (!Outputs) { Outputs = [] };
            if (!Messages) { Messages = [] };
            if (!Enums) { Enums = [] };
            if (!SubTrees) { SubTrees = [] };
            globalvars = Globals;
            inputvars = Inputs;
            outputvars = Outputs;
            messages = Messages;
            enums = Enums;
            types = Globals.concat(Inputs).concat(Outputs).concat(Messages).concat(Enums).concat(SubTrees);
            var corelists = [getKeywords(), ["Variables", "Messages", "Enumerations"], []];

            // also add the inputs and outputs to the Variables node
            var variablesItem = types.filter(function (value) {
                return value.className === "Variables";
            })
            if (variablesItem !== undefined && variablesItem.length == 1) {
                var inputVars = variablesItem[0].classMembers.filter(function (item) {
                    if (item.memberName == "inputs") {
                        return true;
                    }
                    return false;
                });
                if (inputVars.length == 0) {
                    variablesItem[0].classMembers.push({ memberName: "inputs", memberTypes: "group" });
                }

                var outputVars = variablesItem[0].classMembers.filter(function (item) {
                    if (item.memberName == "outputs") {
                        return true;
                    }
                    return false;
                });
                if (outputVars.length == 0) {
                    variablesItem[0].classMembers.push({ memberName: "outputs", memberTypes: "group" });
                }

                var topLevelSubTrees = SubTrees.filter(function (item) {
                    return item.className === "Variables::subtrees";
                });
                if (topLevelSubTrees.length == 1) {
                    var subtrees = variablesItem[0].classMembers.filter(function (item) {
                        return item.memberName === "subtrees";
                    })
                    if (subtrees.length === 0) {
                        variablesItem[0].classMembers.push({ memberName: "subtrees", memberTypes: "group" });
                    }
                }


                //// top level subtree
                //var topLevelSubTree = null;
                //var topLevelSubTrees = SubTrees.filter(function (item) {
                //    return item.className === "subtrees";
                //});
                //if (topLevelSubTrees.length == 1) {
                //    topLevelSubTree = topLevelSubTrees[0];
                //}

                //if (topLevelSubTree !== null) {

                //    var subtreesItem = { memberName: "subtrees", memberTypes: "group" };
                //    variablesItem[0].classMembers.push(subtreesItem);

                //    for (var i = 0, c = topLevelSubTree.classMembers.length; i < c; i++) {
                //        variablesItem[0].classMembers.push({ memberName: "subtrees::" + topLevelSubTree.classMembers[i].memberName, memberTypes: "group" });
                //    }
                //}

                var sorted = variablesItem[0].classMembers.sort(function (a, b) {
                    return a.memberName > b.memberName ? 1 :
                    ((b.memberName > a.memberName) ? -1 :
                    0);
                });
                variablesItem[0].classMembers = sorted;
            }

            // add the messages methods
            var messagesItem = types.filter(function (value) {
                return value.className === "Messages";
            })
            if (messagesItem !== undefined && messagesItem.length == 1) {
                var addInfos = messagesItem[0].classMembers.filter(function (item) {
                    if (item.memberName == "AddInfo()") {
                        return true;
                    }
                    return false;
                });
                if (addInfos.length == 0) {
                    messagesItem[0].classMembers.push({ memberName: "AddInfo()", memberTypes: "method" });
                }

                var addErrors = messagesItem[0].classMembers.filter(function (item) {
                    if (item.memberName == "AddError()") {
                        return true;
                    }
                    return false;
                });
                if (addInfos.length == 0) {
                    messagesItem[0].classMembers.push({ memberName: "AddError()", memberTypes: "method" });
                }

                var addWarnings = messagesItem[0].classMembers.filter(function (item) {
                    if (item.memberName == "AddWarning()") {
                        return true;
                    }
                    return false;
                });
                if (addWarnings.length == 0) {
                    messagesItem[0].classMembers.push({ memberName: "AddWarning()", memberTypes: "method" });
                }

                var sorted = messagesItem[0].classMembers.sort(function (a, b) {
                    return a.memberName > b.memberName ? 1 :
                    ((b.memberName > a.memberName) ? -1 :
                    0);
                });
                messagesItem[0].classMembers = sorted;
            }

            stypes = types ? types : [];
        }
    }

    var eventHandlers = {
        onBeforeNodeSelectionChanged: function (ev) { //need to update code
            //ev contains node info of node which is about to LOSE its selection
            //if haschanges, mark as changed
            if (ev && ev.data) {
                if (hasChanges) {
                    // replace
                    ev.data.code = operations.replaceInvalidJSONCharacters(txtOut);
                    $rootScope.document.hasChanges = true;
                }
            }

            hasChanges = false;
        },
        onBeforeDocumentSaved : function(documentThatIsSaving){
            operations.updateNodeCode(txtOut);
        },
        onChangedSelection: function (ev) { //ev contains data of newly selected node
            if (ev && ev.data) {
                if (ev.data.code == undefined)
                    ev.data.code = "";
                txtOut = operations.removeJSONEncodedCharacters(ev.data.code);
                mCodeMirror.setOption('readOnly', false);
            }
            else {
                txtOut = "";
                mCodeMirror.setOption('readOnly', 'nocursor');
            }
            mCodeMirror.setValue(txtOut);
            mCodeMirror.refresh();
        }
    }

    return {
        removeUnderline: function () {
            mCodeMirror.removeOverlay("errorunderline");
        },
        removeHighLight: function () {
            mCodeMirror.removeOverlay("searching");
        },
        underlineRed: function (textToHighlight) {
            mCodeMirror.addOverlay({
                token: function (stream, state) {
                    var ch;
                    var matchThis
                    if (textToHighlight == "(")
                        matchThis = new RegExp("\\(");
                    else
                        matchThis = new RegExp("\\b" + textToHighlight + "\\b");
                    if (stream.match(matchThis)) {
                        //stream.next();
                        return "errorunderline";
                    }
                    while (stream.next() != null && !stream.match(matchThis, false)) { }
                    return null;
                },
                name: "errorunderline"
            });
        },
        highlightYellow: function (textToHighlight) {
            mCodeMirror.addOverlay({
                token: function (stream, state) {
                    var ch;
                    var matchThis = new RegExp(textToHighlight);
                    if (stream.match(matchThis)) {
                        //stream.next();
                        return "searching";
                    }
                    while (stream.next() != null && !stream.match(matchThis, false)) { }
                    return null;
                },
                name: "searching"
            });
        },
        scrollIntoView: function (gotoLine, gotoChr, length) {
            if (mCodeMirror != null && gotoLine != null && gotoChr != null && length != null) {
                mCodeMirror.scrollIntoView({ what: { line: gotoLine, ch: gotoChr } });
            }
        },
        setSelection: function (gotoLine, gotoChr, length) {
            if (mCodeMirror != null && gotoLine != null && gotoChr != null && length != null) {
                mCodeMirror.setSelection({ line: gotoLine - 1, ch: gotoChr }, { line: gotoLine - 1, ch: gotoChr + length });
            }
        },
        setCursor: function (gotoLine) {
            if (mCodeMirror != null && gotoLine != null) {
                mCodeMirror.setCursor(0,0,true);
            }
        },
        initEditor: function (globals, inputs, outputs, messages, enums, subTrees) {
            operations.refreshAutoCompleteVariables(globals, inputs, outputs, messages, enums, subTrees);
            var corelists = [getKeywords(), ["Variables", "Messages", "Enumerations"], []]; //level 0 types only

            CodeMirror.registerHelper("hint", "customwords", function (editor, options) {
                var cur = editor.getCursor(), curLine = editor.getLine(cur.line);
                var tok = editor.getTokenAt(editor.getCursor());
                var start = tok.start; //replace the whole word
                var end = tok.end;

                //check for dots
                var presentword, startPosAdj, endPosAdj, nextSpace, elem, oldBlock, newBlock;
                presentword = editor.getLine(editor.getCursor().line);
                //startPosAdj = presentword.slice(0, editor.getCursor().ch).length - presentword.slice(0, editor.getCursor().ch).lastIndexOf(' ') - 1;
                //endPosAdj = presentword.length - editor.getCursor().ch;
                //presentword = presentword.slice(editor.getCursor().ch - startPosAdj, editor.getCursor().ch + endPosAdj);

                presentwordSlice = presentword.slice(0, editor.getCursor().ch);
                // first look for the first (
                var lastAdjuster = Math.max(presentwordSlice.lastIndexOf('('), presentwordSlice.lastIndexOf(' '));
                var startPosAdj = presentwordSlice.length;
                if (lastAdjuster !== -1) {
                    startPosAdj = presentword.slice(0, editor.getCursor().ch).length - lastAdjuster - 1;
                }

                endPosAdj = presentword.length - editor.getCursor().ch;
                presentword = presentword.slice(editor.getCursor().ch - startPosAdj, editor.getCursor().ch + endPosAdj);

                var list = [];
                if (presentword != "") {
                    if ((presentword.indexOf('.') >= 0) || (presentword.indexOf('::') >= 0)) {
                        if (presentword.indexOf(' ') >= 0)
                            list = [];
                        else {
                            var groupsOnly = presentword.indexOf('::') > presentword.indexOf('.');

                            var start = groupsOnly ? presentword.lastIndexOf('::') : presentword.lastIndexOf('.');
                            start = start + (groupsOnly ? 2: 1); //replace just from the dot or ::
                            var end = tok.end;
                            var lastType;
                            var dotIndex, lastDotIndex;
                            if (!groupsOnly) {
                                dotIndex = presentword.lastIndexOf('.');
                                lastDotIndex = dotIndex + 1;

                            }
                            else {
                                dotIndex = presentword.lastIndexOf('::');
                                lastDotIndex = dotIndex + 2;
                            }
                            start = start + 2;

                            lastType = presentword.substring(0, dotIndex);
                            var basetype = lastType;
                            list = getSubTypes(lastType, basetype, groupsOnly);
                            //get just the latest fraction of word
                            var fractionword = presentword.substring(lastDotIndex, presentword.length);
                            //re-filter
                            list = getlist(fractionword, [list]);
                        }
                    }
                    else
                        var list = getlist(tok.string, corelists); //no no, just core lists!
                }
                return { list: list, from: CodeMirror.Pos(cur.line, start), to: CodeMirror.Pos(cur.line, end) };
            });

            CodeMirror.registerHelper("hint", "customtypes", function (editor, options) {
                var checkIsDoubleColon = function (editor) {
                    var cursor = editor.getCursor();
                    var check = editor.getLine(cursor.line);
                    // last :
                    if (check.substring(cursor.ch - 2, cursor.ch) == "::")
                        return true;
                    // in between the two ::
                    if (check.substring(cursor.ch - 1, cursor.ch + 1) == "::")
                        return true;
                    return false;
                };

                var checkIsDot = function (editor) {
                    var cursor = editor.getCursor();
                    var check = editor.getLine(cursor.line);
                    // last :
                    if (check.substring(cursor.ch - 1, cursor.ch) == ".")
                        return true;
                    return false;
                }

                var list = [];
                var cur = editor.getCursor(), curLine = editor.getLine(cur.line);
                var tok = editor.getTokenAt(editor.getCursor());
                var start = tok.start;
                var end = tok.end;
                //unfortunately, '.' is a token
                var isDoubleColon = false;
                var presentword, startPosAdj, endPosAdj, nextSpace, elem, oldBlock, newBlock;
                presentword = editor.getLine(editor.getCursor().line);
                presentwordSlice = presentword.slice(0, editor.getCursor().ch);
                // first look for the first (
                var lastAdjuster = Math.max(presentwordSlice.lastIndexOf('('), presentwordSlice.lastIndexOf(' '));
                var startPosAdj = presentwordSlice.length;
                if (lastAdjuster !== -1) {
                    startPosAdj = presentword.slice(0, editor.getCursor().ch).length - lastAdjuster - 1;
                }

                endPosAdj = presentword.length - editor.getCursor().ch;
                presentword = presentword.slice(editor.getCursor().ch - startPosAdj, editor.getCursor().ch + endPosAdj);

                //double colon possible
                if (checkIsDoubleColon(editor)) { //double-colon press, not .
                    isDoubleColon = true;
                    var lastType = presentword.substring(0, presentword.lastIndexOf("::"));
                    start = tok.end;
                    end = tok.end;
                }
                else
                    if (checkIsDot(editor)) {
                        var lastType = presentword.substring(0, presentword.lastIndexOf("."));
                        start = tok.end;
                        end = tok.end;
                        var basetype;
                    }

                if (lastType) { //exists
                    //check if array

                    //search for it as a supertype
                    //weed out based on doubleColon
                    list = getSubTypes(lastType, basetype, isDoubleColon);
                }

                return { list: list, from: CodeMirror.Pos(cur.line, start), to: CodeMirror.Pos(cur.line, end) };
            });

            CodeMirror.commands.autocomplete = function (cm, isNested) {
                if (!isNested)
                    CodeMirror.showHint(cm, CodeMirror.hint.customwords, { completeSingle: false });
                else
                    CodeMirror.showHint(cm, CodeMirror.hint.customtypes, { completeSingle: false });
            }
        },
        getText: function () {
            return txtOut;
        },
        setText: function (txt) {
            txtOut = txt;
        },
        attachEditor: function (txtAreaElement) {
            mCodeMirror = CodeMirror.fromTextArea(txtAreaElement, {
                lineNumbers: true,
                mode: "text/x-ruby",
                tabMode: "indent",
                matchBrackets: true,
                indentUnit: 2,
                value: txtOut,
                onKeyEvent: function (e, s) {
                    if (s.type == "keyup") {
                        $eventAggregatorService.publish($eventDefinitions.onKeyUpCodeEditor, { "codeEditor": e, "keyEvent": s });
                    }
                    hasChanges = true;
                    $rootScope.document.hasChanges = true; //in case of premature closure
                    if ((s.type == "keyup") && (s.keyCode != 9) && (s.keyCode != 13) && (s.keyCode != 27) && (s.keyCode != 190) && (s.keyCode != 186) && (s.keyCode != 16) && (s.keyCode != 40) && (s.keyCode != 38) && (s.keyCode != 8)) {
                        if ((e.getTokenAt(e.getCursor()).string != ' ') && (e.getTokenAt(e.getCursor()).string.length > 0)) {
                            CodeMirror.commands.autocomplete(e);
                        }
                    }
                    else if ((s.type == "keyup") && (s.keyCode == 186)) { //colon
                        if (checkDoubleColon(e))
                            CodeMirror.commands.autocomplete(e, true);
                    }
                    else if ((s.type == "keyup") && (s.keyCode == 190)) { //dot
                        CodeMirror.commands.autocomplete(e, true);
                    }
                    else if ((s.type == "keyup") && (s.keyCode == 8)) { //backspace
                        if ((e.getTokenAt(e.getCursor()).string == '.') || (checkDoubleColon(e)))
                            CodeMirror.commands.autocomplete(e,true);
                        else
                            CodeMirror.commands.autocomplete(e, false);
                    }
                    txtOut = e.getValue();
                    //add asterisk to show modified input
                }
            });
            var checkDoubleColon = function (ed) {
                var cursor = ed.getCursor();
                var check = ed.getLine(cursor.line);
                // last :
                if (check.substring(cursor.ch - 2, cursor.ch) == "::")
                    return true;
                // in between the two ::
                if (check.substring(cursor.ch - 1, cursor.ch + 1) == "::")
                    return true;
                return false;
            };
            mCodeMirror.setSize('100%', '100%');
            mCodeMirror.focus();
            mCodeMirror.setValue(txtOut);
            mCodeMirror.refresh();

            operations.setupCursorStatusBarUpdate(mCodeMirror);

            $eventAggregatorService.subscribe($eventDefinitions.onBeforeNodeSelectionChanged, eventHandlers.onBeforeNodeSelectionChanged);
            $eventAggregatorService.subscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.onChangedSelection);
            $eventAggregatorService.subscribe($eventDefinitions.onBeforeDocumentSaved, eventHandlers.onBeforeDocumentSaved);

        },
        detachEditor: function () {
            $eventAggregatorService.unsubscribe($eventDefinitions.onBeforeNodeSelectionChanged, eventHandlers.onBeforeNodeSelectionChanged);
            $eventAggregatorService.unsubscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.onChangedSelection);
            $eventAggregatorService.unsubscribe($eventDefinitions.onBeforeDocumentSaved, eventHandlers.onBeforeDocumentSaved);
        },
        refresh: function () {
            if (mCodeMirror) {
                mCodeMirror.refresh();
            }
        },
        setReadOnly: function (isReadOnly) {
            if (mCodeMirror) {
                mCodeMirror.setOption('readOnly', isReadOnly);
            }
        },
        refreshAutoCompleteVariables: function (globals, inputs, outputs, messages, enums) {
            operations.refreshAutoCompleteVariables(globas, inputs, outputs, messages, enums);
        }
    }
}]);