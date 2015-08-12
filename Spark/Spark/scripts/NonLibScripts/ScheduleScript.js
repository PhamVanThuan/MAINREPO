(function () {
    var app = angular.module("app", []);

    var DateController = function ($scope) {
        $scope.rowCollection = [{area : 'Avoca', time: '8-00'}, 
                                {area : 'Pheonix', time : '10:00'},
                                { area: 'Verulam', time: '12-00' }];
    };

    app.controller("DateController", DateController);



})();