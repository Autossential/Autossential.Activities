using Autossential.Activities.Design.Designers;
using Autossential.Activities.Design.PropertyEditors;
using Autossential.Activities.Properties;
using Autossential.Activities.Security.Algorithms;
using Autossential.Core.Security.Algorithms;
using Autossential.Shared.Activities.Design;
using System.Activities;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;
using System.ComponentModel;

namespace Autossential.Activities.Design
{

    public class DesignerMetadata : IRegisterMetadata
    {
        public const string MAIN_CATEGORY = "Autossential";
        public const string DATA_CATEGORY = MAIN_CATEGORY + ".Data";
        public const string FILE_CATEGORY = MAIN_CATEGORY + ".File";
        public const string FILE_COMPRESSION_CATEGORY = FILE_CATEGORY + ".Compression";
        public const string PROGRAMMING_CATEGORY = MAIN_CATEGORY + ".Programming";
        public const string WORKFLOW_CATEGORY = MAIN_CATEGORY + ".Workflow";
        public const string SECURITY_CATEGORY = MAIN_CATEGORY + ".Security";
        public const string SECURITY_ALGORITHMS_CATEGORY = SECURITY_CATEGORY + ".Algorithms";
        public const string MISCELLANEOUS_CATEGORY = MAIN_CATEGORY + ".Misc";

        public void Register()
        {
            var data = new CategoryAttribute(DATA_CATEGORY);
            var workflow = new CategoryAttribute(WORKFLOW_CATEGORY);
            var file = new CategoryAttribute(FILE_CATEGORY);
            var fileCompression = new CategoryAttribute(FILE_COMPRESSION_CATEGORY);
            var programming = new CategoryAttribute(PROGRAMMING_CATEGORY);
            var security = new CategoryAttribute(SECURITY_CATEGORY);
            var securityAlgorithms = new CategoryAttribute(SECURITY_ALGORITHMS_CATEGORY);
            var miscellaneous = new CategoryAttribute(MISCELLANEOUS_CATEGORY);

            var options = new CategoryAttribute(Resources.Options_Category);
            var searchPattern = new DescriptionAttribute(Resources.Common_SearchPattern);
            var searchPatternMode = new DescriptionAttribute(Resources.Common_SearchPatternMode);

            var builder = new ActivitiesTableBuilder(Resources.ResourceManager);

            builder
                .Add<Stopwatch, StopwatchDesigner>(miscellaneous)
                .Add<TerminateProcess, TerminateProcessDesigner>(miscellaneous)
                .Add<MapDrive, MapDriveDesigner>(miscellaneous)
                .Add<UnmapDrive, UnmapDriveDesigner>(miscellaneous);

            builder
                .Add<Aggregate, AggregateDesigner>(data)
                .Add<DataRowToDictionary, DataRowToDictionaryDesigner>(data)
                .Add<DataTableToText, DataTableToTextDesigner>(data)
                .Add<DictionaryToDataTable, DictionaryToDataTableDesigner>(data)
                .Add(typeof(ExtractDataColumnValues<>), typeof(ExtractDataColumnValuesDesigner), data)
                .Add<PromoteHeaders, PromoteHeadersDesigner>(data)
                .Add<RemoveDataColumns, RemoveDataColumnsDesigner>(data)
                .Add<RemoveDuplicateRows, RemoveDuplicateRowsDesigner>(data)
                .Add<RemoveEmptyRows, RemoveEmptyRowsDesigner>(data)
                .Add<TransposeData, TransposeDataDesigner>(data)
                .Add<FillDataColumn, FillDataColumnDesigner>(data)
                .Add(typeof(AddRangeToCollection<>), typeof(AddRangeToCollectionDesigner), data)
                .Add(typeof(AddToDictionary<,>), typeof(AddToDictionaryDesigner), data)
                .Add(typeof(RemoveFromDictionary<,>), typeof(RemoveFromDictionaryDesigner), data);

            builder
                .Add<CleanUpFolder, CleanUpFolderDesigner>(file)
                .Add<EnumerateFiles, EnumerateFilesDesigner>(file)
                .Add<WaitFile, WaitFileDesigner>(file)
                .Add<WaitDynamicFile, WaitDynamicFileDesigner>(file);

            builder
                .Add<Zip, ZipDesigner>(fileCompression)
                .Add<ZipEntriesCount, ZipEntriesCountDesigner>(fileCompression)
                .Add<Unzip, UnzipDesigner>(fileCompression);

            builder
                .Add<CheckPoint, CheckPointDesigner>(workflow)
                .Add<Container, ContainerDesigner>(workflow)
                .Add<Exit, ExitDesigner>(workflow)
                .Add<Iterate, IterateDesigner>(workflow)
                .Add<Next, NextDesigner>(workflow)
                .Add<TimeLoop, TimeLoopDesigner>(workflow)
                .Add<WhenDo, WhenDoDesigner>(workflow);

            builder
                .Add<CultureScope, CultureScopeDesigner>(programming)
                .Add<Decrement, DecrementDesigner>(programming)
                .Add<Increment, IncrementDesigner>(programming)
                .Add<IsTrue, IsTrueDesigner>(programming)
                .Add<ReplaceTokens, ReplaceTokensDesigner>(programming);

            builder
                .Add<DataTableEncryption, EncryptionDesigner>(security)
                .Add<TextEncryption, EncryptionDesigner>(security);

            builder
                .Add<AesAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                .Add<DESAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                .Add<RC2AlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                .Add<RijndaelAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                .Add<TripleDESAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms);

