// centers an element on the screen
function centerElement(element, left, top)
{
    var x = document.body.scrollWidth;
    var y = document.body.scrollHeight;
    if (getBrowser().indexOf('IE') == 0)
    {
        x = document.documentElement.clientWidth;
        y = document.documentElement.clientHeight;
    }
    x = (x / 2) + document.body.scrollLeft - (element.offsetWidth / 2);
    y = (y / 2) + document.body.scrollTop - (element.offsetHeight / 2);
    if (arguments.length < 2) left = true;
    if (left) element.style.left = x + 'px';
    if (arguments.length < 3) top = true;
    if (top) element.style.top = y + 'px';
}

// number formatting function
// copyright Stephen Chapman 24th March 2006, 10th February 2007
// permission to use this function is granted provided
// that this copyright notice is retained intact
// @num - the number to be formatted
// @dec - the number of decimals to display (2 for currencies)
// @thou - the thousand separator character (e.g. a comma)
// @pnt - the decimal character (e.g. a period)
// @curr1 - (optional) the left hand currency symbol for currencies (e.g. R for Rands)
// @curr2 - (optional) the right hand currency symbol (e.g. c for cents)
// @n1 - (optional) additional characters on the left hand side (appear after curr1)
// @n2 - (optional) additional characters on the right hand side (appear before curr2)
function formatNumber(num,dec,thou,pnt,curr1,curr2,n1,n2) 
{
    if (arguments.length < 8) n2 = '';
    if (arguments.length < 7) n1 = '';
    if (arguments.length < 6) curr2 = '';
    if (arguments.length < 5) curr1 = '';
    var neg = (num < 0 ? '-' : '');
    var x = Math.round(num * Math.pow(10,dec));
    if (x >= 0) 
        n1=n2='';
    var y = (''+Math.abs(x)).split('');
    var z = y.length - dec; 
    if (z<0) 
        z--; 
    for (var i = z; i < 0; i++) 
        y.unshift('0');
    y.splice(z, 0, pnt); 
    while (z > 3) 
    {
        z-=3; 
        y.splice(z,0,thou);
    }
    var r = neg + curr1 + n1 +y.join('') +n2 + curr2;
    return r;
}

// gets the absolute position of an element on the screen
function getAbsolutePosition(element) 
{
    var left = 0;
    var top = 0;
    // internet explorer
    if (element.offsetParent) {
        while (element.offsetParent) {
            left += element.offsetLeft
            top += element.offsetTop;
            element = element.offsetParent;
        }
    }
    // other browsers
    else if (element.x) {
        left = element.x;
        top = element.y;
    }
    return { x:left, y:top };
}

// returns all elements on a page, allowing you to pass in multiple tags e.g. getAllElementsByTagName('object', 'div', 'iframe');
function getAllElementsByTag()
{
    var allElements = new Array();
    for (var i=0; i<arguments.length; i++)
    {
        var tagName = arguments[i];
        var elements = document.body.getElementsByTagName(tagName);
        for (var j=0; j<elements.length; j++)
        {
            allElements[allElements.length] = elements[j];
        }
    }
    return allElements;
}

// method to determine the browser for browser-specific javascript.  
// Currently returns a string with possible values IE6, IE7, Firefox, Other
function getBrowser()
{
    if (navigator.appName == 'Microsoft Internet Explorer')
    {
        return 'IE' + parseFloat((new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})")).exec(navigator.userAgent)[1]);
    }
    else if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1)
    {
        return 'Firefox';
    }
        
    return 'Other';
}

// gets the value of a cookie
function getCookie(sName)
{
    var aCookie = document.cookie.split("; ");
    for (var i=0; i < aCookie.length; i++)
    {
        var aCrumb = aCookie[i].split("=");
        if (sName == aCrumb[0])
            return unescape(aCrumb[1]);
    }
    return null;
}

// cross-browser way of getting the event source
// @evt - the event object for Firefox
function getEventSource(evt)
{
	return (window.event ? window.event.srcElement : evt.target);
}

// hides all elements with a specified tag name
function hideAllElementsByTag(tagName)
{
    var elements = document.body.getElementsByTagName(tagName);
    for (var i=0; i<elements.length; i++)
    {
        elements[i].style.display = 'none';
    }
}

