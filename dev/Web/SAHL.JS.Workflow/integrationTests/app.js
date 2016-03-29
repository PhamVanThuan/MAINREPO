'use strict'
var intergrationTests = angular.module('intergrationTests', ['sahl.js.workflow.workflowManager','scenarioMaps.createInstance',
        'scenarioMaps.completeCreateInstanceActivity',
        'scenarioMaps.startActivity',
        'scenarioMaps.completeActivity',
        'scenarioMaps.cancelActivity',
        'scenarioMaps.startActivityMoveApplication',
        'jsonFormatter'
    ])
    .run(['$rootScope', function($rootScope) {
        $rootScope.testData = {};
    }]);


function escapeRegExp(value) {
    return value.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
}

function replaceAll(value, find, replace) {
    return value.replace(new RegExp(escapeRegExp(find), 'g'), replace);
}
