function checkForEnterSelect(dropDownId)
{
    var key = window.event ? event.keyCode : event.which;
    
    if (key == 13)	
    { 
        var objDropDown = document.getElementById(dropDownId);
        if (objDropDown.style.visibility == 'visible'){
            hideDropDown();
            event.keyCode = 0;
            return;	
        }
    }
}

function getSuburbResultMatches(event, partialResultText, textboxId, dropdownId)
{
	var key = window.event ? event.keyCode : event.which;
	//38 is the up arrow key, 40 is the down arrow key
	if (key == 38 || key == 40)
	{
		selectSuburbLink(dropdownId, textboxId, key == 40);
		return;
	}
	// get the currently selected provincekey if there is one
	var provinceCombobox;
	var provinceKey;
	var textbox = document.getElementById(textboxId);
	if (typeof(textbox.ProvinceCombobox) == "string")
	    provinceCombobox = document.getElementById(textbox.ProvinceCombobox);
	if(provinceCombobox != null)
	    provinceKey = provinceCombobox.value;
	if(provinceKey == null || provinceKey == "")
	    provinceKey = "-1";
	
	//show the dropdown list on alphanumeric keys, delete, and backspace
	partialResultText = trimSuburbAll(partialResultText);
	if (partialResultText.length > 0 && (key >= 32 || key == 8 || key == 0))
    {
        var context = textboxId;
        if(key == 0)
            context = context +"|0";
        GetSuburbMatchedResults(partialResultText + '|' + provinceKey, context);
    }
	else
    {
		hideSuburbDropDown(textboxId, dropdownId, false);
    }
	return key;
}

