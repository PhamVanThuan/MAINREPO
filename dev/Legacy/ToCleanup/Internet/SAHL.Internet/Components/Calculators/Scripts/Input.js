(function ($) {
    $.fn.restrictDbl = function () {
        return this.filter("input[type='text']").keypress(function (ev) {
            var key = ev.which;
            if (ev.keyCode === 8 || ev.keyCode === 9) return; // backspace and tab.

            if (key !== 46 && (key < 48 || key > 57)) ev.preventDefault();
        });
    };

    $.fn.restrictInt = function () {
        return this.filter("input[type='text']").keypress(function (ev) {
            var key = ev.which;
            if (ev.keyCode === 8 || ev.keyCode === 9) return; // backspace and tab.
            
            if (key < 48 || key > 57) ev.preventDefault();
        });
    };

    $.hideInput = function (input) {
        var $input = $(input);
        $input.each(function () {
            var $this = $(this);
            $this.hide();
            if (this.id) {
                var selector = "label[for='" + this.id + "']";
                $this.next(selector).hide();
                $this.prev(selector).hide();
            }
        });
        return input;
    };

    $.showInput = function (input) {
        var $input = $(input);
        $input.each(function () {
            var $this = $(this);
            $this.show();
            if (this.id) {
                var selector = "label[for='" + this.id + "']";
                $this.next(selector).show();
                $this.prev(selector).show();
            }
        });
        return input;
    };
})(jQuery);