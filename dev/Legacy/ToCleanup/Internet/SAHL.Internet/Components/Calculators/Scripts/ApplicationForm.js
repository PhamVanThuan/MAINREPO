(function ($) {
    var settings = null;

    $.applicationForm = {
        init: function (options) {
            settings = $.extend({
                controls: {
                    phCode: null,
                    phNumber: null,
                    tbNumApplicants: null
                }
            }, options);

            $("#" + settings.controls.phCode).restrictInt();
            $("#" + settings.controls.phNumber).restrictInt();
            $("#" + settings.controls.tbNumApplicants).restrictInt();
        }
    };
})(jQuery);