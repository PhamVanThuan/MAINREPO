<!--
	var MM_contentVersion = 6;
	var plugin = (navigator.mimeTypes && navigator.mimeTypes["application/x-shockwave-flash"]) ? navigator.mimeTypes["application/x-shockwave-flash"].enabledPlugin : 0;
	if ( plugin ) {
			var words = navigator.plugins["Shockwave Flash"].description.split(" ");
			for (var i = 0; i < words.length; ++i)
			{
			if (isNaN(parseInt(words[i])))
			continue;
			var MM_PluginVersion = words[i]; 
			}
        var MM_FlashCanPlay = MM_PluginVersion >= MM_contentVersion;
	}
	else if (navigator.userAgent && navigator.userAgent.indexOf("MSIE")>=0 
	&& (navigator.appVersion.indexOf("Win") != -1)) {
		document.write('<SCR' + 'IPT LANGUAGE=VBScript\> \n'); //FS hide this from IE4.5 Mac by splitting the tag
		document.write('on error resume next \n');
		document.write('MM_FlashCanPlay = ( IsObject(CreateObject("ShockwaveFlash.ShockwaveFlash." & MM_contentVersion)))\n');
		document.write('</SCR' + 'IPT\> \n');
	}
	if ( MM_FlashCanPlay ) {
		document.write('<OBJECT classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"');
		document.write(' codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0" ');
		document.write('WIDTH=' + swfWidth + ' HEIGHT=' + swfHeight + ' id=' + swfID + ' wmode="transparent" ALIGN="TOP">');
		document.write('<PARAM NAME=allowScriptAccess VALUE=sameDomain><PARAM NAME=movie VALUE="' + swfMovie + '"><PARAM NAME=quality VALUE=high><PARAM name=scale VALUE=noscale><PARAM name=wmode VALUE=transparent>');
		document.write('<EMBED src="' + swfMovie + '" quality=high scale="noscale" WIDTH=' + swfWidth + ' HEIGHT=' + swfHeight + ' NAME=' + swfID + ' wmode="transparent" ALIGN="TOP" TYPE="application/x-shockwave-flash" PLUGINSPAGE="https://www.macromedia.com/go/getflashplayer"></EMBED>');
		document.write('</OBJECT>');
	} else{
		document.write(noFlash);
	}
-->