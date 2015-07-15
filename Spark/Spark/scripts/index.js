// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
(function () {
    "use strict";

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener('pause', onPause.bind( this ), false );
        document.addEventListener('resume', onResume.bind(this), false);
        document.addEventListener("batterycritical", onBatteryCritical, false);

        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here.
    };

    function onBatteryCritical(info) {
        // Handle the battery critical event
        alert("Battery Level Critical " + info.level + "%\nRecharge Soon!");
        onPause(" Due to a low battery.");
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here. Handle the on pause event
        alert("Application has been paused "+onPause());
        setTimeout(function () {
            // TODO: do your thing!
        }, 0);
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here. Handle the resume event
    };

} )();