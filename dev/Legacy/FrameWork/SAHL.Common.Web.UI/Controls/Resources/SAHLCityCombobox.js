
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

function getCityResultMatches(event, partialResultText, textboxId, dropdownId)
{
    var key = window.event ? event.keyCode : event.which;
    //38 is the up arrow key, 40 is the down arrow key
    if (key == 38 || key == 40)
    {
	    selectCityLink(dropdownId, textboxId, key == 40);
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
	partialResultText = trimCityAll(partialResultText);
	if (partialResultText.length > 0 && (key >= 32 || key == 8 || key == 0))
    {
        var context = textboxId;
        if(key == 0)
            context = context +"|0";
        GetCityMatchedResults(partialResultText + '|' + provinceKey, context);
    }
	else
    {
		hideCityDropDown(textboxId, dropdownId, false);
    }
	return key;
}

function selectCityLink(dropdownId, textboxId, doNext)
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
		
		var CityInput = document.getElementById(textboxId + '_City');
		CityInput.value = lnkToChange.getAttribute('Value'); 
		objTextBox.value = lnkToChange.getAttribute('City'); 
	}
}

function ReceiveCityServerData(response, context)
{
    var items=response.split(',');
    var showDropDown = context.split('|');
    if(showDropDown != null && showDropDown.length == 1)
	    showCityDropDown(showDropDown[0], showDropDown[0] + '_dropdown', items);
}

function getCityAbsolutePositionOf(element)
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

function showCityDropDown(textboxId, dropDownId, items)
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
			        newLinkText += "<A class='dropdownlist_link_normal' tabindex=-1 style='width:100%;display:block' href='javascript:false;' onMouseDown=\"javascript:dropDownItemCitySelected('" + 
				    textboxId + "', '" + 
				    dropDownId + "', '" + 
				    objTextbox.CityCombobox + "', '" +
				    vals[0] + "', '" +
				    escape(vals[1]) + "');\" Value='" + vals[0]+ "' City='" + vals[1] + "'>" + vals[1] + '</A>';
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
		hideCityDropDown(textboxId, dropDownId, false);
	}
}

function hideCityDropDown(textboxId, dropDownId, MatchClosest)
{
    var objDropDown = document.getElementById(dropDownId);
    if(objDropDown != null && objDropDown.style.visibility != 'hidden')
    {
        if(MatchClosest)
            setClosestCityMatch(textboxId, dropDownId)
	    objDropDown.style.visibility = 'hidden';
	    document.getElementById(dropDownId + '_iframe').style.display = 'none';
	}
}

function dropDownItemCitySelected(textboxId, dropDownId, citydropDownId, CityValue, CityText)
{
	hideCityDropDown(textboxId, dropDownId, false);
	var textbox = document.getElementById(textboxId);
	document.getElementById(textboxId + "_City").value = unescape(CityValue);
	textbox.value = unescape(CityText);
	
    if(typeof(textbox.SuburbCombobox) == "string" && textbox.SuburbCombobox != "")
    {
        var suburbcombobox = document.getElementById(textbox.SuburbCombobox);
        if(suburbcombobox != null)
            suburbcombobox.value = '';
    }
    if(typeof(textbox.PostCodeInput) == "string" && textbox.PostCodeInput != "")
    {
        var postalinput = document.getElementById(textbox.PostCodeInput);
        if(postalinput != null)
            postalinput.value = '';
    }	    
	
	
}

function setClosestCityMatch(textboxId, dropDownId)
{
	var objDropDownDiv = document.getElementById(dropDownId);
	var objTextbox = document.getElementById(textboxId);
	if(objTextbox == null)
	    return;
    var closestMatchCount = 0;
    var closestMatchText = '';
    var closestMatchValue = '';
    var MatchText = objTextbox.value;
    if(MatchText.length > 0)
        MatchText = MatchText.toLowerCase();
	var Citytext = '';
    for (var i = 0; i < document.links.length; i++)
    {
	    var lnk = document.links[i];
	    Citytext = '';
	    if (lnk.parentElement.id == dropDownId)
	    {
	        Citytext = lnk.getAttribute('City').toLowerCase(); 
	        var matchcount = 1;
            while(Citytext.indexOf(MatchText.substring(0, matchcount)) == 0)
            {
                matchcount++;
                if(matchcount > closestMatchCount)
                {
                    closestMatchText = lnk.getAttribute('City');
                    closestMatchValue = lnk.getAttribute('Value');
                    closestMatchCount = matchcount;
                }
                if(matchcount > MatchText.length)
                    break;
            }
	    }
    }	
    
    if(closestMatchCount > 0){
        objTextbox.value = closestMatchText;
        document.getElementById(textboxId + "_City").value = closestMatchValue;
        
        if(typeof(objTextbox.SuburbCombobox) == "string" && objTextbox.SuburbCombobox != "")
	    {
	        var suburbcombobox = document.getElementById(objTextbox.SuburbCombobox);
	        if(suburbcombobox != null)
	            suburbcombobox.value = '';
	    }
        if(typeof(objTextbox.PostCodeInput) == "string" && objTextbox.PostCodeInput != "")
	    {
	        var postalinput = document.getElementById(objTextbox.PostCodeInput);
	        if(postalinput != null)
	            postalinput.value = '';
	    }	    
    }
    else
    {
        objTextbox.value = '';
        document.getElementById(textboxId + "_City").value = '';
    }
}

function OnProvinceChangedForCity(textboxId)
{
    var objTextbox = document.getElementById(textboxId);
    objTextbox.value = '';
    document.getElementById(textboxId + "_City").value = '';
}

function trimCityAll(sString)
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