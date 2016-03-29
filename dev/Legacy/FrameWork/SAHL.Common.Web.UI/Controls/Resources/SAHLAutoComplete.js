var SAHLAutoComplete_arrTextBoxes = new Array();
var SAHLAutoComplete_arrAutoComplete = new Array();
var SAHLAutoComplete_current = null;

// event raised when the textbox loses focus
function SAHLAutoComplete_blur(textBox) 
{
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    autoComplete.handleBlur();
}

// callback function for autocomplete requests - this marshalls the result back to the appropriate SAHLAutoComplete object
function SAHLAutoComplete_callBack(result)
{
    SAHLAutoComplete_current.handleCallBack(result);
    SAHLAutoComplete_current = null;
}

// helper function to locate an autocomplete object for a text box (returns null if one does not exist yet)
function SAHLAutoComplete_findAutoComplete(textBox)
{
    for (var i=0; i<SAHLAutoComplete_arrTextBoxes.length; i++)
    {
        if (SAHLAutoComplete_arrTextBoxes[i] == textBox)
            return SAHLAutoComplete_arrAutoComplete[i];
    }
    return null;
}

// event that occurs when a textbox requiring autocomplete items gets focus - this will create a new SAHLAutoComplete 
// object if one has not already been created for the textbox
// @textBox - the textbox receiving the autocomplete data
// @hiddenInputId - the hidden control associated with the SAHLAutoComplete control
// @serviceMethod - the web service method called via AJAX
// @verticalPosition - if 0, the items will be shown below the text box, else they will be shown above it
// @horizontalPosition - if 0, the items will line up with the left hand side of the text box, otherwise to the right hand side
// @maxRowCount - the maximum number of rows that can be displayed (set to -1 for no limit)
// @minCharacters - the minimum number of characters that must be entered before the AJAX call is made
// @cssClass - the css class to apply to the AutoComplete box
// @cssClassItem - the css class to apply to each item in the box
// @cssClassItemSelected - the css class to apply to the selected item in the box
// @parentControls - a pipe-delimited string of control IDs that this control depends on
// @postBackFunction - the .NET postback function for the control (empty if AutoPostBack=false)
// @clientClickFunction - a JavaScript function to be called when an item is clicked (default is "" - no JavaScript function is called)
function SAHLAutoComplete_focus(textBox, hiddenInputId, serviceMethod, verticalPosition, horizontalPosition, maxRowCount,
    minCharacters, cssClass, cssClassItem, cssClassItemSelected,
    parentControls, postBackFunction, clientClickFunction) 
{
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    if (autoComplete == null)
    {
        // create the new SAHLAutoComplete client-side object
        autoComplete = new SAHLAutoComplete(textBox);
        autoComplete.hiddenInput = document.getElementById(hiddenInputId);
        autoComplete.serviceMethod = serviceMethod;
        autoComplete.verticalPosition = verticalPosition;
        autoComplete.horizontalPosition = horizontalPosition;
        autoComplete.maxRowCount = maxRowCount;
        autoComplete.minCharacters = parseInt(minCharacters);
        autoComplete.cssClass = cssClass;
        autoComplete.cssClassItem = cssClassItem;
        autoComplete.cssClassItemSelected = cssClassItemSelected;
        autoComplete.postBackFunction = postBackFunction;
        autoComplete.clientClickFunction = clientClickFunction;
        
        if (parentControls.length > 0)
        {
            var arrControls = new Array();
            var arrControlIds = parentControls.split('|');
            for (var i=0; i<arrControlIds.length; i++)
            {
                arrControls[arrControls.length] = document.getElementById(arrControlIds[i]);
            }        
            autoComplete.parentControls = arrControls;
        }
        
        // add the new object to the global array
        SAHLAutoComplete_arrTextBoxes[SAHLAutoComplete_arrTextBoxes.length] = textBox;
        SAHLAutoComplete_arrAutoComplete[SAHLAutoComplete_arrAutoComplete.length] = autoComplete;
    }
    autoComplete.handleFocus();
}

