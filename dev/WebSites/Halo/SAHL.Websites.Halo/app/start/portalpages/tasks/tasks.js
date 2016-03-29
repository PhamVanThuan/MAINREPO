'use strict';
angular.module('halo.start.portalpages.tasks', [
    'halo.start.portalpages.tasks.mytasks',
    'halo.start.portalpages.tasks.tags.mytags',
    'halo.start.portalpages.tasks.tags.mytagmanager'
])
    .config([
        '$stateProvider', function ($stateProvider) {
            $stateProvider.state('start.portalPages.tasks', {
                url: 'tasks/',
                templateUrl: 'app/start/portalPages/tasks/tasks.tpl.html',
                controller: 'TasksCtrl'
            });
        }
    ])
    .controller('TasksCtrl', [
        '$scope', function ($scope) {
            $scope.menuTabs = [{
                Name: 'myTasks',
                page: 'app/start/portalPages/tasks/mytasks/mytasks.tpl.html'
            }];

            $scope.selectTab = function (index) {
                $scope.activeIndex = index;
                $scope.visableTab = $scope.menuTabs[$scope.activeIndex].page;
            };

            $scope.selectTab(0);
            $scope.tagWindow = 'app/start/portalPages/tasks/mytags/mytags.tpl.html';
            $scope.tagManager = 'app/start/portalPages/tasks/mytags/myTagManager.tpl.html';
        }
    ]);
