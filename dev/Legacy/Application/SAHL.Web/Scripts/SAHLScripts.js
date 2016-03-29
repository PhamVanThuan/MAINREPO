function NumOnly(allowNegative)
{
    if ( ((window.event.keyCode<48) || (window.event.keyCode>57)) && (window.event.keyCode != 13) )
        window.event.returnValue = false;
    
    // check to see if negatives allowed
    if (arguments.length > 0 
        && allowNegative 
        && window.event.keyCode == 45 
        && (event.srcElement.value.indexOf("-") == -1 || (document.selection.createRange().text.length == event.srcElement.value.length))
        )
        window.event.returnValue = true;
        
    return;
}


function AlphaNumNoSpace()
{
    if ((window.event.keyCode>=48) && (window.event.keyCode<=57) || 
        (window.event.keyCode >= 65 && window.event.keyCode <= 90) || 
        (window.event.keyCode >= 97 && window.event.keyCode <= 122) ||
         window.event.keyCode == 64 || 
         window.event.keyCode == 45 || 
         window.event.keyCode == 95 || 
         window.event.keyCode == 46)
    {
        window.event.returnValue = true;
    }
    else
    {
        window.event.returnValue = false;
    }
}
function NumCurrency(allowNegative)
{
    var se = /\d|[.]/g
    var key = String.fromCharCode(event.keyCode);

    if ( key.search(se) )
        event.returnValue = false;

    // check to see if negatives allowed
    if (arguments.length > 0 
        && allowNegative 
        && window.event.keyCode == 45 
        && (event.srcElement.value.indexOf("-") == -1 || (document.selection.createRange().text.length == event.srcElement.value.length))
        )
        window.event.returnValue = true;

    if (event.keyCode == 46)
        if (event.srcElement.value.indexOf(".") > -1)
            event.returnValue = false;
}
function getParentElem(el, pTagName)
{
    if (el == null) return null;
    else if (el.nodeType == 1 && el.tagName.toLowerCase() == pTagName.toLowerCase())	// Gecko bug, supposed to be uppercase
        return el;
    else
        return getParentElem(el.parentNode, pTagName);
}
function selectGridRow(row,fixedheader,hiddenTxt)
{
    // Set all the rows back to normal.
    var gridTable = getParentElem(row,"TABLE");

    var iC;
    var iStart;
    var iM = gridTable.rows.length;
    
    if ( fixedheader == undefined ) fixedheader = false;
    if ( fixedheader )
        iStart = 0;
    else
        iStart = 1;
    
    for ( iC=iStart; iC<iM; iC++ )
    {
        if (iC%2 == 0)
            gridTable.rows[iC].className = "TableRowA2";
        else
            gridTable.rows[iC].className = "TableRowA";
        //if( gridTable.rows[iC].cells[0].RowColor != undefined )
        //    gridTable.rows[iC].style.backgroundColor = gridTable.rows[iC].cells[0].RowColor;
    }
    
    row.className = "TableSelectRowA";
    //if( row.cells[0].RowColor != undefined )
    //    row.style.backgroundColor = "#000000";
    
    if ( hiddenTxt != undefined )
    {
        var oH = document.getElementsByName(hiddenTxt);
        if ( oH != null )
            oH[0].value = row.rowIndex-1;
    }
}
// Search Combobox by typed chars.
var sKey = "";
var Timeout;
function searchKey(cbo)
{
    sKey += String.fromCharCode(window.event.keyCode);
    window.status = "Searching: " + sKey;
    window.clearTimeout(Timeout);
    Timeout = window.setTimeout( "doKeySearch('"+cbo+"')", 500 );
}
function doKeySearch(cbo)
{
    var o = document.getElementById(cbo);
    if ( o != null )
    {
        if ( o.disabled == true ) return;
        
        var i;
        var iLen = sKey.length;
        var mCount = o.length;
        for ( i=0; i<mCount; i++ )
        {
            if ( sKey.toLowerCase() == o.item(i).text.substr(0,iLen).toLowerCase() )
            {
                o.selectedIndex = i;
                if (typeof o.onchange == "function")
                    o.onchange();
                break;
            }
        }
    }
    
    sKey = "";
    window.status = "Done";
}