function selectSuburbLink(dropdownId, textboxId, doNext)
{
	var objDropDown = document.getElementById(dropdownId);
	var objTextBox = document.getElementById(textboxId);
	if (objDropDown.style.visibility == 'visible')
	{
		var indexOfCurrentLink = -1;
		var indexOfFirstLink = -1;
		var indexOfLastLink = -1;
		for (var i = 0; i < document.links.length; i++)
		{
			var lnk = document.links[i];
			if (lnk.parentElement.id == dropdownId)
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
		
		objTextBox.value = lnkToChange.getAttribute('Suburb'); 
		
		var PostCodeInput = document.getElementById(objTextBox.getAttribute('PostCodeInput'));
		PostCodeInput.value = lnkToChange.getAttribute('PostCode'); 
		var CityCombobox = document.getElementById(objTextBox.getAttribute('CityCombobox'));
		CityCombobox.value = lnkToChange.getAttribute('City'); 
        var SuburbCode = document.getElementById(textboxId + '_Suburb');    
	    SuburbCode.value = lnkToChange.getAttribute('Value');
	}
}

function ReceiveSuburbServerData(response, context)
{
    var items=response.split(',');
        var showDropDown = context.split('|');
    if(showDropDown != null && showDropDown.length == 1)
	    showSuburbDropDown(showDropDown[0], showDropDown[0] + '_dropdown', items);
}

function getSuburbAbsolutePositionOf(element)
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

function showSuburbDropDown(textboxId, dropDownId, items)
{
	var objDropDownDiv = document.getElementById(dropDownId);
	var objTextbox = document.getElementById(textboxId);
	var objIframe = document.getElementById(dropDownId + '_iframe');
	if (items != null && items.length > 0 && !(items.length == 1 && items[0] == objTextbox.value))
	{
		newLinkText = '';
		for (var i = 0; i < items.length; i++)
		{
		    if(items[i] != "")
		    {
		        var vals = items[i].split('|');
		        if(vals != null && vals.length > 0)
		        {
			        newLinkText += "<A class='dropdownlist_link_normal' tabindex=-1 style='width:100%;display:block' href='javascript:false;' onMouseDown=\"javascript:dropDownItemSuburbSelected('" + 
				    textboxId + "', '" + 
				    dropDownId + "', '" + 
				    objTextbox.CityCombobox + "', '" +
				    vals[0] + "', '" +
				    escape(vals[1]) + "', '" +
				    vals[2] + "', '" +
				    vals[3] + "', '" +
				    vals[4] + "');\" Value='" + vals[0]+ "' Suburb='" + vals[1] + "' CityValue ='" + vals[2] + "' City='" + vals[3] + "' PostCode='" + vals[4] + "'>" + vals[1] + ' <span class=\"suburbcity\">(' + vals[3] + ')</span></A>';
			    }
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
		hideSuburbDropDown(textboxId, dropDownId, false);
	}
}

function hideSuburbDropDown(textboxId, dropDownId, MatchClosest)
{
    var objDropDown = document.getElementById(dropDownId);
    if(objDropDown != null && objDropDown.style.visibility != 'hidden')
    {
        if(MatchClosest)
            setClosestSuburbMatch(textboxId, dropDownId)
	    objDropDown.style.visibility = 'hidden';
	    document.getElementById(dropDownId + '_iframe').style.display = 'none';
	}
}

function dropDownItemSuburbSelected(textboxId, dropDownId, citydropDownId, suburbValue, suburbText, cityValue, cityText, postCode)
{
	hideSuburbDropDown(textboxId, dropDownId, false);
	var textbox = document.getElementById(textboxId);
	document.getElementById(textboxId + "_Suburb").value = unescape(suburbValue);
	textbox.value = unescape(suburbText);
	
    var PostCodeInput = document.getElementById(textbox.getAttribute('PostCodeInput'));
    PostCodeInput.value = postCode; 
    var CityCombobox = document.getElementById(textbox.getAttribute('CityCombobox'));
    CityCombobox.value = unescape(cityText); 
	
}

function setClosestSuburbMatch(textboxId, dropDownId)
{
	var objDropDownDiv = document.getElementById(dropDownId);
	var objTextbox = document.getElementById(textboxId);
	if(objTextbox == null)
	    return;
    var closestMatchCount = 0;
    var closestMatchText = '';
    var closestMatchValue = '';
    var cityText = '';
    var cityValue = '';
    var postcode = '';
    var MatchText = objTextbox.value;
    if(MatchText.length > 0)
        MatchText = MatchText.toLowerCase();
	var suburbtext = '';
    for (var i = 0; i < document.links.length; i++)
    {
	    var lnk = document.links[i];
	    suburbtext = '';
	    if (lnk.parentElement.id == dropDownId)
	    {
	        suburbtext = lnk.getAttribute('Suburb').toLowerCase(); 
	        var matchcount = 1;
            while(suburbtext.indexOf(MatchText.substring(0, matchcount)) == 0)
            {
                matchcount++;
                if(matchcount > closestMatchCount)
                {
                    closestMatchText = lnk.getAttribute('Suburb');
                    closestMatchValue = lnk.getAttribute('Value');
                    cityValue = lnk.getAttribute('CityValue');
                    cityText = lnk.getAttribute('City');
                    postcode = lnk.getAttribute('PostCode');
                    closestMatchCount = matchcount;
                }
                if(matchcount > MatchText.length)
                    break;
            }
	    }
    }	
    
    if(closestMatchCount > 0)
    {
        objTextbox.value = closestMatchText;
        document.getElementById(textboxId + "_Suburb").value = closestMatchValue;
        
       	if (typeof(objTextbox.CityCombobox) == "string" && objTextbox.CityCombobox != "")
	    {
	        var citycombo = document.getElementById(objTextbox.CityCombobox);
	        var city = document.getElementById(objTextbox.CityCombobox + "_City");
	        if(citycombo != null)
	        {
	            citycombo.value = cityText;
	            city.value = cityValue;
	        }
	    }  
	    if(typeof(objTextbox.PostCodeInput) == "string" && objTextbox.PostCodeInput != "")
	    {
	        var postcodeinput = document.getElementById(objTextbox.PostCodeInput);
	        if(postcodeinput != null)
	            postcodeinput.value = postcode;
	    }
    }
    else
    {
        objTextbox.value = '';
        document.getElementById(textboxId + "_Suburb").value = '';
    }
    
  
}

function OnProvinceChangedForSuburb(textboxId)
{
    var objTextbox = document.getElementById(textboxId);
    objTextbox.value = '';
    document.getElementById(textboxId + "_Suburb").value = '';
    if(typeof(objTextbox.PostCodeInput) == "string" && objTextbox.PostCodeInput != "")
    {
        var postcodeinput = document.getElementById(objTextbox.PostCodeInput);
        if(postcodeinput != null)
            postcodeinput.value = '';
    }
    
}


function trimSuburbAll(sString)
{
    while (sString.substring(0,1) == ' ')
    {
    sString = sString.substring(1, sString.length);
    }
    while (sString.substring(sString.length-1, sString.length) == ' ')
    {
    sString = sString.substring(0,sString.length-1);
    }
    return sString;
}