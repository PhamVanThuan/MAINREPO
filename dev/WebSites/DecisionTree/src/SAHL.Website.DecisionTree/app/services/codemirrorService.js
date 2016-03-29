angular.module('sahl.tools.app.services.codeMirrorService', [

]).factory('$codeMirrorService', ['$compile', '$parseHttpService', '$q', '$rootScope', '$codemirrorVariablesService', '$statusbarManager', '$eventAggregatorService', '$eventDefinitions', '$decisionTreeDesignQueries','$queryManager', function ($compile, $parseHttpService, $q, $rootScope, $codemirrorVariablesService, $statusbarManager, $eventAggregatorService, $eventDefinitions, $decisionTreeDesignQueries, $queryManager) {
    var txtOut = "";
    var hasChanges = false;
    var globalvars, inputvars, outputvars, types,messages,enums;
    var mCodeMirror;

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
    var svars = null;
    var stypes = null;

    var getVars = function () {
        return svars;
    }

    var getTypes = function () {
        return stypes;
    }

    var getlist = function (presentString, startlists) {
        if (presentString.slice(-1) == ']')
            return [];
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
            else { //this branch should now NEVER HAPPEN
                console.log("Oh dear!");
                //check messages vs variables!
                var startpos = 0;
                results.forEach(function (result) {
                    var index = (getTypes().map(getTypeName)).indexOf(result, startpos);
                    startpos = index + 1;
                    var typename = getTypes()[index].classMembers[0].memberTypes;
                    //type is either Message or not
                    /*
                    if ((typename === "Message") && (baseone === "Messages")) {
                        newlist = getTypes()[index].classMembers.map(getMemberName);
                    }
                    if ((typename != "Message") && (baseone != "Messages")) {
                        newlist = getTypes()[index].classMembers.map(getMemberName);
                    }*/
                    if ((typename == baseone)) {
                        newlist = getTypes()[index].classMembers.filter(usefilter).map(getMemberName);
                    }
                });
            }
        }

        return newlist;
    };
    
    var setInputs = function (inputs) {
        inputvars = inputs;
        stypes = globalvars.concat(inputvars).concat(outputvars).concat(messages).concant(enums);
    };
    var setOutputs = function (outputs) {
        outputvars = outputs;
        stypes = globalvars.concat(inputvars).concat(outputvars).concat(messages).concat(enums);
    };

    var operations = {
        replaceInvalidJSONCharacters : function(rawData){
            return rawData.replace(/'/g, "_sgl_quote_").replace(/\n/g, "_newline_").replace(/\r/g, "_carriage_").replace(/\t/g, "_tab_").replace(/"/g, "_quote_")
        },
        removeJSONEncodedCharacters: function (rawData) {
            return rawData.replace( /_sgl_quote_/g, "'").replace(/_newline_/g, "\n").replace(/_carriage_/g, "\r").replace(/_tab_/g, "\t").replace(/_quote_/g, "\"")
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
        onChangedSelection: function (ev) { //ev contains data of newly selected node
            if (ev&&ev.data) {
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
        removeUnderline: function() {
            mCodeMirror.removeOverlay("errorunderline");
        },
        removeHighLight: function() {
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
                //mCodeMirror.setCursor(gotoLine);
            }
        },
        setSelection: function (gotoLine, gotoChr, length) {            
            if (mCodeMirror != null && gotoLine != null && gotoChr != null && length != null) {
                mCodeMirror.setSelection({ line: gotoLine - 1, ch: gotoChr }, { line: gotoLine - 1, ch: gotoChr + length });
            }
        },
        setCursor: function (gotoLine) {
            if (mCodeMirror != null && gotoLine != null) {                
                mCodeMirror.setCursor({ line: gotoLine, ch: 0 });
            }
        },
        initEditor: function (startText, variables, Globals, Inputs, Outputs, Messages, Enums) {

            if (!Globals) { Globals = [] };
            if (!Inputs) { Inputs = [] };
            if (!Outputs) { Outputs = [] };
            if (!Messages) { Messages = [] };
            if (!Enums) { Enums = [] };
            globalvars = Globals;
            inputvars = Inputs;
            outputvars = Outputs;
            messages = Messages;
            enums = Enums;
            types = Globals.concat(Inputs).concat(Outputs).concat(Messages).concat(Enums);
            svars = variables ? variables : [];
            stypes = types ? types : [];
            var startlists = [getKeywords(), getVars(), getTypes().map(getTypeName)]; //add on primary name of types here
            var corelists = [getKeywords(), ["Variables", "Variables::inputs", "Variables::outputs", "Messages", "Enumerations"], []]; //level 0 types only
            if (!startText)
                startText = "";
            txtOut = startText;
            CodeMirror.registerHelper("hint", "customwords", function (editor, options) {
                var cur = editor.getCursor(), curLine = editor.getLine(cur.line);
                var tok = editor.getTokenAt(editor.getCursor());
                var start = tok.start; //replace the whole word
                var end = tok.end;

                //check for dots
                var presentword, startPosAdj, endPosAdj, nextSpace, elem, oldBlock, newBlock;
                presentword = editor.getLine(editor.getCursor().line);
                startPosAdj = presentword.slice(0, editor.getCursor().ch).length - presentword.slice(0, editor.getCursor().ch).lastIndexOf(' ') - 1;
                endPosAdj = presentword.length - editor.getCursor().ch;
                presentword = presentword.slice(editor.getCursor().ch - startPosAdj, editor.getCursor().ch + endPosAdj);
                var list = [];
                if (presentword != "") {
                    if ((presentword.indexOf('.') >= 0)||(presentword.indexOf('::') >= 0)) {
                        if (presentword.indexOf(' ') >= 0)
                            list = [];
                        else {
                            var groupsOnly = presentword.indexOf('::') > presentword.indexOf('.');

                            var start = groupsOnly ? presentword.lastIndexOf('::') : presentword.lastIndexOf('.');
                            start = start+1; //replace just from the dot or ::
                            var end = tok.end;
                            var lastType;
                            var dotIndex,lastDotIndex;
                            if (!groupsOnly) {
                                dotIndex = presentword.lastIndexOf('.');
                                lastDotIndex = dotIndex + 1;

                            }
                            else {
                                dotIndex = presentword.lastIndexOf('::');
                                lastDotIndex = dotIndex + 2;
                                start = start + 1;
                            }

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
                var cur = editor.getCursor(), curLine = editor.getLine(cur.line);
                var tok = editor.getTokenAt(editor.getCursor());
                //unfortunately, '.' is a token
                var isDoubleColon = false;
                var presentword, startPosAdj, endPosAdj, nextSpace, elem, oldBlock, newBlock;
                presentword = editor.getLine(editor.getCursor().line);
                startPosAdj = presentword.slice(0, editor.getCursor().ch).length - presentword.slice(0, editor.getCursor().ch).lastIndexOf(' ') - 1;
                endPosAdj = presentword.length - editor.getCursor().ch;
                presentword = presentword.slice(editor.getCursor().ch - startPosAdj, editor.getCursor().ch + endPosAdj);

                //double colon possible
                var check = editor.getLine(editor.getCursor().line);
                if ((check.substring(check.length - 2)) == "::") { //double-colon press, not .
                    isDoubleColon = true;
                    var lastType = presentword.substring(0, presentword.lastIndexOf("::"));
                }
                else {
                    //how many dots?
                    var words = presentword.split(".");
                    var start = tok.end; //replace just from the dot
                    var end = tok.end;
                    var lastType = presentword.substring(0, presentword.lastIndexOf("."));
                    var basetype;
                }
                var list = [];
                if (lastType) { //exists
                    //check if array

                    //search for it as a supertype
                    //weed out based on doubleColon
                    console.log("last type: " + lastType);
                    
                    list = getSubTypes(lastType, basetype,isDoubleColon);
                    
                    
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
            var checkDoubleColon = function (ed) {
                var check = ed.getLine(ed.getCursor().line);
                if ((check.substring(check.length - 2)) == "::")
                    return true;
                return false;
            };
            mCodeMirror = CodeMirror.fromTextArea(txtAreaElement, {                
                lineNumbers: true,
                mode: "text/x-ruby",
                tabMode: "indent",
                matchBrackets: true,
                indentUnit: 2,
                value: txtOut,
                onKeyEvent: function (e, s) {
                    $eventAggregatorService.publish($eventDefinitions.onKeyUpCodeEditor, {"codeEditor": e, "keyEvent":s});
                    hasChanges = true;
                    $rootScope.document.hasChanges = true; //in case of premature closure
                    if ((s.type == "keyup") && (s.keyCode != 9) && (s.keyCode != 13) && (s.keyCode != 190) && (s.keyCode != 186) && (s.keyCode != 16) && (s.keyCode != 40) && (s.keyCode != 38) && (s.keyCode != 8)) {
                        if ((e.getTokenAt(e.getCursor()).string != ' ') && (e.getTokenAt(e.getCursor()).string.length > 0)) {
                            CodeMirror.commands.autocomplete(e, false);
                        }
                    }
                    else if ((s.type == "keyup") && (s.shiftKey) && (s.keyCode == 186)) { //colon
                        if (checkDoubleColon(e))
                            CodeMirror.commands.autocomplete(e, true);
                    }
                    else if ((s.type == "keyup") && (s.keyCode == 190)) { //dot
                        CodeMirror.commands.autocomplete(e, true);
                    }
                    else if ((s.type == "keyup") && (s.keyCode == 8)) { //backspace
                        if ((e.getTokenAt(e.getCursor()).string == '.') || (checkDoubleColon(e)))
                            CodeMirror.commands.autocomplete(e, true);
                        else
                            CodeMirror.commands.autocomplete(e, false);
                    }
                    txtOut = e.getValue();
                    //add asterisk to show modified input
                }
            });
            mCodeMirror.setSize('100%', '100%');
            mCodeMirror.focus();
            mCodeMirror.setValue(txtOut);
            mCodeMirror.refresh();
            $eventAggregatorService.subscribe($eventDefinitions.onBeforeNodeSelectionChanged, eventHandlers.onBeforeNodeSelectionChanged);
            $eventAggregatorService.subscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.onChangedSelection);
            setupCursorStatusBarUpdate(mCodeMirror);
            
        },
        refresh: function () {            
            if (mCodeMirror) {
                mCodeMirror.refresh();
            }
        },
        setReadOnly: function (isReadOnly) {            
            if(mCodeMirror) {
                mCodeMirror.setOption('readOnly', isReadOnly);
            }
        }
    };
        function setupCursorStatusBarUpdate(editor) {
            editor.on("cursorActivity", function() {
                var cursorPos = editor.getCursor();
                $statusbarManager.setStatusbarSegmentedPanelValue("textPosition", "Ln", cursorPos.line);
                $statusbarManager.setStatusbarSegmentedPanelValue("textPosition", "Col", cursorPos.ch + 1);
                $statusbarManager.setStatusbarSegmentedPanelValue("textSelection", "Sel", editor.getSelection().length);
            })
        }
}]);