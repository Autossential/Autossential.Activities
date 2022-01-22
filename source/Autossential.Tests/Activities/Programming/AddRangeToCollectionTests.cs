using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class AddRangeToCollectionTests
    {
        [TestMethod]
        public void Default()
        {
            var list = new List<int>();
            WorkflowTester.Run(new AddRangeToCollection<int>(), GetArgs(list, new[] { 1, 2, 3 }));
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
        }

        [TestMethod]
        public void NullCollection()
        {
            List<int> list = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
                WorkflowTester.Run(new AddRangeToCollection<int>(), GetArgs(list, new[] { 1, 2, 3 })));
        }

        [TestMethod]
        public void NullItems()
        {
            var list = new List<int>();
            Assert.ThrowsException<ArgumentNullException>(() =>
                WorkflowTester.Run(new AddRangeToCollection<int>(), GetArgs(list, null)));
        }

        private static Dictionary<string, object> GetArgs<T>(ICollection<T> collection, T[] items)
        {
            return new Dictionary<string, object>
            {
                { nameof(AddRangeToCollection<T>.Collection), collection },
                { nameof(AddRangeToCollection<T>.Items), items }
            };
        }
    }
}
