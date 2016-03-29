    
/*----------------------------------------------------------------------------\
|                               JSBalloon                                     |
|-----------------------------------------------------------------------------|
|                   Created by Arkady (Alex) Lesniara                         |
|                           (arkady@lesniara.com)                             |
|-----------------------------------------------------------------------------|
|                  Copyright (c) 2005 Arkady Lesniara                         |
|-----------------------------------------------------------------------------|
| This software is provided "as is", without warranty of any kind, express or |
| implied, including  but not limited  to the warranties of  merchantability, |
| fitness for a particular purpose and noninfringement. In no event shall the |
| authors or  copyright  holders be  liable for any claim,  damages or  other |
| liability, whether  in an  action of  contract, tort  or otherwise, arising |
| from,  out of  or in  connection with  the software or  the  use  or  other |
| dealings in the software.                                                   |
\----------------------------------------------------------------------------*/

var ie = document.all ? true : false
/*	Class: JSBalloon
	Provides a flexible, encapsulated way to implement a passive feedback mechanism
	in a DHTML environment. Define and initialize this object globally, otherwise it will create a new instance 
	each time you call it's constructor. Set up the object with an object array passed to the constructor or, once instantiated,
	with properties. See <Usage> for more.
*/
function JSBalloon()
{
	var tmrBalloonPopup;
	
	var blbWidth=200;;
	var titleFontStyle='font-family: MS Sans Serif;font-weight: bold; font-size:10pt;';
	var messageFontStyle='font-family: MS Sans Serif\; font-size:10pt\;';
	var footerFontStyle='font-family: MS Sans Serif\; font-size:10pt\;';
	var transShow=true;
	var transHide=false;
	var transShowFilter='progid:DXImageTransform.Microsoft.Fade(Overlap=1.00)';
	var transHideFilter='progid:DXImageTransform.Microsoft.Fade(Overlap=1.00)';
	var autoHide=true;
	var autoHideInterval=20000; // 20 sec
	var autoAway=true;
	var showCloseBox=false;
	
	if(JSBalloon.arguments.length>0)
	{
		var oArg=JSBalloon.arguments[0];
		
		if(oArg.width!=null)
		{
			blbWidth=oArg.width;
		}
		
		if(oArg.titleFontStyle!=null)
		{
			titleFontStyle=oArg.titleFontStyle;
		}
		
		if(oArg.messageFontStyle!=null)
		{
			messageFontStyle=oArg.messageFontStyle;
		}
		
		if(oArg.footerFontStyle!=null)
		{
			footerFontStyle=oArg.footerFontStyle;
		}
		
		if(oArg.transShow!=null)
		{
			transShow=oArg.transShow;
		}
		
		if(oArg.transHide!=null)
		{
			transHide=oArg.transHide;
		}
		
		if(oArg.transShowFilter!=null)
		{
			transShowFilter=oArg.transShowFilter;
		}
		
		if(oArg.transHideFilter!=null)
		{
			transHideFilter=oArg.transHideFilter;
		}
		
		if(oArg.autoHide!=null)
		{
			autoHide=oArg.autoHide;
		}
		
		if(oArg.autoHideInterval!=null)
		{
			autoHideInterval=oArg.autoHideInterval;
		}
		
		if(oArg.autoAway!=null)
		{
			autoAway=oArg.autoAway;
		}
		
		if(oArg.showCloseBox!=null)
		{
			showCloseBox=oArg.showCloseBox;
		}
	}
	
	this.titleFontStyle=titleFontStyle;
	this.messageFontStyle=messageFontStyle;
	this.footerFontStyle=footerFontStyle;
	this.transShowFilter=transShowFilter;
	this.transHideFilter=transHideFilter;
	this.transShow=transShow;
	this.transHide=transHide;
	this.autoHide=autoHide;
	this.autoHideInterval=autoHideInterval;
	this.autoAway=autoAway;
	this.width=blbWidth;
	this.showCloseBox=showCloseBox;
	
	var childID;
	
	// Constructor
	var balloonDIV = document.createElement("DIV");
	balloonDIV.style.width=String(blbWidth);
	balloonDIV.style.position="absolute";
	balloonDIV.style.visibility="hidden";
	balloonDIV.style.filter=transShowFilter+' '+transHideFilter;
	balloonDIV.style.zIndex=2001;
	this.balloon=balloonDIV;

	// Pulic Methods
	this.Show=Show;
	this.Hide=Hide;
	
	function Show()
	{
		var title;
		var message='';
		var icon='';
		var footer='';
		var btop=0, bleft=0;
		var atop=0, aleft=0;
		var anchor;
		var direction='SE';

		// Updates
		blbWidth=String(this.width);
		titleFontStyle=this.titleFontStyle;
		messageFontStyle=this.messageFontStyle;
		footerFontStyle=this.footerFontStyle;
		transShowFilter=this.transShowFilter;
		transHideFilter=this.transHideFilter;
		transShow=this.transShow;
		transHide=this.transHide;
		autoHide=this.autoHide;
		autoHideInterval=this.autoHideInterval;
		autoAway=this.autoAway;
		
		if(Show.arguments.length>0)
		{
			var oArg=Show.arguments[0];
			
			if(oArg.title!=null)
			{
				title=oArg.title;
			}
			
			if(oArg.message!=null)
			{
				message=oArg.message;
			}
			
			if(oArg.icon!=null)
			{
				icon=oArg.icon;
				
				switch(icon)
				{
					case 'Exclaim':
						icon = '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.stoperror.ico") %>';
						SoundFx = 'Exclaim';
						break;
						
					case 'Stop':
						icon = '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.stop.ico") %>';
						SoundFx = 'Stop';
						break;
						
					case 'Info':
						icon = '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.info.ico") %>';
						SoundFx = 'Info';
						break;
					
					case 'Help':
						icon = '<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.help.ico") %>';
						SoundFx = 'Info';
						break;
						
					default:
						SoundFx = 'Info';
				}
			}

			if(oArg.footer!=null)
			{
				footer=oArg.footer;
			}
			
			if(oArg.top!=null)
			{
				btop=oArg.top;
			}
			
			if(oArg.left!=null)
			{
				bleft=oArg.left;
			}
			
			if(oArg.anchor!=null)
			{
				anchor=oArg.anchor;
				atop=getTop(anchor);
				aleft=getLeft(anchor);
			}
		}
		
		// Figure out the best direction for the pointer
		
		// Assume SE
		var ret=balloonInfrastructure(balloonBody(	title, 
													icon, 
													message, 
													footer, 
													this.titleFontStyle,
													this.messageFontStyle,
													this.footerFontStyle,
													this.showCloseBox), direction);
		
		// Populate the contents
		balloonDIV.innerHTML=ret;
		
		// changes for firefox var btnClose=balloonDIV.children[0].children[0].children[1].children[0].children[0].children[0].children[0].children[2].children[0];
        var btnClose = ie ? balloonDIV.children[0].children[0].children[1].children[0].children[0].children[0].children[0].children[2].children[0] : balloonDIV.childNodes[0].childNodes[1].childNodes[2].childNodes[1].childNodes[1].childNodes[1].childNodes[0].childNodes[5].childNodes[0];		
		btnClose.onclick=this.Hide;
		
		// check if the object is already initialized
		if(typeof(childID)!='object')
		{
			childID=document.body.appendChild(balloonDIV);	
		}
		
		if(anchor!=null)
		{

			balloonDIV.style.left = aleft+bleft;
			balloonDIV.style.top = (atop-balloonDIV.offsetHeight)+btop;
		}
		else
		{
			balloonDIV.style.left = bleft;
			balloonDIV.style.top = btop;		
		}
		
		
		var bAdjustedLeft=parseInt(balloonDIV.style.left, 10);
		var showSE;

	
		if(document.body.offsetWidth < (bAdjustedLeft+balloonDIV.offsetWidth+20))
		{		
			direction='SW';
			
			ret=balloonInfrastructure(balloonBody(	title, 
													icon, 
													message, 
													footer, 
													this.titleFontStyle,
													this.messageFontStyle,
													this.footerFontStyle,
													this.showCloseBox), direction);
			balloonDIV.innerHTML=ret;

			balloonDIV.style.left = bAdjustedLeft-balloonDIV.offsetWidth+20;
			showSE=false;
		}
		else
		{
			direction='SE';
			showSE=true;
		} 
		
		if(parseInt(balloonDIV.style.top, 10)<0)
		{
			if(showSE)
			{
				direction='NE';
				ret=balloonInfrastructure(balloonBody(	title, 
													icon, 
													message, 
													footer, 
													this.titleFontStyle,
													this.messageFontStyle,
													this.footerFontStyle,
													this.showCloseBox), direction);
				balloonDIV.innerHTML=ret;
			}
			else
			{
				direction='NW';
				ret=balloonInfrastructure(balloonBody(	title, 
													icon, 
													message, 
													footer, 
													this.titleFontStyle,
													this.messageFontStyle,
													this.footerFontStyle,
													this.showCloseBox), direction);
				balloonDIV.innerHTML=ret;
			}
			balloonDIV.style.top = parseInt(balloonDIV.style.top, 10)+balloonDIV.offsetHeight;
			if(anchor!=null)
			{
				balloonDIV.style.top = parseInt(balloonDIV.style.top, 10)+anchor.offsetHeight
			}
		}			
		
		if(this.showCloseBox)
		{
			if(direction=='SE' || direction=='SW')
			{
				// changes for firefox btnClose=balloonDIV.children[0].children[0].children[1].children[0].children[0].children[0].children[0].children[2].children[0];
                btnClose = ie ? balloonDIV.children[0].children[0].children[1].children[0].children[0].children[0].children[0].children[2].children[0] : balloonDIV.childNodes[0].childNodes[1].childNodes[2].childNodes[1].childNodes[1].childNodes[1].childNodes[0].childNodes[5].childNodes[0];				
			}
			else
			{
				// changes for firefox btnClose=balloonDIV.children[0].children[0].children[2].children[0].children[0].children[0].children[0].children[2].children[0];
                btnClose = ie ? balloonDIV.children[0].children[0].children[2].children[0].children[0].children[0].children[0].children[2].children[0] : balloonDIV.childNodes[0].childNodes[1].childNodes[4].childNodes[1].childNodes[1].childNodes[1].childNodes[0].childNodes[5].childNodes[0];				
			}
			btnClose.onclick=this.Hide;
			//btnClose.removed=this.Hide;
		}
		
		// Adjust all scrollers
		var scrollOffsets=ScrollOffsets(anchor);
		balloonDIV.style.top=parseInt(balloonDIV.style.top, 10)-scrollOffsets[0];
		balloonDIV.style.left=parseInt(balloonDIV.style.left, 10)-scrollOffsets[1];
		
		// Hide any overlapping selects
		SuppressSelects();
		
		// Show balloon
		if(this.transShow && ie)
		{
			balloonDIV.style.filter=this.transShowFilter+' '+this.transHideFilter;
			if(balloonDIV.filters(0).status==2 || balloonDIV.filters(0).status==1)
			{
				balloonDIV.filters(0).Stop();
			}
			balloonDIV.filters(0).Apply();
			balloonDIV.style.visibility='visible';
			balloonDIV.filters(0).Play();
		}
		else
		{
			balloonDIV.style.visibility='visible';
		}
					
		// Init autohide if true
		if(this.autoHide)
		{
			clearTimeout(this.tmrBalloonPopup);
			transHide=this.transHide;
			this.tmrBalloonPopup=setTimeout(this.Hide, this.autoHideInterval);
		}
		
		if(this.autoAway)
		{
			balloonDIV.onmouseover=Hide;
		}
		else
		{
			balloonDIV.onmouseover='';
		}
	}

	function Hide()
	{
		if(balloonDIV.style.visibility=='hidden')
		{
			return;
		}

		if(transHide && ie)
		{
			if(balloonDIV.filters(1).status==2 || balloonDIV.filters(1).status==1)
			{
				balloonDIV.filters(1).Stop();
			}
			balloonDIV.filters(1).Apply();
			balloonDIV.style.visibility='hidden';
			balloonDIV.filters(1).Play();
		}
		else
		{
			balloonDIV.style.visibility='hidden';
		}	
		
		RestoreSelects();
	}
	
	// Private

	function SuppressSelects()
	{
		var selObjects=document.getElementsByTagName("SELECT");
		
		for(var i=0;i<selObjects.length;i++)
		{	
			if(selObjects[i].balloonMember!='true')
			{
				if(selObjects[i].style.visibility=='visible' || selObjects[i].style.visibility=='')
				{
					if(ObjectOverlap(balloonDIV, selObjects[i]))
					{
						selObjects[i].style.visibility='hidden';
						// Mark as ballonhidden
						selObjects[i].baloonHidden=true;
					}
				}
			}
		}
	}
	
	function RestoreSelects()
	{
		var selObjects=document.getElementsByTagName("SELECT");
		
		for(var i=0;i<selObjects.length;i++)
		{	
			if(selObjects[i].baloonHidden)
			{
				selObjects[i].style.visibility='visible';
				// Mark as ballonhidden
				selObjects[i].baloonHidden=false;
			}
		}
	}
	
	function ObjectOverlap(obj1, obj2)
	{
		var obj1TopX = getLeft(obj1);
		var obj1TopY = getTop(obj1);
		var obj1BottomX = getLeft(obj1)+obj1.offsetWidth;
		var obj1BottomY = getTop(obj1)+obj1.offsetHeight;
		
		var obj2TopX = getLeft(obj2);
		var obj2TopY = getTop(obj2);
		var obj2BottomX = getLeft(obj2)+obj2.offsetWidth;
		var obj2BottomY = getTop(obj2)+obj2.offsetHeight;
		
		var overlapOnX = (obj1TopX < obj2BottomX && obj2TopX < obj1BottomX);
		var overlapOnY = (obj1TopY < obj2BottomY && obj2TopY < obj1BottomY);
		
		return (overlapOnX && overlapOnY);
	}

	//Positioning functions
	function getObjLeft(anObject) 
	{
	  return(anObject.offsetParent ? (getObjLeft(anObject.offsetParent) + anObject.offsetLeft) : anObject.offsetLeft);
	}
		 
	function getObjTop(anObject) 
	{
	  return(anObject.offsetParent ? (getObjTop(anObject.offsetParent) + anObject.offsetTop) : anObject.offsetTop); 
	}
		 
		 
	function getLeft(anObject) 
	{
	    return(getObjLeft(anObject));
	}
		 
	function getTop(anObject) 
	{
	    return(getObjTop(anObject));
	}
	
	function ScrollOffsets(anchor)
	{
		var aryScrolls = new Array(0,0);

		try
		{
			var parentElem=anchor.parentElement;

			while(parentElem!=null)
			{
				if(parentElem.scrollTop!=null)
				{
					aryScrolls[0]+=parseInt(parentElem.scrollTop, 10);
					aryScrolls[1]+=parseInt(parentElem.scrollLeft, 10);
				}
	
				parentElem=parentElem.parentElement;
			}
		}
		catch(ex)
		{
			// continue
		}
		return aryScrolls;
	}

	// Rendering functions
	function balloonInfrastructure(body, direction)
	{
		var ret;
		
		switch(direction)
		{
			case 'SE':
				// South East	
				ret ='<table class="JSBalloon" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse" >'+
					'  <tr>'+
					'    <td height="1" width="10">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftTop.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" width=100% style="border-top:1px solid #999999; border-left-width:1; border-right-width:1; border-bottom-width:1; background-color:#FFFFEA" colspan="4"></td>'+
					'    <td height="7"  width="10">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightTop.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td valign=top colspan="6" style="border-left: 1px solid #999999; border-right: 1px solid #999999; background-color: #FFFFEA">'+
					body +
					'    </td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td width="10" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftBottom.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" style="background-color: #FFFFEA" colspan="4" width="280"></td>'+
					'    <td width="10" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightBottom.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td width="10" height="10"></td>'+
					'    <td width="1" style="border-top: 1px solid #999999; padding-left: 4; padding-right: 4; padding-top: 1; padding-bottom: 1" height="10"></td>'+
					'    <td  height="10">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.aSouthEast.gif") %>" width="67" height="18"></td>'+
					'    <td width=100% height="10" style="border-top: 1px solid #999999; padding-left: 4; padding-right: 4; padding-top: 1; padding-bottom: 1"></td>'+
					'    <td width="70" height="10" style="border-top: 1px solid #999999; padding-left: 4; padding-right: 4; padding-top: 1; padding-bottom: 1"></td>'+
					'    <td width="10" height="10"></td>'+
					'  </tr>'+
					'</table>'
					break;

			case 'SW':					
				// South West
				ret ='<table class="JSBalloon" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111" id="AutoNumber1" >'+
					'  <tr>'+
					'    <td height="1" width="10">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftTop.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" width=179 style="border-top:1px solid #999999; border-left-width:1; border-right-width:1; border-bottom-width:1; background-color:#FFFFEA" colspan="4"></td>'+
					'    <td height="7"  width="11">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightTop.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td valign=top colspan="6" style="border-left: 1px solid #999999; border-right: 1px solid #999999;  background-color: #FFFFEA"">'+
					body +
					'    </td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td width="10" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftBottom.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" style="background-color: #FFFFEA" colspan="4" width="179"></td>'+
					'    <td width="11" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightBottom.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td width="10" height="10"></td>'+
					'    <td width="70" style="border-top: 1px solid #999999; border-left-width:1; border-right-width:1; border-bottom-width:1" height="10"></td>'+
					'    <td  height="10" style="border-left-width: 1; border-right-width: 1; border-top: 1px solid #999999; border-bottom-width: 1" width="100%">'+
					'    </td>'+
					'    <td  align="right">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.aSouthWest.gif") %>" width="67" height="18"></td>'+
					'    <td width="1" height="10" style="border-top: 1px solid #999999;"></td>'+
					'    <td width="10" height="10"></td>'+
					'  </tr>'+
					'</table>'
					break;
					
			case 'NE':	
					// North East
					ret ='<table class="JSBalloon" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111" id="AutoNumber1" >'+
					'   <tr>'+
					'    <td width="10" height="9"></td>'+
					'    <td width="1" style="border-bottom:1px solid #999999; " height="9"></td>'+
					'    <td  height="9" valign="bottom">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.aNorthEast.gif") %>" width="67" height="18"></td>'+
					'    <td width=100% height="9" style="border-bottom:1px solid #999999; "></td>'+
					'    <td width="70" height="9" style="border-bottom:1px solid #999999;"></td>'+
					'    <td width="10" height="9"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td height="1" width="10">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftTop.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" width=100% colspan="4" bgcolor="#FFFFEA"></td>'+
					'    <td height="7"  width="10">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightTop.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td valign=top colspan="6" style="border-left: 1px solid #999999; border-right: 1px solid #999999; background-color: #FFFFEA">'+
					body +
					'    </td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td width="10" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftBottom.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" style="border-bottom:1px solid #999999; border-left-width:1; border-right-width:1; border-top-width:1" colspan="4" width="280" bgcolor="#FFFFEA"></td>'+
					'    <td width="10" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightBottom.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'</table>'
					break;
					
			case 'NW':	
					// North West			
					ret ='<table class="JSBalloon" border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111" id="AutoNumber1" >'+
					'  <tr>'+
					'    <td width="10" height="10"></td>'+
					'    <td width="70" style="border-bottom:1px solid #999999;  border-left-width:1; border-right-width:1; " height="10"></td>'+
					'    <td  height="10" style="border-bottom:1px solid #999999; border-left-width: 1; border-right-width: 1; " width="100%">'+
					'    </td>'+
					'    <td  align="right" valign="bottom">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.aNorthWest.gif") %>" width="67" height="18"></td>'+
					'    <td width="1" height="10" style="border-bottom:1px solid #999999;"></td>'+
					'    <td width="10" height="10"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td height="1" width="10">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftTop.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" width=179 colspan="4" bgcolor="#FFFFEA"></td>'+
					'    <td height="7"  width="11">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightTop.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td valign=top colspan="6" style="border-left: 1px solid #999999; border-right: 1px solid #999999;  background-color: #FFFFEA">'+
					body +
					'    </td>'+
					'  </tr>'+
					'  <tr>'+
					'    <td width="10" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cLeftBottom.gif") %>" width="10" height="10"></td>'+
					'    <td height="7" style="border-bottom:1px solid #999999; border-left-width:1; border-right-width:1; border-top-width:1" colspan="4" width="179" bgcolor="#FFFFEA"></td>'+
					'    <td width="11" height="7">'+
					'    <img border="0" src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.cRightBottom.gif") %>" width="10" height="10"></td>'+
					'  </tr>'+
					'</table>'
					break;
		}
		
		
		return ret;
	}
	
	function balloonBody(title, icon, body, footer, titleFontStyle, 
						messageFontStyle, footerFontStyle,
						showCloseBox)
	{
		var imgShow='none';
		var iconTitle='';
		var ballonBody=body;
		var imgClose='none';
		var headerVisible='block';
		var offsetParent="-7";
		
		if(title!=undefined)
		{
			iconTitle=title;
		}
		
		if(showCloseBox)
		{
			imgClose='block';
		}
		else
		{
			imgClose='none';
		}
		
		if(icon != '')
		{
			imgShow='block';
		}
		else
		{
			imgShow='none';
		}
		
		if(imgShow=='none' && imgClose=='none' && iconTitle=='')
		{
			headerVisible='none';
			offsetParent="0";
		}
		else
		{
			headerVisible='block';
			offsetParent="-7";
		}

		return '    <table border="0" cellpadding="3" cellspacing="0" style="cursor:default;border-collapse: collapse; position:relative; top: '+offsetParent+';left:3" width="100%">' + 
				'      <tr style="display:'+headerVisible+'">' + 
				'        <td id="BIcon" width="3%" align=left><img id=BIcon src="'+icon+'" style="display:'+imgShow+'"></td>' + 
				'        <td id="BTitle" UNSELECTABLE="on" width="90%" style="'+titleFontStyle+'" align=left>'+iconTitle+'</td>' + 
				'        <td id="BClose" width="3%" valign=top align=right><img src="<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.close.jpg") %>" style="position:relative; top: 4;left:-5;display:'+imgClose+'" onmouseover="this.src=\'<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.closeActive.jpg") %>\'" onmouseout="this.src=\'<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.close.jpg") %>\'" onmouseup="this.src=\'<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.closeActive.jpg") %>\'" onmousedown="this.src=\'<%= WebResource("SAHL.Common.Web.UI.Controls.Resources.closeDown.jpg") %>\'" title="Close">&nbsp;</td>' + 
				'      </tr>' + 
				'      <tr>' + 
				'        <td id="BBody" UNSELECTABLE="on" style="'+messageFontStyle+'" width="100%" colspan="3">' + ballonBody +'</td>' + 
				'      </tr>' + 
				'       <tr>' + 
				'        <td id="BFooter" UNSELECTABLE="on" style="'+footerFontStyle+'" width="100%" colspan="3">' + footer +'</td>' + 
				'      </tr>' + 
				'    </table>'
	}
}

/*
	Section: Usage
		Examples of the object instantiation.
	
	Examples:
	
	(start code)
	var bl=new JSBalloon({ width:300});
	var b2=new JSBalloon();
	var b3=new JSBalloon({	width:150, 
							autoAway:false, 
							autoHide:false,
							showCloseBox:true});
							
	(end)
	
*/
