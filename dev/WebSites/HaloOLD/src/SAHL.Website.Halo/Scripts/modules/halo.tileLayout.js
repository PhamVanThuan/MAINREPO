define("halo.tileLayout", ["jquery", "halo.utils"], function ($, haloUtils) {
	var layout = (function () {
		var internaloptions = {};
		function initialize(options) {
			internalOptions = options;
			var timer;
			$(window).resize(function () {
				timer = haloUtils.delay(function () {
					arrangeTiles(internalOptions);
				}, 500, timer);
			});
		}

		function arrangeTiles(options) {
			//var tiles = options.$container.find(options.tileSelector).toArray();

			//var majorTile = getTilesOfType(tiles, options.tileToStampSelector)[0];
			//var minorTiles = getTilesOfType(tiles, options.minorTileSelector);
			//var miniTiles = getTilesOfType(tiles, options.tileThatComesFirstInSecondRowSelector);

			//var numberOfTilesThatCanFitInARow = (options.$container.outerWidth() - $(majorTile).outerWidth()) / $(minorTiles[0]).outerWidth();

			////clear the container
			//options.$container.html('');

			////place the major tile
			//options.$container.append(majorTile);
			//tiles.splice(0, 1);

			//for (row = 0; row < 2; row++) {
			//    for (i = 0; i < Math.floor(numberOfTilesThatCanFitInARow) ; i++) {
			//        if (row == 1) {
			//            while (miniTiles.length > 0) {
			//                var miniTilesTaken = take(miniTiles, 4);
			//                miniTiles.splice(0, 4);
			//                var element = $(document.createElement('div'));
			//                $(element).css('width', options.minorElementWidth + 5);
			//                $(element).css('height', options.minorElementWidth + 5);
			//                $(element).css('float', 'left');
			//                $(element).css('padding', '0');
			//                $(element).css('margin', '0');
			//                $(element).append(miniTilesTaken);
			//                options.$container.append(element);
			//            }
			//        }
			//        else {
			//            var tile = minorTiles;
			//            options.$container.append(tile);
			//            minorTiles.splice(0, 1);
			//        }
			//    }
			//}
			//while (minorTiles.length > 0) {
			//    var tile = minorTiles[0];
			//    options.$container.append(tile);
			//    minorTiles.splice(0, 1);
			//}
		}

		function getTilesOfType(tiles, type) {
			type = type.replace('.', '');
			var tilesToReturn = [];
			$.each(tiles, function (index, tile) {
				if ($(tile).hasClass(type)) {
					tilesToReturn.push(tile);
				}
			});
			return tilesToReturn;
		}

		function take(array, numberOfElements) {
			var tempArray = [];
			for (i = 0; i < numberOfElements; i++) {
				if (array[i]) {
					tempArray.push(array[i])
				}
			}
			return tempArray;
		}

		return {
			initialize: initialize,
			arrangeTiles: arrangeTiles
		}
	})();
	return layout;
});