'use strict';

/* Directives */

angular.module('capitecApp.directives', [])
.directive('appVersion', ['version', function (version) {
    return function (scope, elm) {
        elm.text(version);
    };
}])
.directive('customCheckbox', [function () {
    return {
        restrict: 'A',
        replace: true,
        transclude: true,
        require: 'ngModel',
        scope: {
            item: '=customCheckbox',
            ngModel: '='
        },
        template: '<label class="checkboxLabel">'
                    + '<input type="checkbox" ng-model="ngModel"/>'
                    + '<span></span>'
                    + '{{item}}'
                 + '</label>',
        link: function (scope, element, attrs, ngModel) {
            var checkboxWatch = scope.$watch(function () {
                return ngModel.$modelValue;
            },
            function (modelValue) {
                if (modelValue === true) {
                    element.addClass('checked');
                }
                else {
                    element.removeClass('checked');
                }
            });

            scope.$on('$destroy', function() {
                if(checkboxWatch) { checkboxWatch(); }
            });
        }
    };
}])
.directive('toggleSwitch', [function () {
    return {
        restrict: 'A',
        replace: true,
        transclude: true,
        require: 'ngModel',
        scope: {
            ngModel: '=',
            ngChange: '&',
            textOff: '@textOff',
            textOn: '@textOn'
        },
        template: '<label class="toggle-switch colour">'
                    + '<input type="checkbox" ng-model="ngModel" ng-change="ngChange()"/>'
                    + '<span>{{textOff || "No"}}</span>'
                    + '<div>'
                        + '<span></span>'
                    + '</div>'
                    + '<span>{{textOn || "Yes"}}</span>'
                + '</label>',
        link: function (scope, element, attrs, ngModel) {
            var toggleWatch = scope.$watch(function () {
                return ngModel.$modelValue;
            }, function (modelValue) {
                if (modelValue === true) {
                    element.addClass('toggle-on');
                }
                else {
                    element.removeClass('toggle-on');
                }
            });
            scope.$on('$destroy', function() {
                if(toggleWatch) { toggleWatch(); }
            });
        }
    };
}])
.directive('commandValidator', [function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            var validate = function () {
                var command = scope.makeCommand();
                var valid = true;
                var validationMessages = [];
                var result = command.Validate();
                // If our field is pristine, don't add errors just yet
                if (!elm.$pristine) {
                    // Add new validation messages
                    for (var i = 0; i < result.length; i++) {
                        for (var key in result[i]) {
                            if (key === attrs.ngModel) {
                                validationMessages.push(result[i][key]);
                                valid = false;
                            }
                        }
                    }
                }

                scope.validationMessages[attrs.ngModel] = validationMessages;
                attrs.$set('title', validationMessages[0]);
                ctrl.$setValidity('commandValidation', valid);

                if (!scope.$$phase) { // Check if digest is already in progress.
                    scope.$apply();
                }

                return valid ? command : undefined;
            };
            elm.change(function () {
                validate();
            });
            validate();
        }
    };
}])
.directive('validateOnSubmit', ['ValidationForm', function (ValidationForm) {
    return {
        restrict: 'A',

        link: function (scope, elm, attrs, ctrl) {
            elm.click(function () {
                var form = elm.context.form;
                var ngForm = scope[form.name];
                ValidationForm.Form = ngForm;
            });
        }
    };
}])
.directive('spinClick', ['$parse', '$activityManager', function ($parse, $activityManager) {
    var spinner;
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            var keepWidth = attr.spinKeepWidth === 'true';

            var originalElementWidth = element.width();
            var scopeExpression = attr['spinClick'];
            var fn = $parse(scopeExpression),
            target = element[0];
            element.on('click', function (event) {
                scope.$apply(function () {
                    onActivityStart();
                    $activityManager.registerSpinListener(onActivityStop);
                    fn(scope, { $event: event });
                });
            });

            var onActivityStart = function () {
                var opts = {
                    length: 8,
                    radius: 8,
                    width: 5,
                    color: 'rgb(255,255,255)',
                    left: 2,
                    trail: 40
                }; // customize this "resizing and coloring" algorithm
                if (keepWidth === false) {
                    if(!originalElementWidth){
                        originalElementWidth = element.width();
                    } 
                    var newWidth = originalElementWidth + 50;
                    element.width(newWidth);
                    element.addClass('spin');
                }
                attr.$set('disabled', true);
                spinner = new Spinner(opts).spin(target);
                // expects a promise
                // http://docs.angularjs.org/api/ng.$q
            };
            var onActivityStop = function () {
                attr.$set('disabled', false);
                spinner.stop();
                element.css("width","");
                element.removeClass('spin');
            };
        }
    };
}])
.directive('spinLoader', ['$activityManager', function ($activityManager) {
    return {
        restrict: 'A',
        scope: {},
        link: function (scope, element, attr) {
            var spinner;
            var opts = {
                length: 12,
                radius: 12,
                width: 6,
                color: '#FF6600',
                trail: 40
            };
            var activityName = attr.spinLoader;
            var target = element;
            var timer;

            function onActivityStart() {
                timer = setTimeout(function () {
                    if (!spinner) {
                        spinner = new Spinner(opts)
                    }
                    spinner.spin(target[0]);
                    target.addClass('spinLoad');
                    target.find('*').attr('disabled', true);
                }, 200);
            }

            function onActivityStop() {
                if (timer) {
                    clearTimeout(timer);
                }
                if (spinner) {
                    spinner.stop();
                    target.removeClass('spinLoad');
                    target.find('*').attr('disabled', false);
                }
            }

            $activityManager.registerSpinListenerForKey(onActivityStart, onActivityStop, activityName, element[0].id);

            scope.$on('$destroy', function () {
                $activityManager.removeListenerForKey(element[0].id, activityName);
            });
        }
    };
}])
.directive('sahlAutocomplete', ['$parse', '$queryManager', '$capitecQueries', function ($parse, $queryManager, $capitecQueries) {
    var autoCompleteOptionsFilter = /([a-zA-Z\.]+)(?:\s+(in-query)\s+)([a-zA-Z]+)(?:\s+(for-model)\s+)([a-zA-Z\.]+)/;//(?<filterText>[a-zA-Z\.]+)\s+in-query\s+(?<query>[a-zA-Z\.]+)\s+for-model\s+(?<ngmodel>[a-zA-Z\.]+);
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.reset = false;
            element.data('selectedIndex', -1);
            var matches = autoCompleteOptionsFilter.exec(attr.sahlAutocomplete);

            var selectItem = matches[1],
                queryCommandAttr = matches[3],
                modelToFill = matches[5];

            //Get the width of the auto complete box.
            var inputWidth = element[0].offsetWidth;
            var margin = element[0].clientLeft;
            inputWidth = inputWidth - (margin * 2) - 2;

            var parsedSelectedItem = $parse(selectItem),
                parsedModelToFill = $parse(modelToFill);

            scope.GetTarget = function () {
                var htmlModel = document.getElementById(modelToFill);
                if (htmlModel)
                    htmlModel.style.width = inputWidth + 'px';
                return angular.element(htmlModel);
            };
            scope.GetFilterData = function () {
                return scope.GetTarget().children();
            };
            scope.ClearFilterData = function () {
                scope.GetTarget().empty();
            };

            scope.SetIndex = function (index) {
                var currentIndex = element.data('selectedIndex');
                if (currentIndex !== -1) {
                    angular.element(scope.GetTarget().children()[currentIndex]).removeClass('active');
                }
                if (index !== -1) {
                    angular.element(scope.GetTarget().children()[index]).addClass('active');
                    var top = angular.element(scope.GetTarget().children()[index]).offset().top - angular.element(scope.GetTarget().children()[0]).offset().top;
                    scope.GetTarget().animate({ scrollTop: top }, 'fastest');
                }
                element.data('selectedIndex', index);
            };

            scope.CreateLiElement = function (target, data) {
                var itemValue = parsedSelectedItem({ 'item': data });
                var ngItem = angular.element('<li>' + itemValue + '</li>');
                ngItem.mouseenter(function () {
                    scope.SetIndex(-1);
                    ngItem.addClass('active');
                });
                ngItem.mouseleave(function () {
                    ngItem.removeClass('active');
                });
                ngItem.bind('click', function () {
                    parsedModelToFill.assign(scope, data);
                    target.val(itemValue);
                    scope.$apply();
                    scope.GetTarget().hide();
                });
                return ngItem;
            };

            scope.HideTargetElement = function () {
                if (scope.reset === false) {
                    scope.GetTarget().hide();
                }
                else {
                    scope.reset = false;
                }
            };

            scope.GetTarget().hide();
            var modelToFillWatch = scope.$watch(modelToFill, function () {
                var data = parsedModelToFill(scope);
                if (data !== undefined) {
                    element.val(parsedSelectedItem({ 'item': data }));
                }
            });
            element.bind('keydown', function (blurEvent) {
                if (blurEvent.keyCode === 38 || blurEvent.keyCode === 40 || blurEvent.keyCode === 13 || blurEvent.keyCode === 27) {
                    var currentPostion = element.data('selectedIndex');
                    var itemLength = scope.GetTarget().children().length;
                    switch (blurEvent.keyCode) {
                        case 38:
                            if (currentPostion !== -1) {
                                scope.SetIndex(currentPostion - 1);
                            }
                            break;
                        case 40:
                            if (currentPostion !== (itemLength - 1)) {
                                scope.SetIndex(currentPostion + 1);
                            }
                            break;
                        case 13:
                            angular.element(itemLength = scope.GetTarget().children()[currentPostion]).click();
                            event.preventDefault(); // Doesn't work at all
                            return false; // Don't even know why
                        default:
                            scope.SetIndex(-1);
                            scope.GetTarget().hide();
                    }
                }
            });
            element.bind('propertychange keyup paste', function (blurEvent) {
                if (!scope.$$phase) {
                    scope.$apply(function () {
                        if (blurEvent.keyCode !== 38 && blurEvent.keyCode !== 40 && blurEvent.keyCode !== 13 && blurEvent.keyCode !== 27) {
                            var text = element.val();
                            if (text.length >= 3) {
                                var fn = $parse(queryCommandAttr);
                                var command = fn($capitecQueries);

                                $queryManager.sendQueryAsync(new command(text)).then(function (data) {
                                    scope.DataItems = data.data.ReturnData.Results.$values;
                                    scope.ClearFilterData();
                                    scope.SetIndex(-1);
                                    scope.GetTarget().animate({ scrollTop: 0 }, 'fast');
                                    var targetElement = scope.GetTarget();
                                    targetElement.mouseenter(function () {
                                        scope.FilterListActive = true;
                                        scope.reset = true;
                                    });
                                    targetElement.mouseleave(function () {
                                        scope.FilterListActive = false;
                                        if (scope.searchTextActive !== true) {
                                            scope.reset = false;
                                            setTimeout(function () { scope.HideTargetElement(); }, 3000);
                                        }
                                    });
                                    if (scope.DataItems.length !== 0) {
                                        scope.ClearFilterData();
                                        angular.forEach(scope.DataItems, function (item) {
                                            var ngItem = scope.CreateLiElement(element, item);
                                            targetElement.append(ngItem);
                                        });
                                    }
                                    targetElement.show();
                                });
                            } else {
                                scope.ClearFilterData();
                                scope.GetTarget().hide();
                            }
                        }
                    });
                }
            });
            element.bind('focus', function () {
                scope.searchTextActive = true;
                scope.reset = true;
            });
            element.bind('blur', function () {
                scope.searchTextActive = false;
                if (scope.FilterListActive !== true) {
                    scope.reset = false;
                    setTimeout(function () { scope.HideTargetElement(); }, 3000);
                }
            });

            scope.$on('$destroy', function() {
                if(modelToFillWatch) { modelToFillWatch(); }
            });
        }
    };
}])
.directive('qtip', ['$parse', function ($parse) {
    return {
        restrict: 'A',        // directive is an Element (not Attribute)
        scope: {              // set up directive's isolated scope
            msg: '@'          // name var passed by value (string, one-way)
        },
        template:             // replacement HTML (can use our scope vars here)
        ' <div class=\"toolTip\">' +
                    '<span style=\"display:none;\">' +
                        '<div>' +
                            '{{msg}}' +
                        '</div>' +
                        '<img class=\"toolTipArrow\" alt=\"\" src=\"assets/img/tooltip-open-arrow.png\" />' +
                    '</span>' +
                '</div>',
        replace: false,        // replace original markup with template
        transclude: false,    // do not copy original HTML content
        link: function (scope, element, attr) {
            var originalElementWidth = element.width();
            var scopeExpression = attr['qtip'];
            var fn = $parse(scopeExpression),
            target = element[0];

            element.click(function (event) {
                scope.$apply(function () {
                    element.find('span').slideToggle();
                    return false;
                });
            });
            angular.element('form input').focus(function () {
                element.find('span').fadeOut();
                return false;
            });
        }
    };
}])
.directive('validationtooltip', ['$parse', function ($parse) {
    return {
        restrict: 'A',        // directive is an Element (not Attribute)
        scope: {              // set up directive's isolated scope
            msg: '@',          // name var passed by value (string, one-way)
        },
        template:             // replacement HTML (can use our scope vars here)
        ' <div class=\"validationTooltip\">' +
                    '<span style=\"display:none;\">' +
                        '<div>' +
                            '{{msg}}' +
                        '</div>' +
                        '<div class=\"toolTipArrow\" />' +
                    '</span>' +
                '</div>',
        replace: false,        // replace original markup with template
        transclude: false,    // do not copy original HTML content
        link: function (scope, element, attr) {
            var originalElementWidth = element.width();
            var scopeExpression = attr['validationTooltip'];
            var fn = $parse(scopeExpression),
            target = element[0];

            var watch = null;
            watch = scope.$watch(function () { return element.is(':visible'); }, function () {
                if (element.find('span').is(':hidden')) {
                    element.find('span').slideToggle();
                }
                return false;
            });
            scope.$on('destroy', function () {
                if (watch != null)
                    watch();
                watch = null;
            });


            element.click(function (event) {
                scope.$apply(function () {
                    element.find('span').slideToggle();
                    return false;
                });
            });


            angular.element('form input').focus(function () {
                element.find('span').fadeOut();
                return false;
            });
        }
    };
}])
.directive('paginator', function () {
    return {
        restrict: 'A',
        replace: true,
        template:
        '<div id="pages" ng-show="paginationOptions.totalPages > 0">' +
            '<span id="prevSet" ng-show="paginationOptions.currentPage > 1">' +
                '<a ng-click="getPage(1, sortOptions.sortDirection)">|<&nbsp;&nbsp;</a>' +
                '<a ng-click="getPage(paginationOptions.currentPage-1, sortOptions.sortDirection)"><<</a>' +
            '</span>' +
            '<span>&nbsp;&nbsp;Page {{paginationOptions.currentPage}} of {{paginationOptions.totalPages}}&nbsp;&nbsp;</span>' +
            '<span id="nextSet" ng-show="paginationOptions.currentPage < paginationOptions.totalPages">' +
                '<a ng-click="getPage(paginationOptions.currentPage+1, sortOptions.sortDirection)">>></a>' +
                '<a ng-click="getPage(paginationOptions.totalPages, sortOptions.sortDirection)">&nbsp;&nbsp;>|</a>' +
            '</span>' +
            '<span class="paginatorResults" ng-show="paginationOptions.totalResults > 0">&nbsp;&nbsp;Total results returned {{paginationOptions.totalResults}}</span>' +
        '</div>'
    };
})
.directive('filterer', function () {
    return {
        restrict: 'A',
        replace: true,
        template: '<div id=\"filterer\">' +
                    '<label>Filter:&nbsp;</label>' +
                    '<input ng-model=\"filterInput\" focus=\"true\" />' +
                    '<label>&nbsp;{{filterOptions.filterDescription}}</label>' +
                  '</div>'
    };
})
.directive('sorter', function () {
    return {
        restrict: 'A',
        replace: true,
        template: '<span>' +
                    '<button class=\"textButton\" ng-click=\"getPage(paginationOptions.currentPage, \'ascending\')\" ng-show=\"sortOptions.sortDirection == \'descending\'\">' +
                        '<img src=\"assets/img/arrow-up.gif\" />' +
                    '</button>' +
                    '<button class=\"textButton\" ng-click=\"getPage(paginationOptions.currentPage, \'descending\')\" ng-show=\"sortOptions.sortDirection ==\'ascending\'\">' +
                        '<img src=\"assets/img/arrow-down.gif\" />' +
                    '</button>' +
                '</span>'
    };
})

