using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using Autossential.Shared;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities
{

    public class ExtractDataColumnValues<T> : CodeActivity<T[]>
    {
        public InArgument<DataTable> InputDataTable { get; set; }
        public InArgument Column { get; set; }
        public InArgument<T> DefaultValue { get; set; }
        public InArgument<char[]> Trim { get; set; }
        public InArgument<bool> Sanitize { get; set; }
        public InArgument<bool> Unique { get; set; }
        public TextCase TextCase { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(InputDataTable, nameof(InputDataTable), true);
            metadata.AddRuntimeArgument(DefaultValue, nameof(DefaultValue), false);
            metadata.AddRuntimeArgument(Result, nameof(Result), true);

            if (Column == null)
            {
                metadata.AddRuntimeArgument(Column, typeof(string), nameof(Column), true);
            }
            else if (Column.IsArgumentTypeAnyCompatible<string, int>())
            {
                metadata.AddRuntimeArgument(Column, Column.ArgumentType, nameof(Column), true);
            }
            else
            {
                metadata.AddValidationError(Resources.Validation_TypeErrorFormat("int or string", nameof(Column)));
            }
        }

        protected override T[] Execute(CodeActivityContext context)
        {
            var dt = InputDataTable.Get(context);
            if (dt.Columns.Count == 0)
                return new T[0];

            var col = Column.Get(context);

            int index = 0;
            if (col is string colName)
            {
                try
                {
                    index = dt.Columns[colName].Ordinal;
                }
                catch (NullReferenceException e)
                {
                    throw new ArgumentException(Resources.ExtractDataColumnValues_ErrorMsg_InvalidColumnNameFormat(colName), e);
                }
            }
            else
            {
                index = (int)col;
                if (index >= dt.Columns.Count)
                    throw new ArgumentException(Resources.ExtractDataColumnValues_ErrorMsg_InvalidColumnIndexFormat(index));
            }

            var rows = dt.AsEnumerable();
            if (Sanitize.Get(context))
            {
                rows = rows.Where(row =>
                    !row.IsNull(index)
                    && row[index].ToString().Trim() != string.Empty
                );
            }

            var result = ConvertValues(rows.Select(row => row[index]).ToArray(), DefaultValue.Get(context));
            if (typeof(T) == typeof(string))
            {
                var trim = Trim.Get(context);
                if (trim != null && trim.Length > 0)
                    result = result.Select(value => (value as string)?.Trim(trim)) as IEnumerable<T>;

                if (TextCase == TextCase.ToLower)
                {
                    result = result.Select(value => (value as string)?.ToLower()) as IEnumerable<T>;
                }
                else if (TextCase == TextCase.ToUpper)
                {
                    result = result.Select(value => (value as string)?.ToUpper()) as IEnumerable<T>;
                }
            }

            if (Unique.Get(context))
                result = result.Distinct();

            return result.ToArray();
        }

        private static IEnumerable<T> ConvertValues(object[] values, T defaultValue = default)
        {
            var value = defaultValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == DBNull.Value)
                {
                    yield return defaultValue;
                    continue;
                }

                try
                {
                    value = (T)values[i];
                }
                catch (InvalidCastException)
                {
                    try
                    {
                        value = (T)Convert.ChangeType(values[i], typeof(T));
                    }
                    catch (Exception)
                    {
                        value = defaultValue;
                    }
                }
                catch (Exception)
                {
                    value = defaultValue;
                }
            }

            yield return value;
        }
    }
}