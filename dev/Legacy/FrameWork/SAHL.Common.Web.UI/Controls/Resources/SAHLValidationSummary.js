// initialises the control - this relies on Display.js which should be included by default in all SAHL pages
function SAHLValidationSummary_init(id)
{
    var element = document.getElementById(id);
    centerElement(element, true, false);
    
    // for IE6, we need to attach an Iframe to the drag event, so the div appears correctly in front of 
    // drop down boxes
    if (element.attachEvent)
    {
        element.attachEvent('onmove', SAHLValidationSummary_moveEvent, false);
        window.attachEvent('onload', function() { SAHLValidationSummary_enforceIframe(id) });
    }
    
    
}

// called when the submit button hooked up to the control is clicked
function SAHLValidationSummary_clickCancel(id)
{
    var control = document.getElementById(id);
    var iframe = SAHLValidationSummary_getIframe(control);
    control.style.display = 'none';
    iframe.style.display = 'none';
    return false;
}


// called when the submit button hooked up to the control is clicked
function SAHLValidationSummary_clickIgnore(hiddenId, buttonId)
{
    var hiddenInput = document.getElementById(hiddenId);
    var button = document.getElementById(buttonId);
    hiddenInput.value = "1";
    button.click();
    return false;
}

// this creates an iframe to work around the IE dropdown bug always appearing in front of divs
function SAHLValidationSummary_enforceIframe(id)
{
    var control = document.getElementById(id);

    // create an iframe to slot behind the control
    var iframe = SAHLValidationSummary_getIframe(control);
    if (iframe == null)
    {
        iframe = document.createElement('IFRAME');
        iframe.id = control.id + '_iframe';
        iframe.style.display = 'none';
        iframe.style.position = 'absolute';
        iframe.style.zIndex = 1;
        document.body.appendChild(iframe);
    }
    SAHLValidationSummary_resizeIframe(id);
}

// gets the iframe associated with the control
function SAHLValidationSummary_getIframe(control)
{
    return document.getElementById(control.id + '_iframe');
}

// event handler for when the control is dragged or moved
function SAHLValidationSummary_moveEvent()
{
    var id = window.event.srcElement.id;
    if(id != null && id != '')
        SAHLValidationSummary_resizeIframe(id);
}

// does the work of resizing the background iframe if it exists
function SAHLValidationSummary_resizeIframe(id)
{
    var control = document.getElementById(id);
    var iframe = SAHLValidationSummary_getIframe(control);
    if (iframe != null)
    {
        iframe.style.left = control.offsetLeft;
        iframe.style.top = control.offsetTop;
        iframe.style.width = control.offsetWidth;
        iframe.style.height = control.offsetHeight;
        iframe.style.border = '0px solid white';
        iframe.style.display = 'inline';
    }
}

// toggles the validation summary control between minimised and maximised
function SAHLValidationSummary_toggle(id, headerId, bodyId)
{
    var pnlHeader = document.getElementById(headerId);
    var pnlBody = document.getElementById(bodyId);
    var img = pnlHeader.getElementsByTagName('IMG')[1];
    if (pnlBody.style.display == 'none')
    {
        pnlBody.style.display = "block";
        img.src = '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.minimise_arrow.gif") %>';
        img.title = 'Minimise';
    }
    else
    {
        pnlBody.style.display = "none";
        img.src = '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.maximise_arrow.gif") %>';
        img.title = 'Maximise';
    }
    SAHLValidationSummary_resizeIframe(id);
    if (window.event)
    {
        if (window.event.cancelBubble) window.event.cancelBubble = true;
        if (window.event.returnValue) window.event.returnValue = false;
    }
}
    
