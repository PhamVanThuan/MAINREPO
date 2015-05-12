function validateForm() {
    var x = document.forms["angularApp"][MyCalc].value;
    if (x == null || x == "" || x<=0 ) {
        alert("Cannot equal to or be less than 0");
        alert("Cannot contain alpha characters");
        return false;
    }
}