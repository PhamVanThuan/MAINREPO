'use strict';
describe('[sahl.js.core.fluentRestQuery]', function () {
    var whereBuilder;
    beforeEach(module('sahl.js.core.fluentRestQuery', function ($provide) {
        whereBuilder = function (fieldName, callback) {
            return {
                endWhere: function () {
                    var jsonReturned = {AND: {eq: {}}};
                    jsonReturned.AND.eq[fieldName] = 12;
                    callback(jsonReturned);
                }
            };
        };
        $provide.value('$fluentWhereBuilder', whereBuilder);
    }));

    var fluentRestQueryTemplate;
    var fluentRestQuery;

    beforeEach(inject(function ($fluentRestQuery) {
        fluentRestQuery = new $fluentRestQuery({route: 'test'});
    }));

    describe('- (fluent Rest Query Filter builder)- ', function () {
        beforeEach(function () {
        });
        describe('when no additional filters are added', function () {
            it('should return only the controller route', function () {
                expect(fluentRestQuery.compiledUrl()).toEqual('test');
            });
        });
        describe('when adding filters', function () {
            describe('- limit', function () {
                describe('when items have not been ordered already', function () {
                    it('should throw an error that Items should be ordered First', function () {

                        expect(function () {
                            fluentRestQuery.limit(10);
                        }).toThrow(new Error('Query must first be ordered'));
                    });
                });
                it('Should compile the search url to contain the limit', function () {
                    var returned = fluentRestQuery.order(0, 'field1').ascending();
                    returned.limit(10);
                    expect(fluentRestQuery.compiledUrl()).toEqual('test?filter={"order":{"0":"field1 asc"},"limit":10}');
                });
            });
            describe('- skip', function () {
                describe('when items have not been ordered already', function () {
                    it('should throw an error that Items should be ordered First', function () {

                        expect(function () {
                            fluentRestQuery.skip(10);
                        }).toThrow(new Error('Query must first be ordered'));
                    });
                });
                it('Should compile the search url to contain the limit', function () {
                    fluentRestQuery.order(0, 'field1').ascending().skip(10);
                    expect(fluentRestQuery.compiledUrl()).toEqual('test?filter={"order":{"0":"field1 asc"},"skip":10}');
                });
            });
            describe('- order By', function () {
                describe('when ordering by a column', function () {
                    var ordering;
                    beforeEach(function () {
                        ordering = fluentRestQuery.order(0, 'field1');
                    });
                    describe('ascending', function () {
                        it('should return json for ascending order on field', function () {
                            var ordered = ordering.ascending();
                            expect(ordered.compiledUrl()).toEqual('test?filter={"order":{"0":"field1 asc"}}');
                        });
                    });
                    describe('descending', function () {
                        it('should return json for ascending order on field', function () {
                            var ordered = ordering.descending();
                            expect(ordered.compiledUrl()).toEqual('test?filter={"order":{"0":"field1 desc"}}');
                        });
                    });
                    describe('multiple Order By additions', function () {
                        var orderedMultiple;
                        beforeEach(function () {
                            orderedMultiple = ordering.descending().order(1, 'field2').ascending();
                        });
                        it('should create a filter with the additions ordering', function () {
                            expect(orderedMultiple.compiledUrl()).toEqual('test?filter={"order":{"0":"field1 desc","1":"field2 asc"}}');
                        });
                    });
                });
            });
            describe('- fields', function () {
                it('Should compile the search url to contain the wanted Fields', function () {
                    fluentRestQuery.fields('field1', 'field2');
                    expect(fluentRestQuery.compiledUrl()).toEqual('test?filter={"fields":{"field1":true,"field2":true}}');

                });
                describe('when adding fields over multiple calls', function () {
                    it('should append fields', function () {
                        fluentRestQuery.fields('field1').fields('field2');
                        expect(fluentRestQuery.compiledUrl()).toEqual('test?filter={"fields":{"field1":true,"field2":true}}');
                    });
                });
            });
            describe('- Where', function () {
                it('Where should call where builder and get back the where clause', function () {
                    fluentRestQuery.where('field1').endWhere();
                    expect(fluentRestQuery.compiledUrl()).toEqual('test?filter={"where":{"AND":{"eq":{"field1":12}}}}');
                });
            });
            describe('- include', function () {
                it('should compile the include url to contain wanted relationships', function () {
                    fluentRestQuery.include(['rel1', 'rel2', 'rel3', 'rel4']);
                    expect(fluentRestQuery.compiledUrl()).toBe('test?filter={"include":"rel1, rel2, rel3, rel4"}');
                });
                describe('when including over multiple calls', function () {
                    it('should append includes', function () {
                        fluentRestQuery.include(['rel1', 'rel2']).include(['rel3', 'rel4']);
                        expect(fluentRestQuery.compiledUrl()).toBe('test?filter={"include":"rel1, rel2, rel3, rel4"}');
                    });
                });
            });
        });

        describe('when adding paging', function () {
            describe('with current page 1 and pageSize 100', function () {
                beforeEach(function () {
                    fluentRestQuery.paging(1, 50);
                });
                it(' - should compile with paging queryString', function () {
                    expect(fluentRestQuery.compiledUrl()).toBe('test?paging={"Paging":{"currentPage":1,"pageSize":50}}');
                });
            });
        });

        describe('when both filter and paging is applied', function () {
            beforeEach(function () {
                fluentRestQuery.paging(1, 50).order(0, 'Id').descending();
            });
            it('should have both filter and paging in query string', function () {
                expect(fluentRestQuery.compiledUrl()).toBe('test?filter={"order":{"0":"Id desc"}}&paging={"Paging":{"currentPage":1,"pageSize":50}}');
            });
        });
    });
});