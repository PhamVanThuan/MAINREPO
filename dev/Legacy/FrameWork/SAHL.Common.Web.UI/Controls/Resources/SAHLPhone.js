function SAHLPhone_keyUp(elem,testValue,codeMax,onClientKeyUp)
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

    if (testValue == "CODE" && elem.value.length == codeMax )
    {
        var o = document.getElementById(elem.id.substr(0,elem.id.length-5)+"_NUMB");
        if ( o != null )
            o.select();
    }
    
    if (onClientKeyUp.length > 0)
        eval(onClientKeyUp);
        
    return;
}