// hides the current tooltip
function hideToolTip(evt)
{
	var div = document.getElementById('SAHLToolTip');
	div.style.display = 'none';
	
	// remove the event listener if there is an event source
	var parent = getEventSource(evt);
	if (parent == null) return;
	
	if (window.removeEventListener)
		parent.removeEventListener('mouseout', hideToolTip, false);
	else if (window.detachEvent)
		parent.detachEvent('onmouseout', hideToolTip);
}

// limits the amount of text in a textarea
// @field - the multi-line text box
// @maxLimit - the maximum number of characters allowed
// @countField - (optional) the ID of a label element - this will be populated with HTML stating how many characters have been entered
function limitTextLength(field, maxLimit, countFieldId)
{
    if (field.value.length > maxLimit)
        field.value = field.value.substring(0, maxLimit);
        
    if (arguments.length > 2)
    {
        var countField = document.getElementById(countFieldId);
        if (countField != null)
		    countField.innerHTML = "<b>" + field.value.length + "</b> of <b>" + maxLimit + "</b> characters";
        
    }    
}

// cross-browser-friendly function for registering events
function registerEvent(element, eventName, method)
{
    if (window.addEventListener)
        element.addEventListener(eventName, method, false);
    else if (window.attachEvent)
        element.attachEvent('on' + eventName, method);
}

// cross-browser-friendly function for firing events
function fireEvent(element, eventName) {
    if (document.createEventObject) {
        // dispatch for IE
        var evt = document.createEventObject();
        return element.fireEvent('on' + eventName) //, evt)
    }
    else {
        // dispatch for firefox + others
        var evt = document.createEvent("HTMLEvents");
        evt.initEvent(eventName, true, true); // event type,bubbling,cancelable
        return !element.dispatchEvent(evt);
    }
}

// set the value of a cookie
function setCookie(sName, sValue)
{
    // No expiration, so it will expire when the browser closes.
    document.cookie = sName + "=" + escape(sValue);
}

// cross-browser-friendly function for setting the opacity on an element
function setOpacity(element, percentage)
{
    element.style.filter = 'alpha(opacity=' + percentage.toString() + ')';   // IE 
    element.style.MozOpacity = (percentage / 100);                           // Mozilla
    element.style.opacity = (percentage / 100);                              // Opera
}

// prevents an event from bubbling up (cross-browser)
function stopEventBubble(e) 
{
    if (!e) e = window.event;
    if (!e) return;
    if (e.stopPropagation) 
        e.stopPropagation();
    else 
        e.cancelBubble = true;
}

String.prototype.cleanForJavaScript = function()
{
    var str = this;
    str = str.replace(/\\/g, "\\\\")
    str = str.replace(/'/g, "\\'")
    return str;
}

// remove leading whitespace
String.prototype.ltrim = function() 
{
    var str = this;
    while (str.length > 0) 
    {
        var ch = str.substr(0,1);
        if ((ch == ' ') || (ch == '\n') || (ch == '\r') || (ch == '\t') || (ch == '\f'))
            str = str.substring(1);
        else
            break;
    }
    return str;
}

// replace all instances of a string within another string - for caseSensitive replacements set caseSensitive to true (this is an optional parameter)
String.prototype.replaceAll = function(find, replace, caseSensitive) 
{
    var str = this;
    var re = find;
    if (arguments.length < 3 || (!arguments[2]))
        re = find.toLowerCase() + '|' + find.toUpperCase();
    return str.replace(new RegExp(re, "g"), replace);
}

// remove trailing whitespace
String.prototype.rtrim = function() 
{
    var str = this;
    while (str.length > 0) {
        var ch = str.substr(str.length - 1, 1);
        if ((ch == ' ') || (ch == '\n') || (ch == '\r') || (ch == '\t') || (ch == '\f'))
            str = str.substring(0, str.length - 1);
        else
            break;
    }
    return str;
}


// trims leading/trailing spaces off a string
String.prototype.trim = function(ch) 
{
    if (arguments.length == 0)
        return this.replace(/^\s*|\s*$/g,"");
    else
        return this.replace(new RegExp("^" + ch + "+|" + ch + "+$", "g"), "");
}
