'use strict';
angular.module('sahl.tools.app.home.design.dockpanels.debug-breakpoints', [
	'sahl.tools.app.services'
])
.controller('DebugBreakpointsPanelCtrl', ['$rootScope','$scope','$eventAggregatorService','$eventDefinitions','filterFilter',
  function DebugBreakpointsPanelController($rootScope, $scope, $eventAggregatorService, $eventDefinitions, filterFilter) {
      $scope.breakpoints = new Array();

      var eventHandlers = {
          refreshBreakPoints: function (messagePacket) {
              $scope.breakpoints = messagePacket.breakpoints;
          },
          selectionChanged: function (packet) {
              if ($scope.debugger.isSelectionvalid(packet)) {
                  $eventAggregatorService.publish($eventDefinitions.onBreakpointStatus, { "id": packet.data.key, "text": packet.data.text, "validate": $scope.debugger.validate });
                  $scope.debugger.node = { "id": packet.data.key, "text": packet.data.text };
                  $scope.debugger.canActionBreakpoint = true;
                  $scope.debugger.setSelectIndex(packet.data.key, packet.data.text);
              } else {
                  $scope.debugger.reset(false, true);
                  $scope.debugger.selectedIndex = -1
              }
          },
          breakpointUpdated: function (packet) {
              if ($scope.debugger.node && $scope.debugger.canActionBreakpoint) {
                  if (packet.breakpoint.nodeId == $scope.debugger.node.id && packet.breakpoint.name == $scope.debugger.node.text) {
                      $eventAggregatorService.publish($eventDefinitions.onBreakpointStatus, { "id": packet.breakpoint.nodeId, "text": packet.breakpoint.name, "validate": $scope.debugger.validate });
                      $scope.debugger.setSelectIndex(packet.breakpoint.nodeId, packet.breakpoint.name);
                  }
              }
          }
      }

      $scope.toggleBreakpoint = function (index) {
          var packet = { "id": $scope.breakpoints[index].nodeId, "text": $scope.breakpoints[index].name };
          if ($scope.breakpoints[index].enabled) {
              $eventAggregatorService.publish($eventDefinitions.onBreakpointDisable, packet);
          } else {
              $eventAggregatorService.publish($eventDefinitions.onBreakpointEnable, packet);
          }
          $scope.selectNode($scope.breakpoints[index].nodeId);
      }
      $scope.remove = function(index){
          var message = $scope.breakpoints[index];
          $eventAggregatorService.publish($eventDefinitions.onBreakpointRemove, { "id": message.nodeId, "text": message.name });
      }
      $scope.selectNode = function (nodeId) {
          $eventAggregatorService.publish($eventDefinitions.onChangeSelectedNode, nodeId);
      }

      $scope.debugger = {
          canActionBreakpoint: false,
          hasBreakpoint: false,
          enabled: true,
          node: null,
          selectedIndex : -1,
          validate: function (packet) {
              $scope.debugger.hasBreakpoint = packet.exists;
              $scope.debugger.enabled = packet.enabled;

          },
          toggle: function () {
              var packet = { "id": this.node.id, "text": this.node.text };
              if (this.hasBreakpoint) {
                  if (this.enabled) {
                      $eventAggregatorService.publish($eventDefinitions.onBreakpointDisable, packet);
                  } else {
                      $eventAggregatorService.publish($eventDefinitions.onBreakpointEnable, packet);
                      this.enabled = true;
                  }
              } else {
                  $eventAggregatorService.publish($eventDefinitions.onBreakpointAdd, packet);
              }
          },
          remove: function () {
              var packet = { "id": this.node.id, "text": this.node.text };
              $eventAggregatorService.publish($eventDefinitions.onBreakpointRemove, packet);
              this.reset(true, false);
          },
          reset: function (canActionBreakpoint, resetNode) {
              $scope.debugger.canActionBreakpoint = canActionBreakpoint;
              $scope.debugger.hasBreakpoint = false;
              if (resetNode) {
                  this.node = null;
              }
          },
          setSelectIndex: function (id,name) {
              var results = filterFilter($scope.breakpoints, function (breakpoint) {
                  return breakpoint.nodeId === id && breakpoint.name === name;
              });
              if (results.length > 0) {
                  this.selectedIndex = $scope.breakpoints.indexOf(results[0]);
              } else {
                  this.selectedIndex = -1;
              }
          },
          isSelectionvalid: function (packet) {
              if (!packet) {
                  return false;
              }
              if (packet.data["linkType"] !== undefined) {
                  return false;
              }
              if (packet.data.category === "End" || packet.data.category === "Start") {
                  return false;
              }
              return true
          }
      }

      $eventAggregatorService.subscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.selectionChanged);
      $eventAggregatorService.subscribe($eventDefinitions.onBreakpointStatusChanged, eventHandlers.breakpointUpdated);
      $eventAggregatorService.subscribe($eventDefinitions.onBreakpointsChanged, eventHandlers.refreshBreakPoints);

      $scope.$on('$destroy', function () {
          $eventAggregatorService.unsubscribe($eventDefinitions.onNodeSelectionChanged, eventHandlers.selectionChanged);
          $eventAggregatorService.unsubscribe($eventDefinitions.onBreakpointStatusChanged, eventHandlers.breakpointUpdated);
          $eventAggregatorService.unsubscribe($eventDefinitions.onBreakpointsChanged, eventHandlers.refreshBreakPoints);
      })
}]);