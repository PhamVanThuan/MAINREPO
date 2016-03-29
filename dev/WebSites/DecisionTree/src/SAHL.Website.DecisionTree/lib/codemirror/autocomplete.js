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

var getVars = function () {
    return [
    "LTV", "PTI"
    ];
}

var getTypes = function () {
    return [
    { "className": "Application", "classMembers": [{ "memberName": "Applicants", "memberType": "array", "arrayof":"Applicant" }, { "memberName": "OriginatingBranch", "memberType": "property" }] },
    { "className": "Applicant", "classMembers": [{ "memberName": "Name", "memberType": "property" }, { "memberName": "ID", "memberType": "property" }] },
    { "className": "Salutation", "classMembers": [{ "memberName": "Mr", "memberType": "enum" }, { "memberName": "Mrs", "memberType": "enum" }] }
    ];
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

var reg1 = function () {
    "use strict";

    var startlists = [getKeywords(), getVars(), getTypes().map(getTypeName)]; //add on primary name of types here

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
        if (presentword.indexOf('.') >= 0){
            list = [];
        }
        else
            var list = getlist(tok.string, startlists);

        return { list: list, from: CodeMirror.Pos(cur.line, start), to: CodeMirror.Pos(cur.line, end) };
    });
};

var reg2 = function () {
    "use strict";

    

    CodeMirror.registerHelper("hint", "customtypes", function (editor, options) {
        
        var cur = editor.getCursor(), curLine = editor.getLine(cur.line);
        var tok = editor.getTokenAt(editor.getCursor());
        //unfortunately, '.' is a token

        var presentword, startPosAdj, endPosAdj, nextSpace, elem, oldBlock, newBlock;
        presentword = editor.getLine(editor.getCursor().line);
        startPosAdj = presentword.slice(0, editor.getCursor().ch).length - presentword.slice(0, editor.getCursor().ch).lastIndexOf(' ') - 1;
        endPosAdj = presentword.length - editor.getCursor().ch;
        presentword = presentword.slice(editor.getCursor().ch - startPosAdj, editor.getCursor().ch + endPosAdj);

        //how many dots?
        console.log("presentword: " + presentword);
        var words=presentword.split(".");
        console.log("dots: " + (words.length - 1));
        console.log("last type: " + words[words.length - 2]);
        var start = tok.end; //replace just from the dot
        var end = tok.end;
        var lastType = words[words.length - 2];
        var list = [];
        if (lastType) { //exists
            //check if array
            if (lastType.slice(-1) == ']') {//array
                //what is the array type?
                //get name before brackets
                lastType = lastType.substring(0, lastType.indexOf('['));
                //use arrayof property
                var lastlastType = words[words.length - 3];
                if (lastlastType) {
                    var lastResults = getlistAbsolute(lastlastType, [getTypes().map(getTypeName)]);
                    if (lastResults.length > 0) {
                        var index0 = (getTypes().map(getTypeName)).indexOf(lastResults[0]);
                        var supermembers = getTypes()[index0].classMembers;
                        var subindex = (supermembers.map(getMemberName)).indexOf(lastType);
                        if (subindex >= 0) {
                            lastType = supermembers[subindex].arrayof;
                        }
                        else
                            lastType = "";
                    }

                }
            }
            
            //search for it as a supertype
            var results = getlistAbsolute(lastType, [getTypes().map(getTypeName)]);
            if (results.length > 0) {
                //find sub-members
                var index = (getTypes().map(getTypeName)).indexOf(results[0]); //index within types
                var members = getTypes()[index].classMembers.map(getMemberName); //just the names of the members
                list = members;
                
            }
        }
       

        
        return { list: list, from: CodeMirror.Pos(cur.line, start), to: CodeMirror.Pos(cur.line, end) };
    });
};
