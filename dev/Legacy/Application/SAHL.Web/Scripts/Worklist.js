// Created 2006/12/05 - Donald Massyn
// Scripting functions for the Worklist page
var GRID_NAME      = "MainTree";
var EXP_STATE_ID   = "GridState";

function WorkList()
{
    this.toggle = toggle;
    this.highlightRow = highlightRow;
    this.unhighlightRow = unhighlightRow;
}


function toggle(tableid)
{
    tbl = document.getElementById(tableid);
    if (tbl)
    {
        if (tbl.style.display == "inline")
            tbl.style.display = "none";
        else
            tbl.style.display = "inline";
    }
    
    storeExpandedState();
}

function storeExpandedState()
{
    var aInput=document.getElementsByTagName("table");
    var txtExpandedState="";
    
    for(i = 0; i < aInput.length; ++i)
    {
        if (aInput[i].id.substr(0,6) == "wlgrid")
        {
            if (aInput[i].style.display == "inline")
                txtExpandedState +=  "-" + aInput[i].id + ";";      
            else
                txtExpandedState +=  "+" + aInput[i].id + ";";      
        }
    }
    
    var objExpandedState = document.getElementById("ctl00$Main$" +EXP_STATE_ID);
    objExpandedState.value = txtExpandedState;
}

function highlightRow(row)
{
    row.style.cursor='pointer';
    row.style.backgroundColor = 'lightgrey';
}

function unhighlightRow(row, defColour)
{
    row.style.cursor='default';
    row.style.backgroundColor = defColour;
}