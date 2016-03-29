'use strict';
angular.module('sahl.js.core.applicationManagement')
.service('$documentApplicationService', ['$q',
function () {
    var registeredDocumentApplications = [];

    var models = {
        documentApplication: function (appName, blurb, url, image, order) {
            this.appName = appName;
            this.blurb = blurb;
            this.url = url;
            this.image = image;
            this.order = order;
        }
    };

    var operations = {
        register: function (appName, blurb, url, image, order) {
            var newDocumentApplication = new models.documentApplication(appName, blurb, url, image, order);
            registeredDocumentApplications.push(newDocumentApplication);
        },
        getDocumentApplications : function(){
            return registeredDocumentApplications;
        }
    };

    return {
        register: operations.register,
        getDocumentApplications : operations.getDocumentApplications
    };
}]);
