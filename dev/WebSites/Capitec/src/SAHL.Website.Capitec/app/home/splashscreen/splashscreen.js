angular.module('capitecApp.home.splashscreen', [
  'ui.router',
  'capitecApp.home.still-looking',
  'capitecApp.home.apply',
  'capitecApp.config'
])

.config(['$stateProvider', function ($stateProvider, $config) {
    $stateProvider.state('home.splashscreen', {
        url: '/',
        templateUrl: 'splashscreen.tpl.html',
        controller: 'SplashScreenCtrl',
        data: { title: 'Home' }
    });
}])

.controller('SplashScreenCtrl', ['$scope', '$config', '$templateCache', function SplashScreenController($scope, $config, $templateCache) {
    $scope.CapitecAppVersion = '';
    // validate the version number
    if ($config.CapitecAppVersion !== undefined 
        && $config.CapitecAppVersion !== '0.0.0.0') {
        var versionNumberArray = $config.CapitecAppVersion.split(".");
        if (versionNumberArray.length === 4 &&
            !isNaN(versionNumberArray[0]) &&
            !isNaN(versionNumberArray[1]) &&
            !isNaN(versionNumberArray[2]) &&
            !isNaN(versionNumberArray[3])) {
            $scope.CapitecAppVersion = 'ver. ' + $config.CapitecAppVersion;
        }
    }
}]);
