using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class DictionaryToDataTableTests
    {
        [TestMethod]
        public void Default()
        {
            var dic = new Dictionary<string, object>
            {
                { "Name", "System" },
                { "Year", 1983 },
                { "Active", true },
                { "Date", DateTime.Today },
                { "Remarks", null },
                { "Value", 90.7m }
            };

            var result = WorkflowTester.Invoke(new DictionaryToDataTable(), new Dictionary<string, object> {
                { nameof(DictionaryToDataTable.InputDictionary), dic }
            });

            Assert.AreEqual(1, result.Rows.Count);
            Assert.AreEqual(dic["Name"], result.Rows[0]["Name"]);
            Assert.AreEqual(dic["Year"], result.Rows[0]["Year"]);
            Assert.AreEqual(dic["Active"], result.Rows[0]["Active"]);
            Assert.AreEqual(dic["Date"], result.Rows[0]["Date"]);
            Assert.IsTrue(result.Rows[0].IsNull("Remarks"));
            Assert.AreEqual(dic["Value"], result.Rows[0]["Value"]);
        }
    }
}
