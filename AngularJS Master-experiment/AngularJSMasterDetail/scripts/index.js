
(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );
        
        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
        window.onresize = resizeListIOS.bind(this, false);
        resizeListIOS();
    };

    function onPause() {
        var now = new Date();
        var exitTime = now.getTime();
        while (true) {
            now = new Date();
            if (now.getTime() > exitTime)
                MyCalculator.innerText = "PAUSED";
            return;
        }
    };

    function onResume() {
        document.addEventListener('resume', onResume.bind(this), false);
        alert("Welcome back...");
    };

    function resizeListIOS() {

        var width = isNaN(window.innerWidth) ? window.clientWidth : window.innerWidth;
        var height = isNaN(window.innerHeight) ? window.clientHeight : window.innerHeight;

        var elem = document.querySelector(".main-list");

        if (isVHPartiallySupported == true) {
            // landscape
            if (width > height) {
                elem.style.height = (Math.round(height * .9)) + "px";
            }
            // portrait
            else {
                elem.style.height = (Math.round(height * .15)) + "px";
            }
        }
    }

} )();