function trim(str)
{
    return str.replace(/^\s*|\s*$/g,"");
}

function phoneKeyUp(elem,testValue,codeMax)
{
    if ( ((window.event.keyCode<48) || (window.event.keyCode>57)) && window.event.keyCode!=37 && window.event.keyCode!=39 )
    {
        window.event.returnValue = false;
        return;
    }

    if ( window.event.keyCode==37 )
    {
        if ( testValue == "NUMB" )
        {
            o = document.getElementById(elem.id.substr(0,elem.id.length-5)+"_CODE");
            if ( o != null )
                o.select();
        }
        return;
    }

    if ( window.event.keyCode==39 )
    {
        if ( testValue == "CODE" )
        {
            o = document.getElementById(elem.id.substr(0,elem.id.length-5)+"_NUMB");
            if ( o != null )
                o.select();
        }
        return;
    }

    if ( testValue == "NUMB" ) return;

    if ( elem.value.length == codeMax )
    {
        var o = document.getElementById(elem.id.substr(0,elem.id.length-5)+"_NUMB");
        if ( o != null )
            o.select();
    }
    return;
}
// Limit amount of text in a textarea.
function LimitField(field, countfield, maxlimit)
{
	var oField = document.getElementById(field);
	var oCountfield = document.getElementById(countfield);
	
	if ( oField != null )
	{
		if ( oField.value.length > maxlimit )
			oField.value = oField.value.substring(0, maxlimit);
		else
		{
			if ( oCountfield != null )
				oCountfield.innerHTML = "<b>" + oField.value.length + "</b> of <b>" + maxlimit + "</b> characters";
		}
	}
}
// Exact combo values.
function selectOther(cbo,other_id)
{
    var cbo2 = document.getElementById(other_id);
    if ( cbo2 != null )
        cbo2.selectedIndex = cbo.selectedIndex;
}
// Exact combo values but sorted different.
function selectOtherEx(cbo,other_id)
{
    var cbo2 = document.getElementById(other_id);
    if ( cbo2 != null )
    {
        var iC = 0;
        var iM = cbo2.length;
        for ( iC=0; iC<iM; iC++ )
        {
            if ( cbo2.item(iC).value == cbo.value )
            {
                cbo2.selectedIndex = iC;
                break;
            }
        }
    }
}

function focusCboTreeItem(id)
{
    try {
        
        var obj = document.getElementById(id);
        if (obj){
                obj.scrollIntoView(true);
            }
            
        
        if (id.substr(0, 6) == "li_ctx"){
        
            var ctx = document.getElementById("ctxlist");    
            if (ctx != null){
                ctx.scrollLeft = 0;
                if (ctx.scrollHeight > ctx.clientHeight)//there are scrollbars
                    scrolltoCenter(ctx, obj);
            }
        }
        
        if (id.substr(0, 6) == "li_cbo"){
            var cbo = document.getElementById("cbolist"); 
            if (cbo != null){
                cbo.scrollLeft = 0;
                if (cbo.scrollHeight > cbo.clientHeight)//there are scrollbars
                    scrolltoCenter(cbo, obj);
            }
        }        
        
    }
    catch(e) {}
}

/// scroll the selected item into the center
function scrolltoCenter(tree, item)
{
    var xy = getAbsolutPosition(item);
    var hw = (tree.clientHeight / 2)-16; //it's our goal to always get the selected item in the middle of the client window 
    var bh = tree.scrollHeight - (xy.y - tree.offsetTop+32); // gap from the bottom of the tree to the selected item
    
    // if the item is near the end don's croll it at all, else scroll it just enough to get 
    // in the middle
    
    if (bh > hw){
        if ((bh - hw) > hw)
            tree.scrollTop = tree.scrollTop - hw;
        else{
            tree.scrollTop = tree.scrollTop - (bh - hw);
            }
     }
}

