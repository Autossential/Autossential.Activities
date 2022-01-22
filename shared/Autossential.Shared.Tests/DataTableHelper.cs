using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities.Test
{
    public static class DataTableHelper
    {
        public static DataTable CreateDataTable(Type[] columnTypes, IEnumerable<object[]> rows = null)
        {
            var dt = new DataTable();
            for (int i = 0; i < columnTypes.Length; i++)
                dt.Columns.Add("Col" + i, columnTypes[i]);

            if (rows != null) FillDataTable(dt, rows);
            return dt;
        }

        public static DataTable CreateDataTable<TColumnType>(int columnsCount, IEnumerable<object[]> rows = null)
        {
            return CreateDataTable(Enumerable.Range(0, columnsCount).Select(p => typeof(TColumnType)).ToArray(), rows);
        }

        public static DataTable Generate(int numberOfRows, int numberOfCols, Func<int, int, string> valueGenerator)
        {
            var structure = Enumerable.Range(0, numberOfRows)
                .Select(i => Enumerable.Range(0, numberOfCols).Select(j => valueGenerator(i, j)).ToArray());

            return CreateDataTable<string>(numberOfCols, structure.ToArray());
        }


        public static void FillDataTable(DataTable dt, IEnumerable<object[]> data)
        {
            foreach (object[] obj in data)
            {
                var row = dt.NewRow();
                row.ItemArray = obj;
                dt.Rows.Add(row);
            }
        }
    }
}