.directive('dropdownDatepicker', [function () {
    function getDays(month, year) {
        var days = [];
        var daysInMonth = 31;
        if (month) {
            if (!year) { year = new Date().getFullYear(); }
            daysInMonth = new Date(year, month, 0).getDate();
        }
        for (var i = 1; i <= daysInMonth; i++) {
            days.push(prependZero(i));
        }
        return days;
    }

    function getMonths() {
        var months = [];
        for (var i = 1; i <= 12; i++) {
            months.push(prependZero(i));
        }
        return months;
    }

    function getYears(minYear, maxYear) {
        var years = [];
        var thisYear = new Date().getFullYear();
        minYear = minYear || thisYear - 100;
        maxYear = maxYear || thisYear;
        for (var i = minYear; i <= maxYear; i++) {
            years.push(i.toString());
        }
        return years;
    }

    function prependZero(number) {
        if (number.toString().length < 2) return '0' + number;
        return number.toString();
    }

    return {
        restrict: 'A',
        replace: true,
        scope: {
            date: '=',
            minYear: '=',
            maxYear: '='
        },
        template: '<div class="datepickerDropdown">'
                    + '<label>Day : </label>'
                    + '<span class="selectContainer dayMonthSelect">'
                        + '<select name="day" ng-model="date.day" ng-options="d for d in days" />'
                        + '<span>{{date.day}}</span>'
                    + '</span>'
                    + '<label>Month : </label>'
                    + '<span class="selectContainer dayMonthSelect" >'
                        + '<select ng-model="date.month" ng-options="m for m in months">'
                        + '</select>'
                        + '<span>{{date.month}}</span>'
                    + '</span>'
                    + '<label>Year : </label>'
                    + '<span class="selectContainer yearSelect">'
                        + '<select ng-model="date.year" ng-options="y for y in years" />'
                        + '<span>{{date.year}}</span>'
                    + '</span>'
                + '</div>',
        link: function (scope, element, attr) {
            var monthWatch = scope.$watch('date.month', function () {
                updateDate();
            });
            var yearWatch = scope.$watch('date.year', function () {
                updateDate();
            });
            scope.months = getMonths();
            scope.years = getYears(scope.minYear, scope.maxYear);

            function updateDate() {
                if (!scope.date) {
                    scope.date = {
                        day: "",
                        month: "",
                        year: ""
                    };
                }
                scope.days = getDays(scope.date.month, scope.date.year);
                if (scope.date.year) {
                    var year = getNumberInRange(scope.date.year, scope.years[scope.years.length - 1], scope.years[0]);
                    year = year + "";
                }
                if (scope.date.month) {
                    var month = getNumberInRange(scope.date.month, 12, 1);
                    month = prependZero(month);
                }
                if (scope.date.day) {
                    var day = getNumberInRange(scope.date.day, scope.days[scope.days.length - 1], 1);
                    day = prependZero(day);
                }

                scope.date.year = year ? year : '';
                scope.date.month = month ? month : '';
                scope.date.day = day ? day : '';
            }

            function getNumberInRange(number, max, min) {
                if (Number(number) > Number(max)) return Number(max);
                if (Number(number) < Number(min)) return Number(min);
                return Number(number);
            }

            scope.$on('$destroy', function() {
                if(monthWatch) { monthWatch(); }
                if(yearWatch) { yearWatch(); }
            });
        }
    };
}])
.directive('format', ['$parse', function ($parse) {
    return {
        require: 'ngModel',
        restrict: 'EACM',
        link: function (scope, element, attr, ngModel) {
            if (!ngModel)
                return;

            ngModel.$parsers.unshift(function (a) {
                return Number(a.replace(/[^\d|\-+|\.+]/g, ''));
            });

            ngModel.$formatters.unshift(function (viewValue) {
                element.val(viewValue); // initialise the value
                element.numberFormat(attr.format);
                return element.val();
            });
        }
    };
}])

