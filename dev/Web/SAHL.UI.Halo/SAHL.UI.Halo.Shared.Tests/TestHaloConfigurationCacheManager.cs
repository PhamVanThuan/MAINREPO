using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using NUnit.Framework;

using SAHL.UI.Halo.Shared.Configuration.Caching;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloConfigurationCacheManager
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Test Result -----------------------
            Assert.IsNotNull(cacheManager);
        }

        [Test]
        public void Add_GivenNullCacheName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.Add(null, "cacheKey", "testItem"));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheName", exception.ParamName);
        }

        [Test]
        public void Add_GivenNullCacheKey_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.Add("cacheName", null, "testItem"));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheKey", exception.ParamName);
        }

        [Test]
        public void Add_GivenNullCacheItem_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.Add("cacheName", "cacheKey", null));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheItem", exception.ParamName);
        }

        [Test]
        public void Add_GivenValidItem_ShouldAddToCache()
        {
            //---------------Set up test pack-------------------
            const string cacheName = "TestCache";
            var testCacheObject    = new TestCacheObject("test1", 1234);
            var cacheManager       = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            cacheManager.Add(cacheName, testCacheObject.Name, testCacheObject);
            //---------------Test Result -----------------------
            dynamic foundObject = cacheManager.Find(cacheName, testCacheObject.Name);
            Assert.IsNotNull(foundObject);
            Assert.AreSame(testCacheObject, foundObject);
            Assert.AreEqual(testCacheObject.Name, foundObject.Name);
            Assert.AreEqual(testCacheObject.SomeValue, foundObject.SomeValue);
        }

        [Test]
        public void AddRange_GivenNullCacheName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheObjects = this.CreateRangeOfTestCacheObjects();
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.AddRange(null, cacheObjects));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheName", exception.ParamName);
        }

        [Test]
        public void AddRange_GivenNullCacheItem_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.AddRange("cacheName", null));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheItems", exception.ParamName);
        }

        [Test]
        public void AddRange_GivenValidItems_ShouldAddToCache()
        {
            //---------------Set up test pack-------------------
            const string cacheName = "TestCache";
            var cacheItems         = this.CreateRangeOfTestCacheObjects();

            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            cacheManager.AddRange(cacheName, cacheItems);
            //---------------Test Result -----------------------
            foreach (var cacheItem in cacheItems)
            {
                var foundObject = cacheManager.Find(cacheName, cacheItem.Key);

                Assert.IsNotNull(foundObject, 
                                 string.Format("Failed to find cache object [{0}]", cacheItem.Key));
                Assert.AreSame(cacheItem.Value, foundObject, 
                               string.Format("Cache Items do not match [{0} and {1}]", cacheItem.Key, foundObject.Name));
            }
        }

        [Test]
        public void Find_GivenNullCacheName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.Find(null, "cacheKey"));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheName", exception.ParamName);
        }

        [Test]
        public void Find_GivenNullCacheKey_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.Find("cacheName", null));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheKey", exception.ParamName);
        }

        [Test]
        public void Find_GivenItemDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var foundItem = cacheManager.Find("cacheName", "cacheKey");
            //---------------Test Result -----------------------
            Assert.IsNull(foundItem);
        }

        [Test]
        public void Find_GivenItemDoesExist_ShouldReturnFoundItem()
        {
            //---------------Set up test pack-------------------
            const string cacheName = "cacheName";

            var cacheObject  = new TestCacheObject("key1", 1234);
            var cacheManager = new HaloConfigurationCacheManager();
            cacheManager.Add(cacheName, cacheObject.Name, cacheObject);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var foundItem = cacheManager.Find(cacheName, cacheObject.Name);
            //---------------Test Result -----------------------
            Assert.IsNotNull(foundItem);
            Assert.AreEqual(cacheObject, foundItem);
        }

        [Test]
        public void FindAll_GivenNullCacheName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => cacheManager.FindAll(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("cacheName", exception.ParamName);
        }

        [Test]
        public void FindAll_GivenNoCacheDataExists_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allCacheItems = cacheManager.FindAll("test");
            //---------------Test Result -----------------------
            Assert.IsNull(allCacheItems);
        }

        [Test]
        public void FindAll_GivenCacheItemsExistForCacheName_ShouldReturnCacheItems()
        {
            //---------------Set up test pack-------------------
            const string cacheName = "cacheItems";
            var testCacheObjects   = this.CreateRangeOfTestCacheObjects();
            var cacheManager       = new HaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            cacheManager.AddRange(cacheName, testCacheObjects);
            //---------------Execute Test ----------------------
            var allCacheItems = cacheManager.FindAll(cacheName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(allCacheItems);
            CollectionAssert.AreEqual(testCacheObjects.Select(cacheItem => cacheItem.Value), allCacheItems);
        }

        [Test]
        public void Clear_GivenEmptyCache_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var cacheManager = new FakeHaloConfigurationCacheManager();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cacheManager.AllCacheItems.Count);
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => cacheManager.Clear());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Clear_GivenItemsInCache_ShouldRemoveCacheItemsFromCache()
        {
            //---------------Set up test pack-------------------
            var firstCacheName    = "firstObjects";
            var testCacheObjects1 = this.CreateRangeOfTestCacheObjects("first");
            var secondCacheName   = "secondObjects";
            var testCacheObjects2 = this.CreateRangeOfTestCacheObjects("second");

            var cacheManager = new FakeHaloConfigurationCacheManager();
            cacheManager.AddRange(firstCacheName, testCacheObjects1);
            cacheManager.AddRange(secondCacheName, testCacheObjects2);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cacheManager.AllCacheItems.Count);
            //---------------Execute Test ----------------------
            cacheManager.Clear(secondCacheName);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cacheManager.AllCacheItems.Count);
            Assert.IsTrue(cacheManager.AllCacheItems.ContainsKey(firstCacheName));
            Assert.IsFalse(cacheManager.AllCacheItems.ContainsKey(secondCacheName));
        }

        [Test]
        public void Clear_GivenItemsInCacheAndNoCacheNameProvided_ShouldRemoveAllCacheItemsFromCache()
        {
            //---------------Set up test pack-------------------
            var firstCacheName    = "firstObjects";
            var testCacheObjects1 = this.CreateRangeOfTestCacheObjects("first");
            var secondCacheName   = "secondObjects";
            var testCacheObjects2 = this.CreateRangeOfTestCacheObjects("second");

            var cacheManager = new FakeHaloConfigurationCacheManager();
            cacheManager.AddRange(firstCacheName, testCacheObjects1);
            cacheManager.AddRange(secondCacheName, testCacheObjects2);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cacheManager.AllCacheItems.Count);
            //---------------Execute Test ----------------------
            cacheManager.Clear();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cacheManager.AllCacheItems.Count);
            Assert.IsFalse(cacheManager.AllCacheItems.ContainsKey(firstCacheName));
            Assert.IsFalse(cacheManager.AllCacheItems.ContainsKey(secondCacheName));
        }

        private IDictionary<string, dynamic> CreateRangeOfTestCacheObjects(string itemKeyPrefix = "")
        {
            var testCacheObject1 = new TestCacheObject(string.Join("", itemKeyPrefix, "1"), 1234);
            var testCacheObject2 = new TestCacheObject(string.Join("", itemKeyPrefix, "2"), 2345);

            var cacheItems = new Dictionary<string, dynamic>
                {
                    { testCacheObject1.Name, testCacheObject1 },
                    { testCacheObject2.Name, testCacheObject2 }
                };
            return cacheItems;
        }

        private class TestCacheObject
        {
            public TestCacheObject(string name, int someValue)
            {
                this.Name      = name;
                this.SomeValue = someValue;
            }

            public string Name { get; private set; }
            public int SomeValue { get; private set; }
        }

        private class FakeHaloConfigurationCacheManager : HaloConfigurationCacheManager
        {
            public ConcurrentDictionary<string, IList<CacheItem>> AllCacheItems
            {
                get { return this.CacheItems; }
            }
        }
    }
}
