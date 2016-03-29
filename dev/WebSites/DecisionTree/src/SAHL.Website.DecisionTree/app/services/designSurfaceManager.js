'use strict';

/* Services */

angular.module('sahl.tools.app.services.designSurfaceManager', [
    'sahl.tools.app.serviceConfig',
    'SAHL.Services.Interfaces.DecisionTreeDesign.queries',
    'SAHL.Services.Interfaces.DecisionTreeDesign.commands'
])
.factory('$designSurfaceManager', ['$rootScope', '$parseHttpService', '$codemirrorVariablesService', '$q', '$eventAggregatorService', '$eventDefinitions',
    function ($rootScope, GOparseHttpService, GOvariablesService, $q, $eventAggregatorService, $eventDefinitions) {
    var GO = go.GraphObject.make;  // for conciseness in defining templates
    var designSurface = null;
    var designPalette = null;

    var isStarting = true;
    var internalFunctions = {
        defineShapes: function () {
            // define the Node template for regular nodes
            var fontStyle = "9pt Segoe UI, Arial, sans-serif"
            var sahlOrange = "#ed7900"
            var strokeColor = "#525252"
            var lightText = "#444";//"525252";
            var darkText = '#454545';
            var startColor = "#FFFFFF";
            var mainColor = "#FFFFFF";
            var endColor = "#FFAAAA";

            //go.TextBlock.prototype.codeholder = "";
            //go.TextBlock.prototype.required_varholder = []
            //go.TextBlock.prototype.output_varholder = [];


            designSurface.toolManager.linkingTool.linkValidation = function (fromnode, fromport, tonode, toport) {
                if (fromnode.category != 'Decision') { // nodes should only have 1 out link except Decisions which should have 2 only.
                    if (fromnode.findLinksOutOf().count > 0) {
                        return false;
                    }
                }
                if (tonode && toport) { // Dont allow an in-link to an out-link
                    var links = designSurface.links;
                    while (links.next()) {
                        var l = links.value;
                        var fromNode = l.fromNode;
                        var port = l.fromPort;

                        if (fromNode == tonode && port == toport) {
                            return false;
                        }
                    }
                }
                return true;
            };


            designSurface.nodeTemplateMap.add("Start",
              GO(go.Node, "Vertical", internalFunctions.nodeStyle(),
                new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
                GO(go.Panel, "Spot",
                  GO(go.Shape, "circle", {
                      width: 20,
                      height: 20,
                      stroke: lightText,
                      fill: "#CDDE87",
                      alignment: go.Spot.Center
                  }),
                    internalFunctions.makePort("B", go.Spot.Bottom, true, false)
                ),
                GO(go.TextBlock, "Start", {
                    margin: 5,
                    font: fontStyle,
                    stroke: lightText,
                    background: startColor
                }),
                {
                    selectionChanged: function (p) {
                        if (p.isSelected) p.layerName = "Foreground"; else p.layerName = "Nodes";
                    },
                    layerName: "Nodes"
                }
              ));

            designSurface.nodeTemplateMap.add("Process",  // the default category
              GO(go.Node, "Vertical", internalFunctions.nodeStyle(),
                new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
                GO(go.Panel, "Spot",
                  GO(go.Shape, "RoundedRectangle", {
                      fill: mainColor,
                      stroke: lightText,
                      width: 40,
                      height: 24,
                      name: "shape"
                  },
                            new go.Binding("figure", "figure")
                        ),
                        GO(go.Shape, { stroke: sahlOrange, strokeWidth: 1, geometryString: "m 16.790928,40.496513 c -0.176693,0.01766 -0.321331,0.0083 -0.480355,0.04367 -0.053,0.371053 -0.113329,0.746661 -0.131006,1.135383 -0.07068,0 -0.103994,0.02599 -0.174674,0.04367 -0.01766,0 -0.06967,-0.01766 -0.08734,0 -0.141354,0.03534 -0.251664,0.104004 -0.393018,0.174674 -0.123683,0.053 -0.243334,0.104003 -0.349348,0.174674 -0.300377,-0.247368 -0.572997,-0.451329 -0.873372,-0.698697 l -0.04367,-0.04367 c -0.07068,0.053 -0.121671,0.121671 -0.174674,0.174675 -0.123684,0.08834 -0.217334,0.199665 -0.30568,0.30568 -0.159023,0.141353 -0.295333,0.303662 -0.436686,0.480354 0.229699,0.300376 0.494996,0.572997 0.742366,0.873372 -0.07068,0.106021 -0.121671,0.243334 -0.174675,0.349349 -0.07068,0.141354 -0.139334,0.295333 -0.174674,0.436686 -0.03534,0.07068 -0.04367,0.165336 -0.04367,0.218343 -0.388721,0.03534 -0.807999,0.078 -1.179052,0.131006 -0.03534,0.176692 -0.04367,0.329662 -0.04367,0.524023 0,0.265038 0.03432,0.538666 0.08734,0.786035 0.371053,0.03534 0.746662,0.052 1.135383,0.08734 0,0.07068 0.0084,0.165335 0.04367,0.218343 0.03534,0.159022 0.103994,0.277662 0.174674,0.436686 0.07068,0.123683 0.08633,0.243334 0.174675,0.349348 -0.247368,0.300376 -0.468998,0.616664 -0.698698,0.917041 0.265037,0.371053 0.589656,0.652002 0.960709,0.91704 0.282707,-0.229699 0.546997,-0.494997 0.829704,-0.742366 l 0.04367,0 c 0.08834,0.053 0.173666,0.121672 0.262012,0.174675 0.159022,0.07068 0.365001,0.139337 0.524023,0.174674 0.07068,0.01766 0.121671,0.02601 0.174674,0.04367 0.03534,0.371053 0.078,0.79033 0.131006,1.179052 0.159024,0.01766 0.303662,0 0.480355,0 l 0.262011,0 c 0.194363,0 0.373331,-0.0083 0.567692,-0.04367 0.03534,-0.371052 0.09567,-0.746662 0.131006,-1.135383 l 0.131006,-0.04367 c 0.159023,-0.03534 0.339001,-0.103995 0.480354,-0.174674 0.123684,-0.053 0.225664,-0.129996 0.349349,-0.218343 0,0.01766 -0.01766,0.04367 0,0.04367 0.282707,0.24737 0.616663,0.468998 0.91704,0.698698 0.053,-0.03534 0.104004,-0.078 0.174675,-0.131006 0.10602,-0.08834 0.217334,-0.155997 0.30568,-0.262011 0.159022,-0.159024 0.295332,-0.373331 0.436686,-0.567692 -0.24737,-0.300377 -0.494998,-0.590665 -0.742366,-0.873372 0.053,-0.08834 0.121672,-0.199666 0.174674,-0.30568 0.053,-0.141354 0.09567,-0.295333 0.131006,-0.436686 0,-0.01766 0.02601,-0.04367 0.04367,-0.04367 0,-0.07068 0.02599,-0.09567 0.04367,-0.131006 0.388722,-0.03534 0.76433,-0.09567 1.135384,-0.131005 0.03534,-0.176693 0.02601,-0.347333 0.04367,-0.524024 l 0,-0.131005 0,-0.174675 c -0.01766,-0.176691 -0.0083,-0.347331 -0.04367,-0.524023 -0.371054,-0.03534 -0.746662,-0.052 -1.135384,-0.08734 -0.01766,-0.053 -0.04367,-0.121671 -0.04367,-0.174674 -0.053,-0.159023 -0.104002,-0.321331 -0.174674,-0.480355 l -0.174675,-0.34935 C 20.03376,42.87759 20.25539,42.587301 20.502758,42.286925 20.23772,41.933542 19.913101,41.608923 19.542049,41.326216 l 0,0.04367 c -0.300377,0.2297 -0.590665,0.451329 -0.873372,0.698697 -0.123683,-0.07068 -0.199665,-0.121665 -0.30568,-0.174674 -0.141353,-0.07068 -0.321331,-0.139337 -0.480354,-0.174674 -0.053,-0.01766 -0.078,-0.04367 -0.131006,-0.04367 -0.03534,-0.388722 -0.078,-0.746662 -0.131006,-1.135383 -0.176691,-0.03534 -0.373331,-0.02601 -0.567692,-0.04367 l -0.131006,0 z m 0.174674,2.79479 c 0.459399,-0.01766 0.799669,0.144639 1.135384,0.480355 0.318045,0.318044 0.480354,0.693654 0.480354,1.135383 0,0.459399 -0.162309,0.843336 -0.480354,1.179052 -0.335715,0.318044 -0.675985,0.480355 -1.135384,0.480355 -0.441729,0 -0.861006,-0.162311 -1.179052,-0.480355 -0.318046,-0.335716 -0.480355,-0.719653 -0.480355,-1.179052 0,-0.441729 0.162309,-0.817339 0.480355,-1.135383 0.318046,-0.335716 0.737323,-0.498024 1.179052,-0.480355 z m 0,0.218343 c -0.388723,0 -0.721671,0.127979 -1.004378,0.393017 -0.265038,0.282707 -0.393017,0.615656 -0.393017,1.004378 0,0.388722 0.127973,0.721671 0.393017,1.004378 0.282707,0.282707 0.615655,0.393017 1.004378,0.393017 0.388722,0 0.721671,-0.11031 1.004378,-0.393017 0.282707,-0.282707 0.393017,-0.615656 0.393017,-1.004378 0,-0.388722 -0.11031,-0.721671 -0.393017,-1.004378 -0.282707,-0.265038 -0.615656,-0.393017 -1.004378,-0.393017 z m 0,0.349349 c 0.282707,0 0.530337,0.111325 0.742366,0.30568 0.212031,0.212031 0.30568,0.44199 0.30568,0.742366 0,0.300376 -0.09365,0.548004 -0.30568,0.742366 -0.212029,0.21203 -0.459659,0.30568 -0.742366,0.30568 -0.282707,0 -0.530337,-0.09366 -0.742366,-0.30568 -0.194361,-0.194362 -0.30568,-0.44199 -0.30568,-0.742366 0,-0.282707 0.111325,-0.530335 0.30568,-0.742366 0.212029,-0.194361 0.459659,-0.30568 0.742366,-0.30568 z m -5.371237,2.358104 c -0.194361,0.477068 -0.321331,0.937997 -0.480355,1.397395 l -0.262011,0 -0.08734,0 c -0.212031,0.01766 -0.373331,0.052 -0.567692,0.08734 -0.159022,0.01766 -0.321333,0.078 -0.480355,0.131006 C 9.433905,47.426448 9.125947,46.998841 8.84324,46.610119 l -0.04367,0 c -0.106021,0.03534 -0.173666,0.078 -0.262012,0.131006 -0.176692,0.07068 -0.364999,0.155997 -0.524023,0.262012 -0.247368,0.123683 -0.486668,0.285993 -0.698697,0.480354 0.212029,0.441731 0.442998,0.911996 0.655028,1.353727 C 7.846183,48.960901 7.726532,49.045213 7.620518,49.186566 7.479164,49.32792 7.333515,49.490229 7.2275,49.666921 7.1745,49.737601 7.1755,49.840586 7.14016,49.928933 6.645424,49.840593 6.150165,49.737598 5.6554279,49.666921 5.5670819,49.878952 5.4640931,50.09225 5.3934164,50.32195 5.3227404,50.675334 5.2540803,50.990613 5.218742,51.326328 l 1.353726,0.567691 0,0.218343 c 0,0.194363 0.03432,0.39933 0.08734,0.611361 0.01766,0.17669 0.078,0.347331 0.131005,0.524023 -0.40639,0.282707 -0.816329,0.572996 -1.2227202,0.873372 0.2120301,0.530075 0.5199882,1.025333 0.8733722,1.484732 l 0.04367,0 c 0.441731,-0.212031 0.850659,-0.39933 1.310058,-0.61136 0.10602,0.10602 0.243333,0.199665 0.349349,0.30568 0.159022,0.141353 0.355661,0.269332 0.567692,0.393017 0.053,0.03534 0.103994,0.078 0.174674,0.131006 -0.08834,0.477068 -0.147667,0.946326 -0.218343,1.441063 0.194361,0.08834 0.399329,0.165336 0.61136,0.218343 0.053,0.01766 0.128994,0.052 0.30568,0.08734 0.247369,0.07068 0.512667,0.09566 0.742367,0.131005 0.19436,-0.477067 0.365001,-0.920327 0.524023,-1.397395 l 0.218343,0 c 0.212031,0 0.416999,-0.0084 0.61136,-0.04367 0.176692,-0.03534 0.347331,-0.078 0.524023,-0.131006 l 0.04367,0 c 0.282707,0.406392 0.546996,0.81633 0.829703,1.222721 0.08834,-0.053 0.191334,-0.09567 0.262012,-0.131006 0.176692,-0.07068 0.321332,-0.173666 0.480354,-0.262012 0.265039,-0.159022 0.512667,-0.329662 0.742366,-0.524023 -0.194361,-0.459399 -0.399329,-0.894327 -0.61136,-1.353726 l 0.30568,-0.30568 c 0.123684,-0.159024 0.243334,-0.321333 0.349349,-0.480355 0,-0.01766 0.04367,-0.06967 0.04367,-0.08734 0.03534,-0.053 0.052,-0.121671 0.08734,-0.174675 0.494738,0.08834 0.989994,0.173667 1.484732,0.262012 0.08834,-0.21203 0.147665,-0.425331 0.218343,-0.655029 0.01766,-0.053 0.026,-0.104008 0.04367,-0.174674 0.01766,-0.053 0.052,-0.104008 0.08734,-0.174675 0.03534,-0.229699 0.06966,-0.442999 0.08734,-0.655029 -0.459398,-0.19436 -0.937997,-0.391001 -1.397395,-0.567691 l 0,-0.174675 c 0,-0.212029 -0.03432,-0.442998 -0.08734,-0.655029 -0.01766,-0.159022 -0.052,-0.321331 -0.08734,-0.480354 0.406392,-0.282707 0.833999,-0.616665 1.222721,-0.917041 -0.212031,-0.530076 -0.48465,-1.025333 -0.873372,-1.484732 -0.459399,0.194361 -0.911997,0.442999 -1.353726,0.655029 -0.123684,-0.123683 -0.225666,-0.287003 -0.349349,-0.393017 -0.159024,-0.123684 -0.347331,-0.217334 -0.524023,-0.305681 -0.053,-0.053 -0.121667,-0.09567 -0.174675,-0.131005 0.07068,-0.494737 0.147667,-0.989996 0.218343,-1.484733 -0.212029,-0.08834 -0.425329,-0.191334 -0.655028,-0.262011 -0.053,-0.01766 -0.121666,-0.04367 -0.174675,-0.04367 -0.07068,-0.03534 -0.121667,-0.04367 -0.174674,-0.04367 -0.212031,-0.053 -0.442998,-0.09567 -0.655032,-0.131003 z m -0.917041,3.624493 c 0.267247,-0.03534 0.555831,0.01666 0.829704,0.08734 0.565414,0.159022 0.983682,0.448302 1.266389,0.960709 0.300377,0.512406 0.377365,1.050324 0.218343,1.615738 -0.141354,0.565414 -0.465971,0.983682 -0.960709,1.266389 -0.530075,0.300376 -1.067993,0.377365 -1.615738,0.218343 C 9.849899,53.848757 9.431631,53.524138 9.148924,53.029402 8.848548,52.516995 8.771558,51.979076 8.930581,51.413664 9.071934,50.84825 9.422553,50.403983 9.934958,50.103606 c 0.24737,-0.141354 0.47512,-0.226674 0.742366,-0.262014 z m 0.04367,0.30568 c -0.231908,0.03092 -0.442998,0.138326 -0.655029,0.262012 -0.441731,0.247368 -0.706018,0.596978 -0.829703,1.091715 -0.141354,0.477068 -0.07269,0.929666 0.174674,1.353726 0.247368,0.441731 0.614647,0.732019 1.091715,0.873372 0.477068,0.123684 0.955666,0.07269 1.397395,-0.174674 0.441731,-0.265037 0.68835,-0.658316 0.829703,-1.135384 0.123684,-0.477067 0.07269,-0.929666 -0.174674,-1.353726 -0.247368,-0.441729 -0.614647,-0.749687 -1.091715,-0.873372 -0.238534,-0.07068 -0.510459,-0.07459 -0.742366,-0.04367 z m 0.08734,0.480355 c 0.170066,-0.0243 0.347332,-0.0094 0.524023,0.04367 0.353385,0.08834 0.635343,0.275646 0.829704,0.61136 0.19436,0.318046 0.219351,0.694662 0.131005,1.048046 -0.10602,0.353383 -0.293314,0.652312 -0.61136,0.829004 l 0,7.01e-4 C 11.345992,53.3371 10.987043,53.353761 10.63366,53.247748 10.262607,53.159408 9.998317,52.972102 9.803956,52.636387 9.627264,52.318343 9.584605,51.959393 9.672951,51.588341 c 0.10602,-0.353384 0.293314,-0.591672 0.61136,-0.786035 0.167857,-0.09717 0.353957,-0.150379 0.524023,-0.174679 z" }),
                        internalFunctions.makePort("T", go.Spot.Top, false, true),
                        internalFunctions.makePort("L", go.Spot.Left, true, true),
                        internalFunctions.makePort("R", go.Spot.Right, true, true),
                        internalFunctions.makePort("B", go.Spot.Bottom, true, false)
                    ),
                    GO(go.TextBlock,
                    {
                        font: fontStyle,
                        stroke: lightText,
                        margin: 8,
                        maxSize: new go.Size(80, 50),
                        wrap: go.TextBlock.WrapFit,
                        editable: true,
                        width: 800,
                        name: "text",
                        background: startColor
                    },
                    new go.Binding("text", "text").makeTwoWay()
                ), {
                    selectionChanged: function (p) {
                        if (p.isSelected) p.layerName = "Foreground"; else p.layerName = "Nodes";
                    },
                    layerName: "Nodes"
                }
              ));

            designSurface.nodeTemplateMap.add("Decision",
              GO(go.Node, "Vertical", internalFunctions.nodeStyle(),
                new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
                GO(go.Panel, "Spot",
                    GO(go.Shape, "Diamond", {
                        fill: mainColor,
                        stroke: strokeColor,
                        width: 40,
                        height: 40
                    }),
                    GO(go.TextBlock, "?", {
                        font: fontStyle,
                        stroke: sahlOrange,
                        margin: 5,
                        alignment: new go.Spot(0.535, 0.53)
                    }),
                    internalFunctions.makePort("T", go.Spot.Top, false, true),
                    internalFunctions.makePort("L", go.Spot.Left, true, true),
                    internalFunctions.makePort("R", go.Spot.Right, true, true),
                    internalFunctions.makePort("B", go.Spot.Bottom, true, false)
                ),
                GO(go.TextBlock, "Decision", {
                    font: fontStyle,
                    name: "text",
                    stroke: lightText,
                    background: startColor,
                    editable: true
                },
                    new go.Binding("text", "text").makeTwoWay()
                ), {
                    selectionChanged: function (p) {
                        if (p.isSelected) p.layerName = "Foreground"; else p.layerName = "Nodes";
                    },
                    layerName: "Nodes"
                }
            ));

            designSurface.nodeTemplateMap.add("End",
              GO(go.Node, "Spot", internalFunctions.nodeStyle(),
                new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
                GO(go.Panel, "Spot",
                  GO(go.Shape, "RoundedRectangle", { width: 40, height: 18, fill: endColor, stroke: strokeColor }),
                  GO(go.TextBlock, "END", { font: "bold 7pt Helvetica, Arial, sans-serif", stroke: lightText, alignment: new go.Spot(0.5, 0.56) })
                ),
                internalFunctions.makePort("T", go.Spot.Top, false, true),
                internalFunctions.makePort("L", go.Spot.Left, false, true),
                internalFunctions.makePort("R", go.Spot.Right, false, true),
                {
                    selectionChanged: function (p) {
                        if (p.isSelected) p.layerName = "Foreground"; else p.layerName = "Nodes";
                    },
                    layerName: "Nodes",
                }
              ));

            designSurface.nodeTemplateMap.add("SubTree",
              GO(go.Node, "Vertical", internalFunctions.nodeStyle(),
                new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
                GO(go.Panel, "Spot",
                GO(go.Shape, "Subroutine",
                  { fill: "#FFFFFF", stroke: strokeColor, width: 40, height: 28 }),
                  internalFunctions.makePort("T", go.Spot.Top, false, true),
                  internalFunctions.makePort("L", go.Spot.Left, true, true),
                  internalFunctions.makePort("R", go.Spot.Right, true, true),
                  internalFunctions.makePort("B", go.Spot.Bottom, true, true)
                ),
                GO(go.TextBlock,
                  {
                      margin: 5,
                      maxSize: new go.Size(200, NaN),
                      wrap: go.TextBlock.WrapFit,
                      textAlign: "center",
                      editable: true,
                      font: fontStyle,
                      stroke: strokeColor,
                      background: startColor
                  },
                  new go.Binding("text", "text").makeTwoWay()),
                  {
                      selectionChanged: function (p) {
                          if (p.isSelected) p.layerName = "Foreground"; else p.layerName = "Nodes";
                      },
                      layerName: "Nodes"
                  }
              ));

            designSurface.nodeTemplateMap.add("ClearMessages",
              GO(go.Node, "Vertical", internalFunctions.nodeStyle(),
                new go.Binding("location", "loc", go.Point.parse).makeTwoWay(go.Point.stringify),
                GO(go.Panel, "Spot",
                GO(go.Shape, "Subroutine",
                  { fill: "#FFFFFF", stroke: strokeColor, width: 40, height: 28 }),
                GO(go.Shape, "BpmnTaskScript", { fill: "#FFFFFF",strokeWidth: 1.5, stroke: sahlOrange, width: 45, height: 20, alignment: new go.Spot(0.5, 0.5) }),
                GO(go.Shape, "IrritationHazard", { fill: endColor, stroke: strokeColor, width: 15, height: 15, alignment: new go.Spot(0.69, 0.7) }),
                  internalFunctions.makePort("T", go.Spot.Top, false, true),
                  internalFunctions.makePort("L", go.Spot.Left, true, true),
                  internalFunctions.makePort("R", go.Spot.Right, true, true),
                  internalFunctions.makePort("B", go.Spot.Bottom, true, true)
                ),
                GO(go.TextBlock,
                  {
                      margin: 5,
                      maxSize: new go.Size(200, NaN),
                      wrap: go.TextBlock.WrapFit,
                      textAlign: "center",
                      editable: true,
                      font: fontStyle,
                      stroke: strokeColor,
                      background: startColor
                  },
                  new go.Binding("text", "text").makeTwoWay()),
                  {
                      selectionChanged: function (p) {
                          if (p.isSelected) p.layerName = "Foreground"; else p.layerName = "Nodes";
                      },
                      layerName: "Nodes"
                  }
              ));

            // replace the default Link template in the linkTemplateMap
            designSurface.linkTemplate =
              GO(go.Link,  // the whole link panel
                {
                    routing: go.Link.AvoidsNodes,
                    curve: go.Link.JumpOver,
                    corner: 5, toShortLength: 4,
                    relinkableFrom: true, relinkableTo: true, reshapable: true,
                    layerName: "Link",
                    selectionChanged: function (p) {
                        if (p.isSelected) {
                            p.oldLayerName = p.layerName;
                            p.layerName = "Foreground";
                        } else{
                            p.layerName = p.oldLayerName;
                        }
                    }
                },
                new go.Binding("layerName", "$status", function ($status) {
                    if ($status) {
                        if ($status == "#7FE01F") {
                            return "GreenLink"
                        } else {
                            return "RedLink"
                        }
                    }
                    return "Link"
                }),
                new go.Binding("points", "points", internalFunctions.toPointsConvertor).makeTwoWay(internalFunctions.fromPointsConvertor),//, internalFunctions.toPointsConvertor
                GO(go.Shape,{
                          isPanelMain: true,
                          stroke:"grey",
                          strokeWidth: 2
                      },
                      new go.Binding("stroke", "$status")
                  ),
                GO(go.Shape,  // the arrowhead
                  {
                      toArrow: "standard",
                      stroke: null, fill: "grey"
                  },
                  new go.Binding("fill", "$status")
                  ),
                GO(go.Panel, "Auto",
                  { visible: false, name: "LABEL", segmentIndex: 2, segmentFraction: 0.5 },
                  new go.Binding("visible", "linkType",
                        function (textFromLinkData) {
                            if (textFromLinkData === 'decision_yes') {
                                return true;
                            }
                            else
                                if (textFromLinkData === 'decision_no') {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                        }),
                  GO(go.Shape, "RoundedRectangle",  // the link shape
                    { fill: "#F8F8F8", stroke: null }),
                  GO(go.TextBlock, "",  // the label
                    {
                        name: "labeltext",
                        textAlign: "center",
                        font: "10pt helvetica, arial, sans-serif",
                        
                        stroke: "#919191",
                        margin: 2, editable: false
                    }, new go.Binding("text", "linkType",
                        function (textFromLinkData) {
                            if (textFromLinkData === 'decision_yes') {
                                return "Yes";
                            }
                            else
                                if (textFromLinkData === 'decision_no') {
                                    return "No";
                                }
                                else {
                                    return "";
                                }
                            }))
                )
              );
        },
        toPointsConvertor: function (nodeDataPoints) {
            if(Array.isArray(nodeDataPoints)){
                var list = new go.List(go.Point);  // make a list of Points
                for (var i = 0; i < nodeDataPoints.length; i += 2) {
                    list.add(new go.Point(nodeDataPoints[i], nodeDataPoints[i+1]));
                }
                return list;
            }
            return nodeDataPoints;
        },
        fromPointsConvertor: function(nodePoints){
            var nodes = [];
            var it = nodePoints.iterator;
            while (it.next()) {
                var item = it.value;
                nodes.push(+item.x.toFixed(1));
                nodes.push(+item.y.toFixed(1));
            }
            return nodes;
        },
        nodeStyle: function () {
            return {
                // the Node.location is at the center of each node
                locationSpot: go.Spot.Center,
                mouseDragEnter: function (e, obj) { /*GOmodalService.closeCurrentPopup();*/ },
                mouseEnter: function (e, obj) { internalFunctions.showPorts(obj.part, true); },
                mouseLeave: function (e, obj) { internalFunctions.showPorts(obj.part, false); /*GOmodalService.closeCurrentPopup();*/ }
            };
        },

        // Define a function for creating a "port" that is normally transparent.
        // The "name" is used as the GraphObject.portId, the "spot" is used to control how links connect
        // and where the port is positioned on the node, and the boolean "output" and "input" arguments
        // control whether the user can draw links from or to the port.
        makePort: function (name, spot, output, input) {
            // the port is basically just a small circle that has a white stroke when it is made visible
            return GO(go.Shape, "Circle",
                     {
                         fill: "transparent",
                         stroke: null,  // this is changed to "white" in the showPorts function
                         desiredSize: new go.Size(6, 6),
                         alignment: spot, alignmentFocus: spot,  // align the port on the main Shape
                         portId: name,  // declare this object to be a "port"
                         fromSpot: spot, toSpot: spot,  // declare where links may connect at this port
                         fromLinkable: output, toLinkable: input,  // declare whether the user may draw links to/from here
                         cursor: "pointer",  // show a different cursor to indicate potential link point
                         fromMaxLinks: 1 // allow only 1 out link
                     });
        },

        makeButton: function (position, namebt) {
            return GO("Button", { alignment: go.Spot.TopRight, click: changeCategory, visible: false, name: namebt }, GO(go.Shape, { figure: "Circle", width: 10, height: 10 }));
        },

        // Make all ports on a node visible when the mouse is over the node
        showPorts: function (node, show) {
            var diagram = node.diagram;
            if (!diagram || diagram.isReadOnly || !diagram.allowLink) return;
            var it = node.ports;
            var bt = node.findObject("one");
            if (bt != null) {
                bt.visible = show;
            }
            var bt = node.findObject("two");
            if (bt != null) {
                bt.visible = show;
            }
            while (it.next()) {
                var port = it.value;
                port.stroke = (show ? "blue" : null);
                //port.fill = (show ? "white" : null);
            }
        },
        
        load: function () {
            var modelData = $rootScope.document.dataModel;

            designSurface.model = go.Model.fromJson(modelData);
            designSurface.model.undoManager.isEnabled = true;

            // this next step is just to force an ismodified flag on a new tree, we just wiggle the start node around
            if ($rootScope.document.hasChanges) {
                var node = designSurface.model.nodeDataArray[0];
                var oldNodeLoc = node.loc;
                designSurface.model.setDataProperty(node, 'loc', '1 1');
                designSurface.model.setDataProperty(node, 'loc', oldNodeLoc);
            }

            $eventAggregatorService.publish($eventDefinitions.onModelCreated, designSurface.model);
        },

        attachSurfaceInternal: function (surfaceId, paletteId) {
            designSurface =
                GO(go.Diagram, surfaceId,  // must name or refer to the DIV HTML element
                  {
                      allowDrop: true,
                      "toolManager.hoverDelay": 250
                  });  // must be true to accept drops from the Palette

            var forelayer = designSurface.findLayer("Foreground");
            designSurface.addLayerBefore(GO(go.Layer, { name: "Link" }), forelayer);
            designSurface.addLayerBefore(GO(go.Layer, { name: "RedLink" }), forelayer);
            designSurface.addLayerBefore(GO(go.Layer, { name: "GreenLink" }), forelayer);
            designSurface.addLayerBefore(GO(go.Layer, { name: "Nodes" }), forelayer);

            designSurface.toolManager.linkingTool.temporaryLink.routing = go.Link.Orthogonal;
            designSurface.toolManager.relinkingTool.temporaryLink.routing = go.Link.Orthogonal;

            internalFunctions.defineShapes();

            designPalette =
              GO(go.Palette, paletteId,  // must name or refer to the DIV HTML element
                {
                    nodeTemplateMap: designSurface.nodeTemplateMap,  // share the templates used by designSurface
                    model: new go.GraphLinksModel([  // specify the contents of the Palette
                      { category: "Process", text: "Process" },
                      { category: "Decision", text: "If" },
                      { category: "End", text: "End" },
                      { category: "SubTree", text: "Sub Tree", figure: "RoundedRectangle" },
                      { category: "ClearMessages", text: "Clear Messages", figure: "ClearMessages" }
                    ])
                });

            internalFunctions.load();

            designSurface.model.addChangedListener(eventHandlers.onModelChanged);

            designSurface.addDiagramListener("ChangingSelection", eventHandlers.onChangingSelection);

            designSurface.addDiagramListener("ChangedSelection", eventHandlers.onChangedSelection);

            designSurface.addDiagramListener("LinkDrawn", eventHandlers.onLinkDrawn);

            designSurface.addDiagramListener("Modified", eventHandlers.onModified);

            designSurface.addDiagramListener("ClipboardChanged", eventHandlers.onClipboardChanged);

            designSurface.addDiagramListener("ViewportBoundsChanged", eventHandlers.onViewportBoundsChanged);


            designSurface.hasHorizontalScrollBar = true;
            designSurface.hasVerticalScrollbar = true;
            designPalette.requestUpdate();
            designSurface.requestUpdate();

            $eventAggregatorService.publish($eventDefinitions.onSurfaceAttached, designSurface);
        },
        detatchSurfaceInternal: function () {
            designSurface.removeDiagramListener("ChangedSelection", eventHandlers.onChangedSelection);
            designSurface.removeDiagramListener("LinkDrawn", eventHandlers.onLinkDrawn);
            designSurface.removeDiagramListener("Modified", eventHandlers.onModified);
            designSurface.removeDiagramListener("ClipboardChanged", eventHandlers.onClipboardChanged);
            designSurface.removeDiagramListener("ChangingSelection", eventHandlers.onChangingSelection);
            $eventAggregatorService.publish($eventDefinitions.onModelDestroyed);
            designSurface.model.clear();
            designSurface.clear();

            designSurface = null;
            designPalette = null;

            $('#designSurface').empty();
            $('designPalette').empty();
            
            $eventAggregatorService.publish($eventDefinitions.onSurfaceDetached);
        },
        reattachSurfaceInternal: function (surfaceId, paletteId) {
            designSurface.removeDiagramListener("ChangedSelection", eventHandlers.onChangedSelection);
            designSurface.removeDiagramListener("LinkDrawn", eventHandlers.onLinkDrawn);
            designSurface.removeDiagramListener("Modified", eventHandlers.onModified);

            internalFunctions.attachSurfaceInternal(surfaceId, paletteId);
        },
        changeSelectedNode: function (key) {
            console.log('changing location' + key);
            designSurface.select(designSurface.findPartForKey(key));
            var loc = designSurface.findPartForKey(key).location;            
            designSurface.alignDocument(new go.Spot(0, 0, 0, 0), new go.Spot(0, 0, loc.x, loc.y));            
        }
    }
    var surfaceId = "designSurface";
    var paletteId = "designPalette";
    var eventHandlers = {
        onChangedSelection: function (e) {
            var sel = e.diagram.selection;
            var str = "";
            if (!sel || sel.count == 0) {
                $eventAggregatorService.publish($eventDefinitions.onNodeSelectionChanged, null);
                return;
            }

            if (sel.count > 1) {
                //not handling multi-selection now
                $eventAggregatorService.publish($eventDefinitions.onNodeSelectionChanged, null);
                return;
            }
            // One object selected, display some information
            var elem = sel.first();
            $eventAggregatorService.publish($eventDefinitions.onNodeSelectionChanged, elem);
        },
        onLinkDrawn: function (e) {
            if (e.subject.fromNode.data.category === "Decision") {
                var label = e.subject.findObject("LABEL");
                var labeltext = e.subject.findObject("labeltext");
                var linkType = "none";
                if (label !== null) {
                    label.visible = true;
                    var hasYes = false;
                    var hasNo = false;
                    var index = 0;
                    var it = e.subject.fromNode.findLinksOutOf();
                    while (it.next()) {
                        index++;
                        var labeltext = it.value.findObject("labeltext");
                        if (labeltext) {
                            if (labeltext.text == "Yes") {
                                hasYes = true;
                            } else if (labeltext.text == "No") {
                                hasNo = true;
                            }
                        }
                    }
                    if (hasYes) {
                        labeltext.text = "No";
                        linkType = "decision_no";
                    }
                    if (hasNo) {
                        labeltext.text = "Yes";
                        linkType = "decision_yes";
                    }
                    if (!hasYes && !hasNo) {
                        labeltext.text = "Yes";
                        linkType = "decision_yes";
                    }
                    if ((index - 1) > 2) {
                        alert("Link not allowed please click on the canvas to cancel operation");
                        e.subject.remove();
                    }
                }

                var links = $rootScope.document.dataModel.linkDataArray;
                var count = links.length;
                links[count - 1].linkType = linkType;
            }
            else {
                var links = $rootScope.document.dataModel.linkDataArray;
                var count = links.length;
                links[count - 1].linkType = "link";
            }
        },
        onModified: function (e) {
            if (!isStarting) {
                if (!$rootScope.document.isReadOnly || $rootScope.document.isReadOnly == false) {
                    $rootScope.document.hasChanges = designSurface.isModified;
                }
            }
            else {
                isStarting = false;
            }
        },
        onModelChanged: function (e) {
            $eventAggregatorService.publish($eventDefinitions.onModelChanged, e);
        },
        onChangingSelection: function (e) {
            var sel = e.diagram.selection;
            var str = "";
            if (!sel || sel.count == 0) {
                $eventAggregatorService.publish($eventDefinitions.onBeforeNodeSelectionChanged, null);
                return;
            }

            if (sel.count > 1) {
                //not handling multi-selection now
                $eventAggregatorService.publish($eventDefinitions.onBeforeNodeSelectionChanged, null);
                return;
            }
            // One object selected, display some information
            var elem = sel.first();
            $eventAggregatorService.publish($eventDefinitions.onBeforeNodeSelectionChanged, elem);
        },
        onSelectionDeleting: function(e){

        },
        onSelectionDeleted: function (e) {

        },
        onClipboardChanged: function (e) {
            $eventAggregatorService.publish($eventDefinitions.onClipboardChanged, e);
        },
        onViewportBoundsChanged: function (e) {
            $eventAggregatorService.publish($eventDefinitions.onViewportBoundsChanged, e);
        }
    }

    var externalEventHandlers = {
        onDocumentLoaded: function (documentLoaded) {
            isStarting = true;
            if (designSurface !== null) {
                internalFunctions.reattachSurfaceInternal(surfaceId, paletteId);
            }
            else {
                internalFunctions.attachSurfaceInternal(surfaceId, paletteId);
            }
        },
        onDocumentUnloaded: function (unloadedDocument) {
            internalFunctions.detatchSurfaceInternal();
        },
        onDocumentSaved: function (savedDocument) {
            if (designPalette)
                designPalette.requestUpdate();
            if (designSurface)
                designSurface.requestUpdate();
        }
        ,
        onChangeSelectedNode: function (key) {
            internalFunctions.changeSelectedNode(key);
        }
    }

    $eventAggregatorService.subscribe($eventDefinitions.onChangeSelectedNode, externalEventHandlers.onChangeSelectedNode);
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, externalEventHandlers.onDocumentLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, externalEventHandlers.onDocumentUnloaded);
    return {
        attachSurface: function (surfaceId, paletteId) {
            if (designSurface !== null) {
                internalFunctions.reattachSurfaceInternal(surfaceId, paletteId);
            }
            else {
                internalFunctions.attachSurfaceInternal(surfaceId, paletteId);
            }
        },
        detatchSurface: function () {
            internalFunctions.detatchSurfaceInternal();
        },
        updateBindings: function() {
            designSurface.updateAllTargetBindings();
        },
        requestUpdate: function () {
            if (designPalette != null) {
                designPalette.requestUpdate();
            }
            if (designSurface != null) {
                designSurface.requestUpdate();
            }
        },
        markAsModified: function (isModified) {
            designSurface.isModified = isModified;
        },
        exportToSVG: function (width, height) {
            if (width !== undefined && height !== undefined) {
                return designSurface.makeSVG({ size: new go.Size(width, height) });
            }
            else {
                return designSurface.makeSVG({ scale: 1, maxSize: new go.Size(Infinity, Infinity) });
            }
        },
        exportToPNG: function (width, height) {
            if (width !== undefined && height !== undefined) {
                return designSurface.makeImage({ size: new go.Size(width, height), background: "rgba(255, 255, 255, 255)", details: 1, type: "image/png" });
            }
            else {
                return designSurface.makeImage({ scale: 1, background: "rgba(255, 255, 255, 255)", details: 1, type: "image/png", maxSize: new go.Size(Infinity, Infinity) });
            }
        },
        getThumbnail: function () {
            return designSurface.makeImage({ size: new go.Size(110, 110), background: "rgba(255, 255, 255, 255)" }).attributes[0].nodeValue;
        },
        exportToPDF: function () {
            
            var pdfRendering = {
                pageY: 35,
                incrementY:function(y){
                    pdfRendering.pageY += y;
                    if (pdfRendering.pageY > 200) // we want to make a page break here and reset y
                    {
                        doc.addPage()
                        pdfRendering.setPageTitle();
                        pdfRendering.setHeading(pageHeading, 30)
                        pdfRendering.pageY = 45;
                    }
                    return pdfRendering.pageY;
                },
                setPageTitle: function () {
                    doc.setFontSize(18);
                    doc.setFontType("bold");
                    doc.text(20, 20, 'Decision Tree : ' + $rootScope.document.data.name + " (v" + $rootScope.document.data.version + ")");
                },
                setHeading : function (heading, lineWidth) {
                    doc.setFontSize(16);
                    doc.setFontType("normal");
                    doc.text(20, 30, heading);
                    doc.setLineWidth(0.5);
                    doc.line(20, 31, 20+lineWidth, 31);
                },
                setVariableTypeHeading : function (x1, x2, y) {
                    doc.setFontSize(10);
                    doc.text(x1, y, 'Name'); doc.text(x2, y, 'Type');
                    doc.setLineWidth(0.2);
                    doc.line(x1, y + 1, x1 + 10, y + 1); doc.line(x2, y + 1, x2 + 8, y + 1);
                },
                writeVariables : function (variableUsage, x1, x2, y) {
                    $.each($rootScope.document.data.tree.variables, function (index, variable) {
                        if (variable.usage == variableUsage) {
                            if (y > 200) // we want to create a second column of variables
                            {
                                x1 = 140; x2 = 200;
                                y = 50;

                                this.setVariableTypeHeading(doc, x1, x2, 40);
                            }

                            doc.setFontSize(10);
                            doc.text(x1, y, variable.name); doc.text(x2, y, variable.type);
                            y = y + 5;
                        }
                    });
                },
                writeTestCases : function (y) {

                    // stories
                    $.each($rootScope.document.data.testCases, function (index, testCase) {
                        var y = pdfRendering.incrementY(10);
                        doc.setFontSize(10);
                        doc.setFontType("bold");
                        doc.text(20, y, testCase.name);
                        doc.setFontType("normal");
                        if (testCase.scenarios) {
                            // craigs temp paging test
                            //for (var i = 0; i < 30; i++) {
                            //    y = pdfRendering.incrementY(5);
                            //    doc.setFontSize(10);
                            //    doc.text(40, y, "craigs temp scenario " + i);
                            //}

                            // scenarios
                            y = pdfRendering.incrementY( 5);
                            $.each(testCase.scenarios, function (index, scenario) {
                                doc.setFontSize(10);
                                doc.text(40, y, scenario.name);
                                // inputs
                                if (scenario.input_values) {
                                    y = pdfRendering.incrementY( 5);
                                    doc.setFontSize(10);
                                    doc.setFontStyle("italic");
                                    doc.text(60, y, "Given:");
                                    doc.setFontStyle("normal");
                                    y = pdfRendering.incrementY( 5);
                                    $.each(scenario.input_values, function (index, inputValue) {
                                        doc.setFontSize(10);
                                        doc.text(60, y, inputValue.name + " of " + inputValue.value);
                                        y = pdfRendering.incrementY( 5);
                                    });
                                }
                                // outputs
                                if (scenario.output_values) {
                                    y = pdfRendering.incrementY(5);
                                    doc.setFontSize(10);
                                    doc.setFontStyle("italic");
                                    doc.text(60, y, "Expect:");
                                    doc.setFontStyle("normal");
                                    y = pdfRendering.incrementY( 5);
                                    $.each(scenario.output_values, function (index, outputValue) {
                                        doc.setFontSize(10);
                                        doc.text(60, y, "Expect " + outputValue.name + " to equal " + outputValue.value);
                                        y = pdfRendering.incrementY( 5);
                                    });
                                }
                                y = pdfRendering.incrementY( 5);
                            });
                        }
                    });
                }
            }

            // create a new pdf document
            var doc = new jsPDF('landscape');
            var pageHeading = "";

            // DecisionTree Image : we have to use JPEG as jsPDF only supports this format at the moment           
            pdfRendering.setPageTitle();

            // generate jpeg
            var jpeg = designSurface.makeImage({ size: new go.Size(800, 600), background: "rgba(255, 255, 255, 255)", type: "image/jpeg", details: 1 });

            // convert the image size from pixels to mm
            var dpi = 96;
            var mmWidth = (jpeg.width * 25.4) / dpi;
            var mmHeight = (jpeg.height * 25.4) / dpi;

            doc.addImage(jpeg.src, 'JPEG', 15, 40, mmWidth, mmHeight);

            // Input Variables
            doc.addPage()
            pdfRendering.setPageTitle();
            pdfRendering.setHeading("Input Variables", 38)
            pdfRendering.setVariableTypeHeading(20, 80, 40);
            pdfRendering.writeVariables('input', 20, 80, 50);

            // Output Variables
            doc.addPage();
            pdfRendering.setPageTitle();
            pdfRendering.setHeading("Output Variables", 42)
            pdfRendering.setVariableTypeHeading(20, 80, 40);
            pdfRendering.writeVariables('output', 20, 80, 50);

            // last page should contain the tests
            pageHeading = "Test Cases";
            doc.addPage()
            pdfRendering.setPageTitle();
            pdfRendering.setHeading(pageHeading, 30)
            pdfRendering.writeTestCases(35);

            return doc;
        },
        setReadOnly: function (isReadOnly) {
            designSurface.isReadOnly = isReadOnly;
        },
        start: function () { },
        zoom: function (scale) {
            designSurface.scale = scale / 100.0;
        }
    }
}]);