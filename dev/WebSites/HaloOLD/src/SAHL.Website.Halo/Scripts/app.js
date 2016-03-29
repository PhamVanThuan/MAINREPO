
var debug = true;
require.config({
    baseUrl: rootDir + "scripts/modules",
    paths: {
        "jquery": "../libs/jquery",
        "pnotify": "../libs/pines/jquery.pnotify",
        "bootstrap": "../libs/bootstrap",
        "bootstrap-datepicker": "../libs/bootstrap-datepicker",
        "underscorejs": "../libs/underscore",
        "moment": "../libs/moment",
        "hammer": "../libs/hammer",
        "jqueryHammer": "../libs/jquery.hammer"
    },
    shim: {
        'bootstrap': ["jquery"],
        'pnotify': ["jquery"],
        'underscorejs': [],
        'bootstrap-datepicker': ["jquery"],
        'hammer': ["jquery", "jqueryHammer"]
    }
});

require(["jquery", "halo.refreshables", "halo.notifications", "halo.menu", "halo.scrollables", "halo.tile", "halo.tileLayout", "halo.actions",
    "bootstrap", "bootstrap-datepicker", "halo.datepicker", "halo.notifications", "pnotify", "underscorejs", "moment", "hammer"],
    function ($, refreshables, notifications, menu, scrollables, tile, layout, actions, search, hammer) {
        function start() {
            refreshables.addRefreshable(menu.initialize);
            refreshables.addRefreshable(tile.initialize);
            refreshables.addRefreshable(layout.arrangeTiles);
            refreshables.addRefreshable(scrollables.initialize);
            refreshables.refresh();

            layout.initialize({ containerID: '#minor-tile-area', itemSelector: '.tile', pinnedItem: '.tile-major' });

            $('.dropdown-toggle').dropdown();
            if ($('.context-menu-item :last').length > 0) {
                $('.context-menu-item :last').trigger('click');
            } else {
                var $activeRibbonItem = $('.ribbon-menu-item[data-business-key][data-active]');
                if ($activeRibbonItem.length > 0) {
                    actions.performAction({ actionToPerform: 'LoadTilesFromContext' }, tile.tileActionCallback);
                }
            }
        }
        start();
    });