            builder
                .AddToMembers<Aggregate>(options, p => p.Columns)
                .AddToMembers(typeof(ExtractDataColumnValues<>), options, new[]
                {
                    nameof(ExtractDataColumnValues<object>.Sanitize),
                    nameof(ExtractDataColumnValues<object>.Trim),
                    nameof(ExtractDataColumnValues<object>.Unique),
                    nameof(ExtractDataColumnValues<object>.TextCase)})
                .AddToMembers(typeof(ExtractDataColumnValues<>), new BrowsableAttribute(false), new[]
                {
                    nameof(ExtractDataColumnValues<object>.TextCase),
                    nameof(ExtractDataColumnValues<object>.Trim)})
                .AddToMembers<ExtractDataColumnValues<string>>(new BrowsableAttribute(true), p => p.TextCase, p => p.Trim)
                .AddToMembers<RemoveEmptyRows>(new CategoryAttribute(Resources.RemoveEmptyRows_CustomOptions_Category), p => p.Columns, p => p.Operator) // AddCallback

                .AddToMember<CleanUpFolder>(p => p.LastWriteTime, options)
                .AddToMember<CleanUpFolder>(p => p.SearchPattern, options, searchPattern)
                .AddToMember<CleanUpFolder>(p => p.SearchPatternMode, options, searchPatternMode)

                .AddToMember<EnumerateFiles>(p => p.SearchPattern, options, searchPattern)
                .AddToMember<EnumerateFiles>(p => p.SearchPatternMode, options, searchPatternMode)

                .AddToMembers<WaitFile>(options, p => p.Interval, p => p.WaitForExist)
                .AddToMembers<WaitDynamicFile>(options, p => p.Interval, p => p.FromDateTime)

                .AddToMember<CheckPoint>(p => p.Data, new EditorAttribute(typeof(ArgumentDictionaryPropertyEditor), typeof(DialogPropertyValueEditor)))
                .AddToMember<Iterate>(p => p.Reverse, options)
                .AddToMember(typeof(SymmetricAlgorithmEncryptionBase<>), nameof(ActivityWithResult.Result), new BrowsableAttribute(false))
                .AddToMember(typeof(SymmetricAlgorithmEncryptionBase<>), nameof(SymmetricAlgorithmEncryptionBase<AesEncryption>.Iterations), new DescriptionAttribute(Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description))

                .AddToMember<MapDrive>(p => p.Force, options)
                .AddToMember(typeof(AddToDictionary<,>), nameof(AddToDictionary<object, object>.UpdateIfExists), options);

#if NET6_0_OR_GREATER
            builder
                .Add<AesGcmAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                .AddToMember<AesGcmAlgorithmEncryption>(p => p.Iterations, new DescriptionAttribute(Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description))
                .AddToMember<AesGcmAlgorithmEncryption>(p => p.Result, new BrowsableAttribute(false));
#endif

            builder.Commit();
        }
    }
}