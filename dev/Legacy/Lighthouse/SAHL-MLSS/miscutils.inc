<SCRIPT ID=clientEventHandlersJS LANGUAGE=javascript>
<!--
LoadMsgTime = 5;
br=navigator.appName;
function Hold(){
  setTimeout('Hold()',2000);
}
Hold();
var co=0;
function HideClicks(){
	if(co<=LoadMsgTime*50){
      co+=1;
      setTimeout("HideClicks()",20);
    }     
}
function IsClick(e){ 
    if(event.button!=1 && !(event.altKey || event.shiftKey || event.ctrlKey)){
         window.showModalDialog("Logo.asp","","dialogTop:5cm");
         return false;
    } else {
		HideClicks();
        return true;
      }
}
function IsOver(e){
 return true;
}

if (document.layers) 
    window.captureEvents(Event.MOUSEDOWN|MOUSEOVER);
    document.onmousedown=IsClick;
    window.onmouseover=IsClick;
    document.onmousedown=IsClick;
    window.onmouseover=IsClick;
//-->
</SCRIPT>