// catches keyDown events on the textbox - this is only used to catch the enter key - all 
// other key presses are handled in SAHLAutoComplete
function SAHLAutoComplete_keyDown(e) 
{
    e = (e ? e : window.event);
    var textBox = e.target || e.srcElement;
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    return autoComplete.handleKeyDown(e);
}

// event that is raised when a key is pressed in the associated text box
function SAHLAutoComplete_keyUp(e) 
{
    e = (e ? e : window.event);
    var textBox = e.target || e.srcElement;
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    return autoComplete.handleKeyUp(e);
}

// event raised when the mouse button is held down over an item in the control
function SAHLAutoComplete_mouseDown(textBoxID, itemIndex) 
{
    var textBox = document.getElementById(textBoxID);
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    autoComplete.handleMouseDown(itemIndex);
}

// event raised when the mouse leaves an item in the control
function SAHLAutoComplete_mouseOut(element, textBoxID) 
{
    var textBox = document.getElementById(textBoxID);
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    autoComplete.handleMouseOut(element);
}

// event raised when the mouse enters an item in the control
function SAHLAutoComplete_mouseOver(element, textBoxID, itemIndex) 
{
    var textBox = document.getElementById(textBoxID);
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    autoComplete.handleMouseOver(element, itemIndex);
}

// tells an SAHLAutoComplete to retrieve data
function SAHLAutoComplete_retrieveData(textBoxID)
{
    var textBox = document.getElementById(textBoxID);
    var autoComplete = SAHLAutoComplete_findAutoComplete(textBox);
    SAHLAutoComplete_current = autoComplete;
    autoComplete.retrieveData();
}

// Constructor for autocomplete class
// @textBox - the textbox that shows the autocomplete list
function SAHLAutoComplete(textBox) 
{
    var divId = 'SAHLAutoCompleteDiv';
    var iFrameId = divId + '_iframe';
    
    // private properties
    this._busy = false;
    this._currentIndex = -1;
    this._div = document.getElementById(divId);
    this._errorOccurred = false;
    this._iframe = document.getElementById(iFrameId);
    this._items;
    this._timer = null;
    
    // public properties
    this.clientClickFunction = '';
    this.cssClass = '';
    this.cssClassItem = '';
    this.cssClassItemSelected = '';
    this.hiddenInput = null;
    this.horizontalPosition = 0;
    this.maxRowCount = -1;
    this.minCharacters = 0;
    this.parentControls = new Array();
    this.postBackFunction = '';
    this.selectedItem = null;
    this.serviceMethod = '';
    this.textBox = textBox;
    this.textBoxID = this.textBox.id;
    this.verticalPosition = 0;
    
    // create a div to use if there isn't one already
    if (this._div == null)
    {
        this._div = document.createElement("DIV");
        this._div.id = divId;
        this._div.style.display = 'none';
        this._div.style.position = 'absolute';
        this._div.style.zIndex = 5;
        document.body.appendChild(this._div);
        
        // create an iframe too, thanks to IE display bug with drop down boxes
        this._iframe = document.createElement('IFRAME');
        this._iframe.id = iFrameId;
        this._iframe.style.display = 'none';
        this._iframe.style.position = 'absolute';
        this._iframe.style.zIndex = 4;
        document.body.appendChild(this._iframe);
    }
}

