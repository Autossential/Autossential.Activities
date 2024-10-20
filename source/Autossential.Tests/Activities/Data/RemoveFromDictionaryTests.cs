using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Autossential.Tests
{
    [TestClass]
    public class RemoveFromDictionaryTests
    {
        private Dictionary<string, object> _dict;

        [TestInitialize]
        public void TestInitialize()
        {
            _dict = new Dictionary<string, object>()
            {
                {"A",1 },
                {"B",2 },
                {"C",3 }
            };
        }


        [TestMethod]
        public void RemoveFromDictionaryTest()
        {
            WorkflowTester.Invoke(new RemoveFromDictionary<string, object>(), GetArgs(_dict, "A"));
            Assert.AreEqual(2, _dict.Count);
            CollectionAssert.AreEqual(new[] { "B", "C" }, _dict.Keys.ToArray());
        }


        private static Dictionary<string, object> GetArgs(Dictionary<string, object> dict, string key)
        {
            return new Dictionary<string, object>
            {
                { nameof(RemoveFromDictionary<string, object>.InputDictionary), dict },
                { nameof(RemoveFromDictionary<string, object>.Key), key },
            };
        }
    }
}
