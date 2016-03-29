using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace DomainService2.Tests
{
    public class TypeValueProvider
    {
        public bool boolArg = true;
        public int intArg = 777;
        public string stringArg = "777";
        public long longArg = 8888L;
        public double doubleArg = 999.999;
        public decimal decimalArg = 5555.5M;
        public Single floatArg = 444.44F;
        public int enumArg = 1;
        public DateTime dateTimeArg = new DateTime(2008, 05, 05);
        public DateTime? dateTimeNullableArg = new DateTime(2008, 05, 05);

        public DataTable dataTableArg = new DataTable("TestTable");

        public List<string> strArgList = new List<string>();

        public List<int> intArgList = new List<int>();

        public Dictionary<string, object> objArgDict = new Dictionary<string, object>();

        public object objectArg = new object();

        public string[] stringArrayArgs = new string[] { "A", "B" };

        public object[] objectArrayArg = new object[] { "C", "D" };

        public TypeValueProvider()
        {
            strArgList.Add("A");
            strArgList.Add("B");

            intArgList.Add(1);
            intArgList.Add(2);

            objArgDict.Add("A", "C");
            objArgDict.Add("B", "D");
        }

        public object GetValueForType(Type type)
        {
            if (type.BaseType == typeof(Enum))
            {
                object enumValue = Enum.ToObject(type, this.enumArg);
                return enumValue;
            }
            else
                if (type == typeof(Boolean))
                {
                    return this.boolArg;
                }
                else
                    if (type == typeof(string))
                    {
                        return this.stringArg;
                    }
                    else
                        if (type == typeof(int))
                        {
                            return this.intArg;
                        }
                        else
                            if (type == typeof(long))
                            {
                                return this.longArg;
                            }
                            else
                                if (type == typeof(double))
                                {
                                    return this.doubleArg;
                                }
                                else
                                    if (type == typeof(decimal))
                                    {
                                        return this.decimalArg;
                                    }
                                    else
                                        if (type == typeof(float))
                                        {
                                            return this.floatArg;
                                        }
                                        else
                                            if (type == typeof(object))
                                            {
                                                return this.objectArg;
                                            }
                                            else
                                                if (type.FullName == "System.String&")
                                                {
                                                    return "";
                                                }
                                                else
                                                    if (type == typeof(DateTime))
                                                    {
                                                        return this.dateTimeArg;
                                                    }
                                                    else
                                                        if (type == typeof(DateTime?))
                                                        {
                                                            return this.dateTimeNullableArg;
                                                        }
                                                        else
                                                            if (type == typeof(List<string>))
                                                            {
                                                                return this.strArgList;
                                                            }
                                                            else
                                                                if (type == typeof(List<int>))
                                                                {
                                                                    return this.intArgList;
                                                                }
                                                                else
                                                                    if (type == typeof(string[]))
                                                                    {
                                                                        return this.stringArrayArgs;
                                                                    }
                                                                    else
                                                                        if (type == typeof(object[]))
                                                                        {
                                                                            return this.objectArrayArg;
                                                                        }
                                                                        else
                                                                            if (type == typeof(Dictionary<string, object>))
                                                                            {
                                                                                return this.objArgDict;
                                                                            }
                                                                            else
                                                                                if (type == typeof(DataTable))
                                                                                {
                                                                                    return this.dataTableArg;
                                                                                }
                                                                                else
                                                                                {
                                                                                    throw new Exception("type not yet provided for");
                                                                                }
        }

        public void AssertTypeValueIsCorrect(object result)
        {
            Type type = result.GetType();

            if (type.BaseType == typeof(Enum))
            {
                object enumValue = Enum.ToObject(type, this.enumArg);
                object realValue = Enum.ToObject(type, result);
                Assert.That((int)enumValue == (int)realValue);
            }
            else
                if (type == typeof(Boolean))
                {
                    Assert.That((bool)result == this.boolArg);
                }
                else
                    if (type == typeof(string))
                    {
                        Assert.That((string)result == this.stringArg);
                    }
                    else
                        if (type.FullName == "System.String&")
                        {
                            Assert.That((string)result == this.stringArg);
                        }
                        else
                            if (type == typeof(int))
                            {
                                Assert.That((int)result == this.intArg);
                            }
                            else
                                if (type == typeof(long))
                                {
                                    Assert.That((long)result == this.longArg);
                                }
                                else
                                    if (type == typeof(double))
                                    {
                                        Assert.That((double)result == this.doubleArg);
                                    }
                                    else
                                        if (type == typeof(decimal))
                                        {
                                            Assert.That((decimal)result == this.decimalArg);
                                        }
                                        else
                                            if (type == typeof(float))
                                            {
                                                Assert.That((float)result == this.floatArg);
                                            }
                                            else
                                                if (type == typeof(DateTime))
                                                {
                                                    Assert.That((DateTime)result == this.dateTimeArg);
                                                }
                                                else
                                                    if (type == typeof(DateTime?))
                                                    {
                                                        Assert.That((DateTime?)result == this.dateTimeNullableArg);
                                                    }
                                                    else
                                                        if (type == typeof(object))
                                                        {
                                                            Assert.That(result == this.objectArg);
                                                        }
                                                        else
                                                            if (type == typeof(object[]))
                                                            {
                                                                Assert.That((object[])result == this.objectArrayArg);
                                                            }
                                                            else
                                                                if (type == typeof(List<string>))
                                                                {
                                                                    Assert.That(result == this.strArgList);
                                                                }
                                                                else
                                                                    if (type == typeof(List<int>))
                                                                    {
                                                                        Assert.That(result == this.intArgList);
                                                                    }
                                                                    else
                                                                        if (type == typeof(string[]))
                                                                        {
                                                                            Assert.That(result == this.stringArrayArgs);
                                                                        }
                                                                        else
                                                                            if (type == typeof(Dictionary<string, object>))
                                                                            {
                                                                                Assert.That(this.objArgDict.DictionaryEqual<string, object>((Dictionary<string, object>)result));
                                                                            }
                                                                            else
                                                                                if (type == typeof(DataTable))
                                                                                {
                                                                                    Assert.That(((DataTable)result).DefaultView.Table.TableName == this.dataTableArg.DefaultView.Table.TableName);
                                                                                }
                                                                                else
                                                                                {
                                                                                    throw new Exception("type not yet provided for");
                                                                                }
        }
    }

    public static class Helper
    {
        public static bool DictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            var comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!comparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }
    }
}