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

        // ####### FORMATTERS ###############################################
        
        
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
   

        // ####### GETTERS ##################################################

        
        /// <summary>
        /// Looks up a localized string similar to 'The input Dictionary.'.
        /// </summary>
        public static string DictionaryToDataTable_InputDictionary_Description => ResourceManager.GetString("DictionaryToDataTable_InputDictionary_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Condition which determines if the activity will be evaluated. If empty it assumes True.'.
        /// </summary>
        public static string Next_Condition_Description => ResourceManager.GetString("Next_Condition_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Triple DES'.
        /// </summary>
        public static string TripleDESAlgorithmEncryption_DisplayName => ResourceManager.GetString("TripleDESAlgorithmEncryption_DisplayName", Culture);
    

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
        /// Looks up a localized string similar to 'Returns the number of entries (files and folders) from a Zip archive.'.
        /// </summary>
        public static string ZipEntriesCount_Description => ResourceManager.GetString("ZipEntriesCount_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Please provide an output variable for at least one of the available options.'.
        /// </summary>
        public static string ZipEntriesCount_ErrorMsg_OutputMissing => ResourceManager.GetString("ZipEntriesCount_ErrorMsg_OutputMissing", Culture);
    

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
        /// Looks up a localized string similar to 'Compress files into a zip archive.'.
        /// </summary>
        public static string Zip_Description => ResourceManager.GetString("Zip_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to '(Optional) Returns the information about the file.'.
        /// </summary>
        public static string WaitFile_FileInfo_Description => ResourceManager.GetString("WaitFile_FileInfo_Description", Culture);
    

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
        /// Looks up a localized string similar to 'The items to be added to the collection.'.
        /// </summary>
        public static string AddRangeToCollection_Items_Description => ResourceManager.GetString("AddRangeToCollection_Items_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The path of the file(s) or folder(s) that will be compressed. Can be a string or a collection of strings.'.
        /// </summary>
        public static string Zip_ToCompress_Description => ResourceManager.GetString("Zip_ToCompress_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Replaces an empty column name by the value of this property.'.
        /// </summary>
        public static string PromoteHeaders_EmptyColumnName_Description => ResourceManager.GetString("PromoteHeaders_EmptyColumnName_Description", Culture);
    

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
        /// Looks up a localized string similar to 'The exception that will be thrown if the expression is not true.'.
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
        /// Looks up a localized string similar to 'Executes the 'Do' block if the condition activity returns true.'.
        /// </summary>
        public static string WhenDo_Description => ResourceManager.GetString("WhenDo_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Specifies the amount of time in milliseconds to wait for the activity to run before an error is thrown. The default value is 30000 (30s).'.
        /// </summary>
        public static string Common_Timeout => ResourceManager.GetString("Common_Timeout", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The resulting string from the encrypt or decrypt operation.'.
        /// </summary>
        public static string TextEncryption_Result_Description => ResourceManager.GetString("TextEncryption_Result_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'When true, it avoids the "column name already belongs to DataTable" error by adding a numeric suffix to it.'.
        /// </summary>
        public static string PromoteHeaders_AutoRename_Description => ResourceManager.GetString("PromoteHeaders_AutoRename_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'An object containing the number of files deleted, folders deleted and total deleted.'.
        /// </summary>
        public static string CleanUpFolder_Result_Description => ResourceManager.GetString("CleanUpFolder_Result_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The column indexes or column names to apply the aggregation. If not specified, the aggregation will be applied in all possible columns.'.
        /// </summary>
        public static string Aggregate_Columns_Description => ResourceManager.GetString("Aggregate_Columns_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'If set, continue executing the remaining activities even if the current activity has failed.'.
        /// </summary>
        public static string Common_ContinueOnError => ResourceManager.GetString("Common_ContinueOnError", Culture);
    

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
        /// Looks up a localized string similar to 'A container that allows you to run a set of activities over a different culture.'.
        /// </summary>
        public static string CultureScope_Description => ResourceManager.GetString("CultureScope_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Determines the type of each value extracted from the data column.'.
        /// </summary>
        public static string ExtractDataColumnValues_TypeArgument_Description => ResourceManager.GetString("ExtractDataColumnValues_TypeArgument_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The key to be use. Can be a string or a SecureString.'.
        /// </summary>
        public static string DataTableEncryption_Key_Description => ResourceManager.GetString("DataTableEncryption_Key_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Clean Up Folder'.
        /// </summary>
        public static string CleanUpFolder_DisplayName => ResourceManager.GetString("CleanUpFolder_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Remove Data Columns'.
        /// </summary>
        public static string RemoveDataColumns_DisplayName => ResourceManager.GetString("RemoveDataColumns_DisplayName", Culture);
    

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
        /// Looks up a localized string similar to 'RC2'.
        /// </summary>
        public static string RC2AlgorithmEncryption_DisplayName => ResourceManager.GetString("RC2AlgorithmEncryption_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The column name or index where the values will be extracted from.'.
        /// </summary>
        public static string ExtractDataColumnValues_Column_Description => ResourceManager.GetString("ExtractDataColumnValues_Column_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Returns an array with all values of a respective data column.'.
        /// </summary>
        public static string ExtractDataColumnValues_Description => ResourceManager.GetString("ExtractDataColumnValues_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'DataRow To Dictionary'.
        /// </summary>
        public static string DataRowToDictionary_DisplayName => ResourceManager.GetString("DataRowToDictionary_DisplayName", Culture);
    

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
        /// Looks up a localized string similar to 'Common'.
        /// </summary>
        public static string Common_Category => ResourceManager.GetString("Common_Category", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The path of the zip archive.'.
        /// </summary>
        public static string Zip_ZipFilePath_Description => ResourceManager.GetString("Zip_ZipFilePath_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Promote Headers'.
        /// </summary>
        public static string PromoteHeaders_DisplayName => ResourceManager.GetString("PromoteHeaders_DisplayName", Culture);
    

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
        /// Looks up a localized string similar to 'The input DataTable.'.
        /// </summary>
        public static string PromoteHeaders_InputDataTable_Description => ResourceManager.GetString("PromoteHeaders_InputDataTable_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The column names or column indexes to be removed. Can be either a collection of string or int.'.
        /// </summary>
        public static string RemoveDataColumns_Columns_Description => ResourceManager.GetString("RemoveDataColumns_Columns_Description", Culture);
    

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
        /// Looks up a localized string similar to 'The desired culture, eg.: en-US, pt-BR, jp-JP, etc.'.
        /// </summary>
        public static string CultureScope_CultureName_Description => ResourceManager.GetString("CultureScope_CultureName_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Remove Duplicate Rows'.
        /// </summary>
        public static string RemoveDuplicateRows_DisplayName => ResourceManager.GetString("RemoveDuplicateRows_DisplayName", Culture);
    

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
        /// Looks up a localized string similar to 'The key to be use. Can be a string or a SecureString.'.
        /// </summary>
        public static string TextEncryption_Key_Description => ResourceManager.GetString("TextEncryption_Key_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'DataTable Encryption'.
        /// </summary>
        public static string DataTableEncryption_DisplayName => ResourceManager.GetString("DataTableEncryption_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Cannot be used outside of {0} activities.'.
        /// </summary>
        public static string Validation_ScopesError => ResourceManager.GetString("Validation_ScopesError", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Returns the information about the file.'.
        /// </summary>
        public static string WaitDynamicFile_FileInfo_Description => ResourceManager.GetString("WaitDynamicFile_FileInfo_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Adds a set of items to the specified collection.'.
        /// </summary>
        public static string AddRangeToCollection_Description => ResourceManager.GetString("AddRangeToCollection_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Reverses the order of the output index.'.
        /// </summary>
        public static string Iterate_Reverse_Description => ResourceManager.GetString("Iterate_Reverse_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Provides methods to encrypt or decrypt the values of a DataTable using a specified algorithm and key.'.
        /// </summary>
        public static string DataTableEncryption_Description => ResourceManager.GetString("DataTableEncryption_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Specifies the amount of time (in milliseconds) for the file re-check. Any values out of the range of 100-20000 milliseconds is reseted to its nearest limit. The default value is 500.'.
        /// </summary>
        public static string WaitDynamicFile_Interval_Description => ResourceManager.GetString("WaitDynamicFile_Interval_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'One of the enumeration values that indicates whether to emphasize speed or compression.'.
        /// </summary>
        public static string Zip_CompressionLevel_Description => ResourceManager.GetString("Zip_CompressionLevel_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The Items property value cannot be null.'.
        /// </summary>
        public static string AddRangeToCollection_ErrorMsg_ItemsNull => ResourceManager.GetString("AddRangeToCollection_ErrorMsg_ItemsNull", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The output DataTable.'.
        /// </summary>
        public static string DictionaryToDataTable_Result_Description => ResourceManager.GetString("DictionaryToDataTable_Result_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions. It also supports a collection of strings. Default value is "*.*".'.
        /// </summary>
        public static string WaitDynamicFile_SearchPattern_Description => ResourceManager.GetString("WaitDynamicFile_SearchPattern_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Determines if the left empty folders after files deletion must also be deleted. Default is true.'.
        /// </summary>
        public static string CleanUpFolder_DeleteEmptyFolders_Description => ResourceManager.GetString("CleanUpFolder_DeleteEmptyFolders_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'When Do'.
        /// </summary>
        public static string WhenDo_DisplayName => ResourceManager.GetString("WhenDo_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Extracting Zip entry would have resulted in a file outside the specified destination directory.'.
        /// </summary>
        public static string Unzip_ErrorMsg_OutsideDir => ResourceManager.GetString("Unzip_ErrorMsg_OutsideDir", Culture);
    

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
        /// Looks up a localized string similar to 'The input DataTable.'.
        /// </summary>
        public static string RemoveEmptyRows_InputDataTable_Description => ResourceManager.GetString("RemoveEmptyRows_InputDataTable_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The collection that will receive the new items.'.
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
        /// Looks up a localized string similar to 'The decrement value.'.
        /// </summary>
        public static string Decrement_Value_Description => ResourceManager.GetString("Decrement_Value_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The text to be encrypted or decrypted.'.
        /// </summary>
        public static string TextEncryption_Input_Description => ResourceManager.GetString("TextEncryption_Input_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Removes the empty rows from a DataTable.'.
        /// </summary>
        public static string RemoveEmptyRows_Description => ResourceManager.GetString("RemoveEmptyRows_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The path to the folder where files will be extracted.'.
        /// </summary>
        public static string Unzip_ExtractTo_Description => ResourceManager.GetString("Unzip_ExtractTo_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The date format to be used on the string representation for DateTime column types.'.
        /// </summary>
        public static string DataTableToText_DateTimeFormat_Description => ResourceManager.GetString("DataTableToText_DateTimeFormat_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'A collection of key/value pairs that provide additional user-defined information about the exception.'.
        /// </summary>
        public static string CheckPoint_Data_Description => ResourceManager.GetString("CheckPoint_Data_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The maximum timeout to wait in milliseconds.'.
        /// </summary>
        public static string WaitDynamicFile_Timeout_Description => ResourceManager.GetString("WaitDynamicFile_Timeout_Description", Culture);
    

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
        /// Looks up a localized string similar to 'Waits until an unknown file be available by monitoring a specified folder.'.
        /// </summary>
        public static string WaitDynamicFile_Description => ResourceManager.GetString("WaitDynamicFile_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Wait File'.
        /// </summary>
        public static string WaitFile_DisplayName => ResourceManager.GetString("WaitFile_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The column index '{0}' does not exist on the DataTable.'.
        /// </summary>
        public static string ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex => ResourceManager.GetString("ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'DataTable'.
        /// </summary>
        public static string RemoveDataColumns_ReferenceDataTable_DisplayName => ResourceManager.GetString("RemoveDataColumns_ReferenceDataTable_DisplayName", Culture);
    

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
        /// Looks up a localized string similar to 'AES GCM'.
        /// </summary>
        public static string AesGcmAlgorithmEncryption_DisplayName => ResourceManager.GetString("AesGcmAlgorithmEncryption_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Input'.
        /// </summary>
        public static string Input_Category => ResourceManager.GetString("Input_Category", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Extract Data Column Values'.
        /// </summary>
        public static string ExtractDataColumnValues_DisplayName => ResourceManager.GetString("ExtractDataColumnValues_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The directory to be monitored.'.
        /// </summary>
        public static string WaitDynamicFile_DirectoryPath_Description => ResourceManager.GetString("WaitDynamicFile_DirectoryPath_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Returns the number of folder entries in the zip archive.'.
        /// </summary>
        public static string ZipEntriesCount_FoldersCount_Description => ResourceManager.GetString("ZipEntriesCount_FoldersCount_Description", Culture);
    

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
        /// Looks up a localized string similar to 'The number of iterations that must be run.'.
        /// </summary>
        public static string Iterate_Iterations_Description => ResourceManager.GetString("Iterate_Iterations_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The aggregate function.'.
        /// </summary>
        public static string Aggregate_Function_Description => ResourceManager.GetString("Aggregate_Function_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Exits from the Container or Iterate Activities interrupting any child executions after it.'.
        /// </summary>
        public static string Exit_Description => ResourceManager.GetString("Exit_Description", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The accepted range is from {0} and {1}. The value will be clamped.'.
        /// </summary>
        public static string WaitFile_ErrorMsg_IntervalRange => ResourceManager.GetString("WaitFile_ErrorMsg_IntervalRange", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'Culture Scope'.
        /// </summary>
        public static string CultureScope_DisplayName => ResourceManager.GetString("CultureScope_DisplayName", Culture);
    

        /// <summary>
        /// Looks up a localized string similar to 'The value or expression to be evaluated.'.
        /// </summary>
        public static string IsTrue_Value_Description => ResourceManager.GetString("IsTrue_Value_Description", Culture);
    

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
    
   

        // ####### RESOURCE NAMES ###########################################

        /// <summary>
        /// Contains all resource names stored in their respective constants.
        /// </summary>
        public static class ResourceNames
        {
            
            /// <summary>
            /// Stores the resource name 'DictionaryToDataTable_InputDictionary_Description'.
            /// </summary>
            public const string DictionaryToDataTable_InputDictionary_Description = "DictionaryToDataTable_InputDictionary_Description";
        

            /// <summary>
            /// Stores the resource name 'Next_Condition_Description'.
            /// </summary>
            public const string Next_Condition_Description = "Next_Condition_Description";
        

            /// <summary>
            /// Stores the resource name 'TripleDESAlgorithmEncryption_DisplayName'.
            /// </summary>
            public const string TripleDESAlgorithmEncryption_DisplayName = "TripleDESAlgorithmEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'PromoteHeaders_ErrorMsg_NoData'.
            /// </summary>
            public const string PromoteHeaders_ErrorMsg_NoData = "PromoteHeaders_ErrorMsg_NoData";
        

            /// <summary>
            /// Stores the resource name 'DESAlgorithmEncryption_DisplayName'.
            /// </summary>
            public const string DESAlgorithmEncryption_DisplayName = "DESAlgorithmEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'CheckPoint_Expression_Description'.
            /// </summary>
            public const string CheckPoint_Expression_Description = "CheckPoint_Expression_Description";
        

            /// <summary>
            /// Stores the resource name 'TextEncryption_TextEncoding_Description'.
            /// </summary>
            public const string TextEncryption_TextEncoding_Description = "TextEncryption_TextEncoding_Description";
        

            /// <summary>
            /// Stores the resource name 'Increment_Description'.
            /// </summary>
            public const string Increment_Description = "Increment_Description";
        

            /// <summary>
            /// Stores the resource name 'AddRangeToCollection_DisplayName'.
            /// </summary>
            public const string AddRangeToCollection_DisplayName = "AddRangeToCollection_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_CustomOptions_Category'.
            /// </summary>
            public const string RemoveEmptyRows_CustomOptions_Category = "RemoveEmptyRows_CustomOptions_Category";
        

            /// <summary>
            /// Stores the resource name 'DataTableToText_DataTable_Description'.
            /// </summary>
            public const string DataTableToText_DataTable_Description = "DataTableToText_DataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveDuplicateRows_InputDataTable_Description'.
            /// </summary>
            public const string RemoveDuplicateRows_InputDataTable_Description = "RemoveDuplicateRows_InputDataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'IsTrue_Result_Description'.
            /// </summary>
            public const string IsTrue_Result_Description = "IsTrue_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'ZipEntriesCount_Description'.
            /// </summary>
            public const string ZipEntriesCount_Description = "ZipEntriesCount_Description";
        

            /// <summary>
            /// Stores the resource name 'ZipEntriesCount_ErrorMsg_OutputMissing'.
            /// </summary>
            public const string ZipEntriesCount_ErrorMsg_OutputMissing = "ZipEntriesCount_ErrorMsg_OutputMissing";
        

            /// <summary>
            /// Stores the resource name 'Aggregate_Result_Description'.
            /// </summary>
            public const string Aggregate_Result_Description = "Aggregate_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'Validation_ValueError'.
            /// </summary>
            public const string Validation_ValueError = "Validation_ValueError";
        

            /// <summary>
            /// Stores the resource name 'DataRowToDictionary_Description'.
            /// </summary>
            public const string DataRowToDictionary_Description = "DataRowToDictionary_Description";
        

            /// <summary>
            /// Stores the resource name 'Zip_Description'.
            /// </summary>
            public const string Zip_Description = "Zip_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_FileInfo_Description'.
            /// </summary>
            public const string WaitFile_FileInfo_Description = "WaitFile_FileInfo_Description";
        

            /// <summary>
            /// Stores the resource name 'CheckPoint_DisplayName'.
            /// </summary>
            public const string CheckPoint_DisplayName = "CheckPoint_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'CleanUpFolder_LastWriteTime_Description'.
            /// </summary>
            public const string CleanUpFolder_LastWriteTime_Description = "CleanUpFolder_LastWriteTime_Description";
        

            /// <summary>
            /// Stores the resource name 'IsTrue_Description'.
            /// </summary>
            public const string IsTrue_Description = "IsTrue_Description";
        

            /// <summary>
            /// Stores the resource name 'AddRangeToCollection_Items_Description'.
            /// </summary>
            public const string AddRangeToCollection_Items_Description = "AddRangeToCollection_Items_Description";
        

            /// <summary>
            /// Stores the resource name 'Zip_ToCompress_Description'.
            /// </summary>
            public const string Zip_ToCompress_Description = "Zip_ToCompress_Description";
        

            /// <summary>
            /// Stores the resource name 'PromoteHeaders_EmptyColumnName_Description'.
            /// </summary>
            public const string PromoteHeaders_EmptyColumnName_Description = "PromoteHeaders_EmptyColumnName_Description";
        

            /// <summary>
            /// Stores the resource name 'WhenDo_Inverted_Description'.
            /// </summary>
            public const string WhenDo_Inverted_Description = "WhenDo_Inverted_Description";
        

            /// <summary>
            /// Stores the resource name 'EnumerateFiles_Description'.
            /// </summary>
            public const string EnumerateFiles_Description = "EnumerateFiles_Description";
        

            /// <summary>
            /// Stores the resource name 'ZipEntriesCount_FilesCount_Description'.
            /// </summary>
            public const string ZipEntriesCount_FilesCount_Description = "ZipEntriesCount_FilesCount_Description";
        

            /// <summary>
            /// Stores the resource name 'CheckPoint_Exception_Description'.
            /// </summary>
            public const string CheckPoint_Exception_Description = "CheckPoint_Exception_Description";
        

            /// <summary>
            /// Stores the resource name 'EnumerateFiles_Exclusions_Description'.
            /// </summary>
            public const string EnumerateFiles_Exclusions_Description = "EnumerateFiles_Exclusions_Description";
        

            /// <summary>
            /// Stores the resource name 'Validation_ScopeError'.
            /// </summary>
            public const string Validation_ScopeError = "Validation_ScopeError";
        

            /// <summary>
            /// Stores the resource name 'WhenDo_Description'.
            /// </summary>
            public const string WhenDo_Description = "WhenDo_Description";
        

            /// <summary>
            /// Stores the resource name 'Common_Timeout'.
            /// </summary>
            public const string Common_Timeout = "Common_Timeout";
        

            /// <summary>
            /// Stores the resource name 'TextEncryption_Result_Description'.
            /// </summary>
            public const string TextEncryption_Result_Description = "TextEncryption_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'PromoteHeaders_AutoRename_Description'.
            /// </summary>
            public const string PromoteHeaders_AutoRename_Description = "PromoteHeaders_AutoRename_Description";
        

            /// <summary>
            /// Stores the resource name 'CleanUpFolder_Result_Description'.
            /// </summary>
            public const string CleanUpFolder_Result_Description = "CleanUpFolder_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'Aggregate_Columns_Description'.
            /// </summary>
            public const string Aggregate_Columns_Description = "Aggregate_Columns_Description";
        

            /// <summary>
            /// Stores the resource name 'Common_ContinueOnError'.
            /// </summary>
            public const string Common_ContinueOnError = "Common_ContinueOnError";
        

            /// <summary>
            /// Stores the resource name 'Next_Description'.
            /// </summary>
            public const string Next_Description = "Next_Description";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_DefaultValue_Description'.
            /// </summary>
            public const string ExtractDataColumnValues_DefaultValue_Description = "ExtractDataColumnValues_DefaultValue_Description";
        

            /// <summary>
            /// Stores the resource name 'TextEncryption_Description'.
            /// </summary>
            public const string TextEncryption_Description = "TextEncryption_Description";
        

            /// <summary>
            /// Stores the resource name 'CultureScope_Description'.
            /// </summary>
            public const string CultureScope_Description = "CultureScope_Description";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_TypeArgument_Description'.
            /// </summary>
            public const string ExtractDataColumnValues_TypeArgument_Description = "ExtractDataColumnValues_TypeArgument_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_Key_Description'.
            /// </summary>
            public const string DataTableEncryption_Key_Description = "DataTableEncryption_Key_Description";
        

            /// <summary>
            /// Stores the resource name 'CleanUpFolder_DisplayName'.
            /// </summary>
            public const string CleanUpFolder_DisplayName = "CleanUpFolder_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'RemoveDataColumns_DisplayName'.
            /// </summary>
            public const string RemoveDataColumns_DisplayName = "RemoveDataColumns_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_Action_Description'.
            /// </summary>
            public const string DataTableEncryption_Action_Description = "DataTableEncryption_Action_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_TextEncoding_Description'.
            /// </summary>
            public const string DataTableEncryption_TextEncoding_Description = "DataTableEncryption_TextEncoding_Description";
        

            /// <summary>
            /// Stores the resource name 'PromoteHeaders_Result_Description'.
            /// </summary>
            public const string PromoteHeaders_Result_Description = "PromoteHeaders_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_Operator_Description'.
            /// </summary>
            public const string RemoveEmptyRows_Operator_Description = "RemoveEmptyRows_Operator_Description";
        

            /// <summary>
            /// Stores the resource name 'ZipEntriesCount_DisplayName'.
            /// </summary>
            public const string ZipEntriesCount_DisplayName = "ZipEntriesCount_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'Iterate_Description'.
            /// </summary>
            public const string Iterate_Description = "Iterate_Description";
        

            /// <summary>
            /// Stores the resource name 'Zip_TextEncoding_Description'.
            /// </summary>
            public const string Zip_TextEncoding_Description = "Zip_TextEncoding_Description";
        

            /// <summary>
            /// Stores the resource name 'Stopwatch_Description'.
            /// </summary>
            public const string Stopwatch_Description = "Stopwatch_Description";
        

            /// <summary>
            /// Stores the resource name 'Decrement_Description'.
            /// </summary>
            public const string Decrement_Description = "Decrement_Description";
        

            /// <summary>
            /// Stores the resource name 'EncryptionBase_ErrorMsg_AlgorithmMissing'.
            /// </summary>
            public const string EncryptionBase_ErrorMsg_AlgorithmMissing = "EncryptionBase_ErrorMsg_AlgorithmMissing";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_Input_Description'.
            /// </summary>
            public const string DataTableEncryption_Input_Description = "DataTableEncryption_Input_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveDuplicateRows_Description'.
            /// </summary>
            public const string RemoveDuplicateRows_Description = "RemoveDuplicateRows_Description";
        

            /// <summary>
            /// Stores the resource name 'AesAlgorithmEncryption_DisplayName'.
            /// </summary>
            public const string AesAlgorithmEncryption_DisplayName = "AesAlgorithmEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'Iterate_ErrorMsg_IterationsMinValue'.
            /// </summary>
            public const string Iterate_ErrorMsg_IterationsMinValue = "Iterate_ErrorMsg_IterationsMinValue";
        

            /// <summary>
            /// Stores the resource name 'Aggregate_InputDataTable_Description'.
            /// </summary>
            public const string Aggregate_InputDataTable_Description = "Aggregate_InputDataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'IsTrue_DisplayName'.
            /// </summary>
            public const string IsTrue_DisplayName = "IsTrue_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'DataTableToText_DisplayName'.
            /// </summary>
            public const string DataTableToText_DisplayName = "DataTableToText_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'RemoveDuplicateRows_Columns_Description'.
            /// </summary>
            public const string RemoveDuplicateRows_Columns_Description = "RemoveDuplicateRows_Columns_Description";
        

            /// <summary>
            /// Stores the resource name 'Stopwatch_ReferenceStopwatch_DisplayName'.
            /// </summary>
            public const string Stopwatch_ReferenceStopwatch_DisplayName = "Stopwatch_ReferenceStopwatch_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_Timeout_Description'.
            /// </summary>
            public const string WaitFile_Timeout_Description = "WaitFile_Timeout_Description";
        

            /// <summary>
            /// Stores the resource name 'Aggregate_Description'.
            /// </summary>
            public const string Aggregate_Description = "Aggregate_Description";
        

            /// <summary>
            /// Stores the resource name 'RC2AlgorithmEncryption_DisplayName'.
            /// </summary>
            public const string RC2AlgorithmEncryption_DisplayName = "RC2AlgorithmEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_Column_Description'.
            /// </summary>
            public const string ExtractDataColumnValues_Column_Description = "ExtractDataColumnValues_Column_Description";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_Description'.
            /// </summary>
            public const string ExtractDataColumnValues_Description = "ExtractDataColumnValues_Description";
        

            /// <summary>
            /// Stores the resource name 'DataRowToDictionary_DisplayName'.
            /// </summary>
            public const string DataRowToDictionary_DisplayName = "DataRowToDictionary_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'CheckPoint_Description'.
            /// </summary>
            public const string CheckPoint_Description = "CheckPoint_Description";
        

            /// <summary>
            /// Stores the resource name 'EnumerateFiles_DisplayName'.
            /// </summary>
            public const string EnumerateFiles_DisplayName = "EnumerateFiles_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'DataRowToDictionary_Result_Description'.
            /// </summary>
            public const string DataRowToDictionary_Result_Description = "DataRowToDictionary_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'WhenDo_WithElse_Description'.
            /// </summary>
            public const string WhenDo_WithElse_Description = "WhenDo_WithElse_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveDataColumns_Description'.
            /// </summary>
            public const string RemoveDataColumns_Description = "RemoveDataColumns_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_WaitForExist_Description'.
            /// </summary>
            public const string WaitFile_WaitForExist_Description = "WaitFile_WaitForExist_Description";
        

            /// <summary>
            /// Stores the resource name 'InputOutput_Category'.
            /// </summary>
            public const string InputOutput_Category = "InputOutput_Category";
        

            /// <summary>
            /// Stores the resource name 'Unzip_Overwrite_Description'.
            /// </summary>
            public const string Unzip_Overwrite_Description = "Unzip_Overwrite_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableToText_Result_Description'.
            /// </summary>
            public const string DataTableToText_Result_Description = "DataTableToText_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'Common_Category'.
            /// </summary>
            public const string Common_Category = "Common_Category";
        

            /// <summary>
            /// Stores the resource name 'Zip_ZipFilePath_Description'.
            /// </summary>
            public const string Zip_ZipFilePath_Description = "Zip_ZipFilePath_Description";
        

            /// <summary>
            /// Stores the resource name 'PromoteHeaders_DisplayName'.
            /// </summary>
            public const string PromoteHeaders_DisplayName = "PromoteHeaders_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'DataTableToText_Description'.
            /// </summary>
            public const string DataTableToText_Description = "DataTableToText_Description";
        

            /// <summary>
            /// Stores the resource name 'Decrement_ErrorMsg_MinValue'.
            /// </summary>
            public const string Decrement_ErrorMsg_MinValue = "Decrement_ErrorMsg_MinValue";
        

            /// <summary>
            /// Stores the resource name 'DictionaryToDataTable_DisplayName'.
            /// </summary>
            public const string DictionaryToDataTable_DisplayName = "DictionaryToDataTable_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_ParallelProcessing_Description'.
            /// </summary>
            public const string DataTableEncryption_ParallelProcessing_Description = "DataTableEncryption_ParallelProcessing_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveDuplicateRows_Result_Description'.
            /// </summary>
            public const string RemoveDuplicateRows_Result_Description = "RemoveDuplicateRows_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'CleanUpFolder_Description'.
            /// </summary>
            public const string CleanUpFolder_Description = "CleanUpFolder_Description";
        

            /// <summary>
            /// Stores the resource name 'Increment_ErrorMsg_MinValue'.
            /// </summary>
            public const string Increment_ErrorMsg_MinValue = "Increment_ErrorMsg_MinValue";
        

            /// <summary>
            /// Stores the resource name 'PromoteHeaders_InputDataTable_Description'.
            /// </summary>
            public const string PromoteHeaders_InputDataTable_Description = "PromoteHeaders_InputDataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveDataColumns_Columns_Description'.
            /// </summary>
            public const string RemoveDataColumns_Columns_Description = "RemoveDataColumns_Columns_Description";
        

            /// <summary>
            /// Stores the resource name 'Exit_Condition_Description'.
            /// </summary>
            public const string Exit_Condition_Description = "Exit_Condition_Description";
        

            /// <summary>
            /// Stores the resource name 'SymmetricAlgorithmEncryptionBase_Iterations_Description'.
            /// </summary>
            public const string SymmetricAlgorithmEncryptionBase_Iterations_Description = "SymmetricAlgorithmEncryptionBase_Iterations_Description";
        

            /// <summary>
            /// Stores the resource name 'Output_Category'.
            /// </summary>
            public const string Output_Category = "Output_Category";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_Sort_Description'.
            /// </summary>
            public const string DataTableEncryption_Sort_Description = "DataTableEncryption_Sort_Description";
        

            /// <summary>
            /// Stores the resource name 'TransposeData_InputDataTable_Description'.
            /// </summary>
            public const string TransposeData_InputDataTable_Description = "TransposeData_InputDataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'Container_Description'.
            /// </summary>
            public const string Container_Description = "Container_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_DisplayName'.
            /// </summary>
            public const string RemoveEmptyRows_DisplayName = "RemoveEmptyRows_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'TransposeData_DisplayName'.
            /// </summary>
            public const string TransposeData_DisplayName = "TransposeData_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'DataRowToDictionary_InputDataRow_Description'.
            /// </summary>
            public const string DataRowToDictionary_InputDataRow_Description = "DataRowToDictionary_InputDataRow_Description";
        

            /// <summary>
            /// Stores the resource name 'Zip_ShortEntryNames_Description'.
            /// </summary>
            public const string Zip_ShortEntryNames_Description = "Zip_ShortEntryNames_Description";
        

            /// <summary>
            /// Stores the resource name 'CultureScope_CultureName_Description'.
            /// </summary>
            public const string CultureScope_CultureName_Description = "CultureScope_CultureName_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveDuplicateRows_DisplayName'.
            /// </summary>
            public const string RemoveDuplicateRows_DisplayName = "RemoveDuplicateRows_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_ErrorMsg_FilePathDoesNotExists'.
            /// </summary>
            public const string WaitFile_ErrorMsg_FilePathDoesNotExists = "WaitFile_ErrorMsg_FilePathDoesNotExists";
        

            /// <summary>
            /// Stores the resource name 'Stopwatch_Method_Description'.
            /// </summary>
            public const string Stopwatch_Method_Description = "Stopwatch_Method_Description";
        

            /// <summary>
            /// Stores the resource name 'Zip_FilesCount_Description'.
            /// </summary>
            public const string Zip_FilesCount_Description = "Zip_FilesCount_Description";
        

            /// <summary>
            /// Stores the resource name 'TextEncryption_Key_Description'.
            /// </summary>
            public const string TextEncryption_Key_Description = "TextEncryption_Key_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_DisplayName'.
            /// </summary>
            public const string DataTableEncryption_DisplayName = "DataTableEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'Validation_ScopesError'.
            /// </summary>
            public const string Validation_ScopesError = "Validation_ScopesError";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_FileInfo_Description'.
            /// </summary>
            public const string WaitDynamicFile_FileInfo_Description = "WaitDynamicFile_FileInfo_Description";
        

            /// <summary>
            /// Stores the resource name 'AddRangeToCollection_Description'.
            /// </summary>
            public const string AddRangeToCollection_Description = "AddRangeToCollection_Description";
        

            /// <summary>
            /// Stores the resource name 'Iterate_Reverse_Description'.
            /// </summary>
            public const string Iterate_Reverse_Description = "Iterate_Reverse_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_Description'.
            /// </summary>
            public const string DataTableEncryption_Description = "DataTableEncryption_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_Interval_Description'.
            /// </summary>
            public const string WaitDynamicFile_Interval_Description = "WaitDynamicFile_Interval_Description";
        

            /// <summary>
            /// Stores the resource name 'Zip_CompressionLevel_Description'.
            /// </summary>
            public const string Zip_CompressionLevel_Description = "Zip_CompressionLevel_Description";
        

            /// <summary>
            /// Stores the resource name 'AddRangeToCollection_ErrorMsg_ItemsNull'.
            /// </summary>
            public const string AddRangeToCollection_ErrorMsg_ItemsNull = "AddRangeToCollection_ErrorMsg_ItemsNull";
        

            /// <summary>
            /// Stores the resource name 'DictionaryToDataTable_Result_Description'.
            /// </summary>
            public const string DictionaryToDataTable_Result_Description = "DictionaryToDataTable_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_SearchPattern_Description'.
            /// </summary>
            public const string WaitDynamicFile_SearchPattern_Description = "WaitDynamicFile_SearchPattern_Description";
        

            /// <summary>
            /// Stores the resource name 'CleanUpFolder_DeleteEmptyFolders_Description'.
            /// </summary>
            public const string CleanUpFolder_DeleteEmptyFolders_Description = "CleanUpFolder_DeleteEmptyFolders_Description";
        

            /// <summary>
            /// Stores the resource name 'WhenDo_DisplayName'.
            /// </summary>
            public const string WhenDo_DisplayName = "WhenDo_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'Unzip_ErrorMsg_OutsideDir'.
            /// </summary>
            public const string Unzip_ErrorMsg_OutsideDir = "Unzip_ErrorMsg_OutsideDir";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_Columns_Description'.
            /// </summary>
            public const string DataTableEncryption_Columns_Description = "DataTableEncryption_Columns_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveDataColumns_ReferenceDataTable_Description'.
            /// </summary>
            public const string RemoveDataColumns_ReferenceDataTable_Description = "RemoveDataColumns_ReferenceDataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_Result_Description'.
            /// </summary>
            public const string RemoveEmptyRows_Result_Description = "RemoveEmptyRows_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableEncryption_Result_Description'.
            /// </summary>
            public const string DataTableEncryption_Result_Description = "DataTableEncryption_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'RijndaelAlgorithmEncryption_DisplayName'.
            /// </summary>
            public const string RijndaelAlgorithmEncryption_DisplayName = "RijndaelAlgorithmEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_Mode_Description'.
            /// </summary>
            public const string RemoveEmptyRows_Mode_Description = "RemoveEmptyRows_Mode_Description";
        

            /// <summary>
            /// Stores the resource name 'TransposeData_Result_Description'.
            /// </summary>
            public const string TransposeData_Result_Description = "TransposeData_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'CleanUpFolder_SearchPattern_Description'.
            /// </summary>
            public const string CleanUpFolder_SearchPattern_Description = "CleanUpFolder_SearchPattern_Description";
        

            /// <summary>
            /// Stores the resource name 'EnumerateFiles_DirectoryPath_Description'.
            /// </summary>
            public const string EnumerateFiles_DirectoryPath_Description = "EnumerateFiles_DirectoryPath_Description";
        

            /// <summary>
            /// Stores the resource name 'Unzip_ErrorMsg_DirNameWithData'.
            /// </summary>
            public const string Unzip_ErrorMsg_DirNameWithData = "Unzip_ErrorMsg_DirNameWithData";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_ErrorMsg_IntervalRange'.
            /// </summary>
            public const string WaitDynamicFile_ErrorMsg_IntervalRange = "WaitDynamicFile_ErrorMsg_IntervalRange";
        

            /// <summary>
            /// Stores the resource name 'EnumerateFiles_Result_Description'.
            /// </summary>
            public const string EnumerateFiles_Result_Description = "EnumerateFiles_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_InputDataTable_Description'.
            /// </summary>
            public const string RemoveEmptyRows_InputDataTable_Description = "RemoveEmptyRows_InputDataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'AddRangeToCollection_Collection_Description'.
            /// </summary>
            public const string AddRangeToCollection_Collection_Description = "AddRangeToCollection_Collection_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_Interval_Description'.
            /// </summary>
            public const string WaitFile_Interval_Description = "WaitFile_Interval_Description";
        

            /// <summary>
            /// Stores the resource name 'ZipEntriesCount_ZipFilePath_Description'.
            /// </summary>
            public const string ZipEntriesCount_ZipFilePath_Description = "ZipEntriesCount_ZipFilePath_Description";
        

            /// <summary>
            /// Stores the resource name 'Decrement_Value_Description'.
            /// </summary>
            public const string Decrement_Value_Description = "Decrement_Value_Description";
        

            /// <summary>
            /// Stores the resource name 'TextEncryption_Input_Description'.
            /// </summary>
            public const string TextEncryption_Input_Description = "TextEncryption_Input_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_Description'.
            /// </summary>
            public const string RemoveEmptyRows_Description = "RemoveEmptyRows_Description";
        

            /// <summary>
            /// Stores the resource name 'Unzip_ExtractTo_Description'.
            /// </summary>
            public const string Unzip_ExtractTo_Description = "Unzip_ExtractTo_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableToText_DateTimeFormat_Description'.
            /// </summary>
            public const string DataTableToText_DateTimeFormat_Description = "DataTableToText_DateTimeFormat_Description";
        

            /// <summary>
            /// Stores the resource name 'CheckPoint_Data_Description'.
            /// </summary>
            public const string CheckPoint_Data_Description = "CheckPoint_Data_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_Timeout_Description'.
            /// </summary>
            public const string WaitDynamicFile_Timeout_Description = "WaitDynamicFile_Timeout_Description";
        

            /// <summary>
            /// Stores the resource name 'TextEncryption_DisplayName'.
            /// </summary>
            public const string TextEncryption_DisplayName = "TextEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'Increment_Variable_Description'.
            /// </summary>
            public const string Increment_Variable_Description = "Increment_Variable_Description";
        

            /// <summary>
            /// Stores the resource name 'Validation_TypeError'.
            /// </summary>
            public const string Validation_TypeError = "Validation_TypeError";
        

            /// <summary>
            /// Stores the resource name 'TransposeData_Description'.
            /// </summary>
            public const string TransposeData_Description = "TransposeData_Description";
        

            /// <summary>
            /// Stores the resource name 'Unzip_ZipFilePath_Description'.
            /// </summary>
            public const string Unzip_ZipFilePath_Description = "Unzip_ZipFilePath_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_DisplayName'.
            /// </summary>
            public const string WaitDynamicFile_DisplayName = "WaitDynamicFile_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_Description'.
            /// </summary>
            public const string WaitDynamicFile_Description = "WaitDynamicFile_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_DisplayName'.
            /// </summary>
            public const string WaitFile_DisplayName = "WaitFile_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex'.
            /// </summary>
            public const string ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex = "ExtractDataColumnValues_ErrorMsg_InvalidColumnIndex";
        

            /// <summary>
            /// Stores the resource name 'RemoveDataColumns_ReferenceDataTable_DisplayName'.
            /// </summary>
            public const string RemoveDataColumns_ReferenceDataTable_DisplayName = "RemoveDataColumns_ReferenceDataTable_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'Unzip_Description'.
            /// </summary>
            public const string Unzip_Description = "Unzip_Description";
        

            /// <summary>
            /// Stores the resource name 'ZipEntriesCount_EntriesCount_Description'.
            /// </summary>
            public const string ZipEntriesCount_EntriesCount_Description = "ZipEntriesCount_EntriesCount_Description";
        

            /// <summary>
            /// Stores the resource name 'Options_Category'.
            /// </summary>
            public const string Options_Category = "Options_Category";
        

            /// <summary>
            /// Stores the resource name 'AesGcmAlgorithmEncryption_DisplayName'.
            /// </summary>
            public const string AesGcmAlgorithmEncryption_DisplayName = "AesGcmAlgorithmEncryption_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'Input_Category'.
            /// </summary>
            public const string Input_Category = "Input_Category";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_DisplayName'.
            /// </summary>
            public const string ExtractDataColumnValues_DisplayName = "ExtractDataColumnValues_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'WaitDynamicFile_DirectoryPath_Description'.
            /// </summary>
            public const string WaitDynamicFile_DirectoryPath_Description = "WaitDynamicFile_DirectoryPath_Description";
        

            /// <summary>
            /// Stores the resource name 'ZipEntriesCount_FoldersCount_Description'.
            /// </summary>
            public const string ZipEntriesCount_FoldersCount_Description = "ZipEntriesCount_FoldersCount_Description";
        

            /// <summary>
            /// Stores the resource name 'PromoteHeaders_Description'.
            /// </summary>
            public const string PromoteHeaders_Description = "PromoteHeaders_Description";
        

            /// <summary>
            /// Stores the resource name 'DictionaryToDataTable_Description'.
            /// </summary>
            public const string DictionaryToDataTable_Description = "DictionaryToDataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_DataTable_Description'.
            /// </summary>
            public const string ExtractDataColumnValues_DataTable_Description = "ExtractDataColumnValues_DataTable_Description";
        

            /// <summary>
            /// Stores the resource name 'CleanUpFolder_Folder_Description'.
            /// </summary>
            public const string CleanUpFolder_Folder_Description = "CleanUpFolder_Folder_Description";
        

            /// <summary>
            /// Stores the resource name 'Decrement_Variable_Description'.
            /// </summary>
            public const string Decrement_Variable_Description = "Decrement_Variable_Description";
        

            /// <summary>
            /// Stores the resource name 'RemoveEmptyRows_Columns_Description'.
            /// </summary>
            public const string RemoveEmptyRows_Columns_Description = "RemoveEmptyRows_Columns_Description";
        

            /// <summary>
            /// Stores the resource name 'Increment_Value_Description'.
            /// </summary>
            public const string Increment_Value_Description = "Increment_Value_Description";
        

            /// <summary>
            /// Stores the resource name 'DataTableToText_TextFormat_Description'.
            /// </summary>
            public const string DataTableToText_TextFormat_Description = "DataTableToText_TextFormat_Description";
        

            /// <summary>
            /// Stores the resource name 'Iterate_Iterations_Description'.
            /// </summary>
            public const string Iterate_Iterations_Description = "Iterate_Iterations_Description";
        

            /// <summary>
            /// Stores the resource name 'Aggregate_Function_Description'.
            /// </summary>
            public const string Aggregate_Function_Description = "Aggregate_Function_Description";
        

            /// <summary>
            /// Stores the resource name 'Exit_Description'.
            /// </summary>
            public const string Exit_Description = "Exit_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_ErrorMsg_IntervalRange'.
            /// </summary>
            public const string WaitFile_ErrorMsg_IntervalRange = "WaitFile_ErrorMsg_IntervalRange";
        

            /// <summary>
            /// Stores the resource name 'CultureScope_DisplayName'.
            /// </summary>
            public const string CultureScope_DisplayName = "CultureScope_DisplayName";
        

            /// <summary>
            /// Stores the resource name 'IsTrue_Value_Description'.
            /// </summary>
            public const string IsTrue_Value_Description = "IsTrue_Value_Description";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_Result_Description'.
            /// </summary>
            public const string ExtractDataColumnValues_Result_Description = "ExtractDataColumnValues_Result_Description";
        

            /// <summary>
            /// Stores the resource name 'AddRangeToCollection_ErrorMsg_CollectionNull'.
            /// </summary>
            public const string AddRangeToCollection_ErrorMsg_CollectionNull = "AddRangeToCollection_ErrorMsg_CollectionNull";
        

            /// <summary>
            /// Stores the resource name 'ExtractDataColumnValues_ErrorMsg_InvalidColumnName'.
            /// </summary>
            public const string ExtractDataColumnValues_ErrorMsg_InvalidColumnName = "ExtractDataColumnValues_ErrorMsg_InvalidColumnName";
        

            /// <summary>
            /// Stores the resource name 'TextEncryption_Action_Description'.
            /// </summary>
            public const string TextEncryption_Action_Description = "TextEncryption_Action_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_Description'.
            /// </summary>
            public const string WaitFile_Description = "WaitFile_Description";
        

            /// <summary>
            /// Stores the resource name 'WaitFile_FilePath_Description'.
            /// </summary>
            public const string WaitFile_FilePath_Description = "WaitFile_FilePath_Description";
        

            /// <summary>
            /// Stores the resource name 'EnumerateFiles_SearchPattern_Description'.
            /// </summary>
            public const string EnumerateFiles_SearchPattern_Description = "EnumerateFiles_SearchPattern_Description";
        

            /// <summary>
            /// Stores the resource name 'Stopwatch_ReferenceStopwatch_Description'.
            /// </summary>
            public const string Stopwatch_ReferenceStopwatch_Description = "Stopwatch_ReferenceStopwatch_Description";
        

            /// <summary>
            /// Stores the resource name 'EnumerateFiles_SearchOption_Description'.
            /// </summary>
            public const string EnumerateFiles_SearchOption_Description = "EnumerateFiles_SearchOption_Description";
        
        }
    }
}
    

