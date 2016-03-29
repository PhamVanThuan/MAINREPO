angular.module('capitecApp.home.calculate', [
  'ui.router'
])
.controller('CalculateCtrl', '$templateCache', function CalculateController($scope, $templateCache) {
    window.$myscope = $scope;
});

function DropDown(name) {
    var spanname = name + 'Span';

    var dropdown = document.getElementById(name);
    var span = document.getElementById(spanname);

    if (span.innerText) {
        span.innerText = dropdown[dropdown.selectedIndex].text;
    } else {
        span.textContent = dropdown[dropdown.selectedIndex].text;
    }
}