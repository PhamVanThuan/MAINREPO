function SAHLCurrencyBox_blur(source, txtMainId, txtRandId, txtCentId) 
{
    var txtMain = document.getElementById(txtMainId);
    var txtRand = document.getElementById(txtRandId);
    var txtCent = document.getElementById(txtCentId);
    
    if (isNaN(txtRand.value))
        txtRand.value = '';
        
    if (isNaN(txtCent.value))
        txtCent.value = '';
        
    if (txtCent.value.length == 1)
        txtCent.value = txtCent.value + '0';
        
    if (txtRand.value.length > 0 && txtCent.value.length == 0)
        txtCent.value = '00';
        
    if (txtRand.value.length == 0 && txtCent.value.length == 0)
        txtMain.value = '';
    else
        txtMain.value = txtRand.value + '.' + txtCent.value;
        
}

function SAHLCurrencyBox_getValue(txtMainId)
{
    var txtMain = document.getElementById(txtMainId);
    var txtRand = document.getElementById(txtMainId + '_txtRands');
    var txtCent = document.getElementById(txtMainId + '_txtCents');
    
    if (txtRand.value.length == 0 && txtCent.value.length == 0)
        return '';
    else
        return txtRand.value + '.' + txtCent.value;
}

function SAHLCurrencyBox_keyDown(source, txtRandId, txtCentId) 
{
    var txtRand = document.getElementById(txtRandId);
    var txtCent = document.getElementById(txtCentId);
    
    if (window.event && (window.event.keyCode == 190 || window.event.keyCode == 110))    // if decimal point is clicked, focus on the cents
    {
        txtCent.focus();
        txtCent.select();
        
        if (window.event.cancelBubble) window.event.cancelBubble = true;
        if (window.event.returnValue) window.event.returnValue = false;
    }
}

function SAHLCurrencyBox_setValue(txtMainId, value)
{
    var txtMain = document.getElementById(txtMainId);
    var txtRand = document.getElementById(txtMainId + '_txtRands');
    var txtCent = document.getElementById(txtMainId + '_txtCents');
    
	var ptIndex = value.indexOf('.');
	
	var cents = '';
	var rands = value;
	
	if (ptIndex > -1)
	{
		rands = value.substring(0, ptIndex);
		cents = value.substring(ptIndex + 1);
	}

    if (cents.length == 0)
        cents = '00';
    else if (cents.length == 1)
		cents = '0' + cents;
	else if (cents.length > 2)
		cents = cents.substring(0, 2);
    
    txtRand.value = rands;
    txtCent.value = cents;
    txtMain.value = rands + '.' + cents;
}

// toggles the disabled status of an SAHLCurrencyBox
function SAHLCurrencyBox_setEnabled(txtMainId, enabled)
{
    var txtMain = document.getElementById(txtMainId);
    var txtRand = document.getElementById(txtMainId + '_txtRands');
    var txtCent = document.getElementById(txtMainId + '_txtCents');
    
    txtRand.disabled = !enabled;
    txtCent.disabled = !enabled;
}


