'use strict';
angular.module('sahl.js.ui.forms')
.service('$viewManagerService', ['$rootScope',
function ($rootScope) {
    var internal = {
        viewlocation: '.views/lib/views/',
        editViewPostfix : '.edit.tpl.html',
        viewPostfix: '.tpl.html',
        base: function (modelType,splitChar,section) {
            var split = (modelType.indexOf(',') > -1 ? modelType.substr(0, modelType.indexOf(',')) : modelType).split(splitChar);
            split[1] = _.map(split[1].split('.'), function (key) {
                return key.charAt(0).toLowerCase() + key.slice(1);
            }).join('/');
            split[0] = split[0].toLowerCase();
            return split.join(internal.viewlocation+section);
        },
        getView: function (modelType) {
            return internal.base(modelType,'.Models.','tiles/') + internal.viewPostfix;
        },
        getEditView: function (modelType) {
            return internal.base(modelType,'.Models.','tiles/') + internal.editViewPostfix;
        },
        getPageView: function (modelType) {
            return internal.base(modelType,'.Pages.','pages/') + internal.viewPostfix;
        },
        getWizardPage: function (modelType) {
            return internal.base(modelType, '.Configuration.', 'wizardPages/') + internal.viewPostfix;
        },
        getDashboardView: function (modelType) {
            return internal.base(modelType, '.Configuration.', '') + internal.viewPostfix;
        }
    };
    return {
        getView: internal.getView,
        getEditView: internal.getEditView,
        getPageView: internal.getPageView,
        getWizardPage: internal.getWizardPage,
        getDashboardView : internal.getDashboardView
    };
}]);
