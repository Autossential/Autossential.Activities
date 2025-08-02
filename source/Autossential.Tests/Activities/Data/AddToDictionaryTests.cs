using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Windows.Input;
using UiPath.Workflow.Debugger;

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
            WorkflowTester.Invoke(new AddToDictionary()
            {
                Dictionary = new System.Activities.InOutArgument<Dictionary<string, int>>(_ => _dict),
                Key = new System.Activities.InArgument<string>(key),
                Value = new System.Activities.InArgument<int>(value)
            });
        }

        [TestMethod]
        public void TestCacheMetadata()
        {
            var dict = new Dictionary<string, object>();
            WorkflowTester.Invoke(new AddToDictionary()
            {
                Dictionary = new System.Activities.InOutArgument<Dictionary<string, object>>(_ => dict),
                Key = new System.Activities.InArgument<string>("A"),
                Value = new System.Activities.InArgument<int>(10)
            });
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
    }
}
