// Created 2006/05/10 - Marius Smit
// NOTE: These constants are defined in CoreBusinessObjectNavigator.cs as well!!
var CBO_TREE_NAME      = "MainTree";
var CBO_CTXTREE_NAME   = "CtxTree";
var CBO_TXT_NAV        = "CboHiddenNavUrl";
var CBO_TXT_CTX        = "CboHiddenCtxUrl";
var CBO_TXT_URLTYPE    = "CboHiddenUrlType";
var CBO_TXT_SELITEM    = "CboSelectedItem";
var CBO_TXT_SELCTXITEM = "CboSelectedCtxItem";
var CBO_TXT_STATE      = "CboNodeState";
var CBO_TXT_STATECTX   = "CboNodeCtxState";
var CBO_TXT_SUBMIT     = "CboHiddenSubmit";


function HLTree()
{
    // CBO Functions
    this.onCboRemove = onCboRemove;
    this.onCboClick = onCboClick;
    this.onCtxClick = onCtxClick;
    this.toggle = toggle;
    
    // Other Tree Functions
    this.onHLRCheck = onHLRCheck;
    
 
}

function toggle(preid, id)
{
    ul = "ul_" + id;
    img = "img_li_" + id;
    ulElement = document.getElementById(ul);
    imgElement = document.getElementById(img);
    if (ulElement && imgElement)
    {
        if (imgElement.className == "plus")
        {
            imgElement.className = "minus";
            ulElement.className = "open";
        }
        else
        {           
            imgElement.className = "plus";
            ulElement.className = "closed";
        }
    }
    
    storeTreeState(preid);
}

function onCboClick(preid, postBackText)
{
    setCboSettings(preid, postBackText);
    storeTreeState(preid);
    doSubmitClick("CBO", preid);
}

function onCboRemove(preid, postBackText)
{
    setCboSettings(preid, postBackText);
    storeTreeState(preid);
    doSubmitClick("REMOVE", preid);
}

function onCtxClick(preid, postBackText)
{
    setCtxSettings(preid, postBackText);
    storeTreeState(preid);
    doSubmitClick("CTX", preid);
}

function setTimerCookie()
{
    var dt = new Date();
    SetCookie('SAHLReqTime',dt.getTime());
}

function doSubmitClick(ctx, preid)
{
    storeTreeState(preid);
    var oSubmit = document.getElementById(preid+CBO_TXT_SUBMIT);
    oSubmit.value = ctx;
    oSubmit.click();
}

function setCboSettings(preid, postBackText)
{
    var oTxt = document.getElementById(preid+CBO_TXT_SELITEM);
    oTxt.value = postBackText;
}

function setCtxSettings(preid, postBackText)
{
    var oTxt = document.getElementById(preid+CBO_TXT_SELCTXITEM);
    oTxt.value = postBackText;
}


function storeTreeState(preid)
{
    var aInput=document.getElementsByTagName("ul");
    var txtCboState="";
    var txtCtxState="";
    
    for(i = 0; i < aInput.length; ++i)
    {
        if (aInput[i].id.substr(0,6) == "ul_cbo")
        {
            if (aInput[i].className == "closed")
                txtCboState +=  "+" + aInput[i].id + ";";      
            else
                txtCboState +=  "-" + aInput[i].id + ";";      
        }
        if (aInput[i].id.substr(0,6) == "ul_ctx")
        {
            if (aInput[i].className == "closed")
                txtCtxState +=  "+" + aInput[i].id + ";";      
            else
                txtCtxState +=  "-" + aInput[i].id + ";";      
        }
    }
    
    var objCboState = document.getElementById(preid+CBO_TXT_STATE)
    var objCtxState = document.getElementById(preid+CBO_TXT_STATECTX)
    objCboState.value = txtCboState;
    objCtxState.value = txtCtxState;
}


function onHLRCheck(r,id,bVal)
{
    if ( this.oRow == null )
        this.oRow = document.getElementsByName(id+"_ROW");
        
    if ( this.oCheck == null )
        this.oCheck = document.getElementsByName(id+"_CHECK");
    
    var iC = 0;
    var iM = this.oRow.length;
    
    var bCheckIt;
    if ( bVal == undefined )
    {
        if ( arValue[r] == "_" )
            bCheckIt = true;
        else
            bCheckIt = false;
    }
    else
    {
        bCheckIt = bVal;
    }

    if ( bCheckIt )
    {
        this.oCheck[r].src = "../../Images/TCheck.gif";
        arValue[r] = ""+r;

        // Recurse and select the parents.
        for ( iC=r; iC>=0; iC-- )
        {
            if ( arSubRows[iC].indexOf("["+r+"];") >= 0 )
            {
                this.oCheck[iC].src = "../../Images/TCheck.gif";
                arValue[iC] = ""+r;
                // Recurse
                if ( r == 0 ) return;
                onHLRCheck(iC,id,true);
            }
        }
    }
    else
    {
        this.oCheck[r].src = "../../Images/TCheckClean.gif";
        arValue[r] = "_";
        
        // Deselect the children.
        for ( iC=r; iC<iM; iC++ )
        {
            if ( arDada[iC] == ""+r )
            {
                this.oCheck[iC].src = "../../Images/TCheckClean.gif";
                arValue[iC] = "_";
                // Recurse
                if ( r < 0 ) return;
                onHLRCheck(iC,id,false);
            }
        }
    }
}

function setTreeChecks(id)
{
    var s = "";
    var iC = 0;
    var iM = arValue.length;
    for ( iC=0; iC<iM; iC++ )
    {
        s = s + "[" + arValue[iC] + "];";
    }
    
    var oTxt = document.getElementById(id+"_VALUES");
    if ( oTxt != null )
        oTxt.value = s;
}