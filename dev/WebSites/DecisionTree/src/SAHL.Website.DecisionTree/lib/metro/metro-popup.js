(function ($) {
    var METRO_Popup = 2014;
    if (METRO_Popup == undefined) {
        //var METRO_Popup = false;
    }
    var timeoutRef;
    var params;
    $.Popup = function (params2) {
        params = params2;

        if (!$.Popup.opened) {
            $.Popup.opened = true;
        } else {
            //$.Popup.closeAtOnce();
            return;
        }

        $.Popup.settings = params;

        params = $.extend({
            content: '',
            flat: false,
            shadow: false,
            overlay: false,
            width: 'auto',
            height: 'auto',
            position: 'default',
            top:0,
            left:0,
            padding: false,
            overlayClickClose: true,
            sysButtons: {
                btnClose: true
            },
            onShow: function (_Popup) { },
            sysBtnCloseClick: function (event) { },
            sysBtnMinClick: function (event) { },
            sysBtnMaxClick: function (event) { }
        }, params);

        var  _window,  _content;

       
        _window = $("<div/>").addClass("popup");
        if (params.flat) _window.addClass("flat");
        if (params.shadow) _window.addClass("shadow").css('overflow', 'hidden');
        _content = $("<div/>").addClass("content");
        _content.css({
            paddingTop: 0,
            paddingLeft: 0,
            paddingRight: 0,
            paddingBottom: 0
        });

       



        _content.html(params.content);


        _content.appendTo(_window);
        _window.appendTo('body').fadeIn('fast');

        _window.css('min-width', 0);
        _window.css('min-height', 0);

       

        METRO_Popup = _window;

        _window
            .css("position", "fixed")
            .css("z-index", parseInt(1500))
            .css("top", params.top)
            .css("left", params.left)
        ;

        addTouchEvents(_window[0]);

       
        _window.on('mouseleave', function (e) {
            $.Popup.close();
        });

        _window.on('mouseenter', function (e) {
            if (timeoutRef)
                clearTimeout(timeoutRef);
        });
        _window.on('click', function (e) {
            e.stopPropagation();
        });

       

        params.onShow(METRO_Popup);

        $.Popup.autoResize();

        return METRO_Popup;
    }

    $.Popup.content = function (newContent) {
        if (!$.Popup.opened || METRO_Popup == undefined) {
            return false;
        }

        if (newContent) {
            METRO_Popup.children(".content").html(newContent);
            $.Popup.autoResize();
            return true;
        } else {
            return METRO_Popup.children(".content").html();
        }
    }

    

    $.Popup.autoResize = function () {
        if (!$.Popup.opened || METRO_Popup == undefined) {
            return false;
        }

        var _content = METRO_Popup.children(".content");

        var top = params.top;
        var left = params.left;

        METRO_Popup.css({
            width: _content.outerWidth()+5,
            height: _content.outerHeight()+5,
            top: top,
            left: left
        });

        return true;
    }

    $.Popup.closeAtOnce = function () {
        if (!$.Popup.opened || METRO_Popup == undefined) {
            return false;
        }

        $.Popup.opened = false;
        var _overlay = METRO_Popup;
       
            _overlay.fadeOut(function () {
                $(this).remove();
            });
        
        
        return false;
    }

    $.Popup.close = function () {
        if (!$.Popup.opened || METRO_Popup == undefined) {
            return false;
        }

       
        //var _overlay = METRO_Popup;
        var closefunc = $.Popup.closeAtOnce;
        timeoutRef=setTimeout(closefunc, 250);
        return false;
    }
})(jQuery);
