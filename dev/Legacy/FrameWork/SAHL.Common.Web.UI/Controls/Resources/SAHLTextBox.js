var SAHLTextBox_TextChanged=false;

function SAHLTextBox_NumOnly(allowNegative)
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

function SAHLTextBox_AlphaNumNoSpace()
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

function SAHLTextBox_NumCurrency(allowNegative,limitDecimals)
{
    
    var NegativesAllowed=false, DecimalsLimited=-1;
    if (arguments.length > 0)
    {
        if (allowNegative)
            NegativesAllowed = true;

        DecimalsLimited = limitDecimals;   
    }
    
    if (NegativesAllowed)
    {
        if (DecimalsLimited == -1) // unlimited decimal places
            regex = new RegExp("^[-]?[0-9][0-9]*[.]?[0-9]*$")       
        else // limit to specified number of decimal places 
		{
			var StrRegExp = "^[-]?[0-9][0-9]*[.]";
			for(var i=0; i < DecimalsLimited; i++){
			StrRegExp = StrRegExp + "?[0-9]"
			}
			StrRegExp = StrRegExp + "?$";
			regex = new RegExp(StrRegExp) 
		}

    }
    else
    {
        if (DecimalsLimited == -1) // only 2 decimal places
            regex = new RegExp("^[0-9][0-9]*[.]?[0-9]*$")       
        else // limit to specified number of decimal places 		
        {
			var StrRegExp = "^[0-9][0-9]*[.]";
			for(var i=0; i < DecimalsLimited; i++){
			StrRegExp = StrRegExp + "?[0-9]"
			}
			StrRegExp = StrRegExp + "?$";
			regex = new RegExp(StrRegExp) 
		}
    }    
    // get the control
    var ctrl = event.srcElement; 
    ctrl.focus(); // focus on the cursor position
    // get the text value of the textbox excluding the pressed key value
    var saveText = ctrl.value; 
    // get the text value of the textbox including the pressed key value
    if (document.selection)
    {
        // select the cursor position
        sel = document.selection.createRange(); 
        // this will insert the pressed char at the cursor pos
        sel.text = String.fromCharCode(event.keyCode);
    }
    var fullText = ctrl.value; 

    var cursorpos = SAHLTextBox_getCaretPosition(ctrl)
    
    // validate against regex
    if ((!NegativesAllowed || fullText != "-") && !regex.test(fullText))
    {
        // Invalid Input
        ctrl.value = saveText;
        event.returnValue = false;

    }
    else
    {
        SAHLTextBox_TextChanged=true;
        
        // Valid Input
        ctrl.value = fullText;      
        // set the cursor position
        SAHLTextBox_setCaretPosition(ctrl,cursorpos)
              
        event.returnValue = false;
    }          
}
function SAHLTextBox_getCaretPosition (ctrl) 
{
	var CaretPos = 0;
	// IE Support
	if (document.selection) 
	{
		ctrl.focus ();
		var Sel = document.selection.createRange ();

		Sel.moveStart ('character', -ctrl.value.length);

		CaretPos = Sel.text.length;
	}
	// Firefox support
	else if (ctrl.selectionStart || ctrl.selectionStart == '0')
		CaretPos = ctrl.selectionStart;

	return (CaretPos);
}

function SAHLTextBox_setCaretPosition(ctrl, pos)
{
    if (pos==0) pos = 1
    
	if(ctrl.setSelectionRange)
	{
		ctrl.focus();
		ctrl.setSelectionRange(pos,pos);
	}
	else if (ctrl.createTextRange) 
	{
		var range = ctrl.createTextRange();
		range.collapse(true);
		range.moveEnd('character', pos);
		range.moveStart('character', pos);
		range.select();
	}
}

function SAHLTextBox_checkTextChanged(ctrl)
{
    if (SAHLTextBox_TextChanged)
    {
        ctrl.fireEvent("onchange");       
    }
    SAHLTextBox_TextChanged = false;
}

function SAHLTextBox_negativeBlur(textBox) 
{
    if (textBox.value.indexOf('-') > 0) 
    {
        textBox.value = '-' + textBox.value.replace('-', '');
    }
    else if (textBox.value == '-') 
    {
        textBox.value = '';
    }   
}