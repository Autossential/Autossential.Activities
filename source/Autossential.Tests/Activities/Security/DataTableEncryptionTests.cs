using Autossential.Activities.Security.Algorithms;
using Autossential.Core.Security;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class DataTableEncryptionTests
    {
        private const string CryptoKey = "test-A3B3EAC5-F4E6-4B03-9796-A329DC1F860B";

        [TestMethod]
        [DataRow(nameof(AesAlgorithmEncryption))]
        [DataRow(nameof(DESAlgorithmEncryption))]
        [DataRow(nameof(RC2AlgorithmEncryption))]
        [DataRow(nameof(RijndaelAlgorithmEncryption))]
        [DataRow(nameof(TripleDESAlgorithmEncryption))]
        public void Default(string alg)
        {
            CodeActivity<IEncryption> handler = null;

            switch (alg)
            {
                case nameof(AesAlgorithmEncryption):
                    handler = new AesAlgorithmEncryption();
                    break;
                case nameof(DESAlgorithmEncryption):
                    handler = new DESAlgorithmEncryption();
                    break;
                case nameof(RC2AlgorithmEncryption):
                    handler = new RC2AlgorithmEncryption();
                    break;
                case nameof(RijndaelAlgorithmEncryption):
                    handler = new RijndaelAlgorithmEncryption();
                    break;
                case nameof(TripleDESAlgorithmEncryption):
                    handler = new TripleDESAlgorithmEncryption();
                    break;
            }

            var dt = DataTableHelper.CreateDataTable<object>(6, new[]
            {
                new object[] { "Brazil", null, DBNull.Value, 1, "Latin", "America" },
                new object[] { "Canada", "", DateTime.Today, 0, "North", "America" }
            });

            var activity = new DataTableEncryption()
            {
                Algorithm = new ActivityFunc<IEncryption>()
                {
                    Handler = handler
                },
                ParallelProcessing = false,
                Key = new InArgument<string>(CryptoKey)
            };

            var encrypted = WorkflowTester.Invoke(activity, GetArgs(dt));

            var row = encrypted.Rows[0];
            CollectionAssert.AreNotEqual(new object[] { "Brazil", 1, "Latin", "America" }, new object[] { row["Col0"], row["Col3"], row["Col4"], row["Col5"] });
            CollectionAssert.AreEqual(new object[] { DBNull.Value, DBNull.Value }, new object[] { row["Col1"], row["Col2"] });

            row = encrypted.Rows[1];
            CollectionAssert.AreNotEqual(new object[] { "Canada", DateTime.Today, 0, "North", "America" }, new object[] { row["Col0"], row["Col2"], row["Col3"], row["Col4"], row["Col5"] });
            Assert.AreEqual("", row[1]);

            activity.Action = Core.Enums.CryptoActions.Decrypt;
            var decrypted = WorkflowTester.Invoke(activity, GetArgs(encrypted));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var dr = dt.Rows[i];
                    if (dr.IsNull(j))
                    {
                        Assert.AreEqual(dr[j], decrypted.Rows[i][j]);
                    }
                    else
                    {
                        Assert.AreEqual(dr[j].ToString(), decrypted.Rows[i][j]);
                    }
                }
            }
        }

        [TestMethod]
        public void ParallelPerformance()
        {
            var dt = DataTableHelper.Generate(10, 5, (i, j) => $"Row {i}, Col {j}");
            var activitity = new DataTableEncryption
            {
                Algorithm = new ActivityFunc<IEncryption>
                {
                    Handler = new AesAlgorithmEncryption()
                },
                Key = new InArgument<string>(CryptoKey),
            };

            long elapsed = 0;
            activitity.ParallelProcessing = false;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            WorkflowTester.Invoke(activitity, GetArgs(dt));
            elapsed = sw.ElapsedMilliseconds;

            activitity.ParallelProcessing = true;
            var args = GetArgs(dt);
            args.Add(nameof(DataTableEncryption.Sort), "Col0 DESC");

            sw.Restart();
            WorkflowTester.Invoke(activitity, args);
            sw.Stop();

            Trace.Write($"Parallel: No = {elapsed}, Yes = {sw.ElapsedMilliseconds}");
            Assert.IsTrue(sw.ElapsedMilliseconds < elapsed);
        }

        [TestMethod]
        [DataRow("Col0,Col2,Col4", true)]
        [DataRow("1,3,5,7", false)]
        public void CustomColumns(string expression, bool isColNames)
        {
            var values = expression.Split(',');

            InArgument columns = isColNames
                ? new InArgument<string[]>(_ => values)
                : (InArgument)new InArgument<int[]>(_ => values.Select(int.Parse).ToArray());

            var dt = DataTableHelper.Generate(5, 7, (i, j) => $"{i},{j}");

            var result = WorkflowTester.CompileAndInvoke(new DataTableEncryption
            {
                Algorithm = new ActivityFunc<IEncryption>
                {
                    Handler = new AesAlgorithmEncryption()
                },
                Columns = columns,
                Key = new InArgument<string>(CryptoKey),
                ParallelProcessing = false
            }, GetArgs(dt));

            var indexes = isColNames
                ? expression.Split(',').Select(p => dt.Columns[p].Ordinal)
                : expression.Split(',').Select(int.Parse);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (indexes.Contains(j))
                    {
                        Assert.AreNotEqual(dt.Rows[i][j], result.Rows[i][j]);
                        continue;
                    }

                    Assert.AreEqual(dt.Rows[i][j], result.Rows[i][j]);
                }
            }
        }

        private static IDictionary<string, object> GetArgs(DataTable inputDataTable)
        {
            var dic = new Dictionary<string, object>
            {
                { nameof(DataTableEncryption.Input), inputDataTable },
                { nameof(DataTableEncryption.TextEncoding), Encoding.UTF8  }
            };

            return dic;
        }
    }
}