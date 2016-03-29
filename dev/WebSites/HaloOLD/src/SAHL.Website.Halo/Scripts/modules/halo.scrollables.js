define("halo.scrollables", ["jquery", "halo.utils"], function ($, haloUtils) {
    var scrollable = (function () {
        var internalOptions = [{}];
        var defaults = {
            containerSelector: ".scrollable-container",
            scrollableItemSelector: ".scrollable-item",
            forwardSelector: ".scrollable-forward",
            backSelector: ".scrollable-back",
            scrollableControlsSelector: ".scrollable-controls",
            scrollableContentsSelector: ".scrollable-contents",
            scrollableInnerContainerSelector: ".scrollable-inner-container",
            scrollContainerRightSelector: ".container-right",
            scrollContainerLeftSelector: ".container-left",
            scrollContainerRightClass: "scrollable-inner-container-right",
            scrollContainerLeftClass: "scrollable-inner-container-left",
            enabledClass: "scrollabled-enabled"
        };

        function initialize(options) {

            if (options !== undefined) {
                internalOptions = options;
            }
            $.each(internalOptions, function () {
                var option = this;
                setupDefaults(option);
                enableMenuScrolling(option);
                hookScrolling(option);
            });
            enableResize();
        }

        function setupDefaults(option) {
            option.forwardSelector = option.forwardSelector || defaults.forwardSelector;
            option.backSelector = option.backSelector || defaults.backSelector;
            option.containerSelector = option.containerSelector || defaults.containerSelector;
            option.scrollableItemSelector = option.scrollableItemSelector || defaults.scrollableItemSelector;
            option.scrollableControlsSelector = option.scrollableControlsSelector || defaults.scrollableControlsSelector;
            option.scrollableContentsSelector = option.scrollableContentsSelector || defaults.scrollableContentsSelector;
            option.scrollableInnerContainerSelector = option.scrollableInnerContainerSelector || defaults.scrollableInnerContainerSelector;
            option.scrollContainerRightSelector = option.scrollContainerRightSelector || defaults.scrollContainerRightSelector;
            option.scrollContainerLeftSelector = option.scrollContainerLeftSelector || defaults.scrollContainerLeftSelector;
            option.scrollContainerRightClass = option.scrollContainerRightClass || defaults.scrollContainerRightClass;
            option.scrollContainerLeftClass = option.scrollContainerLeftClass || defaults.scrollContainerLeftClass;
        }

        function enableMenuScrolling(option) {
            var containers = $(option.containerSelector);
            $(containers).each(function () {
                var scrollableContents = $(this).find(option.scrollableContentsSelector);
                var container = $(this);
                if (CanContainerScroll(scrollableContents)) {
                    container.find(option.scrollableControlsSelector).show();
                    ChangeScrollingControlStyles(container, option);
                    container.find(option.scrollContainerLeftSelector).addClass(option.scrollContainerLeftClass);
                    container.find(option.scrollContainerRightSelector).addClass(option.scrollContainerRightClass);
                } else {
                    if (canHideControls(container, option)) {
                        container.find(option.scrollableControlsSelector).hide();
                        container.find(option.scrollContainerLeftSelector).removeClass(option.scrollContainerLeftClass);
                        container.find(option.scrollContainerRightSelector).removeClass(option.scrollContainerRightClass);
                    }
                }
            });
        }

        function ChangeScrollingControlStyles(container, option) {
            if (CanForwardScroll(container, option)) {
                container.find(option.forwardSelector).addClass("scrollable-scroll-enabled");
            } else {
                container.find(option.forwardSelector).removeClass("scrollable-scroll-enabled");
            }
            if (CanBackArrowScroll(container, option)) {
                container.find(option.backSelector).addClass("scrollable-scroll-enabled");
            } else {
                container.find(option.backSelector).removeClass("scrollable-scroll-enabled");
            }
        }
        function CanForwardScroll(container, option) {
            var itemsHidden = container.find(option.scrollableItemSelector + ":not(.hide)").length;
            return itemsHidden;
        }

        function CanBackArrowScroll(container, option) {
            var itemsHidden = container.find(option.scrollableItemSelector + ".hide").length;
            return itemsHidden;
        }

        function canHideControls(container, option) {
            var container = $(container);

            var itemsHidden = !container.find(option.scrollableItemSelector + ".hide").length;
            var hideable = container.find(option.scrollContainerLeftSelector + "," + option.scrollContainerRightSelector).length;

            return itemsHidden && hideable;
        }

        function CanContainerScroll(container) {
            var container = $(container);
            var totalWidth = 0;
            container.children().each(function () {
                totalWidth = totalWidth + $(this).width();
            });
            //console.log(container);
            //console.log("Total " + totalWidth);
            //console.log("Width " + container.width())
            return totalWidth > container.width()
        }

        function enableResize() {
            $(window).resize(function () {
                haloUtils.delay(function () {
                    $.each(internalOptions, function () {
                        var option = this;
                        enableMenuScrolling(option);
                    });
                }, 500);
            });
        }

        function hookScrolling(option) {

            $(option.containerSelector).find(option.forwardSelector).unbind();
            $(option.containerSelector).find(option.forwardSelector).click(function () {
                var clickedElement = this;
                scrollForward(clickedElement, option, function () {
                    ChangeScrollingControlStyles($(clickedElement).parents(option.containerSelector + ":first"), option);
                });
            });
            $(option.containerSelector).find(option.backSelector).unbind();
            $(option.containerSelector).find(option.backSelector).click(function () {
                var clickedElement = this;
                scrollBack(clickedElement, option, function () {
                    ChangeScrollingControlStyles($(clickedElement).parents(option.containerSelector + ":first"), option);
                });
            });

            $(option.containerSelector).hammer().on("swiperight", function (event) {
                var clickedElement = $(this).find(option.forwardSelector).first();
                scrollBack(clickedElement, option, function () {
                    ChangeScrollingControlStyles($(clickedElement).parents(option.containerSelector + ":first"), option);
                });
            });
            $(option.containerSelector).hammer().on("swipeleft", function (event) {
                var clickedElement = $(this).find(option.backSelector).first();
                scrollForward(clickedElement, option, function () {
                    ChangeScrollingControlStyles($(clickedElement).parents(option.containerSelector + ":first"), option);
                });
            });

        }

        function scrollForward(element, option, callback) {
            var scrollableItem = $(element).parents(option.containerSelector + ":first").find(option.scrollableItemSelector).filter(":not(.hide)").first();
            scrollableItem.animate({
                width: 'hide',
            }, 600, function () {
                scrollableItem.addClass("hide");
                callback();
            });

        }

        function scrollItemForward(element, parentContainer) {
            var option = _.findWhere(internalOptions, { containerSelector: parentContainer });
            $(element).parents(option.containerSelector).find(option.forwardSelector).click();
        }

        function scrollItemBack(element, parentContainer) {
            var option = _.findWhere(internalOptions, { containerSelector: parentContainer });
            $(element).parents(option.containerSelector).find(option.backSelector).click();
        }

        function scrollBack(element, option, callback) {
            var scrollableItem = $(element).parents(option.containerSelector + ":first").find(option.scrollableItemSelector).filter(".hide").last();
            scrollableItem.animate({
                width: 'show',
            }, 600, function () {
                scrollableItem.removeClass("hide");
                callback();
            });
        }

        return {
            initialize: initialize,
            hookScrolling: hookScrolling,
            scrollForward: scrollItemForward,
            scrollBack: scrollItemBack
        }
    }());
    return scrollable;
});

