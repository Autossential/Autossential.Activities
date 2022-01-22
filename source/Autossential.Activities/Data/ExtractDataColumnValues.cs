using Autossential.Activities.Properties;
using Autossential.Shared;
using System;
using System.Activities;
using System.Data;
using System.Linq;

namespace Autossential.Activities
{
    public class ExtractDataColumnValues<T> : CodeActivity<T[]>
    {
        public InArgument<DataTable> InputDataTable { get; set; }
        public InArgument Column { get; set; }
        public InArgument<T> DefaultValue { get; set; }

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

            var values = dt.AsEnumerable().Select(row => row[index]).ToArray();
            return ConvertValues(values, DefaultValue.Get(context));
        }

        private static T[] ConvertValues(object[] values, T defaultValue = default)
        {
            if (values.Length == 0)
                return new T[0];

            var result = new T[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == DBNull.Value)
                {
                    result[i] = defaultValue;
                    continue;
                }

                try
                {
                    result[i] = (T)values[i];
                }
                catch (InvalidCastException)
                {
                    try
                    {
                        result[i] = (T)Convert.ChangeType(values[i], typeof(T));
                    }
                    catch (Exception)
                    {
                        result[i] = defaultValue;
                    }
                }
                catch (Exception)
                {
                    result[i] = defaultValue;
                }
            }

            return result;
        }
    }
}