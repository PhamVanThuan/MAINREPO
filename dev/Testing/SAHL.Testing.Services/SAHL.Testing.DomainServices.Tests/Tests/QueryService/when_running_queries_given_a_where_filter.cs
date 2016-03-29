using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fluent;
using SAHL.Services.Query.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Core.Data.Models.FETest;

namespace SAHL.Testing.Services.Tests.QueryService
{
    [TestFixture]
    public class when_running_queries_given_a_where_filter : FluentTest
    {
        [Test]
        public void equal_to_string_should_return_specified_data()
        {
            test.Setup<Query>(y =>
                {
                    y.Set("url", "/api/attorneys/");
                })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Equals()
                            .Field("Name")
                            .Value("Moodie And Robertson")
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                 {
                     Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                     var _results2 = (IDictionary<string, object>)results["_embedded"];
                     var attorneys = (IList<dynamic>)_results2["attorney"];
                     Assert.AreEqual(1, attorneys.Count());
                     Assert.AreEqual("Moodie And Robertson", attorneys.FirstOrDefault().name);

                 });
        }

        [Test]
        public void equal_to_integer_should_return_specified_data()
        {
            string eqValue = "1";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Equals()
                            .Field("AttorneyKey")
                            .Value(eqValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var attorneys = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(attorneys.Count, attorneys.Where(x => x.attorneyKey == Convert.ToInt32(eqValue)).Count(),
                        string.Format(@"Count of total results did not match count of results equal to value"));
                });
        }

        [Test]
        public void equal_to_date_should_return_specified_data()
        {
            AccountDataModel account = null;
            DateTime eqValue = DateTime.MinValue;
            test.Setup<Query>(y =>
            {
                account = TestApiClient.GetAny<AccountDataModel>(new { AccountStatusKey = (int)GeneralStatus.Active }, 1000);
                eqValue = account.OpenDate.GetValueOrDefault();
                y.Set("url", "/api/accounts/");
            })
            .Execute<Query, IDictionary<string, object>>(y =>
            {
                //var openDate = eqValue.ToString("yyyy-MM-dd HH:mm:ss");
                return y.Where()
                        .And()
                        .Equals()
                        .Field("OpenDate")
                        .Value(eqValue.ToString())
                        .Execute();
            })
            .Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                var _results2 = (IDictionary<string, object>)results["_embedded"];
                var accounts = (IList<dynamic>)_results2["account"];
                Assert.AreEqual(1, accounts.Where(x => x.id == account.AccountKey).Count(),
                    string.Format(@"Results did not contain account {0}", account.AccountKey));
                Assert.AreEqual(accounts.Count, accounts.Where(x => Convert.ToDateTime(x.openDate) == eqValue).Count(),
                    string.Format(@"Count of total results did not match count of results equal to value"));
            });
        }

        [Test]
        public void greater_than_integer_should_return_specified_data()
        {
            string gtValue = "5";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .IsGreaterThan()
                            .Field("deedsofficekey")
                            .Value(gtValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var attorneys = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(attorneys.Count, attorneys.Where(x => x.deedsOfficeKey > Convert.ToInt32(gtValue)).Count(),
                        string.Format(@"Count of total results did not match count of results greater than value"));
                });
        }

        [Test]
        public void greater_than_date_should_return_specified_data()
        {
            AccountDataModel account = null;
            DateTime gtValue = DateTime.MinValue;
            test.Setup<Query>(y =>
            {
                account = TestApiClient.GetAny<AccountDataModel>(new { AccountStatusKey = (int)GeneralStatus.Active }, 1000);
                gtValue = account.OpenDate.GetValueOrDefault();
                y.Set("url", "/api/accounts/");
            })
            .Execute<Query, IDictionary<string, object>>(y =>
            {
                //var openDate = eqValue.ToString("yyyy-MM-dd HH:mm:ss");
                return y.Where()
                        .And()
                        .IsGreaterThan()
                        .Field("OpenDate")
                        .Value(gtValue.ToString())
                        .Execute();
            })
            .Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                var _results2 = (IDictionary<string, object>)results["_embedded"];
                var accounts = (IList<dynamic>)_results2["account"];
                Assert.AreEqual(0, accounts.Where(x => x.id == account.AccountKey).Count(),
                    string.Format(@"Results should not contain account {0}", account.AccountKey));
                Assert.AreEqual(accounts.Count, accounts.Where(x => Convert.ToDateTime(x.openDate) > gtValue).Count(),
                    string.Format(@"Count of total results did not match count of results greater than value"));
            });
        }

        [Test]
        public void less_than_integer_should_return_specified_data()
        {
            string ltValue = "5";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .IsLessThan()
                            .Field("deedsofficekey")
                            .Value(ltValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var attorneys = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(attorneys.Count, attorneys.Where(x => x.deedsOfficeKey < Convert.ToInt32(ltValue)).Count(),
                        string.Format(@"Count of total results did not match count of results less than value"));
                });
        }

        [Test]
        public void less_than_date_should_return_specified_data()
        {
            AccountDataModel account = null;
            DateTime ltValue = DateTime.MinValue;
            test.Setup<Query>(y =>
            {
                account = TestApiClient.GetAny<AccountDataModel>(new { AccountStatusKey = (int)GeneralStatus.Active }, 1000);
                ltValue = account.OpenDate.GetValueOrDefault();
                y.Set("url", "/api/accounts/");
            })
            .Execute<Query, IDictionary<string, object>>(y =>
            {
                //var openDate = eqValue.ToString("yyyy-MM-dd HH:mm:ss");
                return y.Where()
                        .And()
                        .IsLessThan()
                        .Field("OpenDate")
                        .Value(ltValue.ToString())
                        .Execute();
            })
            .Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                var _results2 = (IDictionary<string, object>)results["_embedded"];
                var accounts = (IList<dynamic>)_results2["account"];
                Assert.AreEqual(0, accounts.Where(x => x.id == account.AccountKey).Count(),
                    string.Format(@"Results should not contain account {0}", account.AccountKey));
                Assert.AreEqual(accounts.Count, accounts.Where(x => Convert.ToDateTime(x.openDate) < ltValue).Count(),
                    string.Format(@"Count of total results did not match count of results less than value"));
            });
        }

        [Test]
        public void greater_than_equal_to_integer_should_return_specified_data()
        {
            string gteValue = "5";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .IsGreaterThanEqualTo()
                            .Field("deedsofficekey")
                            .Value(gteValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var attorneys = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(attorneys.Count, attorneys.Where(x => x.deedsOfficeKey >= Convert.ToInt32(gteValue)).Count(),
                        string.Format(@"Count of total results did not match count of results greater then or equal to value"));
                });
        }

        [Test]
        public void greater_than_equal_to_date_should_return_specified_data()
        {
            AccountDataModel account = null;
            DateTime gteValue = DateTime.MinValue;
            test.Setup<Query>(y =>
            {
                account = TestApiClient.GetAny<AccountDataModel>(new { AccountStatusKey = (int)GeneralStatus.Active }, 50);
                gteValue = account.OpenDate.GetValueOrDefault();
                y.Set("url", "/api/accounts/");
            })
            .Execute<Query, IDictionary<string, object>>(y =>
            {
                //var openDate = eqValue.ToString("yyyy-MM-dd HH:mm:ss");
                return y.Where()
                        .And()
                        .IsGreaterThanEqualTo()
                        .Field("OpenDate")
                        .Value(gteValue.ToString())
                        .Execute();
            })
            .Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                var _results2 = (IDictionary<string, object>)results["_embedded"];
                var accounts = (IList<dynamic>)_results2["account"];
                Assert.AreEqual(accounts.Count, accounts.Where(x => Convert.ToDateTime(x.openDate) >= gteValue).Count(),
                    string.Format(@"Count of total results did not match count of results greater than equal to value"));
            });
        }

        [Test]
        public void less_than_equal_to_integer_should_return_specified_data()
        {
            string lteValue = "5";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .IsLessThanEqualTo()
                            .Field("deedsofficekey")
                            .Value(lteValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var attorneys = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(attorneys.Count, attorneys.Where(x => x.deedsOfficeKey <= Convert.ToInt32(lteValue)).Count(),
                        string.Format(@"Count of total results did not match count of results less than or equal to value"));
                });
        }

        [Test]
        public void less_than_equal_to_date_should_return_specified_data()
        {
            AccountDataModel account = null;
            DateTime lteValue = DateTime.MinValue;
            test.Setup<Query>(y =>
            {
                account = TestApiClient.GetAny<AccountDataModel>(new { AccountStatusKey = (int)GeneralStatus.Active }, 1000);
                lteValue = account.OpenDate.GetValueOrDefault();
                y.Set("url", "/api/accounts/");
            })
            .Execute<Query, IDictionary<string, object>>(y =>
            {
                //var openDate = eqValue.ToString("yyyy-MM-dd HH:mm:ss");
                return y.Where()
                        .And()
                        .IsLessThanEqualTo()
                        .Field("OpenDate")
                        .Value(lteValue.ToString())
                        .Execute();
            })
            .Assert<Query, IDictionary<string, object>>((query, results) =>
            {
                Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                var _results2 = (IDictionary<string, object>)results["_embedded"];
                var accounts = (IList<dynamic>)_results2["account"];
                Assert.AreEqual(accounts.Count, accounts.Where(x => Convert.ToDateTime(x.openDate) <= lteValue).Count(),
                    string.Format(@"Count of total results did not match count of results less than equal to value"));
            });
        }

        [Test]
        public void like_operator_with_start_and_end_string_should_return_specified_data()
        {
            string attorneyRegisteredName = string.Empty;
            string likeStartValue = string.Empty;
            string likeEndValue = string.Empty;
            test.Setup<Query>(y =>
            {
                var attorney = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
                var attorneyLegalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = attorney.LegalEntityKey }).FirstOrDefault();
                attorneyRegisteredName = attorneyLegalEntity.RegisteredName;
                likeStartValue = attorneyRegisteredName.Substring(0, 2);
                likeEndValue = attorneyRegisteredName.Substring(attorneyRegisteredName.Length - 2, 2);
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Like()
                            .Field("name")
                            .Value(string.Concat(likeStartValue, "*", likeEndValue))
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.name.StartsWith(likeStartValue) && x.name.EndsWith(likeEndValue)).Count(),
                        string.Format(@"Count of total results did not match count of results like value"));
                });
        }

        [Test]
        public void like_operator_with_start_and_end_integer_should_return_specified_data()
        {
            string likeStartValue = "40";
            string likeEndValue = "07";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/accounts/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Like()
                            .Field("id")
                            .Value(string.Concat(likeStartValue, "*", likeEndValue))
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["account"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.id.ToString().StartsWith(likeStartValue) && x.id.ToString().EndsWith(likeEndValue)).Count(),
                        string.Format(@"Count of total results did not match count of results like value"));
                });
        }

        [Test]
        public void like_operator_with_start_string_should_return_specified_data()
        {
            string attorneyRegisteredName = string.Empty;
            string likeStartValue = string.Empty;
            test.Setup<Query>(y =>
            {
                var attorney = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
                var attorneyLegalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = attorney.LegalEntityKey }).FirstOrDefault();
                attorneyRegisteredName = attorneyLegalEntity.RegisteredName;
                likeStartValue = attorneyRegisteredName.Substring(0, 2);
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Like()
                            .Field("name")
                            .Value(string.Concat(likeStartValue, "*"))
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.name.StartsWith(likeStartValue)).Count(),
                        string.Format(@"Count of total results did not match count of results like value"));
                });
        }

        [Test]
        public void like_operator_with_start_integer_should_return_specified_data()
        {
            string likeStartValue = "40";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/accounts/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Like()
                            .Field("id")
                            .Value(string.Concat(likeStartValue, "%"))
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["account"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.id.ToString().StartsWith(likeStartValue)).Count(),
                        string.Format(@"Count of total results did not match count of results like value"));
                });
        }

        [Test]
        public void like_operator_with_end_string_should_return_specified_data()
        {
            string attorneyRegisteredName = string.Empty;
            string likeEndValue = string.Empty;
            test.Setup<Query>(y =>
            {
                var attorney = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
                var attorneyLegalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = attorney.LegalEntityKey }).FirstOrDefault();
                attorneyRegisteredName = attorneyLegalEntity.RegisteredName;
                likeEndValue = attorneyRegisteredName.Substring(attorneyRegisteredName.Length - 2, 2);
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Like()
                            .Field("name")
                            .Value(string.Concat("*", likeEndValue))
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.name.EndsWith(likeEndValue)).Count(),
                        string.Format(@"Count of total results did not match count of results like value"));
                });
        }

        [Test]
        public void like_operator_with_end_intiger_should_return_specified_data()
        {
            string likeEndValue = "07";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/accounts/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Like()
                            .Field("id")
                            .Value(string.Concat("*", likeEndValue))
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["account"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.id.ToString().EndsWith(likeEndValue)).Count(),
                        string.Format(@"Count of total results did not match count of results like value"));
                });
        }

        [Test]
        public void starts_with_string_should_return_specified_data() //bad gateway
        {
            string attorneyRegisteredName = string.Empty;
            string startsWithValue = string.Empty;
            test.Setup<Query>(y =>
            {
                var attorney = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
                var attorneyLegalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = attorney.LegalEntityKey }).FirstOrDefault();
                attorneyRegisteredName = attorneyLegalEntity.RegisteredName;
                startsWithValue = attorneyRegisteredName.Substring(0, 2);
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .StartsWith()
                            .Field("name")
                            .Value(startsWithValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.name.StartsWith(startsWithValue)).Count(),
                        string.Format(@"Count of total results did not match count of results starting with value"));
                });
        }

        [Test]
        public void starts_with_integer_should_return_specified_data()
        {
            string startsWithValue = "408030";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/accounts/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .StartsWith()
                            .Field("id")
                            .Value(startsWithValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["account"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.id.ToString().StartsWith(startsWithValue)).Count(),
                        string.Format(@"Count of total results did not match count of results starting with value"));
                });
        }

        [Test]
        public void ends_with_string_should_return_specified_data() //bad gateway
        {
            string attorneyRegisteredName = string.Empty;
            string endsWithValue = string.Empty;
            test.Setup<Query>(y =>
            {
                var attorney = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney });
                var attorneyLegalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = attorney.LegalEntityKey }).FirstOrDefault();
                attorneyRegisteredName = attorneyLegalEntity.RegisteredName;
                endsWithValue = attorneyRegisteredName.Substring(attorneyRegisteredName.Length - 2, 2);
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .EndsWith()
                            .Field("name")
                            .Value(endsWithValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.name.EndsWith(endsWithValue)).Count(),
                        string.Format(@"Count of total results did not match count of results ending with value"));
                });
        }

        [Test]
        public void ends_with_integer_should_return_specified_data()
        {
            string endsWithValue = "307";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/accounts/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .EndsWith()
                            .Field("id")
                            .Value(endsWithValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["account"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.id.ToString().EndsWith(endsWithValue)).Count(),
                        string.Format(@"Count of total results did not match count of results ending with value"));
                });
        }

        [Test, Repeat(20)]
        public void contains_string_should_return_specified_data() //failing on a bad gateway (502)
        {
            string containsValue = string.Empty;
            test.Setup<Query>(y =>
            {
                containsValue = "Strauss";
                var attorney = TestApiClient.Get<ThirdPartiesDataModel>( new { generickeytypekey = 35 });
                var count = attorney.Where(x => x.TradingName.Contains(containsValue));
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Contains()
                            .Field("name")
                            .Value(containsValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.That(results != null && Convert.ToInt32(results["totalCount"]) > 0, string.Format(@"Count of total results is not greater than 0. ContainsValue : {0}", containsValue));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.name.Contains(containsValue)).Count(),
                        string.Format(@"Count of total results did not match count of results containing value. Querying Attorney endpoint with contains : {0}", containsValue));
                });
        }

        [Test]
        public void contains_integer_should_return_specified_data()
        {
            string containsValue = "408030";
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/accounts/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .Contains()
                            .Field("id")
                            .Value(containsValue)
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var accounts = (IList<dynamic>)_results2["account"];
                    Assert.AreEqual(accounts.Count, accounts.Where(x => x.id.ToString().Contains(containsValue)).Count(),
                        string.Format(@"Count of total results did not match count of results containing value"));
                });
        }

        [Test]
        public void less_than_or_greater_than_operators_should_return_specified_data()
        {
            int ltValue = 4;
            int gtValue = 6;
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .Or()
                            .IsLessThan()
                            .Field("deedsOfficeKey")
                            .Value(ltValue.ToString())
                            .Where()
                            .Or()
                            .IsGreaterThan()
                            .Field("deedsOfficeKey")
                            .Value(gtValue.ToString())
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var attorneys = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(attorneys.Count, attorneys.Where(x => x.deedsOfficeKey < ltValue
                        || x.deedsOfficeKey > gtValue).Count(),
                        string.Format(@"Count of total results did not match count of results less than or greater than value"));
                });
        }

        [Test]
        public void less_than_and_greater_than_operators_should_return_specified_data()
        {
            int ltValue = 6;
            int gtValue = 4;
            test.Setup<Query>(y =>
            {
                y.Set("url", "/api/attorneys/");
            })
                .Execute<Query, IDictionary<string, object>>(y =>
                {
                    return y.Where()
                            .And()
                            .IsLessThan()
                            .Field("deedsOfficeKey")
                            .Value(ltValue.ToString())
                            .Where()
                            .And()
                            .IsGreaterThan()
                            .Field("deedsOfficeKey")
                            .Value(gtValue.ToString())
                            .Execute();
                })
                .Assert<Query, IDictionary<string, object>>((query, results) =>
                {
                    Assert.Greater(Convert.ToInt32(results["totalCount"]), 0, string.Format(@"Count of total results is not greater than 0"));
                    var _results2 = (IDictionary<string, object>)results["_embedded"];
                    var attorneys = (IList<dynamic>)_results2["attorney"];
                    Assert.AreEqual(attorneys.Count, attorneys.Where(x => x.deedsOfficeKey < ltValue
                        && x.deedsOfficeKey > gtValue).Count(),
                        string.Format(@"Count of total results did not match count of results less than and greater than value"));
                });
        }
    }
}