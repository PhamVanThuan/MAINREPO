﻿'use strict';
angular.module('sahl.halo.app.organisationStructure.start', ['sahl.halo.app.organisationStructure.organisationStructureChart.directives', 'sahl.halo.app.organisationStructure.organisationStructureQueryService'])
    .config([
        '$stateProvider', function($stateProvider) {
            $stateProvider.state('start.portalPages.apps.organisationStructure', {
                url: 'organisationStructure',
                templateUrl: 'app/start/start.tpl.html',
                controller: 'OrganisationStructureStartCtrl'
            });
        }
    ])
    .controller('OrganisationStructureStartCtrl', [
        '$scope','$rootScope', '$organisationStructureQueryServiceService', function StartController($scope,$rootScope, $organisationStructureQueryServiceService) {

            function onDataFetchComplete(payload) {
                $scope.organisationStructureData = payload.data.ResourceList;

                var tempOrgStructureModel = $organisationStructureQueryServiceService.initialiseBaseTree();
                tempOrgStructureModel.nodeDataArray.push($rootScope.baseCompany);

                for (var i = 0; i < $scope.organisationStructureData.length; i++) {
                    var organisationElement = $scope.organisationStructureData[i];
                    tempOrgStructureModel.nodeDataArray.push({
                        "key": organisationElement.OrganisationStructureKey,
                        "name": organisationElement.Description,
                        "title": organisationElement.OrganisationType,
                        "parent": organisationElement.ParentKey
                    });
                }

                $rootScope.organisationStructureModel = $organisationStructureQueryServiceService.initTreeWithArray(tempOrgStructureModel.nodeDataArray);

                organisationStructure.init($rootScope);
            };

            $organisationStructureQueryServiceService.start().then(onDataFetchComplete);
        
            $scope.$on('$destroy', function() {
                delete $scope.two;
            });
        }
    ]);

var organisationStructure = {};

