
// ################################
// THIS FILE IS AUTO-GENERATE BY T4
// ################################

namespace Autossential.Activities.Properties
{
    using System.Resources;

    public partial class Resources
    {
        public static System.Globalization.CultureInfo Culture { get; set; }

        private static object _internalSyncObject;

        /// <summary>
        /// Thread safe lock object used by this class.
        /// </summary>
        public static object InternalSyncObject
        {
            get
            {
                if (_internalSyncObject is null)
                    System.Threading.Interlocked.CompareExchange(ref _internalSyncObject, new object(), null);

                return _internalSyncObject;
            }
        }
        private static ResourceManager _resourceManager;

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static ResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager is null)
                {
                    System.Threading.Monitor.Enter(InternalSyncObject);

                    try
                    {
                        if (_resourceManager is null)
                            System.Threading.Interlocked.Exchange(ref _resourceManager, new ResourceManager("Autossential.Activities.Properties.Resources", typeof(Resources).Assembly));
                    }
                    finally
                    {
                        System.Threading.Monitor.Exit(InternalSyncObject);
                    }
                }
                return _resourceManager;
            }
        }

        #region FORMATTERS

        /// <summary>
        /// Looks up a localized string similar to 'Please provide a value for {0}.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string Validation_ValueErrorFormat(object arg0)
        {
            return string.Format(Culture, Validation_ValueError, arg0);
        }

        /// <summary>
        /// Looks up a localized string similar to 'Cannot be used outside of {0} activity.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string Validation_ScopeErrorFormat(object arg0)
        {
            return string.Format(Culture, Validation_ScopeError, arg0);
        }

        /// <summary>
        /// Looks up a localized string similar to 'Cannot be used outside of {0} activities.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string Validation_ScopesErrorFormat(object arg0)
        {
            return string.Format(Culture, Validation_ScopesError, arg0);
        }

        /// <summary>
        /// Looks up a localized string similar to 'The accepted range is from {0} and {1}. The value will be clamped.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <param name="arg1">An object (1) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string WaitDynamicFile_ErrorMsg_IntervalRangeFormat(object arg0, object arg1)
        {
            return string.Format(Culture, WaitDynamicFile_ErrorMsg_IntervalRange, arg0, arg1);
        }

        /// <summary>
        /// Looks up a localized string similar to 'Please provide a value of type {0} for {1}.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <param name="arg1">An object (1) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string Validation_TypeErrorFormat(object arg0, object arg1)
        {
            return string.Format(Culture, Validation_TypeError, arg0, arg1);
        }

        /// <summary>
        /// Looks up a localized string similar to 'The column index '{0}' does not exist on the DataTable.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string ExtractDataColumnValues_ErrorMsg_InvalidColumnIndexFormat(object arg0)
        {
            return string.Format(Culture, ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex, arg0);
        }

        /// <summary>
        /// Looks up a localized string similar to 'The accepted range is from {0} and {1}. The value will be clamped.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <param name="arg1">An object (1) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string WaitFile_ErrorMsg_IntervalRangeFormat(object arg0, object arg1)
        {
            return string.Format(Culture, WaitFile_ErrorMsg_IntervalRange, arg0, arg1);
        }

        /// <summary>
        /// Looks up a localized string similar to 'The column '{0}' does not exist on the DataTable.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        public static string ExtractDataColumnValues_ErrorMsg_InvalidColumnNameFormat(object arg0)
        {
            return string.Format(Culture, ExtractDataColumnValues_ErrorMsg_InvalidColumnName, arg0);
        }

        #endregion

        #region GETTERS

        /// <summary>
        /// Looks up a localized string similar to 'The input Dictionary.'.
        /// </summary>
        public static string DictionaryToDataTable_InputDictionary_Description => ResourceManager.GetString("DictionaryToDataTable_InputDictionary_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Timeout'.
        /// </summary>
        public static string WaitDynamicFile_Timeout_DisplayName => ResourceManager.GetString("WaitDynamicFile_Timeout_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Condition which determines if the activity will be evaluated. If empty it assumes True.'.
        /// </summary>
        public static string Next_Condition_Description => ResourceManager.GetString("Next_Condition_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Triple DES'.
        /// </summary>
        public static string TripleDESAlgorithmEncryption_DisplayName => ResourceManager.GetString("TripleDESAlgorithmEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ToCompress'.
        /// </summary>
        public static string Zip_ToCompress_DisplayName => ResourceManager.GetString("Zip_ToCompress_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'There is no rows in the input data table to promote.'.
        /// </summary>
        public static string PromoteHeaders_ErrorMsg_NoData => ResourceManager.GetString("PromoteHeaders_ErrorMsg_NoData", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DES'.
        /// </summary>
        public static string DESAlgorithmEncryption_DisplayName => ResourceManager.GetString("DESAlgorithmEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The expression to be evaluated.'.
        /// </summary>
        public static string CheckPoint_Expression_Description => ResourceManager.GetString("CheckPoint_Expression_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The encoding used to interpret the text and key.'.
        /// </summary>
        public static string TextEncryption_TextEncoding_Description => ResourceManager.GetString("TextEncryption_TextEncoding_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Increments the value of a variable by the specified increment value.'.
        /// </summary>
        public static string Increment_Description => ResourceManager.GetString("Increment_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Add Range To Collection'.
        /// </summary>
        public static string AddRangeToCollection_DisplayName => ResourceManager.GetString("AddRangeToCollection_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Options (Custom Mode)'.
        /// </summary>
        public static string RemoveEmptyRows_CustomOptions_Category => ResourceManager.GetString("RemoveEmptyRows_CustomOptions_Category", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string TextEncryption_Result_DisplayName => ResourceManager.GetString("TextEncryption_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The DataTable.'.
        /// </summary>
        public static string DataTableToText_DataTable_Description => ResourceManager.GetString("DataTableToText_DataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The DataTable to remove duplicated rows.'.
        /// </summary>
        public static string RemoveDuplicateRows_InputDataTable_Description => ResourceManager.GetString("RemoveDuplicateRows_InputDataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The result of the evaluation.'.
        /// </summary>
        public static string IsTrue_Result_Description => ResourceManager.GetString("IsTrue_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Mode'.
        /// </summary>
        public static string RemoveEmptyRows_Mode_DisplayName => ResourceManager.GetString("RemoveEmptyRows_Mode_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns the number of entries (files and folders) from a Zip archive.'.
        /// </summary>
        public static string ZipEntriesCount_Description => ResourceManager.GetString("ZipEntriesCount_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Please provide an output variable for at least one of the available options.'.
        /// </summary>
        public static string ZipEntriesCount_ErrorMsg_OutputMissing => ResourceManager.GetString("ZipEntriesCount_ErrorMsg_OutputMissing", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ProcessName'.
        /// </summary>
        public static string TerminateProcess_ProcessName_DisplayName => ResourceManager.GetString("TerminateProcess_ProcessName_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'An object array containing the result of the aggregation.'.
        /// </summary>
        public static string Aggregate_Result_Description => ResourceManager.GetString("Aggregate_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Please provide a value for {0}.'.
        /// </summary>
        public static string Validation_ValueError => ResourceManager.GetString("Validation_ValueError", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Converts a DataRow to Dictionary.'.
        /// </summary>
        public static string DataRowToDictionary_Description => ResourceManager.GetString("DataRowToDictionary_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Expression'.
        /// </summary>
        public static string CheckPoint_Expression_DisplayName => ResourceManager.GetString("CheckPoint_Expression_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'EntriesCount'.
        /// </summary>
        public static string ZipEntriesCount_EntriesCount_DisplayName => ResourceManager.GetString("ZipEntriesCount_EntriesCount_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Tries to gracefully close all instances of the applications corresponding to the specified processes. If not possible, it kills the process.'.
        /// </summary>
        public static string TerminateProcess_Description => ResourceManager.GetString("TerminateProcess_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Compress files into a zip archive.'.
        /// </summary>
        public static string Zip_Description => ResourceManager.GetString("Zip_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Timer'.
        /// </summary>
        public static string TimeLoop_Timer_DisplayName => ResourceManager.GetString("TimeLoop_Timer_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Variable'.
        /// </summary>
        public static string Increment_Variable_DisplayName => ResourceManager.GetString("Increment_Variable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Check Point'.
        /// </summary>
        public static string CheckPoint_DisplayName => ResourceManager.GetString("CheckPoint_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Deletes only the files with last write time till this reference date. Default is DateTime.Now.'.
        /// </summary>
        public static string CleanUpFolder_LastWriteTime_Description => ResourceManager.GetString("CleanUpFolder_LastWriteTime_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Performs a boolean evaluation of a value or expression.'.
        /// </summary>
        public static string IsTrue_Description => ResourceManager.GetString("IsTrue_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The name of the process to close or kill. Can be either a single name or a list of names.'.
        /// </summary>
        public static string TerminateProcess_ProcessName_Description => ResourceManager.GetString("TerminateProcess_ProcessName_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'LastWriteTime'.
        /// </summary>
        public static string CleanUpFolder_LastWriteTime_DisplayName => ResourceManager.GetString("CleanUpFolder_LastWriteTime_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns only a list of unique values.'.
        /// </summary>
        public static string ExtractDataColumnValues_Unique_Description => ResourceManager.GetString("ExtractDataColumnValues_Unique_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The items to be added to the collection.'.
        /// </summary>
        public static string AddRangeToCollection_Items_Description => ResourceManager.GetString("AddRangeToCollection_Items_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string PromoteHeaders_Result_DisplayName => ResourceManager.GetString("PromoteHeaders_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The path of the file(s) or folder(s) that will be compressed. Can be a string or a collection of strings.'.
        /// </summary>
        public static string Zip_ToCompress_Description => ResourceManager.GetString("Zip_ToCompress_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Replaces an empty column name by the value of this property.'.
        /// </summary>
        public static string PromoteHeaders_EmptyColumnName_Description => ResourceManager.GetString("PromoteHeaders_EmptyColumnName_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The FileInfo object of the respective file when found.'.
        /// </summary>
        public static string WaitDynamicFile_Result_Description => ResourceManager.GetString("WaitDynamicFile_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Inverts the condition of the when test.'.
        /// </summary>
        public static string WhenDo_Inverted_Description => ResourceManager.GetString("WhenDo_Inverted_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns an enumerable collection of full file names that match a search pattern (or collection of patterns) and enumeration options in a specified path (or collection of paths).'.
        /// </summary>
        public static string EnumerateFiles_Description => ResourceManager.GetString("EnumerateFiles_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns the number of file entries in the zip archive.'.
        /// </summary>
        public static string ZipEntriesCount_FilesCount_Description => ResourceManager.GetString("ZipEntriesCount_FilesCount_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The exception to thrown if the expression is not true.'.
        /// </summary>
        public static string CheckPoint_Exception_Description => ResourceManager.GetString("CheckPoint_Exception_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Excludes from the enumeration the files with any of the specified attributes.'.
        /// </summary>
        public static string EnumerateFiles_Exclusions_Description => ResourceManager.GetString("EnumerateFiles_Exclusions_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Cannot be used outside of {0} activity.'.
        /// </summary>
        public static string Validation_ScopeError => ResourceManager.GetString("Validation_ScopeError", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Interval'.
        /// </summary>
        public static string WaitFile_Interval_DisplayName => ResourceManager.GetString("WaitFile_Interval_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Executes the 'Do' block if the condition activity returns true.'.
        /// </summary>
        public static string WhenDo_Description => ResourceManager.GetString("WhenDo_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Specifies the amount of time in milliseconds to wait for the activity to run before an error is thrown. The default value is 30000 (30s).'.
        /// </summary>
        public static string Common_Timeout => ResourceManager.GetString("Common_Timeout", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Column'.
        /// </summary>
        public static string ExtractDataColumnValues_Column_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_Column_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string DataTableToText_Result_DisplayName => ResourceManager.GetString("DataTableToText_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The resulting string from the encrypt or decrypt operation.'.
        /// </summary>
        public static string TextEncryption_Result_Description => ResourceManager.GetString("TextEncryption_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'When true, it avoids the "column name already belongs to DataTable" error by adding a numeric suffix to it.'.
        /// </summary>
        public static string PromoteHeaders_AutoRename_Description => ResourceManager.GetString("PromoteHeaders_AutoRename_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Function'.
        /// </summary>
        public static string Aggregate_Function_DisplayName => ResourceManager.GetString("Aggregate_Function_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'An object containing the number of files deleted, folders deleted and total deleted.'.
        /// </summary>
        public static string CleanUpFolder_Result_Description => ResourceManager.GetString("CleanUpFolder_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column indexes or column names to apply the aggregation. If not specified, the aggregation will be applied in all possible columns.'.
        /// </summary>
        public static string Aggregate_Columns_Description => ResourceManager.GetString("Aggregate_Columns_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Exception'.
        /// </summary>
        public static string CheckPoint_Exception_DisplayName => ResourceManager.GetString("CheckPoint_Exception_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ParallelProcessing'.
        /// </summary>
        public static string DataTableEncryption_ParallelProcessing_DisplayName => ResourceManager.GetString("DataTableEncryption_ParallelProcessing_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string ExtractDataColumnValues_Result_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'TextEncoding'.
        /// </summary>
        public static string DataTableEncryption_TextEncoding_DisplayName => ResourceManager.GetString("DataTableEncryption_TextEncoding_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The maximum number of repetitions to stop the loop in case no exception occur. If not set, its default value is 300.'.
        /// </summary>
        public static string RepeatUntilFailure_MaximumRepetitions_Description => ResourceManager.GetString("RepeatUntilFailure_MaximumRepetitions_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'If set, continue executing the remaining activities even if the current activity has failed.'.
        /// </summary>
        public static string Common_ContinueOnError => ResourceManager.GetString("Common_ContinueOnError", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataRow'.
        /// </summary>
        public static string DataRowToDictionary_InputDataRow_DisplayName => ResourceManager.GetString("DataRowToDictionary_InputDataRow_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Skips the current iteration in an Iterate activity and proceeds the execution with the next iteration.'.
        /// </summary>
        public static string Next_Description => ResourceManager.GetString("Next_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The value to be use in case of the extract value cannot be converted to the specified type.'.
        /// </summary>
        public static string ExtractDataColumnValues_DefaultValue_Description => ResourceManager.GetString("ExtractDataColumnValues_DefaultValue_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Provides methods to encrypt or decrypt a text using a specified algorithm and key.'.
        /// </summary>
        public static string TextEncryption_Description => ResourceManager.GetString("TextEncryption_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string RemoveEmptyRows_InputDataTable_DisplayName => ResourceManager.GetString("RemoveEmptyRows_InputDataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'A container that allows you to run a set of activities over a different culture.'.
        /// </summary>
        public static string CultureScope_Description => ResourceManager.GetString("CultureScope_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Terminate Process'.
        /// </summary>
        public static string TerminateProcess_DisplayName => ResourceManager.GetString("TerminateProcess_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Determines the type of each value extracted from the data column.'.
        /// </summary>
        public static string ExtractDataColumnValues_TypeArgument_Description => ResourceManager.GetString("ExtractDataColumnValues_TypeArgument_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The key to be use. Can be a string or a SecureString.'.
        /// </summary>
        public static string DataTableEncryption_Key_Description => ResourceManager.GetString("DataTableEncryption_Key_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Value'.
        /// </summary>
        public static string IsTrue_Value_DisplayName => ResourceManager.GetString("IsTrue_Value_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Clean Up Folder'.
        /// </summary>
        public static string CleanUpFolder_DisplayName => ResourceManager.GetString("CleanUpFolder_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Remove Data Columns'.
        /// </summary>
        public static string RemoveDataColumns_DisplayName => ResourceManager.GetString("RemoveDataColumns_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Iterations'.
        /// </summary>
        public static string SymmetricAlgorithmEncryptionBase_Iterations_DisplayName => ResourceManager.GetString("SymmetricAlgorithmEncryptionBase_Iterations_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Determines if is to encrypt or decrypt the input value.'.
        /// </summary>
        public static string DataTableEncryption_Action_Description => ResourceManager.GetString("DataTableEncryption_Action_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The encoding used to interpret the data and key.'.
        /// </summary>
        public static string DataTableEncryption_TextEncoding_Description => ResourceManager.GetString("DataTableEncryption_TextEncoding_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The output DataTable.'.
        /// </summary>
        public static string PromoteHeaders_Result_Description => ResourceManager.GetString("PromoteHeaders_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Determines the evaluation condition for the specified columns where (And) checks if all columns are empty and (Or) checks if any of the columns are empty.'.
        /// </summary>
        public static string RemoveEmptyRows_Operator_Description => ResourceManager.GetString("RemoveEmptyRows_Operator_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Data'.
        /// </summary>
        public static string CheckPoint_Data_DisplayName => ResourceManager.GetString("CheckPoint_Data_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Zip Entries Count'.
        /// </summary>
        public static string ZipEntriesCount_DisplayName => ResourceManager.GetString("ZipEntriesCount_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Iterates the activity execution for the specified number of times.'.
        /// </summary>
        public static string Iterate_Description => ResourceManager.GetString("Iterate_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The encoding to use when writing entry names in this archive. Specify a value for this parameter only when an encoding is required for interoperability with zip archive tools and libraries that do not support UTF-8 encoding for entry names.'.
        /// </summary>
        public static string Zip_TextEncoding_Description => ResourceManager.GetString("Zip_TextEncoding_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Provides a set of methods and properties that you can use to accurately measure elapsed time.'.
        /// </summary>
        public static string Stopwatch_Description => ResourceManager.GetString("Stopwatch_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Decrements the value of a variable by the specified decrement value.'.
        /// </summary>
        public static string Decrement_Description => ResourceManager.GetString("Decrement_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'An algorithm is required.'.
        /// </summary>
        public static string EncryptionBase_ErrorMsg_AlgorithmMissing => ResourceManager.GetString("EncryptionBase_ErrorMsg_AlgorithmMissing", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The DataTable to encrypt or decrypt.'.
        /// </summary>
        public static string DataTableEncryption_Input_Description => ResourceManager.GetString("DataTableEncryption_Input_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Removes the duplicate rows from a DataTable keeping only the first occurrence. Allows specify a limited number of columns for this comparison.'.
        /// </summary>
        public static string RemoveDuplicateRows_Description => ResourceManager.GetString("RemoveDuplicateRows_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'AES'.
        /// </summary>
        public static string AesAlgorithmEncryption_DisplayName => ResourceManager.GetString("AesAlgorithmEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Iterations must have a value greater than zero.'.
        /// </summary>
        public static string Iterate_ErrorMsg_IterationsMinValue => ResourceManager.GetString("Iterate_ErrorMsg_IterationsMinValue", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The data table which the aggregate function will be applied.'.
        /// </summary>
        public static string Aggregate_InputDataTable_Description => ResourceManager.GetString("Aggregate_InputDataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Is True'.
        /// </summary>
        public static string IsTrue_DisplayName => ResourceManager.GetString("IsTrue_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable To Text'.
        /// </summary>
        public static string DataTableToText_DisplayName => ResourceManager.GetString("DataTableToText_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column names or column indexes to be analyzed on duplicate rows comparison. Can be either a collection of string or int.'.
        /// </summary>
        public static string RemoveDuplicateRows_Columns_Description => ResourceManager.GetString("RemoveDuplicateRows_Columns_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The content after the tokens replacement be performed.'.
        /// </summary>
        public static string ReplaceTokens_Result_Description => ResourceManager.GetString("ReplaceTokens_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DateTimeFormat'.
        /// </summary>
        public static string DataTableToText_DateTimeFormat_DisplayName => ResourceManager.GetString("DataTableToText_DateTimeFormat_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string DataRowToDictionary_Result_DisplayName => ResourceManager.GetString("DataRowToDictionary_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Stopwatch'.
        /// </summary>
        public static string Stopwatch_ReferenceStopwatch_DisplayName => ResourceManager.GetString("Stopwatch_ReferenceStopwatch_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The maximum timeout to wait in milliseconds.'.
        /// </summary>
        public static string WaitFile_Timeout_Description => ResourceManager.GetString("WaitFile_Timeout_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Aggregates the DataTable data by the specified function resulting in a DataRow, product of this aggregation.'.
        /// </summary>
        public static string Aggregate_Description => ResourceManager.GetString("Aggregate_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string WaitFile_Result_DisplayName => ResourceManager.GetString("WaitFile_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string RemoveDuplicateRows_InputDataTable_DisplayName => ResourceManager.GetString("RemoveDuplicateRows_InputDataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Repeats an action for a specified period of time.'.
        /// </summary>
        public static string TimeLoop_Description => ResourceManager.GetString("TimeLoop_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'RC2'.
        /// </summary>
        public static string RC2AlgorithmEncryption_DisplayName => ResourceManager.GetString("RC2AlgorithmEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Sort'.
        /// </summary>
        public static string DataTableEncryption_Sort_DisplayName => ResourceManager.GetString("DataTableEncryption_Sort_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column name or index where the values will be extracted from.'.
        /// </summary>
        public static string ExtractDataColumnValues_Column_Description => ResourceManager.GetString("ExtractDataColumnValues_Column_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Key'.
        /// </summary>
        public static string TextEncryption_Key_DisplayName => ResourceManager.GetString("TextEncryption_Key_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Overwrite'.
        /// </summary>
        public static string Unzip_Overwrite_DisplayName => ResourceManager.GetString("Unzip_Overwrite_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns an array with all values of a respective data column.'.
        /// </summary>
        public static string ExtractDataColumnValues_Description => ResourceManager.GetString("ExtractDataColumnValues_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataRow To Dictionary'.
        /// </summary>
        public static string DataRowToDictionary_DisplayName => ResourceManager.GetString("DataRowToDictionary_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string TransposeData_InputDataTable_DisplayName => ResourceManager.GetString("TransposeData_InputDataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The amount of time to wait on each loop iteration.'.
        /// </summary>
        public static string TimeLoop_LoopInterval_Description => ResourceManager.GetString("TimeLoop_LoopInterval_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Checks if a given expression is true, if not, thrown the specified exception.'.
        /// </summary>
        public static string CheckPoint_Description => ResourceManager.GetString("CheckPoint_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Enumerate Files'.
        /// </summary>
        public static string EnumerateFiles_DisplayName => ResourceManager.GetString("EnumerateFiles_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The output Dictionary.'.
        /// </summary>
        public static string DataRowToDictionary_Result_Description => ResourceManager.GetString("DataRowToDictionary_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Toggles the 'Else' block activating or deactivating it. '.
        /// </summary>
        public static string WhenDo_WithElse_Description => ResourceManager.GetString("WhenDo_WithElse_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'IterationNumber'.
        /// </summary>
        public static string RepeatUntilFailure_IterationNumber_DisplayName => ResourceManager.GetString("RepeatUntilFailure_IterationNumber_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Removes the specified columns from a DataTable'.
        /// </summary>
        public static string RemoveDataColumns_Description => ResourceManager.GetString("RemoveDataColumns_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Waits until the file exists.'.
        /// </summary>
        public static string WaitFile_WaitForExist_Description => ResourceManager.GetString("WaitFile_WaitForExist_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Reference'.
        /// </summary>
        public static string InputOutput_Category => ResourceManager.GetString("InputOutput_Category", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'When true, overwrites an existing file that has the same name as the destination file.'.
        /// </summary>
        public static string Unzip_Overwrite_Description => ResourceManager.GetString("Unzip_Overwrite_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The string representation of the DataTable.'.
        /// </summary>
        public static string DataTableToText_Result_Description => ResourceManager.GetString("DataTableToText_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'TextEncoding'.
        /// </summary>
        public static string Zip_TextEncoding_DisplayName => ResourceManager.GetString("Zip_TextEncoding_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The exception which caused the loop break. This result can be null in case of no exceptions did occur.'.
        /// </summary>
        public static string RepeatUntilFailure_OutputException_Description => ResourceManager.GetString("RepeatUntilFailure_OutputException_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Common'.
        /// </summary>
        public static string Common_Category => ResourceManager.GetString("Common_Category", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Pattern'.
        /// </summary>
        public static string ReplaceTokens_Pattern_DisplayName => ResourceManager.GetString("ReplaceTokens_Pattern_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The path of the zip archive.'.
        /// </summary>
        public static string Zip_ZipFilePath_Description => ResourceManager.GetString("Zip_ZipFilePath_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Promote Headers'.
        /// </summary>
        public static string PromoteHeaders_DisplayName => ResourceManager.GetString("PromoteHeaders_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'TextFormat'.
        /// </summary>
        public static string DataTableToText_TextFormat_DisplayName => ResourceManager.GetString("DataTableToText_TextFormat_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns a string representation of a datatable on the specified text format (HTML, JSON or XML).'.
        /// </summary>
        public static string DataTableToText_Description => ResourceManager.GetString("DataTableToText_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The decrement value cannot be smaller than 1.'.
        /// </summary>
        public static string Decrement_ErrorMsg_MinValue => ResourceManager.GetString("Decrement_ErrorMsg_MinValue", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Dictionary To DataTable'.
        /// </summary>
        public static string DictionaryToDataTable_DisplayName => ResourceManager.GetString("DictionaryToDataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Uses parallel processing to scan through the DataTable and decrypt the data. Although this can drastically increase the processing speed, the rows in the resulting DataTable may have its order changed. Combine it with Sort property to reorder the DataTable when necessary.'.
        /// </summary>
        public static string DataTableEncryption_ParallelProcessing_Description => ResourceManager.GetString("DataTableEncryption_ParallelProcessing_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The output DataTable with the distinct rows.'.
        /// </summary>
        public static string RemoveDuplicateRows_Result_Description => ResourceManager.GetString("RemoveDuplicateRows_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Deletes all files and folders from a specified folder.'.
        /// </summary>
        public static string CleanUpFolder_Description => ResourceManager.GetString("CleanUpFolder_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The increment value cannot be smaller than 1.'.
        /// </summary>
        public static string Increment_ErrorMsg_MinValue => ResourceManager.GetString("Increment_ErrorMsg_MinValue", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Replaces the tokens of a string by the values of a Dictionary. Tokens are strings written in a specific pattern, usually enclosed in special characters. Use the combination of 'Pattern' and 'Placeholder' properties to define your token format.'.
        /// </summary>
        public static string ReplaceTokens_Description => ResourceManager.GetString("ReplaceTokens_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The input DataTable.'.
        /// </summary>
        public static string PromoteHeaders_InputDataTable_Description => ResourceManager.GetString("PromoteHeaders_InputDataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Allows to transform the text casing during the extraction. Applied only if the TypeArgument is set to System.String.'.
        /// </summary>
        public static string ExtractDataColumnValues_TextCase_Description => ResourceManager.GetString("ExtractDataColumnValues_TextCase_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column names or column indexes to be removed. Can be either a collection of string or int.'.
        /// </summary>
        public static string RemoveDataColumns_Columns_Description => ResourceManager.GetString("RemoveDataColumns_Columns_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The current iteration (one-based) that is being processed.'.
        /// </summary>
        public static string TimeLoop_IterationNumber_Description => ResourceManager.GetString("TimeLoop_IterationNumber_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Condition which determines if the activity will be evaluated. If empty it assumes True.'.
        /// </summary>
        public static string Exit_Condition_Description => ResourceManager.GetString("Exit_Condition_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The number of iterations for the operation.'.
        /// </summary>
        public static string SymmetricAlgorithmEncryptionBase_Iterations_Description => ResourceManager.GetString("SymmetricAlgorithmEncryptionBase_Iterations_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Output'.
        /// </summary>
        public static string Output_Category => ResourceManager.GetString("Output_Category", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Sets the sort column or columns, and sort order for the output DataTable. The value must be a string that contains the column name followed by "ASC" (ascending) or "DESC" (descending). Columns are sorted ascending by default. Multiple columns can be separated by commas.'.
        /// </summary>
        public static string DataTableEncryption_Sort_Description => ResourceManager.GetString("DataTableEncryption_Sort_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The input DataTable to be transposed.'.
        /// </summary>
        public static string TransposeData_InputDataTable_Description => ResourceManager.GetString("TransposeData_InputDataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Its a wrapper that when combined with Exit Activity, interrupts the children execution flow exiting the Container beforehand.'.
        /// </summary>
        public static string Container_Description => ResourceManager.GetString("Container_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Remove Empty Rows'.
        /// </summary>
        public static string RemoveEmptyRows_DisplayName => ResourceManager.GetString("RemoveEmptyRows_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Transpose Data'.
        /// </summary>
        public static string TransposeData_DisplayName => ResourceManager.GetString("TransposeData_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The input DataRow.'.
        /// </summary>
        public static string DataRowToDictionary_InputDataRow_Description => ResourceManager.GetString("DataRowToDictionary_InputDataRow_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'When true, reduces the entry names when it is coming from different root paths.'.
        /// </summary>
        public static string Zip_ShortEntryNames_Description => ResourceManager.GetString("Zip_ShortEntryNames_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Value'.
        /// </summary>
        public static string Increment_Value_DisplayName => ResourceManager.GetString("Increment_Value_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Exclusions'.
        /// </summary>
        public static string EnumerateFiles_Exclusions_DisplayName => ResourceManager.GetString("EnumerateFiles_Exclusions_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ShortEntryNames'.
        /// </summary>
        public static string Zip_ShortEntryNames_DisplayName => ResourceManager.GetString("Zip_ShortEntryNames_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DeleteEmptyFolders'.
        /// </summary>
        public static string CleanUpFolder_DeleteEmptyFolders_DisplayName => ResourceManager.GetString("CleanUpFolder_DeleteEmptyFolders_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DirectoryPath'.
        /// </summary>
        public static string EnumerateFiles_DirectoryPath_DisplayName => ResourceManager.GetString("EnumerateFiles_DirectoryPath_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string IsTrue_Result_DisplayName => ResourceManager.GetString("IsTrue_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The desired culture, eg.: en-US, pt-BR, jp-JP, etc.'.
        /// </summary>
        public static string CultureScope_CultureName_Description => ResourceManager.GetString("CultureScope_CultureName_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Remove Duplicate Rows'.
        /// </summary>
        public static string RemoveDuplicateRows_DisplayName => ResourceManager.GetString("RemoveDuplicateRows_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'TextEncoding'.
        /// </summary>
        public static string TextEncryption_TextEncoding_DisplayName => ResourceManager.GetString("TextEncryption_TextEncoding_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The specified file path does not exists.'.
        /// </summary>
        public static string WaitFile_ErrorMsg_FilePathDoesNotExists => ResourceManager.GetString("WaitFile_ErrorMsg_FilePathDoesNotExists", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Determines which method must be invoked by activity.'.
        /// </summary>
        public static string Stopwatch_Method_Description => ResourceManager.GetString("Stopwatch_Method_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns the amount of files added to zip archive.'.
        /// </summary>
        public static string Zip_FilesCount_Description => ResourceManager.GetString("Zip_FilesCount_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Columns'.
        /// </summary>
        public static string RemoveDuplicateRows_Columns_DisplayName => ResourceManager.GetString("RemoveDuplicateRows_Columns_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The key to be use. Can be a string or a SecureString.'.
        /// </summary>
        public static string TextEncryption_Key_Description => ResourceManager.GetString("TextEncryption_Key_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable Encryption'.
        /// </summary>
        public static string DataTableEncryption_DisplayName => ResourceManager.GetString("DataTableEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The dictionary where each key/value pair are used to replace the tokens.'.
        /// </summary>
        public static string ReplaceTokens_InputDictionary_Description => ResourceManager.GetString("ReplaceTokens_InputDictionary_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Collection'.
        /// </summary>
        public static string AddRangeToCollection_Collection_DisplayName => ResourceManager.GetString("AddRangeToCollection_Collection_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string RemoveDuplicateRows_Result_DisplayName => ResourceManager.GetString("RemoveDuplicateRows_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Cannot be used outside of {0} activities.'.
        /// </summary>
        public static string Validation_ScopesError => ResourceManager.GetString("Validation_ScopesError", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The FileInfo object of the respective file when found.'.
        /// </summary>
        public static string WaitFile_Result_Description => ResourceManager.GetString("WaitFile_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Adds a set of items to the specified collection.'.
        /// </summary>
        public static string AddRangeToCollection_Description => ResourceManager.GetString("AddRangeToCollection_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Removes null values, empty strings, or those which consists only of white-space characters.'.
        /// </summary>
        public static string ExtractDataColumnValues_Sanitize_Description => ResourceManager.GetString("ExtractDataColumnValues_Sanitize_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Input'.
        /// </summary>
        public static string DataTableEncryption_Input_DisplayName => ResourceManager.GetString("DataTableEncryption_Input_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Reverses the order of the output index.'.
        /// </summary>
        public static string Iterate_Reverse_Description => ResourceManager.GetString("Iterate_Reverse_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Interval'.
        /// </summary>
        public static string WaitDynamicFile_Interval_DisplayName => ResourceManager.GetString("WaitDynamicFile_Interval_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Provides methods to encrypt or decrypt the values of a DataTable using a specified algorithm and key.'.
        /// </summary>
        public static string DataTableEncryption_Description => ResourceManager.GetString("DataTableEncryption_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'FolderPath'.
        /// </summary>
        public static string WaitDynamicFile_DirectoryPath_DisplayName => ResourceManager.GetString("WaitDynamicFile_DirectoryPath_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Dictionary'.
        /// </summary>
        public static string DictionaryToDataTable_InputDictionary_DisplayName => ResourceManager.GetString("DictionaryToDataTable_InputDictionary_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Value'.
        /// </summary>
        public static string Decrement_Value_DisplayName => ResourceManager.GetString("Decrement_Value_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Unique'.
        /// </summary>
        public static string ExtractDataColumnValues_Unique_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_Unique_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Specifies the amount of time (in milliseconds) for the file re-check. Any values out of the range of 100-20000 milliseconds is reseted to its nearest limit. The default value is 500.'.
        /// </summary>
        public static string WaitDynamicFile_Interval_Description => ResourceManager.GetString("WaitDynamicFile_Interval_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'One of the enumeration values that indicates whether to emphasize speed or compression.'.
        /// </summary>
        public static string Zip_CompressionLevel_Description => ResourceManager.GetString("Zip_CompressionLevel_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'FromDateTime'.
        /// </summary>
        public static string WaitDynamicFile_FromDateTime_DisplayName => ResourceManager.GetString("WaitDynamicFile_FromDateTime_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Timeout'.
        /// </summary>
        public static string WaitFile_Timeout_DisplayName => ResourceManager.GetString("WaitFile_Timeout_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The Items property value cannot be null.'.
        /// </summary>
        public static string AddRangeToCollection_ErrorMsg_ItemsNull => ResourceManager.GetString("AddRangeToCollection_ErrorMsg_ItemsNull", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The output DataTable.'.
        /// </summary>
        public static string DictionaryToDataTable_Result_Description => ResourceManager.GetString("DictionaryToDataTable_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Replace Tokens'.
        /// </summary>
        public static string ReplaceTokens_DisplayName => ResourceManager.GetString("ReplaceTokens_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions. It also supports a collection of strings. Default value is "*.*".'.
        /// </summary>
        public static string WaitDynamicFile_SearchPattern_Description => ResourceManager.GetString("WaitDynamicFile_SearchPattern_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Determines if the left empty folders after files deletion must also be deleted.'.
        /// </summary>
        public static string CleanUpFolder_DeleteEmptyFolders_Description => ResourceManager.GetString("CleanUpFolder_DeleteEmptyFolders_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'LoopInterval'.
        /// </summary>
        public static string RepeatUntilFailure_LoopInterval_DisplayName => ResourceManager.GetString("RepeatUntilFailure_LoopInterval_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Time Loop'.
        /// </summary>
        public static string TimeLoop_DisplayName => ResourceManager.GetString("TimeLoop_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Inverted'.
        /// </summary>
        public static string WhenDo_Inverted_DisplayName => ResourceManager.GetString("WhenDo_Inverted_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'When Do'.
        /// </summary>
        public static string WhenDo_DisplayName => ResourceManager.GetString("WhenDo_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Extracting Zip entry would have resulted in a file outside the specified destination directory.'.
        /// </summary>
        public static string Unzip_ErrorMsg_OutsideDir => ResourceManager.GetString("Unzip_ErrorMsg_OutsideDir", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'FilePath'.
        /// </summary>
        public static string WaitFile_FilePath_DisplayName => ResourceManager.GetString("WaitFile_FilePath_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Columns'.
        /// </summary>
        public static string RemoveDataColumns_Columns_DisplayName => ResourceManager.GetString("RemoveDataColumns_Columns_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column indexes or column names to be decrypted.'.
        /// </summary>
        public static string DataTableEncryption_Columns_Description => ResourceManager.GetString("DataTableEncryption_Columns_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The DataTable object from which the columns is to be removed.'.
        /// </summary>
        public static string RemoveDataColumns_ReferenceDataTable_Description => ResourceManager.GetString("RemoveDataColumns_ReferenceDataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The output DataTable.'.
        /// </summary>
        public static string RemoveEmptyRows_Result_Description => ResourceManager.GetString("RemoveEmptyRows_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'SearchPattern'.
        /// </summary>
        public static string CleanUpFolder_SearchPattern_DisplayName => ResourceManager.GetString("CleanUpFolder_SearchPattern_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'MaximumRepetitions'.
        /// </summary>
        public static string RepeatUntilFailure_MaximumRepetitions_DisplayName => ResourceManager.GetString("RepeatUntilFailure_MaximumRepetitions_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The encrypted or decrypted DataTable.'.
        /// </summary>
        public static string DataTableEncryption_Result_Description => ResourceManager.GetString("DataTableEncryption_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Rijndael'.
        /// </summary>
        public static string RijndaelAlgorithmEncryption_DisplayName => ResourceManager.GetString("RijndaelAlgorithmEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Determines when a row should be removed. (All) means that all columns must be empty to remove the row. (Any) for any column empty and (Custom) for the user defined rules.'.
        /// </summary>
        public static string RemoveEmptyRows_Mode_Description => ResourceManager.GetString("RemoveEmptyRows_Mode_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The output DataTable resulting of the transpose.'.
        /// </summary>
        public static string TransposeData_Result_Description => ResourceManager.GetString("TransposeData_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'EmptyColumnName'.
        /// </summary>
        public static string PromoteHeaders_EmptyColumnName_DisplayName => ResourceManager.GetString("PromoteHeaders_EmptyColumnName_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions. It also supports a collection of strings. Default value is "*.*".'.
        /// </summary>
        public static string CleanUpFolder_SearchPattern_Description => ResourceManager.GetString("CleanUpFolder_SearchPattern_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The relative or absolute path (or collection of paths) to the directory (or directories) to search.'.
        /// </summary>
        public static string EnumerateFiles_DirectoryPath_Description => ResourceManager.GetString("EnumerateFiles_DirectoryPath_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Zip entry name ends in directory separator character but contains data.'.
        /// </summary>
        public static string Unzip_ErrorMsg_DirNameWithData => ResourceManager.GetString("Unzip_ErrorMsg_DirNameWithData", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The accepted range is from {0} and {1}. The value will be clamped.'.
        /// </summary>
        public static string WaitDynamicFile_ErrorMsg_IntervalRange => ResourceManager.GetString("WaitDynamicFile_ErrorMsg_IntervalRange", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'An enumerable collection of the full names (including paths) for the files in the directory specified by path and that match the specified search pattern and option.'.
        /// </summary>
        public static string EnumerateFiles_Result_Description => ResourceManager.GetString("EnumerateFiles_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string DataTableToText_DataTable_DisplayName => ResourceManager.GetString("DataTableToText_DataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DefaultValue'.
        /// </summary>
        public static string ExtractDataColumnValues_DefaultValue_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_DefaultValue_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The input DataTable.'.
        /// </summary>
        public static string RemoveEmptyRows_InputDataTable_Description => ResourceManager.GetString("RemoveEmptyRows_InputDataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'AutoRename'.
        /// </summary>
        public static string PromoteHeaders_AutoRename_DisplayName => ResourceManager.GetString("PromoteHeaders_AutoRename_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'CultureName'.
        /// </summary>
        public static string CultureScope_CultureName_DisplayName => ResourceManager.GetString("CultureScope_CultureName_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The text which contains the tokens to be replaced.'.
        /// </summary>
        public static string ReplaceTokens_Content_Description => ResourceManager.GetString("ReplaceTokens_Content_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The collection you want to modify.'.
        /// </summary>
        public static string AddRangeToCollection_Collection_Description => ResourceManager.GetString("AddRangeToCollection_Collection_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Specifies the amount of time (in milliseconds) for the file re-check. Any values out of the range of 100-20000 milliseconds is reseted to its nearest limit. The default value is 500.'.
        /// </summary>
        public static string WaitFile_Interval_Description => ResourceManager.GetString("WaitFile_Interval_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The path of the zip archive.'.
        /// </summary>
        public static string ZipEntriesCount_ZipFilePath_Description => ResourceManager.GetString("ZipEntriesCount_ZipFilePath_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string Aggregate_InputDataTable_DisplayName => ResourceManager.GetString("Aggregate_InputDataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ExtractTo'.
        /// </summary>
        public static string Unzip_ExtractTo_DisplayName => ResourceManager.GetString("Unzip_ExtractTo_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The decrement value.'.
        /// </summary>
        public static string Decrement_Value_Description => ResourceManager.GetString("Decrement_Value_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'WaitForExist'.
        /// </summary>
        public static string WaitFile_WaitForExist_DisplayName => ResourceManager.GetString("WaitFile_WaitForExist_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The text to be encrypted or decrypted.'.
        /// </summary>
        public static string TextEncryption_Input_Description => ResourceManager.GetString("TextEncryption_Input_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Condition'.
        /// </summary>
        public static string Next_Condition_DisplayName => ResourceManager.GetString("Next_Condition_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The amount of time between each loop iteration.'.
        /// </summary>
        public static string RepeatUntilFailure_LoopInterval_Description => ResourceManager.GetString("RepeatUntilFailure_LoopInterval_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Removes the empty rows from a DataTable.'.
        /// </summary>
        public static string RemoveEmptyRows_Description => ResourceManager.GetString("RemoveEmptyRows_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ExitOnException'.
        /// </summary>
        public static string TimeLoop_ExitOnException_DisplayName => ResourceManager.GetString("TimeLoop_ExitOnException_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Columns'.
        /// </summary>
        public static string DataTableEncryption_Columns_DisplayName => ResourceManager.GetString("DataTableEncryption_Columns_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Reverse'.
        /// </summary>
        public static string Iterate_Reverse_DisplayName => ResourceManager.GetString("Iterate_Reverse_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The path to the folder where files will be extracted.'.
        /// </summary>
        public static string Unzip_ExtractTo_Description => ResourceManager.GetString("Unzip_ExtractTo_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Sanitize'.
        /// </summary>
        public static string ExtractDataColumnValues_Sanitize_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_Sanitize_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ZipFilePath'.
        /// </summary>
        public static string Zip_ZipFilePath_DisplayName => ResourceManager.GetString("Zip_ZipFilePath_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The date format to be used on the string representation for DateTime column types.'.
        /// </summary>
        public static string DataTableToText_DateTimeFormat_Description => ResourceManager.GetString("DataTableToText_DateTimeFormat_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'OutputException'.
        /// </summary>
        public static string RepeatUntilFailure_OutputException_DisplayName => ResourceManager.GetString("RepeatUntilFailure_OutputException_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'A collection of key/value pairs that provide additional user-defined information about the exception.'.
        /// </summary>
        public static string CheckPoint_Data_Description => ResourceManager.GetString("CheckPoint_Data_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The maximum timeout to wait in milliseconds.'.
        /// </summary>
        public static string WaitDynamicFile_Timeout_Description => ResourceManager.GetString("WaitDynamicFile_Timeout_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ZipFilePath'.
        /// </summary>
        public static string Unzip_ZipFilePath_DisplayName => ResourceManager.GetString("Unzip_ZipFilePath_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Text Encryption'.
        /// </summary>
        public static string TextEncryption_DisplayName => ResourceManager.GetString("TextEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The variable to be incremented.'.
        /// </summary>
        public static string Increment_Variable_Description => ResourceManager.GetString("Increment_Variable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Please provide a value of type {0} for {1}.'.
        /// </summary>
        public static string Validation_TypeError => ResourceManager.GetString("Validation_TypeError", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string PromoteHeaders_InputDataTable_DisplayName => ResourceManager.GetString("PromoteHeaders_InputDataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string EnumerateFiles_Result_DisplayName => ResourceManager.GetString("EnumerateFiles_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string DictionaryToDataTable_Result_DisplayName => ResourceManager.GetString("DictionaryToDataTable_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Transposes a DataTable by swapping its rows and columns so that rows become columns and columns become rows.'.
        /// </summary>
        public static string TransposeData_Description => ResourceManager.GetString("TransposeData_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The compressed file.'.
        /// </summary>
        public static string Unzip_ZipFilePath_Description => ResourceManager.GetString("Unzip_ZipFilePath_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Wait Dynamic File'.
        /// </summary>
        public static string WaitDynamicFile_DisplayName => ResourceManager.GetString("WaitDynamicFile_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Key'.
        /// </summary>
        public static string DataTableEncryption_Key_DisplayName => ResourceManager.GetString("DataTableEncryption_Key_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Iterations'.
        /// </summary>
        public static string Iterate_Iterations_DisplayName => ResourceManager.GetString("Iterate_Iterations_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Waits until an unknown file be available by monitoring a specified folder.'.
        /// </summary>
        public static string WaitDynamicFile_Description => ResourceManager.GetString("WaitDynamicFile_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string CleanUpFolder_Result_DisplayName => ResourceManager.GetString("CleanUpFolder_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The Timer determines when the loop must be stopped. Its value is checked after each iteration. The current iteration of the loop is not affected by the Timer.'.
        /// </summary>
        public static string TimeLoop_Timer_Description => ResourceManager.GetString("TimeLoop_Timer_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string DataTableEncryption_Result_DisplayName => ResourceManager.GetString("DataTableEncryption_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Wait File'.
        /// </summary>
        public static string WaitFile_DisplayName => ResourceManager.GetString("WaitFile_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column index '{0}' does not exist on the DataTable.'.
        /// </summary>
        public static string ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex => ResourceManager.GetString("ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'CompressionLevel'.
        /// </summary>
        public static string Zip_CompressionLevel_DisplayName => ResourceManager.GetString("Zip_CompressionLevel_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string RemoveDataColumns_ReferenceDataTable_DisplayName => ResourceManager.GetString("RemoveDataColumns_ReferenceDataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'SearchPattern'.
        /// </summary>
        public static string WaitDynamicFile_SearchPattern_DisplayName => ResourceManager.GetString("WaitDynamicFile_SearchPattern_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string Aggregate_Result_DisplayName => ResourceManager.GetString("Aggregate_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Decompress files from a zip archive.'.
        /// </summary>
        public static string Unzip_Description => ResourceManager.GetString("Unzip_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The number of entries (files and folders) in the zip archive.'.
        /// </summary>
        public static string ZipEntriesCount_EntriesCount_Description => ResourceManager.GetString("ZipEntriesCount_EntriesCount_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Options'.
        /// </summary>
        public static string Options_Category => ResourceManager.GetString("Options_Category", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Content'.
        /// </summary>
        public static string ReplaceTokens_Content_DisplayName => ResourceManager.GetString("ReplaceTokens_Content_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Repeat Until Failure'.
        /// </summary>
        public static string RepeatUntilFailure_DisplayName => ResourceManager.GetString("RepeatUntilFailure_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'AES GCM'.
        /// </summary>
        public static string AesGcmAlgorithmEncryption_DisplayName => ResourceManager.GetString("AesGcmAlgorithmEncryption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Input'.
        /// </summary>
        public static string Input_Category => ResourceManager.GetString("Input_Category", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string WaitDynamicFile_Result_DisplayName => ResourceManager.GetString("WaitDynamicFile_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Exits form the loop in case of an exception occur.'.
        /// </summary>
        public static string TimeLoop_ExitOnException_Description => ResourceManager.GetString("TimeLoop_ExitOnException_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Method'.
        /// </summary>
        public static string Stopwatch_Method_DisplayName => ResourceManager.GetString("Stopwatch_Method_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Extract Data Column Values'.
        /// </summary>
        public static string ExtractDataColumnValues_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Action'.
        /// </summary>
        public static string DataTableEncryption_Action_DisplayName => ResourceManager.GetString("DataTableEncryption_Action_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string ReplaceTokens_Result_DisplayName => ResourceManager.GetString("ReplaceTokens_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Action'.
        /// </summary>
        public static string TextEncryption_Action_DisplayName => ResourceManager.GetString("TextEncryption_Action_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Dictionary'.
        /// </summary>
        public static string ReplaceTokens_InputDictionary_DisplayName => ResourceManager.GetString("ReplaceTokens_InputDictionary_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The directory to be monitored.'.
        /// </summary>
        public static string WaitDynamicFile_DirectoryPath_Description => ResourceManager.GetString("WaitDynamicFile_DirectoryPath_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'IterationNumber'.
        /// </summary>
        public static string TimeLoop_IterationNumber_DisplayName => ResourceManager.GetString("TimeLoop_IterationNumber_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Columns'.
        /// </summary>
        public static string RemoveEmptyRows_Columns_DisplayName => ResourceManager.GetString("RemoveEmptyRows_Columns_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Operator'.
        /// </summary>
        public static string RemoveEmptyRows_Operator_DisplayName => ResourceManager.GetString("RemoveEmptyRows_Operator_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Returns the number of folder entries in the zip archive.'.
        /// </summary>
        public static string ZipEntriesCount_FoldersCount_Description => ResourceManager.GetString("ZipEntriesCount_FoldersCount_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The placeholder where each dictionary key will found.'.
        /// </summary>
        public static string ReplaceTokens_Placeholder_Description => ResourceManager.GetString("ReplaceTokens_Placeholder_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Items'.
        /// </summary>
        public static string AddRangeToCollection_Items_DisplayName => ResourceManager.GetString("AddRangeToCollection_Items_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'SearchOption'.
        /// </summary>
        public static string EnumerateFiles_SearchOption_DisplayName => ResourceManager.GetString("EnumerateFiles_SearchOption_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Promotes the first row of values to new column headers.'.
        /// </summary>
        public static string PromoteHeaders_Description => ResourceManager.GetString("PromoteHeaders_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Converts a Dictionary to DataTable.'.
        /// </summary>
        public static string DictionaryToDataTable_Description => ResourceManager.GetString("DictionaryToDataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The data table where the values will be extracted from.'.
        /// </summary>
        public static string ExtractDataColumnValues_DataTable_Description => ResourceManager.GetString("ExtractDataColumnValues_DataTable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Input'.
        /// </summary>
        public static string TextEncryption_Input_DisplayName => ResourceManager.GetString("TextEncryption_Input_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Placeholder'.
        /// </summary>
        public static string ReplaceTokens_Placeholder_DisplayName => ResourceManager.GetString("ReplaceTokens_Placeholder_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'TypeArgument'.
        /// </summary>
        public static string ExtractDataColumnValues_TypeArgument_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_TypeArgument_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The folder path to be cleaned up.'.
        /// </summary>
        public static string CleanUpFolder_Folder_Description => ResourceManager.GetString("CleanUpFolder_Folder_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The variable to decrement.'.
        /// </summary>
        public static string Decrement_Variable_Description => ResourceManager.GetString("Decrement_Variable_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column indexes or column names to verify.'.
        /// </summary>
        public static string RemoveEmptyRows_Columns_Description => ResourceManager.GetString("RemoveEmptyRows_Columns_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The increment value.'.
        /// </summary>
        public static string Increment_Value_Description => ResourceManager.GetString("Increment_Value_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The text format.'.
        /// </summary>
        public static string DataTableToText_TextFormat_Description => ResourceManager.GetString("DataTableToText_TextFormat_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Removes all leading and trailing occurrences of a set of specified characters from each extracted value. Applied only if the TypeArgument is set to System.String.'.
        /// </summary>
        public static string ExtractDataColumnValues_Trim_Description => ResourceManager.GetString("ExtractDataColumnValues_Trim_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The number of iterations that must be run.'.
        /// </summary>
        public static string Iterate_Iterations_Description => ResourceManager.GetString("Iterate_Iterations_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Considers only the files whose last write time is greater than this value. If is not set, the value used is the same as the last write time of the most recent file in the folder. If the folder is empty, the current date and time is used.'.
        /// </summary>
        public static string WaitDynamicFile_FromDateTime_Description => ResourceManager.GetString("WaitDynamicFile_FromDateTime_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string RemoveEmptyRows_Result_DisplayName => ResourceManager.GetString("RemoveEmptyRows_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Result'.
        /// </summary>
        public static string TransposeData_Result_DisplayName => ResourceManager.GetString("TransposeData_Result_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'FilesCount'.
        /// </summary>
        public static string ZipEntriesCount_FilesCount_DisplayName => ResourceManager.GetString("ZipEntriesCount_FilesCount_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Repeats an action until an exception occur or the maximum number of repetitions is reached.'.
        /// </summary>
        public static string RepeatUntilFailure_Description => ResourceManager.GetString("RepeatUntilFailure_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The exception which caused the loop break. This result can be null in case of no exceptions did occur.'.
        /// </summary>
        public static string TimeLoop_OutputException_Description => ResourceManager.GetString("TimeLoop_OutputException_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The aggregate function.'.
        /// </summary>
        public static string Aggregate_Function_Description => ResourceManager.GetString("Aggregate_Function_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Trim'.
        /// </summary>
        public static string ExtractDataColumnValues_Trim_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_Trim_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Exits from the Container or Iterate Activities interrupting any child executions after it.'.
        /// </summary>
        public static string Exit_Description => ResourceManager.GetString("Exit_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string ExtractDataColumnValues_DataTable_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_DataTable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The accepted range is from {0} and {1}. The value will be clamped.'.
        /// </summary>
        public static string WaitFile_ErrorMsg_IntervalRange => ResourceManager.GetString("WaitFile_ErrorMsg_IntervalRange", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'FilesCount'.
        /// </summary>
        public static string Zip_FilesCount_DisplayName => ResourceManager.GetString("Zip_FilesCount_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'WithElse'.
        /// </summary>
        public static string WhenDo_WithElse_DisplayName => ResourceManager.GetString("WhenDo_WithElse_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'FolderPath'.
        /// </summary>
        public static string CleanUpFolder_Folder_DisplayName => ResourceManager.GetString("CleanUpFolder_Folder_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'ZipFilePath'.
        /// </summary>
        public static string ZipEntriesCount_ZipFilePath_DisplayName => ResourceManager.GetString("ZipEntriesCount_ZipFilePath_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The token text pattern. It can have characters either before and after the placeholder.'.
        /// </summary>
        public static string ReplaceTokens_Pattern_Description => ResourceManager.GetString("ReplaceTokens_Pattern_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'CustomOptions'.
        /// </summary>
        public static string RemoveEmptyRows_CustomOptions_DisplayName => ResourceManager.GetString("RemoveEmptyRows_CustomOptions_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'FoldersCount'.
        /// </summary>
        public static string ZipEntriesCount_FoldersCount_DisplayName => ResourceManager.GetString("ZipEntriesCount_FoldersCount_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'LoopInterval'.
        /// </summary>
        public static string TimeLoop_LoopInterval_DisplayName => ResourceManager.GetString("TimeLoop_LoopInterval_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Culture Scope'.
        /// </summary>
        public static string CultureScope_DisplayName => ResourceManager.GetString("CultureScope_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'TextCase'.
        /// </summary>
        public static string ExtractDataColumnValues_TextCase_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_TextCase_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The value or expression to be evaluated.'.
        /// </summary>
        public static string IsTrue_Value_Description => ResourceManager.GetString("IsTrue_Value_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The current iteration that was being processed. You can use it to determine in which iteration the process has broken.'.
        /// </summary>
        public static string RepeatUntilFailure_IterationNumber_Description => ResourceManager.GetString("RepeatUntilFailure_IterationNumber_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The array of values extracted from the data column.'.
        /// </summary>
        public static string ExtractDataColumnValues_Result_Description => ResourceManager.GetString("ExtractDataColumnValues_Result_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The Collection property value cannot be null.'.
        /// </summary>
        public static string AddRangeToCollection_ErrorMsg_CollectionNull => ResourceManager.GetString("AddRangeToCollection_ErrorMsg_CollectionNull", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The column '{0}' does not exist on the DataTable.'.
        /// </summary>
        public static string ExtractDataColumnValues_ErrorMsg_InvalidColumnName => ResourceManager.GetString("ExtractDataColumnValues_ErrorMsg_InvalidColumnName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Columns'.
        /// </summary>
        public static string Aggregate_Columns_DisplayName => ResourceManager.GetString("Aggregate_Columns_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Condition'.
        /// </summary>
        public static string Exit_Condition_DisplayName => ResourceManager.GetString("Exit_Condition_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Variable'.
        /// </summary>
        public static string Decrement_Variable_DisplayName => ResourceManager.GetString("Decrement_Variable_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Determines if is to encrypt or decrypt the input value.'.
        /// </summary>
        public static string TextEncryption_Action_Description => ResourceManager.GetString("TextEncryption_Action_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Waits until the file be available.'.
        /// </summary>
        public static string WaitFile_Description => ResourceManager.GetString("WaitFile_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The storage path of the file.'.
        /// </summary>
        public static string WaitFile_FilePath_Description => ResourceManager.GetString("WaitFile_FilePath_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'SearchPattern'.
        /// </summary>
        public static string EnumerateFiles_SearchPattern_DisplayName => ResourceManager.GetString("EnumerateFiles_SearchPattern_DisplayName", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions. It also supports a collection of strings. Default value is "*.*".'.
        /// </summary>
        public static string EnumerateFiles_SearchPattern_Description => ResourceManager.GetString("EnumerateFiles_SearchPattern_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'A variable that represents the instance of System.Diagnostics.Stopwatch class.'.
        /// </summary>
        public static string Stopwatch_ReferenceStopwatch_Description => ResourceManager.GetString("Stopwatch_ReferenceStopwatch_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Specifies whether the search operation should include only the current directory or should include all subdirectories.'.
        /// </summary>
        public static string EnumerateFiles_SearchOption_Description => ResourceManager.GetString("EnumerateFiles_SearchOption_Description", Culture);


        /// <summary>
        /// Looks up a localized string similar to 'Exception'.
        /// </summary>
        public static string TimeLoop_OutputException_DisplayName => ResourceManager.GetString("TimeLoop_OutputException_DisplayName", Culture);


        #endregion
    }
}
