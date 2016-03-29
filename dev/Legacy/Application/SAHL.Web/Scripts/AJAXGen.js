///
/// General AJAX Functionality 
/// Author : Donald Massyb
/// Company : BB&D
/// Date : 15/08/2006

var ajaxHttp = createRequestObject();
var ajaxRequest = createXMLDocument();

function createXMLDocument()
{
    var ro;
    var browser = navigator.appName;
    if(browser == "Microsoft Internet Explorer"){
        ro = new ActiveXObject("Microsoft.XMLDOM");
    }else{
        ro = new XMLDocument();
    }
    return ro;
}

function createRequestObject() {
    var ro;
    var browser = navigator.appName;
    if(browser == "Microsoft Internet Explorer"){
        ro = new ActiveXObject("Microsoft.XMLHTTP");
    }else{
        ro = new XMLHttpRequest();
    }
    return ro;
}

// Ajax Request XML must conform to the documented standard
function postAjaxRequest() {
    
    ajaxHttp.open('post', '../AjaxHandler.ashx');
    ajaxHttp.onreadystatechange = handleResponse;
    ajaxHttp.send(ajaxRequest);
}

function handleResponse() {
    if(ajaxHttp.readyState == 4){
        // the response XML must conform to the documented standard
        var xmlResponse = createXMLDocument();
        xmlResponse.loadXML(ajaxHttp.responseText);

        root = xmlResponse.documentElement;
        oNodeList = root.childNodes;
        for (var i=0; i<oNodeList.length; i++) {
            Item = oNodeList.item(i);
            var nodeType = Item.nodeName;
            
            switch (nodeType)
            {
                // Item.attributes(0).value always contains the ID of the element to be populated
                case "textfield": // the return node contains a name and value attribute with the name of the control and value to set it to
                    var tName = Item.attributes(0).value;
                    var tValue="";
                    if (Item.childNodes.length > 0)
                        tValue = Item.childNodes(0).nodeValue;
                    
                    document.getElementById(tName).innerText = tValue;
                    break;
                case "list": // contains listitem subitems which must be added to the list view
                    var tName = Item.attributes(0).value;
                    var listCtl = document.getElementById(tName);
                    
                    while (listCtl.options.length > 0)
                        listCtl.options.remove(0);
                        
                    var tValue="";
                    for (var i =0; i < Item.childNodes.length; ++i){
                        tValue = Item.childNodes(i);
                        var oOption = document.createElement("OPTION");
                        
                        listCtl.options.add(oOption);
                        oOption.innerText = tValue.text;
                        oOption.value = tValue.attributes(0).text;
                    }
                    break;
                case "table":
                    var tName = Item.attributes(0).value;
                    var tableCtl = document.getElementById(tName);

                    while (tableCtl.rows.length > 0)
                        listCtl.rows.remove(0);
                        
                    var tValue="";
                    for (var i =0; i < Item.childNodes.length; ++i){
                        tValue = Item.childNodes(i);
                        // todo, add the TR's to the table 
                    }
                    break;
                
                    // the response contains <tr> elements that must be added to the table
            }
        }
    }
}