function getAbsolutPosition(el)
{
    var curleft = curtop = 0;
	if (el.offsetParent) {
		curleft = el.offsetLeft
		curtop = el.offsetTop
		while (el = el.offsetParent) {
			curleft += el.offsetLeft
			curtop += el.offsetTop
		}
	}
	var retval = {x: curleft, y: curtop};
	return retval;
}
	
	
function SetCookie(sName, sValue)
{
    // No expiration, so it will expire when the browser closes.
    document.cookie = sName + "=" + escape(sValue);
}
function GetCookie(sName)
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
function disableButton(elem)
{
    if ( elem != null )
    {
        var s = elem.className;
        elem.disabled = true;
        elem.className = s.substr(0,10)+"Dis";
    }
}
function confirmDelete(msg)
{
    if ( confirm("Are you sure you want to delete this item?") )
        return true;
    else
        return false;
}
function confirmMessage(msg)
{
    if ( confirm(msg) )
        return true;
    else
        return false;
}

function popYesNoCancel(msg) // Returns YES, NO or CANCEL.
{
    var dfeatures = "dialogHeight:140px; dialogWidth:300px; resizable:yes; scroll:no; status:no;";
    var args = new Array(msg, true, true, true);
    return window.showModalDialog("PopYesNoCancel.aspx", args, dfeatures);
}

function popUp(msg, showYes, showNo, showCancel) // Returns YES, NO or CANCEL.
{
    var dfeatures = "dialogHeight:140px; dialogWidth:300px; resizable:yes; scroll:no; status:no;";
    var args = new Array(msg, showYes, showNo, showCancel);
    return window.showModalDialog("PopYesNoCancel.aspx", args, dfeatures);
}

function SelectAllCheckboxes(spanChk) // added by Craig Fraser  - used to select all checkboxes in a gridview checkbox column
{
   // Added as ASPX uses SPAN for checkbox
   var oItem = spanChk.children;
   var theBox= (spanChk.type=="checkbox") ? spanChk : spanChk.children.item[0];
   xState=theBox.checked;
   elm=theBox.form.elements;

   for(i=0;i<elm.length;i++)
   {
     if(elm[i].type=="checkbox" && elm[i].id!=theBox.id)
     {
       if(elm[i].checked!=xState)
         elm[i].click();
     }
   }
}    

dontShowBusy = false;
function ShowBusy()
{
    if ( dontShowBusy ) return;
    var md = document.all ? document.all["MainDiv"] : document.getElementById("MainDiv");
    md.style.visibility = 'hidden';
    
    document.body.className='unloading';
    // NOTE: don't remove document elements here - this can cause errors to occur with LinkButtons/anchors
    // because the event fires AFTER the onbeforeunload event in IE6 (MattS:14/02/2007)
    //document.body.innerHTML = "<H2>Please Wait</H2>"
}


// Javascript Number Formatting Functions follow:

// Abbreviations: LODP = Left Of Decimal Point, RODP = Right Of Decimal Point
Number.formatFunctions = {count:0};

Number.prototype.numberFormat = function(format, context) {
    if (isNaN(this) || this == +Infinity || this == -Infinity) {
        return this.toString();
    }
    if (Number.formatFunctions[format] == null) {
        Number.createNewFormat(format);
    }
    return this[Number.formatFunctions[format]](context);
}

Number.createNewFormat = function(format) {
    var funcName = "format" + Number.formatFunctions.count++;
    Number.formatFunctions[format] = funcName;
    var code = "Number.prototype." + funcName + " = function(context){\n";

    // Decide whether the function is a terminal or a pos/neg/zero function
    var formats = format.split(";");
    switch (formats.length) {
        case 1:
            code += Number.createTerminalFormat(format);
            break;
        case 2:
            code += "return (this < 0) ? this.numberFormat(\""
                + String.escape(formats[1])
                + "\", 1) : this.numberFormat(\""
                + String.escape(formats[0])
                + "\", 2);";
            break;
        case 3:
            code += "return (this < 0) ? this.numberFormat(\""
                + String.escape(formats[1])
                + "\", 1) : ((this == 0) ? this.numberFormat(\""
                + String.escape(formats[2])
                + "\", 2) : this.numberFormat(\""
                + String.escape(formats[0])
                + "\", 3));";
            break;
        default:
            code += "throw 'Too many semicolons in format string';";
            break;
    }
    eval(code + "}");
}

