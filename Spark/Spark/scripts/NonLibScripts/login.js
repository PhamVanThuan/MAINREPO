/*function to check userid & password*/
function login(form) {
    /*the following code checkes whether the entered userid and password are matching*/
    if (form.username.value == "Suhail" && form.password.value == "Suhail01" || form.username.value == "Lesley" && form.password.value == "Lesley01" || form.username.value == "Viresh" && form.password.value == "Viresh01" || form.username.value == "Vishav" && form.password.value == "Vishav01")
    {
        window.open('schedule.html?enableripple=cordova-3.0.0-NexusGalaxy') /*opens the target page while Id & password matches*/
    }
    else
    {
        /*alert("The Password or Username Does Not Match!!!")displays error message*/
        document.getElementById('loginerror').innerHTML = "The Password or Username are invalid.";
    }
    
}

function closeWin(form) {

}