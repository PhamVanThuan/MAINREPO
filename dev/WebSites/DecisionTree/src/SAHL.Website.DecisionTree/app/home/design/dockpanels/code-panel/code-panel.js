'use strict';
angular.module('sahl.tools.app.home.design.dockpanels.code-panel', [
	'sahl.tools.app.services'
])
.controller('CodePanelCtrl', ['$rootScope', '$scope', '$codeMirrorService', '$codemirrorVariablesService', '$enumerationDataManager', '$variableDataManager', '$eventAggregatorService', '$eventDefinitions', 'messageDataManager', '$documentManager', '$rubyCodeValidatorServices',
function CodePanelController($rootScope, $scope, $codeMirrorService, $codemirrorVariablesService, enumerationDataManager, variableDataManager, $eventAggregatorService, $eventDefinitions, messageDataManager, $documentManager, $rubyCodeValidatorServices) {
    var currentWatcher = null;

    $scope.AttachCodeEditor = function () {
        if (!$rootScope.document)
            return;
        var treeVariables = $codemirrorVariablesService.loadData();
        $codeMirrorService.initEditor(treeVariables);
        $codeMirrorService.attachEditor(document.getElementById("codeEditor"));
        $codeMirrorService.refresh();
    }

    $scope.showSelectMessage = function () {
        if ($rootScope.document && $rootScope.document.selectionManager && $rootScope.document.selectionManager.current !== undefined) {
            return $rootScope.document.selectionManager.current.type === 'node' && !($rootScope.document.selectionManager.current.data.category === 'Start' || $rootScope.document.selectionManager.current.data.category === 'End' || $rootScope.document.selectionManager.current.data.category === 'SubTree');
        }
        else {
            return false;
        }
    }

    function initCodePanel() {
        $scope.AttachCodeEditor();
        currentWatcher = $scope.$watch('document.selectionManager.current', function () {
            if (!$rootScope.document || !$rootScope.document.selectionManager || !$rootScope.document.selectionManager.current || $rootScope.document.selectionManager.current.type !== 'node' || $rootScope.document.isReadOnly == true) {
                $codeMirrorService.setReadOnly('nocursor');
            }
            $codeMirrorService.refresh();
        });
    }

    function tearDownCodePanel() {
        if (currentWatcher != null) {
            currentWatcher();
            currentWatcher = null;
        }
        $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
        $eventAggregatorService.unsubscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);
        $codeMirrorService.detachEditor();
    }

    var eventHandlers = {
        onDocumentLoaded: function () {
            initCodePanel();
        },

        onDocumentUnloaded: function (unloadedDocument) {
            tearDownCodePanel();
        }
    }
    if ($rootScope.document) {
        initCodePanel();
    }

    $eventAggregatorService.subscribe($eventDefinitions.onDocumentLoaded, eventHandlers.onDocumentLoaded);
    $eventAggregatorService.subscribe($eventDefinitions.onDocumentUnloaded, eventHandlers.onDocumentUnloaded);

    $scope.$on('destroy', function () {
        tearDownCodePanel();
    });
}]);