using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using Autossential.Shared;
using Autossential.Shared.Utils;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities
{
    public sealed class Aggregate : CodeActivity<object[]>
    {
        public InArgument<DataTable> InputDataTable { get; set; }

        public AggregateFunction Function { get; set; } = AggregateFunction.Sum;

        public InArgument Columns { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(Result, nameof(Result), true);
            metadata.AddRuntimeArgument(InputDataTable, nameof(InputDataTable), true);

            if (Columns == null) return;

            if (Columns.IsArgumentTypeAnyCompatible<IEnumerable<int>, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(Columns, Columns.ArgumentType, nameof(Columns), false);
                return;
            }

            metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("IEnumerable<string> or IEnumerable<int>", nameof(Columns)));
        }

        protected override object[] Execute(CodeActivityContext context)
        {
            var dt = InputDataTable.Get(context);
            var columns = Columns?.Get(context);

            var columnIndexes = DataTableUtil.IdentifyDataColumns(dt, columns);

            var result = new object[dt.Columns.Count];
            if (dt.Rows.Count > 0)
                AggregateTo(result, GetConvertibleColumns(dt, columnIndexes), dt.AsEnumerable());

            return result;
        }

        private void AggregateTo(object[] result, Dictionary<int, AggregateFunction[]> convertibles, IEnumerable<DataRow> rows)
        {
            foreach (var c in convertibles)
            {
                if (!c.Value.Contains(Function))
                    continue;

                if (Function == AggregateFunction.DistinctCount)
                {
                    result[c.Key] = rows.Select(row => row[c.Key]).Distinct().Count();
                    continue;
                }

                var validRows = rows.Where(row => !row.IsNull(c.Key)).ToArray();
                switch (Function)
                {
                    case AggregateFunction.Average:
                        result[c.Key] = Average(c.Key, validRows);
                        break;

                    case AggregateFunction.StandardDeviation:
                        result[c.Key] = StDev(c.Key, validRows);
                        break;

                    case AggregateFunction.Max:
                        result[c.Key] = validRows.Max(row => row[c.Key]);
                        break;

                    case AggregateFunction.Median:
                        result[c.Key] = Median(c.Key, validRows);
                        break;

                    case AggregateFunction.Min:
                        result[c.Key] = validRows.Min(row => row[c.Key]);
                        break;

                    case AggregateFunction.Sum:
                        result[c.Key] = Sum(c.Key, validRows);
                        break;

                    case AggregateFunction.Variance:
                        result[c.Key] = Variance(c.Key, validRows);
                        break;
                }
            }
        }

        private static Dictionary<int, AggregateFunction[]> GetConvertibleColumns(DataTable table, IEnumerable<int> columnIndexes)
        {
            var hasColumnIndexes = columnIndexes.Any();

            var convertibles = new Dictionary<int, AggregateFunction[]>();

            foreach (DataColumn col in table.Columns)
            {
                if (hasColumnIndexes && !columnIndexes.Contains(col.Ordinal))
                    continue;

                var dataType = col.DataType;
                if (dataType == typeof(object))
                {
                    // Determines the real column type based on the first value found
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.IsNull(col.Ordinal))
                            continue;

                        dataType = row[col.Ordinal].GetType();
                        break;
                    }
                }

                var functions = new[] { AggregateFunction.DistinctCount };

                if (DataTableUtil.IsNumericDataType(dataType))
                {
                    functions = new[] {
                         AggregateFunction.Sum,
                         AggregateFunction.Average,
                         AggregateFunction.Min,
                         AggregateFunction.Max,
                         AggregateFunction.Median,
                         AggregateFunction.DistinctCount,
                         AggregateFunction.StandardDeviation,
                         AggregateFunction.Variance
                     };
                }

                if (new[] {
                    typeof(bool),
                    typeof(char),
                    typeof(string),
                    typeof(DateTime),
                    typeof(Guid),
                    typeof(TimeSpan),
                }.Contains(dataType))
                {
                    functions = new[]
                    {
                        AggregateFunction.Min,
                        AggregateFunction.Max,
                        AggregateFunction.DistinctCount
                    };
                }

                convertibles.Add(col.Ordinal, functions);
            }

            return convertibles;
        }

        #region Math functions

        private static double Sum(int columnIndex, DataRow[] rows)
        {
            dynamic total = 0;
            for (int i = 0; i < rows.Length; i++)
                total += (dynamic)rows[i][columnIndex];

            return total;
        }

        private static double Average(int columnIndex, DataRow[] rows) => Sum(columnIndex, rows) / rows.Length;

        private static double Median(int columnIndex, DataRow[] rows)
        {
            var values = rows.Select(row => row[columnIndex]).ToArray();
            Array.Sort(values);
            int index = values.Length / 2;
            if (values.Length % 2 == 0)
            {
                return ((dynamic)values[index] + (dynamic)values[index - 1]) / 2;
            }

            var value = values[index];
            if (value is double valueAsDouble)
                return valueAsDouble;

            return (double)Convert.ChangeType(value, typeof(double));
        }

        private static double StDev(int columnIndex, DataRow[] rows)
        {
            var values = new double[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                var value = rows[i][columnIndex];
                if (value is double valueDouble)
                {
                    values[i] = valueDouble;
                    continue;
                }

                values[i] = (double)Convert.ChangeType(value, typeof(double));
            }

            var avg = values.Average();
            var sum = values.Sum(v => Math.Pow(v - avg, 2));
            return Math.Sqrt(sum / (values.Length - 1));
        }

        public static double Variance(int columnIndex, DataRow[] rows)
        {
            double avg = Average(columnIndex, rows);
            double variance = 0;
            foreach (DataRow row in rows)
                variance += Math.Pow((dynamic)row[columnIndex] - avg, 2.0);

            return variance / (rows.Length - 1);
        }

        #endregion Math functions
    }
}