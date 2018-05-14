using BurakSeyhan_CopyFiles.BL.Entity;
using System;
using System.IO;

namespace BurakSeyhan_CopyFiles.BL.Manager
{
    public class FileManager : IBaseManager
    {
        private int maxbytes;
        private int copied;
        private int total;

        public delegate void NotifyMeEventHandler(int bytes, bool specifyMax, bool withStep, string message);
        public event NotifyMeEventHandler NofityMe;

        public FileManager()
        {
            maxbytes = 0;
            copied = 0;
            total = 0;
        }
        
        public OperationResult CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            OperationResult result = new OperationResult();
            
            try
            {
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }

                foreach (FileInfo files in source.GetFiles())
                {
                    files.CopyTo(Path.Combine(target.ToString(), files.Name), true);

                    total += (int)files.Length;

                    copied += (int)files.Length;
                    copied /= 1024;

                    string text = (total / 1048576).ToString() + "MB of " + (maxbytes / 1024).ToString() + "MB copied";

                    NofityMe(copied, false, true, text);
                    
                    if ((total / 1048576) == (maxbytes / 1024))
                    {
                        NofityMe(copied, false, true, "Transfer is success");
                    }

                    result.IsSuccess = true;
                }
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyFolder(diSourceSubDir, nextTargetSubDir);
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                NofityMe(0, false, false, result.Message);
            }

            return result;
        }

        public void GetSize(DirectoryInfo from, DirectoryInfo to)
        {
            try
            {
                if (Directory.Exists(to.FullName) == false)
                {
                    Directory.CreateDirectory(to.FullName);
                }
                foreach (FileInfo fi in from.GetFiles())
                {
                    maxbytes += (int)fi.Length;
                }
                foreach (DirectoryInfo diSourceSubDir in from.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir = to.CreateSubdirectory(diSourceSubDir.Name);
                    GetSize(diSourceSubDir, nextTargetSubDir);
                }
            }
            catch (Exception ex)
            {
                NofityMe(0, false, false, ex.Message);
            }
        }

        public void Initialize(string source, string to)
        {
            NofityMe?.Invoke(maxbytes, true, false, $"{maxbytes}");

            if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(to))
            {
                DirectoryInfo diSource = new DirectoryInfo(source);
                DirectoryInfo diTarget = new DirectoryInfo(to);

                GetSize(diSource, diTarget);
                maxbytes = maxbytes / 1024;
                CopyFolder(diSource, diTarget);
            }
            else
            {
                NofityMe?.Invoke(0, false, false, $"Do not blank");
            }
        }
    }
}
