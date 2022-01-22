namespace Autossential.Core.Models
{
    public struct CleanUpFolderResult
    {
        public int FilesDeleted { get; set; }
        public int FoldersDeleted { get; set; }
        public int TotalDeleted => FilesDeleted + FoldersDeleted;
    }
}