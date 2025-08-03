using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using Autossential.Core.Security;
using Autossential.Shared;
using Autossential.Shared.Utils;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class DataTableEncryption : EncryptionBase<DataTable>
    {
        public InArgument Columns { get; set; }
        public InArgument<string> Sort { get; set; }
        public bool ParallelProcessing { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.AddRuntimeArgument(Sort, nameof(Sort), false);

            if (Columns == null) return;

            if (Columns.IsArgumentTypeAnyCompatible<IEnumerable<int>, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(Columns, Columns.ArgumentType, nameof(Columns), false);
                return;
            }

            metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("IEnumerable<int> or IEnumerable<string>", nameof(Columns)));
        }
        protected override void Execute(NativeActivityContext context)
        {
            context.ScheduleFunc(Algorithm, OnComplete);
        }

        private DataTable CreateCryptoDataTable(DataTable sourceDataTable, List<int> cryptoColumns)
        {
            var allColumns = cryptoColumns.Count == 0;
            var result = new DataTable();
            foreach (DataColumn col in sourceDataTable.Columns)
                result.Columns.Add(col.ColumnName, allColumns || cryptoColumns.Contains(col.Ordinal) ? typeof(string) : col.DataType);

            return result;
        }

        private void OnComplete(NativeActivityContext context, ActivityInstance completedInstance, IEncryption result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            var input = Input.Get(context);
            var colsLen = input.Columns.Count;
            var encoding = TextEncoding.Get(context) ?? Encoding.UTF8;
            var pass = GetRawKey(context);

            var sortBy = Sort.Get(context);
            var columns = DataTableUtil.IdentifyDataColumns(input, Columns?.Get(context), Enumerable.Range(0, colsLen)).ToList();

            Func<string, string> action;
            if (Action == CryptoActions.Encrypt)
            {
                action = (string value) => Convert.ToBase64String(result.Encrypt(encoding.GetBytes(value), encoding.GetBytes(pass)));
            }
            else
            {
                action = (string value) => encoding.GetString(result.Decrypt(Convert.FromBase64String(value), encoding.GetBytes(pass)));
            }

            var rows = input.AsEnumerable();
            var values = new List<object[]>();

            void Add(DataRow row)
            {
                values.Add(ApplyTo(row, colsLen, columns, action));
            }

            if (ParallelProcessing)
            {
                Parallel.ForEach(rows, Add);
            }
            else
            {
                foreach (DataRow row in input.Rows)
                    Add(row);
            }

            var output = CreateCryptoDataTable(input, columns);
            output.BeginLoadData();

            foreach (var value in values)
                output.LoadDataRow(value, false);

            output.AcceptChanges();
            output.EndLoadData();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                output.DefaultView.Sort = sortBy;
                output = output.DefaultView.ToTable();
            }

            Result.Set(context, output);
        }

        private object[] ApplyTo(DataRow row, int colsLen, List<int> columns, Func<string, string> action)
        {
            var values = new object[colsLen];
            for (int i = 0; i < colsLen; i++)
            {
                if (row.IsNull(i)
                    || row[i].ToString()?.Length == 0
                    || !columns.Contains(i)
                    || row[i].GetType() == typeof(byte[]))
                {
                    values[i] = row[i];
                    continue;
                }

                values[i] = action(row[i].ToString());
            }

            return values;
        }
    }
}