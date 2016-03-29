//function init(GOmodalService, GOvariablesService, GOdecisionTreeManagementService, GOvariableDataManager) {
//    //if (window.goSamples) goSamples();  // init for these samples -- you don't need to call this



    





    


//    // temporary links used by LinkingTool and RelinkingTool are also orthogonal:

//    var jsontreeMap = GOdecisionTreeManagementService.convertToMapObject(jsontest2);
//   // document.getElementById("mySavedModel").value = jsontreeMap;

//   load();  // load an initial diagram from some JSON text <-- not to be confused with the actual tree JSON

//    // initialize the Palette that is on the left side of the page




    

//}

//// Show the diagram's model in JSON format that the user may edit

//function save() {
//    var str = designSurface.model.toJson();
//    //document.getElementById("mySavedModel").value = str;
//    designSurface.isModified = false;
//}

//var demoJSON = {
//    "class": "go.GraphLinksModel",
//    "linkFromPortIdProperty": "fromPort",
//    "linkToPortIdProperty": "toPort",
//    "nodeDataArray": [
//    { "key": 1, "category": "Start", "loc": "409 3", "text": "Start" },
//    { "key": 2, "category": "Decision", "loc": "348 139.5", "text": "Salaried" },
//    { "key": 3, "category": "Decision", "loc": "623 202.5", "text": "New Purchase" },
//    { "key": 4, "category": "Decision", "loc": "52 242.5", "text": "Salaried Ded" },
//    { "key": 5, "category": "Process", "loc": "413 347", "text": "Loan Amount", "code": "alias if #helloworld\n", "required_variables": ["2", "3"], "output_variables": ["1"] },
//    { "key": 6, "category": "Process", "loc": "224 431", "text": "Household Income" },
//    { "key": 7, "category": "Process", "loc": "514 513", "text": "Credit Category" },
//    { "key": 8, "category": "End", "loc": "532 660", "text": "Enjoy!" }
//    ],
//    "linkDataArray": [
//    { "from": 1, "to": 2, "fromPort": "B", "toPort": "T", "points": [409, 2, 409, 12, 409, 20, 409, 20, 409, 36, 348, 36, 348, 102.5, 348, 112.5] },
//    { "from": 2, "to": 3, "fromPort": "R", "toPort": "T", "points": [368.5, 133, 378.5, 133, 623, 133, 623, 149.25, 623, 165.5, 623, 175.5] },
//    { "from": 3, "to": 5, "fromPort": "L", "toPort": "T", "points": [602.5, 196, 592.5, 196, 588, 196, 588, 196, 413, 196, 413, 310, 413, 320] },
//    { "from": 5, "to": 6, "fromPort": "B", "toPort": "T", "points": [413, 345, 413, 355, 413, 364, 413, 364, 413, 388, 224, 388, 224, 394, 224, 404] },
//    { "from": 6, "to": 7, "fromPort": "B", "toPort": "T", "points": [224, 429, 224, 439, 224, 436, 224, 436, 224, 468, 514, 468, 514, 476, 514, 486] },
//    { "from": 7, "to": 8, "fromPort": "B", "toPort": "T", "points": [514, 511, 514, 521, 514, 532, 514, 532, 514, 548, 532, 548, 532, 640.5, 532, 650.5] },
//    { "from": 3, "to": 5, "fromPort": "B", "toPort": "T", "points": [623, 216.5, 623, 226.5, 623, 236, 413, 236, 413, 310, 413, 320], "visible": true, "text": "Yes" },
//    { "from": 2, "to": 4, "fromPort": "B", "toPort": "T", "visible": true, "text": "Yes", "points": [348, 153.5, 348, 163.5, 348, 184.5, 52, 184.5, 52, 205.5, 52, 215.5] }
//    ]
//}

//function load() {
//    //var str = document.getElementById("mySavedModel").value;
//    designSurface.model = go.Model.fromJson(demoJSON);
//    designSurface.model.undoManager.isEnabled = true;
//}

//function makeSVG() {
//    var svg = designSurface.makeSVG({
//        scale: 0.5
//    });
//    svg.style.border = "1px solid black";
//    obj = document.getElementById("SVGArea");
//    obj.appendChild(svg);
//}



//// this function changes the category of the node data to cause the Node to be replaced





////var jsonObject = {
////    "decisiontree": {
////        "id": "",
////        "name": "",
////        "tree": {
////            "variables": [],
////            "nodes": [],
////            "links": []
////        },

