function SAHLGridView_selectGridRow(row,fixedheader,hiddenTxt)
{
    // Set all the rows back to normal.
    var gridTable = SAHLGridView_getParentByTag(row,"TABLE");

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

// loops up the control hierarchy till it finds an element with tag pTagName 
function SAHLGridView_getParentByTag(el, pTagName)
{
    if (el == null) return null;
    else if (el.nodeType == 1 && el.tagName.toLowerCase() == pTagName.toLowerCase())	// Gecko bug, supposed to be uppercase
        return el;
    else
        return SAHLGridView_getParentByTag(el.parentNode, pTagName);
}

function SAHLGridView_SelectAllCheckboxes(spanChk) // added by Craig Fraser  - used to select all checkboxes in a gridview checkbox column
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

