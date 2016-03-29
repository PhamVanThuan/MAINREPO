'use strict';
angular.module('sahl.websites.halo.services.paginationHelpers', [

    ])
    .service('$paginationHelperService', [
        function() {
            return {
                buildPagination: function(numberOfTotalPages, maxDisplayedPages, currentPage) {
                    var isNextPageEnabled = false;
                    var isNextPageGroupEnabled = false;
                    var isPrevPageEnabled = false;
                    var isPrevPageGroupEnabled = false;
                    var pages = [];

                    // work out the current page groups bounds
                    var totalPageGroups = Math.ceil(numberOfTotalPages / maxDisplayedPages);
                    var currentPageGroup = Math.ceil(currentPage / maxDisplayedPages);
                    var currentPageGroupStart = (currentPageGroup - 1) * maxDisplayedPages + 1;
                    var pagesInGroup = maxDisplayedPages;
                    if (numberOfTotalPages === 0) {
                        pagesInGroup = 0;
                    }

                    if (totalPageGroups === currentPageGroup && (numberOfTotalPages % maxDisplayedPages) > 0) {
                        pagesInGroup = numberOfTotalPages % maxDisplayedPages;
                    }

                    if (currentPage > 1) {
                        isPrevPageEnabled = true;
                    }
                    if (currentPageGroup > 1) {
                        isPrevPageGroupEnabled = true;
                    }
                    if (currentPageGroup < totalPageGroups) {
                        isNextPageGroupEnabled = true;
                    }
                    if (currentPage < numberOfTotalPages) {
                        isNextPageEnabled = true;
                    }

                    for (var i = currentPageGroupStart; i < (currentPageGroupStart + pagesInGroup); i++) {
                        pages.push({
                            pageNum: i,
                            isSelected: currentPage === i
                        });
                    }

                    return {
                        isNextPageEnabled: isNextPageEnabled,
                        isNextPageGroupEnabled: isNextPageGroupEnabled,
                        isPrevPageEnabled: isPrevPageEnabled,
                        isPrevPageGroupEnabled: isPrevPageGroupEnabled,
                        pages: pages
                    };
                }
            };
        }
    ]);
