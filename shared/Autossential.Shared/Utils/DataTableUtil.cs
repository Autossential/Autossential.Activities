using System;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Shared.Utils
{
    public static class DataTableUtil
    {
        public static IEnumerable<int> IdentifyDataColumns(DataTable dataTable, object columns, IEnumerable<int> defaultValue = null)
        {
            if (columns is IEnumerable<int> indexes)
            {
                foreach (var index in indexes)
                {
                    if (index < dataTable.Columns.Count)
                        yield return index;
                }
            }
            else if (columns is IEnumerable<string> names)
            {
                foreach (var name in names)
                {
                    if (dataTable.Columns.Contains(name))
                        yield return dataTable.Columns[name].Ordinal;
                }
            }
            else if (defaultValue != null)
            {
                foreach (var index in defaultValue)
                {
                    if (index < dataTable.Columns.Count)
                        yield return index;
                }
            }
        }

        public static bool IsNumericDataType(Type type)
        {
            return type == typeof(byte)
                || type == typeof(decimal)
                || type == typeof(double)
                || type == typeof(short)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(sbyte)
                || type == typeof(float)
                || type == typeof(ushort)
                || type == typeof(uint)
                || type == typeof(ulong);
        }
    }
}