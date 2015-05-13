'use strict'

function validateForm() {
    var x = document.forms["angularApp"][MyCalc].value;
    if (x == null || x == "" || x<=0 ) {
        alert("Invalid input, it cannot equal to or be less than 0");
        return false;
    }
}