////        "layout": {
////            "nodes": [],
////            "links": []
////        }
////    }
////}

////var extend = function (objectToExtend) {
    
////    objectToExtend.api = {};

////    objectToExtend.api.addTreeName = function (id, name) {
////        objectToExtend.decisiontree.id = id;
////        objectToExtend.decisiontree.name = name;
////    }
////    objectToExtend.api.addVariables = function (id, usage, type, type_enumeration_values, output_variables, code) {
////        objectToExtend.decisiontree.tree.variables.push({ "id": id, "usage": usage, "type": type, "type_enumeration_values": type_enumeration_values, "name": name });
////    }
////    objectToExtend.api.addLinks = function (id, type, fromNodeId, toNodeId) {
////        objectToExtend.decisiontree.tree.variables.push({ "id": id, "type": type, "fromNodeId": fromNodeId, "toNodeId": toNodeId });
////    }
////    objectToExtend.api.addNode = function (id, name, type, required_variables, output_variables, code) {
////        objectToExtend.decisiontree.tree.nodes.push({ "id": id, "name": name, "type": type, "required_variables": required_variables, "output_variables": output_variables, "code": code });
////    }
////    objectToExtend.api.addLayoutNode = function (id, category, loc, text) {
////        objectToExtend.decisiontree.layout.nodes.push({ "id": id, "category": category, "loc": loc, "text": text });
////    }
////    objectToExtend.api.addLayoutLinks = function (linkid, from, to, fromPort, toPort, points) {
////        objectToExtend.decisiontree.layout.links.push({ "linkid": linkid, "from": from, "to": to, "fromPort": fromPort, "toPort": toPort, "points": points });
////    }

////}

////for testing right now only. Don't do this:

