'use strict';

describe('[sahl.websites.halo.services.paginationHelper]', function () {
    beforeEach(module('sahl.websites.halo.services.paginationHelpers'));
    beforeEach(module('sahl.js.core.logging'));

    beforeEach(inject(function ($injector, $q) { }));

    describe(' - (Service: paginationHelper)-', function () {
        var $paginationHelper;
        beforeEach(inject(function ($injector, $q) {
            $paginationHelper = $injector.get('$paginationHelperService');
        }));

        // paginating with zero pages
        describe('when paginating with no pages', function () {
            var pages = null;
            var pageCount = 0;
            var maxPerPage = 10;
            var currentPage = 1;

            beforeEach(inject(function ($injector, $q) {
                pages = $paginationHelper.buildPagination(pageCount, maxPerPage, currentPage);
            }));

            it('should not enable the previous page button', function () {
                expect(pages.isPrevPageEnabled).toEqual(false);
            });

            it('should not enable the previous page group button', function () {
                expect(pages.isPrevPageGroupEnabled).toEqual(false);
            });

            it('should not enable the next page button', function () {
                expect(pages.isNextPageEnabled).toEqual(false);
            });

            it('should not enable the next page group button', function () {
                expect(pages.isNextPageGroupEnabled).toEqual(false);
            });

            it('should create the correct number of pages for the page group', function () {
                expect(pages.pages.length).toEqual(pageCount);
            });
        });

        // paginating with less than max pages per page group
        describe('when paginating with only one page', function () {
            var pages = null;
            var pageCount = 1;
            var maxPerPage = 10;

            describe('given the first page is the current page', function () {
                var currentPage = 1;
                beforeEach(inject(function ($injector, $q) {
                    pages = $paginationHelper.buildPagination(pageCount, maxPerPage, currentPage);
                }));

                it('should not enable the previous page button', function () {
                    expect(pages.isPrevPageEnabled).toEqual(false);
                });

                it('should not enable the previous page group button', function () {
                    expect(pages.isPrevPageGroupEnabled).toEqual(false);
                });

                it('should not enable the next page button', function () {
                    expect(pages.isNextPageEnabled).toEqual(false);
                });

                it('should not enable the next page group button', function () {
                    expect(pages.isNextPageGroupEnabled).toEqual(false);
                });

                it('should create the correct number of pages for the page group', function () {
                    expect(pages.pages.length).toEqual(pageCount);
                    expect(pages.pages[0].pageNum).toEqual(1);
                });
            });
        });

        // paginating with less than max pages per page group
        describe('when paginating with fewer pages than pages displayed per page group', function () {
            var pages = null;
            var pageCount = 8;
            var maxPerPage = 10;

            describe('given the first page is the current page', function () {
                var currentPage = 1;
                beforeEach(inject(function ($injector, $q) {
                    pages = $paginationHelper.buildPagination(pageCount, maxPerPage, currentPage);
                }));

                it('should not enable the previous page button', function () {
                    expect(pages.isPrevPageEnabled).toEqual(false);
                });

                it('should not enable the previous page group button', function () {
                    expect(pages.isPrevPageGroupEnabled).toEqual(false);
                });

                it('should enable the next page button', function () {
                    expect(pages.isNextPageEnabled).toEqual(true);
                });

                it('should not enable the next page group button', function () {
                    expect(pages.isNextPageGroupEnabled).toEqual(false);
                });

                it('should create the correct number of pages for the page group', function () {
                    expect(pages.pages.length).toEqual(pageCount);
                    expect(pages.pages[0].pageNum).toEqual(1);
                    expect(pages.pages[1].pageNum).toEqual(2);
                });
            });

            describe('given the last page is the current page', function () {
                var currentPage = pageCount;
                beforeEach(inject(function ($injector, $q) {
                    pages = $paginationHelper.buildPagination(pageCount, maxPerPage, currentPage);
                }));

                it('should enable the previous page button', function () {
                    expect(pages.isPrevPageEnabled).toEqual(true);
                });

                it('should not enable the previous page group button', function () {
                    expect(pages.isPrevPageGroupEnabled).toEqual(false);
                });

                it('should not enable the next page button', function () {
                    expect(pages.isNextPageEnabled).toEqual(false);
                });

                it('should not enable the next page group button', function () {
                    expect(pages.isNextPageGroupEnabled).toEqual(false);
                });

                it('should create the correct number of pages for the page group', function () {
                    expect(pages.pages.length).toEqual(pageCount);
                });
            });
        });

        describe('when paginating with more pages than pages displayed per page group', function () {
            var pages = null;
            var pageCount = 12;
            var maxPerPage = 10;

            describe('given the first page is the current page', function () {
                var currentPage = 1;
                beforeEach(inject(function ($injector, $q) {
                    pages = $paginationHelper.buildPagination(pageCount, maxPerPage, currentPage);
                }));

                it('should not enable the previous page button', function () {
                    expect(pages.isPrevPageEnabled).toEqual(false);
                });

                it('should not enable the previous page group button', function () {
                    expect(pages.isPrevPageGroupEnabled).toEqual(false);
                });

                it('should enable the next page button', function () {
                    expect(pages.isNextPageEnabled).toEqual(true);
                });

                it('should not enable the next page group button', function () {
                    expect(pages.isNextPageGroupEnabled).toEqual(true);
                });

                it('should create the correct number of pages for the page group', function () {
                    expect(pages.pages.length).toEqual(maxPerPage);
                });
            });

            describe('given the last page of the first group is the current page', function () {
                var currentPage = maxPerPage;
                beforeEach(inject(function ($injector, $q) {
                    pages = $paginationHelper.buildPagination(pageCount, maxPerPage, currentPage);
                }));

                it('should enable the previous page button', function () {
                    expect(pages.isPrevPageEnabled).toEqual(true);
                });

                it('should not enable the previous page group button', function () {
                    expect(pages.isPrevPageGroupEnabled).toEqual(false);
                });

                it('should enable the next page button', function () {
                    expect(pages.isNextPageEnabled).toEqual(true);
                });

                it('should enable the next page group button', function () {
                    expect(pages.isNextPageGroupEnabled).toEqual(true);
                });

                it('should create the correct number of pages for the page group', function () {
                    expect(pages.pages.length).toEqual(maxPerPage);
                });
            });

            describe('given the last page is the current page', function () {
                var currentPage = pageCount;
                beforeEach(inject(function ($injector, $q) {
                    pages = $paginationHelper.buildPagination(pageCount, maxPerPage, currentPage);
                }));

                it('should enable the previous page button', function () {
                    expect(pages.isPrevPageEnabled).toEqual(true);
                });

                it('should enable the previous page group button', function () {
                    expect(pages.isPrevPageGroupEnabled).toEqual(true);
                });

                it('should not enable the next page button', function () {
                    expect(pages.isNextPageEnabled).toEqual(false);
                });

                it('should not enable the next page group button', function () {
                    expect(pages.isNextPageGroupEnabled).toEqual(false);
                });

                it('should create the correct number of pages for the page group', function () {
                    expect(pages.pages.length).toEqual(pageCount - maxPerPage);
                });
            });
        })
    });
});