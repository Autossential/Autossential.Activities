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
    public class AggregateTests
    {
        private DataTable[] _dataTable;

        [TestInitialize]
        public void CreateTable()
        {
            _dataTable = new DataTable[2];
            _dataTable[0] = DataTableHelper.CreateDataTable<object>(6, new[]
            {
                new object[]{ "A", 1, 1.0, 50.5, true, new DateTime(2022,01,01) },
                new object[]{ "A", 2, 2.0, 120.1, true, new DateTime(2021,12,31) },
                new object[]{ "B", 3, 3.0, 120.1, false, new DateTime(2021,12,30) },
                new object[]{ "C", 4, 7.0, 75.0, false, new DateTime(2021,12,29) },
                new object[]{ "C", 4, 5.0, 10.3, true, new DateTime(2021,12,29) }
            });

            _dataTable[1] = DataTableHelper.CreateDataTable<object>(6, new[]
            {
                new object[]{ "X",     8, null,          8.0,  DBNull.Value, new DateTime(2022,01,01) },
                new object[]{ "Y",    16,  4.2,         null,          null, new DateTime(2021,12,31) },
                new object[]{ null, null,  3.0, DBNull.Value,         false, new DateTime(2021,12,30) },
                new object[]{ "Z",     4,  1.6,         16.5,         false,                     null },
                new object[]{ "K",     4,  5.0,          9.7,          true,                     null }
            });
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void DistinctCount(int tableIndex)
        {
            var result = WorkflowTester.Run(new Aggregate() { Function = AggregateFunction.DistinctCount }, GetArgs(_dataTable[tableIndex]));
            var values = (object[])result.Get(p => p.Result);

            if (tableIndex == 0)
                CollectionAssert.AreEqual(new[] { 3, 4, 5, 4, 2, 4 }, values);
            else
                CollectionAssert.AreEqual(new[] { 5, 4, 5, 4, 3, 4 }, values);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void Min(int tableIndex)
        {
            var result = WorkflowTester.Run(new Aggregate() { Function = AggregateFunction.Min }, GetArgs(_dataTable[tableIndex]));
            var values = (object[])result.Get(p => p.Result);
            if (tableIndex == 0)
                CollectionAssert.AreEqual(new object[] { "A", 1, 1.0, 10.3, false, new DateTime(2021, 12, 29) }, values);
            else
                CollectionAssert.AreEqual(new object[] { "K", 4, 1.6, 8.0, false, new DateTime(2021, 12, 30) }, values);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void Max(int tableIndex)
        {
            var result = WorkflowTester.Run(new Aggregate() { Function = AggregateFunction.Max }, GetArgs(_dataTable[tableIndex]));
            var values = (object[])result.Get(p => p.Result);
            if (tableIndex == 0)
                CollectionAssert.AreEqual(new object[] { "C", 4, 7.0, 120.1, true, new DateTime(2022, 01, 01) }, values);
            else
                CollectionAssert.AreEqual(new object[] { "Z", 16, 5.0, 16.5, true, new DateTime(2022, 01, 01) }, values);
        }

        [TestMethod]
        [DataRow(0, AggregateFunction.Sum, 14, 18, 376)]
        [DataRow(0, AggregateFunction.Average, 2.8, 3.6, 75.2)]
        [DataRow(0, AggregateFunction.Median, 3, 3, 75)]
        [DataRow(0, AggregateFunction.StandardDeviation, 1.30384048104053, 2.40831891575846, 47.0482730820165)]
        [DataRow(0, AggregateFunction.Variance, 1.7, 5.8, 2213.54)]
        [DataRow(1, AggregateFunction.Sum, 32, 13.8, 34.2)]
        [DataRow(1, AggregateFunction.Average, 8, 3.45, 11.4)]
        [DataRow(1, AggregateFunction.Median, 6, 3.6, 9.7)]
        [DataRow(1, AggregateFunction.StandardDeviation, 5.65685424949238, 1.48211560502771, 4.4977772288098)]
        [DataRow(1, AggregateFunction.Variance, 32, 2.19666666666667, 20.23)]
        public void NumericAggregations(int tableIndex, AggregateFunction function, double value1, double value2, double value3)
        {
            var result = WorkflowTester.Run(new Aggregate() { Function = function }, GetArgs(_dataTable[tableIndex]));
            var values = (object[])result.Get(p => p.Result);

            Assert.IsNull(values[0]);
            Assert.IsNull(values[4]);
            Assert.IsNull(values[5]);

            double tolerance = .0000000000001;
            Assert.IsTrue(Math.Abs(value1 - (double)values[1]) <= tolerance);
            Assert.IsTrue(Math.Abs(value2 - (double)values[2]) <= tolerance);
            Assert.IsTrue(Math.Abs(value3 - (double)values[3]) <= tolerance);
        }

        [TestMethod]
        [DataRow(0, 3)]
        [DataRow(0, 1, 2)]
        public void CustomColumns(int tableIndex, params int[] columns)
        {
            var result = WorkflowTester.CompileAndRun(new Aggregate()
            {
                Columns = new InArgument<int[]>(_ => columns)
            }, GetArgs(_dataTable[tableIndex]));

            var values = (object[])result.Get(p => p.Result);
            for (int i = 0; i < values.Length; i++)
            {
                if (columns.Contains(i))
                    Assert.IsNotNull(values[i]);
                else
                    Assert.IsNull(values[i]);
            }
        }

        [TestMethod]
        public void InvalidCustomColumns()
        {
            Assert.ThrowsException<InvalidWorkflowException>(() =>
                 WorkflowTester.CompileAndRun(new Aggregate()
                 {
                     Columns = new InArgument<string>("invalid")
                 }, GetArgs(_dataTable[0])));
        }


        private static Dictionary<string, object> GetArgs(DataTable inputDataTable)
        {
            return new Dictionary<string, object>
            {
                { nameof(Aggregate.InputDataTable), inputDataTable }
            };
        }
    }
}
