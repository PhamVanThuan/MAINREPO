define("halo.actions", ["halo.common", "halo.constants", "halo.notifications", "halo.refreshables", "halo.validations"], function (haloCommon, haloConstants, notifications, refreshables, validations) {
    var actions = (function () {
        function performAction(e, callback) {
            var action = { actionToPerform: e.actionToPerform };
            if (action.actionToPerform === undefined) {
                action = haloCommon.getAction(e.clickedElement);
            }
            var methodToCall = this.getMethod(action.actionToPerform);
            if (methodToCall !== undefined) {
                methodToCall(e.clickedElement, callback);
            }
        }

        function TileChangeContext(item, callback) {
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(item);
            var tileType = haloCommon.getTileTypeName(item);
            var action = haloCommon.getAction(item);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(action.actionUrl),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType, "tileModelTypeName": tileType.tileModelTypeName, "tileConfigTypeName": tileType.tileConfigTypeName }
            }).success(function (data) {
                callback({
                    item: item,
                    data: data
                });
            });
        }
        function TileDrillDown(item, callback) {
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(item);
            var tileType = haloCommon.getTileTypeName(item);
            var action = haloCommon.getAction(item);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(action.actionUrl),
                data: {
                    "context": context,
                    "businessKey": businessKey.businessKey,
                    "businessKeyType": businessKey.businessKeyType,
                    "tileModelTypeName": tileType.tileModelTypeName,
                    "tileConfigTypeName": tileType.tileConfigTypeName
                }
            }).success(function (data) {
                callback({
                    item: item,
                    data: data
                });
            });
        }
        function TileLaunchEditor(item, callback) {
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(item);
            var tileType = haloCommon.getTileTypeName(item);
            var action = haloCommon.getAction(item);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.ShowEditorURL),
                data: {
                    "context": context,
                    "businessKey": businessKey.businessKey,
                    "businessKeyType": businessKey.businessKeyType,
                    "tileModelTypeName": tileType.tileModelTypeName,
                    "tileConfigTypeName": tileType.tileConfigTypeName
                }
            }).success(function (data) {
                callback({
                    item: item,
                    data: data,
                    actionToPerform: 'ShowEditorDialog'
                });
            });
        }
        function ShowEditorDialog(item, callback) {
            $("body").append(item);
            $('#editorModal').on('show', function () {
                $('#editorModal').css({
                    width: 'auto',
                    'min-width': '600px',
                    height: 'auto'
                });
            });
            $('#editorModal').modal();
            callback({
                item: item,
                data: item,
                actionToPerform: 'GetPageModel'
            });
        }
        function GetPageModel(editor) {
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(editor);
            var editorType = haloCommon.getEditorTypeName(editor);
            var action = haloCommon.getAction(editor);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.GetPageModelURL),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType, "editorModelTypeName": editorType.editorModelTypeName, "editorModelConfigurationTypeName": editorType.editorModelConfigurationTypeName }
            }).success(function (data) {
                var $data = $('<div/>').append(data);
                var body = $("#pagemodelbody", $data)[0];
                $('#editorModal .modal-body').html(body);

                var footer = $("#pagemodelfooter", $data)[0];
                $('#editorModal .modal-footer').html(footer);

                $('#editor-back').click(function () {
                    EditorBack($('#editorModal')[0]);
                });
                $('#editor-next').click(function () {
                    EditorNext($('#editorModal')[0]);
                });
                $('#editor-close').click(function () {
                    EditorClose();
                });
            });
        }
        function RemoveBusinessContext(item, callback) {
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(item);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.RemoveBusinessContextURL),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType }
            }).success(function (data) {
                callback({
                    item: item,
                    data: data,
                });
            });
        }

        function ChangeUserRole(item, callback) {
            var action = haloCommon.getAction(item);
            var organisationArea = $(item).data("organisationarea");
            var roleName = $(item).data("rolename");
            var url = GetAjaxRequestUrl(action.actionUrl) + "?" + $.param({ "organisationArea": organisationArea, "roleName": roleName });
            window.location = url;
        }

        function EditorBack(editor) {
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(editor);
            var editorType = haloCommon.getEditorTypeName(editor);
            var action = haloCommon.getAction(editor);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.GetPreviousPageModelURL),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType, "editorModelTypeName": editorType.editorModelTypeName, "editorModelConfigurationTypeName": editorType.editorModelConfigurationTypeName }
            }).success(function (data) {
                var $data = $('<div/>').append(data);
                var body = $("#pagemodelbody", $data)[0];
                $('#editorModal .modal-body').html(body);

                var footer = $("#pagemodelfooter", $data)[0];
                $('#editorModal .modal-footer').html(footer);

                $('#editor-back').click(function () {
                    EditorBack($('#editorModal')[0]);
                });
                $('#editor-next').click(function () {
                    EditorNext($('#editorModal')[0]);
                });
                $('#editor-close').click(function () {
                    EditorClose();
                });
            });
        }
        function EditorNext(editor) {
            var isLastPage = $("#editor-next").data('is-last-page');
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(editor);
            var editorType = haloCommon.getEditorTypeName(editor);
            var action = haloCommon.getAction(editor);
            var serializedForm = JSON.stringify($('form').serializeObject());
            var editorPageModelType = haloCommon.getEditorTypeName($('form')[0]);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(!isLastPage ? haloConstants.SubmitEditorPageURL : haloConstants.SubmitEditorURL),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType, "editorModelTypeName": editorType.editorModelTypeName, "editorModelConfigurationTypeName": editorType.editorModelConfigurationTypeName, "editorPageModelTypeName": editorPageModelType.editorPageModelTypeName, "pageModel": serializedForm }
            }).success(function (data) {
                if (!isLastPage) {
                    GetPageModel(editor);
                } else {
                    EditorClose();
                }
            }).error(function (data) {
                validations.showErrors(data);
            });
        }
        function EditorClose(editor) {
            notifications.hideall();
            $("#editorModal").modal('hide');
            $("#editorModal").remove();
        }

        function loadTileData(tile) {
            // Set a 0.2 second delay on starting the loading bar
            // to prevent the animation from flashing if it loads quickly.
            var interval = setInterval(function () {
                haloCommon.startLoading(tile)
            }, 200);

            var businessKey = haloCommon.getBusinessKey(tile);
            var context = haloCommon.getContext();
            var tileType = haloCommon.getTileTypeName(tile);
            var action = haloCommon.getAction(tile);
            if (businessKey.businessKey === undefined ||
                businessKey.businessKeyType === undefined ||
                context === undefined ||
                tileType === undefined) {
                return;
            }

            //SetTimeout for testing only.
            //setTimeouT GetInstance<T>(string namedInstance);(function () {
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.GetTileDataURL),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType, "tileModelTypeName": tileType.tileModelTypeName, "tileConfigTypeName": tileType.tileConfigTypeName }
            }).done(function (data) {
                $(tile).html(data);
                clearInterval(interval);
            }).error(function (data) {
                $(tile).html(data);
                clearInterval(interval);
            });
            //}, 1000);
        }
        function drillPreviewTileHoverIn(tile, callback) {
            var context = haloCommon.getContext();
            var businessKey = haloCommon.getBusinessKey(tile);
            var tileType = haloCommon.getTileTypeName(tile);

            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.GetDrillPreviewDataURL),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType, "tileModelTypeName": tileType.tileModelTypeName, "tileConfigTypeName": tileType.tileConfigTypeName }
            }).done(function (data) {
                //Remove the transition end events just in case we are re-entering the tile.
                $("#GreyedOutTileArea").off("webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend");
                //Fade the background and highlight the hovered block.
                $("#GreyedOutTileArea").addClass("GreyOut");
                $(tile).addClass("StandOut");

                //Find the tiles that need to be highlighted.
                var jsonData = jQuery.parseJSON(data);
                var keysToFocus = $.map(jsonData.BusinessKeys.split(","), function (n) {
                    return n.replace(/\s+/g, '');
                });
                var keyType = jsonData.BusinessKeysType;
                $(keysToFocus).each(function () {
                    $("div[data-business-key='" + this + "'][data-business-keytype='" + keyType + "']").addClass("StandOut");
                });
            });
        }
        function drillPreviewTileHoverOut(tile, callback) {
            $("#GreyedOutTileArea").removeClass("GreyOut");
            //Remove all the tiles standing out when the grey out transition finishes
            $("#GreyedOutTileArea").on("webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend", function () {
                $(".StandOut").removeClass("StandOut");
                $("#GreyedOutTileArea").off("webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend");
            });
        }
        function LoadTilesFromContext(item, callback) {
            var context = haloCommon.getContext();

            var item = $('.ribbon-menu-item[data-business-key][data-active]')[0];

            var businessKey = haloCommon.getBusinessKey(item);
            var tileType = haloCommon.getTileTypeName(item);
            var action = haloCommon.getAction(item);
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.GetUsersTilesForContextURL),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType, "tileModelTypeName": '', "tileConfigTypeName": '' }
            }).success(function (data) {
                callback({
                    item: item,
                    data: data
                });
            });
        }
        function refreshContextMenu(clickedElement, callback) {
            var businessKey = haloCommon.getBusinessKey(clickedElement);
            var context = haloCommon.getContext();
            if (businessKey.businessKey === undefined ||
                businessKey.businessKeyType === undefined ||
                context === undefined) {
                return;
            }
            $.ajax({
                type: "GET",
                url: GetAjaxRequestUrl(haloConstants.GetContextMenu),
                data: { "context": context, "businessKey": businessKey.businessKey, "businessKeyType": businessKey.businessKeyType }
            }).done(function (data) {
                $("#menubar-context").html(data);
                refreshables.refresh();
            });
        }

        function getMethods() {
            var funcs = [TileChangeContext, TileDrillDown, TileLaunchEditor, GetPageModel, ShowEditorDialog, RemoveBusinessContext, LoadTilesFromContext, ChangeUserRole];
            return funcs;
        }
        function getMethod(name) {
            var methods = getMethods();
            for (var i = 0; i < methods.length; i++) {
                if (getFunctionName(methods[i]) == name)
                    return methods[i];
            };
        }
        function getFunctionName(func) {
            var functionName = func.toString().match(/^function\s*([^\s(]+)/)[1];
            return functionName;
        }

        return {
            performAction: performAction,
            loadTileData: loadTileData,
            TileDrillDown: TileDrillDown,
            TileChangeContext: TileChangeContext,
            getMethod: getMethod,
            LoadTilesFromContext: LoadTilesFromContext,
            refreshContextMenu: refreshContextMenu,
            drillPreviewTileHoverIn: drillPreviewTileHoverIn,
            drillPreviewTileHoverOut: drillPreviewTileHoverOut
        }
    })();
    return actions;

    function GetAjaxRequestUrl(url) {
        return rootDir + url;
    }
});