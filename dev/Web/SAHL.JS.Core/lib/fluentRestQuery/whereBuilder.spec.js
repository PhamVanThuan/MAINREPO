'use strict';
describe('[sahl.js.core.fluentRestQuery]', function () {
    beforeEach(module('sahl.js.core.fluentRestQuery'));
    var fluentWhereBuilderTemplate, firstField = 'field1';

    beforeEach(inject(function ($fluentWhereBuilder) {
        fluentWhereBuilderTemplate = $fluentWhereBuilder;
    }));

    describe('- (helper fluent where builder)- ', function () {
        var fluentWhereBuilder, called = false;

        describe('when ending where, should call callback', function () {
            beforeEach(function () {
                fluentWhereBuilder = new fluentWhereBuilderTemplate(firstField, function () {
                    called = true;
                    return true;
                });
            });
            it('it should return just the base for the query', function () {
                fluentWhereBuilder.endWhere();
                expect(called).toEqual(true);
            });
        });

        describe('when', function () {


            var returnedJson;
            beforeEach(function () {
                fluentWhereBuilder = new fluentWhereBuilderTemplate(firstField, function (json) {
                    returnedJson = json;
                    return {};
                });
            });

            describe('adding additional filters onto the where -', function () {
                describe('checking greater than', function () {
                    it('should add a greater than key to the json', function () {
                        fluentWhereBuilder.greaterThan(12);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({gt: {field1: 12}});
                    });
                });

                describe('checking less than', function () {
                    it('should add a less than key to the json', function () {
                        fluentWhereBuilder.lessThan(12);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({lt: {field1: 12}});
                    });
                });

                describe('checking less than equal to', function () {
                    it('should add a less than equal to to the json', function () {
                        fluentWhereBuilder.lessThanOrEquals(12);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({lte: {field1: 12}});
                    });
                });

                describe('checking greater than or equal to', function () {
                    it('should add a greater than or equal to to the json', function () {
                        fluentWhereBuilder.greaterThanOrEquals(12);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({gte: {field1: 12}});
                    });
                });

                describe('checking like', function () {
                    it('should add a like to the json', function () {
                        var comparedValue = '%testing%';
                        fluentWhereBuilder.isLike(comparedValue);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({like: {field1: comparedValue}});
                    });
                });

                describe('checking endsWith', function () {
                    it('should add a endsWith to the json', function () {
                        var comparedValue = 'testing';
                        fluentWhereBuilder.endsWith(comparedValue);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({endswith: {field1: comparedValue}});
                    });
                });

                describe('checking startsWith', function () {
                    it('should add a startsWith to the json', function () {
                        var comparedValue = 'testing';
                        fluentWhereBuilder.startsWith(comparedValue);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({startswith: {field1: comparedValue}});
                    });
                });

                describe('checking contains', function () {
                    it('should add a contains to the json', function () {
                        var comparedValue = 'testing';
                        fluentWhereBuilder.contains(comparedValue);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({contains: {field1: comparedValue}});
                    });
                });

                describe('checking isEqual', function () {
                    it('should add a like to the json', function () {
                        var comparedValue = 'testing';
                        fluentWhereBuilder.isEqual(comparedValue);
                        fluentWhereBuilder.endWhere();
                        expect(returnedJson).toEqual({eq: {field1: comparedValue}});
                    });
                });
            });

            describe('using logical operators', function () {
                var sencondField = 'field2', thirdField = 'field3';
                describe('when using the And Operator', function () {
                    beforeEach(function () {
                        fluentWhereBuilder.isEqual(12).andWhere(sencondField).greaterThan(15);
                    });

                    describe('when no other operator has been chosen', function () {
                        it('should add the AND operator to the json', function () {
                            fluentWhereBuilder.endWhere();
                            expect(returnedJson).toEqual({AND: {eq: {field1: 12}, gt: {field2: 15}}});
                        });

                        describe('when and is already been used and', function () {
                            describe('using another And', function () {
                                beforeEach(function () {
                                    fluentWhereBuilder.andWhere(thirdField).lessThan(15);
                                });

                                it('should just add to the and property', function () {
                                    fluentWhereBuilder.endWhere();
                                    expect(returnedJson).toEqual({
                                        AND: {
                                            eq: {field1: 12},
                                            gt: {field2: 15},
                                            lt: {field3: 15}
                                        }
                                    });
                                });
                            });

                            describe('attempting to use an OR', function () {
                                it('it should fail and throw errors', function () {
                                    expect(function () {
                                        fluentWhereBuilder.orWhere(thirdField);
                                    }).toThrow(new Error("can currently only build with and OR or operations exclusively"));
                                });
                            });
                        });
                    });
                });

                describe('when using the Or Operator', function () {
                    var sencondField = 'field2', thirdField = 'field3';
                    beforeEach(function () {
                        fluentWhereBuilder.isEqual(12).orWhere(sencondField).greaterThan(15);
                    });

                    describe('when no other operator has been chosen', function () {
                        it('should add the OR operator to the json', function () {
                            fluentWhereBuilder.endWhere();
                            expect(returnedJson).toEqual({OR: {eq: {field1: 12}, gt: {field2: 15}}});
                        });

                        describe('when or is already been used and', function () {
                            describe('using another or', function () {
                                beforeEach(function () {
                                    fluentWhereBuilder.orWhere(thirdField).lessThan(15);
                                });

                                it('should just add to the and property', function () {
                                    fluentWhereBuilder.endWhere();
                                    expect(returnedJson).toEqual({
                                        OR: {
                                            eq: {field1: 12},
                                            gt: {field2: 15},
                                            lt: {field3: 15}
                                        }
                                    });
                                });
                            });

                            describe('attempting to use an AND', function () {
                                it('it should fail and throw errors', function () {
                                    expect(function () {
                                        fluentWhereBuilder.andWhere(thirdField);
                                    }).toThrow(new Error("can currently only build with and OR or operations exclusively"));
                                });
                            });
                        });
                    });
                });
            });
        });
    });
});
