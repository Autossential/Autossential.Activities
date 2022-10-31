using Autossential.Activities.Design.Designers;
using Autossential.Activities.Design.PropertyEditors;
using Autossential.Activities.Properties;
using Autossential.Activities.Security.Algorithms;
using Autossential.Activities.Workflow;
using Autossential.Core.Security.Algorithms;
using Autossential.Shared.Activities.Design;
using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;
using System.ComponentModel;
using System.Linq;

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
        public const string APPS_AND_DIAGNOSTICS_CATEGORY = MAIN_CATEGORY + ".Apps & Diagnostics";

        public void Register()
        {
            var dataTable = new CategoryAttribute(DATA_CATEGORY);
            var workflow = new CategoryAttribute(WORKFLOW_CATEGORY);
            var file = new CategoryAttribute(FILE_CATEGORY);
            var fileCompression = new CategoryAttribute(FILE_COMPRESSION_CATEGORY);
            var programming = new CategoryAttribute(PROGRAMMING_CATEGORY);
            var security = new CategoryAttribute(SECURITY_CATEGORY);
            var securityAlgorithms = new CategoryAttribute(SECURITY_ALGORITHMS_CATEGORY);
            var appsAndDiagnostics = new CategoryAttribute(APPS_AND_DIAGNOSTICS_CATEGORY);

            ActivitiesAttributesBuilder.Build(Resources.ResourceManager, builder =>
            {
                builder.SetDefaultCategories(
                    Resources.Input_Category,
                    Resources.Output_Category,
                    Resources.InputOutput_Category,
                    Resources.Options_Category);

                // DATA TABLE
                builder
                    .Register<DataTableToText, DataTableToTextDesigner>(dataTable)
                    .Register<TransposeData, TransposeDataDesigner>(dataTable)
                    .Register<Aggregate, AggregateDesigner>(dataTable, m =>
                    {
                        //m.Register(new CategoryAttribute(Resources.Output_Category), p => p.Detached);
                        m.Register(new CategoryAttribute(Resources.Options_Category), p => p.Columns);
                    })
                    .Register<DataRowToDictionary, DataRowToDictionaryDesigner>(dataTable)
                    .Register<DictionaryToDataTable, DictionaryToDataTableDesigner>(dataTable)
                    .Register<RemoveEmptyRows, RemoveEmptyRowsDesigner>(dataTable, m =>
                    {
                        m.Register(new CategoryAttribute(Resources.RemoveEmptyRows_CustomOptions_Category),
                            p => p.Columns,
                            p => p.Operator);
                    })
                    .Register<RemoveDataColumns, RemoveDataColumnsDesigner>(dataTable)
                    .Register<RemoveDuplicateRows, RemoveDuplicateRowsDesigner>(dataTable, m => m.Register(p => p.Columns, new CategoryAttribute(Resources.Options_Category)))
                    .Register<PromoteHeaders, PromoteHeadersDesigner>(dataTable, m => m.Register(p => p.EmptyColumnName, new CategoryAttribute(Resources.Options_Category)))
                    .Register(typeof(ExtractDataColumnValues<>), typeof(ExtractDataColumnValuesDesigner), new Attribute[] { dataTable, new DefaultTypeArgumentAttribute(typeof(object)) });


                // FILE
                builder
                    .Register<CleanUpFolder, CleanUpFolderDesigner>(file, m =>
                    {
                        m.Register(new CategoryAttribute(Resources.Options_Category),
                            p => p.LastWriteTime,
                            p => p.SearchPattern);
                    })
                    .Register<EnumerateFiles, EnumerateFilesDesigner>(file, m => m.Register(new CategoryAttribute(Resources.Options_Category), p => p.SearchPattern))
                    .Register<WaitFile, WaitFileDesigner>(file, m =>
                        m.Register(new EditorAttribute(typeof(BooleanPropertyEditor), typeof(DialogPropertyValueEditor)),
                            p => p.ContinueOnError)
                     )
                    .Register<WaitDynamicFile, WaitDynamicFileDesigner>(file);


                // FILE COMPRESSION
                builder
                    .Register<Zip, ZipDesigner>(fileCompression)
                    .Register<ZipEntriesCount, ZipEntriesCountDesigner>(fileCompression)
                    .Register<Unzip, UnzipDesigner>(fileCompression);


                // WORKFLOW
                builder
                    .Register<CheckPoint, CheckPointDesigner>(workflow, m =>
                        m.Register(new EditorAttribute(typeof(ArgumentDictionaryPropertyEditor), typeof(DialogPropertyValueEditor)), p => p.Data))
                    .Register<Container, ContainerDesigner>(workflow)
                    .Register<Exit, ExitDesigner>(workflow)
                    .Register<Next, NextDesigner>(workflow)
                    .Register<Iterate, IterateDesigner>(workflow, m => m.Register(new CategoryAttribute(Resources.Options_Category), p => p.Reverse))
                    .Register<WhenDo, WhenDoDesigner>(workflow)
                    .Register<RepeatUntilFailure, RepeatUntilFailureDesigner>(workflow);


                // PROGRAMMING
                builder
                    .Register(typeof(AddRangeToCollection<>), typeof(AddRangeToCollectionDesigner), new Attribute[] { programming, new DefaultTypeArgumentAttribute(typeof(object)) })
                    .Register<CultureScope, CultureScopeDesigner>(programming)
                    .Register<Decrement, DecrementDesigner>(programming)
                    .Register<Increment, IncrementDesigner>(programming)
                    .Register<IsTrue, IsTrueDesigner>(programming)
                    .Register<ReplaceTokens, ReplaceTokensDesigner>(programming);


                // SECURITY
                builder
                    .Register<TextEncryption, EncryptionDesigner>(security)
                    .Register<DataTableEncryption, EncryptionDesigner>(security)
                    .Register<AesAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                    .Register<DESAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                    .Register<RC2AlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                    .Register<RijndaelAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms)
                    .Register<TripleDESAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms);

                var encryptionTypes = typeof(SymmetricAlgorithmEncryptionBase<>).GetDerivedTypes().ToArray();

                builder
                    .RegisterToMember(new DescriptionAttribute(Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description), "Iterations", encryptionTypes)
                    .RegisterToMember(new BrowsableAttribute(false), "Result", encryptionTypes);


#if NET6_0
                builder
                    .Register<AesGcmAlgorithmEncryption, CryptoAlgorithmDesigner>(securityAlgorithms, m =>
                    {
                        m.Register<SymmetricAlgorithmEncryptionBase<AesGcmEncryption>>(new DescriptionAttribute(Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description), p => p.Iterations);
                        m.Register<SymmetricAlgorithmEncryptionBase<AesGcmEncryption>>(new BrowsableAttribute(false), p => p.Result);
                    });
#endif


                // APPS & DIAGNOSTICS
                builder
                    .Register<Stopwatch, StopwatchDesigner>(appsAndDiagnostics)
                    .Register<TerminateProcess, TerminateProcessDesigner>(appsAndDiagnostics);
            });
        }
    }
}