// Formats a JavaScript Numeric value value as Defined. Example below as currency
//var Displaynum = new Number(VarToFormat).numberFormat(\"00.00\");";
Number.createTerminalFormat = function(format) {
    // If there is no work to do, just return the literal value
    if (format.length > 0 && format.search(/[0#?]/) == -1) {
        return "return '" + String.escape(format) + "';\n";
    }
    // Negative values are always displayed without a minus sign when section separators are used.
    var code = "var val = (context == null) ? new Number(this) : Math.abs(this);\n";
    var thousands = false;
    var lodp = format;
    var rodp = "";
    var ldigits = 0;
    var rdigits = 0;
    var scidigits = 0;
    var scishowsign = false;
    var sciletter = "";
    // Look for (and remove) scientific notation instructions, which can be anywhere
    m = format.match(/\..*(e)([+-]?)(0+)/i);
    if (m) {
        sciletter = m[1];
        scishowsign = (m[2] == "+");
        scidigits = m[3].length;
        format = format.replace(/(e)([+-]?)(0+)/i, "");
    }
    // Split around the decimal point
    var m = format.match(/^([^.]*)\.(.*)$/);
    if (m) {
        lodp = m[1].replace(/\./g, "");
        rodp = m[2].replace(/\./g, "");
    }
    // Look for %
    if (format.indexOf('%') >= 0) {
        code += "val *= 100;\n";
    }
    // Look for comma-scaling to the left of the decimal point
    m = lodp.match(/(,+)(?:$|[^0#?,])/);
    if (m) {
        code += "val /= " + Math.pow(1000, m[1].length) + "\n;";
    }
    // Look for comma-separators
    if (lodp.search(/[0#?],[0#?]/) >= 0) {
        thousands = true;
    }
    // Nuke any extraneous commas
    if ((m) || thousands) {
        lodp = lodp.replace(/,/g, "");
    }
    // Figure out how many digits to the l/r of the decimal place
    m = lodp.match(/0[0#?]*/);
    if (m) {
        ldigits = m[0].length;
    }
    m = rodp.match(/[0#?]*/);
    if (m) {
        rdigits = m[0].length;
    }
    // Scientific notation takes precedence over rounding etc
    if (scidigits > 0) {
        code += "var sci = Number.toScientific(val,"
            + ldigits + ", " + rdigits + ", " + scidigits + ", " + scishowsign + ");\n"
            + "var arr = [sci.l, sci.r];\n";
    }
    else {
        // If there is no decimal point, round to nearest integer, AWAY from zero
        if (format.indexOf('.') < 0) {
            code += "val = (val > 0) ? Math.ceil(val) : Math.floor(val);\n";
        }
        // Numbers are rounded to the correct number of digits to the right of the decimal
        code += "var arr = val.round(" + rdigits + ").toFixed(" + rdigits + ").split('.');\n";
        // There are at least "ldigits" digits to the left of the decimal, so add zeros if needed.
        code += "arr[0] = (val < 0 ? '-' : '') + String.leftPad((val < 0 ? arr[0].substring(1) : arr[0]), "
            + ldigits + ", '0');\n";
    }
    // Add thousands separators
    if (thousands) {
        code += "arr[0] = Number.addSeparators(arr[0]);\n";
    }
    // Insert the digits into the formatting string.  On the LHS, extra digits are copied
    // into the result.  On the RHS, rounding has chopped them off.
    code += "arr[0] = Number.injectIntoFormat(arr[0].reverse(), '"
        + String.escape(lodp.reverse()) + "', true).reverse();\n";
    if (rdigits > 0) {
        code += "arr[1] = Number.injectIntoFormat(arr[1], '" + String.escape(rodp) + "', false);\n";
    }
    if (scidigits > 0) {
        code += "arr[1] = arr[1].replace(/(\\d{" + rdigits + "})/, '$1" + sciletter + "' + sci.s);\n";
    }
    return code + "return arr.join('.');\n";
}

Number.toScientific = function(val, ldigits, rdigits, scidigits, showsign) {
    var result = {l:"", r:"", s:""};
    var ex = "";
    // Make ldigits + rdigits significant figures
    var before = Math.abs(val).toFixed(ldigits + rdigits + 1).trim('0');
    // Move the decimal point to the right of all digits we want to keep,
    // and round the resulting value off
    var after = Math.round(new Number(before.replace(".", "").replace(
        new RegExp("(\\d{" + (ldigits + rdigits) + "})(.*)"), "$1.$2"))).toFixed(0);
    // Place the decimal point in the new string
    if (after.length >= ldigits) {
        after = after.substring(0, ldigits) + "." + after.substring(ldigits);
    }
    else {
        after += '.';
    }
    // Find how much the decimal point moved.  This is #places to LODP in the original
    // number, minus the #places in the new number.  There are no left-padded zeroes in
    // the new number, so the calculation for it is simpler than for the old number.
    result.s = (before.indexOf(".") - before.search(/[1-9]/)) - after.indexOf(".");
    // The exponent is off by 1 when it gets moved to the left.
    if (result.s < 0) {
        result.s++;
    }
    // Split the value around the decimal point and pad the parts appropriately.
    result.l = (val < 0 ? '-' : '') + String.leftPad(after.substring(0, after.indexOf(".")), ldigits, "0");
    result.r = after.substring(after.indexOf(".") + 1);
    if (result.s < 0) {
        ex = "-";
    }
    else if (showsign) {
        ex = "+";
    }
    result.s = ex + String.leftPad(Math.abs(result.s).toFixed(0), scidigits, "0");
    return result;
}

Number.prototype.round = function(decimals) {
    if (decimals > 0) {
        var m = this.toFixed(decimals + 1).match(
            new RegExp("(-?\\d*)\.(\\d{" + decimals + "})(\\d)\\d*$"));
        if (m && m.length) {
            return new Number(m[1] + "." + String.leftPad(Math.round(m[2] + "." + m[3]), decimals, "0"));
        }
    }
    return this;
}

Number.injectIntoFormat = function(val, format, stuffExtras) {
    var i = 0;
    var j = 0;
    var result = "";
    while (i < format.length && j < val.length && format.substring(i).search(/[0#?]/) >= 0) {
        if (format.charAt(i).match(/[0#?]/)) {
            // It's a formatting character; copy the corresponding character
            // in the value to the result
            if (val.charAt(j) != '-') {
                result += val.charAt(j);
            }
            else {
                result += "0";
            }
            j++;
        }
        else {
            result += format.charAt(i);
        }
        ++i;
    }
    if (j == val.length && val.substring(j - 1) == '-') {
        result += '-';
    }
    if (j < val.length && stuffExtras) {
        result += val.substring(j);
    }
    if (i < format.length) {
        result += format.substring(i);
    }
    return result.replace(/#/g, "").replace(/\?/g, " ");
}

Number.addSeparators = function(val) {
    return val.reverse().replace(/(\d{3})/g, "$1,").reverse().replace(/^(-)?,/, "$1");
}

String.prototype.reverse = function() {
    var res = "";
    for (var i = this.length; i > 0; --i) {
        res += this.charAt(i - 1);
    }
    return res;
}

String.leftPad = function (val, size, ch) {
    var result = new String(val);
    if (ch == null) {
        ch = " ";
    }
    while (result.length < size) {
        result = ch + result;
    }
    return result;
}

String.escape = function(string) {
    return string.replace(/('|\\)/g, "\\$1");
}            
