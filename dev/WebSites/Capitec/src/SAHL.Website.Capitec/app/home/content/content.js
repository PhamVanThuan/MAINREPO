'use strict';

angular.module('capitecApp.home.content', [
  'ui.router', 
  'capitecApp.home.apply',
  'capitecApp.home.calculate',
  'capitecApp.home.track'
])

.config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state('home.content', {
        abstract: true,
        templateUrl: 'content.tpl.html'
    });
}]);