(function () {
    var app = angular.module("app", []);


    var DateController = function ($scope) {
        $scope.rowCollection = [{ area: 'Asherville', time: '18:00', etime: '20:30' },
    { area: 'Atholl Heights', time: '20:00', etime: '22:30' },
    { area: 'Avoca Hills', time: '08:00', etime: '10:30' },
    { area: 'Bluff', time: '12:00', etime: '14:30' },
    { area: 'Bothas Hill', time: '14:00', etime: '16:30' },
    { area: 'Durban North', time: '16:00', etime: '18:30' },
    { area: 'Hillcrest', time: '14:00', etime: '16:30' },
    { area: 'La Lucia North', time: '06:00', etime: '08:30' },
    { area: 'Lamontville', time: '06:00', etime: '08:30' },
    { area: 'Sydenham', time: '14:00', etime: '16:30' },
    { area: 'Umlazi', time: '04:00', etime: '06:30' },
    { area: 'Waterfall', time: '20:00', etime: '22:30' },
    { area: 'Woodview', time: '18:00', etime: '22:30' },
    { area: 'Mobeni Heights', time: '20:00', etime: '22:30' },
    { area: 'New Germany', time: '18:00', etime: '20:30' }

        ];

        $scope.search = function (area) { //call this function from ng click on submit button

            var arr = [{ area: 'Asherville', time: '18:00', etime: '20:30' },
                                    { area: 'Atholl Heights', time: '20:00', etime: '22:30' },
                                    { area: 'Avoca Hills', time: '08:00', etime: '10:30' },
   { area: 'Bluff', time: '12:00', etime: '14:30' },
   { area: 'Bothas Hill', time: '14:00', etime: '16:30' },
   { area: 'Durban North', time: '16:00', etime: '18:30' },
   { area: 'Hillcrest', time: '14:00', etime: '16:30' },
   { area: 'La Lucia North', time: '06:00', etime: '08:30' },
   { area: 'Lamontville', time: '06:00', etime: '08:30' },
   { area: 'Sydenham', time: '14:00', etime: '16:30' },
   { area: 'Umlazi', time: '04:00', etime: '06:30' },
   { area: 'Waterfall', time: '20:00', etime: '22:30' },
   { area: 'Woodview', time: '18:00', etime: '20:30' },
   { area: 'Mobeni Heights', time: '20:00', etime: '22:30' },
   { area: 'New Germany', time: '18:00', etime: '20:30' }

            ];
        };//controller
    };
        app.controller("DateController", DateController);

    })();