/**
 * focus
 * 
 * Use this to have an element grab focus on startup,
 * and to grab focus from within a controller. *
 * Usage: <input focus="isFocused">  - where isFocused is a boolean property defined in $scope.
 * Set $scope.isFocused = true to have the input be focused on first load.
 */
.directive('focus', function ($timeout, $parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var destroyWatch = scope.$watch(attrs.focus, function (newValue, oldValue) {
                if (newValue) { element[0].focus(); }
                else { element[0].blur(); }
            });

            /**
             * Fires when the field loses focus
             */
            var onBlurHandler = function (e) {
                $timeout(function () {
                    attrs.focus = false;
                });
            };
            element.bind("blur", onBlurHandler);

            /**
             * Fires when the field gains focus
             */
            var onFocusHandler = function (e) {
                $timeout(function () {
                    attrs.focus = true;
                });
            };
            element.bind("focus", onFocusHandler);

            /*Clean up handlers */
            scope.$on('$destroy', function () {
                if (element) {
                    element.unbind('blur', onBlurHandler);
                    element.unbind('focus', onFocusHandler);
                }
                if(destroyWatch) {
                    destroyWatch();
                }
            });
        }
    };
});

(function ($) {
    $.fn.numberFormat = function (format) {

        function key_check(e) {
            var keyPressed = (e.keyCode ? e.keyCode : e.which);
            var typed = String.fromCharCode(keyPressed);
            var decimalLimit = 2;
            var validKeyPressed = false;
            var tooManyDecimals = false;
            var oldValue = obj.val();

            var newValue = oldValue + typed;

            // 8=backspace, 9=tab, 13=enter, 35=end, 36=home, 37=left arrow, 39=right arrow, 46=delete, 48-57=0 to 9, 96-105=numpad 0 to 9
            var allowedKeyCodes = [8, 9, 13, 35, 36, 37, 39, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];
            var numbersOnly = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];

            // add decimal point & period to allowed keyCodes 
            if (format === "float" || format === "currency") {
                allowedKeyCodes.push(110);
                allowedKeyCodes.push(190);
            }

            // check if key pressed is in the list of allowed keys
            if (($.inArray(keyPressed, allowedKeyCodes)) !== -1)
                validKeyPressed = true;

            // check to see if we have too many decimals if applicable
            if ($.inArray(keyPressed, numbersOnly) !== -1 && newValue.indexOf(".") > -1 && (newValue.split("."))[1].length > decimalLimit)
                tooManyDecimals = true;

            if (validKeyPressed && tooManyDecimals)
                validKeyPressed = false;

            // cancel the keypress if not allowed
            if (!validKeyPressed) {
                e.preventDefault();
                e.stopPropagation();
                if (oldValue != newValue)
                    obj.val(oldValue);
            }
        }

        function format_it() {
            var str = obj.val();
            var formatted = number_format(str);
            if (str != formatted)
                obj.val(formatted);
        }

        function number_format(str) {
            var hasDecimal = str.indexOf(".") > 0;
            var decimalSplit = str.split(".");
            var intPart = decimalSplit[0];
            var decPart = decimalSplit[1] + "";
            intPart = intPart.replace(/[^\d]/g, '');
            if (intPart.length > 3) {
                var intDiv = Math.floor(intPart.length / 3);
                while (intDiv > 0) {
                    var lastComma = intPart.indexOf(",");
                    if (lastComma < 0) { lastComma = intPart.length; }
                    if (lastComma - 3 > 0) { intPart = intPart.splice(lastComma - 3, 0, ","); }
                    intDiv--;
                }
            }

            var formattedValue = intPart;
            if (hasDecimal)
                formattedValue += "." + decPart;

            if (format === "currency") {
                formattedValue = "R " + formattedValue;
            }

            return formattedValue;
        }

        String.prototype.splice = function (idx, rem, s) {
            return (this.slice(0, idx) + s + this.slice(idx + Math.abs(rem)));
        };

        $(this).bind('keydown.number_format', key_check);
        $(this).bind('keyup.number_format', format_it);
        $(this).bind('focusout.number_format', format_it);

        var obj = $(this);

        if ($(this).val().length > 0) {
            format_it();
        }
        format_it();
    };
})(jQuery);

