var SAHLDropDownList_Key = "";
var SAHLDropDownList_Timeout;

function SAHLDropDownList_searchKey(cbo)
{
    SAHLDropDownList_Key += String.fromCharCode(window.event.keyCode);
    window.status = "Searching: " + SAHLDropDownList_Key;
    window.clearTimeout(SAHLDropDownList_Timeout);
    SAHLDropDownList_Timeout = window.setTimeout( "SAHLDropDownList_doKeySearch('"+cbo+"')", 500 );
}

function SAHLDropDownList_doKeySearch(cbo)
{
    var o = document.getElementById(cbo);
    if ( o != null )
    {
        if ( o.disabled == true ) return;
        
        var i;
        var iLen = SAHLDropDownList_Key.length;
        var mCount = o.length;
        for ( i=0; i<mCount; i++ )
        {
            if ( SAHLDropDownList_Key.toLowerCase() == o.item(i).text.substr(0,iLen).toLowerCase() )
            {
                o.selectedIndex = i;
                if (typeof o.onchange == "function")
                    o.onchange();
                break;
            }
        }
    }
    
    SAHLDropDownList_Key = "";
    window.status = "Done";
}
