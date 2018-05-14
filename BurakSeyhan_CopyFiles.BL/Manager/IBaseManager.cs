using BurakSeyhan_CopyFiles.BL.Entity;
using System.IO;

namespace BurakSeyhan_CopyFiles.BL.Manager
{
    public interface IBaseManager
    {
        OperationResult CopyFolder(DirectoryInfo source, DirectoryInfo target);

        void Initialize(string source, string to);
    }
}
