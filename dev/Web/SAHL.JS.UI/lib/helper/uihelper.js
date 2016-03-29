'use strict';

angular.module('sahl.js.ui.helper', [])
    .service('$uiHelperService', ['$rootScope',
        function ($rootScope) {

            var operations = {
                getIconForProduct: function (type, productKey) {
                    if (type === "Account") {
                        return processProductKey(productKey, "acc");
                    }

                    if (type === "Application") {
                        return processProductKey(productKey, "app");
                    }

                    return "";
                }
            };

            function processProductKey(productKey, productKeyType) {
                if (productKey === 1 || productKey === 2 || productKey === 5 || productKey === 6 || productKey === 9 || productKey === 11) {
                    return productKeyType + "-mortgage-open";
                }

                if (productKey === 3) {
                    return productKeyType + "-hoc-open";
                }

                if (productKey === 4) {
                    return productKeyType + "-life-open";
                }

                return "";
            }

            return {
                getIconForProduct: operations.getIconForProduct,
                start: function () {
                    $rootScope.getIconForProduct = operations.getIconForProduct;
                }
            };
        }
    ]);
