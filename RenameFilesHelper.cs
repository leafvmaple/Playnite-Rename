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
        public static string getAlteredRomPath(Game game)
        {
            var region = "";
            if (game.Regions != null && game.Regions.Count > 0)
                region = $" ({game.Regions[0].Name})" ;

            return Path.Combine(Path.GetDirectoryName(game.Roms[0].Path),
                $"{ game.Name }{ region }{ Path.GetExtension(game.Roms[0].Path) }");
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
