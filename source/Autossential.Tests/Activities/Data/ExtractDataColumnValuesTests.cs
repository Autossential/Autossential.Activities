using Autossential.Core.Enums;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class ExtractDataColumnValuesTests
    {
        private static DataTable Initialize()
        {
            return DataTableHelper.CreateDataTable(new[] { typeof(string), typeof(int) }, new[]
            {
                new object[] { "A", 1 },
                new object[] { "B", 2 },
                new object[] { "C", 3 }
            });
        }

        [TestMethod]
        public void DefaultString()
        {
            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<string>()
            {
                Column = new InArgument<string>("Col0")
            }, GetArgs(Initialize(), "Col0"));

            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, result);
        }


        [TestMethod]
        public void DefaultInt()
        {
            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<int>()
            {
                Column = new InArgument<int>(1)
            }, GetArgs(Initialize(), 1));

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [TestMethod]
        public void NullColumn()
        {
            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>(), GetArgs(Initialize(), 1)));
        }

        [TestMethod]
        public void InvalidColumnArgument()
        {
            Assert.ThrowsException<InvalidWorkflowException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>
            {
                Column = new InArgument<bool>(true)
            }, GetArgs(Initialize(), 1)));
        }

        [TestMethod]
        public void InvalidColumnReference()
        {
            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>
            {
                Column = new InArgument<string>("InvalidColName")
            }, GetArgs(Initialize(), 1)));

            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>
            {
                Column = new InArgument<int>(3)
            }, GetArgs(Initialize(), 1)));
        }

        [TestMethod]
        public void EmptyTable()
        {
            var dt = new DataTable();
            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<string>
            {
                Column = new InArgument<string>("Col0")
            }, GetArgs(dt, null));

            Assert.AreEqual(0, result.Length);

        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void Sanitize(bool enabled)
        {
            var dt = DataTableHelper.CreateDataTable<object>(1, new[]
            {
                new object[]{ 1 },
                new object[]{ null },
                new object[]{ "" },
                new object[]{ "   " },
            });

            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<object>
            {
                Column = new InArgument<string>("Col0"),
                Sanitize = enabled
            }, GetArgs(dt, null));

            if (enabled)
            {
                CollectionAssert.AreEqual(new[] { 1 }, result);
            }
            else
            {
                CollectionAssert.AreEqual(new object[] { 1, null, "", "   " }, result);
            }
        }

        [TestMethod]
        public void Trim()
        {
            var dt = DataTableHelper.CreateDataTable<string>(1, new[] {
                new string[] {"  A  "},
                new string[] {"**B**"},
                new string[] {"--C"},
                new string[] {"D:"}
            });

            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<string>
            {
                Column = new InArgument<string>("Col0"),
                Trim = new InArgument<char[]>(_ => new[] { ' ', '*', '-', ':' })
            }, GetArgs(dt, null));

            CollectionAssert.AreEqual(new string[] { "A", "B", "C", "D" }, result);
        }

        [TestMethod]
        [DataRow(Core.Enums.TextCase.None)]
        [DataRow(Core.Enums.TextCase.ToUpper)]
        [DataRow(Core.Enums.TextCase.ToLower)]
        public void TextCase(TextCase textCase)
        {
            var dt = DataTableHelper.CreateDataTable<string>(1, new[] {
                new string[] { "Alexandre" }
            });

            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<string>
            {
                Column = new InArgument<string>("Col0"),
                TextCase = textCase
            }, GetArgs(dt, null));

            Assert.AreEqual(1, result.Length);

            switch (textCase)
            {
                case Core.Enums.TextCase.None:
                    Assert.AreEqual("Alexandre", result[0]);
                    break;
                case Core.Enums.TextCase.ToUpper:
                    Assert.AreEqual("ALEXANDRE", result[0]);
                    break;
                case Core.Enums.TextCase.ToLower:
                    Assert.AreEqual("alexandre", result[0]);
                    break;
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void Unique(bool enabled)
        {
            var rows = new[] {
                new object [] { "A" },
                new object [] { "B" },
                new object [] { 1 },
                new object [] { 2 },
                new object [] { 1 },
                new object [] { 3 },
                new object [] { "" },
                new object [] { "X" },
                new object [] { "A" },
                new object [] { "B" },
                new object [] { true },
                new object [] { true },
                new object [] { null },
                new object [] { false }
            };

            var dt = DataTableHelper.CreateDataTable<object>(1, rows);

            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<object>
            {
                Column = new InArgument<string>("Col0"),
                Unique = enabled
            }, GetArgs(dt, null));

            if (enabled)
            {
                var distinctValues = rows.Select(v => v[0]).Distinct().ToArray();
                CollectionAssert.AreEqual(distinctValues, result);
            }
            else
            {
                var values = rows.Select(v => v[0]).ToArray();
                CollectionAssert.AreEqual(values, result);
            }
        }

        private static IDictionary<string, object> GetArgs(DataTable inputDataTable, object defaultValue)
        {
            return new Dictionary<string, object>
            {
                { nameof(ExtractDataColumnValues<object>.InputDataTable), inputDataTable },
                { nameof(ExtractDataColumnValues<object>.DefaultValue), defaultValue },
            };
        }
    }
}
