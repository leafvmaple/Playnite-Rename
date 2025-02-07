using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rename
{
    using Playnite.SDK.Models;
    using Playnite.SDK;
    using System;
    using System.IO;

    public static class RenameFilesHelper
    {
        public static void RenameFile(string filePath, string newFileName)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(newFileName))
            {
                throw new ArgumentException("Path Is Empty!");
            }

            var newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
            File.Move(filePath, newFilePath);
        }
    }
}
