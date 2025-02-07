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
        public static string getAlteredRomPath(string romPath, string newFileName)
        {
            return Path.Combine(Path.GetDirectoryName(romPath), newFileName + Path.GetExtension(romPath));
        }

        public static string getFormatPath(string installDirectory, string romPath)
        {
            return romPath.Replace("{InstallDir}\\", installDirectory);
        }

        public static void RenameFile(string oldPath, string newPath)
        {
            if (string.IsNullOrEmpty(oldPath) || string.IsNullOrEmpty(newPath))
            {
                throw new ArgumentException("Path Is Empty!");
            }

            File.Move(oldPath, newPath);
        }
    }
}
