function ShowPopup(message, height, width) {
	popup.style.height = height;
	popup.style.width = width;	
	popup.style.top = event.clientY+15;
	popup.style.left = event.clientX+5;
	
	var content = "<table border=0 cellpadding=4 cellspacing=0 style='font-family:arial;font-size:13px'><tr><td>" + 
					message + 
					"</td></tr></table>";
	
	document.all['popup'].innerHTML = content
	document.all['popup'].style.visibility = "visible"
	
	
}

function InitializePopup() {
	document.body.insertAdjacentHTML("BeforeEnd",'<DIV ID="popup" STYLE="z-index:200;position:absolute;width:100;height:100;left:0;top:0;visibility:hidden;background:lightyellow;border-style:solid;border-width:1;border-color:black"></DIV>')
}

function HidePopup() {
	document.all['popup'].style.visibility = "hidden"
}

function RepositionPopup() {
    if (document.all['popup']) {
	    if (document.all['popup'].style.visibility == "visible") {
		    popup.style.top = event.clientY+15;
		    popup.style.left = event.clientX+5;
	    }
	}
}
document.onmousemove = RepositionPopup;
document.onmouseout = HidePopup;
