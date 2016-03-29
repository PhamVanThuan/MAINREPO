(function ($) {
    var settings = null;

    $.contactMeForm = {
        init: function (options) {
            settings = $.extend({
                controls: {
                    phCode: null,
                    phNumber: null
                }
            }, options);

            $("#" + settings.controls.phCode).restrictInt();
            $("#" + settings.controls.phNumber).restrictInt();
        }
    };
})(jQuery);