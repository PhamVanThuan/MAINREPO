define("halo.tile", ["jquery", "halo.actions", "halo.tileLayout"], function ($, haloActions, haloTileLayout) {
    var tile = (function () {
        function initialize() {
            hookTileActionCallbacks();
        }

        function hookTileActionCallbacks() {
            $(".tile").each(function () {
                var tile = $(this);
                tile.unbind();
                tile.click(function () {
                    var clickedElement = tile;
                    haloActions.performAction({ clickedElement: clickedElement }, tileActionCallback);
                });
                if (tile.data("has-peek") === true) {
                    var hoverElement = tile;
                    tile.hover(function () {
                        haloActions.drillPreviewTileHoverIn(tile, tileActionCallback);
                    },
                    function () {
                        haloActions.drillPreviewTileHoverOut(tile, tileActionCallback);
                    });
                }
            });
        }

        function tileActionCallback(data) {
            if (data.actionToPerform !== undefined) {
                var $data = $(data.data);
                haloActions.performAction({ clickedElement: $data[0], actionToPerform : data.actionToPerform }, tileActionCallback);
            }
            else {
                loadTileGrid(data.data);
                haloActions.refreshContextMenu(data.item);
            }
        }

        function loadTileGrid(data) {
            var $container = $("#tile-element-area");
            $container.html(data);
            haloTileLayout.initialize({
                $container: $("#tile-area"),
                tileSelector: '.tile',
                tileToStampSelector: '.tile-major',
                minorTileSelector: '.tile-minor',
                tileThatComesFirstInSecondRowSelector: '.tile-mini',
                majorElementWidth: $container.find('.tile-major').first().outerWidth(),
                minorElementWidth: $container.find('.tile-minor').first().outerWidth()
            });
            loadTileData();
        }
        function loadTileData() {
            $(".tile").each(function () {
                haloActions.loadTileData(this);
            });
            hookTileActionCallbacks();
        }
        return {
            initialize: initialize,
            loadTileGrid: loadTileGrid,
            tileActionCallback: tileActionCallback
        }
    })();
    return tile;
});