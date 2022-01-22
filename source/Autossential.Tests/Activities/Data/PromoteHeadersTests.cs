using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class PromoteHeadersTests
    {
        [TestMethod]
        public void Default()
        {
            var dt = DataTableHelper.CreateDataTable<string>(3, new[] {
                new string[]{ "A", "B", "C" } ,
                new string[]{ "Row1", "Row1", "Row1" } ,
                new string[]{ "Row2", "Row2", "Row2" } ,
            });

            var result = WorkflowTester.Run(new PromoteHeaders(), GetArgs(dt, null));
            var outDt = (DataTable)result.Get(p => p.Result);

            Assert.AreEqual(2, outDt.Rows.Count);

            Assert.AreEqual("A", outDt.Columns[0].ColumnName);
            Assert.AreEqual("B", outDt.Columns[1].ColumnName);
            Assert.AreEqual("C", outDt.Columns[2].ColumnName);
        }

        [TestMethod]
        [DataRow("Empty")]
        [DataRow("Vazio")]
        public void EmptyColumNames(string emptyColName)
        {
            var dt = DataTableHelper.CreateDataTable<string>(1, new[] {
                new string[]{ "" }
            });

            var result = WorkflowTester.Run(new PromoteHeaders(), GetArgs(dt, emptyColName));

            var outDt = (DataTable)result.Get(p => p.Result);
            Assert.AreEqual(0, outDt.Rows.Count);
            Assert.AreEqual(emptyColName, outDt.Columns[0].ColumnName);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void AutoRename(bool rename)
        {
            var dt = DataTableHelper.CreateDataTable<string>(3, new[] {
                new string[]{ "Name", "Name", "Name" }
            });

            ActivityResult<PromoteHeaders> Execute()
            {
                return WorkflowTester.Run(new PromoteHeaders
                {
                    AutoRename = rename,
                }, GetArgs(dt, null));
            }

            if (rename)
            {
                var result = Execute();
                var outDt = (DataTable)result.Get(p => p.Result);
                Assert.AreEqual("Name", outDt.Columns[0].ColumnName);
                Assert.AreEqual("Name1", outDt.Columns[1].ColumnName);
                Assert.AreEqual("Name2", outDt.Columns[2].ColumnName);
            }
            else
            {
                Assert.ThrowsException<DuplicateNameException>(Execute);
            }
        }

        [TestMethod]
        public void NoData()
        {
            var dt = DataTableHelper.CreateDataTable<object>(5);
            Assert.ThrowsException<InvalidOperationException>(() => WorkflowTester.Run(new PromoteHeaders(), GetArgs(dt, null)));
        }

        [TestMethod]
        [DataRow(null)]
        public void EmptyColumnName(string emptyColumnName)
        {
            var dt = DataTableHelper.CreateDataTable<string>(2, new[] {
                new string[]{ "A", "B" }
            });

            Assert.ThrowsException<InvalidWorkflowException>(() => WorkflowTester.Run(new PromoteHeaders
            {
                EmptyColumnName = emptyColumnName,
            }, GetArgs(dt, null)));
        }

        private static Dictionary<string, object> GetArgs(object inputDataTable, object emptyColumnName)
        {
            return new Dictionary<string, object>
            {
                { nameof(PromoteHeaders.InputDataTable), inputDataTable },
                { nameof(PromoteHeaders.EmptyColumnName), emptyColumnName }
            };
        }
    }
}
