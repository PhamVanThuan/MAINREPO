'use strict';

angular.module('sahl.tools.app.serviceConfig', [])
.factory('$serviceConfig', function () {
    return {
        CommandService: 'http://localhost/DecisionTreeDesignService/api/CommandHttpHandler/performhttpcommand',
        QueryService: 'http://localhost/DecisionTreeDesignService/api/QueryHttpHandler/PerformHttpQuery',
        UserProfileImageService: 'http://localhost/DecisionTreeDesignService/api/profile/getimage?username=',
        SignalRService: 'http://localhost/DecisionTreeDesignService/SignalR'
    }
});