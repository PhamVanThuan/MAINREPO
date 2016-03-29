'use strict';

angular.module('sahl.tools.app.home.design.file-menu', [
  'ui.router',
  'sahl.tools.app.home.design.file-menu.info',
  'sahl.tools.app.home.design.file-menu.new',
  'sahl.tools.app.home.design.file-menu.open',
  'sahl.tools.app.home.design.file-menu.print',
  'sahl.tools.app.home.design.file-menu.export',
  'sahl.tools.app.home.design.file-menu.test'
])

.config(function config($stateProvider) {
    $stateProvider.state('home.design.file-menu', {
        abstract:true,
        //url: "/file",
        templateUrl: "./app/home/design/file-menu/file-menu.tpl.html",
        controller: "DesignFileMenuCtrl",
    });
})

.controller('DesignFileMenuCtrl', ['$rootScope', '$scope', '$state', '$keyboardManager', '$documentManager', '$stateParams', '$designSurfaceManager','$modalDialogManager', '$timeout',
    function DesignFileMenuCtrl($rootScope, $scope, $state, $keyboardManager, $documentManager, $stateParams, $designSurfaceManager, $modalDialogManager, $timeout) {

    angular.element('#fileMenuView').addClass('bodyContent');

    $keyboardManager.bind('esc', function ($event) {
        $event.preventDefault();
        $scope.go_back();
    });

    $scope.go_back_no_check = function (setIsNewFalse) {
        var current = $state.current;
        var treeId = $stateParams.treeId;
        var treeVersionId = $stateParams.treeVersionId;
        var isNew = (setIsNewFalse === undefined ? $stateParams.isNew : setIsNewFalse);

        $state.go("home.design", { treeId: treeId, treeVersionId: treeVersionId, isNew: isNew });
        angular.element('#fileMenuView').removeClass('bodyContent');
        $designSurfaceManager.requestUpdate();
    }

    $scope.go_back = function (setIsNewFalse) {

        if ($state.current.name != "home.design.file-menu.test") {
            $scope.go_back_no_check();
        }
    };

    $scope.$on('$destroy', function() {
        $keyboardManager.unbind('esc');
    })

    $scope.documentInfo = function () {
        $state.go("home.design.file-menu.info");
    }

    $scope.newDocument = function () {
        $state.go("home.design.file-menu.new");
    }

    $scope.openDocument = function () {
        $state.go("home.design.file-menu.open.all");
    }

    $scope.saveDocument = function () {
        $timeout(function () {
            $documentManager.saveDocument();
            $scope.go_back(false);
        });
    }

    $scope.saveDocumentAs = function () {

        $documentManager.saveDocumentAs();
        $scope.go_back(false);
    }

    $scope.printDocument = function () {
        $state.go("home.design.file-menu.print");
    }

    $scope.exportDocument = function () {
        $state.go("home.design.file-menu.export.svg");
    }

    $scope.closeDocument = function () {
        $documentManager.closeDocument().then(function () {
        },
        function () {
            $scope.go_back();
        });
    }

    $scope.deleteDocument = function () {
        $documentManager.deleteDocument();
    }

    $scope.publish = function () {
        $documentManager.publishDocument();
    }

    $scope.defineTests = function () {
        $state.go('home.design.file-menu.test');
    }

}]);