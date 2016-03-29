// function for use on the client - enables/disables an SAHLButton
function SAHLButton_setEnabled(id, enabled)
{
    var button = document.getElementById(id);
    var className = button.className;
    var re;

    // if the toggle is the same as what is already there, just exit
    if (enabled == !button.disabled)
        return;
        
    var current = "";
    for (var i=1; i<=6; i++)
    {
        current = "SAHLButton" + i;
        if (className.indexOf(current) > -1)
            break;
    }
    
    if (enabled)
        className = className.replace(current + "Dis", current);
    else
        className = className.replace(current, current + "Dis");
    
    button.className = className;
    button.disabled = !enabled;
}

// updates the event target hidden control so we know if a button caused a postback
function SAHLButton_updateEventTarget(button) 
{
    var eventTarget = document.getElementById('__EVENTTARGET');
    if (eventTarget == null) 
    {
        eventTarget = document.createElement('input');
        eventTarget.setAttribute('type', 'hidden');
        eventTarget.id = '__EVENTTARGET';
        eventTarget.name = '__EVENTTARGET';
        document.forms[0].appendChild(eventTarget);
    }
    eventTarget.value = button.name;
}
