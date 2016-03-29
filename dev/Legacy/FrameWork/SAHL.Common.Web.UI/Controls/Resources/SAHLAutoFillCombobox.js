
// These two variables contain the ID's of the two controls that receive the selected values when the 
// user selects a value from the drop down list
// There values are set when the callback is called from the server, the first two arguments are always these two fields

var InputControlID=""; // ID of the control that fires the callback
var SubmitButtonID=""; // ID of the submit button to click when an item is selected

function checkForEnterSelect(dropDownId)
{
    var key = window.event ? event.keyCode : event.which;
    
    if (key == 13)	
    { 
        var objDropDown = document.getElementById(dropDownId);
        if (objDropDown.style.visibility == 'visible'){
            hideDropDown();
            event.keyCode = 0;
            
            var objInputControl = document.getElementById(InputControlID);
            if (objInputControl != null)
            {
                var szPostBackEvent = objInputControl.postbackEvent;
                
                if (objInputControl.postbackEvent != 'undefined')
                    setTimeout(objInputControl.postbackEvent + ";", 0);
            }
            
            return;	
        }

    }
}

// called by the server on successful completion of the callback
function ReceiveServerData(response, context)
{
    // the context the name of the Input text control and optional Submit button
    
    InputControlID = context;
    
    var items=response.split('!');
    if (items.length > 0)
	    showDropDown(items);
	else 
	    hideDropDown();
}

function getAbsolutePositionOf(element)
{
	var offsetX = 0;
	var offsetY = 0;
	while (element != null)
	{
		offsetX += element.offsetLeft;
		offsetY += element.offsetTop;
		element = element.offsetParent;
	}
	return new Array(offsetX,offsetY);
}


function hideDropDown()
{
    if ( InputControlID != "" )
    {
        var dropDownId = InputControlID + "_dropdown";
	    document.getElementById(dropDownId).style.visibility = 'hidden';
	    document.getElementById(dropDownId + '_iframe').style.display = 'none';
	}
}

function dropDownItemSelected(value, name)
{
    if (InputControlID != "")
    {
        var objInputControl = document.getElementById(InputControlID);
        objInputControl.value = "";
	    document.getElementById(objInputControl.TargetTextControl).value = unescape(name);
	    document.getElementById(objInputControl.TargetIDControl).value = unescape(value);
	    
        var objInputControl = document.getElementById(InputControlID);
        if (objInputControl != null)
        {
            var szPostBackEvent = objInputControl.postbackEvent;
            
            if (objInputControl.postbackEvent != 'undefined')
                setTimeout(objInputControl.postbackEvent + ";", 0);
        }
	}
}


function selectLink(doNext)
{
    if (InputControlID !="")
    {
        var dropDownId = InputControlID + "_dropdown"; 
	    var objDropDown = document.getElementById(dropDownId);
	    var objInputControl = document.getElementById(InputControlID);
    	
	    if (objDropDown.style.visibility == 'visible')
	    {
		    var indexOfCurrentLink = -1;
		    var indexOfFirstLink = -1;
		    var indexOfLastLink = -1;
		    for (i = 0; i < document.links.length; i++)
		    {
			    var lnk = document.links[i];
			    if (lnk.parentElement.id == dropDownId)
			    {
				    if (indexOfFirstLink == -1)
                                indexOfFirstLink = i
				    indexOfLastLink = i;
				    if (lnk.className == 'dropdownlist_link_current')
					    indexOfCurrentLink = i;
				    lnk.className = 'dropdownlist_link_normal';
			    }
		    }
	       var lnkToChange = null;
           if (doNext)
		    {
			    if (indexOfCurrentLink == -1)
				    lnkToChange = document.links[indexOfFirstLink];
			    else if (indexOfCurrentLink == indexOfLastLink)
				    lnkToChange = document.links[indexOfLastLink];
               else
				    lnkToChange = document.links[indexOfCurrentLink + 1];
		    }
		    else
		    {
			    if (indexOfCurrentLink == -1)
				    lnkToChange = document.links[indexOfLastLink];
			    else if (indexOfCurrentLink == indexOfFirstLink)
				    lnkToChange = document.links[indexOfFirstLink];
               else
				    lnkToChange = document.links[indexOfCurrentLink - 1];
		    }

		    lnkToChange.className = 'dropdownlist_link_current';
    		
    		var vals = lnkToChange.Value.split('|');
    		
    		if (vals.length > 1)
    		{
	            document.getElementById(objInputControl.TargetIDControl).value = unescape(vals[0]);
	            document.getElementById(objInputControl.TargetTextControl).value = unescape(vals[1]);
	        }
	        else
	            document.getElementById(objInputControl.TargetTextControl).value = unescape(vals[0]);
	        
		}
	}
}

function showDropDown(items)
{
    if (InputControlID != "")  
    {
        var dropDownId = InputControlID + "_dropdown";
        var objTextbox = document.getElementById(InputControlID);
	    var objDropDownDiv = document.getElementById(dropDownId);
	    var objIframe = document.getElementById(dropDownId + '_iframe');
	    
	    if ((items != null) && (items.length > 0))
	    {
		    newLinkText = '';
		    for (var i = 0; i < items.length; i++)
		    {
		    
		        var vals = items[i].split('|');
			    newLinkText += "<A class='dropdownlist_link_normal'";
			    newLinkText += " Value=\"" + items[i] + "\"";
			    newLinkText += " tabindex=-1";
			    newLinkText += " style='width:100%'";
			    newLinkText += " href='javascript:false;'";
			    
			    newLinkText += " onMouseDown=\"javascript:dropDownItemSelected(";
			    if (vals.length == 1)
			    {
			        newLinkText += "'" + escape(vals[0]) + "',";
			        newLinkText += "'" + escape(vals[0]) + "');\">";
    			    newLinkText += vals[0] + "</A><br>";
    			}
			    else
			    {
			        newLinkText += "'" + escape(vals[0]) + "',";
			        newLinkText += "'" + escape(vals[1]) + "');\">";
    			    newLinkText += vals[1] + "</A><br>";
			    }
			        
		    }
		    objDropDownDiv.innerHTML = newLinkText;
		    
		    var position = getAbsolutePositionOf(objTextbox);
		    
		    objDropDownDiv.style.left = position[0];
		    objDropDownDiv.style.width = objTextbox.offsetWidth;
		    objDropDownDiv.style.visibility = 'visible';
            if (objDropDownDiv.Position == 'bottom')
		        objDropDownDiv.style.top = position[1] + objTextbox.offsetHeight;
		    else
		        objDropDownDiv.style.top = position[1] - objDropDownDiv.offsetHeight;
		        
            objIframe.style.left = objDropDownDiv.style.left;
		    objIframe.style.top = objDropDownDiv.style.top;
		    objIframe.style.width = objTextbox.offsetWidth;
		    objIframe.style.height = objDropDownDiv.offsetHeight;
		    objIframe.style.display = 'block';
	    }
	    else
	    {
		    hideDropDown();
	    }
	}
}

