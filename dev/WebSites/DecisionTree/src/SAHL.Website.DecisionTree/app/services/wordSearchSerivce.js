'use strict';
angular.module('sahl.tools.app.services.wordSearchService', [
])
.factory('$wordSearchService', ['$q', '$utils', function ($q, $utils) {
    // because this uses a ternary Search Tree under the hood
    // http://en.wikipedia.org/wiki/Ternary_search_tree
    // http://www.cs.princeton.edu/~rs/strings/
    // just a little modified to use modules and properties
    // :: => modules block == .M on a node
    // . => properties == .P on a node

    var moduleSplit = "::";
    var propertySplit = ".";

    var extensions = {
        extendChildNodes: function (node, parent) {
            extensions.toStringOverride(node.L, parent);
            extensions.toStringOverride(node.C, node);
            extensions.toStringOverride(node.R, parent);

            extensions.toStringOverride(node.M, undefined);
            extensions.toStringOverride(node.P, undefined);
        },
        toStringOverride: function (node, parent) {
            if (node) {
                node.$parent = parent;
                node.toString = function () {
                    if (parent) {
                        return parent.toString() + this.Char;
                    }
                    return this.Char;
                }
                extensions.extendChildNodes(node, parent);
            }
        }
    }
    var operations = function () {
        this.root = undefined;
        function getNewNode(charValue,parent){
            var newNode = { Char: charValue };
            newNode.$parent = parent;
            newNode.toString = function () {
                if (parent) {
                    return parent.toString() + this.Char;
                }
                return this.Char;
            };
            return newNode;
        }
        this.findNode = function (string, context) {
            var node = context;
            for (var i = 0, c = string.length; i < c; i++) {
                if (node) {
                    if (i !== 0) {
                        node = node.C
                    }
                    node = this.nextNode(string[i], node);
                } else {
                    return undefined;
                }
            }
            return node;
        };
        this.nextNode = function (char, node) {
            if (node) {
                if (char < node.Char) {
                    return this.nextNode(char, node.L);
                } else if (char > node.Char) {
                    return this.nextNode(char, node.R);
                } else if (char === node.Char) {
                    return node;
                }
            }
            return undefined;
        };
        this.findWords = function (nodePartial, wordsArray) {
            if (nodePartial) {
                if (nodePartial.Last) {
                    wordsArray.push(nodePartial.toString());
                }
                this.findWords(nodePartial.L, wordsArray);
                this.findWords(nodePartial.C, wordsArray);
                this.findWords(nodePartial.R, wordsArray);
            }
        },
        this.internalAdd = function (str, position, nodeFn, parent) {
            var node = nodeFn() ? nodeFn() : nodeFn(getNewNode(str[position], parent));
            if (str[position] < node.Char){
                return this.internalAdd(str, position,function (value) {
                    if (value) {
                        node.L = value;
                    }
                    return node.L
                }, node.$parent);
            }
            else if (str[position] > node.Char) {
                return this.internalAdd(str, position, function (value) {
                    if (value) {
                        node.R = value;
                    }
                    return node.R
                }, node.$parent);
            } else{
                if (position + 1 === str.length) {
                    node.Last = true;
                    return node;
                }
                else
                {
                    return this.internalAdd(str, position + 1, function (value) {
                        if (value) {
                            node.C = value;
                        }
                        return node.C
                    }, node);
                }
            }
        },
        this.internalRemove = function (node) {
            var context = node.$parent;
            if (node.Char === context.L) {
                delete context.L;
            } else if (node.Char === context.C) {
                delete context.C;
            } else if (node.Char === context.R) {
                delete context.R;
            } else if (node.Char === context.P) {
                delete context.P;
            } else if (node.Char === context.M) {
                delete context.M;
            }
            if (node.P || node.M || node.C || node.L || node.R || node.Last)
                return;
            this.internalRemove(context);
        }
    };
    var service = function(){
        var _operations = new operations();

        this.startsWith = function (searchTerm) {
            var words = [];
            var keywords = searchTerm.split(propertySplit);
            var modules = keywords[0].split(moduleSplit);
            var endsWithPropertySplit = $utils.string.endsWith(searchTerm, propertySplit);
            var endsWithModuleSplit = $utils.string.endsWith(searchTerm, moduleSplit);

            var contextNode = _operations.root;
            for (var i = 0, c = modules.length; i < c; i++) {
                if(contextNode){
                    if(i > 0){
                        contextNode = contextNode.M;
                    }
                } else {
                    return words;
                }
                contextNode = _operations.findNode(modules[i], contextNode);
            }

            var c = endsWithPropertySplit ? keywords.length - 1 : keywords.length;
            if (keywords.length > 1) {
                for (var i = 1; i < c; i++) {
                    if (contextNode && contextNode.P) {
                        contextNode = _operations.findNode(keywords[i], contextNode.P);
                    }
                }
            }
            
            if (contextNode){
                if (endsWithModuleSplit) {
                    _operations.findWords(contextNode, words);
                } else if (endsWithPropertySplit) {
                    _operations.findWords(contextNode.P, words);
                } else {
                    if (contextNode.Last)
                        words.push(contextNode.toString());
                    _operations.findWords(contextNode.C, words);
                }
            }
            return words;
        };
        this.add = function (string) {
            if (!string || string.length === 0)
                return;
            var keywords = string.split(propertySplit);
            var modules = keywords[0].split(moduleSplit);
            var currentContext = _operations.root;
            for (var i = 0, c = modules.length; i < c; i++) {
                if (i == 0) {
                    currentContext = _operations.internalAdd(modules[i], 0, function (value) {
                        if (value) {
                            _operations.root = value;
                        }
                        return _operations.root
                    }, undefined);
                }
                else{
                    currentContext = _operations.internalAdd(modules[i], 0,function (value) {
                             if (value) {
                                 currentContext.M = value;
                             }
                             return currentContext.M
                    },undefined);
                }
            }
            if (keywords.length > 1) {
                _operations.internalAdd(keywords[1], 0, function (value) {
                    if (value) {
                        currentContext.P = value;
                    }
                    return currentContext.P
                }, undefined);
            }
        };
        this.remove = function (searchTerm) {
            var words = [];
            var keywords = searchTerm.split(propertySplit);
            var modules = keywords[0].split(moduleSplit);
            var endsWithPropertySplit = $utils.string.endsWith(searchTerm, propertySplit);
            var endsWithModuleSplit = $utils.string.endsWith(searchTerm, moduleSplit);

            var contextNode = _operations.root;
            for (var i = 0, c = modules.length; i < c; i++) {
                if (contextNode) {
                    if (i > 0) {
                        contextNode = contextNode.M;
                    }
                } else {
                    return words;
                }
                contextNode = _operations.findNode(modules[i], contextNode);
            }

            var c = endsWithPropertySplit ? keywords.length - 1 : keywords.length;
            if (keywords.length > 1) {
                for (var i = 1; i < c; i++) {
                    if (contextNode && contextNode.P) {
                        contextNode = _operations.findNode(keywords[i], contextNode.P);
                    }
                }
            }
            if (contextNode && contextNode.Last){
                delete contextNode.Last;
                _operations.internalRemove(contextNode);
            }
        }
        this.Initilize = function (tree) {
            _operations.root = tree.Root;
            _operations.root.toString = function () {
                return this.Char;
            }
            extensions.extendChildNodes(_operations.root, undefined);
        }
        return this;
    }

    return {
        Empty : function(){
            return new service();
        },
        Initilized: function (treeRoot) {
            var searchService = new service();
            searchService.Initilize(treeRoot);
            return searchService;
        }
    }
}]);