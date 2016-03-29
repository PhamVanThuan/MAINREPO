define("halo.common", ["jquery", "halo.constants"], function ($, haloConstants) {
    var common = (function () {
        function getContext() {
            var contextMenuItemID = $("[data-active]").attr('id');
            contextMenuItemID = contextMenuItemID.replace("menu_item_", "");
            return contextMenuItemID;
        }
        function getBusinessKey(element) {
            var businessKey = $(element).data('business-key');
            var businessKeyType = $(element).data('business-keytype');
            return { businessKey: businessKey, businessKeyType: businessKeyType };
        }
        function getAction(element) {
            var actionToPerform = $(element).data('action');
            var actionUrl = $(element).data('action-url');
            return { actionToPerform: actionToPerform, actionUrl: actionUrl };
        }
        function getTileTypeName(element) {
            var tileModelTypeName = $(element).data('tilemodeltypename');
            var tileConfigTypeName = $(element).data('tileconfigtypename');
            return { tileModelTypeName: tileModelTypeName, tileConfigTypeName: tileConfigTypeName };
        }
        function getEditorTypeName(element) {
            var editorModelTypeName = $(element).data('editormodeltypename');
            var editorModelConfigurationTypeName = $(element).data('editormodelconfigurationtypename');
            var editorPageModelTypeName = $(element).data('editorpagemodeltypename');
            return { editorModelTypeName: editorModelTypeName, editorModelConfigurationTypeName: editorModelConfigurationTypeName, editorPageModelTypeName : editorPageModelTypeName };
        }
        function GetBusinessContext(businessContext) {
            $.ajax({
                type: "GET",
                url: haloConstants.GetContextURL,
                data: { "context": businessContext.context, "businessKey": businessContext.businessKey, "businessKeyType": businessContext.businessKeyType }
            }).done(function (data) {
                $("#menubar-context").html(data);
            });
        }
        function startLoading(element) {
            if(!$(element).hasClass("no-loading"))
                $(element).addClass("loading").html(getLoadingHtml());
        }
        function getLoadingHtml() {
            return '<div class="loading-bar"><div class="loading-bar-text-wrapper"><div class="loading-bar-text"><h4 class="text-center">Loading...Please wait...</h4></div></div></div>';
        }
        return {
            getContext: getContext,
            getBusinessKey: getBusinessKey,
            getAction: getAction,
            getTileTypeName: getTileTypeName,
            GetBusinessContext: GetBusinessContext,
            getEditorTypeName: getEditorTypeName,
            startLoading: startLoading
        }
    })();
    $.fn.serializeObject = function () {
        var o = Object.create(null),
            elementMapper = function (element) {
                element.name = $.camelCase(element.name);
                return element;
            },
            appendToResult = function (i, element) {
                var node = o[element.name];

                if ('undefined' != typeof node && node !== null) {
                    o[element.name] = node.push ? node.push(element.value) : [node, element.value];
                } else {
                    o[element.name] = element.value;
                }
            };

        $.each($.map(this.serializeArray(), elementMapper), appendToResult);
        return o;
    };
    return common;
});