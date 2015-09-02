(function () {
    var app = angular.module("app", []);

    var TipsController = function ($scope) {
        
            $scope.rowCollection =
       ['1. Go Solar. Install a solar geyser, get solar lamps to put outside in the garden and take inside when the lights are out. There is also a solar cellphone charger available. You can put it on your dash board while driving and if you get home and theres no electricity, you can still charge your phone.',
         '2. Get gas. Gas stoves are becoming a popular choice for people who are building a new home or re-doing their kitchen. Theres also the portable option: you can buy a camping gas stove. This way you can cook food or boil the kettle even if theres no electricity.',
         '3. Use empty plastic cool drink bottles and fill them with water and place in your deep freeze. If the power is out for a long time, you can take them out and put them in your fridge to keep food cold until the power comes back on. It also will create extra freezing in the deep freeze to keep your meats from thawing.',
         '4. Battery operated lights. You can get lanterns, torches and other battery operated lights to keep around the house when the power goes off. It is less dangerous than using just candles.',
         '5. Get a head torch or cap. Many of these are available at your local hardware store. You can strap the head torch around your head or get a cap with a fitted light so that you can walk around the house easily, without trying to make your way in the dark.',
         '6. Get a generator. Often this is the more expensive option, but depending on your needs and your budget, getting a generator may be a good idea. You can get ones that will keep the entire house powered or smaller ones to just keep the fridge running and perhaps the tv on.',
         '7. Make sure you have car chargers for your cell phone and iPad. This way you can always make sure your phone is charged while driving before you get to your destination and theres no electricity.',
         '8. The cost of appliances and how much electricity they use on a monthly basis, according to Eskom Energy calculator.'
       ];
    }
    app.controller("TipsController", TipsController);

})();