'use strict';

/* Directives */

angular.module('sahl.tools.app.directives', [])
.directive('appVersion', ['version', function (version) {
    return function (scope, elm, attrs) {
        elm.text(version);
    };
}])
.directive('backImg', function () {
    return function (scope, element, attrs) {
        attrs.$observe('backImg', function (value) {
            element.css({
                'background-image': 'url(' + value + ')',
                'background-size': 'auto',
                'background-repeat': 'no-repeat',
                'background-position': 'center'
            });
        });
    };
})
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
            scope.$watch(function () {
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
        }
    }
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
            scope.$watch(function () {
                return ngModel.$modelValue;
            }, function (modelValue) {
                if (modelValue === true) {
                    element.addClass('toggle-on');
                }
                else {
                    element.removeClass('toggle-on');
                }
            });
        }
    }
}])
.directive('commandValidator', ['$interpolate', function ($interpolate) {
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
                attrs.$set("title", validationMessages[0]);
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
    }
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
    }
}])
.directive('spinClick', ['$parse', '$activityManager', function ($parse, $activityManager) {
    var spinner;
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            var keepWidth = attr.spinKeepWidth === "true";

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
                    element.width(originalElementWidth + 40);
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
                element.width(originalElementWidth); // restore size
                element.removeClass('spin');
            };
        }
    }
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
                color: 'rgb(237, 121, 0)',
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
                    spinner.spin(target[0]);;
                    target.addClass('spinLoad');
                    target.find("*").attr('disabled', true);
                }, 200);
            }

            function onActivityStop() {
                if (timer) {
                    clearTimeout(timer);
                }
                if (spinner) {
                    spinner.stop();
                    target.removeClass('spinLoad');
                    target.find("*").attr('disabled', false);
                }
            }

            $activityManager.registerSpinListenerForKey(onActivityStart, onActivityStop, activityName, element[0].id);

            scope.$on('$destroy', function () {
                $activityManager.removeListenerForKey(element[0].id, activityName);
            });

        }
    }
}])
.directive('sahlAutocomplete', ['$parse', '$queryManager', '$decisionTreeDesignQueries', function ($parse, $queryManager, $decisionTreeDesignQueries) {
    var autoCompleteOptionsFilter = /([a-zA-Z\.]+)(?:\s+(in-query)\s+)([a-zA-Z]+)(?:\s+(for-model)\s+)([a-zA-Z\.]+)/;//(?<filterText>[a-zA-Z\.]+)\s+in-query\s+(?<query>[a-zA-Z\.]+)\s+for-model\s+(?<ngmodel>[a-zA-Z\.]+);
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            scope.reset = false;
            element.data("selectedIndex", -1);
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
                    htmlModel.style.width = inputWidth + "px";
                return angular.element(htmlModel)
            }
            scope.GetFilterData = function () {
                return scope.GetTarget().children();
            }
            scope.ClearFilterData = function () {
                scope.GetTarget().empty();
            }

            scope.SetIndex = function (index) {
                var currentIndex = element.data("selectedIndex");
                if (currentIndex !== -1) {
                    angular.element(scope.GetTarget().children()[currentIndex]).removeClass("active");
                }
                if (index !== -1) {
                    angular.element(scope.GetTarget().children()[index]).addClass("active");
                    var top = angular.element(scope.GetTarget().children()[index]).offset().top - angular.element(scope.GetTarget().children()[0]).offset().top;
                    scope.GetTarget().animate({ scrollTop: top }, "fastest");
                }
                element.data("selectedIndex", index);
            }

            scope.CreateLiElement = function (target, data) {
                var itemValue = parsedSelectedItem({ 'item': data });
                var ngItem = angular.element("<li>" + itemValue + "</li>");
                ngItem.mouseenter(function () {
                    scope.SetIndex(-1);
                    ngItem.addClass("active")
                });
                ngItem.mouseleave(function () {
                    ngItem.removeClass("active")
                });
                ngItem.bind('click', function () {
                    parsedModelToFill.assign(scope, data);
                    target.val(itemValue);
                    scope.$apply();
                    scope.GetTarget().hide();
                });
                return ngItem;
            }

            scope.HideTargetElement = function () {
                if (scope.reset === false) {
                    scope.GetTarget().hide();
                }
                else {
                    scope.reset = false;
                }
            }

            scope.GetTarget().hide();
            scope.$watch(modelToFill, function () {
                var data = parsedModelToFill(scope);
                if (data !== undefined) {

                    element.val(parsedSelectedItem({ 'item': data }));
                }
            })
            element.bind("keydown", function (blurEvent) {
                if (blurEvent.keyCode === 38 || blurEvent.keyCode === 40 || blurEvent.keyCode === 13 || blurEvent.keyCode === 27) {
                    var currentPostion = element.data("selectedIndex");
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
                            window.stop(); // Works in all browsers but IE    
                            document.execCommand("Stop"); // Works in IE
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
                                var command = fn($decisionTreeDesignQueries);

                                $queryManager.sendQueryAsync(new command(text)).then(function (data) {
                                    scope.DataItems = data.data.ReturnData.Results.$values;
                                    scope.ClearFilterData();
                                    scope.SetIndex(-1);
                                    scope.GetTarget().animate({ scrollTop: 0 }, "fast");
                                    var targetElement = scope.GetTarget();
                                    targetElement.mouseenter(function () {
                                        scope.FilterListActive = true;
                                        scope.reset = true;
                                    })
                                    targetElement.mouseleave(function () {
                                        scope.FilterListActive = false;
                                        if (scope.searchTextActive !== true) {
                                            scope.reset = false;
                                            setTimeout(function () { scope.HideTargetElement() }, 3000);
                                        }
                                    })
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
                    setTimeout(function () { scope.HideTargetElement() }, 3000);
                }
            });
        }
    }
}])
.directive("qtip", ['$parse', '$activityManager', function ($parse, $activityManager) {
    return {
        restrict: "A",        // directive is an Element (not Attribute)
        scope: {              // set up directive's isolated scope
            msg: "@",          // name var passed by value (string, one-way)
        },
        template:             // replacement HTML (can use our scope vars here)
        " <div class=\"toolTip\">" +
                    "<span style=\"display:none;\">" +
                        "<div>" +
                            "{{msg}}" +
                        "</div>" +
                        "<img class=\"toolTipArrow\" alt=\"\" src=\"assets/img/tooltip-open-arrow.png\" />" +
                    "</span>" +
                "</div>",
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
            angular.element("form input").focus(function () {
                element.find('span').fadeOut();
                return false;
            });
        }
    }
}])
.directive("paginator", function () {
    return {
        restrict: "A",
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
    }
})
.directive("filterer", function () {
    return {
        restrict: "A",
        replace: true,
        template: "<div id=\"filterer\">" +
                    "<label>Filter:&nbsp;</label>" +
                    "<input ng-model=\"filterInput\" />" +
                    "<label>&nbsp;{{filterOptions.filterDescription}}</label>" +
                  "</div>"
    }
})
.directive("sorter", function () {
    return {
        restrict: "A",
        replace: true,
        template: "<span>" +
                    "<button class=\"textButton\" ng-click=\"getPage(paginationOptions.currentPage, 'ascending')\" ng-show=\"sortOptions.sortDirection == 'descending'\">" +
                        "<img src=\"assets/img/arrow-up.gif\" />" +
                    "</button>" +
                    "<button class=\"textButton\" ng-click=\"getPage(paginationOptions.currentPage, 'descending')\" ng-show=\"sortOptions.sortDirection == 'ascending'\">" +
                        "<img src=\"assets/img/arrow-down.gif\" />" +
                    "</button>" +
                "</span>"
    }
})
.directive("treeIcon", function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            var svgFunction = $parse(attr.treeIcon);
            var svgText = svgFunction(scope);
            var svgElement = angular.element(svgText);
            var padding = (element.width() - svgElement.width()) / 4;
            svgElement.css("margin", padding + "px");
            element.append(svgElement);
        }
    }
}).directive("dockpanel", ['$parse', '$parseHttpService', '$compile', '$q', function ($parse, $parseHttpService, $compile, $q) {
    return {
        restrict: "A",        // directive is an Element (not Attribute)
        scope: false,
        template:             // replacement HTML (can use our scope vars here)
        " <div><form id =\"docContent\"> {{msg}}  </form></div>",
        replace: true,        // replace original markup with template
        transclude: false,    // do not copy original HTML content
        controller: function ($scope, $element, $attrs) {
            var _dockcontent = $("#docContent");
            var _dockright = $element;
            var _maindock = _dockright.siblings(".dockmain").eq(0);
            _dockright.hide();

            $scope.dockVisible = false;

            $scope.dockPanelSetContent = function (stringContent) {
                $parseHttpService.getString(stringContent).then(function (templateString) {
                    var tpl = $compile(templateString)($scope);
                    _dockcontent.replaceWith(tpl);
                });

            };
            $scope.showDockBar = function () {
                var deferred = $q.defer();
                $scope.dockVisible = true;
                _maindock.width("80%");
                _dockright.show("slide", { direction: 'right' }, 500, function () {
                    deferred.resolve();
                });
                return deferred.promise;
            }

            $scope.hideDockBar = function () {
                var deferred = $q.defer();
                $scope.dockVisible = false;
                _dockright.hide("slide", { direction: 'right' }, 500, function () {
                    _maindock.width("100%");
                    deferred.resolve();
                });
                return deferred.promise;
            }



        },
        link: function (scope, element, attr) {

        }
    }
}])
.directive('refreshAfterLoad', ['$parse', function ($parse) {
    var spinner;
    return {
        restrict: 'A',
        link: function (scope, element, attr) {

            scope.requestUpdate();
        }
    }
}])
.directive('focusMe', function ($timeout, $parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.focusMe);
            scope.$watch(model, function (value) {
                console.log('value=', value);
                if (value === true) {
                    $timeout(function () {
                        element[0].focus();
                    });
                }
            });
        }
    };
}).directive('isNumber', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            var keyCode = [8, 9, 35, 36, 37, 39, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];
            var numbersOnly = [48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105];
            var negatives = [109, 189];
            var maxLength = 32;
            if (attr.isNumber === "float") {
                keyCode.push(110);
                keyCode.push(190);
            }
            element.bind("keydown", function (event) {
                var caretStart = attr.type === "number" ? 0 : this.selectionStart;
                if (
                        ($.inArray(event.which, keyCode) === -1 && $.inArray(event.which, negatives) === -1)
                        || ($.inArray(event.which, numbersOnly) !== -1 && element.val().length == maxLength)
                        || (element.val().length === 0 && $.inArray(event.which, numbersOnly) === -1 && $.inArray(event.which, negatives) === -1)
                        || ($.inArray(event.which, negatives) === 1 && caretStart !== 0)
                        || ($.inArray(event.which, negatives) === 1 && element.val().indexOf('-') === 0)
                        || (attr.isNumber === "float" && $.inArray(event.which, [110, 190]) !== -1 && element.val().indexOf(".") > -1)
                ) {
                    scope.$apply(function () {
                        scope.$eval(attr.isNumber);
                        event.preventDefault();
                    });
                    event.preventDefault();
                }

            });
        }
    };
}).directive('contenteditable', ['$timeout', function ($timeout) {
      return {
          restrict: 'A',
          require: '?ngModel',
          link: function (scope, element, attrs, ngModel) {
              // don't do anything unless this is actually bound to a model
              if (!ngModel) {
                  return
              }

              // options
              var opts = {}
              angular.forEach([
                'stripBr',
                'noLineBreaks',
                'selectNonEditable',
                'moveCaretToEndOnChange',
              ], function (opt) {
                  var o = attrs[opt]
                  opts[opt] = o && o !== 'false'
              })

              // view -> model
              element.bind('input', function (e) {
                  scope.$apply(function () {
                      var html, html2, rerender
                      html = element.html()
                      rerender = false
                      if (opts.stripBr) {
                          html = html.replace(/<br>$/, '')
                      }
                      if (opts.noLineBreaks) {
                          html2 = html.replace(/<div>/g, '').replace(/<br>/g, '').replace(/<\/div>/g, '')
                          if (html2 !== html) {
                              rerender = true
                              html = html2
                          }
                      }
                      ngModel.$setViewValue(html)
                      if (rerender) {
                          ngModel.$render()
                      }
                      if (html === '') {
                          // the cursor disappears if the contents is empty
                          // so we need to refocus
                          $timeout(function () {
                              element[0].blur()
                              element[0].focus()
                          })
                      }
                  })
              })

              // model -> view
              var oldRender = ngModel.$render
              ngModel.$render = function () {
                  var el, el2, range, sel
                  if (!!oldRender) {
                      oldRender()
                  }
                  element.html(ngModel.$viewValue || '')
                  if (opts.moveCaretToEndOnChange) {
                      el = element[0]
                      range = document.createRange()
                      sel = window.getSelection()
                      if (el.childNodes.length > 0) {
                          el2 = el.childNodes[el.childNodes.length - 1]
                          range.setStartAfter(el2)
                      } else {
                          range.setStartAfter(el)
                      }
                      range.collapse(true)
                      sel.removeAllRanges()
                      sel.addRange(range)
                  }
              }
              if (opts.selectNonEditable) {
                  element.bind('click', function (e) {
                      var range, sel, target
                      target = e.toElement
                      if (target !== this && angular.element(target).attr('contenteditable') === 'false') {
                          range = document.createRange()
                          sel = window.getSelection()
                          range.setStartBefore(target)
                          range.setEndAfter(target)
                          sel.removeAllRanges()
                          sel.addRange(range)
                      }
                  })
              }
          }
      }
  }]);