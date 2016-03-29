var masterPageTimeout;                  // page timeout in minutes
var masterPageTimer = null;             // timer object for updating status
var masterPreventBeforeUnload = false;
var masterHiddenObjects = new Array();

// cross-browser-friendly function for registering events - this is a copy of the one in Default.js but we need
// it to be here so we're absolutely certain it exists - this MUST be the first function declared here!
function _masterRegisterEvent(element, eventName, method)
{
    if (window.addEventListener)
        element.addEventListener(eventName, method, false);
    else if (window.attachEvent)
        element.attachEvent('on' + eventName, method);
}    

function masterBeforeUnload(e)
{
    // initial check is done to see if we haven't prevented the unload div being shown
    if (masterPreventBeforeUnload)
    {
        masterPreventBeforeUnload = false;
        return;
    }
    
    // hide any drop down boxes
    if (getBrowser().indexOf('IE6') == 0)
    {
        hideAllElementsByTag('select');
    }
    
    // hide any iframes and objects as these tend to sit on top
    hideAllElementsByTag('object');
    hideAllElementsByTag('iframe');
    
    masterResetUnloadControls(e);
    
    // add a window event so if the window resizes, the divs are realigned
    _masterRegisterEvent(window, 'resize', masterResetUnloadControls);
    return "";
}
_masterRegisterEvent(window, 'beforeunload', masterBeforeUnload);

// this function can be called from JavaScript functions to prevent the unload elements from displaying
function masterCancelBeforeUnload()
{
    var div = document.getElementById('masterUnloading');
    
    // if the div is hidden, the order of events mean the event handler will still be called - rather
    // set the flag, but if it is not null and visible then just hide it and forget about it
    if (div != null && div.style.display != 'none')
        div.style.display = 'none';
    else
        masterPreventBeforeUnload = true; 
}

function masterDisplayModalBackground()
{
    var div = document.getElementById("masterModalBackground");
    
    // work out the width and height
    if (window.innerWidth)
    {
        w = window.innerWidth; 
        h = window.innerHeight; 
    }
    else
    {
        w = document.documentElement.clientWidth + 40;
        h = document.documentElement.clientHeight + 40;
        document.documentElement.style.overflow = 'hidden';
    }
    div.style.width = parseInt(w) + 'px';
    div.style.height = parseInt(h) + 'px';
    div.style.display = 'inline';
}

function masterLayout()
{
    // set the width of the menu
    var menuWidth = document.getElementById('hidMenuWidth').value;
    var masterLeft = document.getElementById('masterLeft');
    masterLeft.style.width = menuWidth + 'px';
}

// registers timeout events - this fires on document load
function masterLoad(e)
{
    masterLayout();
   
    // initialise the page timeout and register methods to reset the timeout
    masterPageTimeout = document.getElementById('hidPageTimeout').value;
    masterUpdateTimeout();
    masterPageTimer = setTimeout('masterUpdateTimeout()', 60000);
    masterSetPingTransparency();
}  
_masterRegisterEvent(window, 'load', masterLoad);

// does an ajax call to update the user settings when a tab is clicked
function masterMenuActiveTabChanged(sender, e) 
{
    var hidNodeSet = document.getElementById('hidNodeSet');
    hidNodeSet.value = sender.get_activeTabIndex().toString();
    __doPostBack('hidNodeSet', '');
    
}

// callback for masterMenuActiveTabChanged - 
function masterMenuActiveTabChangedCallBack(result) 
{
    window.location.href = document.getElementById('hidVirtualRoot').value + 'Utils/Redirect.aspx';
}

// updates the hidden navigate value so views can determine whether to run page life cycle methods on post back
function masterNavigate()
{
    document.forms[0].navigate.value = "1";
}

// creates/realigns the controls shown when the page is unloading
function masterResetUnloadControls(e)
{
    masterDisplayModalBackground();
    var divUnloading = document.getElementById('masterUnloading');
    divUnloading.style.display = 'inline';
    centerElement(divUnloading);
}

function masterResize(e)
{
    masterLayout();
}
_masterRegisterEvent(window, 'resize', masterResize);

// this function is a work-around to prevent the AjaxToolKit modal popup from displaying scrollbars in IE6
function masterSetBodyScrolling(enabled)
{
    if (enabled)
        document.documentElement.style.overflow = 'auto';
    else
        document.documentElement.style.overflow = 'hidden';
}

// adds a transparency filter to pings in IE6
function masterSetPingTransparency() 
{
    if (getBrowser() == 'IE6')
    {
	    for (var i = document.images.length - 1, img = null; (img = document.images[i]); i--) {
		    if (img.src.match(/\.png$/i) != null) 
		    {
			    var src = img.src;
			    var div = document.createElement("DIV");
			    div.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + src + "', sizing='scale')"
			    div.style.width = img.width + "px";
			    div.style.height = img.height + "px";
			    img.replaceNode(div);
		    }
		    // img.style.visibility = "visible";
	    }
	}
}

// displays the info message box - this will hide all 'iframe' and 'object'
function masterToggleStatusInfo(show)
{
    var divPopup = document.getElementById('masterStatusInfoPopup');

    if (show)
    {
        masterHiddenObjects = new Array();
        var objs = getAllElementsByTag('iframe', 'object', 'select');
        for (var i=0; i<objs.length; i++)
        {
            if (objs[i].style.display != 'none')
            {
                var subArray = new Array();
                masterHiddenObjects[masterHiddenObjects.length] = subArray;
                subArray[0] = objs[i];
                subArray[1] = objs[i].style.display;
                objs[i].style.display = 'none';
            }
        }
        
        // create the background div and display it    
        masterSetBodyScrolling(false);
        masterDisplayModalBackground();
        
        // display the info div
        divPopup.style.display = 'inline';
        centerElement(divPopup);
    }
    else
    {
        var bg = document.getElementById("masterModalBackground");
        bg.style.display = 'none';

        // reset objects that were hidden        
        for (var i=0; i<masterHiddenObjects.length; i++)
            masterHiddenObjects[i][0].style.display = masterHiddenObjects[i][1];
            
        masterSetBodyScrolling(true);
        divPopup.style.display = 'none';
        masterHiddenObjects = null;
    }
}

// handles page timeout
function masterUpdateTimeout() 
{
    clearTimeout(masterPageTimer);
    if (masterPageTimeout == 0)
    {
        window.location.replace(document.getElementById('hidTimeoutUrl').value);
        return;
    }
    else
    {
        var statusLabel = document.getElementById('masterStatusInfo').getElementsByTagName('span')[0];
        statusLabel.innerHTML = '<strong>Status</strong>: Timeout in ' + masterPageTimeout.toString() + ' minutes. &nbsp;&nbsp;'
        masterPageTimeout--;
        masterPageTimer = setTimeout('masterUpdateTimeout()', 60000);
    }
}
