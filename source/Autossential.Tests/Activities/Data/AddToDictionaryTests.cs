using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Autossential.Tests
{
    [TestClass]
    public class AddToDictionaryTests
    {
        private Dictionary<string, int> _dict;

        [TestInitialize]
        public void TestInitialize()
        {
            _dict = new Dictionary<string, int>();
        }

        private void AddItem(string key, int value, bool updateIfExists)
        {
            WorkflowTester.Invoke(new AddToDictionary<string, int>()
            {
                ReferenceDictionary = new System.Activities.InOutArgument<Dictionary<string, int>>(_ => _dict)
            }, GetArgs(key, value, updateIfExists));
        }

        [TestMethod]
        [DataRow("A", 1)]
        public void AddKeyValuePairs(string key, int value)
        {
            AddItem(key, value, false);
            Assert.AreEqual(1, _dict.Count);
            Assert.AreEqual(_dict[key], value);
        }

        [TestMethod]
        public void AddDuplicateKeyValueError()
        {
            _dict.Add("A", 1);
            Assert.ThrowsException<ArgumentException>(() => AddItem("A", 2, false));
        }


        [TestMethod]
        public void AddDuplicatedKeyValueAllowed()
        {
            _dict.Add("A", 1);
            AddItem("A", 2, true);
            Assert.AreEqual(2, _dict["A"]);
        }

        [TestMethod]
        public void InitializeDictionary()
        {
            _dict = null;
            AddItem("A", 1, false);
            Assert.IsNotNull(_dict);
            Assert.AreEqual(1, _dict.Count);
        }

        private static Dictionary<string, object> GetArgs<TKey, TValue>(TKey key, TValue value, bool updateIfExists)
        {
            return new Dictionary<string, object>
            {
                { nameof(AddToDictionary<TKey,TValue>.Key), key },
                { nameof(AddToDictionary<TKey,TValue>.Value), value },
                { nameof(AddToDictionary<TKey,TValue>.UpdateIfExists), updateIfExists }
            };
        }
    }
}
