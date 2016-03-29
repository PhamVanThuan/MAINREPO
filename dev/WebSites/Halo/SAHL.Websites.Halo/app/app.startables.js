'use strict';
angular.module('halo.startables', [
    'sahl.js.core.serviceManagement',
    'sahl.halo.charm.mail.app',
    'sahl.js.core.logging',
    'sahl.halo.app.variables',
    'sahl.halo.app.variables.templates',
    'sahl.ui.halo.myhalo.views.templates',
    'sahl.halo.app.organisationStructure',
    'sahl.halo.app.organisationstructure.templates',
    'sahl.halo.app.domainDocumentation',
    'sahl.halo.app.domaindocumentation.templates',
    'sahl.websites.halo.services.entityManagement',
    'sahl.websites.halo.services.entityViewManagement',
    'sahl.js.ui.helper',
    'sahl.websites.halo.services.lookupDataService',
    'sahl.websites.halo.services.searchManagement',
    'sahl.js.core.documentManagement.types.userprofile',
    'sahl.websites.halo.services.entityBreadcrumbManagement'
])
.provider('$haloStartables', [function () {
    this.decoration = ['$delegate', '$q'
    , '$eventAggregatorService'
    , '$mailCharmService'
    , '$logger'
    , '$variablesAppService'
    , '$organisationStructureAppService'
    , '$entityManagerService'
    , '$entityViewManagerService'
    , '$uiHelperService'
    , '$lookupDataService'
    , '$searchManagerService'
    , '$userprofileDocumentVersion_0_1'
    , '$userprofileDocumentVersion_0_2'
    , '$userprofileDocumentVersion_0_3'
    , '$entityBreadcrumbManagerService'
    , '$domainDocumentationAppService'
    , function ($delegate, $q) {
        var startables = arguments;
        return {
            startServices: function() {
                return $delegate.startServices.apply(this, startables);
            }
        };
    }];
    this.$get = [function (){}];
}]);