(function () {

    var oscDiagram = {};

    organisationStructure.init = function ($rootScope) {
        var $$ = go.GraphObject.make; // for conciseness in defining templates

        oscDiagram =
            $$(go.Diagram, "oscDiagram", // must be the ID or reference to div
            {
                initialContentAlignment: go.Spot.Center,
                // make sure users can only create trees
                validCycle: go.Diagram.CycleDestinationTree,
                // users can select only one part at a time
                maxSelectionCount: 1,
                layout:
                    $$(go.TreeLayout,
                    {
                        treeStyle: go.TreeLayout.StyleLastParents,
                        arrangement: go.TreeLayout.ArrangementHorizontal,
                        // properties for most of the tree:
                        angle: 90,
                        layerSpacing: 35,
                        // properties for the "last parents":
                        alternateAngle: 90,
                        alternateLayerSpacing: 35,
                        alternateAlignment: go.TreeLayout.AlignmentBus,
                        alternateNodeSpacing: 20
                    }),
                // support editing the properties of the selected person in HTML
                "ChangedSelection": onSelectionChanged,
                "TextEdited": onTextEdited,
                // enable undo & redo
                "undoManager.isEnabled": true
            });


        //when the document is modified, add a "*" to the title and enable the "Save" button
        oscDiagram.addDiagramListener("Modified", function (e) {
            var button = document.getElementById("SaveButton");
            if (button) button.disabled = !oscDiagram.isModified;
            var idx = document.title.indexOf("*");
            if (oscDiagram.isModified) {
                if (idx < 0) document.title += "*";
            } else {
                if (idx >= 0) document.title = document.title.substr(0, idx);
            }
        });

        var levelColors = [
            "#AC193D/#BF1E4B", "#2672EC/#2E8DEF", "#8C0095/#A700AE", "#5133AB/#643EBF",
            "#008299/#00A0B1", "#D24726/#DC572E", "#008A00/#00A600", "#094AB2/#0A5BC4"
        ];

        // override TreeLayout.commitNodes to also modify the background brush based on the tree depth level
        oscDiagram.layout.commitNodes = function () {
            go.TreeLayout.prototype.commitNodes.call(oscDiagram.layout); // do the standard behavior

            // then go through all of the vertexes and set their corresponding node's Shape.fill
            // to a brush dependent on the TreeVertex.level value
            oscDiagram.layout.network.vertexes.each(function (treeNode) {
                if (treeNode.node) {
                    var level = treeNode.level % (levelColors.length);
                    var colors = levelColors[level].split("/");
                    var shape = treeNode.node.findObject("SHAPE");
                    if (shape) shape.fill = $$(go.Brush, go.Brush.Linear, { 0: colors[0], 1: colors[1], start: go.Spot.Left, end: go.Spot.Right });
                }
            });
        }

        // when a node is double-clicked, add a child to it
        function nodeDoubleClick(e, obj) {
            var clicked = obj.part;
            if (clicked !== null) {
                var thisemp = clicked.data;
                oscDiagram.startTransaction("add employee");
                var nextkey = (oscDiagram.model.nodeDataArray.length + 1).toString();
                var newemp = { key: nextkey, name: "(new person)", title: "", parent: thisemp.key };
                oscDiagram.model.addNodeData(newemp);
                oscDiagram.commitTransaction("add employee");
            }
        }

        // this is used to determine feedback during drags
        function mayWorkFor(node1, node2) {
            if (!(node1 instanceof go.Node)) return false; // must be a Node
            if (node1 === node2) return false; // cannot work for yourself
            if (node2.isInTreeOf(node1)) return false; // cannot work for someone who works for you
            return true;
        }

        // This function provides a common style for most of the TextBlocks.
        // Some of these values may be overridden in a particular TextBlock.
        function textStyle() {
            return { font: "9pt  Segoe UI,sans-serif", stroke: "white" };
        }

        // This converter is used by the Picture.
        function findHeadShot(key) {
            return "images/HS" + key + ".png";
        };

        // whenever a GoJS transaction has finished modifying the model, update all Angular bindings
        function updateAngular(e) {
            if (e.isTransactionFinished) $rootScope.$apply();
        }

        // notice when the value of "model" changes: update the Diagram.model
        $rootScope.$watch("organisationStructureModel", function (newmodel) {
            var oldmodel = oscDiagram.model;
            if (oldmodel !== newmodel) {
                if (oldmodel) oldmodel.removeChangedListener(updateAngular);
                newmodel.addChangedListener(updateAngular);
                oscDiagram.model = go.Model.fromJson(newmodel);
            }
        });
        // update the model when the selection changes
        oscDiagram.addDiagramListener("ChangedSelection", function (e) {
            //var selnode = oscDiagram.selection.first();
            //oscDiagram.model.selectedNodeData = (selnode instanceof go.Node ? selnode.data : null);
            //onSelectionChanged(e);
            $rootScope.$apply();
        });

        // define the Node template
        oscDiagram.nodeTemplate =
            $$(go.Node, "Auto",
                { doubleClick: nodeDoubleClick },
                {
                    // handle dragging a Node onto a Node to (maybe) change the reporting relationship
                    mouseDragEnter: function (e, node, prev) {
                        var diagram = node.diagram;
                        var selnode = diagram.selection.first();
                        if (!mayWorkFor(selnode, node)) return;
                        var shape = node.findObject("SHAPE");
                        if (shape) {
                            shape._prevFill = shape.fill; // remember the original brush
                            shape.fill = "darkred";
                        }
                    },
                    mouseDragLeave: function (e, node, next) {
                        var shape = node.findObject("SHAPE");
                        if (shape && shape._prevFill) {
                            shape.fill = shape._prevFill; // restore the original brush
                        }
                    },
                    mouseDrop: function (e, node) {
                        var diagram = node.diagram;
                        var selnode = diagram.selection.first(); // assume just one Node in selection
                        if (mayWorkFor(selnode, node)) {
                            // find any existing link into the selected node
                            var link = selnode.findTreeParentLink();
                            if (link !== null) { // reconnect any existing link
                                link.fromNode = node;
                            } else { // else create a new link
                                diagram.toolManager.linkingTool.insertLink(node, node.port, selnode, selnode.port);
                            }
                        }
                    }
                },
                // for sorting, have the Node.text be the data.name
                new go.Binding("text", "name"),
                new go.Binding("fill", "color"),
                // bind the Part.layerName to control the Node's layer depending on whether it isSelected
                new go.Binding("layerName", "isSelected", function (sel) { return sel ? "Foreground" : ""; }).ofObject(),
                // define the node's outer shape
                $$(go.Shape, "RoundedRectangle",
                {
                    name: "SHAPE",
                    fill: "white",
                    stroke: null,
                    // set the port properties:
                    portId: "",
                    fromLinkable: true,
                    toLinkable: true,
                    cursor: "pointer"
                }),
                $$(go.Panel, "Horizontal",
                    $$(go.Picture,
                        {
                            maxSize: new go.Size(39, 50),
                            margin: new go.Margin(6, 8, 6, 10),
                        },
                        new go.Binding("source", "key", findHeadShot)),
                    // define the panel where the text will appear
                    $$(go.Panel, "Table",
                        {
                            maxSize: new go.Size(150, 999),
                            margin: new go.Margin(6, 10, 0, 3),
                            defaultAlignment: go.Spot.Left
                        },
                        $$(go.RowColumnDefinition, { column: 2, width: 4 }),
                        $$(go.TextBlock, textStyle(), // the name
                            {
                                row: 0,
                                column: 0,
                                columnSpan: 5,
                                font: "12pt Segoe UI,sans-serif",
                                editable: true,
                                isMultiline: false,
                                minSize: new go.Size(10, 16)
                            },
                            new go.Binding("text", "name").makeTwoWay()),
                        $$(go.TextBlock, "Title: ", textStyle(),
                        { row: 1, column: 0 }),
                        $$(go.TextBlock, textStyle(),
                            {
                                row: 1,
                                column: 1,
                                columnSpan: 4,
                                editable: true,
                                isMultiline: false,
                                minSize: new go.Size(10, 14),
                                margin: new go.Margin(0, 0, 0, 3)
                            },
                            new go.Binding("text", "title").makeTwoWay()),
                        $$(go.TextBlock, textStyle(),
                            { row: 2, column: 0 },
                            new go.Binding("text", "key", function (v) { return "ID: " + v; })),
                        $$(go.TextBlock, textStyle(),
                            { row: 2, column: 3, },
                            new go.Binding("text", "parent", function (v) { return "Parent: " + v; })),
                        $$(go.TextBlock, textStyle(), // the comments
                            {
                                row: 3,
                                column: 0,
                                columnSpan: 5,
                                font: "italic 9pt sans-serif",
                                wrap: go.TextBlock.WrapFit,
                                editable: true, // by default newlines are allowed
                                minSize: new go.Size(10, 14)
                            },
                            new go.Binding("text", "comments").makeTwoWay())
                    ) // end Table Panel
                ) // end Horizontal Panel
            ); // end Node

        // define the Link template
        oscDiagram.linkTemplate =
            $$(go.Link, go.Link.Orthogonal,
                { corner: 5, relinkableFrom: true, relinkableTo: true },
                $$(go.Shape, { strokeWidth: 4, stroke: "#00a4a4" })); // the link shape

        // read in the JSON-format data from the "mySavedModel" element
        load($rootScope);
    };

    // Allow the user to edit text when a single node is selected
    function onSelectionChanged(e) {
        var node = e.diagram.selection.first();
        if (node instanceof go.Node) {
            updateProperties(node.data);
        } else {
            updateProperties(null);
        }
    };

    function load($rootScope) {
        //oscDiagram.model = go.Model.fromJson(document.getElementById("mySavedModel").value);
        oscDiagram.model = go.Model.fromJson($rootScope.organisationStructureModel);
    };

    // Show the diagram's model in JSON format
    function save() {
        organisationStructureModel = organisationStructureModel.toJson();
        oscDiagram.isModified = false;
    };

    // Update the data fields when the text is changed
    function updateData(text, field) {
        var node = oscDiagram.selection.first();
        // maxSelectionCount = 1, so there can only be one Part in this collection
        var data = node.data;
        if (node instanceof go.Node && data !== null) {
            var model = oscDiagram.model;
            model.startTransaction("modified " + field);
            if (field === "name") {
                model.setDataProperty(data, "name", text);
            } else if (field === "title") {
                model.setDataProperty(data, "title", text);
            } else if (field === "comments") {
                model.setDataProperty(data, "comments", text);
            }
            model.commitTransaction("modified " + field);
        }
    };

    // This is called when the user has finished inline text-editing
    function onTextEdited(e) {
        var tb = e.subject;
        if (tb === null || !tb.name) return;
        var node = tb.part;
        if (node instanceof go.Node) {
            updateProperties(node.data);
        }
    };


    // Update the HTML elements for editing the properties of the currently selected node, if any
    function updateProperties(data) {
        //if (data === null) {
        //    document.getElementById("propertiesPanel").style.display = "none";
        //    document.getElementById("name").value = "";
        //    document.getElementById("title").value = "";
        //    document.getElementById("comments").value = "";
        //} else {
        //    document.getElementById("propertiesPanel").style.display = "block";
        //    document.getElementById("name").value = data.name || "";
        //    document.getElementById("title").value = data.title || "";
        //    document.getElementById("comments").value = data.comments || "";
        //}
    };
})();
