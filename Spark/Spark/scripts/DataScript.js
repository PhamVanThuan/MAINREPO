(function () {
    app.controller('DataCtrl', ['$scope', function (scope) {
        scope.rowCollection = [
            { area: 'Avoca', time: Date.now },
        ];
    }]);
})();