//var globaltest2 = {
//    "variables": {
//        "groups": [{
//            "name": "Credit",
//            "variables": [{
//                "id": "1",
//                "type": "float",
//                "usage": "global",
//                "name": "HouseHold Income"
//            },
//			{
//			    "id": "2",
//			    "usage": "global",
//			    "type": "enumeration",
//			    "type_enumeration_values": ["Switch",
//				"NewPurchase",
//				"Refinance"],
//			    "name": "Loan Purpose"
//			},
//			{
//			    "id": "3",
//			    "usage": "global",
//			    "type": "enumeration",
//			    "type_enumeration_values": ["Salaried",
//				"Salaried With Deduction",
//				"Self Employed"],
//			    "name": "Household Income Type"
//			},
//			{
//			    "id": "4",
//			    "usage": "global",
//			    "type": "float",
//			    "name": "Loan Amount"
//			},
//			{
//			    "id": "5",
//			    "usage": "global",
//			    "type": "float",
//			    "name": "Property Value"
//			},
//			{
//			    "id": "6",
//			    "usage": "global",
//			    "type": "float",
//			    "name": "LTV"
//			}]
//        }],
//        "values": [{
//            "variableId": "1",
//            "value": "250000"
//        },
//		{
//		    "variableId": "2",
//		    "value": "NewPurchase"
//		},
//		{
//		    "variableId": "3",
//		    "value": "Salaried"
//		},
//		{
//		    "variableId": "4",
//		    "value": "1000000"
//		},
//		{
//		    "variableId": "5",
//		    "value": "1200000"
//		},
//		{
//		    "variableId": "6",
//		    "value": "0.80"
//		}]
//    }
//}
//var jsontest2 = {
//    "decisiontree": {
//        "id": "1",
//        "name": "Credit Category Decision",
//        "tree": {
//            "variables": [{
//                "id": "1",
//                "type": "float",
//                "usage": "input",
//                "name": "HouseHold Income"
//            },
//			{
//			    "id": "2",
//			    "type": "float",
//			    "usage": "input",
//			    "name": "Loan Purpose"
//			},
//			{
//			    "id": "3",
//			    "usage": "input",
//			    "type": "enumeration",
//			    "type_enumeration_values": ["Salaried",
//				"Salaried With Deduction",
//				"Self Employed"],
//			    "name": "Household Income Type"
//			},
//			{
//			    "id": "4",
//			    "usage": "input",
//			    "type": "float",
//			    "name": "Loan Amount"
//			},
//			{
//			    "id": "5",
//			    "usage": "input",
//			    "type": "float",
//			    "name": "Property Value"
//			},
//			{
//			    "id": "6",
//			    "usage": "input",
//			    "type": "float",
//			    "name": "LTV"
//			},
//			{
//			    "id": "7",
//			    "usage": "output",
//			    "type": "string",
//			    "name": "CreditCategory"
//			}],
//            "nodes": [{
//                "id": "1",
//                "name": "Start",
//                "type": "start",
//                "required_variables": ["1"],
//                "output_variables": ["2","3"],
//                "code": "hello there"
//            },
//			{
//			    "id": "2",
//			    "type": "decision",
//			    "name": "Is Salaried",
//			    "required_variables": ["3", "4"],
//			    "output_variables": ["7"],
//			    "code": ""
//			},
//			{
//			    "id": "3",
//			    "type": "decision",
//			    "name": "Is New Purchase",
//			    "required_variables": ["2"],
//			    "output_variables": [],
//			    "code": ""
//			},
//			{
//			    "id": "4",
//			    "type": "decision",
//			    "name": "Is Salaried With Deduction",
//			    "required_variables": ["2"],
//			    "output_variables": [],
//			    "code": ""
//			},
//			{
//			    "id": "5",
//			    "type": "process",
//			    "name": "Check Loan Amount",
//			    "required_variables": ["4"],
//			    "output_variables": [],
//			    "code": " if Variables.Input.LoanAmount >  Variables.Credit.MaximumSalariedNewPurchaseLoanAmount && Variables.Input.LoanAmount < Variables.Credit.MinimumSalariedNewPurchaseLoanAmount then NodeMessages.AddError(Messages.Credit.SalariedNewPurchaseLoanAmountOutsideLimits) end"
//			},
//			{
//			    "id": "6",
//			    "type": "process",
//			    "name": "Check Household Income",
//			    "required_variables": ["4"],
//			    "output_variables": [],
//			    "code": " if Variables.Input.HouseholdIncome >  Variables.Credit.MaximumSalariedNewPurchaseHouseholdIncome && Variables.Input.HouseholdIncome < Variables.Credit.MinimumSalariedNewPurchaseHouseholdIncome then NodeMessages.AddError(Messages.Credit.SalariedNewPurchaseHouseholdIncometOutsideLimits) end"
//			},
//			{
//			    "id": "7",
//			    "type": "process",
//			    "name": "Establish Credit Category",
//			    "required_variables": ["4",
//				"6"],
//			    "output_variables": ["7"],
//			    "code": " if Variables.Input.LTV <  Variables.Credit.Category0SalariedNewPurchaseMaxLTV then Variables.Outputs.CreditCategory = Variables.Credit.Categories.Category0 end"
//			}],
//            "links": [{
//                "id": "1",
//                "type": "decision_yes",
//                "fromNodeId": "1",
//                "toNodeId": "2"
//            },
//			{
//			    "id": "2",
//			    "type": "decision_yes",
//			    "fromNodeId": "2",
//			    "toNodeId": "3"
//			},
//			{
//			    "id": "3",
//			    "type": "decision_no",
//			    "fromNodeId": "2",
//			    "toNodeId": "4"
//			},
//			{
//			    "id": "4",
//			    "type": "decision_yes",
//			    "fromNodeId": "3",
//			    "toNodeId": "5"
//			},
//			{
//			    "id": "5",
//			    "type": "link",
//			    "fromNodeId": "5",
//			    "toNodeId": "6"
//			}]
//        },
//        "layout": {
//            "nodes": [{
//                "id": "1",
//                "loc": "0 90"
//            }],
//            "links": [{
//                "linkId": "1",
//                "fromPort": "L",
//                "toPort": "R",
//                "visible": "true",
//                "points": "",
//                "text": "False"
//            }]
//        }
//    }
//} //send structure

//var msgjson2 = {
//    "messages": {
//        "groups":
//			[{
//			    "name": "Credit",
//			    "messages": [{
//			        "id": "1",
//			        "name": "SalariedNewPurchaseLoanAmountOutsideLimits",
//			        "message": "You do not have enough income for this application."
//			    },
//				{
//				    "id": "2",
//				    "name": "SelfEmployedNewPurchaseLoanAmountOutsideLimits",
//				    "message": "You do not have enough income for this application."
//				}]
//			},
//			{
//			    "name": "General",
//			    "messages": [{
//			        "id": "3",
//			        "name": "SomeMessageName",
//			        "message": "Some Message."
//			    }]
//			}]

//    }
//};