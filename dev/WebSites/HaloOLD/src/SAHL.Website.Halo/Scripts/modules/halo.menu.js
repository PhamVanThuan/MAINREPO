define("halo.menu", ["jquery", "halo.actions", "halo.common", "halo.tile", "halo.scrollables"], function ($, haloActions, haloCommon, haloTile, scrollables) {
    var menu = (function () {
        function initialize() {
            hookMenuActionCallbacks();
            menuStyles();
        }

        function hookMenuActionCallbacks() {
            $(".context-menu-item").each(function () {
                var self = $(this);
                self.unbind();
                self.click(function () {
                    var clickedElement = this;
                    haloActions.performAction({ clickedElement: clickedElement }, menuActionCallback);
                });
            });

            $(".userRole").each(function () {
                var self = $(this);
                self.unbind();
                self.click(function () {
                    var clickedElement = this;
                    haloActions.performAction({ clickedElement: clickedElement, actionToPerform: "ChangeUserRole" });
                    e.preventDefault();
                });
            })

            $(".icon-remove").each(function () {
                $(this).click(function (e) {
                    $(this).unbind();
                    var clickedElement = $(this).parents("li:first");
                    haloActions.performAction({ clickedElement: clickedElement, actionToPerform: "RemoveBusinessContext" }, function (data) {
                        scrollables.scrollBack($(clickedElement), ".scrollable-container");
                        $(clickedElement).remove();
                        if ($(clickedElement).hasClass("active")) {
                            $('#ribbon_menu_item_client_search > a')[0].click();
                        }
                    });
                    e.preventDefault();
                });
            });
        }

        function menuStyles() {
            $(".ribbon-menu-item").each(function () {
                var element = $(this);
                var width = element.width();
                element.width(width + 0.5);
            });

            $(".icon-remove").hover(function () {
                $(this).addClass("icon-remove-sign");
            }, function () {
                $(this).removeClass("icon-remove-sign");
            });
        }
        function menuActionCallback(data) {
            haloTile.loadTileGrid(data.data);
            haloActions.refreshContextMenu(data.item);
        }

        return {
            initialize: initialize,
        }
    })();
    return menu;
});