SAHLAutoComplete.prototype = 
{
    // cleans a string for client-side manipulation by 
    //      1) replacing \ characters with \\
    //      2) replacing ' characters with \'
    _cleanString : function(str)
    {
        var result = str;
	    result = result.replace(/\\/g, "\\\\")
	    result = result.replace(/'/g, "\\'")
        return result;
    },
    
    // Gets an object containing the position of the autocomplete box
    // .x   - the absolute x position
    // .y   - the absolute y position
    _getAbsolutePosition : function(element)
    {
	    var posX = 0;
	    var posY = 0;
	    if (element.offsetParent)
	    {
	        while (element != null)
	        {
		        posX += element.offsetLeft;
		        posY += element.offsetTop;
		        element = element.offsetParent;
	        }
	    }
	    else if (element.x)
	    {
            posX = obj.x;
            posY = obj.y;
	    }
	    return { x:posX, y:posY };
    },

    // clears the value in the text box and the hidden input field
    clearSelection : function()
    {
        this.selectedItem = null;
        this.hiddenInput.value = '';
        this.textBox.value = '';
    },
    
    // this is the main worker method, called by AjaxPro when the server-side data retrieval completes
    // this MUST accept a ASP.NET DataTable
    handleCallBack : function(result) 
    {
        var itemHtml = '';
        this._items = new Array();
        
        if (result.length > 0) 
        {    
            for (var i=0; i<result.length; i++) 
            {
                // hop out if we've exceed the maximum row count
                if (this.maxRowCount > -1 && i >= this.maxRowCount)
                    break;
                    
                var itemText = result[i].Text;
                var itemValue = result[i].Value;
                var itemDisplayText = result[i].DisplayText;
                this._items[i] = new Array();
                this._items[i][0] = itemText;
                this._items[i][1] = itemValue;
                this._items[i][2] = itemDisplayText;
                
                // add the new item to the resulting HTML
                itemHtml += '<div class="' + this.cssClassItem + '"' +
                    ' onmouseout="SAHLAutoComplete_mouseOut(this, \'' + this.textBoxID + '\')"' +
                    ' onmouseover="SAHLAutoComplete_mouseOver(this, \'' + this.textBoxID + '\', ' + i + ')"' +
                    ' onmousedown="SAHLAutoComplete_mouseDown(\'' + this.textBoxID + '\', ' + i + ')"' +
                    '>';
                itemHtml += itemText;
                itemHtml += '</div>';
                
            }

            // reposition the div and write the HTML string to it's contents
            var position = this._getAbsolutePosition(this.textBox);
            this._div.innerHTML = itemHtml;
            this._div.style.display = 'block';
            // this._div.style.width = this.textBox.offsetWidth;
            this._div.className = this.cssClass;
            
            // position the div, may be above/below, and to the left/right
            if (this.horizontalPosition == 0) 
                this._div.style.left = position.x + 'px';   
            else
                this._div.style.left = (position.x + this.textBox.offsetWidth - this._div.offsetWidth) + 'px';
            if (this.verticalPosition == 0) 
                this._div.style.top = (position.y + this.textBox.offsetHeight) + 'px';
            else
                this._div.style.top = (position.y - this._div.offsetHeight) + 'px';
                
            // reset the width of the base div - this is required as not all browsers 
            // will automatically resize and the child items then flow over the edge
            var width = this._div.offsetWidth;
            for (var c=0; c < this._div.childNodes.length; c++)
            {
                if (this._div.childNodes[c].offsetWidth > width)
                    width = this._div.childNodes[c].offsetWidth;
            }
            if (width < this.textBox.offsetWidth)
                width = this.textBox.offsetWidth;
            this._div.style.width = width;
                
            // move the iframe under the div
            this._iframe.style.left = this._div.style.left;
            this._iframe.style.top = this._div.style.top;
            this._iframe.style.width = this._div.offsetWidth;
            this._iframe.style.height = this._div.offsetHeight;
            this._iframe.style.display = 'block';

        }
        else 
        {
            // no items to display - just call hide
            this.hide();
        }
        
        // reset the busy status
        this._busy = false;
    },
    
    // when the text box loses focus, hide the AutoComplete box
    handleBlur : function() 
    {
        this.hide();
    },
    
    // event raised when the textbox receives focus
    handleFocus : function() 
    {
        if (!this._errorOccurred) 
        {
            if (this.textBox.value.length > 0 && this.textBox.value.length >= this.minCharacters) 
            {
                clearTimeout(this._timer);
                this._timer = setTimeout('SAHLAutoComplete_retrieveData("' + this.textBoxID + '")', 500);
            }       
        }
        this._errorOccurred = false;
    },  
    
    // handles key down events on the text box
    handleKeyDown : function(ev)
    {
        if ((ev.which && ev.which == 13) || (ev.keyCode && ev.keyCode == 13))
        {
            this.selectCurrent(this._currentIndex);
            ev.returnValue = false;
            return false;
        }
    },
    
    // handles key up events on the text box
    handleKeyUp : function(ev) 
    {
        clearTimeout(this._timer);
        switch (ev.keyCode || ev.which) 
        {
            case 13:        // enter key
                ev.returnValue = false;
                return;
            case 27:        // escape key
                this.hide();
                return;
            case 37:        // left arrow
            case 39:        // right arrow
                return;
            case 38:        // up arrow
                this.moveHighlight(-1);
                return;
            case 40:        // down arrow
                this.moveHighlight(1);
                return;
        }
        // valid key has been pressed - clear the input value
        this.hiddenInput.value = '';
        
        if (this.textBox.value.length > 0) 
        {
            if (this.textBox.value.length >= this.minCharacters)
                this._timer = setTimeout('SAHLAutoComplete_retrieveData("' + this.textBoxID + '")', 500);
            else
                this.hide();
        }
        else 
        {
            this.clearSelection();
            this.hide();
        }
        return;    
    },

    // handles mousedown events on the items
    handleMouseDown : function(itemIndex)
    {
        this.selectCurrent(itemIndex);
    },
    
    // handles mouseout events on the items
    handleMouseOut : function(element)
    {
        element.className = this.cssClassItem;
        this._currentIndex = -1;
    },
    
    // handles mouseover events on the items
    handleMouseOver : function(element, itemIndex)
    {
        element.className = this.cssClassItemSelected;
        this._currentIndex = itemIndex;
    },

    // hides the autocomplete box
    hide : function() 
    {
        this._div.style.display = 'none';
        this._iframe.style.display = 'none';
    },
    
    // moves the selected item up or down a supplied increment (this is used when up/down arrows are pressed)
    moveHighlight : function(increment) 
    {
        var items = this._div.getElementsByTagName('DIV');
        
        // reset the previously selected item
        if ((this._currentIndex >= 0) && (this._currentIndex < items.length))
            items[this._currentIndex].className = this.cssClassItem;
        
        this._currentIndex += increment;
        // make sure the new index is within an acceptable range
        if (this._currentIndex < 0) 
            this._currentIndex = 0;
        else if (this._currentIndex >= items.length) 
            this._currentIndex = (items.length - 1);
            
        items[this._currentIndex].className = this.cssClassItemSelected;
    },
        
    // data retrieval method for AJAX call
    retrieveData : function() 
    {
        if (this._busy) return;
        this._busy = true;
        
        var serviceCall = this.serviceMethod + '(\'' + this._cleanString(this.textBox.value) + '\', ';
        
        // if there are any parent controls, then add their values
        for (var i=0; i<this.parentControls.length; i++)
        {
            var val = '';
            if (this.parentControls[i].tagName.toLowerCase() == 'select')
                val = this.parentControls[i].options[this.parentControls[i].selectedIndex].value;
            else
                val = this.parentControls[i].value;
            serviceCall += '\'' + this._cleanString(val) + '\', ';
        }

        serviceCall = serviceCall + 'SAHLAutoComplete_callBack)';
        eval(serviceCall);
    },
    
    // selects a specific item, and updates the hidden control with the selected value
    selectCurrent : function(itemIndex)
    {
        this.selectedItem = this._items[itemIndex];
        this.textBox.value = this.selectedItem[2];
        this.hiddenInput.value = this.selectedItem[1];
        this.hide();
        
        // if a javascript function has been defined, call it
        if (this.clientClickFunction.length > 0)
        {
            var result = { textBox:this.textBox, autoComplete:this, key:this.selectedItem[1], value:this.selectedItem[0], displayText:this.selectedItem[2] };
            var clickCall = this.clientClickFunction + '(result)';
            eval(clickCall);
        }
        
        // if a postback function has been defined, call it
        if (this.postBackFunction.length > 0)
            eval(this.postBackFunction);

    }
}
